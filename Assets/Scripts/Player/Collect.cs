using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collect : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out ICollectible collectible))
        {
            collectible.Collect();
        }
    }
}
