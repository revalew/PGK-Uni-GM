using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    public ObjPlacer objectplacer;

    IBuildingState buildingState;

    [SerializeField]
    private InputMenager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private DataBase database;

    [SerializeField]
    private GameObject gridVisualization;

    private GridData floorData, objectData;


    [SerializeField]
    private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private void Start()
    {
        StopPlacement();
        floorData = new();
        objectData = new();
    }
    
    public void StartPlacement (int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        buildingState = new PlacementState(ID, grid, preview, database, floorData, objectData, objectplacer);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemoveState(grid, preview, floorData, objectData, objectplacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }
    private void PlaceStructure()
    {
        if(inputManager.IsPointerOverUI())
        {
            return;
        }
        
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
    }

    // private bool CheckPlacementValidity(Vector3Int gridPosition, int seleectedObjectIndex)
    // {
    //     GridData selectedData = database.objectData[seleectedObjectIndex].ID == 0 ? 
    //     floorData : 
    //     objectData;

    //     return selectedData.CanPlaceObjectAt(gridPosition, database.objectData[seleectedObjectIndex].Size);
    // }

    private void StopPlacement()
    {
        if(buildingState == null)
            return;
        gridVisualization.SetActive(false);
        buildingState.EndState();

        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        if(buildingState == null)
            return;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if(lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);

            lastDetectedPosition = gridPosition;
        }
        
        // if (Input.GetMouseButtonDown(1))
        // {
        //     preview.RotatePreviewObject(90f);  // Rotate 90 degrees around Y-axis
        // }
    }
}
