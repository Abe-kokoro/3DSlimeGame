using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlyerAnimator : MonoBehaviour
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
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            AttackAction();
        }
        if (Input.GetKeyDown("space"))
        {
            if (isGround == true)
            {
                JumpAction();
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
            enemy?.OnAttackHit(CurrentStatus.Power);
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
}
