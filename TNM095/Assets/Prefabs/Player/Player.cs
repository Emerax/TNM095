using UnityEngine;

public class Player : MonoBehaviour {
    public Color playerColor;
    public string playerName;

    private int maxUnits;

    public void UpdateMaxUnits(int maxUnitsChanged){
      maxUnits += maxUnitsChanged;
    }
}
