using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIPathfinding))]
[RequireComponent(typeof(AIPatrolController))]
public class EnemyUnit : BattleUnit {
    const int RANDOM_PATROL_CHANCE = 30;
    const float LOSE_SIGHT_MODIFIER = 1.5f;

    [SerializeField] float _sightRange;
    AIPathfinding _pathFinding;
    AIPatrolController _patrolController;
    Transform _target;
    bool _targetSighted;
    (int, int) _lastMove;

    void Start() {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _pathFinding = GetComponent<AIPathfinding>();
        _patrolController = GetComponent<AIPatrolController>();
        IsPlayer = false;
        _lastMove = (0, 1);
    }

    public override void StartTurn() {
        base.StartTurn();
        
        if (CheckForTarget()) {
            MoveTowardsTarget();
        }
        else {
            PatrolMovement();
        }
    }

    bool CheckForTarget() {
        float distance = Vector3.Distance(transform.position, _target.position);
        if (!_targetSighted && distance <= _sightRange) {
            DungeonLog.Instance.CreateLog($"{name} ({X}, {Y}) has spotted you.", Color.yellow);
            _targetSighted = true;
        }
        else if (_targetSighted && distance > _sightRange * LOSE_SIGHT_MODIFIER) {
            DungeonLog.Instance.CreateLog($"{name} ({X}, {Y}) has lost sight of you.", Color.yellow);
            _targetSighted = false;
        }
        return _targetSighted;
    }

    void MoveTowardsTarget() {
        BattleUnit playerUnit = _target.GetComponent<BattleUnit>();
        Tile path = _pathFinding.Path(X, Y, playerUnit.X, playerUnit.Y);
        if (path != null) {
            _lastMove = (path.X - X, path.Y - Y);
            MoveInDirection(_lastMove.Item1, _lastMove.Item2);
        }
        else {
            PatrolMovement();
        }
    }

    void PatrolMovement() {
        bool calculatedPatrol = Random.Range(1, 101) > RANDOM_PATROL_CHANCE;
        (int, int) direction = calculatedPatrol ? _patrolController.CalculatePatrolPath(X, Y, _lastMove) : _patrolController.RandomPatrol();
        _lastMove = direction;
        MoveInDirection(direction.Item1, direction.Item2);
    }
}
