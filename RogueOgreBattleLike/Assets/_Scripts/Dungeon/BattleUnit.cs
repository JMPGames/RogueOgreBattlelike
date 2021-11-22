using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitTurnState { IDLE, ACTIVE, DEAD }

public class BattleUnit : MonoBehaviour {
    [SerializeField] BattleEntity[] entities;
    public UnitTurnState TurnState { get; private set; }

    public int X { get; private set; }
    public int Y { get; private set; }
    public Vector2 PreBattleMoveDirection { get; private set; }
    public bool IsPlayer { get; protected set; }

    public BattleEntity[] GetEntities() => entities;
    public BattleEntity GetEntityAtPosition(int index) => entities[index];

    public void InitializePosition(int x, int y) {
        X = x;
        Y = y;
    }

    public bool MoveInDirection(int x, int y) {
        int newX = X + x;
        int newY = Y + y;

        if (MapController.instance.CheckTileForBattleStart(newX, newY, IsPlayer)) {
            PreBattleMoveDirection = new Vector2(x, y);
            BattleUnit opponent = MapController.instance.GetTileAtPosition(newX, newY).Occupant.GetComponent<BattleUnit>();
            BattleController.instance.StartBattle(this, opponent);
            return true;
        }

        if (MapController.instance.TileAtPositionIsBlocked(newX, newY)) {
            if (!IsPlayer) {
                EndTurn();
            }
            return false;
        }

        MapController.instance.GetTileAtPosition(X, Y).Move();
        X = newX;
        Y = newY;
        //#TODO::Smooth movement
        transform.position = new Vector2(X, Y);
        MapController.instance.GetTileAtPosition(X, Y).Move(gameObject);
        EndTurn();
        return true;
    }

    public virtual void StartTurn() {
        TurnState = UnitTurnState.ACTIVE;
    }

    public virtual void EndTurn() {
        TurnState = UnitTurnState.IDLE;
        DungeonController.instance.EndTurn();
    }

    public void SetAsDead() {
        TurnState = UnitTurnState.DEAD;
    }
}
