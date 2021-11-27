using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleController : MonoBehaviour {
    public static BattleController Instance;
    const int NUMBER_OF_ROUNDS = 3;

    [SerializeField] Vector2[] _playerPositions;
    [SerializeField] Vector2[] _enemyPositions;

    List<BattleEntity> _turnList = new List<BattleEntity>();
    BattleUnit _playerUnit;
    BattleUnit _enemyUnit;

    int _currentRound;
    int _currentTurn;

    public void StartBattle(BattleUnit attacker, BattleUnit defender) {
        DungeonController.Instance.StartBattle();
        _currentRound = 1;
        _currentTurn = 0;

        _playerUnit = attacker.IsPlayer ? attacker : defender;
        _enemyUnit  = attacker.IsPlayer ? defender : attacker;

        BuildTurnList();
        PlaceEntities(_playerUnit, _playerPositions);
        PlaceEntities(_enemyUnit, _enemyPositions);

        _turnList[_currentTurn].StartTurn();
    }

    public void NextTurn() {
        if (BattleStatusCheck()) {
            return;
        }
        IncrementTurn();

        if (_currentRound > NUMBER_OF_ROUNDS) {
            EndBattle();
            return;
        }
        _turnList[_currentTurn].StartTurn();
    }

    void BuildTurnList() {
        _turnList = _playerUnit.GetEntities().Concat(_enemyUnit.GetEntities()).ToList();
        _turnList.Sort( (e1, e2) => e2.GetSpeed().CompareTo(e1.GetSpeed()) );
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
        DungeonController.Instance.EndBattle();
        _turnList.Clear();
    }

    bool BattleStatusCheck() {
        if (CheckIfUnitIsDead(_playerUnit)) {
            _playerUnit.SetAsDead();
            DungeonController.Instance.GameOver();
            return true;
        }
        else if (CheckIfUnitIsDead(_enemyUnit)) {
            _enemyUnit.SetAsDead();
            DungeonController.Instance.RemoveBattleUnit(_enemyUnit.gameObject);
            Destroy(_enemyUnit.gameObject);
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
        _currentTurn++;
        if (_currentTurn >= _turnList.Count) {
            _currentRound++;
            _currentTurn = 0;
        }
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }
    }
}
