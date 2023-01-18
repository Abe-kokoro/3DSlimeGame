using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.InputSystem;
using System;
//PUN�̃R�[���o�b�N���󂯎���悤�ɂ���
public class GameController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject menu;
    [SerializeField] public static bool isPC  = true;
    [SerializeField] bool isMenu;
    [SerializeField] GameObject AndroidPanel;
    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[���g�̖��O��"Player"�ɐݒ肷��
        PhotonNetwork.NickName = TitleManager.PlayerName;

        //PhotonServerSettings�̐ݒ���e���g����
        //�}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();

        if (isPC)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            AndroidPanel.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            AndroidPanel.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPC)
        {

            if (Input.GetKeyDown("escape"))
            {
                if (!Menu.isMenu)
                {
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    menu.GetComponent<Menu>().Pause();




                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    menu.GetComponent<Menu>().Resume();




                }

            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
    //�}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        //�w��̖��O�̕����ɎQ������
        //�Ȃ���΍쐬���ĎQ������
        PhotonNetwork.JoinOrCreateRoom("RoomSlimeBattle",new RoomOptions(),TypedLobby.Default);
    }
    
    //�Q�[���T�[�o�[�ɐڑ��������������ɌĂ΂��R�[���o�b�N�֐�
    public override void OnJoinedRoom()
    {
        //�����_���Ɏ����̃L�������쐬
        var position = PlayerController.resPos;
        //�}���`�v���C�����܂ł̓R�����g�A�E�g
        //PhotonNetwork.Instantiate("Avator",position,Quaternion.identity);
        PhotonNetwork.Instantiate("Player",position,Quaternion.identity);

    }
    
}
