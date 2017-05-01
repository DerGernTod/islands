using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using scripts.entities;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class SelectionManager : MonoBehaviour {
    bool isSelecting = false;
    private Vector3 mousePosition1;
    private List<SelectableUnit> currentSelection = new List<SelectableUnit>();
    private int ownerId;
    private void Start() {
        ownerId = GetComponent<PlayerManager>().ownerID;
    }
    void Update() {
        // If we press the left mouse button, begin selection and remember the location of the mouse
        if (Input.GetMouseButtonDown(0)) {
            isSelecting = true;
            mousePosition1 = Input.mousePosition;
            currentSelection.ForEach(selectableObject => selectableObject.Deselect(ownerId));
            currentSelection.Clear();

        }
        // If we let go of the left mouse button, end selection
        if (Input.GetMouseButtonUp(0)) {
            var selectedObjects = FindObjectsOfType<SelectableUnit>().Where(selectableObject => IsWithinSelectionBounds(selectableObject.gameObject));
            currentSelection.AddRange(selectedObjects);
            currentSelection.ForEach(e => e.Select(ownerId));


            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Selecting [{0}] Units", currentSelection.Count));
            foreach (var selectedObject in selectedObjects)
                sb.AppendLine("-> " + selectedObject.gameObject.name);
            Debug.Log(sb.ToString());

            isSelecting = false;
        }

        // Highlight all objects within the selection box
        if (isSelecting) {
            var selectablesInArea = FindObjectsOfType<SelectableUnit>().Where(selectableObject => IsWithinSelectionBounds(selectableObject.gameObject));
            foreach (var selectableObject in FindObjectsOfType<SelectableUnit>()) {
                if (IsWithinSelectionBounds(selectableObject.gameObject)) {
                    selectableObject.CreateSelection(ownerId);
                }
                else {
                    selectableObject.DestroySelection();
                }
            }
        }
    }

    public bool IsWithinSelectionBounds(GameObject gameObject) {
        if (!isSelecting)
            return false;

        var camera = Camera.main;
        var viewportBounds = UnitSelectionUtils.GetViewportBounds(camera, mousePosition1, Input.mousePosition);
        return viewportBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
    }

    void OnGUI() {
        if (isSelecting) {
            // Create a rect from both mouse positions
            var rect = UnitSelectionUtils.GetScreenRect(mousePosition1, Input.mousePosition);
            UnitSelectionUtils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            UnitSelectionUtils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }
}