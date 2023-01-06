using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class PlayerHPText : MonoBehaviour//PunCallbacks
{
    [SerializeField] GameObject PController;
    [SerializeField] TextMeshProUGUI TextValue;
    [SerializeField] int CurrentHP;
    [SerializeField] int MaxHP;
    // Start is called before the first frame update
    private void Start()
    {
        

    }
    // Update is called once per frame
    void Update()
    {
        CurrentHP = (int)PController.GetComponent<PlayerController>().GetPlayerCurrentHP();
        MaxHP = (int)PController.GetComponent<PlayerController>().GetPlayerMaxHP();

        TextValue.text = CurrentHP+"/"+MaxHP;
        
    }
}
