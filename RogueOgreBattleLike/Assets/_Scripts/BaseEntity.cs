using UnityEngine;

public enum EntityType { ENEMY, PLAYER, CRITTER, NPC }

public abstract class BaseEntity : MonoBehaviour {    
    [SerializeField] EntityType _entityType;
    [SerializeField] string _title;
    [SerializeField] Sprite _icon;

    public EntityType GetEntityType() => _entityType;
    public string GetTitle() => _title;
    public Sprite GetIcon() => _icon;

    public void Initialize(int x, int y) {
        transform.position = new Vector2(x, y);        
    }
}
