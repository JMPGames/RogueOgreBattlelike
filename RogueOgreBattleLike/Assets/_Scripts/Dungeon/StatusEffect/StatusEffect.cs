using UnityEngine;

public class StatusEffect : MonoBehaviour {
    [SerializeField] int _id;
    [SerializeField] string _title;
    [SerializeField] int _minAmount;
    [SerializeField] int _maxAmount;
    [SerializeField] int _minTurns;
    [SerializeField] int _maxTurns;

    public int Amount { get; protected set; }
    public int Turns { get; protected set; }

    BattleEntity _target;

    public int GetId() => _id;
    public string GetTitle() => _title;

    public virtual void Apply(BattleEntity target) {
        Amount = Random.Range(_minAmount, _maxAmount);
        Turns = Random.Range(_minTurns, _maxTurns);
        _target = target;
        _target.AddStatusEffect(this);
    }

    public virtual void Tick() {
        Turns--;
        if (Turns <= 0) {
            _target.RemoveStatusEffect(this);
            StatusEffectPool.Instance.AddToPool(this);
        }
    }
}
