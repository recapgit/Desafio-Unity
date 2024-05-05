using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IA simples usando MoveTowards que apenas faz com que o inimigo siga o player constantemente
public class EnemyMovement : MonoBehaviour
{   
    EnemyStats enemy;
    Transform player;

    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime);
    }
}
