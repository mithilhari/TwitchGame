using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.IO;

public class PlayerComponent : MonoBehaviour
{
    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;
    
    [SerializeField]
    private string username;
    [SerializeField]
    private string password;
    [SerializeField]
    private string channelName;
    // Start is called before the first frame update
    void Start()
    {
        Connect();
    }

    // Update is called once per frame
    void Update()
    {
        if(!twitchClient.Connected) {
            Connect();
        }

        ReadChat();
    }
    
    private void Connect() {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        writer.WriteLine("PASS " + password);
        writer.WriteLine("NICK " + username);
        // writer.WriteLine("USER " + username + " 8 * :" + username);
        writer.WriteLine("JOIN #" + channelName);
        writer.Flush();
    }
    private void ReadChat() {
        if(twitchClient.Available > 0)
        {
            var message = reader.ReadLine();
            print(message);
            // parse message and do equality check for left, right, up, down
            // if statements for each move leading to actual playermovement
        }
    }
}
