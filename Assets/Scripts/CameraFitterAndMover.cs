using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFitterAndMover : MonoBehaviour
{
    public float originalAspect = 9/16f;
    Camera cam;

    public Transform pointStandardPlay;
    public Transform pointBonusPlay;
    public Color bgColorStandardPlay;
    public Color bgColorBonusPlay;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        MatchCamera();
    }

    void MatchCamera()
    {
        float currentAspect = cam.aspect;     
        float aspectShift = originalAspect / currentAspect;
        cam.orthographicSize *= aspectShift;
    }

    public void MoveToBonusTable()
    {
        cam.transform.position = new Vector3(pointBonusPlay.position.x, pointBonusPlay.position.y, cam.transform.position.z);
        cam.backgroundColor = bgColorBonusPlay;
    }

    public void MoveToStandardTable()
    {
        cam.transform.position = new Vector3(pointStandardPlay.position.x, pointStandardPlay.position.y, cam.transform.position.z);
        cam.backgroundColor = bgColorStandardPlay;
    }
}
