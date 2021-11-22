using UnityEngine;

public class DungeonCamera : MonoBehaviour {
    const float Speed = 2.0f;
    Transform player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        float interp = Speed * Time.deltaTime;
        Vector3 position = transform.position;

        position.x = Mathf.Lerp(transform.position.x, player.position.x, interp);
        position.y = Mathf.Lerp(transform.position.y, player.position.y, interp);

        transform.position = position;
    }
}
