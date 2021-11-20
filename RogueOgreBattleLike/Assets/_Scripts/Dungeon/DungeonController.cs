using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DungeonState { PAUSED, PLAYERTURN, ENEMYTURN, BATTLE }

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
        DungeonState = DungeonState.PLAYERTURN;
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

    public void EndPlayerTurn() {
        DungeonState = DungeonState.ENEMYTURN;
    }

    public void EndEnemyTurn() {
        DungeonState = DungeonState.PLAYERTURN;
    }

    public void GameOver() {
        Debug.Log("GAMEOVER");
    }
}
