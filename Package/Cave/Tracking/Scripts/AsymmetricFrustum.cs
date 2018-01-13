// Set an off-center projection, where perspective's vanishing
// point is not necessarily in the center of the screen.
//
// adapted by Henrique Debarba to dynamically correct pMatrix
// according to screen size/pos and camera pos
// this script is ment to be attached to the camera

using UnityEngine;
using System.Collections;
using Cave;

[ExecuteInEditMode]
public class AsymmetricFrustum : MonoBehaviour
{

    public bool drawAxes;

    void OnPreCull()
    {
        Transform screenPlane = NodeInformation.own.screenPlane;
        // screen parameters must be defined in a gameObject transform in the scene (make input more intuitive) 
        if (screenPlane != null)
        {
            // screen size as defined by screenPlane transform
            float horScreenSize = screenPlane.localScale.x;
            float verScreenSize = screenPlane.localScale.y;

            Quaternion resetOrient = Quaternion.Inverse(transform.rotation);

            // allign screen and camera with world coord system
            Vector3 axisAllignedCamPos = resetOrient * transform.position;
            Vector3 axisAllignedScreenPos = resetOrient * screenPlane.position;

            // distance between screen and camera along screen normal direction (z axis)
            float distance2Screen = Mathf.Abs(axisAllignedCamPos.z - axisAllignedScreenPos.z);

            // normilized position of the camera in a plane parallel to the screen
            // values < 0 and > 1 means the camera is beyond the borders of the camera
            float xZero = (axisAllignedCamPos.x - axisAllignedScreenPos.x) / horScreenSize + 0.5f;
            float yZero = (axisAllignedCamPos.y - axisAllignedScreenPos.y) / verScreenSize + 0.5f;


            // this script is ment to be attached to a camera
            if (GetComponent<Camera>() != null)
            {
                float dist2NearPlane = GetComponent<Camera>().nearClipPlane;
                if (distance2Screen < dist2NearPlane)
                    distance2Screen = dist2NearPlane;

                //float top = GetComponent<Camera>().nearClipPlane * (screenPlane.position.y - transform.position.y + verScreenSize * 0.5f) / distance2Screen;
                //float bottom = GetComponent<Camera>().nearClipPlane * (screenPlane.position.y - transform.position.y - verScreenSize * 0.5f) / distance2Screen;
                //float left = GetComponent<Camera>().nearClipPlane * (screenPlane.position.x - transform.position.x - horScreenSize * 0.5f) / distance2Screen;
                //float right = GetComponent<Camera>().nearClipPlane * (screenPlane.position.x - transform.position.x + horScreenSize * 0.5f) / distance2Screen;

                // define boundaries of the projection in terms of camera near plane (plane of projection)
                float top = (((1.0f - yZero) * verScreenSize) / distance2Screen) * dist2NearPlane;
                float bottom = (((-yZero) * verScreenSize) / distance2Screen) * dist2NearPlane;
                float right = (((1.0f - xZero) * horScreenSize) / distance2Screen) * dist2NearPlane;
                float left = (((-xZero) * horScreenSize) / distance2Screen) * dist2NearPlane;

                // create new projection matrix
                Matrix4x4 m = OffCenter(left, right, bottom, top, GetComponent<Camera>().nearClipPlane, GetComponent<Camera>().farClipPlane);				
                GetComponent<Camera>().projectionMatrix = m;
				GetComponent<Camera> ().fieldOfView = Mathf.Atan (1.0f / m.m11) * 2.0f * 180 / Mathf.PI;
            }
            else
                Debug.Log("OffCenterPerspective: Behavior must be attached to a camera");
        }
    }

    static Matrix4x4 OffCenter(float left, float right, float bottom, float top, float near, float far) {
        float x = 2.0F * near / (right - left);
        float y = 2.0F * near / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0F * far * near) / (far - near);
        float e = -1.0F;
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x;
        m[0, 1] = 0;
        m[0, 2] = a;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = y;
        m[1, 2] = b;
        m[1, 3] = 0;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = c;
        m[2, 3] = d;
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = e;
        m[3, 3] = 0;
        return m;
    }


    public virtual void OnDrawGizmos()
    {
        foreach (ConfigurationNode node in NodeInformation.slaves)
        {
            if (node.screenPlane != null)
                DrawScreenPlane(node.screenPlane);
        }
    }

    private void DrawScreenPlane(Transform screenPlane)
    {
        float horScreenSize = screenPlane.localScale.x;
        float verScreenSize = screenPlane.localScale.y;

        if (drawAxes)
        {
            //Gizmos.DrawLine(GetComponent<Camera>().transform.position, GetComponent<Camera>().transform.position + GetComponent<Camera>().transform.up * 10);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(screenPlane.transform.position, screenPlane.transform.position + screenPlane.transform.up);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(screenPlane.transform.position - screenPlane.transform.forward * 0.5f * verScreenSize, screenPlane.transform.position + screenPlane.transform.forward * 0.5f * verScreenSize);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(screenPlane.transform.position - screenPlane.transform.right * 0.5f * horScreenSize, screenPlane.transform.position + screenPlane.transform.right * 0.5f * horScreenSize);
        }

        Gizmos.color = Color.white;
        Vector3 leftBottom = screenPlane.transform.position - screenPlane.transform.right * 0.5f * horScreenSize - screenPlane.transform.forward * 0.5f * verScreenSize;
        Vector3 leftTop = screenPlane.transform.position - screenPlane.transform.right * 0.5f * horScreenSize + screenPlane.transform.forward * 0.5f * verScreenSize;
        Vector3 rightBottom = screenPlane.transform.position + screenPlane.transform.right * 0.5f * horScreenSize - screenPlane.transform.forward * 0.5f * verScreenSize;
        Vector3 rightTop = screenPlane.transform.position + screenPlane.transform.right * 0.5f * horScreenSize + screenPlane.transform.forward * 0.5f * verScreenSize;

        Gizmos.DrawLine(leftBottom, leftTop);
        Gizmos.DrawLine(leftTop, rightTop);
        Gizmos.DrawLine(rightTop, rightBottom);
        Gizmos.DrawLine(rightBottom, leftBottom);
    }
}