using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEntity : BaseEntity {
    //[SerializeField] Skill[] skills = new Skill[4];
    [SerializeField] int maxHealth;
    [SerializeField] int speed;

    public int Health { get; private set; }
    public int GetSpeed() => speed;
    public bool IsDead() => Health <= 0;

    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            Debug.Log(GetTitle());
        }
    }

    public virtual void StartTurn() { }
    public virtual void EndTurn() { }
}
