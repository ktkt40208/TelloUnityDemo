using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPReceiver : MonoBehaviour
{
    int LOCA_LPORT = 22222;
    static UdpClient udp;
    Thread thread;

    void Start()
    {
        udp = new UdpClient(LOCA_LPORT);
        //udp.Client.ReceiveTimeout = 1000;
        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start();
    }

    void Update()
    {
    }

    void OnApplicationQuit()
    {
        thread.Abort();
    }

    private static void ThreadMethod()
    {
        while (true)
        {
            IPEndPoint remoteEP = null;
            byte[] data = udp.Receive(ref remoteEP);
           
            

            // 変換した値
            int value = System.BitConverter.ToInt32(data, 0);


            //Debug.Log("バイト配列：");
            //foreach (var b in data)
            //    Debug.Log(b);
            float[] floatarray = new float[data.Length];
            data.CopyTo(floatarray, 0);

            Debug.Log("変換した値：" + floatarray);

            //string text = Encoding.ASCII.GetString(data);
            Debug.Log(value);
        }
    }
}