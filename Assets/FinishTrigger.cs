using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.SceneManagement;

public class FisnishTrigger : MonoBehaviour
{
    [SerializeField] 
    private GameObject endScreen = null;

    private bool sceneActive = false;

    public void OnTriggerEnter(Collider collision)
    {
      if(collision.tag == "Hero")
      {
        endScreen.SetActive(true);
        sceneActive = true;
      }
    }
  private void Update()
   {
    if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && sceneActive)
    {
      endScreen.SetActive(false);
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   }
}
