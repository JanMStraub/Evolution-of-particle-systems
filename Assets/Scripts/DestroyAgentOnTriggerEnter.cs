using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAgentOnTriggerEnter : MonoBehaviour {

    private int _studentId; 


    void OnTriggerEnter(Collider collider){
        _studentId = collider.gameObject.GetComponent<NavMeshAgentController>().GetStudentId();

        if (collider.tag == "Agent") {
            foreach (Student student in CommuteController.CommuteControllerInstance.GetStudentList()) {
                if (_studentId == student.GetId()) {
                    student.SetCurrentlyEnRoute(false);
                }
            }
            Destroy(collider.gameObject);
        }
    }
}
