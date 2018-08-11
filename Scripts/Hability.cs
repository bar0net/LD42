using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hability", menuName = "GameAssets/Hability")]
public class Hability : ScriptableObject {
    public new string name;
    public string description;

    public int swordCost = 0;
    public int shieldCost = 0;

    public int damage = 0;
    public int defense = 0;
}
