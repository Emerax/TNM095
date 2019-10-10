using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public List<Capturable> capturables;
    public List<Raid> raids;
    public List<BaseController> controllers;

    // Start is called before the first frame update
    void Start() {
        capturables = new List<Capturable>(FindObjectsOfType<Capturable>());
        raids = new List<Raid>();
        controllers = new List<BaseController>(FindObjectsOfType<BaseController>());
    }

    // Update is called once per frame
    void Update() {

    }

    private void onVictory(BaseController winner) {
        Debug.Log(winner.player.playerName + " is our winner!");
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
