using System;
using UnityEngine;

namespace UnityClusterPackage
{
    class InputSynchronizer
    {
        private static float axisHorizontal;
        private static float axisVertical;

        public static void Synchronize(NetworkNode node)
        {
            if (NodeInformation.type.Equals("master"))
            {
                node.BroadcastMessage(new SynchroMessage(SynchroMessageType.SetHorizontalAxis, Input.GetAxis("Horizontal")));
                node.BroadcastMessage(new SynchroMessage(SynchroMessageType.SetVerticalAxis, Input.GetAxis("Vertical")));
                axisHorizontal = Input.GetAxis("Horizontal");
                axisVertical = Input.GetAxis("Vertical");
            }
            else
            {
                axisHorizontal = -2;
                axisVertical = -2;
                do
                {
                    SynchroMessage message = ((Client)node).WaitForNextMessage();

                    if (message.type == SynchroMessageType.SetHorizontalAxis)
                        axisHorizontal = (float)message.data;
                    else if (message.type == SynchroMessageType.SetVerticalAxis)
                        axisVertical = (float)message.data;
                    else
                        throw new Exception("Received unexpected message.");

                } while (axisHorizontal == -2 || axisVertical == -2);
            }
        }



        public static float GetAxis(string orientation)
        {
            if (orientation == "Vertical")
                return axisVertical;
            else
                return axisHorizontal;
        }

    }
}
