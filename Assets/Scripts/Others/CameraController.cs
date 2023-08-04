using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraType type;
    private PlayerStatusSystem playerStats;
    private PlayerController playerRef;
    public static CameraController instance;
    public Camera mainCamera;
    public Transform target;
    private bool isZoom;
    private float defaultZoom, currentZoom, zoomValue;
    [SerializeField] private float rotateSpeed, waitTime;
    
    // private float x = -90f;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {
        if (type == CameraType.InGame)
        {
            //playerStats = PlayerStatusSystem.instance;
            playerRef = PlayerController.instance;
            target = playerRef.transform;
            defaultZoom = 5f;
        }
        isZoom = false;
        zoomValue = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case CameraType.InGame:
                if (isZoom)
                {
                    currentZoom = mainCamera.orthographicSize;
                    mainCamera.orthographicSize = Mathf.MoveTowards(currentZoom, zoomValue, 0.6f * 0.02f);
                }
                else
                {
                    currentZoom = mainCamera.orthographicSize;
                    mainCamera.orthographicSize = Mathf.MoveTowards(currentZoom, defaultZoom, 1f * 0.02f);
                }
                transform.position = new Vector3(target.position.x, target.position.y, -10f);
                break;
            case CameraType.MainMenu:
                if (waitTime > 0)
                    waitTime -= Time.deltaTime;
                else
                {
                    // This line works fine but the rotation speed is inconsistent
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector2.zero), Time.deltaTime * rotateSpeed);

                    // This method also works but the rotation speed is consistent through out the entire transition
                    //x = Mathf.MoveTowards(x, 0f, rotateSpeed * Time.deltaTime);
                    //transform.rotation = Quaternion.Euler(x, 0f, 0f);
                }
                
                break;
        }
        
    }
    public void CameraZoom (Transform zoomTarget, bool value, float zoomAmount)
    {
        isZoom = value;
        target = zoomTarget;
        zoomValue = zoomAmount;
    }
    public void CameraZoom (bool value)
    {
        target = playerRef.transform;
        isZoom = value;
    }
    public enum CameraType
    {
        MainMenu = 0,
        InGame = 1
    }
}
