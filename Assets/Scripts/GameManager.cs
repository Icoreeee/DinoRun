using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    MusicControl musicControl;
    
    public Player _player;
    public Player _player2;
    public Spawner _spawner1;
    public Spawner _spawner2;
    public bool isMultiplayer;
    public static GameManager Instance { get; private set; }
    
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float GameSpeed { get; set; }
    public float GameSpeed2 { get; set; }

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
    
    public float score;



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
        _player.gameObject.SetActive(true);
        _spawner1.gameObject.SetActive(true);
        
        if (isMultiplayer)
        {
            GameSpeed2 = initialGameSpeed;
            _player2.gameObject.SetActive(true);
            _spawner2.gameObject.SetActive(true);
            _player2.OnRunning();
        }
        
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        if (isMultiplayer == false) UpdateTopScore();
        // StartCoroutine(musicControl.MusicJump());
    }

    public void GameOver()
    {
        // StopCoroutine(musicControl.MusicJump());
        musicControl.MusicSwitch(6);
        
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
        if (_spawner2)
        {
            _spawner2.gameObject.SetActive(false);
        }
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        _player.StopAnimation();
        _player.ResetGodMode();
        godModeTextP1.text = "God Mode Player 1 (sec): 0:00";
        
        if (isMultiplayer)
        {
            _player2.ResetGodMode();
            _player2.StopAnimation();
            GameSpeed2 = 0f;
            godModeTextP2.text = "God Mode Player 2 (sec): 0:00";
            if (_player.lives > _player2.lives) gameOverText.text = "Player 1 Won!";
            else gameOverText.text = "Player 2 Won!";
        }
        
        if (isMultiplayer == false)
        {
            UpdateTopScore();
            score = 0.0f;
            scoreText.text = Mathf.FloorToInt(score).ToString("D7");
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
            
            if (isMultiplayer) GameSpeed2 += gameSpeedIncrease * Time.deltaTime;
            
            if (_player.isGod)
            {
                godModeTextP1.text = "God Mode Player 1 (sec): " + _player.timeleft.ToString("F");
                _player.timeleft -= Time.deltaTime;
            }

            if(_player2)
            {
                if (_player2.isGod)
                {
                    godModeTextP2.text = "God Mode Player 2 (sec): " + _player2.timeleft.ToString("F");
                    _player2.timeleft -= Time.deltaTime;
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
                musicControl.MusicSwitch(0);
                musicControl.Drum1Control(0);
                musicControl.DrumDelay();
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
        if (Time.timeScale == 0)
        {
            ContinueGame();
            return;
        }
        Time.timeScale = 0;
        pauseMenu.gameObject.SetActive(true);
    }
    
    public void ContinueGame()
    {
        Time.timeScale = 1;
        pauseMenu.gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }
}