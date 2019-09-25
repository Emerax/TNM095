using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomController : BaseController {
    /// <summary>
    /// [0.0, 0.1] Chance to do nothing whenever an action is taken.
    /// </summary>
    public float noActionChance;

    /// <summary>
    /// Time between actions in seconds.
    /// </summary>
    public float actionCooldown;
    private float currentActionCooldown = 0;

    private readonly List<Capturable> capturables = new List<Capturable>();

    void Start() {
        foreach (var capturable in FindObjectsOfType<Capturable>()) {
            capturables.Add(capturable);
        }
    }

    // Update is called once per frame
    void Update() {
        if(currentActionCooldown <= 0) {
            if (Random.value < noActionChance) {
                List<Capturable> own = capturables.Where(c => c.owner == player).ToList();

                if (own.Count > 0) {
                    Capturable selected = own[Random.Range(0, own.Count)];
                    List<Capturable> targets = capturables.Where(c => c != selected && (c.owner != player || c.unitCount < c.unitCap)).ToList();
                    Capturable target = targets[Random.Range(0, targets.Count)];
                    selected.BeginRaid(target);
                } else {
                    if (new List<Raid>(FindObjectsOfType<Raid>()).Where(r => r.owner == player).ToList().Count == 0) {
                      OnLose();
                    }
                }
            }

            currentActionCooldown = actionCooldown;
        } else {
            currentActionCooldown -= Time.deltaTime;
        }
    }
}
