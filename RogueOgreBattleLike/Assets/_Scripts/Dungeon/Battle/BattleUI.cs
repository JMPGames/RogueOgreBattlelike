using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleUIState { IDLE, FREE_TARGET, LOCKED_TARGET }

[RequireComponent(typeof(BattleUIInput))]
public class BattleUI : MonoBehaviour {
    public static BattleUI Instance;
    [SerializeField] GameObject _battleUIPanel;
    [SerializeField] Text[] _titleTexts;
    [SerializeField] Text[] _healthTexts;
    [SerializeField] Button[] _buttons;

    public BattleUIState BattleUIState { get; private set; }
    BattleEntity _currentEntity;
    public int _targetSelection;
    public List<GameObject> _activeTargetSelectors = new List<GameObject>();

    public void ToggleUI(bool show = true) {
        _battleUIPanel.SetActive(show);
        _targetSelection = 0;
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
                BattleEntity target = BattleController.Instance.GetEntityFromListAtIndex(_targetSelection);
                _activeTargetSelectors.Add(target.ToggleTargetIcon());
                BattleUIState = BattleUIState.FREE_TARGET;
            }
        }
        else {
            foreach(GameObject ts in _activeTargetSelectors) {
                ts.SetActive(false);
            }
            BattleUIState = BattleUIState.IDLE;
        }
    }

    public void AdjustTargetSelector(bool increased = true) {
        _activeTargetSelectors[0].SetActive(false);
        _activeTargetSelectors.Clear();
        AdjustTargetSelectorHelper(increased);
        while (BattleController.Instance.GetEntityFromListAtIndex(_targetSelection).IsDead()) {            
            AdjustTargetSelectorHelper(increased);
        }
        _activeTargetSelectors.Add(BattleController.Instance.GetEntityFromListAtIndex(_targetSelection).ToggleTargetIcon());
    }

    void AdjustTargetSelectorHelper(bool increased = true) {
        _targetSelection += increased ? 1 : -1;
        if (_targetSelection >= BattleController.Instance.AmountOfEntities()) {
            _targetSelection = 0;
        }
        else if (_targetSelection < 0) {
            _targetSelection = BattleController.Instance.AmountOfEntities() - 1;
        }
    }

    public void ToggleButtons(bool activate = false, BattleEntity entity = null) {
        for (int i = 0; i < _buttons.Length; i++) {
            _buttons[i].interactable = activate;
        }
        _currentEntity = entity;
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
        ToggleTargetSelector(true, BattleController.Instance.EnemyUnit);
    }

    public void SkillsButton() {

    }

    public void ItemButton() {

    }

    public void DefendButton() {
        _currentEntity.EndTurn();
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
