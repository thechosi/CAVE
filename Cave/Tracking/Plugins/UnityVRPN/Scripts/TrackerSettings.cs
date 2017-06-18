using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class TrackerSettings : MonoBehaviour
{
    [SerializeField]
    private TrackerHostSettings hostSettings;
    [SerializeField]
    private string objectName = "";
    [SerializeField]
    private int channel = 0;
    [SerializeField]
    private bool trackPosition = true;
    [SerializeField]
    private bool trackRotation = true;
    //added by pohl 09.12.16
    [SerializeField]
    private int nButtons;

    private bool[] pressedButtons;


    public TrackerHostSettings HostSettings
    {
        get { return hostSettings; }
        set
        {
            hostSettings = value;
        }
    }

    public string ObjectName
    {
        get { return objectName; }
        set
        {
            objectName = value;
        }
    }

    public int Channel
    {
        get { return channel; }
        set
        {
            channel = value;
        }
    }
    public int Buttons
    {
        get { return nButtons; }
        set
        {
            nButtons = value;
        }
    }
    public bool TrackPosition
    {
        get { return trackPosition; }
        set
        {
            trackPosition = value;
            StopCoroutine("Position");
            if (trackPosition && Application.isPlaying)
            {
                StartCoroutine("Position");
            }
        }
    }

    public bool TrackRotation
    {
        get { return trackRotation; }
        set
        {
            trackRotation = value;
            StopCoroutine("Rotation");
            if (trackRotation && Application.isPlaying)
            {
                StartCoroutine("Rotation");
            }
        }
    }


    private void Start()
    {
        if (trackPosition)
        {
            StartCoroutine("Position");
        }
        if (trackRotation)
        {
            StartCoroutine("Rotation");
        }
        // StartCoroutine ("GetAnalog");
        StartCoroutine("PushButton");
    }

    private IEnumerator Position()
    {
        while (true)
        {
            if (hostSettings.GetPosition(objectName, channel).Equals(new Vector3(-505, -505, -505)))
            {
                //transform.localPosition = new Vector3(0, 0, 0);
                //If there is no usable information coming from the tracker then leave the position as it is.
            }
            else {
                transform.localPosition = hostSettings.GetPosition(objectName, channel);
            }
            yield return null;
        }
    }

    private IEnumerator Rotation()
    {
        while (true)
        {
            transform.rotation = hostSettings.GetRotation(objectName, channel);
            yield return null;
        }
    }

    //added by pohl
    //private IEnumerator GetAnalog()
    public Vector2 getAnalog()
    {
        Vector2 analogStick = new Vector2();
        //while (true) {
        analogStick.x = (float)hostSettings.GetAnalog(objectName, 0);
        analogStick.y = (float)hostSettings.GetAnalog(objectName, 1);
        transform.parent.position = new Vector3(transform.parent.position.x, transform.parent.position.y + analogStick.y / 100, transform.parent.position.z + analogStick.x / 100);

        //	yield return null;
        //}
        return analogStick;
    }

    private IEnumerator PushButton()
    {
        pressedButtons = new bool[nButtons];
        while (true)
        {
            for (int i = 0; i < nButtons; i += 1)
            {
                pressedButtons[i] = hostSettings.GetButton(objectName, i);
            }
            yield return null;
        }
    }

    // 0 = trigger, 1 = left, 2 = right, 3 = middle
    public bool IsButtonPressed(int buttonChannel)
    {
        return pressedButtons != null && buttonChannel < pressedButtons.Length && pressedButtons[buttonChannel];
    }
}
