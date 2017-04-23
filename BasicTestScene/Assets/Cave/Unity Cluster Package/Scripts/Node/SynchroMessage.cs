namespace UnityClusterPackage
{
    public enum SynchroMessageType
    {
        SetDeltaTime,
        SetParticleDeltaTime,
        SetAnimationTime,
        SetTime,
        FinishedRendering,
        SetParticleSeed,
        SetHorizontalAxis,
        SetVerticalAxis
    }

    public class SynchroMessage : UnityEngine.Networking.MessageBase
    {
        public SynchroMessageType type;
        public double data;

        public SynchroMessage()
        {

        }

        public SynchroMessage(SynchroMessageType type, double data)
        {
            this.type = type;
            this.data = data;
        }
    }
  }
