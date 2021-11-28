using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIInput : MonoBehaviour {
    void Update() {
        if (BattleUI.Instance.BattleUIState == BattleUIState.FREE_TARGET) {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D)) {
                BattleUI.Instance.AdjustTargetSelector();
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A)) {
                BattleUI.Instance.AdjustTargetSelector(false);
            }
        }
    }
}
