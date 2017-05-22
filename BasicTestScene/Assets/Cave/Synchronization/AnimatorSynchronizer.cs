using System;
using System.Threading;
using UnityEngine;
using AwesomeSockets.Domain.Sockets;

namespace Cave
{
    public class AnimatorSynchronizer
    {
        private static Animator[] animators = MonoBehaviour.FindObjectsOfType(typeof(Animator)) as Animator[];

        public static void ProcessMessage(InputMessage message)
        {
            if (animators.Length > 0)
            {
                animators[0].Play(animators[0].GetAnimatorTransitionInfo(0).nameHash, 0, message.animatorTime);
            }
        }

        public static void BuildMessage(InputMessage message)
        {
            if (animators.Length > 0)
            {
                message.animatorTime = animators[0].GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
        }

    }
}
