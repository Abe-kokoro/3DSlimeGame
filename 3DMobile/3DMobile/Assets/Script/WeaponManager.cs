using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class WeaponManager : MonoBehaviour
{
    [SerializeField] GameObject RHand;
    [SerializeField] GameObject LHand;
    [SerializeField] GameObject Sword;
    bool GenerateFlg;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in RHand.transform)
        {
            //�폜����
            Destroy(child.gameObject);
        }
        Instantiate(Sword, RHand.transform);

        foreach (Transform child in LHand.transform)
        {
            //�폜����
            Destroy(child.gameObject);
        }
        GameObject obj = (GameObject)Resources.Load("Sword_02");
        Instantiate(obj, LHand.transform);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
