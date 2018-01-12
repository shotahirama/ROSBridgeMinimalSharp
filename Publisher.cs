using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Advertise
{
    [SerializeField]
    private string op = "advertise";
    public string topic;
    public string type;
}

[Serializable]
public class UnAdvertise
{
    [SerializeField]
    private string op = "unadvertise";
    public string topic;
}

[Serializable]
public class Publish<T>
{
    [SerializeField]
    private string op = "publish";
    public string topic;
    public T msg;
}

public class Publisher<T> where T : MessageType, new()
{
    private string topic;
    Publish<T> pub;
    T messagetype;

    public Publisher(string topic)
    {
        this.topic = topic;
        messagetype = new T();
        Advertise adv = new Advertise();
        adv.topic = topic;
        adv.type = messagetype.GetType();  
        WebSocketManager.Instance.Send(JsonUtility.ToJson(adv));
        UnAdvertise unadv = new UnAdvertise();
        unadv.topic = topic;
        //WebSocketManager.Instance.Send(JsonUtility.ToJson(unadv));
        WebSocketManager.Instance.unadvertise_queue.Enqueue(JsonUtility.ToJson(unadv));
        pub = new Publish<T>();
        pub.topic = topic;
    }

    ~Publisher()
    {
    }

    public void publish(T msg)
    {
        if (messagetype.GetType() == msg.GetType())
        {
            pub.msg = msg;
            WebSocketManager.Instance.Send(JsonUtility.ToJson(pub));
        }
        else
        {
            Debug.Log("Error MessageType");
        }


    }
}
