using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWall : MonoBehaviour
{
    public GameObject boss = null;
    public Vector3 bossPosition = Vector3.zero;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player == null)
                return;
            transform.GetChild(0).gameObject.SetActive(true);
            Instantiate(boss);
            boss.transform.position = bossPosition;
            triggered = true;
        }
    }
}