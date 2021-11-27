using UnityEngine;

public class DungeonCamera : MonoBehaviour {
    const float SPEED = 2.0f;
    Transform _player;

    void Start() {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        float smoothing = SPEED * Time.deltaTime;
        Vector3 position = transform.position;

        position.x = Mathf.Lerp(transform.position.x, _player.position.x, smoothing);
        position.y = Mathf.Lerp(transform.position.y, _player.position.y, smoothing);

        transform.position = position;
    }
}
