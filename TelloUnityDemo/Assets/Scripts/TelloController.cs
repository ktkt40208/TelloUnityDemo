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
    public float TelloCurrentQuaternion_X;
    public float TelloCurrentQuaternion_Y;
    public float TelloCurrentQuaternion_Z;
    public float TelloCurrentQuaternion_W;
    public float TelloPreviousQuaternion_X;
    public float TelloPreviousQuaternion_Y;
    public float TelloPreviousQuaternion_Z;
    public float TelloPreviousQuaternion_W;

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

	// Update is called once per frame
	void Update () {

        //if (Input.GetKeyDown(KeyCode.T)) {
        //	Tello.takeOff();
        //} else if (Input.GetKeyDown(KeyCode.L)) {
        //	Tello.land();
        //}
        if (Input.GetKey(KeyCode.T) || (OSC_Receiver.GetComponent<OSC_Receiver>().Y_controller1_b1_pressed > 0))
        {
            Tello.takeOff();
        }
        else if ((Input.GetKeyDown(KeyCode.L)) ||(OSC_Receiver.GetComponent<OSC_Receiver>().X_controller1_b3_pressed > 0))
        {
            Tello.land();
        }
        Debug.Log(OSC_Receiver.GetComponent<OSC_Receiver>().Y_controller1_b1_pressed);

        float lx = 0f;
		float ly = 0f;
		float rx = 0f;
		float ry = 0f;

        //Go Up!
        if (OSC_Receiver.GetComponent<OSC_Receiver>().B_controller2_b1_pressed > 0) { ly = 1;}
        //Go Down!
        if (OSC_Receiver.GetComponent<OSC_Receiver>().A_controller2_b3_pressed > 0) { ly = -1;}
        //Turn Right!
        if (OSC_Receiver.GetComponent<OSC_Receiver>().stick_controller2_a1x > 0.1f) { lx = 1;}
        //Turn Left!
        if (OSC_Receiver.GetComponent<OSC_Receiver>().stick_controller2_a1x < -0.1f) { lx = -1;}
        //Go Forward or Back!
        rx = OSC_Receiver.GetComponent<OSC_Receiver>().stick_controller1_a1x;
        //Go Right or Left!
        ry = OSC_Receiver.GetComponent<OSC_Receiver>().stick_controller1_a1y;

        Debug.Log("lx =" + lx + ", ly =" + ly + "rx =" + rx + ", ry =" + ry);

        Tello.controllerState.setAxis(lx, ly, rx, ry); //float values
        //Tello.controllerState.setSpeedMode(int mode);

        if (Tello.state.posX <= 0.05 && Tello.state.posY <= 0.05 && Tello.state.posZ <= 0.05)
        {
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
