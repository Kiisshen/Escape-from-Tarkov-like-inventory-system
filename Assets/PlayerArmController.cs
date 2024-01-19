using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmController : MonoBehaviour
{
    public GameObject player;
    public GameObject arms;
    public float rotationSpeed;
    private Camera mainCam;
    public float maxAngle;
    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        transform.position = mainCam.transform.position;
        transform.rotation = Quaternion.RotateTowards(arms.transform.rotation, mainCam.transform.rotation, rotationSpeed);

        Vector3 armEulers = arms.transform.rotation.eulerAngles;

        float angleY = arms.transform.rotation.eulerAngles.y;
        Vector3 eulers = player.transform.rotation.eulerAngles;
        angleY = ClampAngle(angleY, eulers.y - maxAngle, eulers.y + maxAngle);
        armEulers.y = angleY;

        float angleX = arms.transform.rotation.eulerAngles.x;
        Vector3 camEulers = mainCam.transform.rotation.eulerAngles;
        angleX = ClampAngle(angleX, camEulers.x - maxAngle, camEulers.x + maxAngle);
        armEulers.x = angleX;

        transform.rotation = Quaternion.Euler(armEulers);
    }
    public static float ClampAngle(float current, float min, float max)
    {
        float dtAngle = Mathf.Abs(((min - max) + 180) % 360 - 180);
        float hdtAngle = dtAngle * 0.5f;
        float midAngle = min + hdtAngle;

        float offset = Mathf.Abs(Mathf.DeltaAngle(current, midAngle)) - hdtAngle;
        if (offset > 0)
            current = Mathf.MoveTowardsAngle(current, midAngle, offset);
        return current;
    }
}