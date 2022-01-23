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

    private static void GenerateLevel(int currentPrefabIndex = 0)
    {
        int lastPrefabIndex = 0;
        for (int i = 0; i < startingNumberOfPieces; i++)
        {
            if (levelRules.GameType == GameType.Sandwich && i > 1)
            {
                while (currentPrefabIndex == lastPrefabIndex)
                {
                    currentPrefabIndex = Random.Range(1, levelRules.PieceList.Count);
                }
            }

            var newOccupiedTile = GetAvailableTile(currentPrefabIndex);

            if (newOccupiedTile == Vector3Int.one * -1)
            {
                ResetLevel();
                Debug.LogError("Error on creating level!!! Please, confirm the level data.");
                return;
            }
            
            currentGameBoard.Add(newOccupiedTile);

            Debug.Log("Tile " + i + " - " + newOccupiedTile);

            lastPrefabIndex = currentPrefabIndex;
        }
    }

    private static Vector3Int GetAvailableTile(int currentPrefabIndex)
    {
        if (currentGameBoard.Count == 0)
        {
            return (new Vector3Int(Random.Range(0, levelRules.GameBoardSize),
                Random.Range(0, levelRules.GameBoardSize), currentPrefabIndex));
        }

        int attempts = 1000;
        while (attempts > 0)
        {
            attempts--;
            int randomOccupiedTileIndex = Random.Range(0, currentGameBoard.Count);
            
            if (levelRules.GameType == GameType.g2048 && currentGameBoard[randomOccupiedTileIndex].z == 0)
            {
                continue;
            }
            
            Vector2Int occupiedTilePosition = new Vector2Int(currentGameBoard[randomOccupiedTileIndex].x,currentGameBoard[randomOccupiedTileIndex].y);
            
            Vector2Int neighborDifPosition = Vector2Int.zero;
            switch (Random.Range(0, 4))
            {
                case 0:
                    neighborDifPosition = Vector2Int.right;
                    break;
                case 1:
                    neighborDifPosition = Vector2Int.left;
                    break;
                case 2:
                    neighborDifPosition = Vector2Int.up;
                    break;
                case 3:
                    neighborDifPosition = Vector2Int.down;
                    break;
            }
            
            if (IsTileAvailable(occupiedTilePosition.x + neighborDifPosition.x, occupiedTilePosition.y + neighborDifPosition.y))
            {
                int z = currentPrefabIndex;
                
                if (levelRules.GameType == GameType.g2048)
                {
                    currentGameBoard[randomOccupiedTileIndex] += new Vector3Int(0,0,-1);
                    z = currentGameBoard[randomOccupiedTileIndex].z;
                }
                
                return new Vector3Int(occupiedTilePosition.x + neighborDifPosition.x, occupiedTilePosition.y + neighborDifPosition.y, z);
            }
        }

        return Vector3Int.one * -1;
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