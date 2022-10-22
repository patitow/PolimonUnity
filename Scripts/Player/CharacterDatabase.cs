using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Characters", menuName = "Character/Criar lista de personagens")]
public class CharacterDatabase : ScriptableObject
{
    [SerializeField] public List<Character> CharacterList;
}
