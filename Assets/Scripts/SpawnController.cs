using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class SpawnController : MonoBehaviour {

    private static SpawnController _spawnControllerInstance;
    private Student[] _studentList;
    private NavMeshPath[,] _pathList;
    private ClockManagement _clockManagement;

    [SerializeField] List<GameObject> _doors = new List<GameObject>();

    public GameObject agent;
    public GameObject lineObject;
    public GameObject doorsParent;
    public Dictionary<int, NavMeshPath> alreadyUsedPaths = new Dictionary<int, NavMeshPath>();
    public Dictionary<int, GameObject> instantiatedLineList = new Dictionary<int, GameObject>();
    public int pathID = 0;


    // Instance for reference during run time
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
                    GameObject startDoor;
                    GameObject endDoor;

                    string[] routePoints = student.RoutePoints();

                    if(routePoints[0] == routePoints[1]) {
                        continue;
                    }

                    if(routePoints[0] == "Spawn") {
                        startDoor = GameObject.Find("SpawnPoint (" + student.GetSpawnID() + ")");
                    } else {
                        startDoor = FindDoor(routePoints[0]);
                    }

                    if(routePoints[1] == "Spawn") {
                        endDoor = GameObject.Find("SpawnPoint (" + student.GetSpawnID() + ")");
                    } else {
                        endDoor = FindDoor(routePoints[1]);
                    }

                    Vector3 spawnPoint = UnityEngine.Random.insideUnitSphere * 30f + startDoor.transform.position;
                    NavMeshHit hit;
                    NavMesh.SamplePosition (spawnPoint, out hit, 30f, 1);
                    spawnPoint = hit.position;
                    spawnPoint.y = 1;

                    instantiatedAgent = (GameObject)Instantiate(agent, spawnPoint, transform.rotation);
                    int startIndex = NameToIndex(startDoor.name);
                    int endIndex = NameToIndex(endDoor.name);
                    NavMeshPath path = _pathList[startIndex, endIndex];

                    instantiatedAgent.GetComponent<NavMeshAgentMovement>().SetPersonality(new float[]{student.GetSpeed(), 0.04f, 4f});
                    instantiatedAgent.GetComponent<NavMeshAgentMovement>().SetPath(path.corners);
                    
                    if(!alreadyUsedPaths.ContainsValue(path)) {
                        instantiatedLine = (GameObject)Instantiate(lineObject);
                        instantiatedLine.GetComponent<DrawPath>().DrawPathOnFloor(path.corners);
                        alreadyUsedPaths.Add(pathID, path);
                        instantiatedLineList.Add(pathID, instantiatedLine);
                        pathID++;
                    } else {
                        foreach (var pair in alreadyUsedPaths) {
                            if (pair.Value == path) {
                                instantiatedLineList[pair.Key].GetComponent<DrawPath>().ChangeWidthOfLine();
                            }
                        }
                    }
                }
            }
            yield return new WaitForSeconds(3f);
        }
    }


    // Find door from its name
    private GameObject FindDoor(string tag) {
        GameObject[] doors = GameObject.FindGameObjectsWithTag(tag);
        float randomPosition = UnityEngine.Random.Range(0,doors.Length);
        return doors[(int)randomPosition];
    }


    // Precalculate paths
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


    // Get number of the door out of its name
    private int NameToIndex(string name) { 
        string number = name.Split(' ')[1];
        number = number.Substring(1, number.Length - 2);
        return int.Parse(number);
    }
}