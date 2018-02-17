using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MessageType
{
    public abstract string GetType();
}

[Serializable]
public class StdString : MessageType
{
    public string data;
    public override string GetType()
    {
        return "std_msgs/String";
    }
}

[Serializable]
public class PoseArray : MessageType
{
    public Header header;
    public Pose[] poses;
    public override string GetType()
    {
        return "geometry_msgs/PoseArray";

    }

}

[Serializable]
public class Header : MessageType
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
public class TimeType : MessageType
{
    public int secs;
    public int nsecs;
    public override string GetType()
    {
        return "";
    }
}

[Serializable]
public class Pose : MessageType
{
    public Vector3 position;
    public Quaternion orientation;
    public override string GetType()
    {
        return "geometry_msgs/Pose";

    }
}

[Serializable]
public class SensorImage : MessageType
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
public class CompressedImage : MessageType
{
    public Header header;
    public string format;
    public string data;
    public override string GetType()
    {
        return "sensor_msgs/CompressedImage";
    }
}