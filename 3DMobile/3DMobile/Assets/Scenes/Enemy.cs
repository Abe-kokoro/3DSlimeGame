using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    NavMeshAgent nav;
    [SerializeField] GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {


            if (nav.pathStatus != NavMeshPathStatus.PathInvalid)
            {
                nav.SetDestination(target.transform.position);
            }
        }
    }
}
