using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject zombiePrefab;
    public Transform enemyParent;
    public Transform player;

    int spawnNumber = 5;
//    int spawnPosition = 100;


    public int spawningDistance = 35;




    private void Start()
    {

        //SPAWN 5 ZOMBIES

        SpawnFunction(5, -45);
    }






    public void SpawnFunction(int number, int spawnPos)
    {
        for (int i = 0; i < number; i++)
        {
            Vector2 spawningPos = new Vector2(Random.Range(spawnPos - 50, spawnPos + 1), 0f);
            Quaternion spawningRot = zombiePrefab.transform.rotation;
            GameObject tmp = Instantiate(zombiePrefab, spawningPos, spawningRot, enemyParent);
            tmp.GetComponent<Enemy>().player = player.gameObject;
            tmp.GetComponent<Enemy>().spawner = gameObject.GetComponent<EnemySpawner>();
        }
    }


    public void IncreaseSpawnNumber(int value)
    {
        spawnNumber += value;
    }

    public int GetSpawnNumber()
    {
        return spawnNumber;
    }

}
