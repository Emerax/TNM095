using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargettingCylinder : MonoBehaviour {
    public readonly List<Raid> currentColliders = new List<Raid>();

    private void OnTriggerEnter(Collider other) {
        Raid r = other.gameObject.GetComponent<Raid>();
        if(r != null) {
            currentColliders.Add(r);
        }
    }

    private void OnTriggerExit(Collider other) {
        Raid r = other.gameObject.GetComponent<Raid>();
        if (r != null) {
            if (currentColliders.Contains(r)) {
                currentColliders.Remove(r);
            }
        }
    }


}
