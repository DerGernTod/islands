using System.Collections;
using System.Collections.Generic;
using System.Text;
using scripts.entities;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class SelectionManager : MonoBehaviour {
    bool isSelecting = false;
    Vector3 mousePosition1;

    public GameObject mySelectionCirclePrefab;
    public GameObject enemySelectionCirclePrefab;
    public GameObject allySelectionCirclePrefab;
    public GameObject neutralSelectionCirclePrefab;

    void Update() {
        // If we press the left mouse button, begin selection and remember the location of the mouse
        if (Input.GetMouseButtonDown(0)) {
            isSelecting = true;
            mousePosition1 = Input.mousePosition;

            foreach (var selectableObject in FindObjectsOfType<SelectableUnit>()) {
                if (selectableObject.selectionCircle != null) {
                    Destroy(selectableObject.selectionCircle.gameObject);
                    selectableObject.selectionCircle = null;
                    if (selectableObject.GetComponent<Entity>().ownerID == GetComponent<PlayerManager>().ownerID && selectableObject.GetComponent<ClickToMove>() != null) {
                        selectableObject.GetComponent<ClickToMove>().enabled = false;
                    }
                }
            }
        }
        // If we let go of the left mouse button, end selection
        if (Input.GetMouseButtonUp(0)) {
            var selectedObjects = new List<SelectableUnit>();
            foreach (var selectableObject in FindObjectsOfType<SelectableUnit>()) {
                if (IsWithinSelectionBounds(selectableObject.gameObject)) {
                    selectedObjects.Add(selectableObject);
                }
            }

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Selecting [{0}] Units", selectedObjects.Count));
            foreach (var selectedObject in selectedObjects)
                sb.AppendLine("-> " + selectedObject.gameObject.name);
            Debug.Log(sb.ToString());

            isSelecting = false;
        }

        // Highlight all objects within the selection box
        if (isSelecting) {
            foreach (var selectableObject in FindObjectsOfType<SelectableUnit>()) {
                if (IsWithinSelectionBounds(selectableObject.gameObject)) {
                    if (selectableObject.selectionCircle == null) {
                        if (selectableObject.GetComponent<Entity>().ownerID == GetComponent<PlayerManager>().ownerID)
                        {
                            selectableObject.selectionCircle = Instantiate(mySelectionCirclePrefab);


                            if (selectableObject.GetComponent<ClickToMove>() != null)
                            {
                                selectableObject.GetComponent<ClickToMove>().enabled = true;
                            }
                        }
                        else
                        {
                            //TODO: check if is neutral or ally
                            selectableObject.selectionCircle = Instantiate(enemySelectionCirclePrefab);
                        }

                        selectableObject.selectionCircle.transform.SetParent(selectableObject.transform, false);
                        selectableObject.selectionCircle.transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                }
                else {
                    if (selectableObject.selectionCircle != null) {
                        Destroy(selectableObject.selectionCircle.gameObject);
                        selectableObject.selectionCircle = null;
                    }
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