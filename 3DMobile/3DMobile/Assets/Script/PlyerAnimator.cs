using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlyerAnimator : MonoBehaviour
{
    // �U������p�I�u�W�F�N�g.
    [SerializeField] GameObject attackHit = null;
    // �A�j���[�^�[.
    Animator animator = null;
    //! �U���A�j���[�V�������t���O.
    bool isAttack = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        attackHit.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            AttackAction();
        }
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
}
