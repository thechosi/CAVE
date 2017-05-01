using UnityEngine;
using System.Collections;
using System;

public class BlockManager : MonoBehaviour
{
    public Collider floorCollider;
    public GameObject arrowObject;
    public Color hoverColor = new Color(0.7f, 1f, 0.7f);
    public Color selectionColor = new Color(0.51f, 1f, 0.51f);

    [Range(0.5f, 45f)]
    public float
        angleTolerantion = 5f;

    private bool isSelected = false;
    private bool isMoved = false;
    private Mode mode = Mode.NONE;
    private float timer = -0.5f;
    private Vector3 lerpTo;
    private Vector3 screenPoint, offset;
    private GameObject pressedArrow;
    private Vector3 originalPosition;
    private TowerManager tower;

    public event BlockEventHandler Dropped;
    public event BlockEventHandler Fading;
    public event BlockEventHandler Faded;
    public event BlockEventHandler AngleExceeded;

    enum Mode
    {
        MOVING,
        FADING, 
        REVERSING,
        NONE
    }

    #region Messages
    void Start()
    {
        if (floorCollider == null)
            floorCollider = GameObject.Find("Terrain").GetComponent<Collider>();
        tower = GameObject.FindObjectOfType<TowerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == Mode.MOVING)
            GetComponent<Rigidbody>().useGravity = false;
        else
            GetComponent<Rigidbody>().useGravity = true;
        if (GameManager.instance.State == GameState.PLAYING)
        {
            switch (mode)
            {
                case Mode.NONE:
                    if (!isSelected)
                        originalPosition = transform.localPosition;
                    break;
                case Mode.MOVING:
                    MoveUpdate();
                    break;
                case Mode.FADING:
                    FadeUpdate();
                    break;
                case Mode.REVERSING:
                    ReverseUpdate();
                    break;
            }
            
            var rot = transform.rotation.eulerAngles;
            if ((rot.x > angleTolerantion && rot.x < 360 - angleTolerantion) ||
                (rot.z > angleTolerantion && rot.z < 360 - angleTolerantion))
            {
                if (AngleExceeded != null)
                    AngleExceeded(this, new BlockEventArgs(this));
            }

            if (Vector3.Distance(transform.localPosition, originalPosition) > ((BoxCollider)GetComponent<Collider>()).size.x * 1.1f && mode != Mode.FADING)
            {
                mode = Mode.FADING;
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Collider>().enabled = false;
                lerpTo = new Vector3(transform.position.x, tower.Center.transform.position.y * 3, transform.position.z);
                timer = 0f;
                if (Fading != null)
                    Fading(this, new BlockEventArgs(this));
            }

            RaycastUpdate();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Terrain")
        {
            Dropped(this, new BlockEventArgs(this));
        }
    }

    #endregion

    #region Update methods
    void MoveUpdate()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        transform.localPosition = new Vector3(curPosition.x, transform.localPosition.y, transform.localPosition.z);
    }

    void FadeUpdate()
    {
        if (isSelected)
            Deselect();
        timer += Time.deltaTime * 1.5f;
        transform.position = Vector3.Lerp(transform.position, lerpTo, timer);
        if (timer >= 1)
        {
            if (Faded != null)
                Faded(this, new BlockEventArgs(this));
            Destroy(gameObject);
        }
            
    }

    void ReverseUpdate()
    {
        if (!GetComponent<Rigidbody>().isKinematic)
            GetComponent<Rigidbody>().isKinematic = true;
        timer += Time.deltaTime * 1.5f;
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, timer);
        if (timer > 1)
        {
            timer = -0.5f;
            GetComponent<Rigidbody>().isKinematic = false;
            mode = Mode.NONE;
        }
    }

    void RaycastUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (hit.collider == GetComponent<Collider>())
                {
                    if (isSelected)
                        Deselect();
                    else
                        Select();
                }
            }

            if (isSelected)
                GetComponent<Renderer>().material.color = selectionColor;
            else if (hit.collider == GetComponent<Collider>())
                GetComponent<Renderer>().material.color = hoverColor;
            else
                GetComponent<Renderer>().material.color = Color.white;
        }
    }
    #endregion

    #region Select/deselect methods
    public void Select()
    {
        foreach (BlockManager obj in FindObjectsOfType<BlockManager>())
        {
            obj.Deselect();
        }
            
        isSelected = true;
        originalPosition = transform.localPosition;
        GetComponent<Renderer>().material.color = hoverColor;
        var orbitCamera = Camera.main.GetComponent<OrbitCamera>();
        orbitCamera.Retarget(transform.parent);
        orbitCamera.distance = 5;

        for (int i = 0; i <= 1; i++)
        {
            var arrow = (GameObject)GameObject.Instantiate(arrowObject);
            arrow.name = "Arrow";
            arrow.transform.parent = transform;
            arrow.transform.localRotation = Quaternion.Euler(90, 90 + (180 * i), 0);
            var x = ((BoxCollider)GetComponent<Collider>()).size.x - arrow.GetComponent<Renderer>().bounds.size.x / 2;
            if (i == 1)
                x = -x;
            arrow.transform.localPosition = new Vector3(x, 0, 0);
            var arrowDrag = arrow.GetComponent<ArrowDrag>();
            arrowDrag.MouseDown += HandleArrowPressed;
            arrowDrag.MouseUp += HandleArrowReleased;
        }
    }

    public void Deselect()
    {
        isSelected = false;
        if (mode != Mode.FADING && isMoved)
        {
            isMoved = false;
            timer = -0.5f;
            mode = Mode.REVERSING;
        }
        GetComponent<Renderer>().material.color = Color.white;
        foreach (Transform t in transform)
        {
            if (t.gameObject.name == "Arrow")
                Destroy(t.gameObject);
        }
    }
    #endregion

    #region Arrow Event Handlers
    void HandleArrowPressed(object sender, EventArgs e)
    {
        mode = Mode.MOVING;
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.localPosition - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        isMoved = true;
    }

    void HandleArrowReleased(object sender, EventArgs e)
    {
        if (mode == Mode.MOVING)
            mode = Mode.NONE;
    }
    #endregion
}
