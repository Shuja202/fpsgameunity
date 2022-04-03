using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameramove : MonoBehaviour
{
    public Transform myplayerhead;
    public float startFOV, targetFOV, FOVSpeed;
    public Camera myCamera;

    // Start is called before the first frame update
    void Start()
    {
        myCamera = FindObjectOfType<Camera>();
        startFOV = myCamera.fieldOfView;
        targetFOV = startFOV;

    }

    private void LateUpdate()
    {
        transform.rotation = myplayerhead.transform.rotation;
        transform.position = myplayerhead.transform.position;
        transform.localScale = myplayerhead.transform.localScale;

        myCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, FOVSpeed * Time.deltaTime);

    }

    public void zoomOut()
    {
        targetFOV = startFOV;
    }

    public void zoomIn(float a)
    {
        targetFOV = a;
    }
   
}
