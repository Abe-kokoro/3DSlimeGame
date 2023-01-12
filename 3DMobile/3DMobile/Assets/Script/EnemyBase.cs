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
    //! �U������p�R���C�_�[�R�[��.
    [SerializeField] ColliderCallReceiver attackHitColliderCall = null;
    // �U���Ԋu.
    [SerializeField] float attackInterval = 3f;
    //�G�L�����N�^�[HP�o�[
    [SerializeField] Slider EnemyHPBar;
   
    // �A�j���[�^�[.
    Animator animator = null;
    // ----------------------------------------------------------
    /// <summary>
    /// �X�e�[�^�X.
    /// </summary>
    // ----------------------------------------------------------
    [System.Serializable]
    public class Status
    {
        // HP.
        public int Hp = 10;
        // �U����.
        public int Power = 1;
        public bool isAlive = true;
        public float DefaultSpeed = 4.0f;
        public float TraseSpeed = 8.0f;

    }

    // ��{�X�e�[�^�X.
    [SerializeField] Status DefaultStatus = new Status();
    // ���݂̃X�e�[�^�X.
    public Status CurrentStatus = new Status();
    // ���Ӄ��[�_�[�R���C�_�[�R�[��.
    [SerializeField] ColliderCallReceiver aroundColliderCall = null;
    [SerializeField] ColliderCallReceiver AtkColliderCall = null;

    // �U����ԃt���O.
    bool isBattle = false;
    bool isTrase = false;
    bool isMove = false;
    // �U�����Ԍv���p.
    float attackTimer = 0f;
    [SerializeField]float RotateTimer = 0f;
    [SerializeField]float NextRotateCoolTime = 3f;
    [SerializeField] GameObject DmgText;
    [SerializeField] Transform EnemyCanvas;
    void Start()
    {
        
        animator = GetComponent<Animator>();
        // ���ӃR���C�_�[�C�x���g�o�^.
        aroundColliderCall.TriggerEnterEvent.AddListener(OnAroundTriggerEnter);
        aroundColliderCall.TriggerStayEvent.AddListener(OnAroundTriggerStay);
        aroundColliderCall.TriggerExitEvent.AddListener(OnAroundTriggerExit);
        // �U���R���C�_�[�C�x���g�o�^.
        AtkColliderCall.TriggerEnterEvent.AddListener(OnAtkTriggerEnter);
        AtkColliderCall.TriggerStayEvent.AddListener(OnAtkTriggerStay);
        AtkColliderCall.TriggerExitEvent.AddListener(OnAtkTriggerExit);

        // �U���R���C�_�[�C�x���g�o�^.
        attackHitColliderCall.TriggerEnterEvent.AddListener(OnAttackTriggerEnter);
        // �ŏ��Ɍ��݂̃X�e�[�^�X����{�X�e�[�^�X�Ƃ��Đݒ�.
        CurrentStatus.Hp = DefaultStatus.Hp;
        CurrentStatus.Power = DefaultStatus.Power;
        attackHitColliderCall.gameObject.SetActive(false);
        //DmgText.SetActive(false);
        
    }
    void Update()
    {
        EnemyHPBar.value = (float)CurrentStatus.Hp / (float)DefaultStatus.Hp;
        // �U���ł����Ԃ̎�.
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
                    // ��]
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
    /// �U���q�b�g���R�[��.
    /// </summary>
    /// <param name="damage"> �H������_���[�W. </param>
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
            // RectTransform �̎w�肾�ƍ폜�����s����̂� gameObject ���w�肷��
            Destroy(EnemyCanvas.GetChild(index).gameObject);
        }
    }
    // ----------------------------------------------------------
    /// <summary>
    /// ���S���R�[��.
    /// </summary>
    // ----------------------------------------------------------
    void OnDie()
    {
        Debug.Log("���S");
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
    /// ���S�A�j���[�V�����I�����R�[��.
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
    /// ���Ӄ��[�_�[�R���C�_�[�G���^�[�C�x���g�R�[��.
    /// </summary>
    /// <param name="other"> �ڋ߃R���C�_�[. </param>
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
    /// ���Ӄ��[�_�[�R���C�_�[�X�e�C�C�x���g�R�[��.
    /// </summary>
    /// <param name="other"> �ڋ߃R���C�_�[. </param>
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
    /// ���Ӄ��[�_�[�R���C�_�[�I���C�x���g�R�[��.
    /// </summary>
    /// <param name="other"> �ڋ߃R���C�_�[. </param>
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
    /// ���Ӄ��[�_�[�R���C�_�[�G���^�[�C�x���g�R�[��.
    /// </summary>
    /// <param name="other"> �ڋ߃R���C�_�[. </param>
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
    /// ���Ӄ��[�_�[�R���C�_�[�X�e�C�C�x���g�R�[��.
    /// </summary>
    /// <param name="other"> �ڋ߃R���C�_�[. </param>
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
    /// ���Ӄ��[�_�[�R���C�_�[�I���C�x���g�R�[��.
    /// </summary>
    /// <param name="other"> �ڋ߃R���C�_�[. </param>
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
    /// �U���R���C�_�[�G���^�[�C�x���g�R�[��.
    /// </summary>
    /// <param name="other"> �ڋ߃R���C�_�[. </param>
    // ------------------------------------------------------------
    void OnAttackTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var player = other.GetComponent<PlyerAnimator>();
            player?.OnEnemyAttackHit(CurrentStatus.Power);
            Debug.Log("�v���C���[�ɓG�̍U�����q�b�g�I" + CurrentStatus.Power + "�̗͂ōU���I");
            attackHitColliderCall.gameObject.SetActive(false);
        }
    }
    // ----------------------------------------------------------
    /// <summary>
    /// �U��Hit�A�j���[�V�����R�[��.
    /// </summary>
    // ----------------------------------------------------------
    void Anim_AttackHit()
    {
        attackHitColliderCall.gameObject.SetActive(true);
    }

    // ----------------------------------------------------------
    /// <summary>
    /// �U���A�j���[�V�����I�����R�[��.
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
            // Transform�̒l���X�g���[���ɏ�������ő��M����
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.localRotation);
            stream.SendNext(transform.localScale);
            stream.SendNext(CurrentStatus.Hp);
            stream.SendNext(CurrentStatus.isAlive);

        }
        else
        {
            // ��M�����X�g���[����ǂݍ����Transform�̒l���X�V����
            transform.localPosition = (Vector3)stream.ReceiveNext();
            transform.localRotation = (Quaternion)stream.ReceiveNext();
            transform.localScale = (Vector3)stream.ReceiveNext();
            CurrentStatus.Hp = (int)stream.ReceiveNext();
            CurrentStatus.isAlive = (bool)stream.ReceiveNext();

        }
    }
}