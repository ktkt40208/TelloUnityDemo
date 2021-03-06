﻿using UnityEngine;
using System.Collections;

public class OSC_Receiver : MonoBehaviour
{

    public OSC osc;

    public float stick_controller1_a1x; //L Stick X
    public float stick_controller1_a1y; //L Stick Y
    public float Y_controller1_b1_pressed; //L B or Y
    public float middleFin_controller1_b2_pressed; //L 中指
    public float X_controller1_b3_pressed; //L A or X
    public float thumbFin_controller1_b4_pressed; //L 親指
    public float indexFin_controller1_b5_pressed; //L 人差し指

    public float stick_controller2_a1x; //R Stick X
    public float stick_controller2_a1y; //R Stick Y
    public float B_controller2_b1_pressed; //R B or Y
    public float middleFin_controller2_b2_pressed; //R 中指
    public float A_controller2_b3_pressed; //R A or X
    public float thumbFin_controller2_b4_pressed; //R 親指
    public float indexFin_controller2_b5_pressed; //R 人差し指

    public float HMD_head_tx; //
    public float HMD_head_ty; //
    public float HMD_head_tz; //
    public float HMD_head_qx; //
    public float HMD_head_qy; //
    public float HMD_head_qz; //
    public float HMD_head_qw; //


    // Use this for initialization
    void Start()
    {
        //osc.SetAddressHandler("/CubeXYZ", OnReceiveXYZ);
        osc.SetAddressHandler("/controller1:a1x", OnReceive_1_Stick_X);
        osc.SetAddressHandler("/controller1:a1y", OnReceive_1_Stick_Y);
        osc.SetAddressHandler("/controller1:b1:pressed", OnReceive_1_Button_Y);
        osc.SetAddressHandler("/controller1:b2:pressed", OnReceive_1_Middle);
        osc.SetAddressHandler("/controller1:b3:pressed", OnReceive_1_Button_X);
        osc.SetAddressHandler("/controller1:b4:pressed", OnReceive_1_Thumb);
        osc.SetAddressHandler("/controller1:b5:pressed", OnReceive_1_Index);

        osc.SetAddressHandler("/controller2:a1x", OnReceive_2_Stick_X);
        osc.SetAddressHandler("/controller2:a1y", OnReceive_2_Stick_Y);
        osc.SetAddressHandler("/controller2:b1:pressed", OnReceive_2_Button_Y);
        osc.SetAddressHandler("/controller2:b2:pressed", OnReceive_2_Middle);
        osc.SetAddressHandler("/controller2:b3:pressed", OnReceive_2_Button_X);
        osc.SetAddressHandler("/controller2:b4:pressed", OnReceive_2_Thumb);
        osc.SetAddressHandler("/controller2:b5:pressed", OnReceive_2_Index);

        osc.SetAddressHandler("/head:tx", OnReceive_HMD_head_tx);
        osc.SetAddressHandler("/head:ty", OnReceive_HMD_head_ty);
        osc.SetAddressHandler("/head:tz", OnReceive_HMD_head_tz);
        osc.SetAddressHandler("/head:qx", OnReceive_HMD_head_qx);
        osc.SetAddressHandler("/head:qy", OnReceive_HMD_head_qy);
        osc.SetAddressHandler("/head:qz", OnReceive_HMD_head_qz);
        osc.SetAddressHandler("/head:qw", OnReceive_HMD_head_qw);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //void OnReceiveXYZ(OscMessage message)
    //{
    //    float x = message.GetFloat(0);
    //    float y = message.GetFloat(1);
    //    float z = message.GetFloat(2);

    //    transform.position = new Vector3(x, y, z);
    //}

    void OnReceive_1_Stick_X(OscMessage message)
    {
        stick_controller1_a1x = message.GetFloat(0);
    }

    void OnReceive_1_Stick_Y(OscMessage message)
    {
        stick_controller1_a1y = message.GetFloat(0);
    }

    void OnReceive_1_Button_Y(OscMessage message)
    {
        Y_controller1_b1_pressed = message.GetFloat(0);
    }

    void OnReceive_1_Middle(OscMessage message)
    {
        middleFin_controller1_b2_pressed = message.GetFloat(0);
    }

    void OnReceive_1_Button_X(OscMessage message)
    {
        X_controller1_b3_pressed = message.GetFloat(0);
    }

    void OnReceive_1_Thumb(OscMessage message)
    {
        thumbFin_controller1_b4_pressed = message.GetFloat(0);
    }

    void OnReceive_1_Index(OscMessage message)
    {
        indexFin_controller1_b5_pressed = message.GetFloat(0);
    }


    void OnReceive_2_Stick_X(OscMessage message)
    {
        stick_controller2_a1x = message.GetFloat(0);
    }

    void OnReceive_2_Stick_Y(OscMessage message)
    {
        stick_controller2_a1y = message.GetFloat(0);
    }

    void OnReceive_2_Button_Y(OscMessage message)
    {
        B_controller2_b1_pressed = message.GetFloat(0);
    }

    void OnReceive_2_Middle(OscMessage message)
    {
        middleFin_controller2_b2_pressed = message.GetFloat(0);
    }

    void OnReceive_2_Button_X(OscMessage message)
    {
        A_controller2_b3_pressed = message.GetFloat(0);
    }

    void OnReceive_2_Thumb(OscMessage message)
    {
        thumbFin_controller2_b4_pressed = message.GetFloat(0);
    }

    void OnReceive_2_Index(OscMessage message)
    {
        indexFin_controller2_b5_pressed = message.GetFloat(0);
    }

    void OnReceive_HMD_head_tx(OscMessage message)
    {
        HMD_head_tx = message.GetFloat(0);
    }
    void OnReceive_HMD_head_ty(OscMessage message)
    {
        HMD_head_ty = message.GetFloat(0);
    }
    void OnReceive_HMD_head_tz(OscMessage message)
    {
        HMD_head_tz = message.GetFloat(0);
    }
    void OnReceive_HMD_head_qx(OscMessage message)
    {
        HMD_head_qx = message.GetFloat(0);
    }
    void OnReceive_HMD_head_qy(OscMessage message)
    {
        HMD_head_qy = message.GetFloat(0);
    }
    void OnReceive_HMD_head_qz(OscMessage message)
    {
        HMD_head_qz = message.GetFloat(0);
    }
    void OnReceive_HMD_head_qw(OscMessage message)
    {
        HMD_head_qw = message.GetFloat(0);
    }

}
