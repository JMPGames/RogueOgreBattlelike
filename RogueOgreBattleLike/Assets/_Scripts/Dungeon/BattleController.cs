using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleController : MonoBehaviour {
    public static BattleController instance;
    const int NumberOfRounds = 3;

    List<BattleEntity> turnList = new List<BattleEntity>();
    BattleUnit playerUnit;
    BattleUnit enemyUnit;

    int currentRound;
    int currentTurn;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    public void StartBattle(BattleUnit playerUnit, BattleUnit enemyUnit) {
        currentRound = 1;
        currentTurn = 0;

        this.playerUnit = playerUnit;
        this.enemyUnit = enemyUnit;

        BuildTurnList();

        turnList[currentTurn].StartTurn();
    }

    //Build the turnList, sorted by entity speed *DESC
    void BuildTurnList() {
        turnList = new List<BattleEntity>();
        turnList = playerUnit.GetEntities().Concat(enemyUnit.GetEntities()).ToList();
        turnList.Sort( (e1, e2) => e2.GetSpeed().CompareTo(e1.GetSpeed()) );
    }

    void EndBattle(bool battleWon = false) {
        if (battleWon) {
            //earn items, remove unit from dungeon
        }
        //Close battle window and start DungeonController back up
    }

    public void NextTurn() {
        if (BattleStatusCheck()) {
            return;
        }
        IncrementTurn();

        if (currentRound > NumberOfRounds) {
            EndBattle();
            return;
        }
        turnList[currentTurn].StartTurn();
    }

    bool BattleStatusCheck() {
        if (CheckIfUnitIsDead(playerUnit)) {
            DungeonController.instance.GameOver();
            return true;
        }
        else if (CheckIfUnitIsDead(enemyUnit)) {
            EndBattle(true);
            return true;
        }
        return false;
    }

    bool CheckIfUnitIsDead(BattleUnit unit) {
        foreach(BattleEntity e in unit.GetEntities()) {
            if (!e.IsDead()) {
                return false;
            }
        }
        return true;
    }

    void IncrementTurn() {
        currentTurn++;
        if (currentTurn >= turnList.Count) {
            currentRound++;
            currentTurn = 0;
        }
    }
}
