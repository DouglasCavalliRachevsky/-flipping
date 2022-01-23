using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector3Int StartingTile;
    public Vector3Int CurrentTile;
    private GameManager gameManager;
    [SerializeField]
    private Piece topPiece;
    [SerializeField]
    private Piece bottomPiece;
    private Transform defaultParent;

    private void Start()
    {
        defaultParent = transform.parent;
        gameManager = defaultParent.GetComponent<GameManager>();
        CurrentTile = StartingTile;
    }

    private void OnMouseDown()
    {
        gameManager.SelectPiece(GetUpperPiece());
    }
    
    
    private void OnMouseEnter()
    {
        gameManager.SelectOtherPiece(GetUpperPiece());
    }

    public Piece GetLowerPiece()
    {
        if (bottomPiece == null)
        {
            return this;
        }

        return bottomPiece.GetLowerPiece();
    }
    
    public Piece GetUpperPiece()
    {
        if (topPiece == null)
        {
            return this;
        }

        return topPiece.GetUpperPiece();
    }
    
    public void UpdateTopPieces(Piece newTop)
    {
        topPiece = newTop;
    }

    public void UpdateBottomPieces(Piece newBottom)
    {
        if (bottomPiece != null)
        {
            bottomPiece.InvertBottomPieces();
        }
        
        topPiece = bottomPiece;
        bottomPiece = newBottom;
    }

    private void InvertBottomPieces()
    {
        if (bottomPiece != null)
        {
            bottomPiece.InvertBottomPieces();
        }
        
        Piece aux = bottomPiece;
        bottomPiece = topPiece;
        topPiece = aux;
    }

    public void UpdateParentTransforms()
    {
        transform.parent = defaultParent;
        
        if (topPiece != null)
        {
            topPiece.UpdateParentTransforms();
            transform.parent = topPiece.transform;
        }
    }
    
    public void UpdateCurrentTileRecursively()
    {
        if (bottomPiece != null)
        {
            bottomPiece.UpdateCurrentTileRecursively();
        }

        CurrentTile = GetLowerPiece().CurrentTile;
    }
    
    public void UpdateAllStackCurrentTile()
    {
        GetUpperPiece().UpdateCurrentTileRecursively();
    }
}
