using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleController : MonoBehaviour {
    public static BattleController Instance;
    const float END_BATTLE_SCREEN_TIME = 3.0f;
    const int NUMBER_OF_ROUNDS = 1;
    const int TILES_BETWEEN_TEAMS = 10;
    const int PLAYER_POSITION_X = 95;
    const int FIRST_POSITION_Y = 98;

    Camera _battleCamera;
    Vector2[] _enemyPositions = new Vector2[5];
    Vector2[] _playerPositions = new Vector2[5];

    List<BattleEntity> _turnList = new List<BattleEntity>();
    List<BattleEntity> _entityList = new List<BattleEntity>();
    public BattleUnit PlayerUnit { get; private set; }
    public BattleUnit EnemyUnit { get; private set; }

    int _currentRound;
    int _currentTurn;

    public int AmountOfEntities() => _entityList.Count;

    void Start() {
        _battleCamera = GameObject.FindGameObjectWithTag("BattleCamera").GetComponent<Camera>();
        PositionSetup();
    }

    public void StartBattle(BattleUnit attacker, BattleUnit defender) {
        DungeonController.Instance.StartBattle();
        _currentRound = 1;
        _currentTurn = 0;

        PlayerUnit = attacker.IsPlayer ? attacker : defender;
        EnemyUnit  = attacker.IsPlayer ? defender : attacker;

        BuildTurnList();
        
        // ALWAYS "Place" THE PLAYER UNIT FIRST
        PlaceEntities(PlayerUnit, _playerPositions);
        PlaceEntities(EnemyUnit, _enemyPositions);

        BattleUI.Instance.SetupHealthText(PlayerUnit.GetEntities());
        BattleUI.Instance.ToggleUI();

        _battleCamera.enabled = true;
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

    public BattleEntity GetEntityFromListAtIndex(int index) {
        return _entityList[index];
    }

    void BuildTurnList() {
        _turnList = PlayerUnit.GetEntities().Concat(EnemyUnit.GetEntities()).ToList();
        _turnList.Sort( (e1, e2) => e2.GetSpeed().CompareTo(e1.GetSpeed()) );
    }

    void PlaceEntities(BattleUnit unit, Vector2[] positions) {
        for (int i = 0; i < unit.GetEntities().Length; i++) {
            BattleEntity entity = unit.GetEntityAtPosition(i);
            _entityList.Add(entity);
            entity.transform.position = positions[i];
            entity.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void HideEntities() {
        for (int i = 0; i < PlayerUnit.GetEntities().Length; i++) {
            PlayerUnit.GetEntityAtPosition(i).GetComponent<SpriteRenderer>().enabled = false;
        }

        for (int i = 0; i < EnemyUnit.GetEntities().Length; i++) {
            EnemyUnit.GetEntityAtPosition(i).GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void EndBattle(bool battleWon = false) {
        if (battleWon) {
            //earn items, exp
        }
        _battleCamera.enabled = false;
        HideEntities();
        BattleUI.Instance.ToggleUI();
        _turnList.Clear();
        StartCoroutine(EndBattleScreen());
    }

    void PositionSetup() {
        int x;
        int y = FIRST_POSITION_Y;
        int enemyPositionX = PLAYER_POSITION_X + TILES_BETWEEN_TEAMS;

        for (int i = 0; i < 5; i++) {
            y += 1;

            x = i % 2 == 0 ? PLAYER_POSITION_X : PLAYER_POSITION_X - 1;
            _playerPositions[i] = new Vector2(x, y);

            x = i % 2 == 0 ? enemyPositionX : enemyPositionX - 1;
            _enemyPositions[i] = new Vector2(x, y);
        }
    }

    bool BattleStatusCheck() {
        if (CheckIfUnitIsDead(PlayerUnit)) {
            PlayerUnit.SetAsDead();
            DungeonController.Instance.GameOver();
            return true;
        }
        else if (CheckIfUnitIsDead(EnemyUnit)) {
            EnemyUnit.SetAsDead();
            DungeonController.Instance.RemoveBattleUnit(EnemyUnit.gameObject);
            Destroy(EnemyUnit.gameObject);
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

    IEnumerator EndBattleScreen() {
        yield return new WaitForSeconds(END_BATTLE_SCREEN_TIME);
        DungeonController.Instance.EndBattle();
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
