using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect_Hability : ItemEffect {
    public Hability hability;

    public override void Add(Manager m)
    {
        m.player.habilities.Add(hability);
    }

    public override void Remove(Manager m)
    {
        if (m.player.habilities.Contains(hability))
            m.player.habilities.Remove(hability);
    }
}
