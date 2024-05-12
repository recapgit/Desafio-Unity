using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GarlicController : MonoBehaviour
{   
    
    public WeaponScriptableObject weaponData;
    float currentCooldown;
    
    [Header("UI")]
    public Image skillIcon;

    // Start is called before the first frame update
    protected void Start()
    {
        currentCooldown = 0f;
    }   

    void Update()
    {   
        if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
            updateSkillIcon();
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentCooldown <= 0f)
        {
            Parry();
        }
    }

    // Update is called once per frame
    protected void Parry()
    {
        //base.Attack();
        GameObject spawnedGarlic = Instantiate(weaponData.Prefab);
        spawnedGarlic.transform.position = transform.position;
        spawnedGarlic.transform.parent = transform;    
        currentCooldown = weaponData.CooldownDuration;
    }

    void updateSkillIcon()
    {
        skillIcon.fillAmount = currentCooldown / weaponData.CooldownDuration;
    }
}
