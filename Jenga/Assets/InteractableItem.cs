using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{


    private Rigidbody rigidbody;

    private bool currentlyInteracting;

    private FlyStickSim attachedWand;

    private Transform interactionPoint;

    private Vector3 posDelta;

    private Quaternion rotationDelta;

    private float angle;

    private Vector3 axis;

    public float rotationFactor = 400f;
    public float velocityFactor = 20000f;


    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        interactionPoint = new GameObject().transform;
        velocityFactor /= rigidbody.mass;
        rotationFactor /= rigidbody.mass;
    }

    // Update is called once per frame
    void Update()
    {
        if (attachedWand && currentlyInteracting)
        {
            posDelta = attachedWand.transform.position - interactionPoint.position;
            this.rigidbody.velocity = posDelta * velocityFactor * Time.fixedDeltaTime; //TODO change to networkTime

            rotationDelta = attachedWand.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
            rotationDelta.ToAngleAxis(out angle, out axis);

            if (angle > 180)
            {
                angle -= 360;
            }

            this.rigidbody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
        }
    }

    public void BeginInteraction(FlyStickSim wand)
    {
        attachedWand = wand;
        interactionPoint.position = wand.transform.position;
        interactionPoint.rotation = wand.transform.rotation;
        interactionPoint.SetParent(transform, true);

        currentlyInteracting = true;
    }

    public void EndInteraction(FlyStickSim wand)
    {
        if (wand == attachedWand) //not needed for us
        {
            attachedWand = null;
            currentlyInteracting = false;
        }
    }

    public bool isInteracting()
    {
        return currentlyInteracting;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Plane" && transform.parent.name != "Row#1")
        {
            Debug.Log("Turm fällt");
        }
    }

}
