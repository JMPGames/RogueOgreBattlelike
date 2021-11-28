using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleUIState { IDLE, FREE_TARGET, LOCKED_TARGET }
public enum BattleOptionState { IDLE, ATTACKING, SELECTING_USABLE, TARGETING_USABLE, DEFENDING }

[RequireComponent(typeof(BattleUIInput))]
public class BattleUI : MonoBehaviour {
    public static BattleUI Instance;
    [SerializeField] GameObject _battleUIPanel;
    [SerializeField] Text[] _titleTexts;
    [SerializeField] Text[] _healthTexts;
    [SerializeField] Button[] _buttons;

    public BattleUIState BattleUIState { get; private set; }
    public BattleOptionState BattleOptionState { get; private set; }
    public BattleEntity CurrentEntity { get; private set; }

    IUsable _objectBeingUsed;
    int _targetSelection;
    int _uiSelection;

    List<BattleEntity> _entityList = new List<BattleEntity>();
    List<GameObject> _activeTargetSelectors = new List<GameObject>();

    public BattleEntity CurrentTarget() => _entityList[_targetSelection];

    public void AddToEntityList(BattleEntity entity) {
        _entityList.Add(entity);
    }

    public void ToggleUI(bool show = true) {
        _battleUIPanel.SetActive(show);
        BattleUIState = BattleUIState.IDLE;
        BattleOptionState = BattleOptionState.IDLE;
        _targetSelection = 0;
        _uiSelection = 0;
        ResetTargetSelectors();
    }

    public void SetToUseObject(IUsable obj) {
        BattleOptionState = BattleOptionState.TARGETING_USABLE;
        _objectBeingUsed = obj;
        BattleUnit targetUnit = obj.TargetsEnemy ? 
            BattleController.Instance.EnemyUnit : BattleController.Instance.PlayerUnit;
        ToggleTargetSelector(true, targetUnit, obj.TargetsAll);
    }

    public void ToggleTargetSelector(bool show = false, BattleUnit targetUnit = null, bool targetAll = false) { 
        if (show && targetUnit != null) {
            if (targetAll) {
                foreach (BattleEntity be in targetUnit.GetEntities()) {
                    _activeTargetSelectors.Add(be.ToggleTargetIcon());
                }
                BattleUIState = BattleUIState.LOCKED_TARGET;
            }
            else {
                _targetSelection = targetUnit.IsPlayer ? 0 : BattleController.Instance.PlayerUnit.GetEntities().Length;
                BattleEntity target = CurrentTarget();
                _activeTargetSelectors.Add(target.ToggleTargetIcon());
                BattleUIState = BattleUIState.FREE_TARGET;
            }
        }
        else {
            ResetTargetSelectors();
            BattleUIState = BattleUIState.IDLE;
        }
    }

    public void AdjustTargetSelector(bool increased = true) {
        _activeTargetSelectors[0].SetActive(false);
        _activeTargetSelectors.Clear();
        AdjustTargetSelectorHelper(increased);
        while (CurrentTarget().IsDead()) {            
            AdjustTargetSelectorHelper(increased);
        }
        _activeTargetSelectors.Add(CurrentTarget().ToggleTargetIcon());
    }

    void AdjustTargetSelectorHelper(bool increased = true) {
        _targetSelection += increased ? 1 : -1;
        if (_targetSelection >= _entityList.Count) {
            _targetSelection = 0;
        }
        else if (_targetSelection < 0) {
            _targetSelection = _entityList.Count - 1;
        }
    }

    void ResetTargetSelectors() {
        foreach (GameObject ts in _activeTargetSelectors) {
            ts.SetActive(false);
        }
        _activeTargetSelectors.Clear();
    }

    public void ToggleButtons(bool activate = false, BattleEntity entity = null) {
        for (int i = 0; i < _buttons.Length; i++) {
            _buttons[i].interactable = activate;
        }
        CurrentEntity = entity;
    }

    public void SetupTitleTexts(BattleEntity[] entities) {
        for (int i = 0; i < entities.Length; i++) {
            entities[i].Id = i;
            _titleTexts[i].text = entities[i].GetTitle();
        }
    }

    public void SetupHealthText(BattleEntity[] entities) {
        for (int i = 0; i < entities.Length; i++) {
            _healthTexts[i].text = entities[i].Health.ToString();
        }
    }

    public void UpdateHealthText(int index, int health) {
        _healthTexts[index].text = health.ToString();
    }

    public void AttackButton() {
        ToggleOptionState(BattleOptionState.ATTACKING);
        if (BattleOptionState == BattleOptionState.ATTACKING) {
            ToggleTargetSelector(true, BattleController.Instance.EnemyUnit);
        }
        else {
            ToggleTargetSelector();
        }
    }

    public void SkillsButton() {
        ToggleOptionState(BattleOptionState.SELECTING_USABLE);
        ToggleTargetSelector();
    }

    public void ItemButton() {
        ToggleOptionState(BattleOptionState.SELECTING_USABLE);
        ToggleTargetSelector();
    }

    public void DefendButton() {
        ToggleOptionState(BattleOptionState.DEFENDING);
        ToggleTargetSelector();
    }

    void ToggleOptionState(BattleOptionState newState) {
        BattleOptionState = BattleOptionState == newState ? BattleOptionState.IDLE : newState;
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
