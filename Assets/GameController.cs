using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gCtrl;
    public GameObject player;
    public GameObject enemy;

    public void Awake()
    {
        if (gCtrl == null)
        {
            DontDestroyOnLoad(gameObject);
            gCtrl = this;
        }
    }

    public void SaveGameState()
    {
        SetPlayerPosition(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        SetEnemyPosition(enemy.transform.position.x, player.transform.position.y, enemy.transform.position.z);
    }

    public float[] GetPlayerPosition()
    {
        float[] playerPosition = { PlayerPrefs.GetFloat("PlayerX"), PlayerPrefs.GetFloat("PlayerY"), PlayerPrefs.GetFloat("PlayerZ") };
        return playerPosition;
    }

    public void SetPlayerPosition(float x, float y, float z)
    {
        PlayerPrefs.SetFloat("PlayerX", x);
        PlayerPrefs.SetFloat("PlayerY", y);
        PlayerPrefs.SetFloat("PlayerZ", z);
    }

    public float[] GetEnemyPosition()
    {
        float[] enemyPosition = { PlayerPrefs.GetFloat("EnemyX"), PlayerPrefs.GetFloat("EnemyY"), PlayerPrefs.GetFloat("EnemyZ") };
        return enemyPosition;
    }

    public void SetEnemyPosition(float x, float y, float z)
    {
        PlayerPrefs.SetFloat("EnemyX", x);
        PlayerPrefs.SetFloat("EnemyY", y);
        PlayerPrefs.SetFloat("EnemyZ", z);
    }
}
