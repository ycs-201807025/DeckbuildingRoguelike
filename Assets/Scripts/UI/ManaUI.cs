using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 마나 UI를 표시하는 컴포넌트
/// ManaManager의 데이터를 받아서 UI에 표시
/// </summary>
public class ManaUI : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private TextMeshProUGUI manaText;  // 마나 텍스트 (예: "3/4")

    [Header("마나 오브 (선택사항)")]
    [SerializeField] private Transform manaOrbContainer;  // 마나 오브들을 담을 컨테이너
    [SerializeField] private GameObject manaOrbPrefab;    // 마나 오브 프리팹 (동그라미)

    private ManaManager manaManager;  // 마나 매니저 참조

    private void Start()
    {
        // ManaManager 찾기
        manaManager = FindObjectOfType<ManaManager>();

        if (manaManager == null)
        {
            Debug.LogError("[ManaUI] ManaManager를 찾을 수 없습니다!");
            return;
        }

        // ManaManager의 이벤트에 구독
        manaManager.OnManaChanged.AddListener(UpdateManaDisplay);

        // 초기 표시
        UpdateManaDisplay(manaManager.CurrentMana, manaManager.MaxMana);
    }

    /// <summary>
    /// 마나 표시 업데이트
    /// </summary>
    /// <param name="current">현재 마나</param>
    /// <param name="max">최대 마나</param>
    private void UpdateManaDisplay(int current, int max)
    {
        // 텍스트 업데이트
        if (manaText != null)
        {
            manaText.text = $"{current}/{max}";  // "3/4" 형식
        }

        // TODO: 마나 오브 업데이트 (선택사항)
        // UpdateManaOrbs(current, max);
    }

    /// <summary>
    /// 마나 오브 업데이트 (시각적 표시)
    /// </summary>
    private void UpdateManaOrbs(int current, int max)
    {
        // 기존 오브들 삭제
        if (manaOrbContainer != null)
        {
            foreach (Transform child in manaOrbContainer)
            {
                Destroy(child.gameObject);
            }
        }

        // 새로 생성
        if (manaOrbPrefab != null && manaOrbContainer != null)
        {
            for (int i = 0; i < max; i++)
            {
                GameObject orb = Instantiate(manaOrbPrefab, manaOrbContainer);

                // 현재 마나보다 많으면 회색으로 표시
                bool isFilled = i < current;
                // TODO: orb 색상 변경 로직
            }
        }
    }
}
