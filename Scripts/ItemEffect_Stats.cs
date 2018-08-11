using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect_Stats : ItemEffect {
    public int health = 0;
    public int strength = 0;
    public int endurance = 0;
    public int energy = 0;
    public int resilience = 0;

    public override void Add(Manager m)
    {
        m.player.health += this.health;
        m.player.strength += this.strength;
        m.player.endurance += this.endurance;
        m.player.energy += this.energy;
        m.player.resilience += this.resilience;
    }

    public override void Remove(Manager m)
    {
        m.player.health -= this.health;
        m.player.strength -= this.strength;
        m.player.endurance -= this.endurance;
        m.player.energy -= this.energy;
        m.player.resilience -= this.resilience;
    }
}
