using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] float FarClipping = 100;
    // Start is called before the first frame update
    void Start()
    {
        //this.GetComponent<Camera>().farClipPlane = FarClipping;
    }

    // Update is called once per frame
    void Update()
    {
        //this.GetComponent<Camera>().farClipPlane = FarClipping;
        FarClipping = this.GetComponent<Camera>().farClipPlane;
    }
}
