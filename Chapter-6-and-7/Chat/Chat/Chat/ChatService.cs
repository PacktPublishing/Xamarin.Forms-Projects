using Chat.Events;
using Chat.Messages;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Chat
{
    public class ChatService : IChatService
    {
        private HttpClient httpClient;
        private HubConnection hub;
        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public event EventHandler<NewMessageEventArgs> NewMessage;
        public bool IsConnected { get; set; }

        public async Task CreateConnection()
        {
            await semaphoreSlim.WaitAsync();

            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }

            var result = await httpClient.GetStringAsync("https://{theNameOfTheFunctionApp}.azurewebsites.net/api/GetSignalRInfo");

            var info = JsonConvert.DeserializeObject<Models.ConnectionInfo>(result);

            var connectionBuilder = new HubConnectionBuilder();
            connectionBuilder.WithUrl(info.Url, (Microsoft.AspNetCore.Http.Connections.Client.HttpConnectionOptions obj) =>
            {
                obj.AccessTokenProvider = () => Task.Run(() => info.AccessToken);
            });

            hub = connectionBuilder.Build();
            hub.On<object>("newMessage", (message) =>
            {
                var json = message.ToString();
                var obj = JsonConvert.DeserializeObject<Message>(json);
                var msg = (Message)JsonConvert.DeserializeObject(json, obj.TypeInfo);
                NewMessage?.Invoke(this, new NewMessageEventArgs(msg));
            });

            await hub.StartAsync();

            IsConnected = true;
            semaphoreSlim.Release();
        }

        public async Task SendMessage(Message message)
        {
            var json = JsonConvert.SerializeObject(message);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await httpClient.PostAsync("https://{TheNameOfTheFunctionApp}.azurewebsites.net/api/messages", content);
        }

        public async Task Dispose()
        {
            await semaphoreSlim.WaitAsync();

            if (hub != null)
            {
                await hub.StopAsync();
                await hub.DisposeAsync();
            }

            httpClient = null;

            IsConnected = false;

            semaphoreSlim.Release();
        }
    }
}
