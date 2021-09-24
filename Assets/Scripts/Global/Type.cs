using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct GameOptions
{
    public int level;
}

[System.Serializable]
public struct SpriteColorCustom
{
    public string target;
    public enumIgrendients targetIgrendients;
    public Color color;
}

public enum GameState
{
    IDDLE,          // STATE BEFORE INITIALIZE GAME
    PLAY,           // GAME ALREADY TO PLAYING
    PAUSE,          // GAME ON PAUSE TRIGGER
    STOP,           // GAME STOPPED, CRASH, FINISH 
    FINISH          // GAME FINISHED
}

public enum GameMode
{
    POINT,
    TIME,
    ORDER
}

[System.Serializable]
public struct ResourceData
{
    public int buyerTypeCount;
    public int menuTypeCount;
}

public enum GlassState : byte
{
    EMPTY,              // glass empty no igrendients
    FILLED,             // glass have once/some igrendients
    PROCESS,            // glass on process, any request will be reject
    VALIDMENU           // glass have igrendients and return a valid Menu Type
}

public enum MachineState : byte
{
    ON_IDDLE,    // OUTPUT EMPTY
    ON_PROCESS,  // MACHINE GOING PROCESS
    ON_DONE,     // PROCESSING DONE, BUT OUTPUT NOT EQUALS NULL
}

public enum MenuClassification
{
    COFFEE,
    LATTE,
    MILKSHAKE,
    SQUASH
}

public enum MachineType : byte
{
    COFEE_MAKER,
    BEANS_ARABICA,
    BEANS_ROBUSTA,
    FRESHMILK,
    MILK_STEAMMED,
    SQUASH_ORANGE,
    SQUASH_PEACH,
    SQUASH_FRUIT,
    SQUASH_CHIA_SEED,
    SODA,
    MILK_MELON,
    MILK_STRAWBERRY,
    MILK_CHOCO,
    LATTE_CHOCO,
    LATTE_RED_VELVET,
    LATTE_MATCHA,
    LATTE_TARO,
    WHIPPED_CREAM,
    ICE,
    GLASS
}


/// <summary>
/// List for igrendients menu
/// </summary>
public enum enumIgrendients : byte
{
    NULL,
    COFEE_MAKER,
    COFFEE_FILTERED,
    BEANS_ARABICA,
    BEANS_ROBUSTA,
    FRESHMILK,
    MILK_STEAMMED,
    SQUASH_ORANGE,
    SQUASH_PEACH,
    SQUASH_FRUIT,
    SQUASH_CHIA_SEED,
    SODA,
    MILK_MELON,
    MILK_STRAWBERRY,
    MILK_CHOCO,
    LATTE_CHOCO,
    LATTE_RED_VELVET,
    LATTE_MATCHA,
    LATTE_TARO,
    WHIPPED_CREAM,
    ICE,
    GLASS
}


/// <summary>
/// List menu registered
/// </summary>
public enum menuListName : byte
{
    NOT_VALID,
    ARABICA_COFFEE_LATTE,
    ROBUSTA_COFFEE_LATTE,
    LATTE_MATCHA,
    LATTE_RED_VELVET,
    LATTE_TARO,
    LATTE_CHOCOLATE,
    HOT_LATTE_MATCHA,
    HOT_LATTE_RED_VELVET,
    HOT_LATTE_TARO,
    HOT_LATTE_CHOCOLATE,
    SQUASH_ORANGE,
    SQUASH_PEACH,
    SQUASH_GRENADINE,
    MIILKSHAKE_MELON,
    MIILKSHAKE_STRAWBERRY,
    MIILKSHAKE_CHOCO,
    ICE_CAPPUCINO,
    ICE_CAFFE_LATTE,
    ICE_KOPI_SUSU,
    HOT_ICE_CAPPUCINO,
    HOT_ICE_CAFFE_LATTE,
    HOT_ICE_KOPI_SUSU,
    ARABICA_FILTER,
    ROBUSTA_FILTER

}

public enum enumBuyerType
{
    COWOK,
    CEWEK,
    INDIE,
    OJOL_KANTORAN,
    OJOL
}


/// <summary>
/// List menu classification
/// </summary>
public enum menuClassification
{
    COFFEE,
    SQUASH,
    LATTE,
    MILKSHAKE
}

public enum BuyerState
{
    ON_IDDLE,
    ON_WAITING,
    ON_DONE
}