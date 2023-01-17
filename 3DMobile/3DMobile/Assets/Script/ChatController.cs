using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class ChatController : MonoBehaviourPunCallbacks
{
    string ChatLog;
    [SerializeField] TextMeshProUGUI ChatView;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //ChatView.text = this.gameObject.GetComponent<TextMeshProUGUI>().text;
    }
    
    
}