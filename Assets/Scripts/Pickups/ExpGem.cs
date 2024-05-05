using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGem : MonoBehaviour, ICollectible
{
    public int expFornecida;

    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.AumentarExp(expFornecida);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
