using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] GameObject EnemyParents; 
    [SerializeField] ColliderCallReceiver CCR = null;
    [SerializeField] string EnemyPrefabName;
    [SerializeField] int EnemyLv;
    [SerializeField] int EnemySpawnedCount = 0;
    [SerializeField] int MaxSpawn = 5;
    [SerializeField] float ReActiveTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        CCR.TriggerEnterEvent.AddListener(CollisionEnter);
    }

    // Update is called once per frame
    void Update()
    {
        if(EnemySpawnedCount >=MaxSpawn)
        {
            ReActiveTime += Time.deltaTime;
            if(ReActiveTime >= 120)
            {
                ReActiveTime = 0;
                EnemySpawnedCount = 0;
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            SpawnEnemy();
        }
    }
    void CollisionEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(EnemySpawnedCount <MaxSpawn)
            SpawnEnemy();
        }
    }
    void SpawnEnemy()
    {
        EnemySpawnedCount++;
        EnemyPrefab.transform.position = this.transform.position;
        GameObject enemy = PhotonNetwork.Instantiate(EnemyPrefabName,EnemyPrefab.transform.position,Quaternion.identity,0,null);
        enemy.GetComponent<EnemyBase>().SetEnemyLv(EnemyLv);
        //Instantiate(EnemyPrefab,EnemyParents.transform);
    }
     void DestroyEnemy()
    {
        
    }
}
