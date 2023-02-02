using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class WeaponManager : MonoBehaviour
{
    [SerializeField] GameObject RHand;
    [SerializeField] GameObject LHand;
    [SerializeField] GameObject Sword;
    [SerializeField] GameObject Back;
    PlyerAnimator Animator;
    bool GenerateFlg;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in RHand.transform)
        {
            //çÌèúÇ∑ÇÈ
            Destroy(child.gameObject);
        }
        Instantiate(Sword, RHand.transform);

        foreach (Transform child in LHand.transform)
        {
            //çÌèúÇ∑ÇÈ
            Destroy(child.gameObject);
        }
        GameObject obj = (GameObject)Resources.Load("Sword_02");
        //Instantiate(obj, LHand.transform.position,LHand.transform.rotation,LHand.transform);
        Animator = this.GetComponent<PlyerAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Animator.isGround)
        {
            
            if(Animator.isRelax)
            {
                RHand.SetActive(false);
                LHand.SetActive(false);
                Back.SetActive(true);
            }
            else
            {
                RHand.SetActive(true);
                LHand.SetActive(true);
                Back.SetActive(false);
                if(this.gameObject.GetComponent<Player2>().DashFlg&& this.gameObject.GetComponent<Player2>().PlayerMoveFlg)
                {
                    RHand.SetActive(false);
                    LHand.SetActive(false);
                    Back.SetActive(true);
                }
            }
        }
        else
        {
            if (Animator.isAttack2)
            {
                RHand.SetActive(true);
                LHand.SetActive(true);
            }
            else
            {
                RHand.SetActive(false);
                LHand.SetActive(false);
            }

        }
        
    }
}
