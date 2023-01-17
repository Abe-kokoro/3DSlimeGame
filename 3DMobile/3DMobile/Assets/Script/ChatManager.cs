using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class ChatManager : MonoBehaviourPunCallbacks
{
    //�I�u�W�F�N�g�ƌ��т���
    public TMP_InputField inputField;
    public TextMeshProUGUI text;
    [SerializeField] GameObject PlayerController;
    [SerializeField] TextMeshProUGUI Chat;
    [SerializeField] private GameObject SendButton;
    public static string PlayerName;
    bool Writing  = false;
    void Start()
    {
        SendButton.GetComponent<Button>().onClick.AddListener(Send);

        PlayerName = TitleManager.PlayerName;
       
        //Component��������悤�ɂ���
        inputField = inputField.GetComponent<TMP_InputField>();
        
    }
    private void Update()
    {
        if(Writing)
        {
            photonView.RPC(nameof(RPCUpdateChat), RpcTarget.AllBuffered, inputField.text);
        }
        else
        {
            text.text = Chat.text;
        }

    }
    
    public void InputText()
    {

       // Writing = true;
        
    }
    public void InputStart()
    {
        inputField.text = "";
    }
    public void InputExit()
    {
        //inputField.text = "���b�Z�[�W�����...";
    }
    private void Send()
    {
        Writing = true;
    }
    [PunRPC]
    private void RPCUpdateChat(string ChatLog, PhotonMessageInfo info)
    {
        //�e�L�X�g��inputField�̓��e�𔽉f
        Chat.text += "\n[" + info.Sender.NickName + "]:" + ChatLog;

        inputField.text = "���b�Z�[�W�����...";
        Writing = false;
       
        
    }
    

}
