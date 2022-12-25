using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlyerAnimator : MonoBehaviour
{
    // 攻撃判定用オブジェクト.
    [SerializeField] GameObject attackHit = null;
    // アニメーター.
    Animator animator = null;
    //! 攻撃アニメーション中フラグ.
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
            // AnimationのisAttackトリガーを起動.
            animator.SetTrigger("isAttack");
            // 攻撃開始.
            isAttack = true;
        }
    }
    void AnimAtk_Hit()
    {
        Debug.Log("Hit");
        // 攻撃判定用オブジェクトを表示.
        attackHit.SetActive(true);
    }
    void AnimAtk_End()
    {
        Debug.Log("End");
        // 攻撃判定用オブジェクトを非表示に.
        attackHit.SetActive(false);
        // 攻撃終了.
        isAttack = false;
    }
}
