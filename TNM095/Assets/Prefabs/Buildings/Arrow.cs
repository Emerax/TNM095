using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    private int damage;

    public float speed;
    public Raid target;

    void Start() {
        damage = 15;
        speed = 7;
    }

    void Update() {
        Move();
    }

    public void Init(Raid target, Transform parent, int damage) {
      transform.parent = parent;
      this.target = target;
      this.damage = damage;
    }

    private void Move() {
      if(target != null) {
        Vector3 movVec = Vector3.Normalize(target.transform.position - transform.position);
        transform.rotation = Quaternion.LookRotation(movVec);
        transform.position += movVec * speed * Time.deltaTime;
        if (!target) {
          Destroy(gameObject);
        } else if ((transform.position - target.transform.position).magnitude < 0.5) {
          target.Attacked(damage);
          Destroy(gameObject);
        }
      } else {
          Destroy(gameObject);
      }
    }
}
