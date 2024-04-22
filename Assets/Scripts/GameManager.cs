using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Player _player;
    private Spawner _spawner;
    public static GameManager Instance { get; private set; }
    
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float GameSpeed { get; private set; }

    private bool _isGameStopped;
    

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
        
        NewGame();
    }

    private void NewGame()
    {
        _isGameStopped = false;
        GameSpeed = initialGameSpeed;

        _player.gameObject.SetActive(true);
        _spawner.gameObject.SetActive(true);
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
    }

    private void Update()
    {
        if (_isGameStopped == false)
        {
            GameSpeed += gameSpeedIncrease * Time.deltaTime;    
        }
    }
}
