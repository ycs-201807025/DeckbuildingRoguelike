using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool debugMode = true;

    private void Start()
    {
        Log("Town Scene Loaded");
        GameManager.Instance.ChangeState(GameState.Town);
    }

    public void EnterDungeon()
    {
        Log("Entering Dungeon");
        GameManager.Instance.LoadScene("Dungeon");
    }

    public void OpenPartyMenu()
    {
        Log("Opening Party Menu");
        // TODO: 파티 편성 UI
    }

    private void Log(string message)
    {
        if (debugMode)
            Debug.Log($"[TownManager] {message}");
    }
}
