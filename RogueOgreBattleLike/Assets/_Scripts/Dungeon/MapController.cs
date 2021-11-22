using System.Linq;
using UnityEngine;

public class MapController : MonoBehaviour {
    public static MapController instance;

    [SerializeField] GameObject tilePrefab;
    [Header("Note: width and height need to be the outer wall")]
    [SerializeField] int height;
    [SerializeField] int width;
    [SerializeField] int maxTraps;
    [SerializeField] GameObject[] trapPrefabs;
    [Header("Note: 10 = 10% Chance")]
    [SerializeField] int trapChance = 10;
    [Header("Note: 'Player' TileHelper FIRST")]
    [SerializeField] TileHelper[] tileHelpers;

    Tile[,] tileGrid;

    public int GetHeight() => height;
    public int GetWidth() => width;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        MapGeneration();
    }

    public Tile GetTileAtPosition(int x, int y) {
        return tileGrid[x, y];
    }

    public bool TileAtPositionIsBlocked(int x, int y) {
        return tileGrid[x, y].Blocked;
    }

    public bool CheckTileForBattleStart(int x, int y, bool player) {
        Tile tileToCheck = tileGrid[x, y];
        if (tileToCheck.Occupied()) {
            if (player) {
                return true;
            }
            else {
                return tileToCheck.Occupant.GetComponent<BattleUnit>().IsPlayer;
            }
        }
        return false;
    }

    void MapGeneration() {
        int trapsSet = 0;
        tileGrid = new Tile[width + 1, height + 1];

        for (int x = 0; x < width + 1; x++) {
            for (int y = 0; y < height + 1; y++) {
                bool blocked = false;
                bool isExit = false;
                Vector3 position = new Vector2(x, y);
                Tile tile = (Instantiate(tilePrefab, position, Quaternion.identity, transform) as GameObject).GetComponent<Tile>();
                TileHelper tileHelper = tileHelpers.FirstOrDefault(th => th.transform.position == position);

                if (tileHelper != null) {
                    if (!tileHelper.isExit) {
                        if (tileHelper.occupant != null) {
                            GameObject unit = tileHelper.occupant;
                            tile.Occupant = unit;
                            DungeonController.instance.AddBattleUnit(unit);
                            unit.GetComponent<BattleUnit>().InitializePosition(x, y);
                        }
                        else {
                            blocked = true;
                        }
                    }
                    else {
                        isExit = true;
                    }
                    tileHelper.gameObject.SetActive(false);
                }

                blocked = blocked == true ? true : (x == 0 || x == width || y == 0 || y == height);
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
        DungeonController.instance.MapLoaded();
    }
}
