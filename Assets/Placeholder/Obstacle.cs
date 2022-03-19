using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Obstacles", menuName = "ScriptableObjects/Obstacles", order = 1)]
public class Obstacle : ScriptableObject
{
    public Vector2Int size;
    public GameObject prefab;
}