using UnityEngine;

public class Player : MonoBehaviour {
    public int maxUnits = 0;

    public Material playerMaterial;
    public Color playerColor;

    void Start(){

    }

    void Update(){

    }

    public void updateMaxUnits(int maxUnitsChanged){
      maxUnits += maxUnitsChanged;
    }
}
