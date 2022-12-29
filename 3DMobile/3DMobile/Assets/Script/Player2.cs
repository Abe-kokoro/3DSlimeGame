using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Cinemachine;
using Photon.Pun;
public class Player2 : MonoBehaviourPunCallbacks
{
   //[SerializeField] GameObject MainCamera;
    private Joystick joystick;
    [SerializeField] private float PlayerMove;
    [SerializeField] private Vector2 JoystickValue;
    [SerializeField] private GameObject TPSCamera;
    [SerializeField] private bool PlayerMoveFlg = false;
    private Rigidbody RB;
    public bool AttackFlg = false;
    public bool JumpFlg = false;
    public bool DashFlg = false;
    public static bool Right, Left, Up, Down;
    [SerializeField] private float JoystickAngle = 0.0f;
    [SerializeField] public static bool MouseFlg = false;
    [SerializeField] private float RotateSpeed = 0.1f;
    //[SerializeField] private GameObject PlayerDirect;
    private int PlayerRotate = 0;
    [SerializeField] private float PlayerRotateVar = 0.0f;
    private Vector3 FixedAngle;
    // Start is called before the first frame update
    //[SerializeField] bool isWalk = false;
    // アニメーター
    Animator animator = null;
    bool isDash = false;
    [SerializeField] bool OnField = false;
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        RB = this.GetComponent<Rigidbody>();
        TPSCamera = GameObject.FindGameObjectWithTag("Camera");
        animator = GetComponent<Animator>();

    }

    private void FixedUpdate()
    {
              transform.eulerAngles += FixedAngle;
    }
    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown("m"))
            {
                MouseFlg = !MouseFlg;
            }
            PlayerMoveFlg = false;
            Right = false;
            Left = false;
            Up = false;
            Down = false;
            float CameraEulerY = TPSCamera.transform.eulerAngles.y;
            Vector2 direction;
            if (joystick)
            {
                MouseFlg = true;

                JoystickValue.x = joystick.Horizontal;
                JoystickValue.y = joystick.Vertical;
                direction = joystick.Direction;
                if (JoystickValue.magnitude > 0.2f)
                {
                    MouseFlg = false;
                    PlayerMoveFlg = true;
                    JoystickAngle = GetAngle(new Vector3(0, 0), JoystickValue);
                    if (JoystickValue.y > 0)
                    {
                        Up = true;
                    }
                    if (JoystickValue.x > 0)
                    {
                        Right = true;
                    }
                    if (JoystickValue.y < 0)
                    {
                        Down = true;
                    }
                    if (JoystickValue.x < 0)
                    {
                        Left = true;
                    }

                }
            }
            //this.transform.forward += new Vector3(x *Time.deltaTime, 0, z *Time.deltaTime);

            //this.transform.position += new Vector3(x * Time.deltaTime*PlayerMove, 0, z * Time.deltaTime*PlayerMove);
            if (Input.GetKey("w"))
            {
                PlayerMoveFlg = true;
                Up = true;
            }
            if (Input.GetKey("s"))
            {
                PlayerMoveFlg = true;
                Down = true;
            }
            if (Input.GetKey("a"))
            {
                PlayerMoveFlg = true;
                Left = true;
            }
            if (Input.GetKey("d"))
            {
                PlayerMoveFlg = true;
                Right = true;
            }
            float step = 0;
            step = RotateSpeed * Time.deltaTime;
             if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashFlg = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            DashFlg = false;
        }
        if (this.GetComponent<PlyerAnimator>().GetAttackAnim()==false)
        {
            if (PlayerMoveFlg)
            {
                if (DashFlg)
                {
                    transform.position += transform.forward * PlayerMove * 2.0f * Time.deltaTime;
                    //isDash = true;
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        transform.position += transform.forward * PlayerMove * 2.0f * Time.deltaTime;
                        //isDash = true;
                    }
                    else
                    {
                        //isDash = false;   
                    }
                    transform.position += transform.forward * PlayerMove * Time.deltaTime;
                }
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, PlayerDirect.transform.rotation, 1);

            }
        }
        else
        {
            //isDash = false;
        }



            //
            if (Up)
            {
                // transform.eulerAngles = new Vector3(0,CameraEulerY,0);
                PlayerRotate = 0;
            }
            if (Down)
            {
                //transform.eulerAngles = new Vector3(0, CameraEulerY-180, 0);
                PlayerRotate = -180;
            }
            if (Right)
            {
                //transform.eulerAngles = new Vector3(0, CameraEulerY+90, 0);
                PlayerRotate = 90;
            }
            if (Left)
            {
                //transform.eulerAngles = new Vector3(0, CameraEulerY-90, 0);
                PlayerRotate = -90;
            }
            if (Up && Right)
            {
                PlayerRotate = 45;
                //transform.eulerAngles = new Vector3(0, CameraEulerY+ 45, 0);
            }
            if (Up && Left)
            {
                PlayerRotate = -45;
                //transform.eulerAngles = new Vector3(0, CameraEulerY - 45, 0);
            }
            if (Down && Right)
            {
                PlayerRotate = 135;
                //transform.eulerAngles = new Vector3(0, CameraEulerY + 135, 0);
            }
            if (Down && Left)
            {
                PlayerRotate = -135;
                //transform.eulerAngles = new Vector3(0, CameraEulerY - 135, 0);
            }
            if (Input.GetKey("q"))
            {
                transform.eulerAngles += new Vector3(0, 1, 0);
            }
            PlayerRotateVar = -(CameraEulerY) + transform.eulerAngles.y - PlayerRotate;
            if (PlayerRotateVar < -180)
            {
                PlayerRotateVar += 360;
            }
            if (PlayerRotateVar > 180)
            {
                PlayerRotateVar -= 360;
            }
            PlayerRotateVar = Mathf.Round(PlayerRotateVar);
            //transform.eulerAngles = new Vector3(0,transform.eulerAngles.y*0.9f+(CameraEulerY+PlayerRotate)*0.1f , 0);
            if (!(PlayerRotateVar == 0) && PlayerMoveFlg)
            {
                FixedAngle = new Vector3(0, -PlayerRotateVar * 0.1f, 0);
                //transform.eulerAngles += FixedAngle;
            }
            if(!PlayerMoveFlg)
            {
            FixedAngle = new Vector3(0, 0, 0);
            }
            if(PlayerMoveFlg)
        {
            animator.SetBool("isWalk", true);
            if (DashFlg)
            {
                animator.SetBool("isRun", true);

            }
            else
            {
                animator.SetBool("isRun", false);

            }
        }
        else
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isRun", false);
        }
        

    }
    float GetAngle(Vector2 start, Vector2 target)
    {
        Vector2 dt = target - start;
        float rad = Mathf.Atan2(dt.x, dt.y);
        float degree = rad * Mathf.Rad2Deg;

        if (degree < 0)
        {
            degree += 360;
        }

        return degree;
    }
    public static bool GetMouseFlg()
    {
        return MouseFlg;
    }
    public static bool GetUp()
    {
        return Up;
    }
    public static bool GetDown()
    {
        return Down;
    }
    public static bool GetLeft()
    {
        return Left;
    }
    public static bool GetRight()
    {
        return Right;
    }
    public void ButtonClicked()
    {
        AttackFlg = true;
    }
    public void JumpClicked()
    {
        JumpFlg = true;
    }
    public void DashClicked()
    {
        DashFlg = true;
    }
    public void DashClickedUp()
    {
        DashFlg = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ("Field"))
        {
            OnField = true;
        }
        else
        {
            OnField = false;
        }
    }
}
