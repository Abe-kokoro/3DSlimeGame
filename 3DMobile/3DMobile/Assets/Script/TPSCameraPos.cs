using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TPSCameraPos : MonoBehaviour
{
    [SerializeField] public Transform PlayerTransform;
    //[SerializeField] private GameObject objPlayer;
    [SerializeField] private float TPSCameraDistance;
    [SerializeField] public Vector2 TPSMouseSensi;
    [SerializeField] public Vector2 TPSScreenSensi;
    [SerializeField] private Vector2 TPSMouseMove;
    [SerializeField] private Slider ScreenSensiX;
    [SerializeField] private Slider ScreenSensiY;
    [SerializeField] private GameObject SensiXText;
    [SerializeField] private GameObject SensiYText;
    private bool[] b_touchObj = new bool[10];

    public static Vector2 MouseMove = Vector2.zero;
    private Vector3 pos = Vector3.zero;
    private Vector3 nowPos;
    private static float MouseX;
    [Header("コントローラー感度")]
    [SerializeField] public static Vector2 RightStickSensi;
    [Header("コントローラーデッドゾーン")]
    [SerializeField] public static float RightStickDeadZone;
    [Header("デバッグ用")]
    [SerializeField] private Vector2 RightStick;
    [SerializeField] private float RoghtStickRot;
    [SerializeField] private Vector2 MouseAxis;
    [SerializeField] private Vector2 MouseAxisOld;
    [SerializeField] private float WheelAxis;
    private bool ChargeFlg = false;
    Touch touchInfo;
    private bool CameraMoveFlg = false;
    private Vector2 TapPos;
    private Vector2 TapOldPos;
    private int TapIndex = 0;
    private GUIStyle style;
    private int TouchCount;
    Vector2 OldScreensensi;
    public static bool IsMenu = false;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        style = new GUIStyle();
        style.fontSize = 50;
        MouseMove.y = 0.6f;
        MouseMove.x = 0;
        nowPos = transform.position;
        for (int i=0;i<10; i++)
        {

            b_touchObj[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!Menu.isMenu)
        {
            Vector2 SetSensi = new Vector2(ScreenSensiX.value, ScreenSensiY.value);


            OldScreensensi = new Vector2(ScreenSensiX.value, ScreenSensiY.value);

            SetScreenSensi(SetSensi / 50);
            SetMouseSensi(SetSensi / 5);
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                if (!PlayerTransform)
                {
                    PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;

                }
                else
                {




                    if (Mathf.Approximately(Time.timeScale, 0f))
                    {
                        return;
                    }
                    TouchCount = Input.touchCount;
                    CameraMoveFlg = false;
                    for (int i = 0; i < Input.touchCount; i++)
                    {

                        // タッチ数分、タッチ情報を確認する
                        touchInfo = Input.GetTouch(i);
                        

                        //if (touchInfo.position.x > Screen.width / 2)
                        if (touchInfo.position.x > 0)
                        {
                                if (touchInfo.phase == TouchPhase.Began)
                            {
                                Debug.Log("TouchBigan");

                                //RaycastAllの引数（PointerEventData）作成
                                PointerEventData pointData = new PointerEventData(EventSystem.current);

                                //RaycastAllの結果格納用List
                                List<RaycastResult> RayResult = new List<RaycastResult>();

                                //PointerEventDataにマウスの位置をセット
                                pointData.position = touchInfo.position;
                                //RayCast（スクリーン座標）
                                EventSystem.current.RaycastAll(pointData, RayResult);
                                b_touchObj[i] = false;
                                foreach (RaycastResult result in RayResult)
                                {
                                    if(result.gameObject.tag=="UI")
                                    {
                                        Debug.Log("TouchhitObj");
                                        b_touchObj[i] = true;
                                    }
                                    
                                }

                                if(!b_touchObj[i])
                                {
                                    TapOldPos = touchInfo.position;
                                    Debug.Log("NoHitobj");
                                }

                                //Vector2 world_point = Camera.main.ScreenToWorldPoint(touchInfo.position);

                                //foreach (RaycastHit2D hit in Physics2D.RaycastAll(world_point, Vector2.zero))
                                //{

                                //    //オブジェクトが見つかったときの処理
                                //    if (hit)
                                //    {

                                //        //タグがアイテムなら
                                //        if (hit.collider.gameObject.CompareTag("UI"))
                                //        {

                                //            Debug.Log("TouchhitObj");
                                //            b_touchObj[i] = true;
                                //        }
                                //        else
                                //        {

                                //            Debug.Log("NoTouchhitObj");
                                //            b_touchObj[i] = false;
                                            

                                //        }
                                //    }
                                //    else
                                //    {

                                //        Debug.Log("NoTouchhitObj");
                                //        b_touchObj[i] = false;
                                        

                                //    }
                                //}
                                // RaycastHit2D hit = Physics2D.Raycast(world_point, Vector2.zero);
                                
                            }
                            if (touchInfo.phase != TouchPhase.Ended && touchInfo.phase != TouchPhase.Canceled)
                            {
                                if (!b_touchObj[i])
                                {
                                    CameraMoveFlg = true;
                                    TapPos = touchInfo.position;
                                    TapIndex = i;


                                    transform.LookAt(PlayerTransform);
                                    //MouseAxis.x = Input.GetAxis("Mouse X")-MouseAxisOld.x;
                                    //MouseAxis.y = Input.GetAxis("Mouse Y") - MouseAxisOld.x;

                                    MouseAxis.x = TapPos.x - TapOldPos.x;
                                    MouseAxis.y = TapPos.y - TapOldPos.y;




                                    // MouseMove -= new Vector2(-Input.GetAxis("Mouse X") * TPSMouseSensi.x, Input.GetAxis("Mouse Y")) * Time.deltaTime * TPSMouseSensi.y;
                                    MouseMove -= new Vector2(-MouseAxis.x * TPSScreenSensi.x * Time.deltaTime * 2, MouseAxis.y * Time.deltaTime * TPSScreenSensi.y);

                                    WheelAxis = -Input.GetAxis("Mouse ScrollWheel");
                                    TPSCameraDistance += WheelAxis;
                                    TapOldPos = touchInfo.position;
                                }
                            }
                            else
                            {
                                b_touchObj[i] = false;
                                CameraMoveFlg = false;
                            }
                        }


                    }
                    if (TouchCount == 0)
                    {
                        transform.LookAt(PlayerTransform);
                        //MouseAxis.x = Input.GetAxis("Mouse X")-MouseAxisOld.x;
                        //MouseAxis.y = Input.GetAxis("Mouse Y") - MouseAxisOld.x;

                        // MouseAxis.x = TapPos.x - TapOldPos.x;
                        //MouseAxis.y = TapPos.y - TapOldPos.y;




                        MouseMove -= new Vector2(-Input.GetAxis("Mouse X") * TPSMouseSensi.x, Input.GetAxis("Mouse Y")) * Time.deltaTime * TPSMouseSensi.y;
                        //MouseMove -= new Vector2(-MouseAxis.x * TPSMouseSensi.x * Time.deltaTime * 2, MouseAxis.y * Time.deltaTime * TPSMouseSensi.y);

                        WheelAxis = -Input.GetAxis("Mouse ScrollWheel");
                        TPSCameraDistance += WheelAxis;
                        TapOldPos = touchInfo.position;

                    }
                    //  if (Screen.width / 2 < Input.mousePosition.x && Input.GetMouseButton(0))
                    //      if(0<Input.mousePosition.x)

                    TPSCameraDistance = Mathf.Clamp(TPSCameraDistance, 4.0f, 12);
                    MouseMove.y = Mathf.Clamp(MouseMove.y, -0.4f + 0.5f, 0.4f + 0.5f);
                    //MouseMove += new Vector2(Input.GetAxis("Mouse X")*FPSMouseSensi, Input.GetAxis("Mouse Y")*FPSMouseSensi);
                    // 球面座標系変換
                    pos.x = TPSCameraDistance * Mathf.Sin(MouseMove.y * Mathf.PI) * Mathf.Cos(MouseMove.x * Mathf.PI);
                    pos.y = -TPSCameraDistance * Mathf.Cos(MouseMove.y * Mathf.PI);
                    pos.z = -TPSCameraDistance * Mathf.Sin(MouseMove.y * Mathf.PI) * Mathf.Sin(MouseMove.x * Mathf.PI);
                    //pos *= nowPos.z;

                    //pos.y += nowPos.y;

                    MouseX = MouseMove.x - 0.5f;
                    // 座標の更新
                    if (!(PlayerTransform == null))
                        transform.position = pos + PlayerTransform.position;
                    // transform.LookAt(PlayerTransform.position);
                    TPSMouseMove = MouseMove;
                    // if(1980 / 2 > Input.mousePosition.x)
                    //if (Screen.width / 2 < Input.mousePosition.x)
                    if (CameraMoveFlg)
                    {

                        //MouseAxisOld = Input.mousePosition;
                        //MouseAxisOld.x = Input.GetAxis("Mouse X");
                        //MouseAxisOld.y = Input.GetAxis("Mouse Y");
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {

    }
    public static float GetMouseX()
    {
        return MouseX;
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
    void OnGUI()
    {
        //デバッグ表示
       // GUILayout.Label($"TapPos:x{TapPos.x}y{TapPos.y}", style);
       // GUILayout.Label($"TapOldPos:x{TapOldPos.x}y{TapOldPos.y}", style);
       // GUILayout.Label($"TapCount{TouchCount}", style);
    }
    public void SetMouseSensi(Vector2 MSens)
    {
        TPSMouseSensi = MSens;
    }
    public void SetScreenSensi(Vector2 SSens)
    {
        TPSScreenSensi = SSens;
    }
    public Vector2 GetMouseSensi()
    {
        return TPSMouseSensi;
    }
    public Vector2 GetScreenSensi()
    {
        return TPSScreenSensi;
    }
}
