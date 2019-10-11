using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeuristicController : BaseController {

    public float actionCooldown;
    private float currentActionCooldown = 0;
    private enum State {ATTACK, DEFEND, REINFORCE, EXPAND, GROW, EMPTY};
    private State currState;
    private GameState gameState;
    private readonly List<Capturable> capturables = new List<Capturable>();

    void Start() {
      foreach (var capturable in FindObjectsOfType<Capturable>()) {
          capturables.Add(capturable);
      }
      gameState = FindObjectOfType<GameState>();
      currState = State.GROW;
    }

    void Update() {
      if(currentActionCooldown <= 0) {
        if (capturables.Where(c => c.owner == player).ToList().Count == 0) {
          if (new List<Raid>(FindObjectsOfType<Raid>()).Where(r => r.owner == player).ToList().Count == 0) {
            OnLose();
          }
        }

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
      List<Raid> conqHostileRaids = gameState.raids.Where(r => ownCaps.Contains(r.dest) && r.owner != player && r.unitCount > r.dest.unitCount && r.dest.unitCount != r.dest.unitCap).ToList();
      int currentUnits = 0;
      int currentMaxUnits = 0;

      foreach (Capturable cap in ownCaps) {
        currentUnits += cap.unitCount;
        currentMaxUnits += cap.unitCap;
      }

      if (neutralCaps.Count > 0) {
        currState = State.EXPAND;
      } else if (conqHostileRaids.Count > 0) {
        currState = State.DEFEND;
      } else if (currentUnits >= 0.7 * currentMaxUnits) {
        currState = State.EMPTY;
      } else {
        currState = State.GROW;
      }
    }

    // Changes state into the selected one
    private void ChangeState() {
        switch (currState) {
          case State.DEFEND:
              Defend();
              break;
          case State.EXPAND:
              Expand();
              break;
          case State.GROW:
              Grow();
              break;
          case State.EMPTY:
              Empty();
              break;
          default:
              Debug.Log("You have entered a state that is not possible");
              break;
        }
    }

    // Send troops to a friendly structure to avoid that it is taken over
    private void Defend() {
      List<Capturable> ownCaps = gameState.capturables.Where(c => c.owner == player).ToList();
      List<Raid> conqHostileRaids = gameState.raids.Where(r => ownCaps.Contains(r.dest) && r.unitCount > r.dest.unitCount && r.dest.unitCount != r.dest.unitCap).ToList();
      if(conqHostileRaids != null) {
        Raid hostileRaid = conqHostileRaids[0];
        int backupCount = hostileRaid.unitCount - hostileRaid.dest.unitCount;

        List<Capturable> backupBuildings = gameState.capturables.Where(c => c.owner == player && c.unitCount >= backupCount*2 && c is Building).ToList();
        List<Capturable> inReachBuildings = backupBuildings.Where(c => Vector3.Distance(hostileRaid.transform.position, hostileRaid.dest.transform.position) > Vector3.Distance(c.transform.position, hostileRaid.dest.transform.position)).ToList();
        if (inReachBuildings.Count > 0){
          inReachBuildings[0].BeginRaid(hostileRaid.dest);
        }
      }
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
      int rand  = Random.Range(0, 4);

      if (rand < 1) {
        Reinforce();
      } else {
        Attack();
      }
    }

    // Move units to a building to increase their unit count
    private void Reinforce() {
      List<Capturable> fullCaps = gameState.capturables.Where(c => c.owner == player && c.unitCount >= c.unitCap).ToList();
      List<Capturable> noneFullCaps = gameState.capturables.Where(c => c.owner == player && c.unitCount < c.unitCap).ToList();
      Capturable weakestCap = null;
      float smallestUnitCount = Mathf.Infinity;
      Capturable supportCap = null;
      float smallestDist = Mathf.Infinity;

      foreach (Capturable cap in noneFullCaps) {
        if(cap.unitCount < smallestUnitCount) {
          weakestCap = cap;
          smallestUnitCount = cap.unitCount;
        }
      }

      foreach (Capturable cap in fullCaps) {
        float tempDist = Mathf.Infinity;
        if (weakestCap != null) {
          tempDist = Vector3.Distance(weakestCap.transform.position, cap.transform.position);
        }

        if (tempDist < smallestDist) {
            smallestDist = tempDist;
            supportCap = cap;
        }
      }

      if (supportCap != null && weakestCap != null) {
          supportCap.BeginRaid(weakestCap);
      }
    }

    // Attacking a hostile structure
    private void Attack() {
      List<Capturable> hostileCaps = gameState.capturables.Where(c => c.owner != player).ToList();
      Capturable weakestCap = null;
      float smallestUnitCount = Mathf.Infinity;
      Capturable attackCap = null;
      float largestUnitCount = 0;

      foreach (Capturable hostileCap in hostileCaps) {
        if(hostileCap.unitCount < smallestUnitCount) {
          weakestCap = hostileCap;
          smallestUnitCount = hostileCap.unitCount;
        }
      }

      List<Capturable> attackCaps = gameState.capturables.Where(c => c.owner == player && !(c is Tower)).ToList();

      foreach (Capturable cap in attackCaps) {
        if(cap.unitCount > largestUnitCount) {
          attackCap = cap;
          largestUnitCount = cap.unitCount;
        }
      }

      if (attackCap != null && weakestCap != null) {
          attackCap.BeginRaid(weakestCap);
      }
    }
}
