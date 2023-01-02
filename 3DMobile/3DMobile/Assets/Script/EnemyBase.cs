using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
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
    }

    // ��{�X�e�[�^�X.
    [SerializeField] Status DefaultStatus = new Status();
    // ���݂̃X�e�[�^�X.
    public Status CurrentStatus = new Status();
    // ���Ӄ��[�_�[�R���C�_�[�R�[��.
    [SerializeField] ColliderCallReceiver aroundColliderCall = null;

    void Start()
    {
        animator = GetComponent<Animator>();
        // ���ӃR���C�_�[�C�x���g�o�^.
        aroundColliderCall.TriggerStayEvent.AddListener(OnAroundTriggerStay);
        // �ŏ��Ɍ��݂̃X�e�[�^�X����{�X�e�[�^�X�Ƃ��Đݒ�.
        CurrentStatus.Hp = DefaultStatus.Hp;
        CurrentStatus.Power = DefaultStatus.Power;
    }

    // ----------------------------------------------------------
    /// <summary>
    /// �U���q�b�g���R�[��.
    /// </summary>
    /// <param name="damage"> �H������_���[�W. </param>
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
    /// ���S���R�[��.
    /// </summary>
    // ----------------------------------------------------------
    void OnDie()
    {
        Debug.Log("���S");
        animator.SetTrigger("isHit02");
    }

    // ----------------------------------------------------------
    /// <summary>
    /// ���S�A�j���[�V�����I�����R�[��.
    /// </summary>
    // ----------------------------------------------------------
    void Anim_Hit02End()
    {
        this.gameObject.SetActive(false);

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
}