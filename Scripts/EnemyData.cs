using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "GameAssets/Enemy")]
public class EnemyData : ScriptableObject {
    public new string name;
    public string description;

    public int health = 100;

    public int strength = 10;
    public int endurance = 10;
    public int energy = 10;
    public int resilience = 10;

    public List<Hability> habilities;
    public List<Card> cards;
}
