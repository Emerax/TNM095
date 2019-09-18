using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectionIndicator : MonoBehaviour {
    public string selectionMarker;

    public TextMeshPro textRenderer;

    void Start() {
        textRenderer.text = selectionMarker;
        textRenderer.alpha = 0;
    }

    // Update is called once per frame
    void Update() {
        transform.rotation = Camera.main.transform.rotation;
    }

    public void SetColor(Color color) {
        textRenderer.color = color;
    }
}
