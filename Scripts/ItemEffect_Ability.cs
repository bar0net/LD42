using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect_Ability : ItemEffect {
    public Ability ability;

    public override void Add(Manager m)
    {
        m.player.abilities.Add(ability);
    }

    public override void Remove(Manager m)
    {
        if (m.player.abilities.Contains(ability))
            m.player.abilities.Remove(ability);
    }
}
