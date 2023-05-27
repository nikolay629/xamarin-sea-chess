using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SeaChess
{
    public partial class MainPage : ContentPage
    {
        private const int Valid = 0;
        private const int EmptyData = -1;
        private const int EqualSymbols = -2;
        private readonly string[] _symbols = new string[] { "X", "O", "#", "$", "%", "&", "@", "!" };
        // List<string> firstPlayerSymbolPicker = 
        public MainPage()
        {
            InitializeComponent();
            InitializePicker(FirstPlayerSymbol);
            InitializePicker(SecondPlayerSymbol);
        }

        private async void StartGame(object sender, EventArgs e)
        {
            if (FirstPlayerSymbol.SelectedIndex == -1 || SecondPlayerSymbol.SelectedIndex == -1)
            {
                await DisplayAlert("Empty Fields", "Please fill all fields!", "OK");
                return; 
            }
            
            var firstPlayer= FirstPlayerName.Text;
            var firstPlayerSymbol = FirstPlayerSymbol.SelectedItem.ToString();
            var secondPlayer = SecondPlayerName.Text;
            var secondPlayerSymbol = SecondPlayerSymbol.SelectedItem.ToString();
            
            var validateResult = ValidateData(firstPlayer, firstPlayerSymbol, secondPlayer, secondPlayerSymbol);
            
            switch (validateResult)
            {
                case EmptyData:
                    await DisplayAlert("Empty Fields", "Please fill all fields!", "OK");
                    return;
                case EqualSymbols:
                    await DisplayAlert("Equal Symbols", "Please select different symbols!", "OK");
                    return;
                default:
                    await Navigation.PushModalAsync(new GamePage(firstPlayer, firstPlayerSymbol, secondPlayer, secondPlayerSymbol));
                    break;
            }
        }

        private void InitializePicker(Picker picker)
        {
            foreach (var symbol in _symbols)
            {
               picker.Items.Add(symbol); 
            }
        }

        private int ValidateData(
            string firstPlayer,
            string firstPlayerSymbol,
            string secondPlayer,
            string secondPlayerSymbol
        ) {
            if (
                String.IsNullOrEmpty(firstPlayer) ||
                String.IsNullOrEmpty(firstPlayerSymbol) ||
                String.IsNullOrEmpty(secondPlayer) ||
                String.IsNullOrEmpty(secondPlayerSymbol)
            ) {
                return EmptyData;
            }

            return firstPlayerSymbol == secondPlayerSymbol ? EqualSymbols : Valid;
        }
    }
}