using System.Collections.Generic;
using UnityEngine;

public class DungeonLog : MonoBehaviour {
    public static DungeonLog Instance;
    const int MAX_LOG_INSTANCES = 6;

    [SerializeField] GameObject _logPrefab;
    [SerializeField] RectTransform _logContentParent;

    List<GameObject> _activeLogsList = new List<GameObject>();
    List<GameObject> _logPoolList = new List<GameObject>();

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        for (int i = 0; i < MAX_LOG_INSTANCES; i++) {
            _logPoolList.Add(Instantiate(_logPrefab, Vector2.zero, Quaternion.identity, transform) as GameObject);
        }
    }

    public void CreateLog(string message, Color color) {
        if (_activeLogsList.Count >= MAX_LOG_INSTANCES) {
            ActiveToPool(_activeLogsList[0]);
        }
        DungeonLogMessage log = _logPoolList[0].GetComponent<DungeonLogMessage>();
        _logPoolList.RemoveAt(0);
        log.transform.SetParent(_logContentParent);
        log.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        log.Show(message, color);
        _activeLogsList.Add(log.gameObject);
    }

    public void ActiveToPool(GameObject log) {
        log.GetComponent<DungeonLogMessage>().Hide();
        log.transform.SetParent(transform);
        _logPoolList.Add(log);
        _activeLogsList.Remove(log);
    }
}
