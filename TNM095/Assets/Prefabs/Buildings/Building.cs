using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Subtype of <see cref="Capturable"/> that periodically produces new units for its owner.
/// </summary>
public class Building : Capturable {
    /// <summary>
    /// Number of new units produced each cycle.
    /// </summary>
    public int baseUnitProduction;

    /// <summary>
    /// How many bonus units are produced based on currently held units.
    /// </summary>
    public float unitProductionModifier;

    /// <summary>
    /// Seconds until new units are created.
    /// </summary>
    public float unitProductionTime;

    private float unitProductionProgress = 0;

    private void Update() {
        if(owner != null) {
            UpdateUnitProduction();
        }
    }

    private void UpdateUnitProduction() {
        unitProductionProgress += Time.deltaTime;

        if(unitProductionProgress >= unitProductionTime) {
            ProduceUnits();
            unitProductionProgress = 0;
        }
    }

    private void ProduceUnits() {
        if(unitCount < unitCap) {
            unitCount = Mathf.Min(unitCount + baseUnitProduction + Mathf.FloorToInt(unitProductionModifier * unitCount), unitCap);
            unitIndicator.UpdateText(trainingID + ": (" + unitCount.ToString() + ")", owner);
        }
    }
}
