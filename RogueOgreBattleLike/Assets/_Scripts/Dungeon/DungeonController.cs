using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DungeonState { PLAYERTURN, ENEMYTURN, BATTLE }

[RequireComponent(typeof(BattleController))]
[RequireComponent(typeof(MapController))]
public class DungeonController : MonoBehaviour {
    public static DungeonController instance;
    public DungeonState BattleState { get; private set; }
    List<GameObject> battleUnits = new List<GameObject>();

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        BattleState = DungeonState.PLAYERTURN;
    }

    public void AddBattleUnit(GameObject unit) {
        battleUnits.Add(unit);
    }

    public void EndPlayerTurn() {
        BattleState = DungeonState.ENEMYTURN;
    }

    public void EndEnemyTurn() {
        BattleState = DungeonState.PLAYERTURN;
    }

    public void GameOver() {

    }
}
