using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; }

    [Header("Debug")]
    [SerializeField] private bool debugMode = true;

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
        Log("DeckManager Initialized");
    }

    // TODO: 덱 관리 메서드들
    // - DrawCards()
    // - ShuffleDeck()
    // - AddCard()
    // - RemoveCard()

    private void Log(string message)
    {
        if (debugMode)
            Debug.Log($"[DeckManager] {message}");
    }
}
