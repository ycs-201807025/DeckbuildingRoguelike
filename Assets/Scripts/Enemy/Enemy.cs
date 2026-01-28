using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 적 캐릭터를 나타내는 클래스
/// 적의 HP, 행동, AI 등을 관리
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("적 데이터")]
    [SerializeField] private EnemyData enemyData;  // 적의 기본 데이터 (SO)

    [Header("현재 상태")]
    [SerializeField] private int currentHP;        // 현재 HP
    [SerializeField] private int maxHP;            // 최대 HP
    [SerializeField] private int currentDefense = 0;  // 현재 방어도 (턴마다 초기화)

    [Header("다음 행동")]
    [SerializeField] private EnemyAction nextAction;  // 다음에 할 행동

    [Header("이벤트")]
    public UnityEvent<int, int> OnHPChanged;  // HP 변경 시 (현재, 최대)
    public UnityEvent OnDeath;                // 사망 시

    [Header("Debug")]
    [SerializeField] private bool debugMode = true;

    /// <summary>
    /// 적 데이터 프로퍼티
    /// </summary>
    public EnemyData Data => enemyData;

    /// <summary>
    /// 현재 HP 프로퍼티
    /// </summary>
    public int CurrentHP => currentHP;

    /// <summary>
    /// 최대 HP 프로퍼티
    /// </summary>
    public int MaxHP => maxHP;

    /// <summary>
    /// 적 초기화
    /// </summary>
    /// <param name="data">적 데이터</param>
    public void Initialize(EnemyData data)
    {
        if (data == null)
        {
            Debug.LogError("[Enemy] EnemyData가 null입니다!");
            return;
        }

        enemyData = data;           // 데이터 설정
        maxHP = data.maxHP;         // 최대 HP 설정
        currentHP = maxHP;          // 현재 HP를 최대로
        currentDefense = 0;         // 방어도 초기화

        Log($"{data.enemyName} 생성됨 (HP: {currentHP}/{maxHP})");

        // 첫 행동 결정
        DecideNextAction();

        // HP UI 업데이트
        OnHPChanged?.Invoke(currentHP, maxHP);
    }

    /// <summary>
    /// 다음 행동 결정
    /// </summary>
    public void DecideNextAction()
    {
        if (enemyData == null || enemyData.actionPattern.Count == 0)
        {
            Debug.LogError("[Enemy] 행동 패턴이 없습니다!");
            return;
        }

        // 간단한 AI: 랜덤으로 행동 선택
        int randomIndex = Random.Range(0, enemyData.actionPattern.Count);
        nextAction = enemyData.actionPattern[randomIndex];

        Log($"다음 행동 결정: {nextAction.actionType} ({nextAction.value})");

        // TODO: Intent UI 업데이트
    }

    /// <summary>
    /// 행동 실행 (적 턴에 호출됨)
    /// </summary>
    public void ExecuteAction()
    {
        if (nextAction == null)
        {
            Debug.LogError("[Enemy] 실행할 행동이 없습니다!");
            return;
        }

        Log($"행동 실행: {nextAction.actionType}");

        // 행동 타입에 따라 처리
        switch (nextAction.actionType)
        {
            case EnemyActionType.Attack:
                PerformAttack(nextAction.value);
                break;

            case EnemyActionType.Defend:
                PerformDefend(nextAction.value);
                break;

            case EnemyActionType.Buff:
                // TODO: 버프 효과
                Log("버프 사용 (미구현)");
                break;

            case EnemyActionType.Special:
                // TODO: 특수 행동
                Log("특수 행동 (미구현)");
                break;
        }

        // 턴 종료 시 방어도 초기화
        currentDefense = 0;

        // 다음 행동 미리 결정
        DecideNextAction();
    }

    /// <summary>
    /// 공격 행동 수행
    /// </summary>
    /// <param name="damage">공격력</param>
    private void PerformAttack(int damage)
    {
        Log($"플레이어에게 {damage} 데미지 공격!");

        // TODO: PartyManager를 통해 플레이어에게 데미지
        // PartyManager.Instance.TakeDamage(damage);

        // 임시로 로그만 출력
        Debug.Log($"[Enemy] 플레이어가 {damage} 데미지를 받았습니다!");
    }

    /// <summary>
    /// 방어 행동 수행
    /// </summary>
    /// <param name="defenseAmount">방어도</param>
    private void PerformDefend(int defenseAmount)
    {
        currentDefense += defenseAmount;  // 방어도 추가

        Log($"방어도 {defenseAmount} 획득 (현재: {currentDefense})");
    }

    /// <summary>
    /// 데미지 받기
    /// </summary>
    /// <param name="damage">받을 데미지</param>
    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;  // 0 이하면 무시

        // 방어도 계산
        int actualDamage = Mathf.Max(0, damage - currentDefense);  // 방어도만큼 감소

        currentHP -= actualDamage;  // HP 감소

        Log($"{damage} 데미지 받음 (방어: {currentDefense}, 실제: {actualDamage}) - HP: {currentHP}/{maxHP}");

        // HP가 0 이하면 사망
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }

        // HP UI 업데이트
        OnHPChanged?.Invoke(currentHP, maxHP);
    }

    /// <summary>
    /// 사망 처리
    /// </summary>
    private void Die()
    {
        Log($"{enemyData.enemyName} 사망!");

        OnDeath?.Invoke();  // 사망 이벤트 발생

        // TODO: 사망 애니메이션

        // 오브젝트 삭제 (약간의 딜레이 후)
        Destroy(gameObject, 1f);
    }

    /// <summary>
    /// 현재 살아있는지 확인
    /// </summary>
    public bool IsAlive()
    {
        return currentHP > 0;
    }

    /// <summary>
    /// 디버그 로그 출력
    /// </summary>
    private void Log(string message)
    {
        if (debugMode)
        {
            Debug.Log($"[Enemy-{enemyData?.enemyName}] {message}");
        }
    }

    /// <summary>
    /// 테스트용: 데미지 받기
    /// </summary>
    [ContextMenu("Test: Take 10 Damage")]
    private void TestTakeDamage()
    {
        TakeDamage(10);
    }
}
