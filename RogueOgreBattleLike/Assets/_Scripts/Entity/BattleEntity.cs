using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEntity : MonoBehaviour {
    //[SerializeField] Skill[] skills = new Skill[4];
    [SerializeField] int maxHealth;
    public int Health { get; private set; }
}
