using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raider : MonoBehaviour {

    private float sinHeight;

    // Start is called before the first frame update
    void Start() {
      sinHeight = UnityEngine.Random.value*Mathf.PI*2;
    }

    // Update is called once per frame
    void Update() {
      Vector3 currPos = transform.localPosition;
      transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Abs(Mathf.Sin(sinHeight))*0.8f, transform.localPosition.z);

      if(sinHeight >= Mathf.PI*2) {
        sinHeight = 0;
      } else {
        sinHeight += 10*Time.deltaTime;
      }
    }
}
