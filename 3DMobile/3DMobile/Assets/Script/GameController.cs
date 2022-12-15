using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
//PUN�̃R�[���o�b�N���󂯎���悤�ɂ���
public class GameController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[���g�̖��O��"Player"�ɐݒ肷��
        PhotonNetwork.NickName = "Player";

        //PhotonServerSettings�̐ݒ���e���g����
        //�}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //�}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        //�w��̖��O�̕����ɎQ������
        //�Ȃ���΍쐬���ĎQ������
        PhotonNetwork.JoinOrCreateRoom("RoomTTestATB01",new RoomOptions(),TypedLobby.Default);
    }

    //�Q�[���T�[�o�[�ɐڑ��������������ɌĂ΂��R�[���o�b�N�֐�
    public override void OnJoinedRoom()
    {
        //�����_���Ɏ����̃L�������쐬
        var position = new Vector3(Random.Range(-3.0f, 3.0f), 0.0f, Random.Range(-3.0f, 3.0f));

        PhotonNetwork.Instantiate("Avator",position,Quaternion.identity);
    }
}
