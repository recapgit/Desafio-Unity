using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    public Vector3 noTerrainPosition;
    public LayerMask terrainMask;
    public GameObject currentChunk;
    PlayerMovement pm;

    //necessário senão vai criar chunks infinitamente sem descarregar elas
    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxOpDist; //Precisa ser maior que as dimensoes x e y do tilemap
    float opDist;
    float optimizerCooldown;
    public float optimizerCooldownDuration;


    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        ChunkChecker();
    }

    // Chunk = 28 tiles (quadrados)
    // função pra checar o spawn de chunks no mapa

    void ChunkChecker()
    {

        if (!currentChunk)
        {
            return;
        }

        if (pm.moveDir.x > 0 && pm.moveDir.y == 0)                                                                         //se for pra direita
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Static Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Static Right").position;  
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y == 0)                                                                     //se for pra esquerda
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Static Left").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Static Left").position;
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x == 0 && pm.moveDir.y > 0)                                                                     //se for pra cima
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Static Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Static Up").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x == 0 && pm.moveDir.y < 0)                                                                     //se for pra baixo
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Static Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Static Down").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x > 0 && pm.moveDir.y > 0)                                                                     //se for pra cima+direita
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Static Right Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Static Right Up").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x > 0 && pm.moveDir.y < 0)                                                                     //se for pra baixo+direita
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Static Right Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Static Right Down").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y > 0)                                                                     //se for pra cima+esquerda
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Static Left Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Static Left Up").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y < 0)                                                                     //se for pra baixo+esquerda
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Static Left Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Static Left Down").position;
                SpawnChunk();
            }
        }
    }

    void SpawnChunk()
    {
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], noTerrainPosition, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
    }

    void ChunkOptimzer()
    {
        optimizerCooldown -= Time.deltaTime;

        if (optimizerCooldown <= 0f)
        {
            optimizerCooldown = optimizerCooldownDuration;   //checa a cada 1 segundo por padrão, se preciso, baixar esse valor para checar mais vezes
        }
        else
        {
            return;
        }

        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (opDist > maxOpDist)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }

    
}

