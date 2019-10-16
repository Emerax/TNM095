using UnityEngine;

public class Player : MonoBehaviour {
    public Color playerColor;
    public bool isMLControlled;

    private int maxUnits;

    public void UpdateMaxUnits(int maxUnitsChanged){
      maxUnits += maxUnitsChanged;
    }

    public void Reset() {
        maxUnits = 0;
    }
}
