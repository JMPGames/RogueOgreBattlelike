using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIPathfinding))]
[RequireComponent(typeof(AIPatrolController))]
public class EnemyUnit : BattleUnit {
    const int RandomPatrolChance = 30;
    const float LoseSightModifier = 1.5f;

    [SerializeField] float sightRange;
    AIPathfinding pathFinding;
    AIPatrolController patrolController;
    Transform target;
    bool targetSighted;
    (int, int) lastMove;

    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        pathFinding = GetComponent<AIPathfinding>();
        patrolController = GetComponent<AIPatrolController>();
        IsPlayer = false;
        lastMove = (0, 1);
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
        float distance = Vector3.Distance(transform.position, target.position);
        if (!targetSighted && distance <= sightRange) {
            targetSighted = true;
        }
        else if (targetSighted && distance > sightRange * LoseSightModifier) {
            targetSighted = false;
        }
        return targetSighted;
    }

    void MoveTowardsTarget() {
        BattleUnit playerUnit = target.GetComponent<BattleUnit>();
        Tile path = pathFinding.Path(X, Y, playerUnit.X, playerUnit.Y);
        if (path != null) {
            lastMove = (path.X - X, path.Y - Y);
            MoveInDirection(lastMove.Item1, lastMove.Item2);
        }
        else {
            PatrolMovement();
        }
    }

    void PatrolMovement() {
        bool calculatedPatrol = Random.Range(1, 101) > RandomPatrolChance;
        (int, int) direction = calculatedPatrol ? patrolController.CalculatePatrolPath(X, Y, lastMove) : patrolController.RandomPatrol();
        lastMove = direction;
        MoveInDirection(direction.Item1, direction.Item2);
    }
}
