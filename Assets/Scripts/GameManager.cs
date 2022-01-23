using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private LevelManager level;
    private Piece currentSelectedPiece;

    private void Awake()
    {
        level = GetComponent<LevelManager>();
    }

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
        StackSelectedPieces(currentSelectedPiece, selected);
        DeselectPiece();
    }

    private void StackSelectedPieces(Piece origin, Piece target)
    {
        Transform originTransform = origin.transform;
        originTransform.position = target.transform.position + new Vector3(0, level.Rules.TileHeight, 0);
        var originRotation = originTransform.rotation;
        originTransform.Rotate(new Vector3(originRotation.x + 180,
            originRotation.y, originRotation.z), Space.Self);

        target.UpdateTopPieces(origin);
        origin.UpdateBottomPieces(target);
        
        target.UpdateParentTransforms();
        origin.UpdateAllStackCurrentTile();
    }

    private bool AreNeighborTiles(Vector3Int tileA, Vector3Int tileB)
    {
        Vector3Int dif = tileA - tileB;
        int totalDif = Mathf.Abs(dif.x) + Mathf.Abs(dif.y);

        return totalDif == 1;
    }
}