using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Net;

public class WebSocketTest : MonoBehaviour
{
    Publisher<StdString> pub;
    void Start()
    {
        pub = new Publisher<StdString>("hoge");
        Subscriber<StdString> sub = new Subscriber<StdString>("/foobar",Callback);
    }

    void Update()
    {
        if (Input.GetKeyUp("s"))
        {
            StdString stdstr = new StdString();
            stdstr.data = "Hello";
            pub.publish(stdstr);
        }
    }

    void Callback(StdString msg)
    {
        Debug.Log("callback = ");
        Debug.Log(msg.data);
    }

    private void OnDestroy()
    {
    }
}
