using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_Transform : MonoBehaviour
{
    public GameObject TelloController;
    public GameObject Drone_Yup_Virtual;
    private bool initinalState = true;
    private float initialPx;
    private float initialPy;
    private float initialPz;
    public float initialHoveringHightZ = 1.1f; //床面から安定した高さにホバリングするのでその高さを名指しで指定する。

    void Start()
    {
        
    }

    void Update()
    {
        //if (TelloController.GetComponent<TelloController>().isFlying && initinalState && (TelloController.GetComponent<TelloController>().TelloCurrentPos_X > 0))
        //{
        //    initialPx = TelloController.GetComponent<TelloController>().TelloCurrentPos_X;
        //    initialPy = TelloController.GetComponent<TelloController>().TelloCurrentPos_Y;
        //    initialPz = TelloController.GetComponent<TelloController>().TelloCurrentPos_Z;
        //    Debug.Log("initialPx = " + initialPx + ", initialPy = " + initialPy + ", initialPz = " + initialPz );

    //    initinalState = false;
    //}else if (!TelloController.GetComponent<TelloController>().isFlying)
    //    {
    //        initinalState = true;
    //    }

        //最初期のまだドローンのポジションの第一更新（大きな数値が流れ込んでくる）が来る前の微小な数値を無視する為のZの閾値を設けている。
        if (TelloController.GetComponent<TelloController>().isLanding && (Mathf.Abs(TelloController.GetComponent<TelloController>().TelloCurrentPos_Z) > 0.02) && initinalState) 
        {
            initialPx = TelloController.GetComponent<TelloController>().TelloCurrentPos_X;
            initialPy = TelloController.GetComponent<TelloController>().TelloCurrentPos_Y;
            initialPz = TelloController.GetComponent<TelloController>().TelloCurrentPos_Z; // Zの値は、センサーからの数値のずれが激しいためこれまでのは用いない。毎回床面から安定した高さにホバリングするのでその高さを名指しで指定する。
            Debug.Log("initialPx = " + initialPx + ", initialPy = " + initialPy + ", initialPz = " + initialPz);
            initinalState = false;
                
        }else if (!TelloController.GetComponent<TelloController>().isFlying)
        {
            initinalState = true;
        }
        //Debug.Log("telloPosX = " + TelloController.GetComponent<TelloController>().TelloCurrentPos_X);
        float telloPx = TelloController.GetComponent<TelloController>().TelloCurrentPos_X;
        float telloPy = TelloController.GetComponent<TelloController>().TelloCurrentPos_Y;
        float telloPz = TelloController.GetComponent<TelloController>().TelloCurrentPos_Z;
        float telloFloatingFPz = TelloController.GetComponent<TelloController>().TelloFloatingInitialPos_Z;
        float telloQx = TelloController.GetComponent<TelloController>().TelloCurrentQuaternion_X;
        float telloQy = TelloController.GetComponent<TelloController>().TelloCurrentQuaternion_Y;
        float telloQz = TelloController.GetComponent<TelloController>().TelloCurrentQuaternion_Z;
        float telloQw = TelloController.GetComponent<TelloController>().TelloCurrentQuaternion_W;
        this.transform.localPosition = new Vector3(telloPx - initialPx, telloPy - initialPy, -(telloPz - telloFloatingFPz + (-initialHoveringHightZ))); //床面から安定した高さにホバリングするのでその高さを名指しで指定する。
        this.transform.localRotation = new Quaternion(telloQx, telloQy, telloQz, telloQw);
        //Drone_Yup_Virtual.transform.position = new Vector3(telloPx - initialPx, -(telloPz - telloloatingFPz + (-1.1f)), telloPy - initialPy); //Only Inherit Position
    }
}
