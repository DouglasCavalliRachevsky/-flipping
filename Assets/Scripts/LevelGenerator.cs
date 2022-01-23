using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class LevelGenerator
{
    private static LevelRules levelRules;

    private static int startingNumberOfPieces;

    private static List<Vector3Int> currentGameBoard = new List<Vector3Int>();

    enum NeighborType
    {
        min,
        right,
        left,
        top,
        bottom,
        max
    }

    public static List<Vector3Int> GenerateANewLevel(LevelRules _levelRules, int _startingNumberOfPieces)
    {
        startingNumberOfPieces = _startingNumberOfPieces;
        levelRules = _levelRules;
        ResetLevel();
        GenerateLevel(levelRules.MostImportantPieceIndex);
        return currentGameBoard;
    }

    private static void GenerateLevel(int startingPrefabIndex = 0)
    {
        int lastPrefabIndex = 0;
        for (int i = 0; i < startingNumberOfPieces; i++)
        {
            if (i > 1)
            {
                while (startingPrefabIndex == lastPrefabIndex)
                {
                    startingPrefabIndex = Random.Range(1, levelRules.PieceList.Count);
                }
            }

            var newOccupiedTile = GetAvailableTile(startingPrefabIndex);
            currentGameBoard.Add(newOccupiedTile);

            Debug.Log("Tile " + i + " - " + newOccupiedTile);

            lastPrefabIndex = startingPrefabIndex;
        }
    }

    private static Vector3Int GetAvailableTile(int prefabIndex)
    {
        if (currentGameBoard.Count == 0)
        {
            return (new Vector3Int(Random.Range(0, levelRules.GameBoardSize),
                Random.Range(0, levelRules.GameBoardSize), prefabIndex));
        }

        while (true)
        {
            int randomOccupiedTileIndex = Random.Range(0, currentGameBoard.Count);
            Vector3Int occupiedTile = currentGameBoard[randomOccupiedTileIndex];

            NeighborType neighborType =
                (NeighborType) Random.Range((int) NeighborType.min + 1, (int) NeighborType.max);

            switch (neighborType)
            {
                case NeighborType.right:
                    if (IsTileAvailable(occupiedTile.x + 1, occupiedTile.y))
                    {
                        return (new Vector3Int(occupiedTile.x + 1, occupiedTile.y, prefabIndex));
                    }

                    break;
                case NeighborType.left:
                    if (IsTileAvailable(occupiedTile.x - 1, occupiedTile.y))
                    {
                        return (new Vector3Int(occupiedTile.x - 1, occupiedTile.y, prefabIndex));
                    }

                    break;
                case NeighborType.top:
                    if (IsTileAvailable(occupiedTile.x, occupiedTile.y + 1))
                    {
                        return (new Vector3Int(occupiedTile.x, occupiedTile.y + 1, prefabIndex));
                    }

                    break;
                case NeighborType.bottom:
                    if (IsTileAvailable(occupiedTile.x, occupiedTile.y - 1))
                    {
                        return (new Vector3Int(occupiedTile.x, occupiedTile.y - 1, prefabIndex));
                    }

                    break;
            }
        }
    }

    private static bool IsTileAvailable(int x, int y)
    {
        if (IsOnGameBoard(x, y) == false)
        {
            return false;
        }

        foreach (var tile in currentGameBoard)
        {
            if (tile.x == x && tile.y == y)
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsOnGameBoard(int x, int y)
    {
        return x < levelRules.GameBoardSize
               && x >= 0
               && y < levelRules.GameBoardSize
               && y >= 0;
    }

    private static void ResetLevel()
    {
        currentGameBoard = new List<Vector3Int>();
    }
}