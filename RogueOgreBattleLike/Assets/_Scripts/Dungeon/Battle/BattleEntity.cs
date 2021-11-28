﻿using UnityEngine;

public class BattleEntity : BaseEntity {
    //[SerializeField] Skill[] skills = new Skill[4];
    [SerializeField] int _maxHealth;
    [SerializeField] int _speed;
    [SerializeField] GameObject _targetIcon;

    public int Id { get; set; }
    public int Health { get; private set; }
    public int GetSpeed() => _speed;
    public bool IsDead() => Health <= 0;

    void Awake() {
        Id = -1;
        Health = _maxHealth;
    }

    public void LoseHealth(int amount) {
        Health -= amount;
        if (Health < 0) {
            Health = 0;
        }

        if (Id >= 0) {
            BattleUI.Instance.UpdateHealthText(Id, Health);
        }
    }

    public GameObject ToggleTargetIcon(bool show = true) {
        _targetIcon.SetActive(show);
        return _targetIcon;
    }

    public virtual void StartTurn() { }
    public virtual void EndTurn() { BattleController.Instance.NextTurn(); }
}