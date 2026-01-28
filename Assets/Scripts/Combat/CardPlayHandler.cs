using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 카드를 드래그하여 사용하는 기능을 처리
/// 카드를 적에게 드래그하면 효과 발동
/// </summary>
public class CardPlayHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("참조")]
    private Card card;                    // 이 카드 컴포넌트
    private Canvas canvas;                // 부모 Canvas
    private RectTransform rectTransform;  // RectTransform
    private CanvasGroup canvasGroup;      // CanvasGroup (드래그 시 투명도)

    [Header("드래그 상태")]
    private Vector2 originalPosition;     // 원래 위치
    private Transform originalParent;     // 원래 부모
    private bool isDragging = false;      // 현재 드래그 중인지

    [Header("설정")]
    [SerializeField] private float dragAlpha = 0.6f;  // 드래그 시 투명도

    private void Awake()
    {
        // 컴포넌트 가져오기
        card = GetComponent<Card>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        // CanvasGroup 추가 (없으면)
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    /// <summary>
    /// 드래그 시작
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 플레이어 턴이 아니면 드래그 불가
        TurnManager turnManager = FindObjectOfType<TurnManager>();
        if (turnManager == null || !turnManager.IsPlayerTurn())
        {
            Debug.Log("[CardPlayHandler] 플레이어 턴이 아닙니다!");
            eventData.pointerDrag = null;  // 드래그 취소
            return;
        }

        // 마나가 부족하면 드래그 불가
        ManaManager manaManager = FindObjectOfType<ManaManager>();
        if (manaManager == null || !manaManager.CanAffordCard(card.GetCost()))
        {
            Debug.Log("[CardPlayHandler] 마나가 부족합니다!");
            eventData.pointerDrag = null;  // 드래그 취소
            return;
        }

        isDragging = true;

        // 원래 위치와 부모 저장
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;

        // 드래그 중에는 약간 투명하게
        canvasGroup.alpha = dragAlpha;

        // 레이캐스트 무시 (다른 UI 위로 지나갈 수 있게)
        canvasGroup.blocksRaycasts = false;

        // 카드를 맨 앞으로
        transform.SetAsLastSibling();

        Debug.Log($"[CardPlayHandler] {card.Data.cardName} 드래그 시작");
    }

    /// <summary>
    /// 드래그 중
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        // 마우스 위치로 카드 이동
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    /// <summary>
    /// 드래그 종료
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        isDragging = false;

        // 투명도 복원
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // 마우스 아래 오브젝트 확인
        GameObject targetObject = eventData.pointerCurrentRaycast.gameObject;

        if (targetObject != null)
        {
            // 적 오브젝트인지 확인
            Enemy enemy = targetObject.GetComponentInParent<Enemy>();

            if (enemy != null && enemy.IsAlive())
            {
                // 카드 사용!
                UseCardOnEnemy(enemy);
                return;
            }
        }

        // 사용하지 않았으면 원래 위치로 복귀
        ReturnToOriginalPosition();
    }

    /// <summary>
    /// 적에게 카드 사용
    /// </summary>
    /// <param name="enemy">대상 적</param>
    private void UseCardOnEnemy(Enemy enemy)
    {
        Debug.Log($"[CardPlayHandler] {card.Data.cardName}을(를) {enemy.Data.enemyName}에게 사용!");

        // 마나 소비
        ManaManager manaManager = FindObjectOfType<ManaManager>();
        if (manaManager != null)
        {
            manaManager.UseMana(card.GetCost());
        }

        // 카드 효과 적용
        ApplyCardEffect(enemy);

        // 손패에서 카드 제거
        HandManager handManager = FindObjectOfType<HandManager>();
        if (handManager != null)
        {
            handManager.RemoveCardFromHand(card);
        }
    }

    /// <summary>
    /// 카드 효과 적용
    /// </summary>
    /// <param name="enemy">대상 적</param>
    private void ApplyCardEffect(Enemy enemy)
    {
        // 카드 타입에 따라 효과 적용
        switch (card.Data.cardType)
        {
            case CardType.Attack:
                // 공격 카드: 데미지
                int damage = card.GetValue();
                enemy.TakeDamage(damage);
                Debug.Log($"[CardPlayHandler] {damage} 데미지!");
                break;

            case CardType.Defend:
                // 방어 카드: 방어도 (TODO: 플레이어 방어도 시스템)
                int block = card.GetValue();
                Debug.Log($"[CardPlayHandler] {block} 방어도 획득! (미구현)");
                break;

            case CardType.Skill:
                // 스킬 카드: 다양한 효과 (TODO)
                Debug.Log($"[CardPlayHandler] 스킬 효과 (미구현)");
                break;
        }
    }

    /// <summary>
    /// 원래 위치로 복귀
    /// </summary>
    private void ReturnToOriginalPosition()
    {
        rectTransform.anchoredPosition = originalPosition;
        Debug.Log("[CardPlayHandler] 카드 복귀");
    }
}