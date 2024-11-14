using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPlacer : MonoBehaviour
{
    public static Action OnBuildingPlaced;
    private List<GameObject> placedGameObject = new();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;

        placedGameObject.Add(newObject);
        return placedGameObject.Count - 1;
    }
        public void RotateLastPlacedObject(float rotationAmount)
    {
        if (placedGameObject.Count == 0)
            return;

        GameObject lastObject = placedGameObject[placedGameObject.Count - 1];
        lastObject.transform.Rotate(0, rotationAmount, 0, Space.Self);
        OnBuildingPlaced?.Invoke();
    }

    private void Update()
    {
        // Rotate the last placed object when right-clicking
        if (Input.GetMouseButtonDown(1))
        {
            RotateLastPlacedObject(90f);
        }
    }
    
    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if(placedGameObject.Count <= gameObjectIndex || placedGameObject[gameObjectIndex] == null)
            return;
        Destroy(placedGameObject[gameObjectIndex]);
        placedGameObject[gameObjectIndex] = null;
    }

}
