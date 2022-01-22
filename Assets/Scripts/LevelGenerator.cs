using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public LevelRules LevelRules;

    [Range(4, 16)] public int startingNumberOfPieces = 4;

    List<Vector3Int> currentGameBoard = new List<Vector3Int>();
    private const int MAX_GAMEBOARD_INDEX = LevelRules.GAME_BOARD_SIZE - 1;

    enum NeighborType
    {
        min,
        right,
        left,
        top,
        bottom,
        max
    }

#if UNITY_EDITOR
    private void Awake()
    {
        ResetLevel();
        GenerateLevel();
    }
    
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ResetLevel();
            GenerateLevel();
        }
    }
#endif

    public void GenerateLevel()
    {
        int lastPrefabIndex = 0;
        int prefabIndex = 0;
        for (int i = 0; i < startingNumberOfPieces; i++)
        {
            if (i > 1)
            {
                while (prefabIndex == lastPrefabIndex)
                {
                    prefabIndex = Random.Range(1, LevelRules.PieceList.Count - 1);
                }
            }

            var newOccupiedTile = GetAvailableTile(prefabIndex);
            currentGameBoard.Add(newOccupiedTile);

            Debug.Log("Tile " + i + " - " + newOccupiedTile);

            lastPrefabIndex = prefabIndex;
        }
    }

    private Vector3Int GetAvailableTile(int prefabIndex)
    {
        if (currentGameBoard.Count == 0)
        {
            return (new Vector3Int(Random.Range(0, MAX_GAMEBOARD_INDEX),
                Random.Range(0, MAX_GAMEBOARD_INDEX), prefabIndex));
        }

        while (true)
        {
            int randomOccupiedTileIndex = Random.Range(0, currentGameBoard.Count - 1);
            Vector3Int occupiedTile = currentGameBoard[randomOccupiedTileIndex];

            NeighborType neighborType =
                (NeighborType) Random.Range((int) NeighborType.min + 1, (int) NeighborType.max - 1);

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

    private bool IsTileAvailable(int x, int y)
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

    private bool IsOnGameBoard(int x, int y)
    {
        return x <= MAX_GAMEBOARD_INDEX
               && x >= 0
               && y <= MAX_GAMEBOARD_INDEX
               && y >= 0;
    }

    private void ResetLevel()
    {
        currentGameBoard = new List<Vector3Int>();
    }
}