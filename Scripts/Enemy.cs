using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {
    public const float _SHIELD_GRWOTH_ = 0.12f;

    public ActionDisplay actionDisplay;

    [Header("Enemy Data")]
    public EnemyData data;
    public int level = 1;

    Player _p;
    Manager _m;

    Ability next = null;
    System.Random rng = new System.Random(System.DateTime.Now.Millisecond);
    

    protected override void Start()
    {
        _m = FindObjectOfType<Manager>();
        _p = FindObjectOfType<Player>();
 
        this.health = data.health + level * data.healthGrow;
        this.strength = data.strength + level * data.strengthGrow;
        this.energy = data.energy + level * data.energyGrow;
        this.shieldRatio += _SHIELD_GRWOTH_ * level;

        base.Start();
    }

    public override void StartTurn()
    {
        if (next != null) ActivateAbility(next, _m.player);
        
        int r = rng.Next(data.abilities.Count);
        next = data.abilities[r];

        int damage = next.damage;
        int magic = next.magic;

        if (damage > 0) damage += strength;
        if (magic > 0) magic += energy;

        actionDisplay.Display(next, damage, magic);

        if (_m == null) _m = FindObjectOfType<Manager>();
        _m.EndTurn();
    }

    public override void Attack(Character target, int damage, int magic)
    {
        base.Attack(_p, damage, magic);
    }

    protected override void Die()
    {
        _m.EnemyDefeated(this);

        base.Die();
    }
}
