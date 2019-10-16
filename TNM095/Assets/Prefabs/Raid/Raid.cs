using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raid : MonoBehaviour {

    private List<GameObject> raiders;
    private int spawnNumber;
    private Vector3 movVec;
    private float distanceRemaining;

    public Capturable dest;
    public int unitCount;
    public float circleSize;
    public float speed;
    public GameObject raiderPrefab;
    public UnitIndicator indicator;
    public Player owner;
    public bool trainingMode;

    void Start() {
        spawnNumber = (int)Mathf.Floor(unitCount / Mathf.Sqrt(unitCount));
        raiders = new List<GameObject>();
        movVec = Vector3.Normalize(dest.transform.position - transform.position);
        indicator.UpdateText(unitCount.ToString(), owner);

        if (!trainingMode) {
            for (int i = 0; i < spawnNumber; i++) {
                Vector2 randPos = circleSize * Random.insideUnitCircle;
                raiders.Add(Instantiate(raiderPrefab, new Vector3(transform.position.x + randPos.x, transform.position.y, transform.position.z + randPos.y), transform.rotation, transform));
            }
        }
        Colorize();

        transform.parent.GetComponent<GameBoard>().RaidCreated(this);
    }

    void Update() {
        Move();
    }

    private void Move() {
        transform.position += movVec * speed * Time.deltaTime;
        distanceRemaining -= speed * Time.deltaTime;
        if (distanceRemaining < 0.1f) {
            dest.UnitsArrive(this);
        }
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

    public void Init(Player owner, Transform parent, Capturable target, int units) {
        this.owner = owner;
        transform.parent = parent;
        dest = target;
        unitCount = units;
        distanceRemaining = Vector3.Distance(transform.position, dest.transform.position);
    }

    public void Attacked(int damage) {
        unitCount -= damage;
        indicator.UpdateText(unitCount.ToString(), owner);
        if (unitCount <= 0) {
            transform.parent.GetComponent<GameBoard>().RaidRemoved(this);
        }
    }

}
