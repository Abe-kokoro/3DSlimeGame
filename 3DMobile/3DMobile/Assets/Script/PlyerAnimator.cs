using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Photon.Pun;
public class PlyerAnimator : MonoBehaviourPunCallbacks
{
    // -------------------------------------------------------
    /// <summary>
    /// �X�e�[�^�X.
    /// </summary>
    // -------------------------------------------------------
    [System.Serializable]
    public class Status
    {
        // �̗�.
        public int Hp = 10;
        // �U����.
        public int Power = 1;
    }
    //HPBar slider
    public Slider HPslider;

    [SerializeField]
    [Tooltip("effect")]
    private ParticleSystem Attack1particle;

    // �U��Hit�I�u�W�F�N�g��ColliderCall.
    [SerializeField] ColliderCallReceiver attackHitCall = null;
    // ��{�X�e�[�^�X.
    [SerializeField] Status DefaultStatus = new Status();
    // ���݂̃X�e�[�^�X.
    public Status CurrentStatus = new Status();

    // �U������p�I�u�W�F�N�g
    [SerializeField] GameObject attackHit = null;
    // �A�j���[�^�[
    Animator animator = null;
    //�W�����v��
    [SerializeField] float jumpPower = 20f;
    // ���W�b�h�{�f�B
    Rigidbody rigid = null;
    //! �U���A�j���[�V�������t���O.
    public bool isAttack = false;
    public bool isAttack2 = false;

    // �ݒu����pColliderCall.
    [SerializeField] ColliderCallReceiver footColliderCall = null;
    // �ڒn�t���O.
    bool isGround = false;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        attackHit.SetActive(false);
        // FootSphere�̃C�x���g�o�^.
        footColliderCall.TriggerStayEvent.AddListener(OnFootTriggerStay);
        footColliderCall.TriggerExitEvent.AddListener(OnFootTriggerExit);
        // �U������p�R���C�_�[�C�x���g�o�^.
        attackHitCall.TriggerEnterEvent.AddListener(OnAttackHitTriggerEnter);
        // ���݂̃X�e�[�^�X�̏�����.
        CurrentStatus.Hp = DefaultStatus.Hp;
        CurrentStatus.Power = DefaultStatus.Power;
        HPslider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                AttackAction();
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
        }
    }
    // ---------------------------------------------------------------------
    /// <summary>
    /// �U������g���K�[�G���^�[�C�x���g�R�[��.
    /// </summary>
    /// <param name="col"> �N�������R���C�_�[. </param>
    // ---------------------------------------------------------------------
    void OnAttackHitTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            var enemy = col.gameObject.GetComponent<EnemyBase>();
            if (isAttack)
            {


                enemy?.OnAttackHit(CurrentStatus.Power);
            }
            if(isAttack2)
            {
                enemy?.OnAttackHit((int)(CurrentStatus.Power*1.5f));

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
            // Animation��isAttack�g���K�[���N��.
            animator.SetTrigger("isAttack");
            // �U���J�n.
            isAttack = true;
        }
    }
    void AttackAction2()
    {
        if (isAttack2 == false)
        {
            // Animation��isAttack�g���K�[���N��.
            animator.SetTrigger("isAttack2");
            // �U���J�n.
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
        Debug.Log("Hit");
        // �U������p�I�u�W�F�N�g��\��.
        attackHit.SetActive(true);
    }
    void AnimAtk_End()
    {
        Debug.Log("End");
        // �U������p�I�u�W�F�N�g���\����.
        attackHit.SetActive(false);
        // �U���I��.
        isAttack = false;
        
    }

    void Attack2_Start()
    {
        Debug.Log("Hit");
        // �U������p�I�u�W�F�N�g��\��.
        attackHit.SetActive(true);
    }
    void Attack2_End()
    {
        Debug.Log("End");
        // �U������p�I�u�W�F�N�g���\����.
        attackHit.SetActive(false);
        // �U���I��.
        isAttack2 = false;
    }
    // ---------------------------------------------------------------------
    /// <summary>
    /// FootSphere�g���K�[�X�e�C�R�[��.
    /// </summary>
    /// <param name="col"> �N�������R���C�_�[. </param>
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
    /// FootSphere�g���K�[�C�O�W�b�g�R�[��.
    /// </summary>
    /// <param name="col"> �N�������R���C�_�[. </param>
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
    /// �G�̍U�����q�b�g�����Ƃ��̏���.
    /// </summary>
    /// <param name="damage"> �H������_���[�W. </param>
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
            //Debug.Log(damage + "�̃_���[�W��H�����!!�c��HP" + CurrentStatus.Hp);
        }
    }
    public void AddPlayerDamage(int dmg)
    {
        CurrentStatus.Hp -= dmg;
        Debug.Log(dmg + "�̃_���[�W��H�����!!�c��HP" + CurrentStatus.Hp);
        HPslider.value = (float)CurrentStatus.Hp / (float)DefaultStatus.Hp;

    }
    public void AddPlayerHP(int HealValue)
    {
        CurrentStatus.Hp += HealValue;
        if(CurrentStatus.Hp>DefaultStatus.Hp)
        {
            CurrentStatus.Hp = DefaultStatus.Hp;
        }
        Debug.Log("HP��"+HealValue + "�񕜂���!!�c��HP" + CurrentStatus.Hp);
        HPslider.value = (float)CurrentStatus.Hp / (float)DefaultStatus.Hp;

    }
    // ---------------------------------------------------------------------
    /// <summary>
    /// ���S������.
    /// </summary>
    // ---------------------------------------------------------------------
    void OnDie()
    {
        Debug.Log("���S���܂����B");
    }
    public Vector2 GetPlayerHP()
    {
        Vector2 HPStatus = new Vector2(CurrentStatus.Hp, DefaultStatus.Hp);
        return HPStatus;
    }
}
