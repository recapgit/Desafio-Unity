using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

    //Atributos durante o jogo
    public float currentHealth;
    public float currentRecovery;
    public float currentMoveSpeed;
    public float currentMight;
    public float currentProjectileSpeed;
    public float currentMagnet;

    //I-frames
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public TextMeshProUGUI levelText;


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
        characterData = CharacterSelector.GetData();
        CharacterSelector.instancia.DestroyGameInstance();

        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnet = characterData.Magnet;
    }


    void Start()
    {
        expLimit = levelRanges[0].expLimitIncrease;
        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();
    }

    
    public void AumentarExp(int quantidade)
    {
        exp += quantidade;
        LevelUpChecker();
        UpdateExpBar();
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
            
            UpdateLevelText();
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

        Recover();
    }

    void UpdateExpBar()
    {
        expBar.fillAmount = (float)exp / expLimit;
    }

    void UpdateLevelText()
    {
        levelText.text = "Lv. " + level.ToString();
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
            
            UpdateHealthBar();
        }
    }

    // public void RestoreHealth(float amount)
    // {
    //     if (currentHealth < characterData.MaxHealth)
    //     {
    //         currentHealth += amount;
    //         if(currentHealth > characterData.MaxHealth)
    //         {
    //             currentHealth = characterData.MaxHealth;
    //         }
    //     }
    //     UpdateHealthBar();
    // }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / characterData.MaxHealth;
    }

    public void Kill()
    {
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.GameOver();
        }
    }

    void Recover()
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime;

            UpdateHealthBar();
            
            if(currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }
}
