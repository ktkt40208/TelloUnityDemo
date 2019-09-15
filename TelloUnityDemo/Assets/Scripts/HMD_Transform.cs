using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMD_Transform : MonoBehaviour
{
    public GameObject OSC_Receiver;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = new Vector3(OSC_Receiver.GetComponent<OSC_Receiver>().HMD_head_tx, OSC_Receiver.GetComponent<OSC_Receiver>().HMD_head_ty, OSC_Receiver.GetComponent<OSC_Receiver>().HMD_head_tz);
        this.transform.localRotation = new Quaternion(OSC_Receiver.GetComponent<OSC_Receiver>().HMD_head_qx, OSC_Receiver.GetComponent<OSC_Receiver>().HMD_head_qy, OSC_Receiver.GetComponent<OSC_Receiver>().HMD_head_qz, OSC_Receiver.GetComponent<OSC_Receiver>().HMD_head_qw);
    }
}
