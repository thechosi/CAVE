using UnityEngine;
using System.Collections;


public class FlyStickInteraction : MonoBehaviour
{
    private RaycastHit rHit;
    private LineRenderer lineRender;
    private GameObject model, selectedPart;
    private TrackerSettings trackerSettings;

    public Transform origin;
    public Transform dest;
    public float maxRayDist = 10f;


    public GameObject SelectedPart
    {
        get
        {
            return selectedPart;
        }

        set
        {
            selectedPart = value;
        }
    }
    public TrackerSettings TrackerSettings
    {
        get
        {
            return trackerSettings;
        }

        set
        {
            trackerSettings = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        lineRender = this.GetComponent<LineRenderer>();
        trackerSettings = this.GetComponent<TrackerSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        if (model == null)
        {
            model = GameObject.FindWithTag("InteractiveModel");
            print(model.name);
        }
        drawLaser();
        rotateSelectedObject();
    }

    public void sendRay()
    {
        if (Physics.Raycast(this.transform.position, this.transform.up, out rHit, maxRayDist))
        {
            selectedPart = rHit.collider.gameObject.transform.parent.gameObject;
        }
    }

    public void sendRayForBlocks()
    {
        if (Physics.Raycast(this.transform.position, this.transform.up, out rHit, maxRayDist))
        {
            selectedPart = rHit.collider.gameObject;
        }
    }

    public void TestLog(string Button)
    {
        Debug.Log("Button Pressed " + Button);
    }

    private void drawLaser()
    {
        lineRender.SetPosition(0, origin.position);
        lineRender.SetPosition(1, dest.position);
    }

    public void trigger()
    {
        model.GetComponent<interactionTrigger>().trigger();
    }

    private void rotateSelectedObject()
    {
        Vector2 vec = trackerSettings.getAnalog();
        Vector3 rot = new Vector3(0, vec.x, 0);
        model.transform.Rotate(rot);
    }


}

