using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector3Int StartingTile;
    public Vector3Int CurrentTile;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        CurrentTile = StartingTile;
    }

    private void OnMouseDown()
    {
        gameManager.SelectPiece(this);
    }
    
    
    private void OnMouseEnter()
    {
        gameManager.SelectOtherPiece(this);
    }
}
