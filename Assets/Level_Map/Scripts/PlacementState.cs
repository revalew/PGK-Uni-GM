using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    DataBase database;
    GridData floorData;
    GridData furnitureData;
    ObjPlacer objectPlacer;

     public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          DataBase database,
                          GridData floorData,
                          GridData furnitureData,
                          ObjPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.objectData.FindIndex(data => data.ID == ID);
         if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(
                database.objectData[selectedObjectIndex].Prefab,
                database.objectData[selectedObjectIndex].Size);
        }
        else
            throw new System.Exception($"No object with ID {ID}");
    }


    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if(placementValidity == false)
            return;
        int index = objectPlacer.PlaceObject(database.objectData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ? 
        floorData : 
        furnitureData;
        selectedData.AddObjectAt(gridPosition, 
            database.objectData[selectedObjectIndex].Size,
            database.objectData[selectedObjectIndex].ID,
            index);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition),false);
    }
        private bool CheckPlacementValidity(Vector3Int gridPosition, int seleectedObjectIndex)
    {
        GridData selectedData = database.objectData[seleectedObjectIndex].ID == 0 ? 
        floorData : 
        furnitureData;
        return selectedData.CanPlaceObjectAt(gridPosition, database.objectData[seleectedObjectIndex].Size);
    }
    public void UpdateState (Vector3Int gridPosition)
    {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
            
            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}

