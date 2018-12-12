using Chat.Messages;
using Xamarin.Forms;

namespace Chat.Selectors
{
    public class ChatMessageSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var list = (ListView)container;

            if (item is LocalSimpleTextMessage)
            {
                return (DataTemplate)list.Resources["LocalSimpleText"];
            }
            else if (item is SimpleTextMessage)
            {
                return (DataTemplate)list.Resources["SimpleText"];
            }
            else if (item is UserConnectedMessage)
            {
                return (DataTemplate)list.Resources["UserConnected"];
            }
            else if (item is PhotoUrlMessage)
            {
                return (DataTemplate)list.Resources["Photo"];
            }
            else if (item is PhotoMessage)
            {
                return (DataTemplate)list.Resources["LocalPhoto"];
            }

            return null;
        }
    }
}
