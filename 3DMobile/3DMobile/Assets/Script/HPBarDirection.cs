using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class HPBarDirection : MonoBehaviourPunCallbacks
{
    public Canvas canvas;
    public GameObject PlayerCanvas;
    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        
        if (photonView.IsMine)
        {
            if(PlayerCanvas == null)
            {

            }
            else
            {
               PlayerCanvas.SetActive(false);

            }
        }
        else
        {
            canvas.transform.rotation = Camera.main.transform.rotation;

        }
    }
}
