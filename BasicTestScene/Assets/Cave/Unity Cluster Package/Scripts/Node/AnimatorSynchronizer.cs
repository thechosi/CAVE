using System;
using System.Threading;
using UnityEngine;
using AwesomeSockets.Domain.Sockets;

namespace UnityClusterPackage
{
    public class AnimatorSynchronizer : MonoBehaviour
    {
        public void Update()
        {
           Animator animator = FindObjectOfType<Animator>();
            if (NodeInformation.type.Equals("master"))
            {
                Synchronizer.node.BroadcastMessage(new SynchroMessage(SynchroMessageType.SetAnimationTime,  animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
            }
            else
            {
                SynchroMessage message = ((Client)Synchronizer.node).WaitForNextMessage();

                if (message.type == SynchroMessageType.SetAnimationTime)
                    animator.Play(animator.GetAnimatorTransitionInfo(0).nameHash, 0, (float)message.data);
                else
                    throw new Exception("Received unexpected message.");
            }
        }

    }
}
