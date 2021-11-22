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

    public void StartBattle(BattleUnit attacker, BattleUnit defender) {
        DungeonController.instance.StartBattle();
        currentRound = 1;
        currentTurn = 0;

        playerUnit = attacker.IsPlayer ? attacker : defender;
        enemyUnit  = attacker.IsPlayer ? defender : attacker;

        BuildTurnList();

        turnList[currentTurn].StartTurn();
    }

    void BuildTurnList() {
        turnList = new List<BattleEntity>();
        turnList = playerUnit.GetEntities().Concat(enemyUnit.GetEntities()).ToList();
        turnList.Sort( (e1, e2) => e2.GetSpeed().CompareTo(e1.GetSpeed()) );
    }

    void EndBattle(bool battleWon = false) {
        if (battleWon) {
            int x = (int)playerUnit.PreBattleMoveDirection.x;
            int y = (int)playerUnit.PreBattleMoveDirection.y;
            playerUnit.MoveInDirection(x, y);
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
            playerUnit.SetAsDead();
            DungeonController.instance.GameOver();
            return true;
        }
        else if (CheckIfUnitIsDead(enemyUnit)) {
            enemyUnit.SetAsDead();
            EndBattle(true);
            return true;
        }
        return false;
    }

    bool CheckIfUnitIsDead(BattleUnit unit) {
        int numberDead = 0;
        foreach(BattleEntity entity in unit.GetEntities()) {
            if (entity.IsDead()) {
                numberDead++;
            }
        }
        return numberDead >= unit.GetEntities().Length;
    }

    void IncrementTurn() {
        currentTurn++;
        if (currentTurn >= turnList.Count) {
            currentRound++;
            currentTurn = 0;
        }
    }
}
