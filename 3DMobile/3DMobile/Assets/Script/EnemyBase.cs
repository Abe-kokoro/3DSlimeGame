using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //! 攻撃判定用コライダーコール.
    [SerializeField] ColliderCallReceiver attackHitColliderCall = null;
    // 攻撃間隔.
    [SerializeField] float attackInterval = 3f;
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
    }

    // 基本ステータス.
    [SerializeField] Status DefaultStatus = new Status();
    // 現在のステータス.
    public Status CurrentStatus = new Status();
    // 周辺レーダーコライダーコール.
    [SerializeField] ColliderCallReceiver aroundColliderCall = null;
    // 攻撃状態フラグ.
    bool isBattle = false;
    // 攻撃時間計測用.
    float attackTimer = 0f;

    void Start()
    {
        
        animator = GetComponent<Animator>();
        // 周辺コライダーイベント登録.
        aroundColliderCall.TriggerEnterEvent.AddListener(OnAroundTriggerEnter);
        aroundColliderCall.TriggerStayEvent.AddListener(OnAroundTriggerStay);
        aroundColliderCall.TriggerExitEvent.AddListener(OnAroundTriggerExit);
        // 攻撃コライダーイベント登録.
        attackHitColliderCall.TriggerEnterEvent.AddListener(OnAttackTriggerEnter);
        // 最初に現在のステータスを基本ステータスとして設定.
        CurrentStatus.Hp = DefaultStatus.Hp;
        CurrentStatus.Power = DefaultStatus.Power;
        attackHitColliderCall.gameObject.SetActive(false);
    }
    void Update()
    {
        // 攻撃できる状態の時.
        if (isBattle == true)
        {
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
        }
    }
    // ----------------------------------------------------------
    /// <summary>
    /// 攻撃ヒット時コール.
    /// </summary>
    /// <param name="damage"> 食らったダメージ. </param>
    // ----------------------------------------------------------
    public void OnAttackHit(int damage)
    {
        CurrentStatus.Hp -= damage;
        Debug.Log("Hit Damage " + damage + "/CurrentHp = " + CurrentStatus.Hp);

        if (CurrentStatus.Hp <= 0)
        {
            OnDie();
        }
        else
        {
            animator.SetTrigger("isHit01");
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

    // ----------------------------------------------------------
    /// <summary>
    /// 死亡アニメーション終了時コール.
    /// </summary>
    // ----------------------------------------------------------
    void Anim_Hit02End()
    {
        this.gameObject.SetActive(false);

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
            isBattle = true;
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
            isBattle = false;
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
}