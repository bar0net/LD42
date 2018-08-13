using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "GameAssets/Ability")]
public class Ability : ScriptableObject {
    [Header("Description")]
    public new string name;
    public string description;
    public Sprite icon;

    [Header("Cost")]
    [SerializeField]
    public Collection.TokenValue[] tokenCost;

    [Header("Action")]
    public int damage = 0;
    public int magic = 0;
    public int defense = 0;
}
