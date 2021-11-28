using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleEntity : BattleEntity {
    public override void StartTurn() {
        base.StartTurn();
        StartCoroutine(test());
    }

    IEnumerator test() {
        yield return new WaitForSeconds(1);
        BattleController.Instance.PlayerUnit.GetEntityAtPosition(0).LoseHealth(5);
        EndTurn();
    }
}
