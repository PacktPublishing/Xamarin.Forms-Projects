using Acr.UserDialogs;
using Chat.Chat;
using Chat.Messages;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Chat.ViewModels
{
    public class ChatViewModel : ViewModel
    {
        private readonly IChatService chatService;
        public ObservableCollection<Message> Messages { get; private set; }

        public ChatViewModel(IChatService chatService)
        {
            this.chatService = chatService;

            Messages = new ObservableCollection<Message>();

            chatService.NewMessage += ChatService_NewMessage;

            Task.Run(async () =>
            {
                if (!chatService.IsConnected)
                {
                    await chatService.CreateConnection();
                }

                await chatService.SendMessage(new UserConnectedMessage(User));
            });
        }

        private string text;
        public string Text
        {
            get => text;
            set => Set(ref text, value);
        }

        public ICommand Send => new Command(async () =>
        {
            var message = new SimpleTextMessage(User)
            {
                Text = this.Text
            };

            Messages.Add(new LocalSimpleTextMessage(message));

            await chatService.SendMessage(message);

            Text = string.Empty;
        });

        public ICommand Photo => new Command(async () =>
        {
            var options = new PickMediaOptions();
            options.CompressionQuality = 50;

            var photo = await CrossMedia.Current.PickPhotoAsync();

            UserDialogs.Instance.ShowLoading("Uploading photo");

            var stream = photo.GetStream();
            var bytes = ReadFully(stream);

            var base64photo = Convert.ToBase64String(bytes);

            var message = new PhotoMessage(User)
            {
                Base64Photo = base64photo,
                FileEnding = photo.Path.Split('.').Last()
            };

            Messages.Add(message);
            await chatService.SendMessage(message);

            UserDialogs.Instance.HideLoading();
        });

        private void ChatService_NewMessage(object sender, Events.NewMessageEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (!Messages.Any(x => x.Id == e.Message.Id))
                {
                    Messages.Add(e.Message);
                }
            });
        }

        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];

            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
