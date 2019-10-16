using MLAgents;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameBoard : MonoBehaviour {
    public List<Capturable> capturables;
    public List<Raid> raids;
    public List<BaseController> controllers;
    public Dictionary<Player, float> unitPercent = new Dictionary<Player, float>();
    public PerformanceUI sceneCanvas;
    public RandomController randomControllerPrefab;
    public HeuristicController heuristicControllerPrefab;
    public List<Player> players;
    public List<Agent> agents;
    public bool isTrainingGame;
    public bool randomResets;

    public float timeLimit;

    private float currentTime = 0;
    private readonly List<Player> contestants = new List<Player>();

    void Start() {
        capturables = new List<Capturable>(GetComponentsInChildren<Capturable>());
        raids = new List<Raid>();
        controllers = new List<BaseController>(GetComponentsInChildren<BaseController>());
        sceneCanvas = FindObjectOfType<PerformanceUI>();

        contestants.AddRange(players);

        for (int i = 0; i < capturables.Count; ++i) {
            capturables[i].trainingID = i; 
        }
    }

    void Update() {
        if(currentTime >= timeLimit) {
            Reset();
            currentTime = 0;
        } else {
            currentTime += Time.unscaledDeltaTime;
        }
    }

    public void RaidCreated(Raid raid) {
        raids.Add(raid);
    }

    public void RaidRemoved(Raid raid) {
        raids.Remove(raid);
        Destroy(raid.gameObject);
        CheckWinning();
    }

    public void CheckWinning() {
        if (capturables.Select(c => c.owner).Concat(raids.Select(r => r.owner)).Distinct().Count() < 2) {
            //Less than two players remain, meaning one has won.
            Reset();
        }

        if (isTrainingGame) {
            if(!capturables.Select(c => c.owner).Concat(raids.Select(r => r.owner)).Distinct().Any(p => p != null && p.isMLControlled)) {
                //No ML-controlled players remain. If this is a training game, we can reset.
                Reset();
            }
        }
    }

    public void Reset() {
        foreach(Agent agent in agents) {
            agent.Done();
        }

        foreach (Capturable capturable in capturables) {
            capturable.Reset();
        }

        foreach (Raid raid in raids) {
            if(raid != null) {
                Destroy(raid.gameObject);
            }
        }
        raids.Clear();

        foreach(Player player in players) {
            player.Reset();
        }

        if (randomResets) {
            List<Player> playerBuffer = new List<Player>(players);
            List<Capturable> capturableBuffer = new List<Capturable>(capturables);
            for(int i = 0; i < players.Count; ++i) {
                Player player = playerBuffer[Random.Range(0, playerBuffer.Count)];
                int startingCapturables = Random.Range(0, capturableBuffer.Count);
                for(int j = 0; j < startingCapturables; ++j) {
                    Capturable capturable = capturableBuffer[Random.Range(0, capturableBuffer.Count)];
                    capturable.SetOwner(player);

                    capturableBuffer.Remove(capturable);
                }

                playerBuffer.Remove(player);
            }
        } else {
            List<Capturable> forts = capturables.Where(c => c.gameObject.name == "Fort").ToList();
            foreach (Player player in players) {
                forts[0].SetOwner(player);
                forts.RemoveAt(0);
            }
        }

        currentTime = 0;
    }
}
