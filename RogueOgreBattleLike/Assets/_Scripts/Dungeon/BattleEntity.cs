using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEntity : BaseEntity {
    //[SerializeField] Skill[] skills = new Skill[4];
    [SerializeField] int _maxHealth;
    [SerializeField] int _speed;

    public int Health { get; private set; }
    public int GetSpeed() => _speed;
    public bool IsDead() => Health <= 0;

    void Awake() {
        Health = _maxHealth;
    }

    public void LoseHealth(int amount) {
        Health -= amount;
        if (Health < 0) {
            Health = 0;
        }
    }

    public virtual void StartTurn() { Debug.Log($"{name}'s turn"); }
    public virtual void EndTurn() { }
}
