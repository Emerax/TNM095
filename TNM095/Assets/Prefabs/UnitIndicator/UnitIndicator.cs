using TMPro;
using UnityEngine;

public class UnitIndicator : MonoBehaviour {
    public TextMeshPro textRenderer;

    public void UpdateText(string text, Player owner) {
      if (owner != null) {
        textRenderer.color = owner.playerColor;
      } else {
        textRenderer.color = Color.white;
      }
        textRenderer.text = text;
    }

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
