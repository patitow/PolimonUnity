using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Polimon", menuName = "Polimon/Criar novo Polimon")]

public class PolimonBase : ScriptableObject
{
    // Base Elements:
    [SerializeField] string PolimonName;
    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [SerializeField] PolimonType type1;
    [SerializeField] PolimonType type2;

    // Base Stats:
    [SerializeField] int maxhp;
    [SerializeField] int attackPoints;
    [SerializeField] int defensePoints;
    [SerializeField] int magicAttackPoints;
    [SerializeField] int magicDefensePoints;
    [SerializeField] int speed;
    [SerializeField] PolimonBase evo;
    [SerializeField] int evoLevel;

    // Learnable Moves
    [SerializeField] private List<LearnableMoves> learnableMoves;

    public string Name {
        get { return PolimonName; }
    }
    public string Description
    {
        get { return description; }
    }
    public int MaxHP
    {
        get { return maxhp; }
    }
    public int AttackPoints
    {
        get { return attackPoints; }
    }
    public int DefensePoints
    {
        get { return defensePoints; }
    }
    public int MagicAttackPoints
    {
        get { return magicAttackPoints; }
    }
    public int MagicDefensePoints
    {
        get { return magicDefensePoints; }
    }
    public int Speed
    {
        get { return speed; }
    }
    public PolimonType Type1
    {
        get { return type1; }
    }
    public PolimonType Type2
    {
        get { return type2; }
    }
    public Sprite FrontSprite
    {
        get { return frontSprite; }
    }
    public Sprite BackSprite
    {
        get { return backSprite; }
    }
    public List<LearnableMoves> LearnableMoves { get => learnableMoves; }
    public int evolutionLevel
    {
        get { return evoLevel;}
    }
    public PolimonBase evolution
    {
        get { return evo;}
    }
}

public enum PolimonType {
    Nenhum,
    Normal,
    Fogo,
    Agua,
    Planta,
    Lava,
    Gelo,
    Terra
}

public class TypeChart {
    static float[][] chart = {
        /*normal=*/new float[]{ 1f, 1f, 1f, 1f, 1f, 1f, 1f},
        /*fogo=*/  new float[]{ 1f, 0.5f, 0.5f, 2f, 0.5f, 1f, 0.5f},
        /*�gua=*/  new float[]{ 1f, 2f, 0.5f, 0.5f, 1f, 0.5f, 1f},
        /*planta=*/new float[]{ 1f, 0.5f, 2f, 0.5f, 0.5f, 1f, 0.5f},
        /*lava=*/  new float[]{ 1f, 0.5f, 1f, 2f, 0.5f, 2f, 0.5f},
        /*gelo=*/  new float[]{ 1f, 1f, 0.5f, 1f, 0.5f, 0.5f, 2f},
        /*terra=*/ new float[]{ 1f, 1f, 2f, 0.5f, 2f, 0.5f, 0.5f}
    };

    public static float getEffectiveness(PolimonType attackType, PolimonType defenseType) {
        if ((attackType == PolimonType.Nenhum) || (defenseType == PolimonType.Nenhum)){
            return 1f;
        }

        int row = (int)attackType - 1;
        int col = (int)defenseType -1;

        return chart[row][col];
    }
}

[System.Serializable]
public class LearnableMoves {
    [SerializeField] private MoveBase moveBase;
    [SerializeField] private int level;

    public MoveBase MoveBase { get => moveBase;}
    public int Level { get => level;}
}





