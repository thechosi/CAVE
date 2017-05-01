using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class TowerManager : MonoBehaviour
{
    public int rowsNumber = 18;
    public GameObject blockModel, setupBlockModel;
    public float stabilizingTime = 1.5f;
    public float waitDelay = 1f;
    public TowerState State
    {
        get { return _desiredState; }
        set
        {
            if (_desiredState == value)
                return;
            _desiredState = value;
            if (!IsTowerSleeping() && value != TowerState.FALLING)
                _currentState = TowerState.WAITING;
            else
                _currentState = value;
            OnStateChanged();
        }
    }
    public event TowerStateChangedEventHandler StateChanged;
    public event EventHandler RowAdded;
    public Transform Center
    {
        get { return towerCenter.transform; }
    }

    private float waitTimer;
    private float stabilizersTimer;
    private List<GameObject> stabilizers = new List<GameObject>();
    private List<GameObject> rows = new List<GameObject>();
    private OrbitCamera mainCamera;
    private TopBlockPlacer blockPlacer;
    private TowerState _desiredState, _currentState;
    private GameObject towerCenter;

    public enum TowerState
    {
        PLACING,
        REMOVING,
        WAITING,
        FALLING
    }

    #region Messages
    // Use this for initialization
    void Start()
    {
        mainCamera = Camera.main.GetComponent<OrbitCamera>();

        towerCenter = new GameObject("Center");
        towerCenter.transform.parent = transform;
        towerCenter.AddComponent<CenterMaintainer>();

        BuildTower();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (stabilizersTimer < stabilizingTime)
        {
            stabilizersTimer += Time.deltaTime;
            if (stabilizersTimer >= stabilizingTime)
            {
                stabilizersTimer = 0;
                foreach (GameObject obj in stabilizers)
                {
                    Destroy(obj);
                }
            }
        }

        switch (_currentState)
        {
            case TowerState.PLACING:
            case TowerState.REMOVING:
                BlockLastRows();
                break;
            case TowerState.WAITING:
                if (waitTimer < waitDelay)
                {
                    waitTimer += Time.deltaTime;
                } else if (IsTowerSleeping())
                {
                    _currentState = State;
                    OnStateChanged();
                }
                break;
        }
    }
    #endregion

    #region Public methods
    public bool IsTowerSleeping()
    {
        foreach (BlockManager bm in GetComponentsInChildren<BlockManager>())
        {
            if (!bm.GetComponent<Rigidbody>().IsSleeping())
            {
                return false;
            }
        }
        return true;
    }

    public BlockManager[] GetBlockManagers()
    {
        return GetComponentsInChildren<BlockManager>();
    }
    
    public GameObject[] GetAllRows()
    {
        return rows.ToArray();
    }
    #endregion

    #region Event handlers
    void HandleTopBlockPlacerDestroyed(object sender, EventArgs e)
    {
        var row = AddRow();
        var components = rows [rows.Count - 2].GetComponentsInChildren<BlockManager>();
        var component = components.OrderBy(x => x.transform.localPosition.z).ToArray() [1].gameObject.AddComponent<TopBlockPlacer>();
        component.BlockPlaced += HandleBlockPlaced;
        component.Destroyed += HandleTopBlockPlacerDestroyed;
        component.blockModel = blockModel;
        component.setupBlockModel = setupBlockModel;
        component.targetRow = row;
        component.enabled = false;
        blockPlacer = component;
    }
    
    void HandleBlockPlaced(object sender, BlockEventArgs e)
    {
        e.Manager.Faded += HandleBlockFaded;
        e.Manager.Fading += HandleBlockFading;
        e.Manager.Dropped += HandleBlockDropped;
        State = TowerState.REMOVING;
    }

    void HandleBlockDropped(object sender, BlockEventArgs e) // game over
    {

        if (!e.transform.parent.name.Contains("Row #1"))
        {
            State = TowerState.FALLING;
            //Debug.Log(e.transform.parent.name);
        }
               
    }

    void HandleBlockFading(object sender, BlockEventArgs e)
    {
        ToggleBlocksLock(true);
        e.Manager.enabled = true;
    }
    
    void HandleBlockFaded(object sender, BlockEventArgs e)
    {
        State = TowerState.PLACING;
    }
    #endregion

    #region Private methods
    GameObject AddRow()
    {
        GameObject towerRow = new GameObject();
        towerRow.transform.parent = transform;
        int rowNum = rows.Count + 1;
        if (rowNum % 2 != 0)
        {
            towerRow.name = "Row #" + rowNum + " (0)";
            towerRow.transform.localRotation = Quaternion.identity;
            
        } else
        {
            towerRow.name = "Row #" + rowNum + " (90)";
            towerRow.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        towerRow.transform.localPosition = new Vector3(0, rowNum * ((BoxCollider)blockModel.GetComponent<Collider>()).size.y, 0);
        rows.Add(towerRow);
        
        if (RowAdded != null)
            RowAdded(this, new EventArgs());
        
        return towerRow;
    }

    void BuildTower()
    {
        if (rows.Count > 0)
        {
            foreach (GameObject obj in rows)
            {
                Destroy(obj);
            }
        }
        var blockSize = ((BoxCollider)blockModel.GetComponent<Collider>()).size;
        for (int y = 0; y < 4; y++)
        {
            GameObject stabilizer = new GameObject();
            stabilizer.transform.parent = transform;
            stabilizer.AddComponent<BoxCollider>();
            var height = blockSize.y * rowsNumber;
            ((BoxCollider)stabilizer.GetComponent<Collider>()).size = new Vector3(blockSize.x, height, blockSize.x);
            ((BoxCollider)stabilizer.GetComponent<Collider>()).center = new Vector3(0, height / 2, 0);
            stabilizer.transform.localPosition = new Vector3(blockSize.x, 0, 0);
            stabilizer.transform.RotateAround(transform.position, stabilizer.transform.up, 90 * y);
            stabilizers.Add(stabilizer);
        }
        stabilizersTimer = 0f;
        
        while (rows.Count < rowsNumber)
        {
            var row = AddRow();
            
            for (int z = -1; z <= 1; z++)
            {
                GameObject block = (GameObject)Instantiate(blockModel);
                block.transform.parent = row.transform;
                block.transform.localRotation = Quaternion.identity;
                block.transform.localPosition = new Vector3(0, 0, z * blockSize.z);
                var manager = block.GetComponent<BlockManager>();
                manager.Faded += HandleBlockFaded;
                manager.Fading += HandleBlockFading;
                manager.Dropped += HandleBlockDropped;
            }
        }
        HandleTopBlockPlacerDestroyed(this, new EventArgs());
        State = TowerState.REMOVING;
    }

    void BlockLastRows()
    {
        List<BlockManager> managers = new List<BlockManager>(rows [rows.Count - 1].GetComponentsInChildren<BlockManager>());
        managers = managers.Union(rows [rows.Count - 2].GetComponentsInChildren<BlockManager>()).ToList();
        
        foreach (BlockManager bm in managers)
        {
            bm.enabled = false;
        }
    }

    void OnStateChanged()
    {
       
        var isWaiting = false;
        switch (_currentState)
        {
            case TowerState.PLACING:
                mainCamera.Retarget(rows [rows.Count - 1].transform);
                mainCamera.distance = 4;
                ToggleBlocksLock(true);
                blockPlacer.enabled = true;
                Debug.Log("Placing");
                break;
            case TowerState.REMOVING:                
                mainCamera.Retarget(towerCenter.transform);
                mainCamera.distance = 6;
                blockPlacer.enabled = false;
                ToggleBlocksLock(false);
                Debug.Log("Removing");
                break;
            case TowerState.WAITING:
                mainCamera.Retarget(towerCenter.transform);
                mainCamera.distance = 10;
                blockPlacer.enabled = false;
                waitTimer = 0f;
                isWaiting = true;
                ToggleBlocksLock(true);
                Debug.Log("Waiting");
                break;
            case TowerState.FALLING:
                ToggleBlocksLock(true);
                GameManager.instance.State = GameState.OVER;
                Debug.Log("Falling");
                break;
        }
        
        if (StateChanged != null)
        {
            StateChanged(this, new TowerStateChangedEventArgs(State, isWaiting));
        }
    }

    void ToggleBlocksLock(bool value)
    {
        foreach (BlockManager bm in GetBlockManagers())
        {
            bm.enabled = !value;
        }
        if (!value)
            BlockLastRows();
    }
    #endregion
}