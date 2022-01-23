using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRules : ScriptableObject
{
    [Header("Gameboard")]
    public int GameBoardSize = 4;
    public float TileSize = 1.2f;
    public float TileHeight = 0.2f;

    [Header("Pieces")]
    public bool PieceRandomRotationEnabled;
    public bool MergePiecesEnabled;
    public int StartingNumberOfPieces = 4;
    public int MostImportantPieceIndex = 0;
    public List<GameObject> PieceList;
    
    public virtual bool JoinPiecesIfPossible(Piece origin, Piece target)
    {
        Transform originTransform = origin.transform;
        originTransform.position = target.transform.position + new Vector3(0, TileHeight, 0);
        var originRotation = originTransform.rotation;
        originTransform.Rotate(new Vector3(originRotation.x + 180,
            originRotation.y, originRotation.z), Space.Self);
        
        return false;
    }
}
