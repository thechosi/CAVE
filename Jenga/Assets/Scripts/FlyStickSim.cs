using System.Collections;
using System.Collections.Generic;
using Cave;
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
            if (InputSynchronizer.GetKeyDown("space"))
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
                if (towerObject != null && interactingItem != null)
                {
                    TowerInteractivity tower = towerObject.GetComponent<TowerInteractivity>();

                    if (interactingItem.name.Contains(TowerInteractivity.MaxRow.ToString()) && interactingItem.GetComponent<Renderer>().material.color != Color.green)
                    {
                        return;
                    }
                    if(interactingItem.GetComponent<Renderer>().material.color != Color.green)
                    {
                        if (tower.FirstSelected == null)
                        {
                            tower.select(interactingItem.transform.gameObject);
                        }
                        else
                        {
                            return;
                        }
                    }
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
            if (InputSynchronizer.GetKeyUp("space") && interactingItem != null)
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
