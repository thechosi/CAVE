using UnityEngine;
using System.Collections;
using System;

public class ArrowDrag : MonoBehaviour
{
    bool mouseDown = false;

    public event EventHandler MouseDown;
    public event EventHandler MouseUp;

    public Color downColor = new Color(0.32f, 0.55f, 0f);
    private Color originalColor;
	
    void Start()
    {
        originalColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.State == GameState.PLAYING)
        {
            RaycastHit hit;
            if (Input.GetMouseButton(0))
            {
                if (!mouseDown && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if (hit.collider == GetComponent<Collider>())
                    {
                        mouseDown = true;
                        if (MouseDown != null)
                            MouseDown(this, new EventArgs());
                    }
                }
            } else
            {
                if (mouseDown)
                {
                    mouseDown = false;
                    if (MouseUp != null)
                        MouseUp(this, new EventArgs());
                }
            }
            
            if (mouseDown)
                GetComponent<Renderer>().material.color = downColor;
            else
                GetComponent<Renderer>().material.color = originalColor;
        }
    }
}