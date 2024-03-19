using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public bool TestMode = false;
    static GameManager instance = null;
    public static GameManager Instance => instance;

    [SerializeField] PlayerController playerPrefab;
    [SerializeField] private int maxLives = 5;

    public UnityEvent<int> OnLivesValueChanged;

    public PlayerController PlayerInstance => playerInstance;
    PlayerController playerInstance = null;
    Transform currentCheckpoint;

    //Fields and Properties
    private int _lives = 3;
    public int lives
    {
        get => _lives;
        set
        {
            if (value <= 0)
            {
                // If lives reach zero or below, load Game Over scene
                SceneManager.LoadScene("GameOver");
                return;
            }

            if (_lives > value)
                Respawn();

            _lives = value;

            if (TestMode) Debug.Log("Lives has been set to: " + _lives.ToString());

            OnLivesValueChanged?.Invoke(_lives);
        }
    }

    private int _score = 0;
    public int score
    {
        get => _score;
        set
        {
            _score = value;

            if (TestMode) Debug.Log("Score has been set to: " + _score.ToString());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "Level")
                SceneManager.LoadScene(0);
            else
                SceneManager.LoadScene(1);
        }
    }

    // Method to restart the game
    public void RestartGame()
    {
        // Reset the player's lives
        _lives = maxLives;
        // Load the level scene
        SceneManager.LoadScene("Level");
    }

    public void ChangeScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void SpawnPlayer(Transform spawnLocation)
    {
        playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);
        currentCheckpoint = spawnLocation;
    }

    public void UpdateCheckpoint(Transform updatedCheckpoint)
    {
        currentCheckpoint = updatedCheckpoint;
    }

    void Respawn()
    {
        playerInstance.transform.position = currentCheckpoint.position;
    }
}
