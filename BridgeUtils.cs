using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeUtils
{
    public static Vector3 ConvertPosition(Vector3 vector3)
    {
        return new Vector3(-vector3.y, vector3.z, vector3.x);
    }

    public static Quaternion ConvertRotation(Quaternion quaternion)
    {
        return new Quaternion(quaternion.x, -quaternion.z, quaternion.y, quaternion.w);
    }
}
