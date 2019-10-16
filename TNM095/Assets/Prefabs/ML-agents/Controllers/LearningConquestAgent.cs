using MLAgents;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LearningConquestAgent : Agent {
    public Player player;
    public GameBoard board;

    public float actionCooldown;
    private float currentActionCooldown = 0;

    private int previousCapturables;

    public override void CollectObservations() {
        base.CollectObservations();
        List<Capturable> capturables = board.capturables;
        List<Raid> raids = board.raids;
        //Number of own units held in each Capturable.
        IEnumerable<float> unitCounts = capturables.Select(c => {
            if (c.owner == player) {
                return c.unitCount;
            }
            return 0f;
        });
        AddVectorObs(unitCounts); //Increases observations by 13.

        //Number of enemy units held in each capturable.
        IEnumerable<float> enemyUnitCounts = capturables.Select(c => {
            if(c.owner != player) {
                return c.unitCount;
            }
            return 0f;
        });
        AddVectorObs(enemyUnitCounts); //Increases observations by 13.

        //Number of own units in raids targeting each Capturable
        IEnumerable<float> ownRaids = capturables.Select(c => (float)raids.Where(r => r.owner == player).Where(r => r.dest == c).Sum(r => r.unitCount));
        AddVectorObs(ownRaids); //Increases observations by 13.

        //Number of enemy units in raids targeting each Capturable
        IEnumerable<float> enemyRaids = capturables.Select(c => (float)raids.Where(r => r.owner != player).Where(r => r.dest == c).Sum(r => r.unitCount));
        AddVectorObs(enemyRaids); //Increases observations by 13.

        //Total observations: 52.
    }

    public override void AgentAction(float[] vectorAction, string textAction) {
        if(currentActionCooldown <= 0) {
            base.AgentAction(vectorAction, textAction);
            //0-12: index of capturable to select.
            //13: do nothing this step.
            int selectedIndex = (int)vectorAction[0];
            //0-12 index of building to target.
            int targetIndex = (int)vectorAction[1];

            if (selectedIndex != 13) {
                Capturable selected = board.capturables[selectedIndex];
                if (selected.owner == player) {
                    Capturable target = board.capturables[targetIndex];
                    if (selected == target || (target.owner == player && target.unitCount >= target.unitCap)) {
                        //No-op move, penalize.
                        AddReward(-1.0f);
                    }
                    selected.BeginRaid(target);
                } else {
                    //Illegal move, penalize.
                    AddReward(-1.0f);
                }
            } else {
                //Discourage doing nothing for too long.
                AddReward(-0.03f);
            }

            int capturablesOwned = board.capturables.Count(c => c.owner == player);
            if (capturablesOwned > previousCapturables) {
                //Agent has captured capturables.
                AddReward(1.0f);
            } else if (capturablesOwned < previousCapturables) {
                //The agent has lost capturables.
                AddReward(-1.0f);
            }

            previousCapturables = capturablesOwned;

            //Check if we have won, or lost.
            if (board.capturables.All(c => c.owner == player) && !board.raids.Any(r => r.owner != player)) {
                //Win, extra reward.
                AddReward(1.0f);
                Done();
            } else if (!board.capturables.Any(c => c.owner == player) && !board.raids.Any(r => r.owner == player)) {
                //Defeat, extra penalty.
                AddReward(-1.0f);
                Done();
            }
            currentActionCooldown = actionCooldown;
        } else {
            currentActionCooldown -= Time.deltaTime;
        }
    }
}
