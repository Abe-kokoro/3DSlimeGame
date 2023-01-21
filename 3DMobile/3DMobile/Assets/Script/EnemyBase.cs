using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Security.Cryptography;
using TMPro;
using System;
using SimpleEasing;
using Photon.Pun.Demo.PunBasics;

public class EnemyBase : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField, Range(1, 1000)]
    int EnemyLv;
    //! �U������p�R���C�_�[�R�[��.
    [SerializeField] ColliderCallReceiver attackHitColliderCall = null;
    // �U���Ԋu.
    //[SerializeField] float attackInterval = 3f;
    [SerializeField] float SpawnRange = 10f;
    //�G�L�����N�^�[HP�o�[
    [SerializeField] Slider EnemyHPBar;
    [SerializeField] GameObject TrasePlayer = null;
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
        public int Lv = 1;
        // HP.
        public int Hp = 10;
        // �U����.
        public int Power = 1;
        public bool isAlive = true;
        public float DefaultSpeed = 1.0f;
        public float TraseSpeed = 8.0f;

    }

    // ��{�X�e�[�^�X.
    [SerializeField] Status DefaultStatus = new Status();
    // ���݂̃X�e�[�^�X.
    public Status CurrentStatus = new Status();
    // ���Ӄ��[�_�[�R���C�_�[�R�[��.
    [SerializeField] ColliderCallReceiver aroundColliderCall = null;
    [SerializeField] ColliderCallReceiver AtkColliderCall = null;
    [SerializeField] ColliderCallReceiver SensorColliderCall = null;


    // �U����ԃt���O.
    bool isBattle = false;
    bool isTrase = false;
    bool isMove = false;
    bool isWalk = false;
    bool isRotate = false;
    // �U�����Ԍv���p.
    float attackTimer = 0f;
    [SerializeField]float RotateTimer = 0f;
    [SerializeField]float NextRotateCoolTime = 3f;
    [SerializeField] GameObject DmgText;
    [SerializeField] TextMeshPro EnemyLvText;
    [SerializeField] Transform EnemyCanvas;
    void Start()
    {
        DefaultStatus.Lv = EnemyLv;
        CurrentStatus.Lv = EnemyLv;
        animator = GetComponent<Animator>();
        // ���ӃR���C�_�[�C�x���g�o�^.
        aroundColliderCall.TriggerEnterEvent.AddListener(OnAroundTriggerEnter);
        aroundColliderCall.TriggerStayEvent.AddListener(OnAroundTriggerStay);
        aroundColliderCall.TriggerExitEvent.AddListener(OnAroundTriggerExit);
        // �U���R���C�_�[�C�x���g�o�^.
        AtkColliderCall.TriggerEnterEvent.AddListener(OnAtkTriggerEnter);
        AtkColliderCall.TriggerStayEvent.AddListener(OnAtkTriggerStay);
        AtkColliderCall.TriggerExitEvent.AddListener(OnAtkTriggerExit);

        SensorColliderCall.TriggerStayEvent.AddListener(OnSensorTriggerEnter);

        // �U���R���C�_�[�C�x���g�o�^.
        attackHitColliderCall.TriggerEnterEvent.AddListener(OnAttackTriggerEnter);
        DefaultStatus.Hp = 90 + EnemyLv * 12;
        DefaultStatus.Power = 20+EnemyLv * 7;
        // �ŏ��Ɍ��݂̃X�e�[�^�X����{�X�e�[�^�X�Ƃ��Đݒ�.
        CurrentStatus.Hp = DefaultStatus.Hp;
        CurrentStatus.Power = DefaultStatus.Power;
        CurrentStatus.Lv = DefaultStatus.Lv;
        attackHitColliderCall.gameObject.SetActive(false);
        //DmgText.SetActive(false);
        NextRotateCoolTime = UnityEngine.Random.Range(8, 16);
        TrasePlayer = null;
    }
    void Update()
    {
        EnemyLv = CurrentStatus.Lv;
        EnemyLvText.text = "LV." + EnemyLv;
        EnemyHPBar.value = (float)CurrentStatus.Hp / (float)DefaultStatus.Hp;
        // �U���ł����Ԃ̎�.
        if (isBattle == true)
        {
            animator.SetBool("isMove", false);
            animator.SetBool("isWalk", false);
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
                animator.SetBool("isWalk", false);
                if (isMove)
                this.transform.position += this.transform.forward * CurrentStatus.TraseSpeed * Time.deltaTime;
            }
            else
            {
                //this.transform.position += this.transform.forward * CurrentStatus.DefaultSpeed * Time.deltaTime;
                if (isRotate)
                {
                    animator.SetBool("isWalk", false);
                }
                else
                {
                    animator.SetBool("isWalk", true);
                }
                RotateTimer += Time.deltaTime;
                animator.SetBool("isMove", false);
                if (RotateTimer >= NextRotateCoolTime)
                {
                    isRotate = true;
                    Invoke("SetisRotate",2.5f);
                    // ��]
                    this.transform.RotateTo(new Vector3(0, UnityEngine.Random.Range(0, 360)-180, 0), 2.5f, EasingTypes.BackIn);
                    //this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y + UnityEngine.Random.Range(30, 270), 0);
                    
                    
                }
                else
                {
                    if (isWalk)
                        this.transform.position += this.transform.forward * CurrentStatus.TraseSpeed*0.7f * Time.deltaTime;
                }
            }
        }
       
        if(!CurrentStatus.isAlive)
        {
            
            DefaultStatus.Lv = CurrentStatus.Lv + UnityEngine.Random.Range(0,2);
            CurrentStatus.Lv = DefaultStatus.Lv;
            
            CurrentStatus.isAlive = true;
            DefaultStatus.Hp = 90 + CurrentStatus.Lv * 12;
            DefaultStatus.Power = 10 + CurrentStatus.Lv * 3;
            // �ŏ��Ɍ��݂̃X�e�[�^�X����{�X�e�[�^�X�Ƃ��Đݒ�.
            CurrentStatus.Hp = DefaultStatus.Hp;
            CurrentStatus.Power = DefaultStatus.Power;
            

            transform.localPosition = new Vector3(UnityEngine.Random.Range(50,100)*SpawnRange, transform.localPosition.y+5, UnityEngine.Random.Range(50, 100) * SpawnRange);
        }
    }
    void SetisRotate()
    {
        if (!isTrase)
        {
            isRotate = false;
            NextRotateCoolTime = UnityEngine.Random.Range(8, 16);
            RotateTimer = 0;
        }
    }
    void OnSensorTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Field")
        {
            isRotate = false;
            this.transform.eulerAngles += new Vector3(0, 1, 0);
        }

    }
    // ----------------------------------------------------------
    /// <summary>
    /// �U���q�b�g���R�[��.
    /// </summary>
    /// <param name="damage"> �H������_���[�W. </param>
    // ----------------------------------------------------------
    public void OnAttackHit(int damage,int dmgLevel,bool isMine,GameObject PlayerStatus)
    {
        attackTimer = 0f;

            CurrentStatus.Hp -= damage;
            
        Debug.Log("Hit Damage " + damage + "/CurrentHp = " + CurrentStatus.Hp);
        DmgText.GetComponent<TextMeshProUGUI>().text = ""+damage;
        DmgText.transform.localPosition = new Vector3(UnityEngine.Random.Range(-100,100),10,0);
        Instantiate(DmgText, EnemyCanvas);
        //Invoke("ClearDmg(DmgText)", 1);
        if (CurrentStatus.Hp <= 0)
        {
            if(isMine)
            {
                PlayerStatus.gameObject.GetComponent<PlyerAnimator>().AddKillcount();
            }
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
    void SlimeWalkStart()
    {
        isWalk = true;
    }
    void SlimeWalkEnd()
    {
        isWalk = false;
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
        if (TrasePlayer==null)
        { 
            if (other.gameObject.tag == "Player")
            {
                TrasePlayer = other.gameObject;
                isTrase = true;
            }
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
            if (other.gameObject == TrasePlayer)
            {
                var _dir = (other.gameObject.transform.position - this.transform.position).normalized;
                _dir.y = 0;
                this.transform.forward = _dir;
            }
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
            if (TrasePlayer == other.gameObject)
            {
                TrasePlayer = null;
                isTrase = false;
            }
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
            if (other.GetComponent<Player2>().photonView.IsMine)
            {
                var player = other.GetComponent<PlyerAnimator>();
                player?.OnEnemyAttackHit(CurrentStatus.Power);
                Debug.Log("�v���C���[�ɓG�̍U�����q�b�g�I" + CurrentStatus.Power + "�̗͂ōU���I");
                attackHitColliderCall.gameObject.SetActive(false);
            }
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
            stream.SendNext(CurrentStatus.Lv);
            stream.SendNext(CurrentStatus.Power);
            stream.SendNext(DefaultStatus.Hp);
            stream.SendNext(DefaultStatus.isAlive);
            stream.SendNext(DefaultStatus.Lv);
            stream.SendNext(DefaultStatus.Power);
            //stream.SendNext(TrasePlayer);
        }
        else
        {
            // ��M�����X�g���[����ǂݍ����Transform�̒l���X�V����
            transform.localPosition = (Vector3)stream.ReceiveNext();
            transform.localRotation = (Quaternion)stream.ReceiveNext();
            transform.localScale = (Vector3)stream.ReceiveNext();
            CurrentStatus.Hp = (int)stream.ReceiveNext();
            CurrentStatus.isAlive = (bool)stream.ReceiveNext();
            CurrentStatus.Lv = (int)stream.ReceiveNext();
            CurrentStatus.Power = (int)stream.ReceiveNext();
            DefaultStatus.Hp = (int)stream.ReceiveNext();
            DefaultStatus.isAlive = (bool)stream.ReceiveNext();
            DefaultStatus.Lv = (int)stream.ReceiveNext();
            DefaultStatus.Power = (int)stream.ReceiveNext();
            //TrasePlayer = (GameObject)stream.ReceiveNext();
        }
    }
}