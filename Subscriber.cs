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


public class SubscriberManager<T> : SubscriberManager where T : ROSMessageType
{
    Subscribe sub;
    public readonly T messageType;
    Action<T> subscribecallback;

    private SubscribeData<T> ParseJson(string msg)
    {
        var data = msg;
        var starttopic = data.IndexOf("topic\":") - 1;
        var topiccomma = data.IndexOf(",", starttopic) + 1;
        if (topiccomma == 0)
        {
            topiccomma = data.Length;
        }
        data = data.Remove(starttopic, topiccomma - starttopic);
        var startop = data.IndexOf("op\":") - 1;
        var opcomma = data.IndexOf(",", startop) + 1;
        if (opcomma == 0)
        {
            opcomma = data.Length;
        }
        data = data.Remove(startop, opcomma - startop);
        var colon = data.IndexOf(":") + 1;
        data = data.Remove(0, colon);
        data = data.Substring(0, data.Length - 2);
        var dddata = JsonUtility.FromJson<T>(data);
        NoMsgSubTopic nmst = JsonUtility.FromJson<NoMsgSubTopic>(msg);
        var subsub = new SubscribeData<T>();
        subsub.topic = nmst.topic;
        subsub.msg = dddata;
        return subsub;
        //return JsonUtility.FromJson<SubscribeData<T>>(msg);
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

public class Subscriber<T> where T : ROSMessageType
{
    private string topic;

    public Subscriber(string topic, Action<T> callback)
    {
        this.topic = topic;
        SubscriberManager<T> subsss = new SubscriberManager<T>(topic, callback);
        WebSocketManager.Instance.sublist.Add(subsss);

    }
}

