using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform playerSpawn;
    //public GameObject[] enemyPrefabs;

    private void Awake()
    {
        //spawn player + items + enemies
        Instantiate(playerPrefab, playerSpawn);
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
