using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.InputSystem;
using System;
using System.IO;
//PUNのコールバックを受け取れるようにする
public class GameController : MonoBehaviourPunCallbacks
{
    
    [SerializeField] GameObject menu;
    [SerializeField] public static bool isPC  = false;
    [SerializeField] bool isMenu;
    [SerializeField] GameObject AndroidPanel;
    [SerializeField] GameObject ChatPanel;
    [SerializeField] GameObject pController;
    public static bool Loaded = false;
  
    // Start is called before the first frame update
    void Start()
    {
        
        if (!TitleManager.LoadDataflg)
        {
            // プレイヤー自身の名前を"Player"に設定する
            PhotonNetwork.NickName = TitleManager.PlayerName;
            Loaded = true;
        }
        else
        {
            //PlayerSaveData PlayerLoadData = loadPlayerData();
            PlyerAnimator.PlayerSaveData PlayerLoadData = loadPlayerData();
            PhotonNetwork.NickName = PlayerLoadData.PlayerName;
            Loaded = false;
        }
        //PhotonServerSettingsの設定内容を使って
        if (TitleManager.isOffline)
        {

            PhotonNetwork.OfflineMode = true;
            OnConnectedToMaster();
        }
        else
        {

            PhotonNetwork.OfflineMode = false;

            PhotonNetwork.ConnectUsingSettings();
        }
        //マスターサーバーへ接続する

        if (isPC)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            AndroidPanel.SetActive(false);
            ChatPanel.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            AndroidPanel.SetActive(true);
            ChatPanel.SetActive(false);
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
                    
                    menu.GetComponent<Menu>().Pause();




                }
                else
                {
                    menu.GetComponent<Menu>().Resume();

                    //    Cursor.lockState = CursorLockMode.Locked;
                    //    Cursor.visible = false;
                    //
                    //menu.GetComponent<Menu>().Resume();




                }

            }
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            if(Input.GetKeyUp(KeyCode.LeftAlt))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        //if(!Loaded)
        //{
        //    if(pController.GetComponent<PlayerController>().MinePlayer)
        //    {
        //        if(TitleManager.LoadDataflg)
        //        {
        //            PlayerSaveData PlayerLoadData = loadPlayerData();
        //            PlyerAnimator MPlayer = pController.GetComponent<PlayerController>().MinePlayer.GetComponent<PlyerAnimator>();
        //            MPlayer.CurrentStatus.Lv = PlayerLoadData.Lv;
        //            MPlayer.CurrentStatus.Hp = PlayerLoadData.HP;
        //            MPlayer.CurrentStatus.Power = PlayerLoadData.Atk;
        //            Loaded = true;
        //        }
        //    }
        //}
    }
    //マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
       
        //指定の名前の部屋に参加する
        //なければ作成して参加する
        PhotonNetwork.JoinOrCreateRoom("RoomSlimeBattle",new RoomOptions(),TypedLobby.Default);
    }
    
    //ゲームサーバーに接続が成功した時に呼ばれるコールバック関数
    public override void OnJoinedRoom()
    {
        //ランダムに自分のキャラを作成
        var position = PlayerController.resPos;
        //マルチプレイ実装まではコメントアウト
        //PhotonNetwork.Instantiate("Avator",position,Quaternion.identity);
        PhotonNetwork.Instantiate("Player",position,Quaternion.identity);

    }
    public PlyerAnimator.PlayerSaveData loadPlayerData()
    {
        
            string datastr = "";
            StreamReader reader;
            reader = new StreamReader(Application.persistentDataPath + "/savedata.json");
            datastr = reader.ReadToEnd();
            reader.Close();

            return JsonUtility.FromJson<PlyerAnimator.PlayerSaveData>(datastr);
        
        
    }

}
