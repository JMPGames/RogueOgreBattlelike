using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool Blocked { get; private set; }
    public GameObject Trap { get; private set; }    
    public GameObject Occupant { get; private set; }

    public Tile Previous { get; set; }
    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }

    public void Initialize(int x, int y, bool blocked = false, GameObject trap = null) {
        X = x;
        Y = y;
        Blocked = blocked;
        Trap = trap;
    }

    public void SetOccupant(GameObject entity) {
        Occupant = entity;
    }

    public bool Occupied() {
        return Occupant != null;
    }

    public void SetTrap(GameObject trap) {
        Trap = trap;
    }

    public bool Trapped() {
        return Trap != null;
    }

    public void Move(GameObject entity = null) {
        Occupant = entity;
        Blocked = entity != null;
    }
}
