using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raid : MonoBehaviour {

    private List<GameObject> raiders;
    private int spawnNumber;
    private Vector3 movVec;
    
    public Capturable dest;
    public int unitCount;
    public float circleSize;
    public float speed;
    public GameObject raiderPrefab;
    public UnitIndicator indicator;
    public Player owner;

    void Start() {
        spawnNumber = (int)Mathf.Floor(unitCount / Mathf.Sqrt(unitCount));
        raiders = new List<GameObject>();
        movVec = Vector3.Normalize(dest.transform.position - transform.position);
        indicator.UpdateText(unitCount.ToString(), owner);

        for (int i = 0; i < spawnNumber; i++) {
            Vector2 randPos = circleSize * Random.insideUnitCircle;
            raiders.Add(Instantiate(raiderPrefab, new Vector3(transform.position.x + randPos.x, transform.position.y, transform.position.z + randPos.y), transform.rotation, transform));
        }
        Colorize();

        GameObject.FindObjectOfType<GameState>().raidCreated(this);
    }

    void Update() {
        Move();
    }

    void OnDestroy() {
        GameObject.FindObjectOfType<GameState>()?.raidRemoved(this);
    }

    private void Move() {
        transform.position += movVec * speed * Time.deltaTime;
        if ((transform.position - dest.transform.position).magnitude < 0.1)
            dest.UnitsArrive(this);
    }
    private void Colorize() {
        Color newColor = Color.gray;
        if (owner != null) {
            newColor = owner.playerColor;
        }
        foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>()) {
            r.material.color = newColor;
        }
    }

    public void Init(Player owner, Capturable target, int units) {
        this.owner = owner;
        dest = target;
        unitCount = units;
    }

    public void Attacked(int damage) {
        unitCount -= damage;
        indicator.UpdateText(unitCount.ToString(), owner);
        if (unitCount <= 0) {
            Destroy(this.gameObject);
        }
    }

}
