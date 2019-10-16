using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerformanceUI : MonoBehaviour
{
    public List<PlayerBar> bars;
    public PlayerBar playerBarPrefab;

    void Start() {
      bars = new List<PlayerBar>();
      var xPos = transform.position.x - GetComponent<RectTransform>().rect.width/2;
      var yPos = transform.position.y + GetComponent<RectTransform>().rect.height/2;
      int pad = 15;

      foreach (Player player in new List<Player>(FindObjectsOfType<Player>())) {
        PlayerBar playerBar = Instantiate<PlayerBar>(playerBarPrefab, new Vector3(xPos + pad, yPos - pad, transform.position.z), transform.rotation, transform);
        playerBar.Init(player);
        bars.Add(playerBar);
        yPos -= 24 + pad;
      }
    }
}
