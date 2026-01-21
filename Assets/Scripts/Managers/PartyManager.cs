using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance { get; private set; }

    [Header("Debug")]
    [SerializeField] private bool debugMode = true;

    [Header("Party")]
    [SerializeField] private List<PartyMember> currentParty = new List<PartyMember>();
    [SerializeField] private int maxPartySize = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        Log("PartyManager Initialized");
        // TODO: 초기 파티 설정 (검사 1명)
    }

    public void AddMember(PartyMember member)
    {
        if (currentParty.Count < maxPartySize)
        {
            currentParty.Add(member);
            Log($"Added {member.characterClass} to party");
        }
        else
        {
            Log("Party is full!");
        }
    }

    public void RemoveMember(PartyMember member)
    {
        currentParty.Remove(member);
        Log($"Removed {member.characterClass} from party");
    }

    public List<PartyMember> GetParty()
    {
        return currentParty;
    }

    public int GetPartySize()
    {
        return currentParty.Count;
    }

    public int GetSharedMana()
    {
        return 3 + currentParty.Count;
    }

    public bool IsPartyAlive()
    {
        foreach (var member in currentParty)
        {
            if (member.currentHP > 0)
                return true;
        }
        return false;
    }

    private void Log(string message)
    {
        if (debugMode)
            Debug.Log($"[PartyManager] {message}");
    }
}

[System.Serializable]
public class PartyMember
{
    public CharacterClass characterClass;
    public int currentHP;
    public int maxHP;
    // TODO: 덱, 손패 등 추가
}

public enum CharacterClass
{
    Warrior,
    Mage,
    Rogue,
    Priest
}