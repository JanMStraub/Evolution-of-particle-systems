using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePlacer : MonoBehaviour {

    private GameObject _obstacle;

    private Camera _mainCamera;


    void Start() {
        _mainCamera = this.GetComponent<Camera>();
        _obstacle = GameObject.Find("Obstacle");
    }
    

    void Update() {
        if(Input.GetButtonDown("Fire1")) {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity)) {
                Instantiate(_obstacle, hit.point, _obstacle.transform.rotation);
                Debug.Log("Object placed");
            }
        }
        if(Input.GetButtonDown("Fire2")) {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity)) {
                Destroy((hit.collider).gameObject);
                Debug.Log("Object");
            }
        }
    }
}
