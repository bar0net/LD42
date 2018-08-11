using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    [Header("Player Stats")]
    public int health = 100;

    public int strength = 10;
    public int endurance = 10;
    public int energy = 10;
    public int resilience = 10;

    public int shield = 0;

    public List<Hability> habilities;

}
