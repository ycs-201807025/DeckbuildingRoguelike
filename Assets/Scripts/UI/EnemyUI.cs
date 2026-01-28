using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

/// <summary>
/// 적의 UI를 표시하는 컴포넌트
/// HP 바, 이름, 다음 행동(Intent) 등을 표시
/// </summary>
public class EnemyUI : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private TextMeshProUGUI nameText;     // 적 이름
    [SerializeField] private Slider hpSlider;              // HP 슬라이더
    [SerializeField] private TextMeshProUGUI hpText;       // HP 텍스트 "20/20"
    [SerializeField] private Image intentIcon;             // 다음 행동 아이콘 (선택)
    [SerializeField] private TextMeshProUGUI intentText;   // 다음 행동 텍스트

    private Enemy enemy;  // 연결된 적

    /// <summary>
    /// 초기화
    /// </summary>
    /// <param name="targetEnemy">표시할 적</param>
    public void Initialize(Enemy targetEnemy)
    {
        if (targetEnemy == null)
        {
            Debug.LogError("[EnemyUI] Enemy가 null입니다!");
            return;
        }

        enemy = targetEnemy;

        // 적의 이벤트에 구독
        enemy.OnHPChanged.AddListener(UpdateHPDisplay);

        // 초기 표시
        UpdateNameDisplay();
        UpdateHPDisplay(enemy.CurrentHP, enemy.MaxHP);
    }

    /// <summary>
    /// 이름 표시
    /// </summary>
    private void UpdateNameDisplay()
    {
        if (nameText != null && enemy != null && enemy.Data != null)
        {
            nameText.text = enemy.Data.enemyName;
        }
    }

    /// <summary>
    /// HP 표시 업데이트
    /// </summary>
    /// <param name="current">현재 HP</param>
    /// <param name="max">최대 HP</param>
    private void UpdateHPDisplay(int current, int max)
    {
        // HP 슬라이더 업데이트
        if (hpSlider != null)
        {
            hpSlider.maxValue = max;
            hpSlider.value = current;
        }

        // HP 텍스트 업데이트
        if (hpText != null)
        {
            hpText.text = $"{current}/{max}";
        }
    }

    /// <summary>
    /// 다음 행동(Intent) 표시
    /// </summary>
    /// <param name="actionType">행동 타입</param>
    /// <param name="value">값</param>
    public void UpdateIntentDisplay(EnemyActionType actionType, int value)
    {
        if (intentText != null)
        {
            string intentString = "";

            switch (actionType)
            {
                case EnemyActionType.Attack:
                    intentString = $"⚔ {value}";  // 공격 아이콘 + 데미지
                    break;

                case EnemyActionType.Defend:
                    intentString = $"🛡 {value}";  // 방어 아이콘 + 방어도
                    break;

                case EnemyActionType.Buff:
                    intentString = "💪";  // 버프 아이콘
                    break;

                case EnemyActionType.Special:
                    intentString = "❓";  // 특수 행동
                    break;
            }

            intentText.text = intentString;
        }

        // TODO: intentIcon 이미지 변경
    }
}