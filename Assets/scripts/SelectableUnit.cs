using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using scripts.entities;

[RequireComponent(typeof(Entity))]
public class SelectableUnit : MonoBehaviour
{
    public GameObject selectionCircle;
}