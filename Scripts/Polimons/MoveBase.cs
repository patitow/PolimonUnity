using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Move", menuName ="Polimon/Criar novo Move")]
public class MoveBase : ScriptableObject
{
    // Base Elements:
    [SerializeField] string MoveName;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] PolimonType type;
    [SerializeField] int damage;
    [SerializeField] int accuraccy;
    [SerializeField] int uses;

    public string Name
    {
        get { return MoveName; }
    }
    public string Description
    {
        get { return description; }
    }
    public PolimonType Type 
    { get => type; }
    public int Damage 
    { get => damage;}
    public int Accuraccy 
    { get => accuraccy;}
    public int Uses 
    { get => uses;}
}
