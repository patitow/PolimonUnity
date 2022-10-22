using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Polimon", menuName = "Polimon/Criar lista de Polimon")]
public class PolimonBaseDatabase : ScriptableObject
{
    [SerializeField] public List<PolimonBase> PolimonList;
}
