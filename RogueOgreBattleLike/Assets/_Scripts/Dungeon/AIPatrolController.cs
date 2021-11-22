using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrolController : MonoBehaviour {
    public (int, int) CalculatePatrolPath(int x, int y, (int, int) moveDirection) {
        (int, int) path = moveDirection;

        (int, int) x_y = CalculatePathPositionHelper(x, y, path);        

        if (MapController.instance.TileAtPositionIsBlocked(x_y.Item1, x_y.Item2)) {
            int temp = path.Item1;
            path.Item1 = path.Item2;
            path.Item2 = temp;

            x_y = CalculatePathPositionHelper(x, y, path);

            if (MapController.instance.TileAtPositionIsBlocked(x_y.Item1, x_y.Item2)) {
                path.Item1 *= -1;
                path.Item2 *= -1;

                x_y = CalculatePathPositionHelper(x, y, path);

                if (MapController.instance.TileAtPositionIsBlocked(x_y.Item1, x_y.Item2)) {
                    temp = path.Item1;
                    path.Item1 = path.Item2;
                    path.Item2 = temp;
                    Debug.Log($"MOVING DOWN {path}");
                }
                else {
                    Debug.Log($"MOVING LEFT {path}");
                }
            }
            else {
                Debug.Log($"MOVING RIGHT {path}");
            }
        }
        else {
            Debug.Log($"MOVING UP {path}");
        }
        Debug.Log($"CALCPATROL = {path}");
        return path;
    }

    (int, int) CalculatePathPositionHelper(int x, int y, (int, int) path) {
        return (x + path.Item1, y + path.Item2);
    }

    public (int, int) RandomPatrol() {
        int r = Random.Range(1, 101);
        (int, int) test = r > 50 ? (RandomPatrolHelper(), 0) : (0, RandomPatrolHelper());
        Debug.Log($"RANDOMPATROL = {test}");
        return r > 50 ? (RandomPatrolHelper(), 0) : (0, RandomPatrolHelper());
    }

    int RandomPatrolHelper() {
        int rand = Random.Range(1, 101);
        return rand > 50 ? 1 : -1;
    }
}
