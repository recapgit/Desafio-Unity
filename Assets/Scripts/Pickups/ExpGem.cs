using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGem : Pickup, ICollectible
{
    public int expFornecida;

    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.AumentarExp(expFornecida);
        //Destroy(gameObject);
    }
}
