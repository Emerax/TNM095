using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour {
    /// <summary>
    /// Player instance controlled by this controller.
    /// </summary>
    public Player player;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    protected virtual void OnLose() {
      Debug.Log("I, player " + player.playerName + ", am no more!");
      FindObjectOfType<GameState>().controllerRemoved(this);
      Destroy(gameObject);
    }

    public virtual void OnWin() {
      Debug.Log("I, player " + player.playerName + ", am the winner!");
      FindObjectOfType<GameState>().controllerRemoved(this);
      Destroy(gameObject);
    }
}
