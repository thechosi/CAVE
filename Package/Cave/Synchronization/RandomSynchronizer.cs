using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Buffers;

namespace Cave
{
	public class RandomSynchronizer
	{
		public static int seed = 0;
		private static bool seedSet = false;

		public static void InitializeFromServer(Server server, ISocket client)
		{
			if (!seedSet) {
				seed = (int)(Random.value * int.MaxValue);
				seedSet = true;
			}

			EventMessage message = new EventMessage();
			message.type = SynchroMessageType.SetRandomSeed;
			message.data = (uint)seed;

			server.SendMessage(message, client);
			Random.InitState (seed);

			Debug.Log ("Seed: " + seed.ToString ());
		}

		public static void InitializeFromClient(Client client)
		{	
			EventMessage message = new EventMessage();
			client.WaitForNextMessage(message);

			Random.InitState ((int)message.data);
			Debug.Log ("Seed: " + ((int)message.data).ToString ());
		}
	}
}

