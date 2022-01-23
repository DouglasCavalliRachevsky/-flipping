using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SandwichRule", menuName = "Rules/SandwichRule", order = 1)]
public class SandwichLevelRules : LevelRules
{
    public override bool JoinPiecesIfPossible(Piece origin, Piece target)
    {
        base.JoinPiecesIfPossible(origin, target);
        
        StackSelectedPieces(origin, target);
        
        return true;
    }
    
    private void StackSelectedPieces(Piece origin, Piece target)
    {
        target.UpdateTopPieces(origin);
        origin.UpdateBottomPieces(target);
        
        target.UpdateParentTransforms();
        origin.UpdateAllStackCurrentTile();
    }
}
