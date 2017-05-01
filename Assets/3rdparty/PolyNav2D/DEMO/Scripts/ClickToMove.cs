using UnityEngine;
using System.Collections.Generic;
using scripts.services;
using scripts.entities;

//example
[RequireComponent(typeof(PolyNavAgent))]
public class ClickToMove : MonoBehaviour{

    private bool canMove = false;
	private PolyNavAgent _agent;
	public PolyNavAgent agent{
		get
		{
			if (!_agent){
				_agent = GetComponent<PolyNavAgent>();
			}
			return _agent;			
		}
	}

    private void Start() {
        Entity entity = GetComponent<Entity>();
        ServiceLocator.Service<EventService>().EntitySelected += (entityId, ownerId) => {
            if (entityId == entity.ID && entity.ownerID == ownerId) {
                canMove = true;
            };
        };
        ServiceLocator.Service<EventService>().EntityDeselected += (entityId, ownerId) => {
            if (entityId == entity.ID && entity.ownerID == ownerId) {
                canMove = false;
            };
        };
    }

    void Update() {
		if (canMove && Input.GetMouseButton(1)){
			agent.SetDestination( Camera.main.ScreenToWorldPoint(Input.mousePosition) );
		}
	}
}