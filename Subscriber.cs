using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Subscribe
{
    [SerializeField]
    private string op = "subscribe";
    public string topic;
}

[Serializable]
public class SubscribeData<T>
{
    public string op;
    public string topic;
    public T msg;
}

public abstract class SubscriberManager
{
    protected string topic;

    public string Topic
    {
        get
        {
            return topic;
        }
    }
    
    public abstract void CallbackEvent(string msg);
}


public class SubscriberManager<T> : SubscriberManager where T : MessageType
{
    Subscribe sub;
    public readonly T messageType;
    Action<T> subscribecallback;

    private SubscribeData<T> ParseJson(string msg)
    {
        return JsonUtility.FromJson<SubscribeData<T>>(msg);
    }
    
    public SubscriberManager(string topic, Action<T> callback)
    {
        sub = new Subscribe();
        this.topic = topic;
        sub.topic = topic;
        WebSocketManager.Instance.Send(JsonUtility.ToJson(sub));
        subscribecallback = callback;
    }

    public override void CallbackEvent(string msg)
    {
        SubscribeData<T> parsemsg = ParseJson(msg);
        subscribecallback(parsemsg.msg);
    }
}

public class Subscriber<T> where T : MessageType
{
    private string topic;

    public Subscriber(string topic, Action<T> callback)
    {
        this.topic = topic;
        SubscriberManager<T> subsss = new SubscriberManager<T>(topic, callback);
        WebSocketManager.Instance.sublist.Add(subsss);

    }
}

