using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerName : MonoBehaviourPunCallbacks
{
    
    // Start is called before the first frame update
    private void Start()
    {
        

    }
    // Update is called once per frame
    void Update()
    {
        var nameLabel = GetComponent<TextMeshPro>();
        // プレイヤー名とプレイヤーIDを表示する
           nameLabel.text = $"{photonView.Owner.NickName}({photonView.OwnerActorNr})";
        //nameLabel.text = "player";

    }
}
