using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
//PUNのコールバックを受け取れるようにする
public class GameController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        // プレイヤー自身の名前を"Player"に設定する
        PhotonNetwork.NickName = "Player";

        //PhotonServerSettingsの設定内容を使って
        //マスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        //指定の名前の部屋に参加する
        //なければ作成して参加する
        PhotonNetwork.JoinOrCreateRoom("RoomTTestATB01",new RoomOptions(),TypedLobby.Default);
    }

    //ゲームサーバーに接続が成功した時に呼ばれるコールバック関数
    public override void OnJoinedRoom()
    {
        //ランダムに自分のキャラを作成
        var position = new Vector3(Random.Range(-3.0f, 3.0f), 0.0f, Random.Range(-3.0f, 3.0f));

        PhotonNetwork.Instantiate("Avator",position,Quaternion.identity);
    }
}
