using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HistoryState
{
    Start, // antes de ir pro bloco c
    AposBlocoC, // apos ir ao bloco c
    AposSamuel, // apos derrotar os professores
    AposProfessores, // apos derrotar samuel
    Fim, // apos chegar na salas
    Busy
}

[CreateAssetMenu(fileName = "Characters", menuName = "Character/Criar novo NPC")]
public class NpcBase : ScriptableObject
{

    [SerializeField] public Character Character;

    [Header("Diálogos do NPC")]
    [SerializeField] public List<string> Start;
    [SerializeField] public List<string> AposBlocoC;
    [SerializeField] public List<string> AposSamuel;
    [SerializeField] public List<string> AposProfessores;
    [SerializeField] public List<string> Fim;

    public List<string> getList(HistoryState state)
    {
        switch (state)
        {
            case HistoryState.Start:
                return Start;
            case HistoryState.AposBlocoC:
                return AposBlocoC;
            case HistoryState.AposSamuel:
                return AposSamuel;
            case HistoryState.AposProfessores:
                return AposProfessores;
            case HistoryState.Fim:
                return Fim;
            case HistoryState.Busy:
                // do nothing
                break;
        }
        return null;
    }
}