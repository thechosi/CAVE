using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using AwesomeSockets.Domain;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Sockets;
using UnityEngine.Networking;
using Buffer = AwesomeSockets.Buffers.Buffer;

namespace UnityClusterPackage
{
    class ParticleSynchronizer
    {
        private static float lastParticleTime = 0;

        public static void Synchronize()
        {
            ParticleSystem[] particleSystems = MonoBehaviour.FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[];
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                //particleSystem.time -= Time.deltaTime;
                particleSystem.time = lastParticleTime;
                particleSystem.Simulate(TimeSynchronizer.deltaTime, true, false);
                lastParticleTime = particleSystem.time;
                //particleSystem.Simulate(deltaTime, true, false);
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
