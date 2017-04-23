using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private int amountOfPlayers = 4;
    public int ownerID = 0;

    public enum RELATIONTYPE
    {
        OWN, ENEMY, ALLY, NEUTRAL
    }

    public static List<List<RELATIONTYPE>> relations = new List<List<RELATIONTYPE>>();


    void Awake()
    {
        relations.Add(new List<RELATIONTYPE>());
        relations.Add(new List<RELATIONTYPE>());
        relations.Add(new List<RELATIONTYPE>());
        relations.Add(new List<RELATIONTYPE>());

        // RELATIONS P1
        relations[0].Add(RELATIONTYPE.OWN);
        relations[0].Add(RELATIONTYPE.ENEMY);
        relations[0].Add(RELATIONTYPE.ALLY);
        relations[0].Add(RELATIONTYPE.NEUTRAL);

        // RELATIONS P2
        relations[1].Add(RELATIONTYPE.ENEMY);
        relations[1].Add(RELATIONTYPE.OWN);
        relations[1].Add(RELATIONTYPE.ENEMY);
        relations[1].Add(RELATIONTYPE.NEUTRAL);

        // RELATIONS P3
        relations[2].Add(RELATIONTYPE.ALLY);
        relations[2].Add(RELATIONTYPE.ENEMY);
        relations[2].Add(RELATIONTYPE.OWN);
        relations[2].Add(RELATIONTYPE.NEUTRAL);

        // RELATIONS P4
        relations[3].Add(RELATIONTYPE.NEUTRAL);
        relations[3].Add(RELATIONTYPE.NEUTRAL);
        relations[3].Add(RELATIONTYPE.NEUTRAL);
        relations[3].Add(RELATIONTYPE.OWN);
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static RELATIONTYPE getRelationBetween(int ownerIdA, int ownerIdB)
    {
        return relations[ownerIdA][ownerIdB];
    }
}
