using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

//o spawner funciona com waves (ondas) de inimigos. cada wave é composto por grupos de inimigos.
//basicamente sao 2 grandes loops, o spawn de inimigos no Update() e passar para a proxima wave no BeginNextWave()
public class EnemySpawner : MonoBehaviour
{   
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;    //lista dos grupos de inimigos que vão spawnar
        public int waveQuota;                   //total para spawnar na wave
        public float spawnInterval;
        public int spawnCount;                  //contador dos que já estão na wave
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;                  //números de inimigos que vão spawnar na wave
        public int spawnCount;                  //número de inimigos já spawnados
        public GameObject enemyPrefab;
    }

    public List<Wave> waves;                    // todas as waves que terão no jogo
    public int currentWaveCount;                //contador/índice básico da wave atual

    [Header("Spawner Attributes")]
    float spawnTimer;                           // timer para spawnar o próx inimigo
    public int enemiesAlive;
    public int maxEnemiesAllowed;                //limite de inimigos no mapa
    public bool maxEnemiesReached = false;
    public float waveInterval;                  //timer entre waves
    bool isWaveActive = false;


    Transform player;


    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        CalculateWaveQuota();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0 && !isWaveActive)  //checa se a wave acabou para a proxima comecar
        {
            StartCoroutine(BeginNextWave());
        }
        
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        isWaveActive = true;
        //cooldown entre waves
        yield return new WaitForSeconds(waveInterval);

        //se tem mais waves depois da atual, segue para a proxima
        if(currentWaveCount < waves.Count - 1)
        {
            isWaveActive = false;
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()
    {

        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }
        
        waves[currentWaveCount].waveQuota = currentWaveQuota;
        //Debug.LogWarning(currentWaveQuota);
    }

    void SpawnEnemies()
    {   
        // se não atingiu a cota máxima de spawn de inimigos, continuar a função até atingir
        if(waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            //spawna cada grupo de inimigos até atingir a cota
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {   
                //checa se a qtd mínima desse tipo de inimigo foi spawnado
                if(enemyGroup.spawnCount < enemyGroup.enemyCount)
                {   
                    Vector2 spawnPosition = new Vector2(player.transform.position.x + Random.Range(-10f, 10f), player.transform.position.y + Random.Range(-10f, 10f));
                    Instantiate(enemyGroup.enemyPrefab, spawnPosition, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;

                    //limitador de inimigos que podem estar spawnados
                    if (enemiesAlive > maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }
                }
            }
        }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;

         //reset caso a qtd de inimigos vivos ficou abaixo do limite
        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }

}
