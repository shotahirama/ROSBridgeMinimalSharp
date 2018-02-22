using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_UWP || NETFX_CORE
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.Web;
using Windows.System.Threading;
using System.Threading.Tasks;
#else
using WebSocketSharp;
#endif

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
    public int port = 9090;
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
#if UNITY_UWP || NETFX_CORE
        private MessageWebSocket ws;
        private DataWriter dataWriter;
#else
    private WebSocket ws;
#endif
    public Queue<string> unadvertise_queue = new Queue<string>();
    public List<SubscriberManager> sublist = new List<SubscriberManager>();
    private bool connected = false;

    void Awake()
    {
        Connect(ip, port);
    }

    void Start()
    {
    }

#if UNITY_UWP || NETFX_CORE
     void UWPOnMessage(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            DataReader dataReader = args.GetDataReader();
            dataReader.UnicodeEncoding = UnicodeEncoding.Utf8;
            string messageString = dataReader.ReadString(dataReader.UnconsumedBufferLength);

            Task.Run(async () =>
            {
                UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                {
                    SubMessage(messageString);
                }, true);
                await Task.Delay(10);
            });
        }

        private void SubMessage(string s)
        {
            NoMsgSubTopic nmst = JsonUtility.FromJson<NoMsgSubTopic>(s);
            foreach (var sl in sublist)
            {
                if (sl.Topic == nmst.topic)
                {
                    sl.CallbackEvent(s);
                }
            }
        }
#endif

    public void Connect(string ip, int port = 9090)
    {
#if UNITY_UWP || NETFX_CORE
        Uri uri = new Uri("ws://" + ip + ":" + port.ToString());
        ws = new MessageWebSocket();
        dataWriter = new DataWriter(ws.OutputStream);
        ws.Control.MessageType = SocketMessageType.Utf8;
        ws.MessageReceived += UWPOnMessage;
        var task = Task.Run(async () =>
        {
            await ws.ConnectAsync(uri);
        });
        task.Wait();
#else
        ws = new WebSocket("ws://" + ip + ":" + port.ToString());
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket Open");
        };

        ws.OnMessage += (sender, e) =>
        {
            NoMsgSubTopic nmst = JsonUtility.FromJson<NoMsgSubTopic>(e.Data);
            foreach (var sl in sublist)
            {
                if (sl.Topic == nmst.topic)
                {
                    sl.CallbackEvent(e.Data);
                }
            }
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
#endif
        connected = true;
    }

    public bool Send(string msg)
    {
        if (msg.Length > 2)
        {
            if (connected)
            {
#if UNITY_UWP || NETFX_CORE
                dataWriter.WriteString(msg);
                dataWriter.StoreAsync();
#else
                ws.Send(msg);
#endif
                return true;
            }
        }
        return false;
    }

    //private void Update()
    //{
    //}

    private void OnDestroy()
    {
        while (unadvertise_queue.Count > 0)
        {
            string unadvrtise = unadvertise_queue.Dequeue();
            Send(unadvrtise);
        }
#if UNITY_UWP || NETFX_CORE
        ws.Close(0,"");
#else
        ws.Close();
#endif
    }
}
