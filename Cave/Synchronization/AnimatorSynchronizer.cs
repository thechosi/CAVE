using System.Threading;
using UnityEngine;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Buffers;

namespace Cave
{

    public class InputAnimatorMessage : ISynchroMessage
    {
        public float animatorTime;

        public void Serialize(Buffer buffer)
        {
            Buffer.Add(buffer, animatorTime);
        }

        public void Deserialize(Buffer buffer)
        {
            animatorTime = Buffer.Get<float>(buffer);
        }

        public int GetLength()
        {
            return sizeof(float) * 1;
        }
    }

    public class AnimatorSynchronizer
    {
        private static Animator[] animators = MonoBehaviour.FindObjectsOfType(typeof(Animator)) as Animator[];

        public static void ProcessMessage(InputAnimatorMessage message)
        {
            if (animators.Length > 0)
            {
                animators[0].Update(message.animatorTime - animators[0].GetCurrentAnimatorStateInfo(0).normalizedTime);
            }
        }

        public static void BuildMessage(InputAnimatorMessage message)
        {
            if (animators.Length > 0)
            {
                message.animatorTime = animators[0].GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
        }

    }
}
