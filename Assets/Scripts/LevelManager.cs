using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelRules Rules;
    private List<GameObject> piecesGameObjects = new List<GameObject>();
    private List<Vector3Int> startingGameBoard = new List<Vector3Int>();

    
    public void InitializeNewLevel(bool restartLevel, LevelRules rules = null)
    {
        if (rules != null)
        {
            Rules = rules;
        }

        Camera.main.transform.position =
            new Vector3(Rules.GameBoardSize / 7f, Rules.GameBoardSize + 1, Rules.GameBoardSize / 2f);

        if (piecesGameObjects.Count > 0)
        {
            for (int i = piecesGameObjects.Count - 1; i >= 0; i--)
            {
                Destroy(piecesGameObjects[i]);
            }
        }

        piecesGameObjects = new List<GameObject>();
        if (restartLevel == false)
        {
            startingGameBoard = LevelGenerator.GenerateANewLevel(Rules, Rules.StartingNumberOfPieces);
        }
        
        foreach (var tile in startingGameBoard)
        {
            GenerateNewPiece(tile);
        }
    }

    public void GenerateNewPiece(Vector3Int tile)
    {
        Quaternion tileRotation = new Quaternion();
        if (Rules.PieceRandomRotationEnabled)
        {
            tileRotation = Quaternion.Euler(0f, 90f * (int) Random.Range(0, 4), 0f);
        }
        else
        {
            tileRotation = Quaternion.Euler(0f, 270f, 0f);
        }
        
        GameObject pieceGameObject = Instantiate(Rules.PieceList[tile.z],
            new Vector3(tile.x * Rules.TileSize, 0, tile.y * Rules.TileSize),
            tileRotation);
        pieceGameObject.name = "[" + tile.x + "," + tile.y + "]_" + Rules.PieceList[tile.z].name;
        pieceGameObject.transform.parent = transform;
        // pieceGameObject.AddComponent<MeshCollider>();
        // pieceGameObject.AddComponent<Rigidbody>().isKinematic = true;
        pieceGameObject.AddComponent<Piece>().StartingTile = tile;
        piecesGameObjects.Add(pieceGameObject);
    }

    public bool AreNeighborTiles(Vector3Int tileA, Vector3Int tileB)
    {
        Vector3Int dif = tileA - tileB;
        int totalDif = Mathf.Abs(dif.x) + Mathf.Abs(dif.y);

        return totalDif == 1;
    }

    public bool CheckUpVictory(int totalValidMovements)
    {
        //Debug.Log(totalValidMovements + "/" + MaxMovements() + " valid moves");
        if (totalValidMovements != MaxMovements())
        {
            return false;
        }
        
        if (Rules.GameType == GameType.Sandwich)
        {
            var piece = piecesGameObjects[0].GetComponent<Piece>();
            if (piece.GetUpperPiece().CurrentTile.z != 0
                || piece.GetLowerPiece().CurrentTile.z != 0)
            {
                return false;
            }
        }

        return true;
    }

    public int MaxMovements()
    {
        return startingGameBoard.Count - 1;
    }
}