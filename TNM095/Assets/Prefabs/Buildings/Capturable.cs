using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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

    public int trainingID;

    private GameBoard board;

    void Start() {
        Colorize();

        board = transform.parent.gameObject.GetComponent<GameBoard>();
        Assert.IsNotNull(board);

        selectionIndicator.gameObject.SetActive(false);

        if (owner != null) {
            owner.UpdateMaxUnits(unitCap);
            unitIndicator.UpdateText(trainingID + ": (" + unitCount.ToString() + ")", owner);
        } else {
            unitIndicator.UpdateText(trainingID + ": (" + unitCount.ToString() + ")", owner);
        }
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
            } else if (raid.unitCount == unitCount) {
                ChangeOwner(null);
            }
            unitCount = Mathf.Abs(unitCount - raid.unitCount);
        }
        unitIndicator.UpdateText(trainingID + ": (" + unitCount.ToString() + ")", owner);
        board.RaidRemoved(raid);
    }

    public void BeginRaid(Capturable target) {
        int raidCount = unitCount / 2;
        if (raidCount > 0 && (target.owner != owner || target.unitCount < target.unitCap) && target != this) {
            unitCount -= raidCount;

            Vector3 targetVector = target.transform.position - transform.position;
            Raid raid = Instantiate(raidPrefab, transform.position, Quaternion.LookRotation(targetVector, Vector3.up));

            raid.Init(owner, transform.parent, target, raidCount);
            unitIndicator.UpdateText(trainingID + ": (" + unitCount.ToString() + ")", owner);
        }
    }

    protected virtual void ChangeOwner(Player newOwner) {
        if (owner != null) {
            owner.UpdateMaxUnits(-unitCap);
        }
        Player oldOwner = owner;
        if(oldOwner != null) {
        }
        owner = newOwner;
        if (owner != null) {
            owner.UpdateMaxUnits(unitCap);
        }
        board.CheckWinning();
        Colorize();
    }

    /// <summary>
    /// Forcibly sets the owner of this capturable while avoiding side-effects. Should only be used for initialization.
    /// In any other case, use <see cref="ChangeOwner(Player)"/> instead.
    /// </summary>
    /// <param name="newOwner"></param>
    public void SetOwner(Player newOwner) {
        owner = newOwner;
        if (owner != null) {
            owner.UpdateMaxUnits(unitCap);
        }
        unitIndicator.UpdateText(trainingID + ": (" + unitCount.ToString() + ")", owner);
        Colorize();
    }

    public void Reset() {
        owner = null;
        unitCount = 0;
        unitIndicator.UpdateText(trainingID + ": (" + unitCount.ToString() + ")", owner);
        Colorize();
    }

    protected virtual void Colorize() {
        Color newColor = Color.gray;
        if (owner != null) {
            newColor = owner.playerColor;
        }
        foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>()) {
            r.material.color = newColor;
        }
    }
}
