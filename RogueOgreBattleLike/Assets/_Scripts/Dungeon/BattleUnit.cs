using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitTurnState { IDLE, ACTIVE, DEAD }

public class BattleUnit : MonoBehaviour {
    [SerializeField] BattleEntity[] _entities;
    public UnitTurnState TurnState { get; private set; }

    public int X { get; private set; }
    public int Y { get; private set; }
    public Vector2 PreBattleMoveDirection { get; private set; }
    public bool IsPlayer { get; protected set; }

    public BattleEntity[] GetEntities() => _entities;
    public BattleEntity GetEntityAtPosition(int index) => _entities[index];

    public void InitializePosition(int x, int y) {
        X = x;
        Y = y;
    }

    public bool MoveInDirection(int x, int y) {
        int newX = X + x;
        int newY = Y + y;

        if (MapController.Instance.CheckTileForBattleStart(newX, newY, IsPlayer)) {
            PreBattleMoveDirection = new Vector2(x, y);
            BattleUnit opponent = MapController.Instance.GetTileAtPosition(newX, newY).Occupant.GetComponent<BattleUnit>();
            BattleController.Instance.StartBattle(this, opponent);
            return true;
        }

        if (MapController.Instance.TileAtPositionIsBlocked(newX, newY)) {
            if (!IsPlayer) {
                EndTurn();
            }
            return false;
        }

        MapController.Instance.GetTileAtPosition(X, Y).Move();
        X = newX;
        Y = newY;
        //#TODO::Smooth movement
        transform.position = new Vector2(X, Y);
        MapController.Instance.GetTileAtPosition(X, Y).Move(gameObject);
        EndTurn();
        return true;
    }

    public virtual void StartTurn() {
        TurnState = UnitTurnState.ACTIVE;
    }

    public virtual void EndTurn() {
        TurnState = UnitTurnState.IDLE;
        DungeonController.Instance.EndTurn();
    }

    public void SetAsDead() {
        TurnState = UnitTurnState.DEAD;
    }
}
