using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TelloLib;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System;

public class TelloController : SingletonMonoBehaviour<TelloController> {

	private static bool isLoaded = false;

	private TelloVideoTexture telloVideoTexture;


    public GameObject OSC_Receiver;

    public float TelloCurrentPos_X;
    public float TelloCurrentPos_Y;
    public float TelloCurrentPos_Z;
    public float TelloPreviousPos_X;
    public float TelloPreviousPos_Y;
    public float TelloPreviousPos_Z;
    public float TelloInitialPos_X;
    public float TelloInitialPos_Y;
    public float TelloInitialPos_Z;

    public float TelloFloatingInitialPos_Z;

    public float TelloCurrentQuaternion_X;
    public float TelloCurrentQuaternion_Y;
    public float TelloCurrentQuaternion_Z;
    public float TelloCurrentQuaternion_W;
    public float TelloPreviousQuaternion_X;
    public float TelloPreviousQuaternion_Y;
    public float TelloPreviousQuaternion_Z;
    public float TelloPreviousQuaternion_W;

    public GameObject HMD_Yup;
    public GameObject Drone_Yup_Virtual;
    public GameObject Drone_Yup_Real;

    private float realDestinationZ;
    private Quaternion realRotationDifferenceY;
    private float realForwardDistanceZ;
    public float idealForwardDistance = 1.0f;
    Vector3 ForwardTargetPos;
    Vector3 m_verocity;
    public bool isFlying = false;
    public bool isLanding = false;

    // FlipType is used for the various flips supported by the Tello.
    public enum FlipType
	{

		// FlipFront flips forward.
		FlipFront = 0,

		// FlipLeft flips left.
		FlipLeft = 1,

		// FlipBack flips backwards.
		FlipBack = 2,

		// FlipRight flips to the right.
		FlipRight = 3,

		// FlipForwardLeft flips forwards and to the left.
		FlipForwardLeft = 4,

		// FlipBackLeft flips backwards and to the left.
		FlipBackLeft = 5,

		// FlipBackRight flips backwards and to the right.
		FlipBackRight = 6,

		// FlipForwardRight flips forewards and to the right.
		FlipForwardRight = 7,
	};

	// VideoBitRate is used to set the bit rate for the streaming video returned by the Tello.
	public enum VideoBitRate
	{
		// VideoBitRateAuto sets the bitrate for streaming video to auto-adjust.
		VideoBitRateAuto = 0,

		// VideoBitRate1M sets the bitrate for streaming video to 1 Mb/s.
		VideoBitRate1M = 1,

		// VideoBitRate15M sets the bitrate for streaming video to 1.5 Mb/s
		VideoBitRate15M = 2,

		// VideoBitRate2M sets the bitrate for streaming video to 2 Mb/s.
		VideoBitRate2M = 3,

		// VideoBitRate3M sets the bitrate for streaming video to 3 Mb/s.
		VideoBitRate3M = 4,

		// VideoBitRate4M sets the bitrate for streaming video to 4 Mb/s.
		VideoBitRate4M = 5,

	};

	override protected void Awake()
	{
		if (!isLoaded) {
			DontDestroyOnLoad(this.gameObject);
			isLoaded = true;
		}
		base.Awake();

		Tello.onConnection += Tello_onConnection;
		Tello.onUpdate += Tello_onUpdate;
		Tello.onVideoData += Tello_onVideoData;

		if (telloVideoTexture == null)
			telloVideoTexture = FindObjectOfType<TelloVideoTexture>();

	}

	private void OnEnable()
	{
		if (telloVideoTexture == null)
			telloVideoTexture = FindObjectOfType<TelloVideoTexture>();
	}

	private void Start()
	{
		if (telloVideoTexture == null)
			telloVideoTexture = FindObjectOfType<TelloVideoTexture>();

		Tello.startConnecting();
	}

	void OnApplicationQuit()
	{
		Tello.stopConnecting();
	}



    void setFlyingTrue() {
        isFlying = true;
    }

    void setFinishLandingTrue()
    {
        Debug.Log("setFinishLandingTrue: Tello.state.posX = " + Tello.state.posX + ", Tello.state.posY = " + Tello.state.posY + ", Tello.state.posZ = " + Tello.state.posZ);
        TelloFloatingInitialPos_Z = Tello.state.posZ;
        isLanding = false;
    }

    void RelativeVectorZ()
    {
        realDestinationZ = HMD_Yup.transform.position.y - Drone_Yup_Real.transform.position.y;
        //Debug.Log("Tello.state.posZ = " + Tello.state.posZ + ", HMD_Yup.transform.position.y = " + HMD_Yup.transform.position.y + ", Drone_Yup.transform.position. = " + Drone_Yup.transform.position.y);
        //Debug.Log("realDestinationZ = " + realDestinationZ);
    }

    void RelativeRotationY()
    {
        Drone_Yup_Virtual.transform.LookAt(HMD_Yup.transform);
        //Debug.Log("Drone_Yup_Virtual.transform.rotation = " + Drone_Yup_Virtual.transform.rotation);
        //Debug.Log("Drone_Yup_Real.transform.rotation = " + Drone_Yup_Real.transform.rotation);
        realRotationDifferenceY = Drone_Yup_Virtual.transform.rotation * Quaternion.Inverse(Drone_Yup_Real.transform.rotation);
        Debug.Log("realRotationDifferenceY = " + realRotationDifferenceY);
    }

    void RelativeForwardDistanceZ() {
        realForwardDistanceZ = Mathf.Pow((Mathf.Pow((HMD_Yup.transform.position.x - Drone_Yup_Virtual.transform.position.x), 2.0f) + Mathf.Pow((HMD_Yup.transform.position.z - Drone_Yup_Virtual.transform.position.z), 2.0f)), 0.5f) - idealForwardDistance;
        //ForwardTargetPos = new Vector3(Drone_Yup_Virtual.transform.position.x, Drone_Yup_Virtual.transform.position.y, Drone_Yup_Virtual.transform.position.z);
        //m_verocity += (ForwardTargetPos - Drone_Yup_Virtual.transform.position) * 5.0f;
        //m_verocity *= 0.5f;
        //Drone_Yup_Virtual.transform.position += m_verocity *= Time.deltaTime;
        Drone_Yup_Virtual.transform.position += realForwardDistanceZ * (Drone_Yup_Virtual.transform.forward) * Time.deltaTime;
    }
    // Update is called once per frame
    void Update () {


        if (Input.GetKey(KeyCode.T) || (OSC_Receiver.GetComponent<OSC_Receiver>().Y_controller1_b1_pressed > 0))
        {
            Tello.takeOff();
            isLanding = true;
            Invoke("setFlyingTrue", 0.2f);
            TelloInitialPos_X = Tello.state.posX;
            TelloInitialPos_Y = Tello.state.posY;
            TelloInitialPos_Z = Tello.state.posZ;
            Invoke("setFinishLandingTrue", 4.5f);

        }
        else if ((Input.GetKeyDown(KeyCode.L)) ||(OSC_Receiver.GetComponent<OSC_Receiver>().X_controller1_b3_pressed > 0))
        {
            Tello.land();
            
            isFlying = false;
        }
        //Debug.Log(OSC_Receiver.GetComponent<OSC_Receiver>().Y_controller1_b1_pressed);

        float lx = 0f;
		float ly = 0f;
		float rx = 0f;
		float ry = 0f;

        RelativeVectorZ();
        
        if ((realDestinationZ > 0.15f) && isFlying && !isLanding)
        {
            ly = 0.3f;
        }
        else if ((realDestinationZ < -0.15f) && isFlying && !isLanding)
        {
            ly = -0.3f;
        }
        else if ((realDestinationZ > 0.075f) && isFlying && !isLanding)
        {
            ly = -0.1f;
        }
        else if ((realDestinationZ < -0.075f) && isFlying && !isLanding)
        {
            ly = -0.1f;
        }
        else
        {
            ly = 0f;
        }


        RelativeRotationY();

        if (((realRotationDifferenceY.y > 0.15f) && (realRotationDifferenceY.w >= 0) && isFlying && !isLanding) || ((realRotationDifferenceY.y < -0.15f) && (realRotationDifferenceY.w < 0) && isFlying && !isLanding))
        {
            lx = 0.6f;
        }
        else if (((realRotationDifferenceY.y < -0.15f) && (realRotationDifferenceY.w >= 0) && isFlying && !isLanding) || ((realRotationDifferenceY.y > 0.15f) && (realRotationDifferenceY.w < 0) && isFlying && !isLanding))
        {
            lx = -0.6f;
        }
        else if (((realRotationDifferenceY.y > 0.075f) && (realRotationDifferenceY.w >= 0) && isFlying && !isLanding) || ((realRotationDifferenceY.y < -0.075f) && (realRotationDifferenceY.w < 0) && isFlying && !isLanding))
        {
            lx = -0.1f;
        }
        else if (((realRotationDifferenceY.y < -0.075f) && (realRotationDifferenceY.w >= 0) && isFlying && !isLanding) || ((realRotationDifferenceY.y > 0.075f && (realRotationDifferenceY.w < 0) && isFlying && !isLanding)))
        {
            lx = -0.1f;
        }
        else
        {
            lx = 0f;
        }

        //RelativeForwardDistanceZ();


        //Go Up!
        if (OSC_Receiver.GetComponent<OSC_Receiver>().B_controller2_b1_pressed > 0 || Input.GetKey(KeyCode.W)) { ly = 1;}
        //Go Down!
        if (OSC_Receiver.GetComponent<OSC_Receiver>().A_controller2_b3_pressed > 0 || Input.GetKey(KeyCode.S)) { ly = -1;}
        //Turn Right!
        if (OSC_Receiver.GetComponent<OSC_Receiver>().stick_controller2_a1x > 0.1f || Input.GetKey(KeyCode.D)) { lx = 1;}
        //Turn Left!
        if (OSC_Receiver.GetComponent<OSC_Receiver>().stick_controller2_a1x < -0.1f || Input.GetKey(KeyCode.A)) { lx = -1;}
        //Go Forward or Back!
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rx = 1;
        } else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rx = -1;
        }
        else {
            rx = OSC_Receiver.GetComponent<OSC_Receiver>().stick_controller1_a1x;
        }

        //Go Right or Left!
        if (Input.GetKey(KeyCode.UpArrow))
        {
            ry = 1;
        }else if (Input.GetKey(KeyCode.DownArrow))
        {
            ry = -1;
        }
        else
        {
            ry = OSC_Receiver.GetComponent<OSC_Receiver>().stick_controller1_a1y;
        }


        //Debug.Log("lx = " + lx + ", ly = " + ly + ", rx = " + rx + ", ry = " + ry);
        //Debug.Log("Tello.state.posX = " + Tello.state.posX + ", Tello.state.posY = " + Tello.state.posY + ", Tello.state.posZ = " + Tello.state.posZ);

        Tello.controllerState.setAxis(lx, ly, rx, ry); //float values
        //Tello.controllerState.setSpeedMode(int mode);

        if (Math.Abs(Tello.state.posX) <= 0.01 && Math.Abs(Tello.state.posY) <= 0.01 && Math.Abs(Tello.state.posZ) <= 0.01)
        {
            Debug.Log("Can't get the transform data, so use previous flame: TelloPreviousPos_X = " + TelloPreviousPos_X + ", TelloCurrentPos_X = " + TelloCurrentPos_X);
            TelloCurrentPos_X = TelloPreviousPos_X;
            TelloCurrentPos_Y = TelloPreviousPos_Y;
            TelloCurrentPos_Z = TelloPreviousPos_Z;
            TelloCurrentQuaternion_X = TelloPreviousQuaternion_X;
            TelloCurrentQuaternion_Y = TelloPreviousQuaternion_Y;
            TelloCurrentQuaternion_Z = TelloPreviousQuaternion_Z;
            TelloCurrentQuaternion_W = TelloPreviousQuaternion_W;

        }
        else
        {
            TelloCurrentPos_X = Tello.state.posX;
            TelloCurrentPos_Y = Tello.state.posY;
            TelloCurrentPos_Z = Tello.state.posZ;
            TelloCurrentQuaternion_X = Tello.state.quatX;
            TelloCurrentQuaternion_Y = Tello.state.quatY;
            TelloCurrentQuaternion_Z = Tello.state.quatZ;
            TelloCurrentQuaternion_W = Tello.state.quatW;
        }

        //Debug.Log("Tello_onUpdate : " + "x = " + TelloCurrentPos_X.ToString("f2") + ", y = " + TelloCurrentPos_Y.ToString("f2") + ", z = " + TelloCurrentPos_Z.ToString("f2"));
        TelloPreviousPos_X = TelloCurrentPos_X;
        TelloPreviousPos_Y = TelloCurrentPos_Y;
        TelloPreviousPos_Z = TelloCurrentPos_Z;
        TelloPreviousQuaternion_X = TelloCurrentQuaternion_X;
        TelloPreviousQuaternion_Y = TelloCurrentQuaternion_Y;
        TelloPreviousQuaternion_Z = TelloCurrentQuaternion_Z;
        TelloPreviousQuaternion_W = TelloCurrentQuaternion_W;
    }



    private void Tello_onUpdate(int cmdId)
	{
        //throw new System.NotImplementedException();
        //Debug.Log("Tello_onUpdate : " + Tello.state);


    }

    private void Tello_onConnection(Tello.ConnectionState newState)
	{
		//throw new System.NotImplementedException();
		//Debug.Log("Tello_onConnection : " + newState);
		if (newState == Tello.ConnectionState.Connected) {
            Tello.queryAttAngle();
            Tello.setMaxHeight(50);

			Tello.setPicVidMode(1); // 0: picture, 1: video
			Tello.setVideoBitRate((int)VideoBitRate.VideoBitRateAuto);
			//Tello.setEV(0);
			Tello.requestIframe();
		}
	}

	private void Tello_onVideoData(byte[] data)
	{
		//Debug.Log("Tello_onVideoData: " + data.Length);
		if (telloVideoTexture != null)
			telloVideoTexture.PutVideoData(data);
	}

}
