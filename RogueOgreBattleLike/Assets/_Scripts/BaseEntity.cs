using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType { ENEMY, PLAYER, CRITTER, NPC }

public abstract class BaseEntity : MonoBehaviour {    
    [SerializeField] EntityType entityType;
    [SerializeField] string title;
    [SerializeField] Sprite icon;

    public int X { get; private set; }
    public int Y { get; private set; }

    public EntityType GetEntityType() => entityType;
    public string GetTitle() => title;
    public Sprite GetIcon() => icon;

    public void Initialize(int x, int y) {
        MoveToPosition(x, y);
    }

    public void MoveToPosition(int x, int y) {
        X = x;
        Y = y;
        //#TODO::Smooth movement/teleport animation
        transform.position = new Vector2(X, Y);
    }

    public void MoveInDirection(Vector2 direction) {
        Y += (int)direction.y;
        X += (int)direction.x;
        //#TODO::Smooth movement
        transform.position = new Vector2(X, Y);
    }
}
