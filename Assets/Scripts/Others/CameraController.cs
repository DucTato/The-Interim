using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Camera mainCamera;
    public Transform target;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {
        target = PlayerController.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target!= null)
        {
            transform.position = new Vector3 (target.position.x, target.position.y, -10f);
        }
    }
}
