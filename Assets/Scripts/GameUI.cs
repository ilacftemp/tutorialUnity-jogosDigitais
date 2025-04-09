using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    private float elapsedTime = 0f;
    private int score = 0;
    private PlayerMovement player;
    public float IncreaseSpeedAmount = 0.05f;

    public GameObject enemyPrefab;
    private int hits = 0;

    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalTimeText;

    public GameObject infoPanel;

    public Image[] hearts; // Importante: use UnityEngine.UI
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public AudioClip spawnSound;
    public AudioClip hitSound;
    private AudioSource audioSource;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = $"Time: {minutes:00}:{seconds:00}";
        scoreText.text = $"Score: {score}";
    }

    public void AddPoint()
    {
        score++;

        if (score % 15 == 0)
        {
            player.IncreaseSpeed(IncreaseSpeedAmount);
        }

        if (score % 10 == 0)
            SpawnEnemy();
    }

    public int GetScore()
    {
        return score;
    }

    public float GetTime()
    {
        return elapsedTime;
    }

    Vector2 GetValidSpawnPosition()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 spawnPos;
        float minDistance = 5f; // distância mínima do player
        float mapMinX = -8f, mapMaxX = 8f;
        float mapMinY = -4f, mapMaxY = 4f;

        int attempts = 0;
        do
        {
            float x = Random.Range(mapMinX, mapMaxX);
            float y = Random.Range(mapMinY, mapMaxY);
            spawnPos = new Vector2(x, y);
            attempts++;
        }
        while (Vector2.Distance(spawnPos, playerPos) < minDistance && attempts < 100);

        return spawnPos;
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = GetValidSpawnPosition();
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        
        if (spawnSound != null)
            audioSource.PlayOneShot(spawnSound);
    }

    public void PlayerHit()
    {
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);

        if (hits < hearts.Length)
        {
            hearts[hits].sprite = emptyHeart;
        }

        hits++;

        if (hits >= 3)
            GameOver();
    }


    void GameOver()
    {
        Time.timeScale = 0f;
        infoPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        finalScoreText.text = $"Pontuação Final: {score}";

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        finalTimeText.text = $"Tempo: {minutes:00}:{seconds:00}";
    }

}