using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Cave
{
    public abstract class CollisionSynchronization : MonoBehaviour
    {
        private EventType[] relevantTypes;
        private NetworkInstanceId netId;

        protected CollisionSynchronization(EventType[] relevantTypes)
        {
            this.relevantTypes = relevantTypes;
        }

        private void LogCollider(EventType type, Collider other)
        {
            if (netId.IsEmpty())
            {
                if (GetComponent<NetworkIdentity>() != null)
                    netId = GetComponent<NetworkIdentity>().netId;
                else
                    throw new MissingComponentException("CollisionSynchronization cannot be used without a NetworkIdentity component!");
            }

            NetworkIdentity networkIdentity = other.gameObject.GetComponent<NetworkIdentity>();
            if (networkIdentity != null)
            {
                EventSynchronizer.LogEvent(type, netId, networkIdentity.netId.Value);
            }
        }

        private void LogCollision(EventType type, Collision collisionInfo)
        {
            foreach (ContactPoint contact in collisionInfo.contacts)
            {
                LogCollider(type, contact.otherCollider);
            }
        }

        public void OnCollisionEnter(Collision collisionInfo)
        {
            if (Array.IndexOf(relevantTypes, EventType.OnCollisionEnter) > -1)
                LogCollision(EventType.OnCollisionEnter, collisionInfo);
        }

        public void OnCollisionStay(Collision collisionInfo)
        {
            if (Array.IndexOf(relevantTypes, EventType.OnCollisionStay) > -1)
                LogCollision(EventType.OnCollisionStay, collisionInfo);
        }

        public void OnCollisionExit(Collision collisionInfo)
        {
            if (Array.IndexOf(relevantTypes, EventType.OnCollisionExit) > -1)
                LogCollision(EventType.OnCollisionExit, collisionInfo);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (Array.IndexOf(relevantTypes, EventType.OnTriggerEnter) > -1)
                LogCollider(EventType.OnTriggerEnter, other);
        }

        public void OnTriggerStay(Collider other)
        {
            if (Array.IndexOf(relevantTypes, EventType.OnTriggerStay) > -1)
                LogCollider(EventType.OnTriggerStay, other);
        }

        public void OnTriggerExit(Collider other)
        {
            if (Array.IndexOf(relevantTypes, EventType.OnTriggerExit) > -1)
                LogCollider(EventType.OnTriggerExit, other);
        }


        public virtual void OnSynchronizedCollisionEnter(GameObject other)
        {

        }

        public virtual void OnSynchronizedCollisionStay(GameObject other)
        {

        }

        public virtual void OnSynchronizedCollisionExit(GameObject other)
        {

        }

        public virtual void OnSynchronizedTriggerEnter(GameObject other)
        {

        }

        public virtual void OnSynchronizedTriggerStay(GameObject other)
        {

        }

        public virtual void OnSynchronizedTriggerExit(GameObject other)
        {

        }
    }
}
