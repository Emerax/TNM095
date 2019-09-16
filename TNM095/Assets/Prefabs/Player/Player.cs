using UnityEngine;

public class Player : MonoBehaviour {
    public Color playerColor;

    private int maxUnits;

    public void UpdateMaxUnits(int maxUnitsChanged){
      maxUnits += maxUnitsChanged;
    }
}
