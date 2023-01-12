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
    
    float PlayerCurrentHp;
    float PlayerMaxHp;
    
    void Start()
    {
        //DashButton.GetComponent<Button>().onClick.AddListener(Dash);
        JumpButton.GetComponent<Button>().onClick.AddListener(Jump);
        AttackButton.GetComponent<Button>().onClick.AddListener(Attack);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            if (!MinePlayer)
            {
                MinePlayer = GameObject.FindGameObjectWithTag("Player");
                //�}���`�܂�
                if (!MinePlayer.GetComponent<Player2>().photonView.IsMine)
                {
                    MinePlayer = null;
                }
            }
        }
        if(MinePlayer)
        {
           

            PlayerCurrentHp = MinePlayer.GetComponent<PlyerAnimator>().GetPlayerHP().x;
            PlayerMaxHp = MinePlayer.GetComponent<PlyerAnimator>().GetPlayerHP().y;
            PlayerHPSlider.value = PlayerCurrentHp / PlayerMaxHp;
            //�}���`�܂�
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

}
