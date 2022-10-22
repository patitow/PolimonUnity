using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    [SerializeField] private GameObject Saida;
    [SerializeField] private Vector3 offsetSaida;

    public Vector3 GetSaida()
    {
        return Saida.transform.position+offsetSaida;
    }

    public Vector3 GetOffset() {
        return offsetSaida;
    }
}
