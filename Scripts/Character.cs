using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    [Header("Character Stats")]
    public int health = 100;

    public int strength = 10;
    public int endurance = 0;
    public int energy = 10;
    public int resilience = 0;

    // Shield = shieldRatio * defense + shieldBase
    public float shieldRatio = 1.0f;
    public float shieldBase = 0;

    public int shield = 0;

    [Header("UI")]
    public Text hpText;
    public Text shieldText;

    protected virtual void Start()
    {
        hpText.text = "HP: " + health.ToString();
        RefreshShieldText();
    }

    // TODO: Refine Attack and Defense
    public virtual void Attack(Character target, int damage, int magic)
    {
        target.TakeDamage(damage, magic);
    }

    public virtual void TakeDamage(int damage, int magic)
    {
        // damage gets mitigated by endurance
        if (damage < endurance) damage = 0;
        else damage -= endurance;

        // magic is mitigated by resilience
        if (magic < resilience) magic = 0;
        else magic -= resilience;

        // Shield absorbs part of the damage
        // Apply damage.
        if (shield > (damage + magic)) shield -= (damage + magic);
        else
        {
            health = health + shield - (damage + magic);
            shield = 0;


            if (health <= 0)
            {
                hpText.text = "HP: 0";
                Die();
            }
            else hpText.text = "HP: " + health.ToString();
        }

        RefreshShieldText();
    }

    protected virtual void Die()
    {
        Destroy(this.gameObject);
    }

    public virtual void StartTurn()
    {

    }

    public void EndTurn()
    {
        this.shield = 0;
    }

    public void ActivateAbility(Ability h, Character target)
    {
        target.TakeDamage(GetDamage(h), GetMagic(h));
        this.shield += GetShield(h);

        RefreshShieldText();
    }

    public int GetDamage(Ability h)
    {
        if (h.damage > 0) return h.damage + this.strength;
        return 0;
    }

    public int GetMagic(Ability h)
    {
        if (h.magic > 0) return h.magic + this.energy;
        return 0;
    }

    public int GetShield(Ability h)
    {
        if (h.defense > 0) return Mathf.FloorToInt(shieldRatio * h.defense + shieldBase);
        return 0;
    }

    void RefreshShieldText()
    {
        if (shield <= 0) shieldText.text = "";
        else shieldText.text = "Shield: " + shield.ToString();
    }
}
