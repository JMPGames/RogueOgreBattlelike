using System.Collections.Generic;
using UnityEngine;

public class DungeonLog : MonoBehaviour {
    public static DungeonLog instance;
    const int maxLogInstances = 6;

    [SerializeField] GameObject logPrefab;
    [SerializeField] RectTransform logContentParent;

    List<GameObject> activeLogs = new List<GameObject>();
    List<GameObject> logPool = new List<GameObject>();

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        for (int i = 0; i < maxLogInstances; i++) {
            logPool.Add(Instantiate(logPrefab, Vector2.zero, Quaternion.identity, transform) as GameObject);
        }
    }

    public void CreateLog(string message, Color color) {
        if (activeLogs.Count >= maxLogInstances) {
            ActiveToPool(activeLogs[0]);
        }
        DungeonLogMessage log = logPool[0].GetComponent<DungeonLogMessage>();
        logPool.RemoveAt(0);
        log.transform.SetParent(logContentParent);
        log.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        log.Show(message, color);
        activeLogs.Add(log.gameObject);
    }

    public void ActiveToPool(GameObject log) {
        log.GetComponent<DungeonLogMessage>().Hide();
        log.transform.SetParent(transform);
        logPool.Add(log);
        activeLogs.Remove(log);
    }
}
