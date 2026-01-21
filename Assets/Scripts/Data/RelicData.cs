using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRelic", menuName = "Game/Relic Data")]
public class RelicData : MonoBehaviour
{
    [Header("Basic Info")]
    public string relicID;
    public string relicName;
    [TextArea(3, 5)]
    public string description;

    [Header("Properties")]
    public RelicRarity rarity;
    public RelicEffect effect;

    [Header("Visual")]
    public Sprite icon;
}

public enum RelicRarity
{
    Common,
    Uncommon,
    Rare,
    Epic
}

public enum RelicEffect
{
    StartingManaPlus1,
    ExtraDrawOnTurnStart,
    ThornsDamage,
    GoldBonus,
    MaxManaPlus1
}
