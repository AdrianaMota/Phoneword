using Xamarin.Forms;
using System;
using Xamarin.Essentials;


namespace Phoneword
{

    public partial class MainPage : ContentPage
    {
        Entry phoneNumber;
        Button translateBtn;
        Button callBtn;
        string translatedNumber;

        public MainPage()
        {
            Padding = new Thickness(20, 100, 20, 20);
            var stack = new StackLayout
            {
                Spacing = 15
            };

            stack.Children.Add (new Label 
            {
                Text = "Enter a Phoneword: ", 
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) 
            });

            stack.Children.Add (phoneNumber = new Entry 
            {
                Placeholder = "1-855-XAMARIN"
            });

            stack.Children.Add(translateBtn = new Button
            {
                Text = "Translate"
            });

            stack.Children.Add(callBtn = new Button
            {
                Text = "Call",
                IsEnabled = false
            });

            translateBtn.Clicked += OnTranslate;
            callBtn.Clicked += OnCall;
            this.Content = stack;
        }
        private void OnTranslate(Object sender, EventArgs e)
        {
            string enteredNumber = phoneNumber.Text;
            translatedNumber = Core.PhonewordTranslator.ToNumber(enteredNumber);
            if (!string.IsNullOrEmpty(translatedNumber))
            {
                callBtn.IsEnabled = true;
                callBtn.Text = "Call " + translatedNumber;
            } else
            {
                callBtn.IsEnabled = false;
                callBtn.Text = "Call";
            }
        }
        async private void OnCall(Object sender, System.EventArgs e)
        {
            if (await this.DisplayAlert(
                "Dial Number",
                "Would you like to call " + translatedNumber + "?",
                "Yes",
                "No"))
            {
                try
                {
                    PhoneDialer.Open(translatedNumber);
                }
                catch (ArgumentNullException)
                {
                    await DisplayAlert("Unable to dial", "Phone number was not valid.", "OK");
                }
                catch (FeatureNotSupportedException)
                {
                    await DisplayAlert("Unable to dial", "Phone dialing not supported.", "OK");
                }
                catch (Exception)
                {
                    // Other error has occurred.
                    await DisplayAlert("Unable to dial", "Phone dialing failed.", "OK");
                }
            }
        }
    }
}
