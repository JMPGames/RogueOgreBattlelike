using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour {
    [SerializeField] BattleEntity[] entities = new BattleEntity[5];

    public BattleEntity[] GetEntities() => entities;
    public BattleEntity GetEntityAtPosition(int index) => entities[index];
}
