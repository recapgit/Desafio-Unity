using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterData;

    //Atributos durante o jogo
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;

    //I-frames
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    //EXP
    //Sistema de níveis do jogo. O jogador coleta exp e se pegar suficiente aumenta o nivel e carrega o que sobrou de exp para o proximo nivel
    [Header("Experiencia/Level")]
    public int exp = 0;
    public int level = 1;
    public int expLimit;

    //Define o range do nível e o limite de exp desse nivel
    [System.Serializable]
    public class LevelRange
    {
        public int levelStart;
        public int levelFinal;
        public int expLimitIncrease;
    }

    public List<LevelRange> levelRanges;


    void Awake()
    {
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
    }


    void Start()
    {
        expLimit = levelRanges[0].expLimitIncrease;
    }

    
    public void AumentarExp(int quantidade)
    {
        exp += quantidade;
        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        if (exp >= expLimit)
        {
            level++;
            exp -= expLimit;

            int expLimitIncrease = 0;

            foreach (LevelRange range in levelRanges)
            {
                if(level > range.levelStart && level <= range.levelFinal)
                {
                    expLimitIncrease = range.expLimitIncrease;
                    break;
                }
            }
            expLimit += expLimitIncrease;
        }
    }

    
    void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }

        //se o timer já chegou a zero, desativar os Iframes
        else if(isInvincible)
        {
            isInvincible = false;
        }
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            currentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        Debug.Log("Morreu");
    }
}
