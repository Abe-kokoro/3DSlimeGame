using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class DeadMSG : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject deadpanel;
    [SerializeField] GameObject TrueButton;
    [SerializeField] GameObject FalseButton;
    [SerializeField] PlayerController Controller;
    // Start is called before the first frame update
    void Start()
    {
        TrueButton.GetComponent<Button>().onClick.AddListener(TrueClicked);
        FalseButton.GetComponent<Button>().onClick.AddListener(FalseClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TrueClicked()
    {
        deadpanel.SetActive(false);
        
        Controller.PlayerRespawn();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void FalseClicked()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("TitleScene");
    }
}
