using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class SpawnController : MonoBehaviour {

    private static SpawnController _spawnControllerInstance;
    private Student[] _studentList;
    private NavMeshPath[,] _pathList;
    private Dictionary<int, NavMeshPath> _alreadyUsedPaths = new Dictionary<int, NavMeshPath>();
    private ClockManagement _clockManagement;
    private int _pathID = 0;
    private Dictionary<int, GameObject> _instantiatedLineList = new Dictionary<int, GameObject>();

    [SerializeField] List<GameObject> _doors = new List<GameObject>();

    public GameObject agent;
    public GameObject lineObject;
    public GameObject doorsParent;
    

    public static SpawnController SpawnControllerInstance {
        get {return _spawnControllerInstance;}
    }


    private void Awake() {
        GameManager.onGameStateChanced += GameManagerOnGameStateChanged;
        _spawnControllerInstance = this;
        
    }


    private void OnDestroy() {
        GameManager.onGameStateChanced -= GameManagerOnGameStateChanged;
    }


    private void GameManagerOnGameStateChanged (GameState state) {
         if (state == GameState.RunSimulation) {
            _studentList = StudentInitialisation.StudentInitialisationInstance.GetStudentList();


            foreach (Transform door in doorsParent.transform) {
                _doors.Add(door.gameObject);
            }

            CalcPaths();

            ClockManagement.ClockManagementInstance.StartTime();
            _clockManagement = GameObject.Find("SimulationHandler").GetComponent<ClockManagement>();

            StartCoroutine(Spawn());
        }
    }
    
    /*

    234/235/236 zusammen als 234
    306/308/324/325/327/328/329/345/346/348 zusammen als 306
    304 Mensa
    700/720 zusammen als 700

    */

    private IEnumerator Spawn() {
        _studentList = CommuteController.CommuteControllerInstance.GetStudentList();
        int studentListSize = _studentList.Length;

        while (true) {

            int gameTime = (int) ClockManagement.ClockManagementInstance.GetTime();
            GameObject instantiatedAgent = null;
            GameObject instantiatedLine = null;

            foreach (Student student in _studentList) {
                if(student.check(gameTime) == 1) {
                    string[] routePoints = student.RoutePoints();

                    if(routePoints[0] == routePoints[1]) {
                        continue;
                    }

                    GameObject startDoor = FindDoor(routePoints[0]);
                    GameObject endDoor = FindDoor(routePoints[1]);

                    Vector3 spawnPoint = UnityEngine.Random.insideUnitSphere * 30f + startDoor.transform.position;
                    NavMeshHit hit;
                    NavMesh.SamplePosition (spawnPoint, out hit, 30f, 1);
                    spawnPoint = hit.position;
                    spawnPoint.y = 1;

                    instantiatedAgent = (GameObject)Instantiate(agent, spawnPoint, transform.rotation);
                    int startIndex = NameToIndex(startDoor.name);
                    int endIndex = NameToIndex(endDoor.name);
                    NavMeshPath path = _pathList[startIndex, endIndex];

                    instantiatedAgent.GetComponent<NavMeshAgentMovement>().SetSpeed(student.GetStudentSpeed());
                    instantiatedAgent.GetComponent<NavMeshAgentMovement>().SetPath(path.corners);
                    
                    if(!_alreadyUsedPaths.ContainsValue(path)) {
                        instantiatedLine = (GameObject)Instantiate(lineObject);
                        instantiatedLine.GetComponent<DrawPath>().DrawPathOnFloor(path.corners);
                        _alreadyUsedPaths.Add(_pathID, path);
                        _instantiatedLineList.Add(_pathID, instantiatedLine);
                        _pathID++;
                    } else {
                        foreach (var pair in _alreadyUsedPaths) {
                            if (pair.Value == path) {
                                _instantiatedLineList[pair.Key].GetComponent<DrawPath>().ChangeWidthOfLine();
                            }
                        }
                    }
                }
            }
            yield return new WaitForSeconds(3f);
        }
    }


    private GameObject FindDoor(string tag) {
        GameObject[] doors = GameObject.FindGameObjectsWithTag(tag);
        float randomPosition = UnityEngine.Random.Range(0,doors.Length);
        return doors[(int)randomPosition];
    }


    private void CalcPaths() {
        _pathList = new NavMeshPath[_doors.Count, _doors.Count];
        int pathcount = 0;
        foreach(GameObject startDoor in _doors) {
            foreach(GameObject finishDoor in _doors) {
                
                NavMeshPath path = new NavMeshPath();

                NavMesh.CalculatePath(startDoor.transform.position, finishDoor.transform.position, 1, path);
                
                if(path.corners.Length < 1) {
                    Debug.Log(startDoor.name + "  " + finishDoor.name);
                }

                if(path == null) {
                    throw new System.Exception("path is null");
                }
                
                _pathList[NameToIndex(startDoor.name), NameToIndex(finishDoor.name)] = path;
                pathcount++;
            }
        }
        Debug.Log("Path count: " + pathcount);
    }


    private int NameToIndex(string name) { // Get number of the door out of its name
        string number = name.Split(' ')[1];
        number = number.Substring(1, number.Length - 2);
        return int.Parse(number);
    }
}