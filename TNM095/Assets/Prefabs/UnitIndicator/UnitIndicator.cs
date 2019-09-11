﻿using TMPro;
using UnityEngine;

public class UnitIndicator : MonoBehaviour {
    public TextMeshPro textRenderer;

    public void UpdateText(string text, Color color) {
        textRenderer.color = color;
        textRenderer.text = text;
    }
}