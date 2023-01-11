using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
public class DmgTextMove : MonoBehaviour
{
    Vector3 MoveValue;
    // Start is called before the first frame update
    void Start()
    {
        MoveValue = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f) * Time.deltaTime*3, 0.1f*Time.deltaTime*3, -0.1f * Time.deltaTime );

    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<RectTransform>().position += MoveValue;
    }
}
