using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeuristicController : BaseController {

    public float actionCooldown;
    private float currentActionCooldown = 0;
    private enum State {CONQUER, DEFEND, REINFORCE, EXPAND, GROW, EMPTY, WEAKEN};
    private State currState;
    private GameState gameState;

    void Start() {
      gameState = FindObjectOfType<GameState>();
      currState = State.GROW;
    }

    void Update() {
      if(currentActionCooldown <= 0) {
        SelectState();
        ChangeState();
        currentActionCooldown = actionCooldown;
      } else {
          currentActionCooldown -= Time.deltaTime;
      }
    }

    // Selects the next state
    private void SelectState() {
      List<Capturable> ownCaps = gameState.capturables.Where(c => c.owner == player).ToList();
      List<Capturable> neutralCaps = gameState.capturables.Where(c => c.owner == null).ToList();

      if (neutralCaps.Count > 0) {
        currState = State.EXPAND;
      } else {
        currState = State.GROW;
      }
    }

    // Changes state into the selected one
    private void ChangeState() {
        switch (currState)
        {
          case State.CONQUER:
              Debug.Log("Conquering...");
              Conquer();
              break;
          case State.DEFEND:
              Debug.Log("Defending...");
              Defend();
              break;
          case State.REINFORCE:
              Debug.Log("Reinforcing...");
              Reinforce();
              break;
          case State.EXPAND:
              Debug.Log("Expanding...");
              Expand();
              break;
          case State.GROW:
              Debug.Log("Growing...");
              Grow();
              break;
          case State.EMPTY:
              Debug.Log("Emptying...");
              Empty();
              break;
          case State.WEAKEN:
              Debug.Log("Weakening...");
              Weaken();
              break;
          default:
              Debug.Log("You have entered a state that is not possible");
              break;
        }
    }

    // Attack with the intent to make the structure once own
    private void Conquer() {

    }

    // Send troops to a friendly structure to avoid that it is taken over
    private void Defend() {

    }

    // Move units to a building to increase their unit count
    private void Reinforce() {

    }

    // Take over neutral buildings
    private void Expand() {
      List<Capturable> neutralCaps = gameState.capturables.Where(c => c.owner == null  && !(c is Tower)).ToList();
      List<Capturable> neutralTowers = gameState.capturables.Where(c => c.owner == null && c is Tower).ToList();
      List<Capturable> ownCaps = gameState.capturables.Where(c => c.owner == player && !(c is Tower)).ToList();
      float smallestDist = Mathf.Infinity;
      Capturable origin = null;
      Capturable destination = null;


      List<Capturable> sixtyPlusCaps = gameState.capturables.Where(c => c.owner == player && c.unitCount > 60).ToList();
      if (neutralTowers.Count > 0 && sixtyPlusCaps.Count > 0) {
        neutralCaps = neutralTowers;
        ownCaps = sixtyPlusCaps;
      }

      if (neutralCaps.Count > 0) {
        foreach (var ownCap in ownCaps) {
          foreach (var neutralCap in neutralCaps) {
            float tempDist = Vector3.Distance(ownCap.transform.position, neutralCap.transform.position);
            if (tempDist < smallestDist) {
                smallestDist = tempDist;
                origin = ownCap;
                destination = neutralCap;
            }
          }
        }
      }

      if (origin != null && destination != null) {
          origin.BeginRaid(destination);
      }
    }

    // Do nothing for a while and grow units
    private void Grow() {
    }

    // Move units from a building because it is at max capacity
    private void Empty() {

    }

    // Attack with the intent to lower a buildings unit count
    private void Weaken() {

    }
}
