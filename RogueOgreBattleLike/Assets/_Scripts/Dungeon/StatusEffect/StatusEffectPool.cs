using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectPool : MonoBehaviour {
    public static StatusEffectPool Instance;

    public GameObject[] AllStatusEffects;
    List<StatusEffect> _pool = new List<StatusEffect>();

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }
    }

    public StatusEffect PullFromPoolOrCreate(int id) {
        StatusEffect statusEffect = _pool.FirstOrDefault(se => se.GetId() == id);
        if (statusEffect == null) {
            statusEffect = (Instantiate(AllStatusEffects[id], transform) as GameObject).GetComponent<StatusEffect>();
        }
        else {
            _pool.Remove(statusEffect);
        }
        return statusEffect;
    }

    public void AddToPool(StatusEffect se) {
        _pool.Add(se);
    }
}
