using System.Collections.Generic;
using UnityEngine;
using AwesomeSockets.Buffers;
using UnityEngine.Networking;

namespace Cave
{
    public enum EventType
    {
        OnCollisionEnter = 0,
        OnCollisionStay = 1,
        OnCollisionExit = 2,
        OnTriggerEnter = 3,
        OnTriggerStay = 4,
        OnTriggerExit = 5,
        OnClick = 6
    }

    public struct StoredEvent
    {
        public uint networkId;
        public short type;
        public uint data;
    }

    public class InputEventsMessage : ISynchroMessage
    {
        public List<StoredEvent> events = new List<StoredEvent>();
        
        public void Serialize(Buffer buffer)
        {
            Buffer.Add(buffer, events.Count);
            foreach (StoredEvent storedEvent in events)
            {
                Buffer.Add(buffer, storedEvent.networkId);
                Buffer.Add(buffer, storedEvent.data);
                Buffer.Add(buffer, storedEvent.type);
            }
        }

        public void Deserialize(Buffer buffer)
        {
            int length = Buffer.Get<int>(buffer);
            for (int i = 0; i < length; i++)
            {
                StoredEvent @event = new StoredEvent();
                @event.networkId = Buffer.Get<uint>(buffer);
                @event.data = Buffer.Get<uint>(buffer);
                @event.type = Buffer.Get<short>(buffer);
                events.Add(@event);
            }
        }

        public int GetLength()
        {
            return (sizeof(uint) * 2 + sizeof(short)) * events.Count + sizeof(uint);
        }
    }

    class EventSynchronizer
    {
        private static List<StoredEvent> collisions = new List<StoredEvent>();

        private static void TriggerCollisionEvent(EventType type, NetworkInstanceId networkInstanceId, uint data, Dictionary<NetworkInstanceId, NetworkIdentity> networkIdentities)
        {
            CollisionSynchronization[] collisionSynchronizations = networkIdentities[networkInstanceId].GetComponents<CollisionSynchronization>();
            NetworkInstanceId otherNetworkInstanceId = new NetworkInstanceId(data);

			foreach (CollisionSynchronization collisionSynchronization in collisionSynchronizations) {
				if (collisionSynchronization != null && networkIdentities.ContainsKey (otherNetworkInstanceId)) {
					if (type == EventType.OnCollisionEnter)
						collisionSynchronization.OnSynchronizedCollisionEnter (networkIdentities [otherNetworkInstanceId].gameObject);
					else if (type == EventType.OnCollisionStay)
						collisionSynchronization.OnSynchronizedCollisionStay (networkIdentities [otherNetworkInstanceId].gameObject);
					else if (type == EventType.OnCollisionExit)
						collisionSynchronization.OnSynchronizedCollisionExit (networkIdentities [otherNetworkInstanceId].gameObject);
					else if (type == EventType.OnTriggerEnter)
						collisionSynchronization.OnSynchronizedTriggerEnter (networkIdentities [otherNetworkInstanceId].gameObject);
					else if (type == EventType.OnTriggerStay)
						collisionSynchronization.OnSynchronizedTriggerStay (networkIdentities [otherNetworkInstanceId].gameObject);
					else if (type == EventType.OnTriggerExit)
						collisionSynchronization.OnSynchronizedTriggerExit (networkIdentities [otherNetworkInstanceId].gameObject);
				}
			}
        }

        private static void TriggerGUIEvent(EventType type, NetworkInstanceId networkInstanceId)
        {
            if (type == EventType.OnClick)
                GUISynchronizer.OnSynchronizedClick(networkInstanceId);
        }

        private static void CallScripts(List<StoredEvent> events)
        {
            Dictionary<NetworkInstanceId, NetworkIdentity> networkIdentities = NodeInformation.isMaster() ? NetworkServer.objects : ClientScene.objects;

            foreach (StoredEvent storedEvent in events)
            {
                NetworkInstanceId networkInstanceId = new NetworkInstanceId(storedEvent.networkId);
                if (networkIdentities.ContainsKey(networkInstanceId))
                {
                    EventType type = (EventType)storedEvent.type;
                    if (type == EventType.OnClick)
                        TriggerGUIEvent(type, networkInstanceId);
                    else
                        TriggerCollisionEvent(type, networkInstanceId, storedEvent.data, networkIdentities);
                }
            }
        }

        public static void ProcessMessage(InputEventsMessage message)
        {
            CallScripts(message.events);
        }

        public static void BuildMessage(InputEventsMessage message)
        {
            message.events = collisions;
            CallScripts(message.events);
            collisions = new List<StoredEvent>();
        }

        public static void LogEvent(EventType type, NetworkInstanceId netId, uint data)
        {
            StoredEvent storedEvent;
            storedEvent.networkId = netId.Value;
            storedEvent.data = data;
            storedEvent.type = (short)type;
            collisions.Add(storedEvent);
        }
    }
}
