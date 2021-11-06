using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManagerScript : MonoBehaviour
{
    [System.Serializable]
    public class Enemy
    {
        public GameObject enemyPrefab;
        public Transform spawnPoint;
        public GameObject[] checkpoints;
    }

    public GameObject playerPrefab, enemy1Prefab;
    public Transform playerSpawn, enemySpawn;
    public GameObject[] checkpoints;
    //public GameObject[] enemyPrefabs;
    public Enemy[] enemies; // can use this for later updates when spawning more enemies of different types

    private void Awake()
    {
        //spawn player + items + enemies
        Instantiate(playerPrefab, playerSpawn);
        GameObject clone1 = Instantiate(enemy1Prefab, enemySpawn);
        clone1.GetComponent<AIController>().Setup(checkpoints, 2);

        // uncomment when changing spawn system
        //foreach (Enemy enemy in enemies)
        //{
        //    GameObject clone = Instantiate(enemy.enemyPrefab, enemy.spawnPoint);
        //    clone.GetComponent<AIController>().Setup(enemy.checkpoints);
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
