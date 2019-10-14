using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Assertions;

public class LearningConquestAgent : Agent {
    public Player player;

    private GameState state;

    // Start is called before the first frame update
    void Start() {
        state = FindObjectOfType<GameState>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public override void CollectObservations() {
        base.CollectObservations();
        List<Capturable> capturables = state.capturables;
        List<Raid> raids = state.raids;
        //Number of own units held in each Capturable.
        IEnumerable<float> unitCounts = capturables.Where(c => c.owner == player).Select(c => (float)c.unitCount);
        AddVectorObs(unitCounts); //Increases observations by 13.

        //Number of enemy units held in each capturable.
        IEnumerable<float> enemyUnitCounts = capturables.Where(c => c.owner != player).Select(c => (float)c.unitCount);
        AddVectorObs(enemyUnitCounts); //Increases observations by 13.

        //Number of own units in raids targeting each Capturable
        IEnumerable<float> ownRaids = raids.Where(r => r.owner == player).Select(r => (float)r.unitCount);
        AddVectorObs(ownRaids); //Increases observations by 13.

        //Number of enemy units in raids targeting each Capturable
        IEnumerable<float> enemyRaids = raids.Where(r => r.owner != player).Select(r => -(float)r.unitCount);
        AddVectorObs(enemyRaids); //Increases observations by 13.
    }

    public override void AgentAction(float[] vectorAction, string textAction) {
        base.AgentAction(vectorAction, textAction);
        //0-12: index of capturable to select.
        //13: do nothing this step.
        int selectedIndex = (int)vectorAction[0];
        //0-12 index of building to target.
        int targetIndex = (int)vectorAction[1];

        if(selectedIndex != 13) {
            Capturable selected = state.capturables[selectedIndex];
            if(selected.owner == player) {
                Capturable target = state.capturables[targetIndex];
                if(target.owner == player && target.unitCount >= target.unitCap) {
                    //No-op move, penalize.
                    AddReward(-0.05f);
                }
                selected.BeginRaid(target);
            } else {
                //Illegal move, penalize.
                AddReward(-0.05f);
            }
        }

        //Gain reward depending on number of capturables owned.
        int capturablesOwned = state.capturables.Count(c => c.owner == player);
        if(capturablesOwned == 13) {
            //The agent has won.
            Done();
        } else if(capturablesOwned < 1) {
            //The agent has lost.
            AddReward(-1.0f);
            Done();
        }

        AddReward(capturablesOwned * 1/13);
    }
}
