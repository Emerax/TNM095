using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public List<Capturable> capturables;
    public List<Raid> raids;
    public List<BaseController> controllers;
    public Dictionary<Player, float> unitPercent = new Dictionary<Player, float>();
    public Canvas sceneCanvas;
    public RandomController randomControllerPrefab;
    public HeuristicController heuristicControllerPrefab;
    public Player player1;
    public Player player2;
    public Player player3;
    public Player player4;

    void Start() {
      MakeThreeRandOneHeurController();

      capturables = new List<Capturable>(GetComponentsInChildren<Capturable>());
      raids = new List<Raid>();
      controllers = new List<BaseController>(GetComponentsInChildren<BaseController>());
      sceneCanvas = FindObjectOfType<Canvas>();

      foreach (BaseController controller in controllers) {
        unitPercent.Add(controller.player, 0);
      }
    }

    void Update() {
      UpdatePercent();
      if (sceneCanvas) {
        UpdateCanvas();
      }
    }

    private void UpdateCanvas() {
      foreach (KeyValuePair<Player, float> pair in unitPercent) {
        foreach (PlayerBar bar in sceneCanvas.bars) {
          if (bar.owner == pair.Key) {
            bar.ReSize(pair.Value);
          }
        }
      }
    }

    private void UpdatePercent() {
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

    private void OnVictory(BaseController winner) {
      Reset();
    }

    public void RaidCreated(Raid raid) {
      raids.Add(raid);
    }

    public void RaidRemoved(Raid raid) {
      raids.Remove(raid);
    }

    public void ControllerRemoved(BaseController controller) {
      controllers.Remove(controller);
      if(controllers.Count == 1) {
        BaseController winner = controllers[0];
        OnVictory(winner);
      }
    }

    public void MakeThreeRandOneHeurController() {
      Instantiate<RandomController>(randomControllerPrefab, Vector3.zero, transform.rotation, transform).player = player1;
      Instantiate<RandomController>(randomControllerPrefab, Vector3.zero, transform.rotation, transform).player = player2;
      Instantiate<RandomController>(randomControllerPrefab, Vector3.zero, transform.rotation, transform).player = player3;
      Instantiate<HeuristicController>(heuristicControllerPrefab, Vector3.zero, transform.rotation, transform).player = player4;
    }

    public void Reset() {
      
    }
}
