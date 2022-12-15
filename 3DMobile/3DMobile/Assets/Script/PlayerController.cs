using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class PlayerController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField] GameObject MinePlayer;
    [SerializeField] GameObject DashButton;
    [SerializeField] GameObject JumpButton;
    [SerializeField] GameObject AttackButton;
    [SerializeField] Slider PlayerHPSlider;
    int PlayerCurrentHp;
    int PlayerMaxHp;
    void Start()
    {
        //DashButton.GetComponent<Button>().onClick.AddListener(Dash);
        JumpButton.GetComponent<Button>().onClick.AddListener(Jump);
        AttackButton.GetComponent<Button>().onClick.AddListener(Attack);
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerCurrentHp=MinePlayer.GetComponent<PlayerHpBar>().GetPlayerHP();
        //PlayerMaxHp = MinePlayer.GetComponent<PlayerHpBar>().GetMaxPlayerHP();
        //PlayerHPSlider.value = (float)PlayerCurrentHp / (float)PlayerMaxHp;
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            if (!MinePlayer)
            {
                MinePlayer = GameObject.FindGameObjectWithTag("Player");
                if (!MinePlayer.GetComponent<Player>().photonView.IsMine)
                {
                    MinePlayer = null;
                }
            }
        }
        if(MinePlayer)
        {
            PlayerCurrentHp=MinePlayer.GetComponent<PlayerHpBar>().GetPlayerHP();
            PlayerMaxHp = MinePlayer.GetComponent<PlayerHpBar>().GetMaxPlayerHP();
            PlayerHPSlider.value = (float)PlayerCurrentHp / (float)PlayerMaxHp;
        }
    }
    public void DClicked()
    {
        MinePlayer.GetComponent<Player>().DashClicked();
    }
    public void DClickedUp()
    {
        MinePlayer.GetComponent<Player>().DashClickedUp();
    }
    private void Attack()
    {
        MinePlayer.GetComponent<Player>().ButtonClicked();
    }
    private void Jump()
    {
        MinePlayer.GetComponent<Player>().JumpClicked();
    }
}
