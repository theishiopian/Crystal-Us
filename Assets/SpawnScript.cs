using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnScript : MonoBehaviour
{
    [SerializeField]
        float leftUnits = 0, rightUnits = 0, upUnits = 0, downUnits = 0;
    [SerializeField]
        GameObject enemy = null;
    private float distanceToPlayer;

    

    void Start()
    {
        GameObject spawnedEnemy = Instantiate(enemy, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector2.Distance(GameObject.Find("Player").transform.position, transform.position);
        Vector2 position = new Vector2(Random.Range(leftUnits * -1, rightUnits), Random.Range(downUnits * -1, upUnits));
        if(transform.childCount == 0 && distanceToPlayer > 5)
        {
            GameObject spawnedEnemy = Instantiate(enemy, gameObject.transform);
        }
    }
}
