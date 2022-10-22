using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Polimon", menuName = "Polimon/Criar novo Polimon customizado")]
public class Polimon : ScriptableObject
{
    
    public PolimonBase Base{get; set;}
    public int Level{get; set;}
    public int HP {get; set;}
    public string Name { get; set; }
    public Sex Sex { get; set; }
    public int xp;

    public List<Move> Moves {get; set;}

    public Polimon(PolimonBase PolimonBase, int PolimonLevel) {
        Base = PolimonBase;
        Level = PolimonLevel;
        xp = Level*100;
        HP = MaxHP;
        Name = PolimonBase.name;

        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves) {
            if (move.Level <= Level) 
                Moves.Add(new Move(move.MoveBase));
            if (Moves.Count >= 4)
                break;
        }
    }

    public void init(PolimonBase PolimonBase, int PolimonLevel) {
        Base = PolimonBase;
        Level = PolimonLevel;
        xp = Level*100;
        HP = MaxHP;
        Name = PolimonBase.name;

        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.MoveBase));
            if (Moves.Count >= 4)
                break;
        }
    }

    public bool vaiEvoluir(int xpPlus){
        if( (xp+xpPlus)/100 >= Base.evolutionLevel && Base.evolutionLevel > 0){
            return true;
        }else{
            return false;
        }
    }

    public void addXp(int xpPlus){
        xp+=xpPlus;
        Level = xp/100;
        if ( Level >= Base.evolutionLevel && Base.evolutionLevel > 0){
            Base = Base.evolution;
        }
        Moves.Clear();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.MoveBase));
            if (Moves.Count >= 4)
                break;
        }
    }

    public int Attack{
        get { return Mathf.FloorToInt((Base.AttackPoints * Level) / 100f) + 5; }
    }
    public int MagicAttack{
        get { return Mathf.FloorToInt((Base.MagicAttackPoints * Level) / 100f) + 5; }
    }
    public int Defense{
        get { return Mathf.FloorToInt((Base.DefensePoints * Level) / 100f) + 5; }
    }
    public int MagicDefense{
        get { return Mathf.FloorToInt((Base.MagicDefensePoints * Level) / 100f) + 5; }
    }
    public int Speed{
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }
    }
    public int MaxHP{
        get { return Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 10; }
    }

    public DamageDetails TakeDamage(Move move, Polimon attacker)
    {
        float critical = 1f;
        if (Random.value * 100f <= 6.25f)
            critical = 2f;

        float type = TypeChart.getEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.getEffectiveness(move.Base.Type, this.Base.Type2);

        var DamageDetails = new DamageDetails()
        {
            Effectiveness = type,
            critical = critical,
            fainted = false
        };

        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Damage * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;

        if (HP <= 0) {
            HP = 0;
            DamageDetails.fainted = true;
        }

        return DamageDetails;
    }

    public Move GetRandomMove() {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}

public class DamageDetails {
    public bool fainted { get; set; }
    public float critical { get; set; }
    public float Effectiveness { get; set; }
    
    }

public enum Sex
{
    Masculino,
    Feminino,
    Nenhum,
}
