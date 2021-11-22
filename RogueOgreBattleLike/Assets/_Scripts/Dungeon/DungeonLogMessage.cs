using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DungeonLogMessage : MonoBehaviour {
    const float logTTL = 5.0f;
    [SerializeField] Text text;
    
    public void Show(string message, Color color) {
        StopAllCoroutines();
        text.text = message;
        text.color = color;
        text.enabled = true;
        StartCoroutine(LogFadeOut());
    }

    public void Hide() {
        text.text = "";
        text.color = Color.white;
        text.enabled = false;
    }

    IEnumerator LogFadeOut() {
        yield return new WaitForSeconds(logTTL);
        while (text.color.a > 0.0f) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / 1.0f));
            yield return null;
        }
        DungeonLog.instance.ActiveToPool(gameObject);
    }
}
