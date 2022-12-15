using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirect : MonoBehaviour
{
    [SerializeField] GameObject TPSCamera;
    private float TPSCameraEulerY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TPSCameraEulerY = TPSCamera.transform.eulerAngles.y;
        if (Player.Up)
        {
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, TPSCamera.transform.rotation, step);
             transform.eulerAngles = new Vector3(0,TPSCameraEulerY,0);
        }
        if (Player.Down)
        {
              transform.eulerAngles = new Vector3(0, TPSCameraEulerY-180, 0);
        }
        if (Player.Right)
        {
             transform.eulerAngles = new Vector3(0, TPSCameraEulerY+90, 0);
        }
        if (Player.Left)
        {
            transform.eulerAngles = new Vector3(0, TPSCameraEulerY-90, 0);
        }
    }
}
