using System;
using System.Runtime.InteropServices;
using UnityEngine;
using AwesomeSockets.Domain.Sockets;

namespace UnityClusterPackage
{
    class ParticleSynchronizer
    {
        private static float lastParticleTime = 0;
        private static bool firstSyncTime = true;

        public static void Synchronize(NetworkNode node)
        {
            ParticleSystem[] particleSystems = MonoBehaviour.FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[];

            if (particleSystems.Length > 0)
            {
                if (NodeInformation.type.Equals("master"))
                {
                    float deltaTime = particleSystems[0].time - lastParticleTime;
                    if (deltaTime < 0)
                        deltaTime = particleSystems[0].time - lastParticleTime + particleSystems[0].main.duration;
                    node.BroadcastMessage(new SynchroMessage(SynchroMessageType.SetParticleDeltaTime, deltaTime));
                    lastParticleTime = particleSystems[0].time;
                    //Debug.Log(lastParticleTime);
                    //Debug.Log("P" + deltaTime);
                }
                else
                {
                    SynchroMessage message = ((Client)node).WaitForNextMessage();
                    float deltaTime;

                    if (message.type == SynchroMessageType.SetParticleDeltaTime)
                        deltaTime = (float)message.data;
                    else
                        throw new Exception("Received unexpected message.");

                    foreach (ParticleSystem particleSystem in particleSystems)
                    {
                        particleSystem.Simulate(deltaTime, true, firstSyncTime);
                        // Debug.Log(particleSystem.time);
                    }
                    firstSyncTime = false;
                }
            }
        }

        public static void InitializeFromServer(Server server, ISocket client)
        {
            ParticleSystem[] particleSystems = MonoBehaviour.FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[];
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                server.SendMessage(new SynchroMessage(SynchroMessageType.SetParticleSeed, particleSystem.randomSeed), client);
                particleSystem.Stop();
                particleSystem.useAutoRandomSeed = false;
                particleSystem.Clear();
                particleSystem.Play();
            }
        }

        public static void InitializeFromClient(Client client)
        {
            ParticleSystem[] particleSystems = MonoBehaviour.FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[];
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                SynchroMessage message = client.WaitForNextMessage();
                particleSystem.Stop();
                particleSystem.randomSeed = (uint)message.data;
                particleSystem.Clear();
                particleSystem.Play();
            }
        }
    }
}
