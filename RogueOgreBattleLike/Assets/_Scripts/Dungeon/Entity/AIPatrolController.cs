using UnityEngine;

public class AIPatrolController : MonoBehaviour {
    public (int, int) CalculatePatrolPath(int x, int y, (int, int) moveDirection) {
        (int, int) direction = moveDirection;
        (int, int) position = CalculatePathPositionHelper(x, y, moveDirection);        

        if (MapController.Instance.TileAtPositionIsBlocked(position.Item1, position.Item2)) {
            int temp = direction.Item1;
            direction.Item1 = direction.Item2;
            direction.Item2 = temp;

            position = CalculatePathPositionHelper(x, y, direction);

            if (MapController.Instance.TileAtPositionIsBlocked(position.Item1, position.Item2)) {
                direction.Item1 *= -1;
                direction.Item2 *= -1;

                position = CalculatePathPositionHelper(x, y, direction);

                if (MapController.Instance.TileAtPositionIsBlocked(position.Item1, position.Item2)) {
                    temp = direction.Item1;
                    direction.Item1 = direction.Item2;
                    direction.Item2 = temp;
                }
            }
        }
        return direction;
    }

    public (int, int) RandomPatrol() {
        int r = Random.Range(1, 101);
        (int, int) test = r > 50 ? (RandomPatrolHelper(), 0) : (0, RandomPatrolHelper());

        return r > 50 ? (RandomPatrolHelper(), 0) : (0, RandomPatrolHelper());
    }

    (int, int) CalculatePathPositionHelper(int x, int y, (int, int) direction) {
        return (x + direction.Item1, y + direction.Item2);
    }

    int RandomPatrolHelper() {
        return Random.Range(1, 101) > 50 ? 1 : -1;
    }
}
