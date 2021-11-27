using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DungeonLogMessage : MonoBehaviour {
    const float LOG_TTL = 5.0f;
    const float FADE_TIME = 1.0f;
    [SerializeField] Text _text;
    
    public void Show(string message, Color color) {
        StopAllCoroutines();
        _text.text = message;
        _text.color = color;
        _text.enabled = true;
        StartCoroutine(LogFadeOut());
    }

    public void Hide() {
        _text.text = "";
        _text.color = Color.white;
        _text.enabled = false;
    }

    IEnumerator LogFadeOut() {
        yield return new WaitForSeconds(LOG_TTL);
        while (_text.color.a > 0.0f) {
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _text.color.a - (Time.deltaTime / FADE_TIME));
            yield return null;
        }
        DungeonLog.Instance.ActiveToPool(gameObject);
    }
}
