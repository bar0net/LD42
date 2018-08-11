using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "GameAssets/Item")]
public class Item : ScriptableObject {
    Sprite artwork;
    ItemEffect[] effects;
}
