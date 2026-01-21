using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    // Scene Names
    public const string SCENE_MAIN_MENU = "MainMenu";
    public const string SCENE_TOWN = "Town";
    public const string SCENE_DUNGEON = "Dungeon";
    public const string SCENE_COMBAT = "Combat";

    // Starting Stats
    public const int WARRIOR_START_HP = 80;
    public const int MAGE_START_HP = 60;
    public const int BASE_MANA = 3;
    public const int MAX_MANA = 10;

    // Card Constants
    public const int DEFAULT_HAND_SIZE = 5;
    public const int WARRIOR_DRAW_PER_TURN = 3;

    // UI
    public const float CARD_HOVER_SCALE = 1.2f;
    public const float ANIMATION_DURATION = 0.3f;
}
