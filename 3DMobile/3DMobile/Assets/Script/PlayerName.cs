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
        // �v���C���[���ƃv���C���[ID��\������
           nameLabel.text = $"{photonView.Owner.NickName}({photonView.OwnerActorNr})";
        //nameLabel.text = "player";

    }
}
