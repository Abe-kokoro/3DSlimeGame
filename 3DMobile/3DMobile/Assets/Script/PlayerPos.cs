using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    [SerializeField] GameObject PlayerObj;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerObj)
            PlayerObj = GameObject.FindGameObjectWithTag("Player");
        if(PlayerObj)
        this.transform.position = PlayerObj.transform.position;
    }
}
