using System.Collections.Generic;
using UnityEngine;

public class AIPathfinding : MonoBehaviour {
    const int MOVE_DIAGONAL_COST = 20;
    const int MOVE_COST = 10;

    public Tile Path(int startX, int startY, int endX, int endY) {
        List<Tile> path = FindPath(startX, startY, endX, endY);

        //UNCOMMENT IF DUNGEON MOVEMENT ISN'T LOCKED TO ONE MOVE PER TURN
        //Function output needs to be switched to List<Tile>
        //return path == null ? null : path;
        //-------------------------------------------------------------

        //REMOVE IF DUNGEON MOVEMENT ISN'T LOCKED TO ONE MOVE PER TURN
        //Return path[1], path[0] is this entities current tile
        return path == null ? null : path[1];
        //-------------------------------------------------------------
    }

    List<Tile> FindPath(int currentX, int currentY, int playerX, int playerY) {
        Tile startTile = MapController.Instance.GetTileAtPosition(currentX, currentY);
        Tile endTile = MapController.Instance.GetTileAtPosition(playerX, playerY);

        List<Tile> openList = new List<Tile> { startTile };
        List<Tile> closedList = new List<Tile>();

        //Reset all tile values
        for (int x = 0; x < MapController.Instance.GetWidth() + 1; x++) {
            for (int y = 0; y < MapController.Instance.GetHeight() + 1; y++) {
                Tile tile = MapController.Instance.GetTileAtPosition(x, y);
                tile.G = int.MaxValue;
                tile.Previous = null;
            }
        }

        startTile.G = 0;
        startTile.H = CalculateDistanceCost(startTile, endTile);

        Tile currentTile = null;

        while (openList.Count > 0) {
            currentTile = GetLowestFCostNode(openList);

            if (currentTile == endTile) {
                return CalculatePath(endTile);
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            foreach (Tile neighbor in GetAdjacentTiles(currentTile)) {
                if (closedList.Contains(neighbor)) {
                    continue;
                }
                if (MapController.Instance.TileAtPositionIsBlocked(neighbor.X, neighbor.Y)) {
                    closedList.Add(neighbor);
                    continue;
                }

                int tentativeGCost = currentTile.G + CalculateDistanceCost(currentTile, neighbor);
                if (tentativeGCost < neighbor.G) {
                    neighbor.Previous = currentTile;
                    neighbor.G = tentativeGCost;
                    neighbor.H = CalculateDistanceCost(neighbor, endTile);

                    if (!openList.Contains(neighbor)) {
                        openList.Add(neighbor);
                    }
                }
            }
        }
        //If the algorithm fails, then the path to the player is cut off, return null
        return null;
    }

    List<Tile> CalculatePath(Tile endTile) {
        List<Tile> path = new List<Tile>();
        Tile currentTile = endTile;

        path.Add(endTile);

        while (currentTile.Previous != null) {
            path.Add(currentTile.Previous);
            currentTile = currentTile.Previous;
        }

        path.Reverse();
        return path;
    }

    List<Tile> GetAdjacentTiles(Tile tile) {
        List<Tile> tiles = new List<Tile>();

        if (tile.X - 1 >= 0) {
            tiles.Add(MapController.Instance.GetTileAtPosition(tile.X - 1, tile.Y));
        }
        if (tile.X + 1 < MapController.Instance.GetWidth() + 1) {
            tiles.Add(MapController.Instance.GetTileAtPosition(tile.X + 1, tile.Y));
        }
        if (tile.Y - 1 >= 0) {
            tiles.Add(MapController.Instance.GetTileAtPosition(tile.X, tile.Y - 1));
        }
        if (tile.Y + 1 < MapController.Instance.GetHeight() + 1) {
            tiles.Add(MapController.Instance.GetTileAtPosition(tile.X, tile.Y + 1));
        }
        return tiles;
    }

    int CalculateDistanceCost(Tile a, Tile b) {
        int xDistance = Mathf.Abs(a.X - b.X);
        int yDistance = Mathf.Abs(a.Y - b.Y);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_COST * Mathf.Abs(xDistance - yDistance);
    }

    Tile GetLowestFCostNode(List<Tile> tiles) {
        Tile tile = tiles[0];

        for (int i = 0; i < tiles.Count; i++) {
            if (tiles[i].F < tile.F) {
                tile = tiles[i];
            }
        }
        return tile;
    }
}
