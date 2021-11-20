using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputState { ACTION, MENU, INACTIVE }

public class PlayerUnit : BattleUnit {
    InputState inputState;

    void Start() {
        IsPlayer = true;
    }

    void Update() {
        if (TurnState == UnitTurnState.ACTIVE) {
            if (inputState == InputState.ACTION) {
                ActionInput();
            }
        }
    }

    public override void StartTurn() {
        inputState = InputState.ACTION;
        base.StartTurn();
    }

    void ActionInput() {
        if (Input.GetKeyDown(KeyCode.W)) {
            MoveInDirection(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            MoveInDirection(1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            MoveInDirection(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            MoveInDirection(-1, 0);
        }
    }
}
