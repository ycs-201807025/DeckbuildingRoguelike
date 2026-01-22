using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 런타임에서 실제 카드 인스턴스를 나타내는 클래스
/// CardData를 참조하여 UI에 표시되고 게임 로직을 처리함
/// </summary>
public class Card : MonoBehaviour
{
    [Header("카드 데이터")]
    [SerializeField] private CardData cardData;  // 이 카드의 기본 데이터 (SO)

    [Header("상태")]
    [SerializeField] private bool isUpgraded = false;  // 업그레이드 여부
    [SerializeField] private CharacterClass ownerClass; // 소유한 캐릭터 클래스

    [Header("UI 참조")]
    [SerializeField] private CardUI cardUI;  // 카드 UI 컴포넌트 참조

    /// <summary>
    /// 카드 데이터 프로퍼티
    /// </summary>
    public CardData Data => cardData;

    /// <summary>
    /// 업그레이드 여부 프로퍼티
    /// </summary>
    public bool IsUpgraded => isUpgraded;

    /// <summary>
    /// 소유 클래스 프로퍼티
    /// </summary>
    public CharacterClass OwnerClass => ownerClass;

    /// <summary>
    /// 카드를 초기화
    /// </summary>
    /// <param name="data">카드 데이터</param>
    /// <param name="owner">소유 캐릭터 클래스</param>
    public void Initialize(CardData data, CharacterClass owner)
    {
        cardData = data;           // 카드 데이터 설정
        ownerClass = owner;        // 소유자 설정
        isUpgraded = false;        // 초기에는 업그레이드 안됨

        UpdateUI();                // UI 업데이트
    }

    /// <summary>
    /// 카드를 업그레이드
    /// </summary>
    public void Upgrade()
    {
        if (isUpgraded) return;    // 이미 업그레이드 되었으면 무시

        isUpgraded = true;         // 업그레이드 상태로 변경
        UpdateUI();                // UI 업데이트

        Debug.Log($"[Card] {cardData.cardName} 업그레이드 완료!");
    }

    /// <summary>
    /// 카드 사용 (실제 효과는 CombatManager에서 처리)
    /// </summary>
    /// <param name="target">대상</param>
    public void Play(GameObject target)
    {
        Debug.Log($"[Card] {cardData.cardName} 사용! (대상: {target?.name ?? "없음"})");
        // TODO: CombatManager에게 카드 사용 알림
    }

    /// <summary>
    /// 현재 카드의 코스트를 반환
    /// </summary>
    /// <returns>마나 코스트</returns>
    public int GetCost()
    {
        return cardData.cost;
    }

    /// <summary>
    /// 현재 카드의 효과 값을 반환
    /// </summary>
    /// <returns>효과 값</returns>
    public int GetValue()
    {
        return cardData.GetValue(isUpgraded);
    }

    /// <summary>
    /// 카드 설명을 반환
    /// </summary>
    /// <returns>설명 문자열</returns>
    public string GetDescription()
    {
        return cardData.GetDescription(isUpgraded);
    }

    /// <summary>
    /// UI 업데이트
    /// </summary>
    private void UpdateUI()
    {
        if (cardUI != null)
        {
            cardUI.UpdateDisplay(this);  // CardUI에게 업데이트 요청
        }
    }

    /// <summary>
    /// 디버그용: Inspector에서 정보 표시
    /// </summary>
    private void OnValidate()
    {
        if (cardData != null)
        {
            gameObject.name = $"Card_{cardData.cardName}";  // GameObject 이름 자동 설정
        }
    }
}
