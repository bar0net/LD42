using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "GameAssets/Item")]
public class Item : ScriptableObject {
    public new string name;
    public string description;
    public Sprite artwork;
    public int cost;
    public ItemEffect_Stats stats;
    public ItemEffect_Ability[] abilities;
    public Card[] cards;
}
