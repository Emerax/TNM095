using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{
    public Player owner;
    public Text txt;
    public Image image;

    private Color boxColor;
    private float originalBoxWidth;

    void Update() {
      image.rectTransform.position = new Vector2(txt.transform.position.x  + (txt.rectTransform.rect.width/2) + (image.rectTransform.rect.width/2), txt.transform.position.y);
    }

    public void Init(Player player) {
      owner = player;
      boxColor = player.playerColor;
      txt.color = boxColor;
      txt.text = player.name;
      originalBoxWidth = image.rectTransform.rect.width;
      image.color = boxColor;
    }

    public void ReSize(float scaler) {
      image.rectTransform.sizeDelta = new Vector2(originalBoxWidth * scaler,txt.rectTransform.rect.height);
    }
}
