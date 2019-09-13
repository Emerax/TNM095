using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Vector3 movVec;
    private int damage;
    public float speed;

    public Capturable tower;
    public Raid target;
    public GameObject arrowPrefab;

    void Start() {
      movVec = Vector3.Normalize(target.transform.position - transform.position);
      damage = 15;
      speed = 7;
    }

    void Update() {
      Move();
    }

    private void Move() {
      transform.position += movVec*speed*Time.deltaTime;
      if (!target) {
        Destroy(this.gameObject);
      } else if ((transform.position - target.transform.position).magnitude < 0.5) {
          target.Attacked(damage);
          Destroy(this.gameObject);
      }
    }
}
