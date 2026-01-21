using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Basic Info")]
    public string enemyID;
    public string enemyName;

    [Header("Stats")]
    public int maxHP;
    public int attackDamage;
    public int defense;

    [Header("AI Behavior")]
    public List<EnemyAction> actionPattern = new List<EnemyAction>();

    [Header("Rewards")]
    public int goldDrop;

    [Header("Visual")]
    public GameObject enemyPrefab;
    public Sprite enemySprite;
}

[System.Serializable]
public class EnemyAction
{
    public EnemyActionType actionType;
    public int value;
    public float weight = 1f; // 행동 선택 가중치
}

public enum EnemyActionType
{
    Attack,
    Defend,
    Buff,
    Special
}
