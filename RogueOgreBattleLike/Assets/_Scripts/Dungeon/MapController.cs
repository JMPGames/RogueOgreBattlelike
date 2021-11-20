﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapController : MonoBehaviour {
    [SerializeField] GameObject tilePrefab;
    [SerializeField] int height;
    [SerializeField] int width;
    [SerializeField] int maxTraps;
    [Header("Note: 10 = 10% Chance")]
    [SerializeField] int trapChance = 10;
    [Header("Note: 'Player' TileHelper FIRST")]
    [SerializeField] TileHelper[] tileHelpers;
    [SerializeField] GameObject[] trapPrefabs;

    Tile[,] tileGrid;

    void Awake() {
        MapGeneration();
    }

    public Tile GetTileAtPosition(int x, int y) {
        return tileGrid[x, y];
    }

    void MapGeneration() {
        int trapsSet = 0;
        tileGrid = new Tile[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                bool blocked = false;
                bool isExit = false;
                Vector3 position = new Vector2(x, y);
                Tile tile = (Instantiate(tilePrefab, position, Quaternion.identity, transform) as GameObject).GetComponent<Tile>();
                TileHelper tileHelper = tileHelpers.FirstOrDefault(th => th.transform.position == position);

                if (tileHelper != null) {
                    if (!tileHelper.isExit) {
                        if (tileHelper.occupant != null) {
                            tile.Occupant = tileHelper.occupant;
                            DungeonController.instance.AddBattleUnit(tileHelper.occupant);
                        }
                        blocked = true;
                    }
                    else {
                        isExit = true;
                    }
                    tileHelper.gameObject.SetActive(false);
                }

                bool isOuterWall = (x == 0 || x == width || y == 0 || y == height);
                blocked = blocked == true ? true : isOuterWall;
                if (!blocked) {
                    if (trapsSet < maxTraps && Random.Range(1, 101) <= trapChance) {
                        //GameObject trap = Instantiate(trapPrefabs[Random.Range(0, trapPrefabs.Length)], position, Quaternion.identity, transform) as GameObject;
                        //tile.Trap = trap;
                    }
                }

                tile.Initialize(x, y, blocked, isExit);
                tileGrid[x, y] = tile;                
            }
        }
    }
}
