using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Buffers;

namespace Cave
{
    public class InputParticleMessage : ISynchroMessage
    {
        public float particleDeltaTime;

        public void Serialize(Buffer buffer)
        {
            Buffer.Add(buffer, particleDeltaTime);
        }

        public void Deserialize(Buffer buffer)
        {
            particleDeltaTime = Buffer.Get<float>(buffer);
        }

        public int GetLength()
        {
            return sizeof(float) * 1;
        }
    }

    class ParticleSynchronizer
    {
        private static float lastParticleTime = 0;
        private static bool firstSyncTime = true;
        private static Dictionary<ParticleSystem, float> target = new Dictionary<ParticleSystem, float>(0);
        private static ParticleSystem[] particleSystems;
        public static void ProcessMessage(InputParticleMessage message)
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

        public static void BuildMessage(InputParticleMessage message)
        {
			if (particleSystems != null)
			{
				foreach (ParticleSystem particleSystem in particleSystems)
				{				
					if (particleSystem.isPlaying) 
					{
						float deltaTime = particleSystem.time - lastParticleTime;
						if (deltaTime < 0)
							deltaTime = particleSystem.time - lastParticleTime + particleSystem.main.duration;
						message.particleDeltaTime = deltaTime;

						lastParticleTime = particleSystem.time;
						break;
					}
				}
			}
        }

        public static void InitializeFromServer(Server server, ISocket client)
        {
            particleSystems = Resources.FindObjectsOfTypeAll(typeof(ParticleSystem)) as ParticleSystem[];
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
            particleSystems = Resources.FindObjectsOfTypeAll(typeof(ParticleSystem)) as ParticleSystem[];
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
