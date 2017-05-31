using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{


    private new Rigidbody rigidbody;

    private bool currentlyInteracting;

    private FlyStickSim attachedWandSim;
    private FlyStickInteraction attachedWand;


    private Transform interactionPoint;

    private Vector3 posDelta;

    private Quaternion rotationDelta;

    private float angle;

    private Vector3 axis;

    public float rotationFactor = 400f;
    public float velocityFactor = 200f;


    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        velocityFactor /= rigidbody.mass;
        rotationFactor /= rigidbody.mass;

        GameObject interactionObject = GameObject.Find("InteractionObject");

        if (interactionObject != null)
        {
            interactionPoint = interactionObject.transform;
        }
        else
        {
            Debug.Log("InteractionObject is missing!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (attachedWand && currentlyInteracting)
        {
            posDelta = attachedWand.transform.position - interactionPoint.position;
            // Debug.Log((posDelta * velocityFactor * Time.fixedDeltaTime).sqrMagnitude);
            this.rigidbody.velocity = posDelta * velocityFactor * Time.fixedDeltaTime; //TODO change to networkTime

            rotationDelta = attachedWand.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
            rotationDelta.ToAngleAxis(out angle, out axis);

            if (angle > 180)
            {
                angle -= 360;
            }
            this.rigidbody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
        }


        //TODO just for FlyStickSim, delete as Sim is no longer needed!
        else
        if (attachedWandSim && currentlyInteracting)
        {
            posDelta = attachedWandSim.transform.position - interactionPoint.position;
            // Debug.Log((posDelta * velocityFactor * Time.fixedDeltaTime).sqrMagnitude);
            this.rigidbody.velocity = posDelta * velocityFactor * Time.fixedDeltaTime; //TODO change to networkTime

            rotationDelta = attachedWandSim.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
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
        attachedWandSim = wand;
        interactionPoint.position = wand.transform.position;
        interactionPoint.rotation = wand.transform.rotation;
        interactionPoint.SetParent(transform, true);

        currentlyInteracting = true;
    }

    public void EndInteraction(FlyStickSim wand)
    {
        if (wand == attachedWandSim) //not needed for us
        {
            attachedWandSim = null;
            currentlyInteracting = false;
        }
    }

    public void BeginInteraction(FlyStickInteraction wand)
    {
        attachedWand = wand;
        interactionPoint.position = wand.transform.position;
        interactionPoint.rotation = wand.transform.rotation;
        interactionPoint.SetParent(transform, true);

        currentlyInteracting = true;
    }

    public void EndInteraction(FlyStickInteraction wand)
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
        if (collision.collider.name == "Plane" && this.transform.parent.name != "Row#1")
        {
            if (this.transform.GetComponent<Renderer>().material.color != Color.green)
            {
                TowerInteractivity tower = FindObjectOfType<TowerInteractivity>();
                TopBlockPlacer.PlayerChangeable = false;
                tower.Players[Player.ActivePlayer].Score++;
                Debug.Log("Turm fällt");


				InfoScreenManager infoScreen = FindObjectOfType<InfoScreenManager>();
				infoScreen.LoserView ();
            }
        }
    }
}