using System.Collections;
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

    private int unitCount;

    void Start()
    {
        if (owner != null)
        {
            foreach (Renderer r in ownerIndicators)
            {
                r.material = owner.playerMaterial;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
