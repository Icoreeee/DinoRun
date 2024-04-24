using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class GameManager : MonoBehaviour
{
    private Player _player;
    private Spawner _spawner;
    public static GameManager Instance { get; private set; }
    
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float GameSpeed { get; private set; }

    private bool _isGameStopped;

    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI topScoreText;
    public Button retryButton;

    private float score;
    
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _spawner = FindObjectOfType<Spawner>();
    }

    public void NewGame()
    {
        _isGameStopped = false;
        GameSpeed = initialGameSpeed;
        _player.OnRunning();

        _player.gameObject.SetActive(true);
        _spawner.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        UpdateTopScore();
    }

    public void GameOver()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        _isGameStopped = true;
        GameSpeed = 0f;
        
        _spawner.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        UpdateTopScore();
        score = 0;
        scoreText.text = score.ToString("D7");
    }

    private void Update()
    {
        if (_isGameStopped == false)
        {
            GameSpeed += gameSpeedIncrease * Time.deltaTime;
            score += GameSpeed * Time.deltaTime;
            scoreText.text = Mathf.FloorToInt(score).ToString("D7");
        }
    }

    private void UpdateTopScore()
    {
        float topScore = PlayerPrefs.GetFloat("topscore", 0);
        if (score > topScore)
       {
           topScore = score;
           PlayerPrefs.SetFloat("topscore", topScore);
       }
       
       topScoreText.text = Mathf.FloorToInt(topScore).ToString("D7");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}