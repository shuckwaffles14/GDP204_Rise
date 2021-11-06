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
        [Tooltip("Keep checkpoints at 2 for the moment -- It should be able to handle more but 2 definitely works")]
        public GameObject[] checkpoints;
        [Tooltip("1 = E1, 2 = E2, 3 = E3")]
        public int enemyType;
    }

    public GameObject playerPrefab;
    public Transform playerSpawn;
    public Enemy[] enemies; // can use this for later updates when spawning more enemies of different types

    private void Awake()
    {
        //spawn player + items + enemies
        Instantiate(playerPrefab, playerSpawn);

        //uncomment when changing spawn system
        foreach (Enemy enemy in enemies)
        {
            GameObject clone = Instantiate(enemy.enemyPrefab, enemy.spawnPoint);
            clone.GetComponent<AIController>().Setup(enemy.checkpoints, enemy.enemyType);
        }
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
