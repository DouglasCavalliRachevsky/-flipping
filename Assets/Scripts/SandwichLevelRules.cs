using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SandwichRule", menuName = "Rules/SandwichRule", order = 1)]
public class SandwichLevelRules : LevelRules
{
    public override void JoinPieces(Piece origin, Piece target)
    {
        StackSelectedPieces(origin, target);
    }
    
    private void StackSelectedPieces(Piece origin, Piece target)
    {
        Transform originTransform = origin.transform;
        originTransform.position = target.transform.position + new Vector3(0, TileHeight, 0);
        var originRotation = originTransform.rotation;
        originTransform.Rotate(new Vector3(originRotation.x + 180,
            originRotation.y, originRotation.z), Space.Self);

        target.UpdateTopPieces(origin);
        origin.UpdateBottomPieces(target);
        
        target.UpdateParentTransforms();
        origin.UpdateAllStackCurrentTile();
    }
}
