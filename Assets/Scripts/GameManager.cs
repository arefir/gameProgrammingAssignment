using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Life playerLife;
    public Life baseLife;

    private void Update()
    {
        if (EnemyManager.instance.enemies.Count <= 0 && WaveManager.instance.waves.Count <= 0)
        {
            SceneManager.LoadScene("WinScreen");
        }

        if (playerLife.currentHp <= 0 || baseLife.currentHp <= 0)
        {
            Invoke(nameof(LoadLose), 3);
        }
    }

    void LoadLose()
    {
        SceneManager.LoadScene("LoseScreen");
    }
}
