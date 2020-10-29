using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CartStats", menuName = "New CartStats")]
public class CartStats : ScriptableObject
{
    public float topSpeed = 0f;
    [Range(0f, 1f)]
    public float acceleration = 0f;
    [Range(0f, 1f)]
    public float handling = 0f;

    public string cartName = "";       

}
