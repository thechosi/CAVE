using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 pan = Vector3.zero;
    public float distance = 5.0f;
    public float maxDistance = 20;
    public float minDistance = .6f;
    public float xSpeed = 200.0f;
    public float ySpeed = 200.0f;
    public int yMinLimit = -80;
    public int yMaxLimit = 80;
    public float heightMin = 0.5f;
    public float heightMax = 40;
    public int zoomRate = 40;
    public float panSpeed = 0.4f;
    public bool panHorizontal = true;
    public bool panVertical = true;
    public float zoomDampening = 5.0f;
    public bool lockInput = false;

    private Vector3 currentTargetPosition;
    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private Quaternion currentRotation, desiredRotation;
    private Quaternion rotation;
    private Vector3 position;
    private Vector3 currentPan;

    void Start()
    {
        Init();
    }
    void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        //If there is no target, create a temporary target at 'distance' from the cameras current viewpoint
        if (!target)
        {
            GameObject go = new GameObject("Cam Target");
            go.transform.position = transform.position + (transform.forward * distance);
            target = go.transform;
        }

        //distance = Vector3.Distance(transform.position, target.position);
        currentDistance = distance;
        //distance = distance;

        //be sure to grab the current rotations as starting points.
        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;

        xDeg = Vector3.Angle(Vector3.right, transform.right);
        yDeg = Vector3.Angle(Vector3.up, transform.up);

        currentPan = pan;
    }

    /*
     * Camera logic on LateUpdate to only update after all character movement logic has been handled. 
     */
    void LateUpdate()
    {
        if (!lockInput)
        {
            // If Control and Alt and Middle button? ZOOM!
            if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl))
            {
                distance -= Input.GetAxis("Mouse Y") * Time.deltaTime * zoomRate * 0.125f * Mathf.Abs(distance);
            }
            // If right button is selected? ORBIT
            else if (Input.GetMouseButton(1))
            {
                xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            }
            // otherwise if middle mouse is selected, we pan
            else if (Input.GetMouseButton(2))
            {
                if (panHorizontal)
                    pan += transform.right * Input.GetAxis("Mouse X") * panSpeed;
                if (panVertical)
                    pan += Vector3.up * Input.GetAxis("Mouse Y") * panSpeed;
            }
        }

        ////////OrbitAngle

        //Clamp the vertical axis for the orbit
        yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
        // set camera rotation 
        desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
        currentRotation = transform.rotation;

        rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
        transform.rotation = rotation;

        ////////Orbit Position

        // affect the desired Zoom distance if we roll the scrollwheel
        distance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(distance);
        //clamp the zoom min/max
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        // For smoothing of the zoom, lerp distance
        currentDistance = Mathf.Lerp(currentDistance, distance, Time.deltaTime * zoomDampening);

        ////////Camera Pan
        currentPan = Vector3.Lerp(currentPan, pan, Time.deltaTime * zoomDampening);

        ////////Target move
        currentTargetPosition = Vector3.Lerp(currentTargetPosition, target.position, Time.deltaTime * zoomDampening);

        // calculate position based on the new currentDistance 
        position = currentTargetPosition - (rotation * Vector3.forward * currentDistance + currentPan);
        position.Set(position.x, Mathf.Clamp(position.y, heightMin, heightMax), position.z);

        transform.position = position;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    public void Retarget(Transform target)
    {
        this.target = target;
        pan = Vector3.zero;
    }
}