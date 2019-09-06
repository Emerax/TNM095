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
    public readonly int startingUnits;

    /// <summary>
    /// <see cref="UnitIndicator"/> used to display number of units within this <see cref="Capturable"/>
    /// </summary>
    public UnitIndicator indicator;

    private int unitCount;

    void Start()
    {
        unitCount = startingUnits;

        if (owner != null)
        {
            foreach (Renderer r in ownerIndicators)
            {
                r.material.color = owner.playerColor;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
