using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public Transform[] enemySpawnPoints;
    public GameObject enemy;
    public float startTimeEnemySpawn;
    float timeBetweenSpawns = 5;

    private void Update(){
        if (PhotonNetwork.IsMasterClient)
        {
            if (timeBetweenSpawns <= 0){
                Vector3 spawnPosition = enemySpawnPoints[UnityEngine.Random.Range(0,enemySpawnPoints.Length)].position;
                PhotonNetwork.Instantiate(enemy.name,spawnPosition,Quaternion.identity);
                timeBetweenSpawns = startTimeEnemySpawn;
            }else{
                timeBetweenSpawns -= Time.deltaTime;
            }
        }
    }

}
