using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Debug")]
    [SerializeField] private bool debugMode = true;

    [Header("Game State")]
    private GameState currentState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Initialize()
    {
        Log("GameManager Initialized");
        currentState = GameState.MainMenu;
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        Log($"Game State Changed: {newState}");
    }

    public void LoadScene(string sceneName)
    {
        Log($"Loading Scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Log("Quitting Game");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Log(string message)
    {
        if (debugMode)
            Debug.Log($"[GameManager] {message}");
    }
}

public enum GameState
{
    MainMenu,
    Town,
    Dungeon,
    Combat
}
