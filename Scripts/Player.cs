using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character {
    [Header("Player Inventory")]
    public GameObject inventoryUI;

    public List<Ability> abilities;

    protected override void Start()
    {
        Overlord o = FindObjectOfType<Overlord>();
        if (o != null)
        { 
            foreach (Item item in o.inventory)
            {
                for (int i = 0; i < item.abilities.Length; i++) abilities.Add(item.abilities[i].ability);

                for (int i = 0; i < item.stats.Length; i++)
                {
                    this.health = item.stats[i].health;
                    this.strength = item.stats[i].strength;
                    this.energy = item.stats[i].energy;
                    this.endurance = item.stats[i].endurance;
                    this.resilience = item.stats[i].resilience;
                }

                GameObject go = new GameObject();
                go.transform.SetParent(inventoryUI.transform);
                go.transform.localScale = Vector3.one;
                Image img = go.AddComponent<Image>();
                img.sprite = item.artwork;
                img.preserveAspect = true;
            }
        }

    
        base.Start();
    }

    protected override void Die()
    {
        FindObjectOfType<Manager>().GameOver();

        base.Die();
    }
}
