using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KartStats", menuName = "New KartStats")]
public class KartStats : ScriptableObject
{
    public float topSpeed = 0f;
    [Range(0f, 1.5f)]
    public float acceleration = 0f;
    [Range(0f, 2f)]
    public float handling = 0f;

    public string kartName = "";       

}
