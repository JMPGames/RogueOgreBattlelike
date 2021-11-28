using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleEntity : BattleEntity {
    public override void StartTurn() {
        BattleUI.Instance.ToggleButtons(true, this);
        base.StartTurn();
    }

    public override void EndTurn() {
        BattleUI.Instance.ToggleButtons();
        base.EndTurn();
    }
}
