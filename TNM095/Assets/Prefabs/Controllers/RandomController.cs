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
                    Capturable target = capturables[Random.Range(0, capturables.Count)];
                    selected.BeginRaid(target);
                } else {
                    Debug.Log("YOU WIN, WELL PLAYED HUMAN!");
                    Destroy(gameObject);
                }
            }

            currentActionCooldown = actionCooldown;
        } else {
            currentActionCooldown -= Time.deltaTime;
        }
    }
}
