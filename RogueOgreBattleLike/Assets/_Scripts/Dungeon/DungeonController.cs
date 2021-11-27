using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DungeonState { PAUSED, MOVE, BATTLE }

[RequireComponent(typeof(BattleController))]
[RequireComponent(typeof(MapController))]
public class DungeonController : MonoBehaviour {
    public static DungeonController Instance;
    public DungeonState DungeonState { get; private set; }

    List<GameObject> _battleUnits = new List<GameObject>();
    int _turn;

    public void MapLoaded() {
        _turn = 0;
        DungeonState = DungeonState.MOVE;
        _battleUnits[_turn].GetComponent<BattleUnit>().StartTurn();
    }

    public void AddBattleUnit(GameObject unit) {
        _battleUnits.Add(unit);
    }

    public void RemoveBattleUnit(GameObject unit) {
        _battleUnits.Remove(unit);
    }

    public void StartBattle() {
        DungeonState = DungeonState.BATTLE;
    }

    public void EndBattle() {
        DungeonState = DungeonState.MOVE;
        EndTurn();
    }

    public void EndTurn() {
        _turn++;
        if (_turn >= _battleUnits.Count) {
            _turn = 0;
        }
        _battleUnits[_turn].GetComponent<BattleUnit>().StartTurn();
    }

    public void GameOver() {
        Debug.Log("GAMEOVER");
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }
        DungeonState = DungeonState.PAUSED;
    }
}
