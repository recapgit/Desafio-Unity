using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IA simples usando MoveTowards que apenas faz com que o inimigo siga o player constantemente
public class EnemyMovement : MonoBehaviour
{   
    public EnemyScriptableObject enemyData;
    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyData.MoveSpeed * Time.deltaTime);
    }
}
