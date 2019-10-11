using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public List<Capturable> capturables;
    public List<Raid> raids;
    public List<BaseController> controllers;
    public Dictionary<Player, float> unitPercent = new Dictionary<Player, float>();
    public Canvas sceneCanvas;

    void Start() {
        capturables = new List<Capturable>(FindObjectsOfType<Capturable>());
        raids = new List<Raid>();
        controllers = new List<BaseController>(FindObjectsOfType<BaseController>());
        sceneCanvas = FindObjectOfType<Canvas>();

        foreach (BaseController controller in controllers) {
          unitPercent.Add(controller.player, 0);
        }
    }

    void Update() {
      updatePercent();
      foreach (KeyValuePair<Player, float> pair in unitPercent) {
          foreach (PlayerBar bar in sceneCanvas.bars) {
              if (bar.owner == pair.Key) {
                bar.ReSize(pair.Value);
              }
          }
      }
    }

    private void updatePercent() {
      Dictionary<Player, int> playerUnitCount = new Dictionary<Player, int>();
      int total = 0;

      foreach (BaseController controller in controllers) {
        int capCount = capturables.Where(c => c.owner == controller.player).Select(c => c.unitCount).Sum();
        int raidCount = raids.Where(r => r.owner == controller.player).Select(r => r.unitCount).Sum();
        playerUnitCount[controller.player] =  capCount + raidCount;
        total += capCount + raidCount;
      }

      var keys = unitPercent.Keys.ToList();

      foreach (Player player in keys) {
        if (playerUnitCount.ContainsKey(player) && total != 0) {
          unitPercent[player] = playerUnitCount[player]/(float)total;
        } else {
          unitPercent[player] = 0;
        }
      }
    }

    private void onVictory(BaseController winner) {
        Debug.Log(winner.player.name + " is our winner!");
        winner.OnWin();
        controllerRemoved(winner);
    }

    public void raidCreated(Raid raid) {
        raids.Add(raid);
    }

    public void raidRemoved(Raid raid) {
        raids.Remove(raid);
    }

    public void controllerRemoved(BaseController controller) {
        controllers.Remove(controller);
        if(controllers.Count == 1) {
          BaseController winner = controllers[0];
          onVictory(winner);
        }
    }

}
