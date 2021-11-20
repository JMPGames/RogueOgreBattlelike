using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool Blocked { get; private set; }
    public bool IsAnExit { get; private set; }
    public GameObject Trap { get; set; }    
    public GameObject Occupant { get; set; }

    public Tile Previous { get; set; }
    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }

    public void Initialize(int x, int y, bool blocked, bool isExit) {
        X = x;
        Y = y;
        Blocked = blocked;
        IsAnExit = isExit;
    }

    public bool Occupied() {
        return Occupant != null;
    }

    public bool Trapped() {
        return Trap != null;
    }

    public void Move(GameObject entity = null) {
        Occupant = entity;
        Blocked = entity != null;
    }
}
