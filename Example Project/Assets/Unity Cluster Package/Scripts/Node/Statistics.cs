using UnityEngine;
using System.Collections;
using System;

namespace UnityClusterPackage {

	// Calculation of FPS (Frame per Second) average, Rendered triangules and vertices, and time from data transit between cluster nodes.
	public class Statistics : MonoBehaviour {
		
		private int nTriangles;
		private int nVertexes;
		
		public int averageFrames = 60;
		private int frameCount;
		private float startCountTime;
		private int fps;
		
		private double transitTime;
		
		void Start () {
			
			nTriangles = 0;
			nVertexes = 0;
			
			frameCount = 0;
			fps = 0;
			startCountTime = Time.time;
			
			transitTime = 0;
		}
		
		void Update () {
			
			frameCount++;
			if(frameCount > averageFrames) {
				
				fps = (int) (averageFrames / (Time.time - startCountTime));
				
				CalcVertexesTrinagles();
				
				
				string log = "#Fps#" + fps + "#Tri#" + nTriangles + "#Verts#" + nVertexes + "#TransitTime#" +  Math.Round(transitTime) + "#";
				
				Debug.Log (log);
				
				frameCount = 0;
				startCountTime = Time.time;
				nTriangles = 0;
				nVertexes = 0;
			}
			
		}
		
		private void CalcVertexesTrinagles() {
			
			foreach(MeshFilter mf in FindObjectsOfType(typeof(MeshFilter))) {
				
				if(mf.GetComponent<Renderer>() && mf.GetComponent<Renderer>().isVisible) {
					nTriangles += mf.mesh.triangles.Length / 3;
					nVertexes += mf.mesh.vertexCount;
				}
			}
		}
		
		void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
			float ping = 1.0F;
			
			if (stream.isWriting) {
				stream.Serialize(ref ping);
			} else {
				transitTime = (Network.time - info.timestamp)*1000;
				stream.Serialize(ref ping);
			}
		}
		
	}

}