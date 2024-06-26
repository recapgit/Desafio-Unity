using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//script para o comportamento das armas que sao projeteis
// colocar em um prefab de uma arma que e um projetil
public class ProjectileWeaponBehaviour : MonoBehaviour
{   
    public WeaponScriptableObject weaponData;

    protected Vector3 direction;
    public float destroyAfterSeconds;

    //stats atuais
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;


    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
    }

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    //transformar (rotacionar/orientar para a direção certa). Como o padrão já é pra direita só é preciso mudar para o resto
    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;
        float dirx = dir.x;
        float diry = dir.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if (dirx < 0 && diry == 0) //esq
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        }
        else if (dirx == 0 && diry < 0) //baixo
        {
            scale.y = scale.y * -1;
        }
        else if (dirx == 0 && diry > 0) //cima
        {
            scale.x = scale.x * -1;
        }
        else if (dirx > 0 && diry > 0) //dir cima
        {
            rotation.z = 0f;
        }
        else if (dirx > 0 && diry < 0) //dir baixo
        {
            rotation.z = -90f;
        }
        else if (dirx < 0 && diry > 0) //esq cima
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = -90f;
        }
        else if (dirx < 0 && diry < 0) //esq baixo
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = 0f;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);
            ReducePierce();
        }
    }

    void ReducePierce()
    {
        currentPierce--;
        if(currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}
