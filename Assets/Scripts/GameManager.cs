using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Player _player;
    public Player _player2;
    public Spawner _spawner1;
    public Spawner _spawner2;
    public bool isMultiplayer;
    public static GameManager Instance { get; private set; }
    
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float GameSpeed { get; private set; }

    private bool _isGameStopped;

    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI topScoreText;
    public Button retryButton;
    public GameObject pauseMenu;
    public TextMeshProUGUI godModeTextP1;
    public TextMeshProUGUI godModeTextP2;
    public TextMeshProUGUI livesPlayer1;
    public TextMeshProUGUI livesPlayer2;
    
    private float score;
    private int livesP1;
    private int livesP2;
    public float timeLeft;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Time.timeScale = 0;
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

    public void NewGame()
    {
        Time.timeScale = 1;
        _isGameStopped = false;
        GameSpeed = initialGameSpeed;
        _player.OnRunning();
        if (isMultiplayer)
        {
            _player2.OnRunning();
            livesP1 = 3;
            livesP2 = 3;
        }

        _player.gameObject.SetActive(true);
        _spawner1.gameObject.SetActive(true);
        _spawner2.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        if (isMultiplayer == false) UpdateTopScore();
    }

    public void GameOver()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        SuperEgg[] superEggs = FindObjectsOfType<SuperEgg>();
        
        foreach (var superEgg in superEggs)
        {
            Destroy(superEgg.gameObject);
        }

        _isGameStopped = true;
        GameSpeed = 0f;
        
        _spawner1.gameObject.SetActive(false);
        _spawner2.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        _player.StopAnimation();
        _player.ResetGodMode();
        if (isMultiplayer)
        {
            _player2.ResetGodMode();
            _player2.StopAnimation();
            livesP1 = 0;
            livesP2 = 0;
        }
        
        if (isMultiplayer == false)
        {
            UpdateTopScore();
            score = 0;
            scoreText.text = score.ToString("D7");
        }
        
    }

    private void Update()
    {
        if (_isGameStopped == false)
        {
            GameSpeed += gameSpeedIncrease * Time.deltaTime;

            if (isMultiplayer == false)
            {
                score += GameSpeed * Time.deltaTime;
                scoreText.text = Mathf.FloorToInt(score).ToString("D7");
            }
            
            if (_player.isGod)
            {
                godModeTextP1.text = "God Mode Player 1(sec):" + timeLeft.ToString("F");
                timeLeft -= Time.deltaTime;
            }
            
            if (_player2.isGod)
            {
                godModeTextP2.text = "God Mode Player 2(sec):" + timeLeft.ToString("F");
                timeLeft -= Time.deltaTime;
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
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

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.gameObject.SetActive(true);
    }
    
    public void ContinueGame()
    {
        Time.timeScale = 1;
        pauseMenu.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}