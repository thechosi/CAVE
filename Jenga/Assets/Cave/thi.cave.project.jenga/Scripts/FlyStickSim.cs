using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyStickSim : MonoBehaviour
{

    HashSet<InteractableItem> objectsHoveringOver = new HashSet<InteractableItem>();

    private InteractableItem closestItem;
    private InteractableItem interactingItem;
    public bool enable = false;

    // Use this for initialization
    void Start()
    {
        if (!enable)
        {
            Destroy(GameObject.Find("FlystickSim"));
        }
    }


    private void Update()
    {
        if (enable)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                float minDistance = float.MaxValue;

                float distance;
                foreach (InteractableItem item in objectsHoveringOver)
                {
                    distance = (item.transform.position - transform.position).sqrMagnitude;
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestItem = item;
                    }
                }

                interactingItem = closestItem;
                closestItem = null;

                GameObject towerObject = GameObject.Find("DynamicTower");
                if (towerObject != null)
                {
                    TowerInteractivity tower = towerObject.GetComponent<TowerInteractivity>();

                    tower.select(interactingItem.transform.gameObject);
                }


                if (interactingItem)
                {
                    if (interactingItem.isInteracting())
                    {
                        interactingItem.EndInteraction(this);
                    }

                    interactingItem.BeginInteraction(this);


                }

            }
            if (Input.GetKeyUp(KeyCode.Space) && interactingItem != null)
            {
                interactingItem.EndInteraction(this);
            }
        }

    }



    private void OnTriggerEnter(Collider other)
    {
        InteractableItem collidedItem = other.GetComponent<InteractableItem>();
        if (collidedItem)
        {
            objectsHoveringOver.Add(collidedItem);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableItem collidedItem = other.GetComponent<InteractableItem>();
        if (collidedItem)
        {
            objectsHoveringOver.Remove(collidedItem);
        }
    }
}
