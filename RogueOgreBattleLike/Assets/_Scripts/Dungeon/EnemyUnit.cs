using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : BattleUnit {

    void Start() {
        IsPlayer = false;
    }

    public override void StartTurn() {
        base.StartTurn();
        //Check if player in range
        //Random movement || A* movement towards player
    }
}
