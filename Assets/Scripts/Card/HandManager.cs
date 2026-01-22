using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 손패를 관리하는 매니저
/// 카드 드로우, 배치, 사용 등을 처리
/// </summary>
public class HandManager : MonoBehaviour
{
    [Header("카드 프리팹")]
    [SerializeField] private GameObject cardPrefab;  // 카드 프리팹 참조

    [Header("손패 영역")]
    [SerializeField] private Transform handArea;     // 손패가 표시될 부모 Transform
    [SerializeField] private float cardSpacing = 220f;  // 카드 간 간격
    [SerializeField] private float maxSpread = 800f;    // 최대 펼쳐질 너비

    [Header("애니메이션")]
    [SerializeField] private float cardMoveSpeed = 10f;  // 카드 이동 속도

    [Header("현재 손패")]
    [SerializeField] private List<Card> cardsInHand = new List<Card>();  // 현재 손패 리스트

    [Header("Debug")]
    [SerializeField] private bool debugMode = true;  // 디버그 모드

    /// <summary>
    /// 테스트용: Inspector 버튼
    /// </summary>
    private void OnValidate()
    {
        // Inspector 갱신용
    }

    // Inspector에서 보이는 테스트 버튼들
#if UNITY_EDITOR
    [Header("테스트 버튼 (Play 모드에서 사용)")]
    [SerializeField] private bool drawCard = false;
    [SerializeField] private bool clearHand = false;

    private void Update()
    {
        // Play 모드에서만 작동
        if (!Application.isPlaying) return;

        // 카드 드로우 버튼
        if (drawCard)
        {
            drawCard = false;
            TestDrawRandomCard();
        }

        // 손패 비우기 버튼
        if (clearHand)
        {
            clearHand = false;
            ClearHand();
        }
    }
#endif
    /// <summary>
    /// 손패에 카드 추가 (드로우)
    /// </summary>
    /// <param name="cardData">추가할 카드 데이터</param>
    /// <param name="ownerClass">카드 소유 클래스</param>
    public void AddCardToHand(CardData cardData, CharacterClass ownerClass)
    {
        if (cardData == null)  // null 체크
        {
            Debug.LogError("[HandManager] CardData가 null입니다!");
            return;
        }

        // 카드 오브젝트 생성
        GameObject cardObj = Instantiate(cardPrefab, handArea);  // handArea 하위에 생성

        // Card 컴포넌트 가져오기
        Card card = cardObj.GetComponent<Card>();
        if (card == null)
        {
            Debug.LogError("[HandManager] Card 컴포넌트를 찾을 수 없습니다!");
            Destroy(cardObj);  // 생성 실패시 삭제
            return;
        }

        // 카드 초기화
        card.Initialize(cardData, ownerClass);

        // 손패 리스트에 추가
        cardsInHand.Add(card);

        // 카드 위치 재배치
        ArrangeCards();

        Log($"카드 추가: {cardData.cardName} (손패: {cardsInHand.Count}장)");
    }

    /// <summary>
    /// 손패에서 카드 제거
    /// </summary>
    /// <param name="card">제거할 카드</param>
    public void RemoveCardFromHand(Card card)
    {
        if (card == null) return;  // null 체크

        // 리스트에서 제거
        if (cardsInHand.Remove(card))
        {
            Log($"카드 제거: {card.Data.cardName}");

            // 오브젝트 삭제
            Destroy(card.gameObject);

            // 남은 카드들 재배치
            ArrangeCards();
        }
    }

    /// <summary>
    /// 손패의 모든 카드 제거 (버리기)
    /// </summary>
    public void ClearHand()
    {
        Log("손패 전체 버리기");

        // 모든 카드 오브젝트 삭제
        foreach (Card card in cardsInHand)
        {
            if (card != null)
            {
                Destroy(card.gameObject);
            }
        }

        // 리스트 비우기
        cardsInHand.Clear();
    }

    /// <summary>
    /// 손패의 카드들을 아름답게 배치
    /// </summary>
    private void ArrangeCards()
    {
        int cardCount = cardsInHand.Count;  // 현재 손패 장수

        if (cardCount == 0) return;  // 카드가 없으면 종료

        // 전체 너비 계산
        float totalWidth = Mathf.Min((cardCount - 1) * cardSpacing, maxSpread);

        // 시작 위치 계산 (중앙 정렬)
        float startX = -totalWidth / 2f;

        // 각 카드 배치
        for (int i = 0; i < cardCount; i++)
        {
            Card card = cardsInHand[i];
            if (card == null) continue;  // null 체크

            // RectTransform 가져오기
            RectTransform cardRect = card.GetComponent<RectTransform>();
            if (cardRect == null) continue;

            // 목표 위치 계산
            float targetX = startX + (i * (totalWidth / Mathf.Max(cardCount - 1, 1)));
            float targetY = 0f;  // Y 위치는 평평하게 (원한다면 곡선으로 가능)

            // 위치 설정 (부드러운 이동은 Update에서 처리 가능)
            cardRect.anchoredPosition = new Vector2(targetX, targetY);

            // 카드 정렬 순서 (앞으로 갈수록 위에 표시)
            cardRect.SetSiblingIndex(i);
        }
    }

    /// <summary>
    /// 현재 손패 카드 수 반환
    /// </summary>
    public int GetHandCount()
    {
        return cardsInHand.Count;
    }

    /// <summary>
    /// 손패의 모든 카드 반환
    /// </summary>
    public List<Card> GetCardsInHand()
    {
        return new List<Card>(cardsInHand);  // 복사본 반환 (안전)
    }

    /// <summary>
    /// 디버그 로그 출력
    /// </summary>
    private void Log(string message)
    {
        if (debugMode)
        {
            Debug.Log($"[HandManager] {message}");
        }
    }

    /// <summary>
    /// 테스트용: 랜덤 카드 드로우
    /// </summary>
    [ContextMenu("Test: Draw Random Card")]
    public void TestDrawRandomCard()
    {
        // Assets/ScriptableObjects/Cards/Warrior 폴더의 카드들 로드
        CardData[] cards = Resources.LoadAll<CardData>("Data/Cards/Warrior");

        if (cards.Length > 0)
        {
            CardData randomCard = cards[Random.Range(0, cards.Length)];
            AddCardToHand(randomCard, CharacterClass.Warrior);
        }
        else
        {
            Debug.LogWarning("[HandManager] 테스트용 카드를 찾을 수 없습니다!");
        }
    }
}
