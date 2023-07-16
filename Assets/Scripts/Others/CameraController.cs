using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerStatusSystem playerStats;
    public static CameraController instance;
    public Camera mainCamera;
    public Transform target, pausedTarget;

    private float defaultZoom, currentZoom;
    
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {
        target = PlayerController.instance.transform;
        playerStats = PlayerStatusSystem.instance;
        defaultZoom = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (target!= null)
        {
            transform.position = new Vector3 (target.position.x, target.position.y, -10f);
        }
        if (playerStats.isPaused)
        {
            currentZoom = mainCamera.orthographicSize;
            mainCamera.orthographicSize = Mathf.MoveTowards(currentZoom, 1.5f, 0.6f * 0.02f);
        }
        else
        {
            currentZoom = mainCamera.orthographicSize;
            mainCamera.orthographicSize = Mathf.MoveTowards(currentZoom, defaultZoom, 1f * 0.02f);
        }
    }
}
