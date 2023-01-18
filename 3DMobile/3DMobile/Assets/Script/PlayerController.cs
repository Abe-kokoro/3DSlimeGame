using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
public class PlayerController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField] GameObject MinePlayer;
    [SerializeField] GameObject DashButton;
    [SerializeField] GameObject JumpButton;
    [SerializeField] GameObject AttackButton;
    [SerializeField] Slider PlayerHPSlider;
    [SerializeField] TextMeshProUGUI PlayerLevel;
    [SerializeField] GameObject Loading;
    [SerializeField] TextMeshProUGUI KillCount;
    [SerializeField] GameObject RespawnPos;
    [SerializeField] GameObject DeadPanel;
    public static Vector3 resPos;
    float PlayerCurrentHp;
    float PlayerMaxHp;
    public bool isDead =  false;
    bool isPlayer = false;
    void Start()
    {
        DeadPanel.SetActive(false);
        //DashButton.GetComponent<Button>().onClick.AddListener(Dash);
        JumpButton.GetComponent<Button>().onClick.AddListener(Jump);
        AttackButton.GetComponent<Button>().onClick.AddListener(Attack);
        Loading.SetActive(true);
        resPos = new Vector3(RespawnPos.transform.position.x, RespawnPos.transform.position.y+5.0f, RespawnPos.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            if (!MinePlayer)
            {
                MinePlayer = GameObject.FindGameObjectWithTag("Player");
                //マルチまで
                if (!MinePlayer.GetComponent<Player2>().photonView.IsMine)
                {
                    MinePlayer = null;
                }
            }
        }
        if(MinePlayer)
        {
            Loading.SetActive(false);
            isPlayer = true;
            PlayerCurrentHp = MinePlayer.GetComponent<PlyerAnimator>().GetPlayerHP().x;
            PlayerMaxHp = MinePlayer.GetComponent<PlyerAnimator>().GetPlayerHP().y;
            PlayerHPSlider.value = PlayerCurrentHp / PlayerMaxHp;
            int Lv = MinePlayer.GetComponent<PlyerAnimator>().GetPlayerLevel(); 
            PlayerLevel.text = "Lv."+Lv;
            KillCount.text ="次のレベルまで"+ MinePlayer.GetComponent<PlyerAnimator>().GetKillCount()+"/"+MinePlayer.GetComponent<PlyerAnimator>().GetLvUpCount();
            if(MinePlayer.GetComponent<PlyerAnimator>().isDead&&!isDead)
            {
                isDead = true;
                Invoke("Dead", 3.0f);
            }
            if(isDead)
            {
                if(!MinePlayer.GetComponent<PlyerAnimator>().isDead)
                {
                    isDead = false; 
                }
            }
            //マルチまで
            // PlayerCurrentHp=MinePlayer.GetComponent<PlayerHpBar>().GetPlayerHP();
            // PlayerMaxHp = MinePlayer.GetComponent<PlayerHpBar>().GetMaxPlayerHP();
            // PlayerHPSlider.value = (float)PlayerCurrentHp / (float)PlayerMaxHp;
        }
    }
    public void DClicked()
    {
        MinePlayer.GetComponent<Player2>().DashClicked();
    }
    public void DClickedUp()
    {
        MinePlayer.GetComponent<Player2>().DashClickedUp();
    }
    private void Attack()
    {
        MinePlayer.GetComponent<PlyerAnimator>().ButtonClicked();
    }
    public void AttackUp()
    {
        MinePlayer.GetComponent<PlyerAnimator>().ButtonClickedUp();
    }
    private void Jump()
    {
        MinePlayer.GetComponent<PlyerAnimator>().JumpClicked();
    }
    public float GetPlayerCurrentHP()
    {
        return PlayerCurrentHp;
    }
    public float GetPlayerMaxHP()
    {
        return PlayerMaxHp;
    }
    public bool GetPlayerFlg()
    {
        return isPlayer;
    }
    public void PlayerRespawn()
    {
        MinePlayer.gameObject.GetComponent<PlyerAnimator>().RespawnPlayer();

    }
    public void Dead()
    {
        
        DeadPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
