using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerMovementOnClick : MonoBehaviour
{

  public Camera cam;
  public NavMeshAgent agent;

  public ThirdPersonCharacter2 character;

  private void Start()
  {
    if (cam == null)
    {
      cam = Camera.main;
    }
    if (agent != null)
    {
      agent.gameObject.SetActive(true);
      agent.updateRotation = false;
    }
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      Ray ray = cam.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      {
        // Move the agent
        agent.SetDestination(hit.point);
      }
    }

    if (agent.remainingDistance > agent.stoppingDistance)
    {
      character.Move(agent.desiredVelocity, false, false);
    }
    else
    {
      character.Move(Vector3.zero, false, false);
    }
  }
}