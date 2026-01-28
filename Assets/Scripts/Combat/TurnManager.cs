using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 전투의 턴 진행을 관리하는 매니저
/// 플레이어 턴과 적 턴을 번갈아가며 진행
/// </summary>
public class TurnManager : MonoBehaviour
{
    [Header("턴 상태")]
    [SerializeField] private TurnState currentTurnState = TurnState.NotStarted;  // 현재 턴 상태
    [SerializeField] private int turnCount = 0;  // 현재 턴 수

    [Header("이벤트")]
    public UnityEvent OnCombatStart;      // 전투 시작 시 발생
    public UnityEvent OnPlayerTurnStart;  // 플레이어 턴 시작 시 발생
    public UnityEvent OnPlayerTurnEnd;    // 플레이어 턴 종료 시 발생
    public UnityEvent OnEnemyTurnStart;   // 적 턴 시작 시 발생
    public UnityEvent OnEnemyTurnEnd;     // 적 턴 종료 시 발생
    public UnityEvent OnCombatEnd;        // 전투 종료 시 발생

    [Header("Debug")]
    [SerializeField] private bool debugMode = true;  // 디버그 로그 출력 여부

    /// <summary>
    /// 현재 턴 상태 프로퍼티 (읽기 전용)
    /// </summary>
    public TurnState CurrentTurnState => currentTurnState;

    /// <summary>
    /// 현재 턴 수 프로퍼티 (읽기 전용)
    /// </summary>
    public int TurnCount => turnCount;

    /// <summary>
    /// 전투 시작
    /// </summary>
    public void StartCombat()
    {
        Log("전투 시작!");

        currentTurnState = TurnState.PlayerTurn;  // 플레이어 턴으로 시작
        turnCount = 1;  // 턴 카운트 1부터 시작

        OnCombatStart?.Invoke();  // 전투 시작 이벤트 발생 (?는 null 체크)

        StartPlayerTurn();  // 플레이어 턴 시작
    }

    /// <summary>
    /// 플레이어 턴 시작
    /// </summary>
    public void StartPlayerTurn()
    {
        Log($"===== 플레이어 턴 {turnCount} 시작 =====");

        currentTurnState = TurnState.PlayerTurn;  // 상태를 플레이어 턴으로 변경

        OnPlayerTurnStart?.Invoke();  // 플레이어 턴 시작 이벤트 발생

        // TODO: 마나 회복, 카드 드로우는 다른 매니저에서 처리
    }

    /// <summary>
    /// 플레이어 턴 종료 (턴 종료 버튼을 누르면 호출됨)
    /// </summary>
    public void EndPlayerTurn()
    {
        // 플레이어 턴이 아니면 무시
        if (currentTurnState != TurnState.PlayerTurn)
        {
            Log("플레이어 턴이 아닙니다!");
            return;
        }

        Log("플레이어 턴 종료");

        OnPlayerTurnEnd?.Invoke();  // 플레이어 턴 종료 이벤트 발생

        StartEnemyTurn();  // 적 턴 시작
    }

    /// <summary>
    /// 적 턴 시작
    /// </summary>
    public void StartEnemyTurn()
    {
        Log($"===== 적 턴 {turnCount} 시작 =====");

        currentTurnState = TurnState.EnemyTurn;  // 상태를 적 턴으로 변경

        OnEnemyTurnStart?.Invoke();  // 적 턴 시작 이벤트 발생

        // 적 행동은 EnemyManager에서 처리하도록 이벤트로 알림
        // 적 행동 완료 후 EndEnemyTurn() 호출됨
    }

    /// <summary>
    /// 적 턴 종료 (적의 모든 행동이 끝나면 호출됨)
    /// </summary>
    public void EndEnemyTurn()
    {
        Log("적 턴 종료");

        OnEnemyTurnEnd?.Invoke();  // 적 턴 종료 이벤트 발생

        turnCount++;  // 턴 카운트 증가

        StartPlayerTurn();  // 다시 플레이어 턴으로
    }

    /// <summary>
    /// 전투 종료 (승리 또는 패배)
    /// </summary>
    /// <param name="isVictory">승리 여부</param>
    public void EndCombat(bool isVictory)
    {
        Log(isVictory ? "전투 승리!" : "전투 패배...");

        currentTurnState = isVictory ? TurnState.Victory : TurnState.Defeat;

        OnCombatEnd?.Invoke();  // 전투 종료 이벤트 발생

        // TODO: 승리/패배 화면 표시
    }

    /// <summary>
    /// 현재 플레이어 턴인지 확인
    /// </summary>
    public bool IsPlayerTurn()
    {
        return currentTurnState == TurnState.PlayerTurn;
    }

    /// <summary>
    /// 현재 적 턴인지 확인
    /// </summary>
    public bool IsEnemyTurn()
    {
        return currentTurnState == TurnState.EnemyTurn;
    }

    /// <summary>
    /// 디버그 로그 출력
    /// </summary>
    private void Log(string message)
    {
        if (debugMode)
        {
            Debug.Log($"[TurnManager] {message}");
        }
    }

    /// <summary>
    /// 테스트용: 턴 종료 버튼 (Inspector에서 사용)
    /// </summary>
    [ContextMenu("Test: End Turn")]
    private void TestEndTurn()
    {
        if (currentTurnState == TurnState.PlayerTurn)
        {
            EndPlayerTurn();
        }
        else if (currentTurnState == TurnState.EnemyTurn)
        {
            EndEnemyTurn();
        }
    }
}

/// <summary>
/// 턴 상태 열거형
/// </summary>
public enum TurnState
{
    NotStarted,  // 전투 시작 전
    PlayerTurn,  // 플레이어 턴
    EnemyTurn,   // 적 턴
    Victory,     // 승리
    Defeat       // 패배
}
