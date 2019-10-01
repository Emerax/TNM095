using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HumanController : BaseController {
    private Capturable selected;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        UpdateMouseInput();

        if(selected != null) {
            UpdateSelection();
        }

        if (new List<Capturable>(FindObjectsOfType<Capturable>()).Where(r => r.owner == player).ToList().Count == 0) {
          if (new List<Raid>(FindObjectsOfType<Raid>()).Where(r => r.owner == player).ToList().Count == 0) {
            OnLose();
          }
        }
    }

    private void UpdateMouseInput() {
        HandleLeftClick();
        HandleRightClick();
    }

    private void HandleLeftClick() {
        if (Input.GetMouseButtonDown(0)) {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit rayHit)) {
                Selectable s = rayHit.collider.GetComponent<Selectable>();
                if (s != null) {
                    Capturable clicked = s.select;
                    if (clicked.owner == player) {
                        Select(clicked);
                        return;
                    }
                }
            }
            Deselect();
        }
    }

    private void HandleRightClick() {
        if (selected != null) {
            if (Input.GetMouseButtonDown(1)) {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit rayHit)) {
                    Selectable s = rayHit.collider.GetComponent<Selectable>();
                    if (s != null) {
                        Capturable target = s.select;
                        selected.BeginRaid(target);
                    }
                }
            }
        }
    }

    private void Select(Capturable capturable) {
        Deselect();
        selected = capturable;
        selected.OnSelected();
    }

    private void Deselect() {
        if(selected) {
            selected.OnDeselected();
        }
        selected = null;
    }

    private void UpdateSelection() {
        if(selected.owner != player) {
            Deselect();
        }
    }
}
