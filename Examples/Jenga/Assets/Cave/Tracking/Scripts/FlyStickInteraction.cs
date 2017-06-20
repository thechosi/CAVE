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
        lineRender = GetComponent<LineRenderer>();
        trackerSettings = GetComponent<TrackerSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        if (model == null)
        {
            model = GameObject.FindWithTag("InteractiveModel");
        }
        drawLaser();
    }

    public void sendRay()
    {
        if (Physics.Raycast(transform.position, transform.forward, out rHit, maxRayDist))
        {
            selectedPart = rHit.collider.gameObject.transform.parent.gameObject;
        }
    }

    public void sendRayForBlocks()
    {
		GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
		foreach (GameObject brick in bricks) 
		{
			Debug.Log ("B1 " + brick.transform.position.ToString () + "," + brick.transform.rotation.ToString () + ","  + brick.GetComponent<Collider>().bounds.center.ToString () + "," + brick.GetComponent<Collider>().bounds.extents.ToString ());
			Debug.Log ("B2 " + brick.transform.position.ToString () + "," + brick.transform.rotation.ToString () + ","  + brick.GetComponent<Collider>().bounds.center.ToString () + "," + brick.GetComponent<Collider>().bounds.extents.ToString ());
		}

		if (Physics.Raycast (transform.position, transform.forward, out rHit, maxRayDist)) 
		{
			selectedPart = rHit.collider.gameObject;
			Debug.Log ("S " + selectedPart.transform.position.ToString () + "," + selectedPart.transform.rotation.ToString ());
		} else
			selectedPart = null;
		Debug.Log (transform.position.x.ToString () + "," + transform.position.y.ToString() +  "," + transform.position.z.ToString() + " - " + transform.forward.x.ToString() + "," + transform.forward.y.ToString() + "," + transform.forward.z.ToString() + " - " + Cave.TimeSynchronizer.time);
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
    

}

