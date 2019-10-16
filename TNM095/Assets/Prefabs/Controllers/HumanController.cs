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
    }

    private void UpdateMouseInput() {
        HandleLeftClick();
        HandleRightClick();
    }

    private void HandleLeftClick() {
        if (Input.GetMouseButtonDown(0)) {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit rayHit)) {
                Capturable c = rayHit.collider.GetComponent<Capturable>();
                if (c != null) {
                    if (c.owner == player) {
                        Select(c);
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
                    Capturable c = rayHit.collider.GetComponent<Capturable>();
                    if (c != null) {
                        selected.BeginRaid(c);
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
