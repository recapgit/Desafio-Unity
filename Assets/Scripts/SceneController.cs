using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void SceneChange(string nome)
    {
        SceneManager.LoadScene(nome);
        Time.timeScale = 1;
    }
}
