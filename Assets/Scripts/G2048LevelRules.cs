using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SandwichRule", menuName = "Rules/SandwichRule", order = 1)]
public class G2048LevelRules : LevelRules
{
    public override bool JoinPiecesIfPossible(Piece origin, Piece target)
    {
        if (origin.CurrentTile.z != target.CurrentTile.z)
        {
            return false;
        }

        base.JoinPiecesIfPossible(origin, target);
        
        target.gameObject.SetActive(false);
        origin.gameObject.SetActive(false);
        
        return true;
    }
}