using System.Collections;
using System.Collections.Generic;
using Cave;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnSphere : MonoBehaviour {

    [Tooltip("Defines which prefab object should be spawned.")]
    public Transform obj;
    [Tooltip("Defines after which ammount of passed frames the object will spawn. Use an value over 1. Can be changed dynamically ingame with w or q.")]
    public int spawnSpeed = 60;
    [Tooltip("Multiplies the ammount of spawned objects by the defined value. Use an value over one. Can be changed dynamically ingame with s or a.")]
    public int multiplicator = 1;
    [Tooltip("Defines how much the spawn time will increase or decrease when pressing w or q.")]
    public int spawnSpeedIncrease = 60;
    // Frame counter
    float counter = 0f;
    bool toggle = false;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if (NodeInformation.type.Equals("master"))
        {
            // Press w or q to change frame spawn time and s or a to change multiplicator.
            if (Input.GetKeyDown(KeyCode.W))
            {
                spawnSpeed += spawnSpeedIncrease;
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                spawnSpeed -= spawnSpeedIncrease;
                if (spawnSpeed < 1)
                {
                    spawnSpeed = 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                multiplicator++;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                multiplicator--;
                if (multiplicator < 1)
                {
                    multiplicator = 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                toggle = !toggle;
            }

            // Checks whether the "E" key is pressed. 
            if (Input.GetKey(KeyCode.E) || toggle)
            {
                // Checks whether the defined ammount of frames have passed.
                if (counter % (float)spawnSpeed == 0)
                {

                    for (int i = 0; i < multiplicator; i++)
                    {

                        // Spawns the object at the appropriate location with an random offset between 0 and 1 multiplied by an offset.
                        Transform spawnSpheresObjects = Instantiate(obj, new Vector3(transform.position.x + Random.value * 30, transform.position.y + Random.value * 8, transform.position.z + Random.value * 28), Quaternion.identity);
                        NetworkServer.Spawn(spawnSpheresObjects.gameObject);

                    }

                }

            }

            counter++;
        }

    }
}
