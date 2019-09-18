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

    public Raid raidPrefab;

    /// <summary>
    /// Current units in this <see cref="Capturable"/>
    /// </summary>
    public int unitCount;

    void Start() {
        Colorize();

        unitCount = startingUnits;

        selectionIndicator.gameObject.SetActive(false);

        if (owner != null) {
            owner.UpdateMaxUnits(unitCap);
            unitIndicator.UpdateText(unitCount.ToString(), owner);
        } else {
            unitIndicator.UpdateText(unitCount.ToString(), owner);
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

    public void UnitsArrive(Raid raid) {
        if (raid.owner == owner) {
            unitCount += raid.unitCount;
        } else {
            if (raid.unitCount > unitCount) {
                ChangeOwner(raid.owner);
            } else if (raid.unitCount == unitCount){
              ChangeOwner(null);
            }
            unitCount = Mathf.Abs(unitCount - raid.unitCount);
        }
        unitIndicator.UpdateText(unitCount.ToString(), owner);
        Destroy(raid.gameObject);
    }

    public void BeginRaid(Capturable target) {
        int raidCount = unitCount / 2;
        if(raidCount > 0 && target != this && target.unitCount < target.unitCap) {
            unitCount -= raidCount;

            Vector3 targetVector = target.transform.position - transform.position;
            Raid raid = Instantiate(raidPrefab, transform.position, Quaternion.LookRotation(targetVector, Vector3.up));

            raid.Init(owner, target, raidCount);
            unitIndicator.UpdateText(unitCount.ToString(), owner);
        }
    }

    private void ChangeOwner(Player newOwner) {
        if (owner != null) {
            owner.UpdateMaxUnits(-unitCap);
        }
        owner = newOwner;
        if (owner != null) {
          owner.UpdateMaxUnits(unitCap);
        }
        Colorize();
    }

    private void Colorize() {
        Color newColor = Color.gray;
        if (owner != null) {
            newColor = owner.playerColor;
        }
        foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>()) {
            r.material.color = newColor;
        }
    }
}
