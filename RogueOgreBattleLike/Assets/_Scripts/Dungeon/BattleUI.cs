using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour {
    public static BattleUI Instance;
    [SerializeField] GameObject _battleUIPanel;
    [SerializeField] Text[] _titleTexts;
    [SerializeField] Text[] _healthTexts;
    [SerializeField] Button[] _buttons;

    BattleEntity _currentEntity;

    public void ToggleUI(bool show = true) {
        _battleUIPanel.SetActive(show);
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
