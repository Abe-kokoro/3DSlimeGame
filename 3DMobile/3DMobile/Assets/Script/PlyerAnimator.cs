using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Photon.Pun;
using System.Diagnostics;

public class PlyerAnimator : MonoBehaviourPunCallbacks
{
    // -------------------------------------------------------
    /// <summary>
    /// ステータス.
    /// </summary>
    // -------------------------------------------------------
    [System.Serializable]
    public class Status
    {
        // 体力.
        public int Hp = 10;
        // 攻撃力.
        public int Power = 1;
    }
    //HPBar slider
    public Slider HPslider;

    [SerializeField]
    [Tooltip("effect")]
    private ParticleSystem Attack1particle;
    [SerializeField] private ParticleSystem Rengeki1particle;
    [SerializeField] private ParticleSystem Rengeki2particle;
    [SerializeField] private ParticleSystem Rengeki3particle;
    [SerializeField] private ParticleSystem Rengeki4particle;
    [SerializeField] private ParticleSystem Finalparticle;
    // 攻撃HitオブジェクトのColliderCall.
    [SerializeField] ColliderCallReceiver attackHitCall = null;
    // 基本ステータス.
    [SerializeField] Status DefaultStatus = new Status();
    

    // 現在のステータス.
    public Status CurrentStatus = new Status();

    // 攻撃判定用オブジェクト
    [SerializeField] GameObject attackHit = null;
    // アニメーター
    Animator animator = null;
    //ジャンプ力
    [SerializeField] float jumpPower = 20f;
    // リジッドボディ
    Rigidbody rigid = null;
    //! 攻撃アニメーション中フラグ.
    public bool isAttack = false;
    public bool isAttack2 = false;
    public bool isAttacking = false;
    public bool isAttackingMBUP = false;
    public bool isAttackChain = false;
    public bool isFinalAtk = false;
    // 設置判定用ColliderCall.
    [SerializeField] ColliderCallReceiver footColliderCall = null;
    // 接地フラグ.
    bool isGround = false;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        attackHit.SetActive(false);
        // FootSphereのイベント登録.
        footColliderCall.TriggerStayEvent.AddListener(OnFootTriggerStay);
        footColliderCall.TriggerExitEvent.AddListener(OnFootTriggerExit);
        // 攻撃判定用コライダーイベント登録.
        attackHitCall.TriggerEnterEvent.AddListener(OnAttackHitTriggerEnter);
        // 現在のステータスの初期化.
        CurrentStatus.Hp = DefaultStatus.Hp;
        CurrentStatus.Power = DefaultStatus.Power;
        HPslider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetMouseButtonDown(0)&&!isAttacking)
            {
                //AttackAction();
                AttackStart();
                isAttackingMBUP = false;
            }
            if (Input.GetMouseButtonDown(1))
            {
                AttackAction2();
            }

            if (Input.GetKeyDown("space"))
            {
                if (isGround == true)
                {
                    JumpAction();
                }
            }
            if(isAttacking)
            {
                if(Input.GetMouseButtonUp(0))
                {
                    isAttackingMBUP = true;
                }
                if (Input.GetMouseButtonDown(0)&&isAttackingMBUP)
                {
                    isAttackChain = true;
                    animator.SetBool("isAttackChain", true);
                    isAttackingMBUP = false;
                }
               
            }
        }
    }
    // ---------------------------------------------------------------------
    /// <summary>
    /// 攻撃判定トリガーエンターイベントコール.
    /// </summary>
    /// <param name="col"> 侵入したコライダー. </param>
    // ---------------------------------------------------------------------
    void OnAttackHitTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            var enemy = col.gameObject.GetComponent<EnemyBase>();
            if (isAttack)
            {


                enemy?.OnAttackHit(CurrentStatus.Power,1);
            }
            else if(isAttack2)
            {
                enemy?.OnAttackHit((int)(CurrentStatus.Power*3f),2);

            }
            else if(isFinalAtk)
            {
                enemy?.OnAttackHit(CurrentStatus.Power*2, 2);

            }
            else
            {
                enemy?.OnAttackHit(CurrentStatus.Power, 1);

            }

            attackHit.SetActive(false);
        }
    }
    void JumpAction()
    {
        rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }
    void AttackAction()
    {
        if (isAttack == false)
        {
            // AnimationのisAttackトリガーを起動.
            animator.SetTrigger("isAttack");
            // 攻撃開始.
            isAttack = true;
        }
    }
    void AttackAction2()
    {
        if (isAttack2 == false)
        {
            // AnimationのisAttackトリガーを起動.
            animator.SetTrigger("isAttack2");
            // 攻撃開始.
            isAttack2 = true;
        }
    }
    
    void Attack1_Start()
    {
        ParticleSystem newParticle = Instantiate(Attack1particle);
        newParticle.transform.position = this.transform.position+ newParticle.transform.position;
        Vector3 LocalAngles = newParticle.transform.localEulerAngles+this.transform.localEulerAngles;
        newParticle.transform.localEulerAngles = LocalAngles;

        newParticle.Play();
        Destroy(newParticle.gameObject, 1.0f);
    }
    void AnimAtk_Hit()
    {
        UnityEngine.Debug.Log("Hit");
        // 攻撃判定用オブジェクトを表示.
        attackHit.SetActive(true);
    }
    void Atk_Hit()
    {
        UnityEngine.Debug.Log("Hit");
        // 攻撃判定用オブジェクトを表示.
        attackHit.SetActive(true);
    }
    void Hit_End()
    {
        UnityEngine.Debug.Log("HitEnd");
        // 攻撃判定用オブジェクトを表示.
        attackHit.SetActive(false);
    }
    void AnimAtk_End()
    {
        UnityEngine.Debug.Log("End");
        // 攻撃判定用オブジェクトを非表示に.
        attackHit.SetActive(false);
        // 攻撃終了.
        isAttack = false;
        
    }

    void Attack2_Start()
    {
        UnityEngine.Debug.Log("Hit");
        // 攻撃判定用オブジェクトを表示.
        attackHit.SetActive(true);
        ParticleSystem newParticle = Instantiate(Finalparticle);
        newParticle.transform.position = this.transform.position + newParticle.transform.position;
        Vector3 LocalAngles = newParticle.transform.localEulerAngles + this.transform.localEulerAngles;
        newParticle.transform.localEulerAngles = LocalAngles;

        newParticle.Play();
        Destroy(newParticle.gameObject, 1.0f);
    }
    void Attack2_End()
    {
        UnityEngine.Debug.Log("End");
        // 攻撃判定用オブジェクトを非表示に.
        attackHit.SetActive(false);
        // 攻撃終了.
        isAttack2 = false;
    }
    void AttackStart()
    {
        if (isAttacking == false)
        {
            // AnimationのisAttackトリガーを起動.
            animator.SetBool("isAttacking",true);
            // 攻撃開始.
            isAttacking = true;
            isFinalAtk = false;
        }
    }
    void AttackS6_Start()
    {
        ParticleSystem newParticle = Instantiate(Rengeki1particle);
        newParticle.transform.position = this.transform.position + newParticle.transform.position;
        Vector3 LocalAngles = newParticle.transform.localEulerAngles + this.transform.localEulerAngles;
        newParticle.transform.localEulerAngles = LocalAngles;

        newParticle.Play();
        Destroy(newParticle.gameObject, 1.0f);
        if (isAttackChain)
        {
            UnityEngine.Debug.Log("Chain");
            isAttackChain = false;
            animator.SetBool("isAttackChain", false);
        }
    }
    void AttackS6_End()
    {
        UnityEngine.Debug.Log("End");
        // AnimationのisAttackトリガーを起動.
        animator.SetBool("isAttacking", false);
        // 攻撃開始.
        isAttacking = false;

    }
    void AttackS2_Start()
    {

        if (isAttackChain)
        {
            UnityEngine.Debug.Log("Chain");
            isAttackChain = false;
            animator.SetBool("isAttackChain", false);
        }
    }
    void AttackS2_Effect()
    {
        ParticleSystem newParticle = Instantiate(Rengeki2particle);
        newParticle.transform.position = this.transform.position + newParticle.transform.position;
        Vector3 LocalAngles = newParticle.transform.localEulerAngles + this.transform.localEulerAngles;
        newParticle.transform.localEulerAngles = LocalAngles;

        newParticle.Play();
        Destroy(newParticle.gameObject, 1.0f);
    }
    void AttackS2_End()
    {
        
            UnityEngine.Debug.Log("End");
            // AnimationのisAttackトリガーを起動.
            animator.SetBool("isAttacking", false);
            // 攻撃開始.
            isAttacking = false;

    }
    void AttackS5_Start()
    {
        
        if (isAttackChain)
        {
            UnityEngine.Debug.Log("Chain");
            isAttackChain = false;
            animator.SetBool("isAttackChain", false);
        }
    }
    void AttackS5_Effect()
    {
        ParticleSystem newParticle = Instantiate(Rengeki3particle);
        newParticle.transform.position = this.transform.position + newParticle.transform.position;
        Vector3 LocalAngles = newParticle.transform.localEulerAngles + this.transform.localEulerAngles;
        newParticle.transform.localEulerAngles = LocalAngles;

        newParticle.Play();
        Destroy(newParticle.gameObject, 1.0f);
    }
    void AttackS5_End()
    {
        
            UnityEngine.Debug.Log("End");
            // AnimationのisAttackトリガーを起動.
            animator.SetBool("isAttacking", false);
            // 攻撃開始.
            isAttacking = false;

    }
    void AttackWS2_Start()
    {
        isFinalAtk = true;
    }
    void AttackWS2_Effect()
    {
        ParticleSystem newParticle = Instantiate(Rengeki4particle);
        newParticle.transform.position = this.transform.position + newParticle.transform.position;
        Vector3 LocalAngles = newParticle.transform.localEulerAngles + this.transform.localEulerAngles;
        newParticle.transform.localEulerAngles = LocalAngles;
        
        newParticle.Play();
        Destroy(newParticle.gameObject, 1.0f);
    }
    void AttackWS2_End()
    {
        UnityEngine.Debug.Log("End");
        // AnimationのisAttackトリガーを起動.
        animator.SetBool("isAttacking", false);
        animator.SetBool("isAttackChain", false);
        // 攻撃開始.
        isAttacking = false;
        isAttackChain = false;
        isFinalAtk = false;
    }
    // ---------------------------------------------------------------------
    /// <summary>
    /// FootSphereトリガーステイコール.
    /// </summary>
    /// <param name="col"> 侵入したコライダー. </param>
    // ---------------------------------------------------------------------
    void OnFootTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Field")
        {
            if (isGround == false) isGround = true;
            if (animator.GetBool("isGround") == false) animator.SetBool("isGround", true);
        }
    }

    // ---------------------------------------------------------------------
    /// <summary>
    /// FootSphereトリガーイグジットコール.
    /// </summary>
    /// <param name="col"> 侵入したコライダー. </param>
    // ---------------------------------------------------------------------
    void OnFootTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Field")
        {
            isGround = false;
            animator.SetBool("isGround", false);
        }
    }
    public bool GetAttackAnim()
    {
        return isAttack;
    }
    // ---------------------------------------------------------------------
    /// <summary>
    /// 敵の攻撃がヒットしたときの処理.
    /// </summary>
    /// <param name="damage"> 食らったダメージ. </param>
    // ---------------------------------------------------------------------
    public void OnEnemyAttackHit(int damage)
    {
        //CurrentStatus.Hp -= damage;
        AddPlayerDamage(damage);
        if (CurrentStatus.Hp <= 0)
        {
            OnDie();
        }
        else
        {
            //UnityEngine.Debug.Log(damage + "のダメージを食らった!!残りHP" + CurrentStatus.Hp);
        }
    }
    public void AddPlayerDamage(int dmg)
    {
        CurrentStatus.Hp -= dmg;
        UnityEngine.Debug.Log(dmg + "のダメージを食らった!!残りHP" + CurrentStatus.Hp);
        HPslider.value = (float)CurrentStatus.Hp / (float)DefaultStatus.Hp;

    }
    public void AddPlayerHP(int HealValue)
    {
        CurrentStatus.Hp += HealValue;
        if(CurrentStatus.Hp>DefaultStatus.Hp)
        {
            CurrentStatus.Hp = DefaultStatus.Hp;
        }
        UnityEngine.Debug.Log("HPを"+HealValue + "回復した!!残りHP" + CurrentStatus.Hp);
        HPslider.value = (float)CurrentStatus.Hp / (float)DefaultStatus.Hp;

    }
    // ---------------------------------------------------------------------
    /// <summary>
    /// 死亡時処理.
    /// </summary>
    // ---------------------------------------------------------------------
    void OnDie()
    {
        UnityEngine.Debug.Log("死亡しました。");
    }
    public Vector2 GetPlayerHP()
    {
        Vector2 HPStatus = new Vector2(CurrentStatus.Hp, DefaultStatus.Hp);
        return HPStatus;
    }
    public bool GetPlayerisAttacking()
    {
        return isAttacking;
    }
    public bool GetPlayerisAttack2()
    {
        return isAttack2;
    }
    public bool GetPlayerisGrounnd()
    {
        return isGround;
    }
}
