using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelRules levelRules;
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
        startingGameBoard = LevelGenerator.GenerateANewLevel(levelRules, levelRules.StartingNumberOfPieces);

        foreach (var tile in startingGameBoard)
        {
            GameObject pieceGameObject = Instantiate(levelRules.PieceList[tile.z],
                new Vector3(tile.x * levelRules.TileSize, 0, tile.y * levelRules.TileSize),
                Quaternion.identity);
            piecesGameObjects.Add(pieceGameObject);
        }
    }
}