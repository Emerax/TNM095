﻿using System.Collections;
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
      Debug.Log("I AM NO MORE!");
      FindObjectOfType<GameState>().playerRemoved(player);
      Destroy(gameObject);
    }
}
