using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    [Header("Debug")]
    [SerializeField] private bool debugMode = true;

    [Header("Combat State")]
    private CombatState currentState;
    private int currentMana;
    private int maxMana;

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
        Log("CombatManager Initialized");
        currentState = CombatState.NotInCombat;
    }

    public void StartCombat()
    {
        Log("Combat Started");
        currentState = CombatState.PlayerTurn;
        maxMana = PartyManager.Instance.GetSharedMana();
        currentMana = maxMana;
    }

    public void EndCombat()
    {
        Log("Combat Ended");
        currentState = CombatState.NotInCombat;
    }

    public void StartPlayerTurn()
    {
        Log("Player Turn Started");
        currentState = CombatState.PlayerTurn;
        currentMana = maxMana;
        // TODO: 카드 드로우
    }

    public void EndPlayerTurn()
    {
        Log("Player Turn Ended");
        currentState = CombatState.EnemyTurn;
        // TODO: 적 턴 시작
    }

    private void Log(string message)
    {
        if (debugMode)
            Debug.Log($"[CombatManager] {message}");
    }
}

public enum CombatState
{
    NotInCombat,
    PlayerTurn,
    EnemyTurn,
    Victory,
    Defeat
}
