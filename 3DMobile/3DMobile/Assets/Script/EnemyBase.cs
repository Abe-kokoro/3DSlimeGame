using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Security.Cryptography;
using TMPro;
using System;

public class EnemyBase : MonoBehaviourPunCallbacks, IPunObservable
{
    //! 攻撃判定用コライダーコール.
    [SerializeField] ColliderCallReceiver attackHitColliderCall = null;
    // 攻撃間隔.
    [SerializeField] float attackInterval = 3f;
    //敵キャラクターHPバー
    [SerializeField] Slider EnemyHPBar;
   
    // アニメーター.
    Animator animator = null;
    // ----------------------------------------------------------
    /// <summary>
    /// ステータス.
    /// </summary>
    // ----------------------------------------------------------
    [System.Serializable]
    public class Status
    {
        // HP.
        public int Hp = 10;
        // 攻撃力.
        public int Power = 1;
        public bool isAlive = true;
        public float DefaultSpeed = 4.0f;
        public float TraseSpeed = 8.0f;

    }

    // 基本ステータス.
    [SerializeField] Status DefaultStatus = new Status();
    // 現在のステータス.
    public Status CurrentStatus = new Status();
    // 周辺レーダーコライダーコール.
    [SerializeField] ColliderCallReceiver aroundColliderCall = null;
    [SerializeField] ColliderCallReceiver AtkColliderCall = null;

    // 攻撃状態フラグ.
    bool isBattle = false;
    bool isTrase = false;
    bool isMove = false;
    // 攻撃時間計測用.
    float attackTimer = 0f;
    [SerializeField]float RotateTimer = 0f;
    [SerializeField]float NextRotateCoolTime = 3f;
    [SerializeField] GameObject DmgText;
    [SerializeField] Transform EnemyCanvas;
    void Start()
    {
        
        animator = GetComponent<Animator>();
        // 周辺コライダーイベント登録.
        aroundColliderCall.TriggerEnterEvent.AddListener(OnAroundTriggerEnter);
        aroundColliderCall.TriggerStayEvent.AddListener(OnAroundTriggerStay);
        aroundColliderCall.TriggerExitEvent.AddListener(OnAroundTriggerExit);
        // 攻撃コライダーイベント登録.
        AtkColliderCall.TriggerEnterEvent.AddListener(OnAtkTriggerEnter);
        AtkColliderCall.TriggerStayEvent.AddListener(OnAtkTriggerStay);
        AtkColliderCall.TriggerExitEvent.AddListener(OnAtkTriggerExit);

        // 攻撃コライダーイベント登録.
        attackHitColliderCall.TriggerEnterEvent.AddListener(OnAttackTriggerEnter);
        // 最初に現在のステータスを基本ステータスとして設定.
        CurrentStatus.Hp = DefaultStatus.Hp;
        CurrentStatus.Power = DefaultStatus.Power;
        attackHitColliderCall.gameObject.SetActive(false);
        //DmgText.SetActive(false);
        
    }
    void Update()
    {
        EnemyHPBar.value = (float)CurrentStatus.Hp / (float)DefaultStatus.Hp;
        // 攻撃できる状態の時.
        if (isBattle == true)
        {
            animator.SetBool("isMove", false);
            attackTimer += Time.deltaTime;

            if (attackTimer >= 3f)
            {
                animator.SetTrigger("isAttack");
                attackTimer = 0;
            }
        }
        else
        {
            attackTimer = 0;
            if (isTrase == true)
            {
                animator.SetBool("isMove", true);
                if(isMove)
                this.transform.position += this.transform.forward * CurrentStatus.TraseSpeed * Time.deltaTime;
            }
            else
            {
                this.transform.position += this.transform.forward * CurrentStatus.DefaultSpeed * Time.deltaTime;
                RotateTimer += Time.deltaTime;
                animator.SetBool("isMove", false);
                if (RotateTimer >= NextRotateCoolTime)
                {
                    // 回転
                    //transform.RotateTo(new Vector3(1, 0, 0), 1, EasingTypes.BounceOut);
                    this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y + UnityEngine.Random.Range(30, 270), 0);
                    NextRotateCoolTime = UnityEngine.Random.Range(2, 10);
                    RotateTimer = 0;
                }
            }
        }
       
        if(!CurrentStatus.isAlive)
        {
            transform.localPosition = new Vector3(0, transform.localPosition.y, 30);
        }
    }
    // ----------------------------------------------------------
    /// <summary>
    /// 攻撃ヒット時コール.
    /// </summary>
    /// <param name="damage"> 食らったダメージ. </param>
    // ----------------------------------------------------------
    public void OnAttackHit(int damage,int dmgLevel)
    {
        CurrentStatus.Hp -= damage;
        Debug.Log("Hit Damage " + damage + "/CurrentHp = " + CurrentStatus.Hp);
        DmgText.GetComponent<TextMeshProUGUI>().text = ""+damage;
        Instantiate(DmgText, EnemyCanvas);
        Invoke("ClearDmg", 1);
        if (CurrentStatus.Hp <= 0)
        {
            OnDie();
        }
        else if(dmgLevel == 1)
        {
            animator.SetTrigger("isHit00");
        }
        else if(dmgLevel == 2)
        {
            animator.SetTrigger("isHit01");
        }
        else if (dmgLevel == 3)
        {
            animator.SetTrigger("isHit02");
        }
    }
    void ClearDmg()
    {
        for (int index = 0; index < EnemyCanvas.childCount; index++)
        {
            // RectTransform の指定だと削除が失敗するので gameObject を指定する
            Destroy(EnemyCanvas.GetChild(index).gameObject);
        }
    }
    // ----------------------------------------------------------
    /// <summary>
    /// 死亡時コール.
    /// </summary>
    // ----------------------------------------------------------
    void OnDie()
    {
        Debug.Log("死亡");
        animator.SetTrigger("isHit02");
    }
    void SlimeMoveStart()
    {
        isMove = true;
    }
    void SlimeMoveEnd()
    {
        isMove = false;
    }
    // ----------------------------------------------------------
    /// <summary>
    /// 死亡アニメーション終了時コール.
    /// </summary>
    // ----------------------------------------------------------
    void Anim_Hit02End()
    {
        //this.gameObject.SetActive(false);
        if (CurrentStatus.Hp <= 0)
        {
            CurrentStatus.isAlive = false;
        }
    }
    // ------------------------------------------------------------
    /// <summary>
    /// 周辺レーダーコライダーエンターイベントコール.
    /// </summary>
    /// <param name="other"> 接近コライダー. </param>
    // ------------------------------------------------------------
    void OnAtkTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isBattle = true;
            animator.SetTrigger("isAttack");
        }
    }
    // ------------------------------------------------------------
    /// <summary>
    /// 周辺レーダーコライダーステイイベントコール.
    /// </summary>
    /// <param name="other"> 接近コライダー. </param>
    // ------------------------------------------------------------
    void OnAtkTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var _dir = (other.gameObject.transform.position - this.transform.position).normalized;
            _dir.y = 0;
            this.transform.forward = _dir;
        }
    }
    // ------------------------------------------------------------
    /// <summary>
    /// 周辺レーダーコライダー終了イベントコール.
    /// </summary>
    /// <param name="other"> 接近コライダー. </param>
    // ------------------------------------------------------------
    void OnAtkTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isBattle = false;
        }
    }
    // ------------------------------------------------------------
    /// <summary>
    /// 周辺レーダーコライダーエンターイベントコール.
    /// </summary>
    /// <param name="other"> 接近コライダー. </param>
    // ------------------------------------------------------------
    void OnAroundTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTrase = true;
        }
    }
    // ------------------------------------------------------------
    /// <summary>
    /// 周辺レーダーコライダーステイイベントコール.
    /// </summary>
    /// <param name="other"> 接近コライダー. </param>
    // ------------------------------------------------------------
    void OnAroundTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var _dir = (other.gameObject.transform.position - this.transform.position).normalized;
            _dir.y = 0;
            this.transform.forward = _dir;

        }
    }
    // ------------------------------------------------------------
    /// <summary>
    /// 周辺レーダーコライダー終了イベントコール.
    /// </summary>
    /// <param name="other"> 接近コライダー. </param>
    // ------------------------------------------------------------
    void OnAroundTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTrase = false;
        }
    }
    // ------------------------------------------------------------
    /// <summary>
    /// 攻撃コライダーエンターイベントコール.
    /// </summary>
    /// <param name="other"> 接近コライダー. </param>
    // ------------------------------------------------------------
    void OnAttackTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var player = other.GetComponent<PlyerAnimator>();
            player?.OnEnemyAttackHit(CurrentStatus.Power);
            Debug.Log("プレイヤーに敵の攻撃がヒット！" + CurrentStatus.Power + "の力で攻撃！");
            attackHitColliderCall.gameObject.SetActive(false);
        }
    }
    // ----------------------------------------------------------
    /// <summary>
    /// 攻撃Hitアニメーションコール.
    /// </summary>
    // ----------------------------------------------------------
    void Anim_AttackHit()
    {
        attackHitColliderCall.gameObject.SetActive(true);
    }

    // ----------------------------------------------------------
    /// <summary>
    /// 攻撃アニメーション終了時コール.
    /// </summary>
    // ----------------------------------------------------------
    void Anim_AttackEnd()
    {
        attackHitColliderCall.gameObject.SetActive(false);
    }
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Transformの値をストリームに書き込んで送信する
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.localRotation);
            stream.SendNext(transform.localScale);
            stream.SendNext(CurrentStatus.Hp);
            stream.SendNext(CurrentStatus.isAlive);

        }
        else
        {
            // 受信したストリームを読み込んでTransformの値を更新する
            transform.localPosition = (Vector3)stream.ReceiveNext();
            transform.localRotation = (Quaternion)stream.ReceiveNext();
            transform.localScale = (Vector3)stream.ReceiveNext();
            CurrentStatus.Hp = (int)stream.ReceiveNext();
            CurrentStatus.isAlive = (bool)stream.ReceiveNext();

        }
    }
}