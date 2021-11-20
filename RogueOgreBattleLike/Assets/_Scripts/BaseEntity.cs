using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType { ENEMY, PLAYER, CRITTER, NPC }

public abstract class BaseEntity : MonoBehaviour {    
    [SerializeField] EntityType entityType;
    [SerializeField] string title;
    [SerializeField] Sprite icon;

    public EntityType GetEntityType() => entityType;
    public string GetTitle() => title;
    public Sprite GetIcon() => icon;

    public void Initialize(int x, int y) {
        transform.position = new Vector2(x, y);        
    }
}
