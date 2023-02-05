using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Photon.Pun;
using System.Diagnostics;
using System;
using TMPro;
using Unity.VisualScripting;
using Photon.Pun.Demo.Cockpit;
using System.IO;
using UnityEditor;
public class PlyerAnimator : MonoBehaviourPunCallbacks, IPunObservable
{
    [System.Serializable]
    public class PlayerSaveData
    {
        public string PlayerName;
        public int Lv;
        public int CurrentHP;
        public int DefaultHP;
        public int Atk;
        public Vector3 Pos;
    }


    [SerializeField] private string PlayerName;
    [SerializeField, Range(1, 100)]
    int PlayerLv;
    public enum AttackElement
    {
        ELEMENT_NORMAL,
        ELEMENT_FIRE,
        ELEMENT_LEAF,
        ELEMENT_WATER,
        MAX
    }
    // -------------------------------------------------------
    /// <summary>
    /// �X�e�[�^�X.
    /// </summary>
    // -------------------------------------------------------
    [System.Serializable]
    public class Status
    {
        public int Lv = 1;
        // �̗�.
        public int Hp = 10;
        // �U����.
        public int Power = 1;
        public int Element = 0;
    }
    [System.Serializable]
    public class SlashEffect
    {
        public ParticleSystem Sword1particle;
        public ParticleSystem Sword2particle;
        public ParticleSystem Sword3particle;
        public ParticleSystem Sword4particle;
    }
    [SerializeField] GameObject PlayerModel;
    //HPBar slider
    public Slider HPslider;
    public TextMeshProUGUI LvText;
    [SerializeField]
    [Tooltip("effect")]
    private ParticleSystem Attack1particle;
    [SerializeField] SlashEffect NormalEffect = new SlashEffect();
    [SerializeField] SlashEffect FireEffect = new SlashEffect();
    [SerializeField] SlashEffect LeafEffect = new SlashEffect();
    [SerializeField] SlashEffect WaterEffect = new SlashEffect();

   


    [SerializeField] private ParticleSystem Finalparticle;
    
    // �U��Hit�I�u�W�F�N�g��ColliderCall.
    [SerializeField] ColliderCallReceiver attackHitCall = null;
    // ��{�X�e�[�^�X.
    [SerializeField] public Status DefaultStatus = new Status();
    [SerializeField]public int LvUpCount = 0;
    [SerializeField]public int EnemyKillCount = 0;
    // ���݂̃X�e�[�^�X.
    public Status CurrentStatus = new Status();

    // �U������p�I�u�W�F�N�g
    [SerializeField] GameObject attackHit = null;
    // �A�j���[�^�[
    Animator animator = null;
    //�W�����v��
    [SerializeField] float jumpPower = 60f;
    // ���W�b�h�{�f�B
    Rigidbody rigid = null;
    //! �U���A�j���[�V�������t���O.
    public bool isAttack = false;
    public bool isAttack2 = false;
    public bool isAttacking = false;
    public bool isAttackingMBUP = false;
    public bool isAttackChain = false;
    public bool isFinalAtk = false;
    public bool JumpFlg = false;
    public bool isHit = false;
    [SerializeField] float NockBackRate = 50.0f;
   
    public bool isDead = false;
    public bool isRespwan = false;
    // �ݒu����pColliderCall.
    [SerializeField] ColliderCallReceiver footColliderCall = null;
    // �ڒn�t���O.
    public bool isGround = false;
    [SerializeField] float RelaxCount = 0f;
    [SerializeField] float RelaxTime = 5f;
    public bool isRelax = false;
    public bool isFight = true;
    public bool isWeaponSwitching = false;
    // Start is called before the first frame update
    void Start()
    {
        LvText.text = "Lv." + CurrentStatus.Lv;
        PlayerName = photonView.Owner.NickName;
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        attackHit.SetActive(false);
        // FootSphere�̃C�x���g�o�^.
        
        footColliderCall.TriggerStayEvent.AddListener(OnFootTriggerStay);
        footColliderCall.TriggerExitEvent.AddListener(OnFootTriggerExit);
        // �U������p�R���C�_�[�C�x���g�o�^.
        attackHitCall.TriggerEnterEvent.AddListener(OnAttackHitTriggerEnter);
        DefaultStatus.Lv = 1;
        
        DefaultStatus.Power = 10;
        if (!TitleManager.LoadDataflg)
        {
            // ���݂̃X�e�[�^�X�̏�����.
            DefaultStatus.Hp = 1000;
            CurrentStatus.Lv = DefaultStatus.Lv;
            CurrentStatus.Hp = DefaultStatus.Hp;
            CurrentStatus.Power = DefaultStatus.Power;
            

        }
        HPslider.value = 1;
        LvUpCount = 3 + CurrentStatus.Lv * 2;
        if(photonView.IsMine)
        {
            //PlayerModel.AddComponent<AudioListener>();
            //Camera.main.GetComponent<AudioListener>().enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        DefaultStatus.Hp = 975+CurrentStatus.Lv * 25;
        CurrentStatus.Power = 10+CurrentStatus.Lv * 3;
        
        LvUpCount = 3 + CurrentStatus.Lv * 2;
        if (photonView.IsMine&&!GameController.Loaded)
        {
            
             if (TitleManager.LoadDataflg)
             {
                 PlayerSaveData PlayerLoadData = loadPlayerData();
                 CurrentStatus.Lv = PlayerLoadData.Lv;
                 CurrentStatus.Hp = PlayerLoadData.CurrentHP;
                 DefaultStatus.Hp = PlayerLoadData.DefaultHP;
                 CurrentStatus.Power = PlayerLoadData.Atk;
                 this.transform.position = PlayerLoadData.Pos;
                 GameController.Loaded = true;
             }
        }
        
        HPslider.value =(float) CurrentStatus.Hp / (float)DefaultStatus.Hp;
        LvText.text = "Lv." + CurrentStatus.Lv;
        if (photonView.IsMine&&!Menu.isMenu&& GameController.isPC)
        //if (isPC)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                ButtonClicked();
                //AttackAction();
                //AttackStart();
                //isAttackingMBUP = false;
            }
            if(Input.GetMouseButtonUp(0))
            {
                ButtonClickedUp();
            }
            if (Input.GetMouseButtonDown(1))
            {
                //AttackAction2();
            }

            if (Input.GetKeyDown("space")||JumpFlg)
            {
                if (isGround == true)
                {
                    JumpAction();
                    JumpFlg = false;
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
        if(EnemyKillCount>=LvUpCount)
        {
            EnemyKillCount = 0;
            LvUpCount = CurrentStatus.Lv * 3;
            PlayerLvUp(1);
        }
        if (photonView.IsMine)
        {
            if (isFight)
                RelaxCount += Time.deltaTime;
            if (RelaxCount > RelaxTime)
            {
                RelaxCount = 0.0f;
                animator.SetBool("isRelax", true);
                isWeaponSwitching = true;
                isFight = false;
            }
            if (this.gameObject.GetComponent<Player2>().PlayerMoveFlg)
            {
                RelaxCount = 0.0f;
            }
            if (Input.GetKeyDown("u"))
            {
                PlayerLvUp(1);
            }
            if (Input.GetKey("p"))
            {
                PlayerLvUp(10);
            }

            if (CurrentStatus.Hp <= 0)
            {
                CurrentStatus.Hp = 0;
                if (!isDead)
                {
                    isDead = true;
                

                
                    animator.SetBool("isDead", true);
                
                }
            }
            else
            {
                //isDead = false;
                //animator.SetBool("isDead", false);
            }
            if(isRespwan)
            {
                isDead = false;
                animator.SetBool("isDead", false);
                isAttack2 = false;
                isAttack = false;
                isAttackChain = false;
                isAttacking = false;
                isFight = false;
                isWeaponSwitching = false;
                isRelax = true;
                RelaxCount = 0.0f;
                CurrentStatus.Hp = (int)((float)DefaultStatus.Hp * 0.8f);
                if (CurrentStatus.Lv > 4)
                {

                    PlayerLvDown((int)((float)CurrentStatus.Lv*0.3f));
                }
                this.transform.position = new Vector3(PlayerController.resPos.x, PlayerController.resPos.y, PlayerController.resPos.z);
                isRespwan = false;
            }
            if(Input.GetKeyDown("f"))
            {
                SetPlayerElement(AttackElement.ELEMENT_FIRE);
            }
            if (Input.GetKeyDown("g"))
            {
                SetPlayerElement(AttackElement.ELEMENT_LEAF);
            }
            if (Input.GetKeyDown("h"))
            {
                SetPlayerElement(AttackElement.ELEMENT_WATER);
            }
            if(Input.GetKeyDown("i"))
            {
                SavePlayerData();
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
            bool isMine = false;
            if (photonView.IsMine)
            {
                isMine = true;
            }
                var enemy = col.gameObject.GetComponent<EnemyBase>();
                if (isAttack)
                {


                    enemy?.OnAttackHit(CurrentStatus.Power, 1,isMine,this.gameObject);
                }
                else if (isAttack2)
                {
                    enemy?.OnAttackHit((int)(CurrentStatus.Power * 3f), 2, isMine, this.gameObject);

                }
                else if (isFinalAtk)
                {
                    enemy?.OnAttackHit(CurrentStatus.Power * 2, 2, isMine, this.gameObject);

                }
                else
                {
                    enemy?.OnAttackHit(CurrentStatus.Power, 1, isMine, this.gameObject);

                }

                //attackHit.SetActive(false);
            
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
        UnityEngine.Debug.Log("Hit");
        // �U������p�I�u�W�F�N�g��\��.
        attackHit.SetActive(true);
    }
    void Atk_Hit()
    {
        UnityEngine.Debug.Log("Hit");
        // �U������p�I�u�W�F�N�g��\��.
        attackHit.SetActive(true);
    }
    void Hit_End()
    {
        UnityEngine.Debug.Log("HitEnd");
        // �U������p�I�u�W�F�N�g��\��.
        attackHit.SetActive(false);
    }
    void AnimAtk_End()
    {
        UnityEngine.Debug.Log("End");
        // �U������p�I�u�W�F�N�g���\����.
        attackHit.SetActive(false);
        // �U���I��.
        isAttack = false;
        
    }

    void Attack2_Start()
    {
        UnityEngine.Debug.Log("Hit");
        // �U������p�I�u�W�F�N�g��\��.
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
        // �U������p�I�u�W�F�N�g���\����.
        attackHit.SetActive(false);
        // �U���I��.
        isAttack2 = false;
    }
    void AttackStart()
    {
        if (isAttacking == false)
        {
            // Animation��isAttack�g���K�[���N��.
            animator.SetBool("isAttacking",true);
            // �U���J�n.
            isAttacking = true;
            isFinalAtk = false;
        }
    }
    void AttackS6_Start()
    {
        ParticleSystem newParticle = null;
        switch ((AttackElement)CurrentStatus.Element)
        {
            case AttackElement.ELEMENT_NORMAL:
                newParticle = Instantiate(NormalEffect.Sword1particle);
                break;
            case AttackElement.ELEMENT_FIRE:
                newParticle = Instantiate(FireEffect.Sword1particle);
                break;
            case AttackElement.ELEMENT_LEAF:
                newParticle = Instantiate(LeafEffect.Sword1particle);
                break;
            case AttackElement.ELEMENT_WATER:
                newParticle = Instantiate(WaterEffect.Sword1particle);
                break;

        }

        
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
        // Animation��isAttack�g���K�[���N��.
        animator.SetBool("isAttacking", false);
        // �U���J�n.
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
        ParticleSystem newParticle = null;
        switch ((AttackElement)CurrentStatus.Element)
        {
            case AttackElement.ELEMENT_NORMAL:
                newParticle = Instantiate(NormalEffect.Sword2particle);
                break;
            case AttackElement.ELEMENT_FIRE:
                newParticle = Instantiate(FireEffect.Sword2particle);
                break;
            case AttackElement.ELEMENT_LEAF:
                newParticle = Instantiate(LeafEffect.Sword2particle);
                break;
            case AttackElement.ELEMENT_WATER:
                newParticle = Instantiate(WaterEffect.Sword2particle);
                break;

        }

        newParticle.transform.position = this.transform.position + newParticle.transform.position;
        Vector3 LocalAngles = newParticle.transform.localEulerAngles + this.transform.localEulerAngles;
        newParticle.transform.localEulerAngles = LocalAngles;
        newParticle.Play();
        Destroy(newParticle.gameObject, 1.0f);
    }
    void AttackS2_End()
    {
        
            UnityEngine.Debug.Log("End");
            // Animation��isAttack�g���K�[���N��.
            animator.SetBool("isAttacking", false);
            // �U���J�n.
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
        ParticleSystem newParticle = null;
        switch ((AttackElement)CurrentStatus.Element)
        {
            case AttackElement.ELEMENT_NORMAL:
                newParticle = Instantiate(NormalEffect.Sword3particle);
                break;
            case AttackElement.ELEMENT_FIRE:
                newParticle = Instantiate(FireEffect.Sword3particle);
                break;
            case AttackElement.ELEMENT_LEAF:
                newParticle = Instantiate(LeafEffect.Sword3particle);
                break;
            case AttackElement.ELEMENT_WATER:
                newParticle = Instantiate(WaterEffect.Sword3particle);
                break;

        }

        newParticle.transform.position = this.transform.position + newParticle.transform.position;
        Vector3 LocalAngles = newParticle.transform.localEulerAngles + this.transform.localEulerAngles;
        newParticle.transform.localEulerAngles = LocalAngles;

        newParticle.Play();
        Destroy(newParticle.gameObject, 1.0f);
    }
    void AttackS5_End()
    {
        
            UnityEngine.Debug.Log("End");
            // Animation��isAttack�g���K�[���N��.
            animator.SetBool("isAttacking", false);
            // �U���J�n.
            isAttacking = false;

    }
    void AttackWS2_Start()
    {
        isFinalAtk = true;
        attackHit.SetActive(false);
    }
    void AttackWS2_Effect()
    {
        ParticleSystem newParticle = null;
        switch ((AttackElement)CurrentStatus.Element)
        {
            case AttackElement.ELEMENT_NORMAL:
                newParticle = Instantiate(NormalEffect.Sword4particle);
                break;
            case AttackElement.ELEMENT_FIRE:
                newParticle = Instantiate(FireEffect.Sword4particle);
                break;
            case AttackElement.ELEMENT_LEAF:
                newParticle = Instantiate(LeafEffect.Sword4particle);
                break;
            case AttackElement.ELEMENT_WATER:
                newParticle = Instantiate(WaterEffect.Sword4particle);
                break;

        }

        newParticle.transform.position = this.transform.position + newParticle.transform.position;
        Vector3 LocalAngles = newParticle.transform.localEulerAngles + this.transform.localEulerAngles;
        newParticle.transform.localEulerAngles = LocalAngles;
        attackHit.SetActive(true);
        newParticle.Play();
        Destroy(newParticle.gameObject, 1.0f);
    }   
    void AttackWS2_End()
    {
        UnityEngine.Debug.Log("End");
        // Animation��isAttack�g���K�[���N��.
        animator.SetBool("isAttacking", false);
        animator.SetBool("isAttackChain", false);
        // �U���J�n.
        
        isAttacking = false;
        isAttackChain = false;
        isFinalAtk = false;
        animator.SetTrigger("isFinalAtk");
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
            OnHit();
            //UnityEngine.Debug.Log(damage + "�̃_���[�W��H�����!!�c��HP" + CurrentStatus.Hp);
        }
    }
    public void AddPlayerDamage(int dmg)
    {
        if (this.photonView.IsMine)
        {
            CurrentStatus.Hp -= dmg;
            UnityEngine.Debug.Log(dmg + "�̃_���[�W��H�����!!�c��HP" + CurrentStatus.Hp);
            HPslider.value = (float)CurrentStatus.Hp / (float)DefaultStatus.Hp;
        }
    }
    public void AddPlayerHP(int HealValue)
    {
        if (this.photonView.IsMine)
        {
            CurrentStatus.Hp += HealValue;
            if (CurrentStatus.Hp > DefaultStatus.Hp)
            {
                CurrentStatus.Hp = DefaultStatus.Hp;
            }
            UnityEngine.Debug.Log("HP��" + HealValue + "�񕜂���!!�c��HP" + CurrentStatus.Hp);
            HPslider.value = (float)CurrentStatus.Hp / (float)DefaultStatus.Hp;
        }
    }

    void OnHit()
    {
        if(UnityEngine.Random.Range(0.0f,100.0f)<NockBackRate)
        {
            animator.SetBool("isHit", true);
            isHit = true;
        }
    }
    void OnHitEnd()
    {
        animator.SetBool("isHit", false);
        isHit = false;
    }
    // ---------------------------------------------------------------------
    /// <summary>
    /// ���S������.
    /// </summary>
    // ---------------------------------------------------------------------
    void OnDie()
    {
        UnityEngine.Debug.Log("���S���܂����B");
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
    void SwordToRelaxEnd()
    {
        isWeaponSwitching = false;
        isRelax = true;
    }
    void RelaxTorSword()
    {
        isRelax = false;
    }
    void RelaxToSwordEnd()
    {
        isWeaponSwitching = false;
    }
    public void ButtonClicked()
    {
        isFight = true;
        RelaxCount = 0.0f;
        if (isRelax)
        {
            isWeaponSwitching = true;
            //isRelax = false;
            
            animator.SetBool("isRelax", false);
            
        }
        else
        {
            if (isGround)
            {
                //isAttacking = true;
                AttackStart();
                //isAttackingMBUP = false;
                if (isAttackingMBUP)
                {
                    isAttackChain = true;
                    animator.SetBool("isAttackChain", true);
                    isAttackingMBUP = false;
                }
            }
            else
            {
                AttackAction2();
            }
        }
       
    }
    public void ButtonClickedUp()
    {
        if (isAttacking)
            isAttackingMBUP = true;
       
    }
    public void JumpClicked()
    {
        if (isGround == true)
        {
            JumpAction();
            JumpFlg = false;
        }
    }

    public void PlayerLvUp(int UpValue)
    {
        DefaultStatus.Hp = 1000 + CurrentStatus.Lv * 25;
        CurrentStatus.Power = 10 + CurrentStatus.Lv * 3;
        CurrentStatus.Hp = DefaultStatus.Hp;
        //PlayerLv += UpValue;
        //DefaultStatus.Power += UpValue * 3;
        //DefaultStatus.Hp += UpValue *25;
        //CurrentStatus.Power = DefaultStatus.Power;
        //CurrentStatus.Hp = DefaultStatus.Hp;
        CurrentStatus.Lv += UpValue;
    }
    public void PlayerLvDown(int DownValue)
    {
        //PlayerLv -= DownValue;
        //DefaultStatus.Power -= DownValue * 3;
        //DefaultStatus.Hp -= DownValue * 25;
        //CurrentStatus.Power = DefaultStatus.Power;
        //CurrentStatus.Hp = DefaultStatus.Hp;
        CurrentStatus.Lv -= PlayerLv;
    }
    public int GetPlayerLevel()
    {
        return PlayerLv;
    }
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Transform�̒l���X�g���[���ɏ�������ő��M����
            
            stream.SendNext(CurrentStatus.Hp);
            stream.SendNext(CurrentStatus.Power);
            stream.SendNext(CurrentStatus.Lv);
            stream.SendNext(CurrentStatus.Element);
            stream.SendNext(DefaultStatus.Hp);
            stream.SendNext(DefaultStatus.Power);
            stream.SendNext(DefaultStatus.Lv);
        }
        else
        {
            // ��M�����X�g���[����ǂݍ����Transform�̒l���X�V����
            

            CurrentStatus.Hp = (int)stream.ReceiveNext();
            CurrentStatus.Power = (int)stream.ReceiveNext();
            CurrentStatus.Lv = (int)stream.ReceiveNext();
            CurrentStatus.Element = (int)stream.ReceiveNext();
            DefaultStatus.Hp = (int)stream.ReceiveNext();
            DefaultStatus.Power = (int)stream.ReceiveNext();
            DefaultStatus.Lv = (int)stream.ReceiveNext();
        }
    }
    public void AddKillcount()
    {
        EnemyKillCount++;
    }
    public int GetKillCount()
    {
        return EnemyKillCount;
    }

    public int GetLvUpCount()
    {
        return LvUpCount;
    }
    public void RespawnPlayer()
    {
        isRespwan = true;

    }
    public int GetPlayerElement()
    {
        return CurrentStatus.Element;
    }
    public void SetPlayerElement(AttackElement element)
    {
        CurrentStatus.Element = (int)element;
    }
    public void SavePlayerData()
    {
        if(photonView.IsMine)
        {
            PlayerSaveData playerData = new PlayerSaveData();
            playerData.PlayerName = PlayerName;
            playerData.Lv = CurrentStatus.Lv;
            playerData.CurrentHP = CurrentStatus.Hp;
            playerData.DefaultHP = DefaultStatus.Hp;
            playerData.Pos = this.transform.position;
            playerData.Atk = CurrentStatus.Power;

            string jsonstr = JsonUtility.ToJson(playerData);

            StreamWriter writer;

            SafeCreateDirectory("/");
            writer = new StreamWriter(Application.persistentDataPath + "/savedata.json", false);
            writer.Write(jsonstr);
            writer.Flush();
            writer.Close();

        }

        //PlayerSaveData player2 = JsonUtility.FromJson<PlayerSaveData>(jsonstr);

    }

    public PlayerSaveData loadPlayerData()
    {
        string datastr = "";
        StreamReader reader;
        reader = new StreamReader(Application.persistentDataPath + "/savedata.json");
        datastr = reader.ReadToEnd();
        reader.Close();

        return JsonUtility.FromJson<PlayerSaveData>(datastr);
    }
    public static DirectoryInfo SafeCreateDirectory(string path)
    {
        //�f�B���N�g�������݂��Ă��邩�̊m�F �Ȃ���ΐ���
        if (Directory.Exists(path))
        {
            return null;
        }
        return Directory.CreateDirectory(path);
    }
}
