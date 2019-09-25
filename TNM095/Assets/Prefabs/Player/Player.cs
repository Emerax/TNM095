using UnityEngine;

public class Player : MonoBehaviour {
    public Color playerColor;
    public string name;

    private int maxUnits;

    public void UpdateMaxUnits(int maxUnitsChanged){
      maxUnits += maxUnitsChanged;
    }
}
