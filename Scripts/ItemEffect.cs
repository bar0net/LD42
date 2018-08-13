using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ItemEffect {
    public abstract void Add(Manager m);
    public abstract void Remove(Manager m);
}
