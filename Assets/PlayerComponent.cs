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
    public Rigidbody2D player;
    [SerializeField]
    private int speed;
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
            string message = reader.ReadLine();
            //:induslion!induslion@induslion.tmi.twitch.tv PRIVMSG #induslion :hello
            if (message.Contains("PRIVMSG")) {
                int splitPoint = message.IndexOf("!", 1);
                string chatName = message.Substring(0, splitPoint);
                chatName = chatName.Substring(1);

                splitPoint = message.IndexOf(":", 1);
                message = message.Substring(splitPoint+1);
                print(String.Format("{0} : {1}", chatName, message));
                GameInputs(message);

            }
            // parse message and do equality check for left, right, up, down
            // if statements for each move leading to actual playermovement
        }
    }

    private void GameInputs(string chatInput) {
        print(chatInput);
        if (chatInput.ToLower() == "left") {
            player.MovePosition(player.position + new Vector2(-1,0) * speed * Time.deltaTime);
        }

        if (chatInput.ToLower() == "right") {
            player.MovePosition(player.position + new Vector2(1,0) * speed * Time.deltaTime);
        }

        if (chatInput.ToLower() == "up") {
            player.MovePosition(player.position + new Vector2(0,1) * speed * Time.deltaTime);
        }

        if (chatInput.ToLower() == "down") {
            player.MovePosition(player.position + new Vector2(0,-1) * speed * Time.deltaTime);
        }
    }
}
