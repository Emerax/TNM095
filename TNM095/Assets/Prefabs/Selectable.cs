using UnityEngine;

/// <summary>
/// Used to simply selecting a single multi-collider game object using raycast detection.
/// </summary>
public class Selectable : MonoBehaviour {
    /// <summary>
    /// The <see cref="Capturable"/> this collider is attached to.
    /// </summary>
    public Capturable select;
}
