using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_Transform : MonoBehaviour
{
    public GameObject TelloController;

    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log("telloPosX = " + TelloController.GetComponent<TelloController>().TelloCurrentPos_X);
        this.transform.position = new Vector3(TelloController.GetComponent<TelloController>().TelloCurrentPos_X, TelloController.GetComponent<TelloController>().TelloCurrentPos_Y, TelloController.GetComponent<TelloController>().TelloCurrentPos_Z);
        this.transform.rotation = new Quaternion(TelloController.GetComponent<TelloController>().TelloCurrentQuaternion_X, TelloController.GetComponent<TelloController>().TelloCurrentQuaternion_Y, TelloController.GetComponent<TelloController>().TelloCurrentQuaternion_Z, TelloController.GetComponent<TelloController>().TelloCurrentQuaternion_W);
    }
}
