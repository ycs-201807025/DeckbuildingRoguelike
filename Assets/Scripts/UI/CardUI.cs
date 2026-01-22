using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 카드 UI를 관리하는 컴포넌트
/// Card 클래스의 데이터를 실제 UI에 표시함
/// </summary>
public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI 요소 참조")]
    [SerializeField] private TextMeshProUGUI nameText;        // 카드 이름 텍스트
    [SerializeField] private TextMeshProUGUI costText;        // 코스트 텍스트
    [SerializeField] private TextMeshProUGUI descriptionText; // 설명 텍스트
    [SerializeField] private Image artworkImage;              // 일러스트 이미지
    [SerializeField] private Image frameImage;                // 프레임 이미지
    [SerializeField] private GameObject upgradeIcon;          // 업그레이드 아이콘 (있으면)

    [Header("호버 효과")]
    [SerializeField] private float hoverScale = 1.2f;         // 호버 시 확대 배율
    [SerializeField] private float animationSpeed = 10f;      // 애니메이션 속도

    private Vector3 originalScale;      // 원래 크기
    private Vector3 targetScale;        // 목표 크기
    private bool isHovering = false;    // 현재 호버 중인지

    private void Awake()
    {
        // 원래 크기 저장
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    private void Update()
    {
        // 부드러운 크기 변화 애니메이션
        transform.localScale = Vector3.Lerp(
            transform.localScale,    // 현재 크기
            targetScale,             // 목표 크기
            Time.deltaTime * animationSpeed  // 속도
        );
    }

    /// <summary>
    /// 카드 데이터를 UI에 표시
    /// </summary>
    /// <param name="card">표시할 카드</param>
    public void UpdateDisplay(Card card)
    {
        if (card == null || card.Data == null) return;  // null 체크

        // 텍스트 업데이트
        if (nameText != null)
            nameText.text = card.Data.cardName;  // 카드 이름

        if (costText != null)
            costText.text = card.GetCost().ToString();  // 코스트

        if (descriptionText != null)
            descriptionText.text = card.GetDescription();  // 설명

        // 이미지 업데이트
        if (artworkImage != null && card.Data.artwork != null)
            artworkImage.sprite = card.Data.artwork;  // 일러스트

        if (frameImage != null)
            frameImage.color = GetFrameColor(card.Data.cardClass);  // 프레임 색상

        // 업그레이드 아이콘 표시/숨김
        if (upgradeIcon != null)
            upgradeIcon.SetActive(card.IsUpgraded);  // 업그레이드 되었으면 표시
    }

    /// <summary>
    /// 클래스에 따른 프레임 색상 반환
    /// </summary>
    /// <param name="characterClass">캐릭터 클래스</param>
    /// <returns>색상</returns>
    private Color GetFrameColor(CharacterClass characterClass)
    {
        switch (characterClass)
        {
            case CharacterClass.Warrior:
                return new Color(1f, 0.3f, 0.3f);  // 빨간색 (검사)
            case CharacterClass.Mage:
                return new Color(0.3f, 0.5f, 1f);  // 파란색 (마법사)
            case CharacterClass.Rogue:
                return new Color(0.3f, 1f, 0.3f);  // 초록색 (도적)
            case CharacterClass.Priest:
                return new Color(1f, 1f, 0.5f);    // 노란색 (성직자)
            default:
                return Color.white;                 // 기본 흰색
        }
    }

    /// <summary>
    /// 마우스가 카드 위에 올라갔을 때
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;                          // 호버 상태 ON
        targetScale = originalScale * hoverScale;   // 목표 크기를 확대된 크기로 설정

        // 카드를 맨 앞으로 가져오기 (다른 카드 위에 표시)
        transform.SetAsLastSibling();
    }

    /// <summary>
    /// 마우스가 카드에서 벗어났을 때
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;           // 호버 상태 OFF
        targetScale = originalScale;  // 목표 크기를 원래 크기로 설정
    }
}
