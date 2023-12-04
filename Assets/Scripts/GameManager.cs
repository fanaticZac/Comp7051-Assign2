using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class GameManager : MonoBehaviour
{
    private static bool initializedFromMiniGame = false;

    private int playerScore = 0;
    private int enemyScore = 0;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI enemyScoreText;

    public GameObject enemy;

    public GameObject player;

    public int mazeWidth;
    public int mazeDepth;

    public AudioClip respawnSound;
    public AudioClip dyingSound;
    private AudioSource audioSource;

    public static GameManager Instance;

    public GameObject magicDoor;

    [System.Serializable]
    [SerializeField]
    class GameData
    {
        public float playerPositionX;
        public float playerPositionY;
        public float playerPositionZ;
        public float enemyPositionX;
        public float enemyPositionY;
        public float enemyPositionZ;
        public int playerScore;
        public int enemyScore;
    };

    const string fileName = "/gameData.dat";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        SetMagicDoorLocation();

    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        UpdatePlayerScoreUI();
        UpdateEnemyScoreUI();

        // Load previous positions for characters, set new for door
        if (initializedFromMiniGame)
        {
            SetMagicDoorLocation();
            LoadFromPlayerPrefs();
        }
        else
        {
            initializedFromMiniGame = true;
        }
        SceneManager.sceneUnloaded += OnSceneUnloaded;

    }

    public void IncreasePlayerScore()
    {
        playerScore++;
        UpdatePlayerScoreUI();

        if (playerScore % 3 == 0)
        {
            RespawnEnemy();
        }
    }

    public void IncreaseEnemyScore()
    {
        enemyScore++;
        UpdateEnemyScoreUI();
    }

    private void UpdatePlayerScoreUI()
    {
        if (playerScoreText != null)
        {
            playerScoreText.text = "Player Score: " + playerScore.ToString();
        }
    }

    private void UpdateEnemyScoreUI()
    {
        if (enemyScoreText != null)
        {
            enemyScoreText.text = "Enemy Score: " + enemyScore.ToString();
        }
    }

    private void RespawnEnemy()
    {
        if (dyingSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(dyingSound);
        }

        if (enemy != null)
        {
            enemy.SetActive(false);
        }

        StartCoroutine(RespawnCoroutine()); // like update, without update
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(5f);

        if (respawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(respawnSound); // like WIIISHHOOOWHHWHEEEEEEEEE 
        }

        float newY = enemy.transform.position.y;
        float newX = Random.Range(0, mazeWidth) + 0.5f;
        float newZ = Random.Range(0, mazeDepth) + 0.5f;

        enemy.transform.position = new Vector3(newX, newY, newZ);
        enemy.SetActive(true);
    }

    public void RestartGame()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        // GAME OVER 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMiniGameScene()
    {
        PlayerPrefs.SetInt("PlayerScore", playerScore);
        PlayerPrefs.SetInt("EnemyScore", enemyScore);

        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.transform.position.z);

        PlayerPrefs.SetFloat("EnemyX", enemy.transform.position.x);
        PlayerPrefs.SetFloat("EnemyY", enemy.transform.position.y);
        PlayerPrefs.SetFloat("EnemyZ", enemy.transform.position.z);

        PlayerPrefs.Save();
        player.SetActive(false);
        enemy.SetActive(false);


        // and also trying additive?
        SceneManager.LoadScene("MiniGame", LoadSceneMode.Additive);
        // SceneManager.LoadScene("MiniGame");
    }
    private void LoadFromPlayerPrefs()
    {
        playerScore = PlayerPrefs.GetInt("PlayerScore", 0);
        enemyScore = PlayerPrefs.GetInt("EnemyScore", 0);

        float playerX = PlayerPrefs.GetFloat("PlayerX", 0f);
        float playerY = PlayerPrefs.GetFloat("PlayerY", 0f);
        float playerZ = PlayerPrefs.GetFloat("PlayerZ", 0f);
        player.transform.position = new Vector3(playerX, playerY, playerZ);


        float enemyX = PlayerPrefs.GetFloat("EnemyX", 0f);
        float enemyY = PlayerPrefs.GetFloat("EnemyY", 0f);
        float enemyZ = PlayerPrefs.GetFloat("EnemyZ", 0f);
        enemy.transform.position = new Vector3(enemyX, enemyY, enemyZ);

        UpdatePlayerScoreUI();
        UpdateEnemyScoreUI();
    }

    private void SetMagicDoorLocation()
    {
        float newY = magicDoor.transform.position.y;
        float newX = Random.Range(0, mazeWidth) + 0.4f;
        float newZ = Random.Range(0, mazeDepth) + 0.4f;

        magicDoor.transform.position = new Vector3(newX, newY, newZ);
    }

    void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == "MiniGame")
        {
            Debug.Log("MiniGame scene has been unloaded.");

            // Add back player and enemy
            SetMagicDoorLocation(); // new postions to prevent loop collision
            player.SetActive(true);
            enemy.SetActive(true);
            LoadFromPlayerPrefs();

        }
    }

    public void LoadState()
    {
        if(File.Exists(Application.persistentDataPath + fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + fileName, FileMode.Open, FileAccess.Read);
            GameData data = (GameData)bf.Deserialize(fs);
            fs.Close();
            playerScore = data.playerScore;
            enemyScore = data.enemyScore;
            player.transform.position = new Vector3(data.playerPositionX, data.playerPositionY, data.playerPositionZ);
            enemy.transform.position = new Vector3(data.enemyPositionX, data.enemyPositionY, data.enemyPositionZ);
        }
    }

    public void SaveState()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(Application.persistentDataPath + fileName, FileMode.OpenOrCreate, FileAccess.Write);
        GameData data = new GameData();
        data.playerPositionX = player.transform.position.x;
        data.playerPositionY = player.transform.position.y;
        data.playerPositionZ = player.transform.position.z;
        data.enemyPositionX = enemy.transform.position.x;
        data.enemyPositionY = enemy.transform.position.y;
        data.enemyPositionZ = enemy.transform.position.z;
        data.playerScore = playerScore;
        data.enemyScore = enemyScore;
        bf.Serialize(fs, data);
        fs.Close();
    }
}
