using UnityEngine;

//[RequireComponent(typeof(Camera))]
public class CompleteCameraController : MonoBehaviour
{

    public float baseDistance = 5f;       // 停止時のカメラ―プレイヤー間の距離[m]
    public float baseCameraHeight = 2f;   // 停止時のカメラの高さ[m]
    public float chaseDamper = 3f;        // カメラの追跡スピード（追跡時のカメラ―プレイヤー間の距離がきまる）
    public GameObject Player;
    public Transform player;
    private Transform cam;

    void Start()
    {
        player = Player.transform;
        cam = this.gameObject.transform;
    }

    void FixedUpdate()
    {
        // カメラの位置を設定
        //var desiredPos = player.position - player.forward * baseDistance + Vector3.up * baseCameraHeight;
        var desiredPos = player.position - player.forward * baseDistance;
        cam.position = Vector3.Lerp(cam.position, desiredPos, Time.deltaTime * chaseDamper);

        // カメラの向きを設定
        cam.LookAt(player);
        //Quaternion.LookRotation(cam.right, player.position);

        //var aim = this.player.transform.position - this.transform.position;
        //var look = Quaternion.LookRotation(aim, Vector3.right);
        //this.transform.localRotation = look;
    }
}