using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour {
    const int _SHAKE_CYCLES_ = 2;
    protected const float _SHAKE_TIME_ = 0.3f;
    const float _SHAKE_WIDTH_ = 4f;

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

    protected Transform _model;
    protected float timer = 0;

    protected virtual void Start()
    {
        _model = this.transform.Find("Model");
        hpText.text = "HP: " + health.ToString();
        RefreshShieldText();
    }

    protected virtual void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            Shake();
        }
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
        this.shield = 0;
        RefreshShieldText();
    }

    public void EndTurn()
    {
    }

    public void ActivateAbility(Ability h, Character target)
    {
        target.TakeDamage(GetDamage(h), GetMagic(h));
        this.shield += GetShield(h);

        RefreshShieldText();
        timer = _SHAKE_TIME_;
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

    public virtual string GetName()
    {
        return this.name;
    }

    protected virtual void Shake()
    {
        if (timer < 0) timer = 0;
        _model.localPosition = _SHAKE_WIDTH_ * Mathf.Sin(_SHAKE_CYCLES_ * 2 * Mathf.PI * timer / _SHAKE_TIME_) * Vector3.right;
    }
}
