using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance { get; private set; }

    [Header("Debug")]
    [SerializeField] private bool debugMode = true;

    [Header("Dungeon State")]
    [SerializeField] private int currentFloor = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        Log("DungeonManager Initialized");
    }

    public void GenerateFloor()
    {
        Log($"Generating Floor {currentFloor}");
        // TODO: ¸Ê »ý¼º ·ÎÁ÷
    }

    public void DescendFloor()
    {
        currentFloor++;
        Log($"Descended to Floor {currentFloor}");
        GenerateFloor();
    }

    public void ReturnToTown()
    {
        Log("Returning to Town");
        currentFloor = 1;
        GameManager.Instance.LoadScene("Town");
    }

    private void Log(string message)
    {
        if (debugMode)
            Debug.Log($"[DungeonManager] {message}");
    }
}
