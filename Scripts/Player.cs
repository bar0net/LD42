using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character {
    public List<Ability> abilities;

    protected override void Start()
    {
        Overlord o = FindObjectOfType<Overlord>();
        if (o != null)
        { 
            foreach (Item item in o.inventory)
            {
                for (int i = 0; i < item.abilities.Length; i++) abilities.Add(item.abilities[i].ability);

                this.strength += item.stats.strength;
                this.energy += item.stats.energy;
                this.endurance += item.stats.endurance;
                this.resilience += item.stats.resilience;
            }
        }

        this.health = PlayerPrefs.GetInt("player_health", 100);
        if (this.health <= 0) this.health = 100;

        base.Start();
    }

    protected override void Die()
    {
        PlayerPrefs.SetInt("player_health", this.health);
        FindObjectOfType<Manager>().GameOver();

        base.Die();
    }

    public override void TakeDamage(int damage, int magic)
    {
        base.TakeDamage(damage, magic);

        PlayerPrefs.SetInt("player_health", this.health);
    }
}
