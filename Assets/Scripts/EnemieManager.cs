using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    /// <summary>
    /// Prefab that will be instantiate
    /// </summary>
    public GameObject[] enemyPrefab;
    /// <summary>
    /// The current number of enemies
    /// </summary>
    public int currentEnemies;
    /// <summary>
    /// The max enemies that can be on play
    /// </summary>
    public int maxEnemies;
    /// <summary>
    /// 
    /// </summary>
    public const int numSpawnsPerType = 4;
    /// <summary>
    /// ghuj
    /// </summary>
    public int dmg;



    private void Spawn()
    {
        /*int enemyType;
        if (currentEnemies < maxEnemies)
        {
            enemyType = Random.Range(0, enemyPrefab.Length);
            //position del instantiate
            Random.Range(enemyType * numSpawnsPerType, enemyType * (numSpawnsPerType + 1));

            if (checkear la pool)
            {
                //moverlo a su sitio

            }
            else
            {
                //Instantiate

            }

        }*/
    }
}
