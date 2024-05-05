using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector instancia;
    public CharacterScriptableObject characterData;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Extra " + this + "deletado");
            Destroy(gameObject);
        }
    }
    
    public static CharacterScriptableObject GetData()
    {
        return instancia.characterData;
    }

    public void EscolherPersonagem(CharacterScriptableObject character)
    {
        characterData = character;
    }

    public void DestroyGameInstance()
    {
        instancia = null;
        Destroy(gameObject);
    }
}
