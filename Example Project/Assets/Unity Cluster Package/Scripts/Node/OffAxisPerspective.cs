using UnityEngine;
using System.Collections;

// This script is an adaptation of https://en.wikibooks.org/wiki/Cg_Programming/Unity/Projection_for_Virtual_Reality, which is based
// on Robert Kooima's publication "Generalized Perspective Projection," 2009, // http://csc.lsu.edu/~kooima/pdfs/gen-perspective.pdf

namespace UnityClusterPackage {

	public class OffAxisPerspective : MonoBehaviour {

		private Transform projectionPlane;
		private Transform userHead;
		
		private Vector3 pa, pb, pc, pe, va, vb, vc, vr, vu , vn;
		private float l, r, b, t, d, n, f, eyeX;
		private Matrix4x4 p, rm, tm;

		void Start () {

			projectionPlane = transform.Find("ProjectionPlane");
			userHead = transform.Find("ProjectionPlane/UserHead");

			if ( NodeInformation.stereo ) 
			{
				if( NodeInformation.eye.Equals("right") ) 
				{
					eyeX = 0.03f;
				}
				else if( NodeInformation.eye.Equals("left") )
				{
					eyeX = -0.03f;
				}
			} 
			else 
			{
				eyeX = 0.0f;
			}

			userHead.localPosition = new Vector3 ( NodeInformation.peX, NodeInformation.peZ, NodeInformation.peY );

			n = GetComponent<Camera>().nearClipPlane;		
			f = GetComponent<Camera>().farClipPlane;				
		}
		
		void OnPreRender () {
			CalcProjectionMatrix ();
		}

		private void CalcProjectionMatrix() {

			pa = projectionPlane.TransformPoint( new Vector3( NodeInformation.paX, NodeInformation.paZ, NodeInformation.paY ) );
			pb = projectionPlane.TransformPoint( new Vector3( NodeInformation.pbX, NodeInformation.pbZ, NodeInformation.pbY ) );
			pc = projectionPlane.TransformPoint( new Vector3( NodeInformation.pcX, NodeInformation.pcZ, NodeInformation.pcY ) );
						
			pe = userHead.position;
			pe.x += eyeX;

			vr = pb - pa;
			vu = pc - pa;
			vr.Normalize();
			vu.Normalize();
			vn = -Vector3.Cross(vr, vu);
			vn.Normalize();
			
			va = pa - pe;
			vb = pb - pe;
			vc = pc - pe;
			
			d = -Vector3.Dot(va, vn);
			l = Vector3.Dot(vr, va) * n / d;
			r = Vector3.Dot(vr, vb) * n / d;
			b = Vector3.Dot(vu, va) * n / d;
			t = Vector3.Dot(vu, vc) * n / d;
			
			
			p = new Matrix4x4();			
			p[0,0] = (float)2.0*n/(r-l); p[0,1] = 0.0f; 			   p[0,2] = (float) (r+l)/(r-l); p[0,3] = 0.0f;
			p[1,0] = 0.0f; 				 p[1,1] = (float) 2.0*n/(t-b); p[1,2] = (float) (t+b)/(t-b); p[1,3] = 0.0f;
			p[2,0] = 0.0f; 				 p[2,1] = 0.0f; 			   p[2,2] = (float) (f+n)/(n-f); p[2,3] = (float) 2.0*f*n/(n-f);
			p[3,0] = 0.0f; 				 p[3,1] = 0.0f; 			   p[3,2] = -1.0f; 				 p[3,3] = 0.0f;

			GetComponent<Camera>().projectionMatrix = p;
			
			
			rm = new Matrix4x4();			
			rm[0,0] = (float) vr.x; rm[0,1] = (float) vr.y; rm[0,2] = (float) vr.z; rm[0,3] = 0.0f;	
			rm[1,0] = (float) vu.x; rm[1,1] = (float) vu.y; rm[1,2] = (float) vu.z; rm[1,3] = 0.0f;	
			rm[2,0] = (float) vn.x; rm[2,1] = (float) vn.y; rm[2,2] = (float) vn.z; rm[2,3] = 0.0f;	
			rm[3,0] = 0.0f;  		rm[3,1] = 0.0f;  		rm[3,2] = 0.0f;  		rm[3,3] = 1.0f;		
			
			
			Matrix4x4 tm = new Matrix4x4();			
			tm[0,0] = 1.0f; tm[0,1] = 0.0f; tm[0,2] = 0.0f; tm[0,3] = (float) -pe.x;
			tm[1,0] = 0.0f; tm[1,1] = 1.0f; tm[1,2] = 0.0f; tm[1,3] = (float) -pe.y;	
			tm[2,0] = 0.0f; tm[2,1] = 0.0f; tm[2,2] = 1.0f; tm[2,3] = (float) -pe.z;	
			tm[3,0] = 0.0f; tm[3,1] = 0.0f; tm[3,2] = 0.0f; tm[3,3] = 1.0f;
			
			GetComponent<Camera>().worldToCameraMatrix = rm * tm;		
		}
	}
}