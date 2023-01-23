using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;
public class ChatManager : MonoBehaviourPunCallbacks
{
    //オブジェクトと結びつける
    public TMP_InputField inputField;
    public TextMeshProUGUI text;
    [SerializeField] GameObject PlayerController;
    [SerializeField] TextMeshProUGUI Chat;
    [SerializeField] private GameObject SendButton;
    [SerializeField] Scrollbar scl;
    [SerializeField] bool isUI = false;
    [SerializeField] GameObject chatview;
    bool isSclSelect = false;
    public static string PlayerName;
    bool Writing  = false;
    void Start()
    {
        SendButton.GetComponent<Button>().onClick.AddListener(Send);

        PlayerName = TitleManager.PlayerName;
       
        //Componentを扱えるようにする
        inputField = inputField.GetComponent<TMP_InputField>();
        
    }
    private void Update()
    {
        

        
        if(Writing)
        {
            photonView.RPC(nameof(RPCUpdateChat), RpcTarget.AllBuffered, inputField.text);
            scl.value = 0.0f;
        }
        else
        {
            if(text.text ==Chat.text)
            {

            }
            else
            {

                
                text.text = Chat.text;
                
            }
            if(!isSclSelect)
            {
                scl.value = 0.0f;
            }
        }
        if(isUI)
        {
            
            if (text.text == "")
            {
                chatview.gameObject.SetActive(false);
            }
            else
            {
                chatview.gameObject.SetActive(true);

            }
        }
        

    }
    public void ValueChanged()
    {
        if (Input.GetKeyDown("enter"))
        {
            SendButton.GetComponent<Button>().Select();
        }
            

    }
    public void EndEdit()
    {
        
            SendButton.GetComponent<Button>().Select();
        
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
        //inputField.text = "メッセージを入力...";
    }
    private void Send()
    {
        if(!(inputField.text == "メッセージを入力...")&&!(inputField.text == ""))
        {
            Writing = true;
        }
        inputField.Select();
    }
    public void SelectScl()
    {
        isSclSelect = true;
    }
    public void DeselectScl()
    {
        isSclSelect = false;
    }
    [PunRPC]
    private void RPCUpdateChat(string ChatLog, PhotonMessageInfo info)
    {
        //テキストにinputFieldの内容を反映
        Chat.text += "\n[" + info.Sender.NickName + "]:" + ChatLog;

        inputField.text = "メッセージを入力...";
        Writing = false;
        scl.value = 0.0f;

    }
    
    

}
