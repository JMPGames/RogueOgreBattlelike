using System.Linq;
using UnityEngine;

public class MapController : MonoBehaviour {
    public static MapController Instance;

    [SerializeField] GameObject _tilePrefab;
    [Header("Note: width and height need to be the outer wall")]
    [SerializeField] int _height;
    [SerializeField] int _width;
    [SerializeField] int _maxTraps;
    [SerializeField] GameObject[] _trapPrefabs;
    [Header("Note: 10 = 10% Chance")]
    [SerializeField] int _trapChance = 10;
    [Header("Note: 'Player' TileHelper FIRST")]
    [SerializeField] TileHelper[] _tileHelpers;

    Tile[,] _tileGrid;

    public int GetHeight() => _height;
    public int GetWidth() => _width;

    void Start() {
        MapGeneration();
    }

    public Tile GetTileAtPosition(int x, int y) {
        return _tileGrid[x, y];
    }

    public bool TileAtPositionIsBlocked(int x, int y) {
        return _tileGrid[x, y].Blocked;
    }

    public bool CheckTileForBattleStart(int x, int y, bool player) {
        Tile tileToCheck = _tileGrid[x, y];
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
        _tileGrid = new Tile[_width + 1, _height + 1];

        for (int x = 0; x < _width + 1; x++) {
            for (int y = 0; y < _height + 1; y++) {
                bool blocked = false;
                bool isExit = false;
                Vector3 position = new Vector2(x, y);
                Tile tile = (Instantiate(_tilePrefab, position, Quaternion.identity, transform) as GameObject).GetComponent<Tile>();
                TileHelper tileHelper = _tileHelpers.FirstOrDefault(th => th.transform.position == position);

                if (tileHelper != null) {
                    if (!tileHelper.IsExit) {
                        if (tileHelper.Occupant != null) {
                            GameObject unit = tileHelper.Occupant;
                            tile.Occupant = unit;
                            DungeonController.Instance.AddBattleUnit(unit);
                            unit.GetComponent<BattleUnit>().InitializePosition(x, y);
                        }
                        else {
                            blocked = true;
                        }
                    }
                    isExit = tileHelper.IsExit;
                    tileHelper.gameObject.SetActive(false);
                }

                blocked = blocked == true ? true : (x == 0 || x == _width || y == 0 || y == _height);
                if (!blocked) {
                    if (trapsSet < _maxTraps && Random.Range(1, 101) <= _trapChance) {
                        //GameObject trap = Instantiate(trapPrefabs[Random.Range(0, trapPrefabs.Length)], position, Quaternion.identity, transform) as GameObject;
                        //tile.Trap = trap;
                    }
                }

                tile.Initialize(x, y, blocked, isExit);
                _tileGrid[x, y] = tile;                
            }
        }
        DungeonController.Instance.MapLoaded();
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }
    }
}
