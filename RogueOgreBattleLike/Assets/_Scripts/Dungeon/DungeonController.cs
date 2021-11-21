using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DungeonState { PAUSED, MOVE, BATTLE }

[RequireComponent(typeof(BattleController))]
[RequireComponent(typeof(MapController))]
public class DungeonController : MonoBehaviour {
    public static DungeonController instance;
    public DungeonState DungeonState { get; private set; }
    public List<GameObject> battleUnits = new List<GameObject>();

    DungeonState previousState;
    int turn;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DungeonState = DungeonState.PAUSED;
    }

    public void MapLoaded() {
        turn = 0;
        DungeonState = DungeonState.MOVE;
        battleUnits[turn].GetComponent<BattleUnit>().StartTurn();
    }

    public void AddBattleUnit(GameObject unit) {
        battleUnits.Add(unit);
    }

    public void StartBattle() {
        previousState = DungeonState;
        DungeonState = DungeonState.BATTLE;
    }

    public void EndBattle() {
        DungeonState = previousState;
    }

    public void EndTurn() {
        turn++;
        if (turn >= battleUnits.Count) {
            turn = 0;
        }
        battleUnits[turn].GetComponent<BattleUnit>().StartTurn();
    }

    public void GameOver() {
        Debug.Log("GAMEOVER");
    }
}
