using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputState { INACTIVE, MENU, ACTION }

public class PlayerUnit : BattleUnit {
    InputState _inputState;

    void Start() {
        IsPlayer = true;
        BattleUI.Instance.SetupTitleTexts(GetEntities());
    }

    void Update() {
        if (_inputState == InputState.ACTION && DungeonController.Instance.DungeonState == DungeonState.MOVE) {
            ActionInput();
        }
    }

    public override void StartTurn() {
        _inputState = InputState.ACTION;
        base.StartTurn();
    }

    public override void EndTurn() {
        _inputState = InputState.INACTIVE;
        base.EndTurn();
    }

    void ActionInput() {
        if (Input.GetKeyDown(KeyCode.W)) {
            MoveInDirection(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            MoveInDirection(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            MoveInDirection(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            MoveInDirection(1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            EndTurn();
        }
    }
}
