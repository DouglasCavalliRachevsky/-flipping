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

        if (level.AreNeighborTiles(selected.CurrentTile, currentSelectedPiece.CurrentTile) == false)
        {
            //Debug.Log("NOT NEIGHBORS");
            return;
        }

        //Debug.Log("YES NEIGHBORS");
        bool joinedPieces = level.Rules.JoinPiecesIfPossible(currentSelectedPiece, selected);

        if (level.Rules.MergePiecesEnabled && joinedPieces)
        {
            level.GenerateNewPiece(new Vector3Int(selected.CurrentTile.x, selected.CurrentTile.y , selected.CurrentTile.z + 1));
        }
        
        DeselectPiece();
    }
}