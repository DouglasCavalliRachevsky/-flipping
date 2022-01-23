using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRules : ScriptableObject
{
    public int GameBoardSize = 4;
    public int StartingNumberOfPieces = 4;
    public float TileSize = 1.2f;
    public float TileHeight = 0.2f;
    public List<GameObject> PieceList;
    
    public virtual void JoinPieces(Piece origin, Piece target)
    {
        
    }
}
