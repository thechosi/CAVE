using System.Collections.Generic;
using System.ComponentModel;
using AwesomeSockets.Domain.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using AwesomeSockets.Buffers;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Cave
{
    public struct GUIEventListener
    {
        public string method;
        public Object target;
    }

    public class GUISynchronizer : MonoBehaviour
    {
        private static Dictionary<NetworkIdentity, GUIEventListener[]> registeredListeners = new Dictionary<NetworkIdentity, GUIEventListener[]>();

        public static void Prepare()
        {
            Button[] buttons = Resources.FindObjectsOfTypeAll(typeof(Button)) as Button[];
            foreach (Button button in buttons)
            {
                NetworkIdentity networkIdentity = button.gameObject.GetComponent<NetworkIdentity>();
                if (networkIdentity != null)
                {
                    GUIEventListener[] eventListeners = new GUIEventListener[button.onClick.GetPersistentEventCount()];

                    for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
                    {
                        GUIEventListener eventListener = new GUIEventListener();
                        eventListener.method = button.onClick.GetPersistentMethodName(i);
                        eventListener.target = button.onClick.GetPersistentTarget(i);
                        button.onClick.SetPersistentListenerState(i, UnityEventCallState.Off);
                        eventListeners[i] = eventListener;
                    }

                    registeredListeners[networkIdentity] = eventListeners;
                    button.onClick.AddListener(() => OnClick(networkIdentity.netId));
                }
            }
        }

        public static void OnClick(NetworkInstanceId netId)
        {
            EventSynchronizer.LogEvent(EventType.OnClick, netId, 0);
        }

        public static void OnSynchronizedClick(NetworkInstanceId netId)
        {
            foreach (KeyValuePair<NetworkIdentity, GUIEventListener[]> listenerPerNetIdentity in registeredListeners)
            {
                if (listenerPerNetIdentity.Key.netId == netId)
                {
                    foreach (GUIEventListener eventListener in listenerPerNetIdentity.Value)
                    {
                        var type = eventListener.target.GetType();
                        var methodInfo = type.GetMethod(eventListener.method);
                        methodInfo.Invoke(eventListener.target, null);
                    }
                    break;
                }
            }
        }
    }

}