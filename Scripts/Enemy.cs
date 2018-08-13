﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Enemy : Character, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public const float _SHIELD_GRWOTH_ = 0.12f;
    public const float _LEVEL_RATIO_ = 0.1f;
    const float _WAIT_TIME_ = 0.5f;

    public ActionDisplay actionDisplay;

    [Header("Enemy Data")]
    public EnemyData data;
    public int level = 1;

    Player _p;
    Manager _m;
    Outline _out;

    Ability next = null;
    System.Random rng = new System.Random(System.DateTime.Now.Millisecond);

    float waitTime = 0;

    protected override void Start()
    {
        _m = FindObjectOfType<Manager>();
        _p = FindObjectOfType<Player>();
        _out = this.GetComponentInChildren<Outline>();
        _out.enabled = false;

        this.health = (int)(data.health + _LEVEL_RATIO_ * level * data.healthGrow);
        this.strength = (int)(data.strength + _LEVEL_RATIO_ * level * data.strengthGrow);
        this.energy = (int)(data.energy + _LEVEL_RATIO_ * level * data.energyGrow);
        this.shieldRatio += _SHIELD_GRWOTH_ * level * _LEVEL_RATIO_;

        this.transform.Find("Model").GetComponent<Image>().sprite = data.artwork;

        base.Start();
    }

    public override void StartTurn()
    {
        waitTime = _WAIT_TIME_;

        if (next != null) ActivateAbility(next, _m.player);
        else
        {
            if (_m == null) _m = FindObjectOfType<Manager>();
            _m.EndTurn();
        }
        
        int r = rng.Next(data.abilities.Count);
        next = data.abilities[r];

        int damage = next.damage;
        int magic = next.magic;

        if (damage > 0) damage += strength;
        if (magic > 0) magic += energy;

        if (actionDisplay != null) actionDisplay.Display(next, damage, magic);

    }

    protected override void Update()
    {
        if (waitTime > 0)  waitTime -= Time.deltaTime;
        else base.Update();
    }

    protected override void Shake()
    {
        base.Shake();

        if (timer == 0) _m.EndTurn();
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

    public override string GetName()
    {
        return data.name;
    }

    public void OnPointerEnter(PointerEventData e)
    {
        _out.enabled = true;
    }

    public void OnPointerExit(PointerEventData e)
    {
        //if (_m.target = this) return;

        _out.enabled = false;
    }


    public void OnPointerClick(PointerEventData e)
    {
        
    }
}
