using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Cave
{
    public abstract class CollisionSynchronization : MonoBehaviour
    {
        private CollisionType[] relevantTypes;
        private NetworkInstanceId netId;

        protected CollisionSynchronization(CollisionType[] relevantTypes)
        {
            this.relevantTypes = relevantTypes;
        }

        private void LogCollider(CollisionType type, Collider other)
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
                CollisionSynchronizer.LogCollision(type, netId, networkIdentity.netId);
            }
        }

        private void LogCollision(CollisionType type, Collision collisionInfo)
        {
            foreach (ContactPoint contact in collisionInfo.contacts)
            {
                LogCollider(type, contact.otherCollider);
            }
        }

        public void OnCollisionEnter(Collision collisionInfo)
        {
            if (Array.IndexOf(relevantTypes, CollisionType.OnCollisionEnter) > -1)
                LogCollision(CollisionType.OnCollisionEnter, collisionInfo);
        }

        public void OnCollisionStay(Collision collisionInfo)
        {
            if (Array.IndexOf(relevantTypes, CollisionType.OnCollisionStay) > -1)
                LogCollision(CollisionType.OnCollisionStay, collisionInfo);
        }

        public void OnCollisionExit(Collision collisionInfo)
        {
            if (Array.IndexOf(relevantTypes, CollisionType.OnCollisionExit) > -1)
                LogCollision(CollisionType.OnCollisionExit, collisionInfo);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (Array.IndexOf(relevantTypes, CollisionType.OnTriggerEnter) > -1)
                LogCollider(CollisionType.OnTriggerEnter, other);
        }

        public void OnTriggerStay(Collider other)
        {
            if (Array.IndexOf(relevantTypes, CollisionType.OnTriggerStay) > -1)
                LogCollider(CollisionType.OnTriggerStay, other);
        }

        public void OnTriggerExit(Collider other)
        {
            if (Array.IndexOf(relevantTypes, CollisionType.OnTriggerExit) > -1)
                LogCollider(CollisionType.OnTriggerExit, other);
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
