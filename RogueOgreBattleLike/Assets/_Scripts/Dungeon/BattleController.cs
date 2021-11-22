using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleController : MonoBehaviour {
    public static BattleController instance;
    const int NumberOfRounds = 3;

    [SerializeField] Vector2[] playerPositions;
    [SerializeField] Vector2[] enemyPositions;

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
        PlaceEntities(playerUnit, playerPositions);
        PlaceEntities(enemyUnit, enemyPositions);

        turnList[currentTurn].StartTurn();
    }

    void BuildTurnList() {
        turnList = playerUnit.GetEntities().Concat(enemyUnit.GetEntities()).ToList();
        turnList.Sort( (e1, e2) => e2.GetSpeed().CompareTo(e1.GetSpeed()) );
    }

    void PlaceEntities(BattleUnit unit, Vector2[] positions) {
        for (int i = 0; i < unit.GetEntities().Length; i++) {
            unit.GetEntityAtPosition(i).transform.position = positions[i];
        }
    }

    void EndBattle(bool battleWon = false) {
        if (battleWon) {
            //earn items, exp
        }
        //Close battle window
        DungeonController.instance.EndBattle();
        turnList.Clear();
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
            Debug.Log($"BATTLE WON  enemyUnit.name = {enemyUnit.name}");
            enemyUnit.SetAsDead();
            DungeonController.instance.RemoveBattleUnit(enemyUnit.gameObject);
            Destroy(enemyUnit.gameObject);
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
