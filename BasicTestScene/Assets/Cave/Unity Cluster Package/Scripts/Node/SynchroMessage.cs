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
    public enum SynchroMessageType
    {
        SetDeltaTime,
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
