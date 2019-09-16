using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(-0.00f) <= 0.05 && Mathf.Abs(-0f) <= 0.05 && Mathf.Abs(-0.06f) <= 0.05)
        {
            Debug.Log("TimeFrame = " + Time.frameCount);
            

        }
    }
}
