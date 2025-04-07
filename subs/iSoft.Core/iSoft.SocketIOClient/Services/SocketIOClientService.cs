using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.SocketIOClientNS.Services
{
  public class SocketIOClientService
  {
    static SocketIO? client = null;
    static string address;
    static SocketIOOptions socketIOOptions;

    public static async Task SendMessageAsync(string channel, string room, string senderName, string message)
    {
      if (client != null && client.Connected)
      {
        await client.EmitAsync(channel, room, senderName, message);
      }
      else
      {
        throw new Exception("SendMessageAsync error");
      }
    }

    public static SocketIO? NewConnection(
      string addressInput, Action<SocketIO?> initFunction)
    {
      if (address == addressInput && client != null && client.Connected)
      {
        return client;
      }

      if (address != addressInput && client != null && client.Connected)
      {
        try
        {
          client.DisconnectAsync().Wait();
          client.Dispose();
        }
        catch { }
      }

      address = addressInput;
      socketIOOptions = new SocketIOOptions();
      socketIOOptions.Path = "/socket.io/";
      socketIOOptions.Transport = SocketIOClient.Transport.TransportProtocol.WebSocket;
      client = new SocketIO(address, socketIOOptions);

      //client.OnDisconnected += async (sender, e) =>
      //{
      //  //Thread.Sleep(1000);
      //  //client.ConnectAsync();
      //};
      //client.OnConnected += async (sender, e) =>
      //{
      //  await client.SendMessageAsync("input", "From c#");
      //};
      //client.On("alert", response =>
      //{
      //  string data = response.ToString();
      //  Debug.WriteLine(data);
      //});

      initFunction(client);

      client.ConnectAsync();
      return client;
    }
  }
}
