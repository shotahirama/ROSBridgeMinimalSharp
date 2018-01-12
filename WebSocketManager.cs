using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Net;

[Serializable]
public class NoMsgSubTopic
{
    public string op;
    public string topic;
    public string msg;
}


public class WebSocketManager : MonoBehaviour
{
    public string ip;
    private static WebSocketManager _Instance;
    public static WebSocketManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<WebSocketManager>();
            }

            return _Instance;
        }
    }
    private WebSocket ws;
    public Queue<string> unadvertise_queue = new Queue<string>();
    public List<SubscriberManager> sublist = new List<SubscriberManager>();
    private bool connected = false;

    void Awake()
    {
        Connect(ip);

    }

    void Start()
    {
    }

    public void Connect(string ip, int port = 9090)
    {
        ws = new WebSocket("ws://" + ip + ":" + port.ToString());
        //unadvertise_queue = new Queue<string>();
        //sublist = new List<SubscriberImpl>();
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket Open");
        };

        ws.OnMessage += (sender, e) =>
        {
            //Debug.Log("WebSocket Message Type: " + e.GetType() + ", Data: " + e.Data);
            NoMsgSubTopic nmst = JsonUtility.FromJson<NoMsgSubTopic>(e.Data);
            foreach (var sl in sublist)
            {
                if (sl.Topic == nmst.topic)
                {
                    //Debug.Log(sl.Topic);
                    sl.CallbackEvent(e.Data);
                }
            }
            //Debug.Log(nmst);y6-^\^^\^[[[
            //Debug.Log(nmst.op);
            //Debug.Log(nmst.topic);
            //Debug.Log(nmst.msg);
            //foreach (var sl in sublist)
            //{
            //    if (sl == nmst.topic)
            //    {
            //        Debug.Log(sl);
            //    }
            //}
        };

        ws.OnError += (sender, e) =>
        {
            Debug.Log("WebSocket Error Message: " + e.Message);
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket Close");
        };

        ws.Connect();
        connected = true;
        //Debug.Log("connected");
    }

    public bool Send(string msg)
    {
        //Debug.Log("Send");
        //Debug.Log(msg);
        if (connected)
        {
            ws.Send(msg);
            return true;
        }
        return false;
    }

    private void Update()
    {
    }

    private void OnDestroy()
    {
        Debug.Log(unadvertise_queue.Count);
        while (unadvertise_queue.Count > 0)
        {
            string hoge = unadvertise_queue.Dequeue();
            ws.Send(hoge);
        }
        ws.Close();
    }
}
