
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField]
    private Material previewMaterialPrefab;
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject preafabe, Vector2Int size)
    {
        previewObject = Instantiate(preafabe);
        PreparePreavie(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size)
    {
       if(size.x > 0 || size.y > 0)
       {
         cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
         cellIndicatorRenderer.material.mainTextureScale = size;
       }
    }

    private void PreparePreavie(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }
    
    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        if(previewObject != null)
            Destroy(previewObject);
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        if(previewObject != null)
        {
            MovePreview(position);
            AppleFeedbackToPreview(validity);
        }
        MoveCursor(position);
        AppleFeedbackToCursor(validity);
        
    }

    private void AppleFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        
        c.a = 0.5f;
        previewMaterialInstance.color = c;
    }

    private void AppleFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        
        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
    }

    private void MoveCursor(Vector3 position)
    {
       cellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(
            position.x, 
            position.y + previewYOffset, 
            position.z);
    }

    // public void RotatePreviewObject(float angle)
    // {
    //     if (previewObject != null)
    //     {
    //         previewObject.transform.Rotate(0, angle, 0, Space.Self);
    //     }
    // }

    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        AppleFeedbackToCursor(false);
    }
}
