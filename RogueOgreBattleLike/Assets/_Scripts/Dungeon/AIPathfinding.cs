using System.Collections.Generic;
using UnityEngine;

public class AIPathfinding : MonoBehaviour {
    const int MoveDiagonalCost = 20;
    const int MoveCost = 10;

    public Tile Path(int startX, int startY, int endX, int endY) {
        List<Tile> path = FindPath(startX, startY, endX, endY);

        //UNCOMMENT IF DUNGEON MOVEMENT ISN'T LOCKED TO ONE MOVE PER TURN
            //Function output needs to be switched to List<Tile>
            //return path == null ? null : path;
        //-------------------------------------------------------------

        //REMOVE IF DUNGEON MOVEMENT ISN'T LOCKED TO ONE MOVE PER TURN
        if (path == null) {
            return null;
        }
        else {
            //return index 1, since index 0 is the AI's current tile
            return path[1];
        }
        //-------------------------------------------------------------
    }

    List<Tile> FindPath(int curX, int curY, int playerX, int playerY) {
        Tile startTile = MapController.instance.GetTileAtPosition(curX, curY);
        Tile endTile = MapController.instance.GetTileAtPosition(playerX, playerY);

        List<Tile> open = new List<Tile> { startTile };
        List<Tile> closed = new List<Tile>();

        //Reset all tile values
        for (int x = 0; x < MapController.instance.GetWidth() + 1; x++) {
            for (int y = 0; y < MapController.instance.GetHeight() + 1; y++) {
                Tile tile = MapController.instance.GetTileAtPosition(x, y);
                tile.G = int.MaxValue;
                tile.Previous = null;
            }
        }

        startTile.G = 0;
        startTile.H = CalculateDistanceCost(startTile, endTile);

        int iteration = 0;
        Tile currentTile = null;

        while (open.Count > 0) {
            iteration++;
            currentTile = GetLowestFCostNode(open);

            if (currentTile == endTile) {
                return CalculatePath(endTile);
            }

            open.Remove(currentTile);
            closed.Add(currentTile);

            foreach (Tile neighbor in GetAdjacentTiles(currentTile)) {
                if (closed.Contains(neighbor)) {
                    continue;
                }
                if (MapController.instance.TileAtPositionIsBlocked(neighbor.X, neighbor.Y)) {
                    closed.Add(neighbor);
                    continue;
                }

                int tentativeGCost = currentTile.G + CalculateDistanceCost(currentTile, neighbor);
                if (tentativeGCost < neighbor.G) {
                    neighbor.Previous = currentTile;
                    neighbor.G = tentativeGCost;
                    neighbor.H = CalculateDistanceCost(neighbor, endTile);

                    if (!open.Contains(neighbor)) {
                        open.Add(neighbor);
                    }
                }
            }
        }
        //If the algorithm fails, then the path to the player is cut off, return null
        return null;
    }

    List<Tile> CalculatePath(Tile endTile) {
        List<Tile> path = new List<Tile>();
        path.Add(endTile);
        Tile currentTile = endTile;

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
            tiles.Add(MapController.instance.GetTileAtPosition(tile.X - 1, tile.Y));
        }
        if (tile.X + 1 < MapController.instance.GetWidth() + 1) {
            tiles.Add(MapController.instance.GetTileAtPosition(tile.X + 1, tile.Y));
        }
        if (tile.Y - 1 >= 0) {
            tiles.Add(MapController.instance.GetTileAtPosition(tile.X, tile.Y - 1));
        }
        if (tile.Y + 1 < MapController.instance.GetHeight() + 1) {
            tiles.Add(MapController.instance.GetTileAtPosition(tile.X, tile.Y + 1));
        }
        return tiles;
    }

    int CalculateDistanceCost(Tile a, Tile b) {
        int xDistance = Mathf.Abs(a.X - b.X);
        int yDistance = Mathf.Abs(a.Y - b.Y);

        int remaining = Mathf.Abs(xDistance - yDistance);
        return MoveDiagonalCost * Mathf.Min(xDistance, yDistance) + MoveCost * remaining;
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
