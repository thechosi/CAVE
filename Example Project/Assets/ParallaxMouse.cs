using UnityEngine;
using UnityClusterPackage;

public class ParallaxMouse : MonoBehaviour {

    private float x, y;

    // Use this for initialization
    void Start() {
        if ( NodeInformation.type.Equals("slave") )
        {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update () {
        x = ( Input.mousePosition.x - Screen.width/2 ) * 16/Screen.width;
        y = ( Input.mousePosition.y - Screen.height/2 ) * 10/Screen.height;

        //transform.localPosition = new Vector3(x, 5, y);                                           
    }
}
