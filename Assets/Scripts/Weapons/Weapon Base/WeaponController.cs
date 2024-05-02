using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script base para todas as armas que terão no jogo

public class WeaponController : MonoBehaviour
{
    [Header("Weapons Stats")]
    public WeaponScriptableObject weaponData;
    float currentCooldown;
    

    protected PlayerMovement pm;

    protected virtual void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        currentCooldown = weaponData.CooldownDuration;
    }

    
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if(currentCooldown <= 0f)   //se o cooldown da arma zerar, ataque
        {
            Attack();
        }
    }


    protected virtual void Attack()
    {
        currentCooldown = weaponData.CooldownDuration;
    }
}
