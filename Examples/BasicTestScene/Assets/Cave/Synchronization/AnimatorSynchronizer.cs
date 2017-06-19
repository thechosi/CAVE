using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Buffers;
using UnityEngine.Networking;

namespace Cave
{
    public struct StoredAnimator
    {
        public uint networkId;
        public float animatorTime;
    }

    public class InputAnimatorMessage : ISynchroMessage
    {
        public List<StoredAnimator> animators = new List<StoredAnimator>();

        public void Serialize(Buffer buffer)
        {
            Buffer.Add(buffer, animators.Count);
            foreach (StoredAnimator animator in animators)
            {
                Buffer.Add(buffer, animator.networkId);
                Buffer.Add(buffer, animator.animatorTime);
            }
        }

        public void Deserialize(Buffer buffer)
        {
            int length = Buffer.Get<int>(buffer);
            for (int i = 0; i < length; i++)
            {
                StoredAnimator animator = new StoredAnimator();
                animator.networkId = Buffer.Get<uint>(buffer);
                animator.animatorTime = Buffer.Get<float>(buffer);
                animators.Add(animator);
            }
        }

        public int GetLength()
        {
            return (sizeof(float) + sizeof(uint)) * animators.Count + sizeof(uint);
        }
    }

    public class AnimatorSynchronizer
    {
        public static void ProcessMessage(InputAnimatorMessage message)
        {
            Dictionary<NetworkInstanceId, NetworkIdentity> networkIdentities = ClientScene.objects;

            foreach (StoredAnimator storedAnimator in message.animators)
            {
                NetworkInstanceId networkInstanceId = new NetworkInstanceId(storedAnimator.networkId);
                if (networkIdentities.ContainsKey(networkInstanceId))
                {
                    Animator animator = networkIdentities[networkInstanceId].GetComponent<Animator>();
                    if (animator != null)
                        animator.Update(storedAnimator.animatorTime - animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                }
            }
        }

        public static void BuildMessage(InputAnimatorMessage message)
        {
            foreach (KeyValuePair<NetworkInstanceId, NetworkIdentity> networkIdentity in NetworkServer.objects)
            {
                if (networkIdentity.Value.gameObject.GetComponent<Animator>() != null)
                {
                    StoredAnimator animator = new StoredAnimator();
                    animator.networkId = networkIdentity.Key.Value;
                    animator.animatorTime = networkIdentity.Value.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
                    message.animators.Add(animator);
                }
            }
        }
    }
}
