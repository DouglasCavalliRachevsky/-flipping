using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Piece currentSelectedPiece;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            DeselectPiece();
        }
    }

    private void DeselectPiece()
    {
        currentSelectedPiece = null;
    }

    public void SelectPiece(Piece selected)
    {
        if (currentSelectedPiece != null)
        {
            return;
        }

        //Debug.Log(selected.gameObject.name + " SELECTED " + selected.CurrentTile);
        currentSelectedPiece = selected;
    }

    public void SelectOtherPiece(Piece selected)
    {
        if (currentSelectedPiece == null)
        {
            return;
        }

        if (currentSelectedPiece.CurrentTile == selected.CurrentTile)
        {
            //Debug.Log("SAME TILE " + selected.CurrentTile);
            return;
        }

        if (AreNeighborTiles(selected.CurrentTile, currentSelectedPiece.CurrentTile) == false)
        {
            Debug.Log("NOT NEIGHBORS");
            return;
        }

        Debug.Log("YES NEIGHBORS");
        StackSelectedPieces(selected);
        DeselectPiece();
    }

    private void StackSelectedPieces(Piece target)
    {
        Transform currentSelectedTransform = currentSelectedPiece.transform;
        currentSelectedTransform.position = target.transform.position + new Vector3(0, 0.2f, 0);
        var currentSelectedPieceRot = currentSelectedTransform.rotation;
        currentSelectedTransform.Rotate(new Vector3(currentSelectedPieceRot.x + 180,
            currentSelectedPieceRot.y, currentSelectedPieceRot.z), Space.Self);

        target.UpdateTopPieces(currentSelectedPiece);
        currentSelectedPiece.UpdateBottomPieces(target);
        
        target.UpdateParentTransforms();
        currentSelectedPiece.UpdateAllStackCurrentTile();
    }

    private bool AreNeighborTiles(Vector3Int tileA, Vector3Int tileB)
    {
        Vector3Int dif = tileA - tileB;
        int totalDif = Mathf.Abs(dif.x) + Mathf.Abs(dif.y);

        return totalDif == 1;
    }
}