using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject doors;
    public bool activateEnemy = false;
    private int enemyCount;
    public GameObject[] Enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemyCount = Enemy.Length;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && enemyCount != 0) 
        {
            doors.SetActive(true);
            activateEnemy = true;
            MakeEnemyActive();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    { 
        if(enemyCount == 0)
        {
            doors.SetActive(false);
        }
        
    }
    public void Dead()
    {
        enemyCount--;

    }
    public void MakeEnemyActive()
    {
        foreach (GameObject go in Enemy)
        {
            go.SetActive(true);
        }
    }
}
