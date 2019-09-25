using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private List<Capturable> buildings;
    private List<Raid> raids;
    private List<Player> players;

    // Start is called before the first frame update
    void Start() {
        buildings = new List<Capturable>(FindObjectsOfType<Capturable>());
        raids = new List<Raid>();
        players = new List<Player>(FindObjectsOfType<Player>());
    }

    // Update is called once per frame
    void Update() {
      if(players.Count == 1) {
        Player winner = players[0];
        onVictory(winner);
      }
    }

    private void onVictory(Player winner) {
        Debug.Log(winner.name + " is our winner!");
        //Remove the winning controller?
        playerRemoved(winner);
    }

    public void raidCreated(Raid raid) {
        raids.Add(raid);
    }

    public void raidRemoved(Raid raid) {
        raids.Remove(raid);
    }

    public void playerRemoved(Player player) {
        players.Remove(player);
    }

}
