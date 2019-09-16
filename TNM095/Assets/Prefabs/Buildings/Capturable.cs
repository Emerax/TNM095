using System.Collections.Generic;
using UnityEngine;

public class Capturable : MonoBehaviour {
    /// <summary>
    /// <see cref="Player"/> instance that currently owns this <see cref="Capturable"/>.
    /// </summary>
    public Player owner;

    /// <summary>
    /// List of Renderers that should change their material to that of the owning player to indicate ownership¨.
    /// </summary>
    public List<Renderer> ownerIndicators;

    /// <summary>
    /// Number of units that begin stationed in this capturable.
    /// </summary>
    public int startingUnits;

    /// <summary>
    /// Maximum number of units that can be held by this <see cref="Capturable"/>
    /// </summary>
    public int unitCap;

    /// <summary>
    /// <see cref="UnitIndicator"/> used to display number of units within this <see cref="Capturable"/>
    /// </summary>
    public UnitIndicator unitIndicator;

    /// <summary>
    /// <see cref="SelectionIndicator"/> used to display if this particular Capturable is selected by a Controller.
    /// </summary>
    public SelectionIndicator selectionIndicator;

    /// <summary>
    /// Current units in this <see cref="Capturable"/>
    /// </summary>
    protected int unitCount;

    void Start() {
        unitCount = startingUnits;

        selectionIndicator.gameObject.SetActive(false);

        if (owner != null) {
          unitIndicator.UpdateText(unitCount.ToString(), owner.playerColor);
        }
        else {
          unitIndicator.UpdateText(unitCount.ToString(), Color.grey);
        }

        if (owner != null) {
            foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>()) {
                r.material.color = owner.playerColor;
            }
        }
    }

    // Update is called once per frame
    void Update() {
    }

    public void OnSelected() {
        selectionIndicator.gameObject.SetActive(true);
        selectionIndicator.SetColor(owner.playerColor);
    }

    public void OnDeselected() {
        selectionIndicator.gameObject.SetActive(false);
    }
}
