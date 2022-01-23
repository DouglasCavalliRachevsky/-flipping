using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIController ui;
    private LevelManager level;
    private Piece currentSelectedPiece;
    private int totalValidMovements = 0;

    private void Awake()
    {
        level = GetComponent<LevelManager>();
        StartNewGame();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && ui.CurrentState == State.Gameplay)
        {
            DeselectPiece();

            if (level.CheckUpVictory(totalValidMovements))
            {
                ui.ChangeState(State.Victory);
                //Debug.Log("WIN");
            }
        }
    }

    [UsedImplicitly]
    public void StartNewGameWithSameRules()
    {
        StartNewGame();
        ui.ChangeState(State.Gameplay);
    }
    
    public void StartNewGame(LevelRules rules = null)
    {
        DeselectPiece();
        level.InitializeNewLevel(false, rules);
        totalValidMovements = 0;
    }
    
    [UsedImplicitly]
    public void RestartGame()
    {
        DeselectPiece();
        level.InitializeNewLevel(true);
        totalValidMovements = 0;
        ui.ChangeState(State.Gameplay);
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

        if (joinedPieces)
        {
            totalValidMovements++;

            if (level.Rules.MergePiecesEnabled)
            {
                level.GenerateNewPiece(new Vector3Int(selected.CurrentTile.x, selected.CurrentTile.y,
                    selected.CurrentTile.z + 1));
            }
        }

        DeselectPiece();
    }
}