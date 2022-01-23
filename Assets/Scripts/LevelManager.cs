using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelRules Rules;
    private List<GameObject> piecesGameObjects = new List<GameObject>();
    private List<Vector3Int> startingGameBoard = new List<Vector3Int>();

    private void Awake()
    {
        InitializeNewLevel();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            InitializeNewLevel();
        }
    }
#endif

    public void InitializeNewLevel()
    {
        if (piecesGameObjects.Count > 0)
        {
            for (int i = piecesGameObjects.Count - 1; i >= 0; i--)
            {
                Destroy(piecesGameObjects[i]);
            }
        }

        piecesGameObjects = new List<GameObject>();
        startingGameBoard = LevelGenerator.GenerateANewLevel(Rules, Rules.StartingNumberOfPieces);

        foreach (var tile in startingGameBoard)
        {
            Quaternion randomRotation = Quaternion.Euler(0f,90f * (int)Random.Range(0, 4), 0f);
            GameObject pieceGameObject = Instantiate(Rules.PieceList[tile.z],
                new Vector3(tile.x * Rules.TileSize, 0, tile.y * Rules.TileSize),
                randomRotation);
            pieceGameObject.name = "[" + tile.x + "," + tile.y + "]_" + Rules.PieceList[tile.z].name;
            pieceGameObject.transform.parent = transform;
            pieceGameObject.AddComponent<MeshCollider>();
            Piece piece = pieceGameObject.AddComponent<Piece>();
            piece.StartingTile = tile;
            piecesGameObjects.Add(pieceGameObject);
        }
    }
}