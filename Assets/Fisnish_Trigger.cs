using Unity.VisualScripting;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] 
    private GameObject endScreen = null;
    public void OnTriggerEnter()
    {
        endScreen.SetActive(true);
    }
  private void Update()
   {
    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
    {
      endScreen.SetActive(false);
    }
   }
}
