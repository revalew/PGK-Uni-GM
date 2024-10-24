using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Przeciagnij_Pusc : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{

    [SerializeField] 
    GameObject PrefabToInstantiate; //Prefab to spawn for menu

    [SerializeField] 
    RectTransform UIDragElement; //UI Element to click

    [SerializeField]
    RectTransform Canvas; //Cache the reference to canva ???

    //Data to store pointer position
    private Vector2 mOrginalLocalPointerPosition;
    private Vector3 mOrginalPanelLocalPosition;
    private Vector2 mOrginalPosition;

    

    void Start()
    {
      //Initial position of the Item
      mOrginalPosition = UIDragElement.localPosition;
    }
 public void OnBeginDrag(PointerEventData data)
 {
    mOrginalPanelLocalPosition = UIDragElement.localPosition;
    RectTransformUtility.ScreenPointToLocalPointInRectangle
    (
      Canvas,
      data.position,
      data.pressEventCamera,
      out mOrginalLocalPointerPosition);
 }
 public void OnDrag(PointerEventData data)
 {
    Vector2 localPointerPosition;
    if(RectTransformUtility.ScreenPointToLocalPointInRectangle
    (
      Canvas,
      data.position,
      data.pressEventCamera,
      out localPointerPosition))
    {
      Vector3 offsetToOrignal = localPointerPosition - mOrginalLocalPointerPosition;

      UIDragElement.localPosition = mOrginalPanelLocalPosition + offsetToOrignal;

    }

    
 }
  public void OnEndDrag(PointerEventData data)
 {
    StartCoroutine(Coroutine_MoveUIElement(UIDragElement, mOrginalPosition, 0.5f));

    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    if(Physics.Raycast(ray, out hit, 1000.0f))
    {
      Vector3 worldPoint = hit.point;
      CreateObject(worldPoint);
    }

   void CreateObject (Vector3 position)
   {
     if(PrefabToInstantiate == null)
     {
      Debug.Log("No prefab");
     }

     GameObject obj = Instantiate(
        PrefabToInstantiate,
        position,
        Quaternion.identity
     );
   }
    
    IEnumerator Coroutine_MoveUIElement(RectTransform r, Vector2 targetPosition, float duration = 0.1f)
    {
      float elsapedTime = 0;
      Vector2 startintPos = r.localPosition;
      while(elsapedTime< duration)
      {
        r.localPosition = Vector2.Lerp(
          startintPos,
          targetPosition,
          (elsapedTime / duration));
          elsapedTime += Time.deltaTime;
          yield return new WaitForEndOfFrame();
      }
      r.localPosition = targetPosition;
    }
 }
}
