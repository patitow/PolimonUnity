using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Character", menuName = "Character/Criar novo Personagem")]
public class Character : ScriptableObject
{
    public string characterName;
    public Sprite characterSprite;
    public RuntimeAnimatorController characterAnimator;
    public Sprite[] battleAnimation;
}
