using AwesomeSockets.Sockets;

namespace UnityClusterPackage
{
    public class Client : NetworkNode
    {
        
        public override void Connect()
        {
            connections.Add(AweSock.TcpConnect(NodeInformation.serverIp, 8888));
            InitializeSelf();
        }
        
        void InitializeSelf()
        {
           ParticleSynchronizer.InitializeFromClient(this);
           RigidBodySynchronizer.InitializeFromClient(this);
        }

        public override void FinishFrame()
        {
            BroadcastMessage(new SynchroMessage(SynchroMessageType.FinishedRendering, 0));
        }

        public SynchroMessage WaitForNextMessage()
        {
            return WaitForNextMessage(connections[0]);
        }

    }
}
