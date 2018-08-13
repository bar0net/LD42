using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "GameAssets/Enemy")]
public class EnemyData : ScriptableObject {
    public new string name;
    public string description;
    public Sprite artwork;

    public int health = 100;

    public int strength = 10;
    public int energy = 10;

    public int healthGrow = 5;
    public int strengthGrow = 2;
    public int energyGrow = 1;

    public List<Ability> abilities;
}
