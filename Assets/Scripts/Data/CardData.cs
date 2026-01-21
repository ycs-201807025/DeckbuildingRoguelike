using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Game/Card Data")]
public class CardData : ScriptableObject
{
    [Header("Basic Info")]
    public string cardID;
    public string cardName;
    [TextArea(3, 5)]
    public string description;

    [Header("Card Properties")]
    public CharacterClass cardClass;
    public CardType cardType;
    public int cost;
    public CardRarity rarity;

    [Header("Effects")]
    public int baseValue;
    public int upgradedValue;
    public List<CardEffect> effects = new List<CardEffect>();

    [Header("Visual")]
    public Sprite artwork;
    public Color frameColor = Color.white;
}

public enum CardType
{
    Attack,
    Defend,
    Skill,
    Support
}

public enum CardRarity
{
    Common,
    Uncommon,
    Rare
}

[System.Serializable]
public class CardEffect
{
    public EffectType effectType;
    public int value;
}

public enum EffectType
{
    Damage,
    Block,
    Draw,
    Strength,
    Vulnerable,
    Weak,
    Poison
}
