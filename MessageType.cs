using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ROSMessageType
{
    public abstract string GetType();
}

[Serializable]
public class StdString : ROSMessageType
{
    public string data;
    public override string GetType()
    {
        return "std_msgs/String";
    }
}

[Serializable]
public class PoseArray : ROSMessageType
{
    public Header header;
    public Pose[] poses;
    public override string GetType()
    {
        return "geometry_msgs/PoseArray";

    }

}

[Serializable]
public class Header : ROSMessageType
{
    public int seq;
    public TimeType stamp;
    public string frame_id;
    public override string GetType()
    {
        return "std_msgs/Header";

    }
}

[Serializable]
public class TimeType : ROSMessageType
{
    public int secs;
    public int nsecs;
    public override string GetType()
    {
        return "";
    }
}

[Serializable]
public class Pose : ROSMessageType
{
    public Vector3 position;
    public Quaternion orientation;
    public override string GetType()
    {
        return "geometry_msgs/Pose";

    }
}

[Serializable]
public class SensorImage : ROSMessageType
{
    public Header header;
    public int height;
    public int width;
    public string encoding;
    public byte is_bigendian;
    public int step;
    public string data;
    public override string GetType()
    {
        return "sensor_msgs/Image";
    }
}

[Serializable]
public class CompressedImage : ROSMessageType
{
    public Header header;
    public string format;
    public string data;
    public override string GetType()
    {
        return "sensor_msgs/CompressedImage";
    }
}

[Serializable]
public class PoseStamped:ROSMessageType
{
    public Header header;
    public Pose pose;
    public override string GetType()
    {
        return "geometry_msgs/PoseStamped";
    }
}

[Serializable]
public class NavPath : ROSMessageType
{
    public Header header;
    public PoseStamped[] poses;
    public override string GetType()
    {
        return "nav_msgs/Path";
    }
}

[Serializable]
public class PoseWithCovariance : ROSMessageType
{
    public Pose pose;
    public float[] covariance;
    public override string GetType()
    {
        return "geometry_msgs/PoseWithCovariance";
    }
}

[Serializable]
public class TwistWithCovariance : ROSMessageType
{
    public Twist pose;
    public float[] covariance;
    public override string GetType()
    {
        return "geometry_msgs/TwistWithCovariance";
    }
}

[Serializable]
public class Twist : ROSMessageType
{
    public Vector3 linear;
    public Vector3 angular;
    public override string GetType()
    {
        return "geometry_msgs/PoseWithCovariance";
    }
}

[Serializable]
public class Odometry : ROSMessageType
{
    public Header header;
    public string child_frame_id;
    public PoseWithCovariance pose;
    public TwistWithCovariance twist;
    public override string GetType()
    {
        return "nav_mags/Odometry";
    }
}