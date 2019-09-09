using UnityEngine;
using System.Collections;

public class OSC_Sender : MonoBehaviour
{


    public OSC oscReference;

    public GameObject TelloState;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        OscMessage message = new OscMessage();
        message.address = "/telloPos";
        message.values.Add(TelloState.GetComponent<TelloController>().TelloCurrentPos_X);
        message.values.Add(TelloState.GetComponent<TelloController>().TelloCurrentPos_Y);
        message.values.Add(TelloState.GetComponent<TelloController>().TelloCurrentPos_Z);
        oscReference.Send(message);

        Debug.Log(TelloState.GetComponent<TelloController>().TelloCurrentPos_X);
    }

    void OnMouseDown()
    {
        OscMessage message = new OscMessage();
        message.address = "/test";
        message.values.Add(transform.position.x);
        message.values.Add(transform.position.y);
        message.values.Add(transform.position.z);
        oscReference.Send(message);
    }
}
