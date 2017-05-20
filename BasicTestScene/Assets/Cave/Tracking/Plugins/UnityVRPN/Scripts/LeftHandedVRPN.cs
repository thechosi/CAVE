using UnityEngine;

public static class LeftHandedVRPN 
{
	public static Vector3 vrpnTrackerPos(string address, int channel)
	{
		var vector = VRPN.vrpnTrackerPos(address, channel);
		return new Vector3(-vector.x, vector.y, vector.z);
	}

	public static Quaternion vrpnTrackerQuat(string address, int channel)
	{
		Quaternion quaternion = VRPN.vrpnTrackerQuat (address, channel);
		Vector3 euler = quaternion.eulerAngles;
		return Quaternion.Euler (euler.x, -euler.y, -euler.z);
	}

	//added by pohl 9.12.16
	public static bool vrpnTrackerButton(string address, int channel)
	{
		return VRPN.vrpnButton (address, channel);
	}
	//added by pohl 16.12.16
	public static double vrpnTrackerAnalog(string address, int channel)
	{
		return VRPN.vrpnAnalog (address, channel);
	}
}

