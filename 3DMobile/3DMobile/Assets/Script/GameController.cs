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
    bool isMenue = false;
    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[���g�̖��O��"Player"�ɐݒ肷��
        PhotonNetwork.NickName = "Player";

        //PhotonServerSettings�̐ݒ���e���g����
        //�}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            isMenue = !isMenue;
            Menue();
        }
        
    }
    //�}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        //�w��̖��O�̕����ɎQ������
        //�Ȃ���΍쐬���ĎQ������
        PhotonNetwork.JoinOrCreateRoom("RoomTTestATB01",new RoomOptions(),TypedLobby.Default);
    }
    void Menue()
    {
        if(isMenue)
        {

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    //�Q�[���T�[�o�[�ɐڑ��������������ɌĂ΂��R�[���o�b�N�֐�
    public override void OnJoinedRoom()
    {
        //�����_���Ɏ����̃L�������쐬
        var position = new Vector3(0, 5.0f, 0);
        //�}���`�v���C�����܂ł̓R�����g�A�E�g
        //PhotonNetwork.Instantiate("Avator",position,Quaternion.identity);
        PhotonNetwork.Instantiate("Player",position,Quaternion.identity);

    }
}
