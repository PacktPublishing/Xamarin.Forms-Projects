using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Weather.Controls
{
    public class RepeaterView : FlexLayout
    {
        public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<object>), typeof(RepeaterView), null,
             propertyChanged: (bindable, oldValue, newValue) => {

                 var repeater = (RepeaterView)bindable;

                 if (repeater.ItemsTemplate == null)
                 {
                     return;
                 }

                 MainThread.BeginInvokeOnMainThread(() => repeater.Generate());
             });

        public IEnumerable<object> ItemsSource
        {
            get => GetValue(ItemsSourceProperty) as IEnumerable<object>;
            set => SetValue(ItemsSourceProperty, value);
        }

        private DataTemplate itemsTemplate;
        public DataTemplate ItemsTemplate
        {
            get => itemsTemplate;
            set
            {
                itemsTemplate = value;
                MainThread.BeginInvokeOnMainThread(() => Generate());
            }
        }


        private void Generate()
        {
            Children.Clear();

            if (ItemsSource == null)
            {
                return;
            }

            foreach (var item in ItemsSource)
            {
                var view = itemsTemplate.CreateContent() as View;

                if (view == null)
                {
                    return;
                }

                view.BindingContext = item;

                Children.Add(view);
            }
        }

        private string visualState;
        public string VisualState
        {
            get => visualState;
            set
            {
                visualState = value;

                SetState(this, visualState);
            }
        }

        private void SetState(VisualElement view, string state)
        {
            VisualStateManager.GoToState(view, visualState);

            if (view is Layout layout)
            {
                foreach (VisualElement child in layout.Children)
                {
                    SetState(child, state);
                }
            }

        }
    }
}
