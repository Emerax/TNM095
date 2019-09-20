using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : Capturable {
    public TargettingCylinder targettingCircle;
    public float targettingCircleAlpha;

    public int damage;
    public float arrowCooldown;

    public Arrow arrowPrefab;

    private float currentCooldown = 0;

    // Update is called once per frame
    void Update() {
        if(currentCooldown <= 0) {
            if(targettingCircle.currentColliders.Count > 0) {
                List<Raid> enemyRaids = targettingCircle.currentColliders.Where(r => r.owner != owner).ToList();
                if(enemyRaids.Count > 0) {
                    foreach(Raid r in enemyRaids) {
                        Arrow arrow = Instantiate<Arrow>(arrowPrefab, transform.position + Vector3.up, Quaternion.identity);
                        arrow.Init(r, damage);
                    }
                }
            }
            currentCooldown = arrowCooldown;
        }

        currentCooldown -= Time.deltaTime;
    }

    protected override void Colorize() {
        base.Colorize();
        if (owner != null) {
            Color targettingCircleColor = owner.playerColor;
            targettingCircleColor.a = targettingCircleAlpha;
            targettingCircle.GetComponent<Renderer>().material.color = targettingCircleColor;
        }
    }
}
