using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AwesomeSockets.Domain.Sockets;

namespace UnityClusterPackage
{
    class ParticleSynchronizer
    {
        private static float lastParticleTime = 0;
        private static bool firstSyncTime = true;
        private static Dictionary<ParticleSystem, float> target = new Dictionary<ParticleSystem, float>(0);
        private static ParticleSystem[] particleSystems;
        public static void ProcessMessage(InputMessage message)
        {
            if (particleSystems != null && particleSystems.Length > 0)
            {
                foreach (ParticleSystem particleSystem in particleSystems)
                {
                    target[particleSystem] = (message.particleDeltaTime + target[particleSystem]) % particleSystem.main.duration;

                    float deltaTime = target[particleSystem] - particleSystem.time;
                    if (deltaTime < -particleSystem.main.duration + 1)
                        deltaTime += particleSystem.main.duration;
                    if (deltaTime > 0 && deltaTime < 1)
                        particleSystem.Simulate(deltaTime, true, firstSyncTime);
                }
                firstSyncTime = false;
            }
        }

        public static void BuildMessage(InputMessage message)
        {
            if (particleSystems != null && particleSystems.Length > 0)
            {
                float deltaTime = particleSystems[0].time - lastParticleTime;
                if (deltaTime < 0)
                    deltaTime = particleSystems[0].time - lastParticleTime + particleSystems[0].main.duration;
                message.particleDeltaTime = deltaTime;

                lastParticleTime = particleSystems[0].time;
            }
        }

        public static void InitializeFromServer(Server server, ISocket client)
        {
            particleSystems = MonoBehaviour.FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[];
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                EventMessage message = new EventMessage();
                message.type = SynchroMessageType.SetParticleSeed;
                message.data = particleSystem.randomSeed;

                server.SendMessage(message, client);
                particleSystem.Stop();
                particleSystem.useAutoRandomSeed = false;
                particleSystem.Clear();
                particleSystem.Play();
            }
        }

        public static void InitializeFromClient(Client client)
        {
            particleSystems = MonoBehaviour.FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[];
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                EventMessage message = new EventMessage();
                client.WaitForNextMessage(message);

                target[particleSystem] = 0;
                particleSystem.Stop();
                particleSystem.randomSeed = message.data;
                particleSystem.Clear();
                particleSystem.Play();
            }
        }
    }
}
