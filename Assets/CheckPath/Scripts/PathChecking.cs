using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityStandardAssets.Characters.ThirdPerson;

public class PathChecking : MonoBehaviour
{
  [SerializeField] private NavMeshSurface _surface = null; // GameObject with NavMesh (check scene in `PGK-Uni-GM/Assets/NavTest`)
  
  [SerializeField] private NavMeshAgent _agent = null; // Player prefab reference
  
  [SerializeField] private GameObject _player = null; // GameObject - not really needed but who knows (just agent.gameObject)
  
  [SerializeField] private GameObject _startingTile = null; // Where we want to spawn the player
  
  [SerializeField] private GameObject _finnishTile = null; // Place we want to be able to reach

  [SerializeField] public bool _pathAvailable = false; // Store info if target can be reached

  [SerializeField] private GameObject enemyUI = null;
  [SerializeField] private GameObject buildingUI = null;
  [SerializeField] private GameObject _pathNotFoundUI = null;

  [SerializeField] private NavMeshPath _navMeshPath = null; // Calculate the path

  private bool _playerSpawned = false; // Simple flag to spawn only 1 player / hero
  private ThirdPersonCharacter2 _character = null;

  public void CheckPath() // Run this on button click
  {
     _surface.BuildNavMesh(); // UPDATE NAVMESH

     _navMeshPath = new(); // Create path object

    // (CalculateNewPath()) ? (_pathAvailable = true;) : (_pathAvailable = false;);
    if (CalculateNewPath())
    {
        _pathAvailable = true;
        //_character = _agent.GetComponent<ThirdPersonCharacter2>();
    }
    else
    {
        _pathAvailable = false;
    }

    if (!_pathAvailable)
    {
        // POP-UP UI
      _pathNotFoundUI.SetActive(true);
    }
    // else
    // {
    //   if (!_playerSpawned) // Should we spawn a player?
    //   {
    //     // Spawn the player on the starting tile
    //     Vector3 pos = new Vector3(_startingTile.transform.position.x, _startingTile.transform.position.y + 0.01f, _startingTile.transform.position.z);
        
    //     _player = Instantiate(_agent.gameObject, pos, Quaternion.identity);
    //     _agent = _player.GetComponent<NavMeshAgent>();
    //     _playerSpawned = true;
    //   };

    //   if (_agent != null)
    //   {
    //     _agent.gameObject.SetActive(true);
    //     _agent.updateRotation = false;
    //   };
    // }
  }

  bool CalculateNewPath()
  {
    _agent.CalculatePath(_finnishTile.transform.position, _navMeshPath);

    if (_navMeshPath.status != NavMeshPathStatus.PathComplete) {
      return false;
    }
    else {
      return true;
    }
  }


  // // Use this after the level started. Hero will go to the finnish tile
  private void Update()
   {
    if (_pathAvailable && !_pathNotFoundUI.activeInHierarchy)
    {
      buildingUI.SetActive(false);
      enemyUI.SetActive(true);
    //   // Move the agent
    //   _agent.SetDestination(_finnishTile.transform.position);

    // if (_agent.remainingDistance > _agent.stoppingDistance)
    // {
    //   _character.Move(_agent.desiredVelocity, false, false);
     }
    // else
    // {
    //   _character.Move(Vector3.zero, false, false);
    // }
    // }

    //Animations w/ ThirdPersonCharacter2 controller

    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
    {
      _pathNotFoundUI.SetActive(false);
    }
  }
}