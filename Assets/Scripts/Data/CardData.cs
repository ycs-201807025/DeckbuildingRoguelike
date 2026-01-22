using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카드 데이터를 저장하는 ScriptableObject
/// Unity 에디터에서 카드를 생성하고 관리할 수 있게 해줌
/// </summary>
[CreateAssetMenu(fileName = "NewCard", menuName = "Game/Card Data")]
public class CardData : ScriptableObject
{
    [Header("기본 정보")]
    public string cardID;              // 카드 고유 ID (예: "warrior_strike")
    public string cardName;            // 카드 이름 (예: "강타")
    [TextArea(3, 5)]                   // Inspector에서 여러 줄 입력 가능
    public string description;         // 카드 설명

    [Header("카드 속성")]
    public CharacterClass cardClass;   // 어느 클래스의 카드인지
    public CardType cardType;          // 카드 타입 (공격/방어/스킬)
    public int cost;                   // 마나 코스트
    public CardRarity rarity;          // 등급 (커먼/언커먼/레어)

    [Header("카드 효과")]
    public int baseValue;              // 기본 효과 값
    public int upgradedValue;          // 업그레이드 시 효과 값
    public List<CardEffect> effects = new List<CardEffect>(); // 추가 효과 목록

    [Header("비주얼")]
    public Sprite artwork;             // 카드 일러스트
    public Color frameColor = Color.white; // 카드 프레임 색상

    /// <summary>
    /// 업그레이드 여부에 따라 실제 효과 값을 반환
    /// </summary>
    /// <param name="isUpgraded">업그레이드 여부</param>
    /// <returns>효과 값</returns>
    public int GetValue(bool isUpgraded)
    {
        return isUpgraded ? upgradedValue : baseValue;
    }

    /// <summary>
    /// 카드 설명 텍스트를 생성 (업그레이드 값 포함)
    /// </summary>
    /// <param name="isUpgraded">업그레이드 여부</param>
    /// <returns>설명 문자열</returns>
    public string GetDescription(bool isUpgraded)
    {
        string desc = description;
        // {value} 를 실제 수치로 교체
        desc = desc.Replace("{value}", GetValue(isUpgraded).ToString());
        return desc;
    }
}

/// <summary>
/// 카드 타입 열거형
/// </summary>
public enum CardType
{
    Attack,   // 공격 카드
    Defend,   // 방어 카드
    Skill,    // 스킬 카드
    Support   // 지원 카드
}

/// <summary>
/// 카드 등급 열거형
/// </summary>
public enum CardRarity
{
    Common,    // 커먼 (흰색)
    Uncommon,  // 언커먼 (초록색)
    Rare       // 레어 (파란색)
}

/// <summary>
/// 카드 효과 데이터
/// </summary>
[System.Serializable]
public class CardEffect
{
    public EffectType effectType;  // 효과 타입
    public int value;              // 효과 값
    public TargetType targetType;  // 대상 타입
}

/// <summary>
/// 효과 타입 열거형
/// </summary>
public enum EffectType
{
    Damage,       // 데미지
    Block,        // 방어
    Draw,         // 카드 드로우
    Strength,     // 힘 버프
    Vulnerable,   // 취약 디버프
    Weak,         // 약화 디버프
    Poison        // 독
}

/// <summary>
/// 대상 타입 열거형
/// </summary>
public enum TargetType
{
    Enemy,        // 적 단일
    AllEnemies,   // 적 전체
    Self,         // 자신
    Ally,         // 아군 단일
    AllAllies     // 아군 전체
}