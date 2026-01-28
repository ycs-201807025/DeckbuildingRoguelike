using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 전투 중 마나를 관리하는 매니저
/// 마나 회복, 소비, UI 업데이트 등을 처리
/// </summary>
public class ManaManager : MonoBehaviour
{
    [Header("마나 설정")]
    [SerializeField] private int currentMana = 0;     // 현재 마나
    [SerializeField] private int maxMana = 3;         // 최대 마나 (기본 3)
    [SerializeField] private int baseMana = 3;        // 기본 마나 (파티원 수에 따라 변함)

    [Header("이벤트")]
    public UnityEvent<int, int> OnManaChanged;  // 마나 변경 시 (현재, 최대) 전달

    [Header("Debug")]
    [SerializeField] private bool debugMode = true;

    /// <summary>
    /// 현재 마나 프로퍼티 (읽기 전용)
    /// </summary>
    public int CurrentMana => currentMana;

    /// <summary>
    /// 최대 마나 프로퍼티 (읽기 전용)
    /// </summary>
    public int MaxMana => maxMana;

    private void Start()
    {
        // TurnManager의 이벤트에 구독
        TurnManager turnManager = FindObjectOfType<TurnManager>();
        if (turnManager != null)
        {
            turnManager.OnCombatStart.AddListener(OnCombatStart);        // 전투 시작 시
            turnManager.OnPlayerTurnStart.AddListener(RestoreMana);      // 플레이어 턴 시작 시
        }
        else
        {
            Debug.LogError("[ManaManager] TurnManager를 찾을 수 없습니다!");
        }
    }

    /// <summary>
    /// 전투 시작 시 호출
    /// </summary>
    private void OnCombatStart()
    {
        Log("전투 시작 - 마나 초기화");

        // 파티 크기에 따라 최대 마나 계산
        CalculateMaxMana();

        // 마나 전체 회복
        RestoreMana();
    }

    /// <summary>
    /// 최대 마나 계산 (3 + 파티원 수)
    /// </summary>
    private void CalculateMaxMana()
    {
        // TODO: PartyManager에서 파티원 수 가져오기
        // 지금은 임시로 파티원 1명(검사)으로 가정
        int partySize = 1;  // 나중에 PartyManager.Instance.GetPartySize()로 변경

        maxMana = baseMana + partySize;  // 기본 3 + 파티원 수

        Log($"최대 마나 설정: {maxMana} (파티원 {partySize}명)");
    }

    /// <summary>
    /// 마나 전체 회복 (턴 시작 시)
    /// </summary>
    public void RestoreMana()
    {
        currentMana = maxMana;  // 최대치로 회복

        Log($"마나 회복: {currentMana}/{maxMana}");

        OnManaChanged?.Invoke(currentMana, maxMana);  // UI 업데이트 이벤트 발생
    }

    /// <summary>
    /// 마나 사용
    /// </summary>
    /// <param name="amount">사용할 마나량</param>
    /// <returns>사용 성공 여부</returns>
    public bool UseMana(int amount)
    {
        // 마나가 부족하면 실패
        if (currentMana < amount)
        {
            Log($"마나 부족! (현재: {currentMana}, 필요: {amount})");
            return false;
        }

        // 마나 소비
        currentMana -= amount;

        Log($"마나 사용: {amount} (남은 마나: {currentMana}/{maxMana})");

        OnManaChanged?.Invoke(currentMana, maxMana);  // UI 업데이트

        return true;  // 사용 성공
    }

    /// <summary>
    /// 마나 추가 (특정 카드 효과 등)
    /// </summary>
    /// <param name="amount">추가할 마나량</param>
    public void AddMana(int amount)
    {
        currentMana += amount;  // 마나 추가

        // 최대치 초과하지 않도록 제한
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }

        Log($"마나 추가: {amount} (현재: {currentMana}/{maxMana})");

        OnManaChanged?.Invoke(currentMana, maxMana);  // UI 업데이트
    }

    /// <summary>
    /// 카드를 사용할 수 있는지 확인 (마나 체크)
    /// </summary>
    /// <param name="cost">카드 코스트</param>
    /// <returns>사용 가능 여부</returns>
    public bool CanAffordCard(int cost)
    {
        return currentMana >= cost;
    }

    /// <summary>
    /// 최대 마나 증가 (유물 효과 등)
    /// </summary>
    /// <param name="amount">증가량</param>
    public void IncreaseMaxMana(int amount)
    {
        maxMana += amount;

        Log($"최대 마나 증가: +{amount} (현재 최대: {maxMana})");

        OnManaChanged?.Invoke(currentMana, maxMana);  // UI 업데이트
    }

    /// <summary>
    /// 디버그 로그 출력
    /// </summary>
    private void Log(string message)
    {
        if (debugMode)
        {
            Debug.Log($"[ManaManager] {message}");
        }
    }

    /// <summary>
    /// 테스트용: 마나 사용
    /// </summary>
    [ContextMenu("Test: Use 1 Mana")]
    private void TestUseMana()
    {
        UseMana(1);
    }

    /// <summary>
    /// 테스트용: 마나 회복
    /// </summary>
    [ContextMenu("Test: Restore Mana")]
    private void TestRestoreMana()
    {
        RestoreMana();
    }
}
