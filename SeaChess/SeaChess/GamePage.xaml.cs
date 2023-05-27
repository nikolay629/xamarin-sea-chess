using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeaChess
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        private const int RowsCount = 5;
        private const int ColumnsCount = 5;
        private readonly string[,] _tables = new string[RowsCount, ColumnsCount]; 
        private string FirstPlayer { get; }
        private string SecondPlayer { get; }
        private string CurrentPlayer { get; set; }
       
        private string FirstPlayerSymbol { get; } 
        private string SecondPlayerSymbol { get; }
        private string CurrentPlayerSymbol { get; set; }
        
        public GamePage(
            string firstPlayer,
            string firstPlayerSymbol,
            string secondPlayer,
            string secondPlayerSymbol
        ) {
            this.FirstPlayer = firstPlayer;
            this.FirstPlayerSymbol = firstPlayerSymbol;
            
            this.SecondPlayer = secondPlayer;
            this.SecondPlayerSymbol = secondPlayerSymbol;
            
            InitializeComponent();

            SetCurrentPlayerData(this.FirstPlayer, FirstPlayerSymbol);
            RenderGameBoard();
        }

        private void SwitchPlayerData()
        {
            if (CurrentPlayer == FirstPlayer)
            {
                SetCurrentPlayerData(SecondPlayer, SecondPlayerSymbol);
            } else
            {
                SetCurrentPlayerData(FirstPlayer, FirstPlayerSymbol);
            }
        }

        private void SetCurrentPlayerData(string player, string symbol)
        {
            this.CurrentPlayer = player;
            this.CurrentPlayerSymbol = symbol;
            CurrentPlayerLabel.Text = "Now is " + CurrentPlayer;
        }

        private void RenderGameBoard()
        {
            Grid grid = new Grid
            {
                RowSpacing = 1,
                ColumnSpacing = 1
            };

            grid.RowDefinitions = new RowDefinitionCollection();
            grid.ColumnDefinitions = new ColumnDefinitionCollection();

            for (int rowCounter = 0; rowCounter < RowsCount; rowCounter++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            }

            for (int columnCounter = 0; columnCounter < ColumnsCount; columnCounter++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int rowCounter = 0; rowCounter < RowsCount; rowCounter++)
            {
                for (int columnCounter = 0; columnCounter < ColumnsCount; columnCounter++)
                {
                    Plate buttonPlate = new Plate
                    {
                        Text = "",
                        FontSize = 30,
                        // HorizontalOptions = LayoutOptions.Fill,
                        // VerticalOptions = LayoutOptions.Fill,
                        BorderColor = Color.Gray,
                        row = rowCounter,
                        column = columnCounter,
                        BackgroundColor = Color.FromHex((columnCounter + rowCounter) % 2 == 0 ? "#2db9b9" : "#527a7a")
                    };

                    buttonPlate.Clicked += SetSymbol;

                    grid.Children.Add(buttonPlate, rowCounter, columnCounter);
                }
            }
            
            BoardUi.Content = grid;
        }

        private async void SetSymbol(object sender, EventArgs e)
        {
            var plate = (sender as Plate);
            
            if (plate == null)
            {
                return;
            }
            
            if (!String.IsNullOrEmpty(plate.Text)){
                return;
            }

            plate.Text = CurrentPlayerSymbol;
            _tables[plate.row, plate.column] = CurrentPlayerSymbol;
           

            var winner = HasWinner(CurrentPlayerSymbol) ? CurrentPlayer : null;
            if (winner != null)
            {
                await DisplayAlert("The Game Finished",$"The {winner} is winner", "New game");
                RenderGameBoard();
                for (int rowCounter = 0; rowCounter < RowsCount; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < ColumnsCount; columnCounter++)
                    {
                        _tables[rowCounter, columnCounter] = "";
                    }
                }
            } else
            {
                SwitchPlayerData();
            }
            
        }

        private bool HasWinner(string player)
        {
            var hasWinner = false;
            var countForWin = 3;

            // check rows
            for (int rowCounter = 0; rowCounter <= RowsCount - countForWin; rowCounter++)
            {
                for (int columnCounter = 0; columnCounter < ColumnsCount; columnCounter++)
                {
                    if (
                        _tables[rowCounter, columnCounter] == player &&
                        _tables[rowCounter+1, columnCounter] == player &&
                        _tables[rowCounter+2, columnCounter] == player
                    ) {
                        hasWinner = true;
                    }
                }
            }
            
            // check columns
            for (int rowCounter = 0; rowCounter < RowsCount; rowCounter++)
            {
                for (int columnCounter = 0; columnCounter <= ColumnsCount - countForWin; columnCounter++)
                {
                    if (
                        _tables[rowCounter, columnCounter] == player &&
                        _tables[rowCounter, columnCounter+1] == player &&
                        _tables[rowCounter, columnCounter+2] == player
                    ) {
                        hasWinner = true;
                    }
                }
            }

            // check diagonals
            for (int rowCounter = 0; rowCounter <= RowsCount - countForWin; rowCounter++)
            {
                for (int columnCounter = 0; columnCounter <= ColumnsCount - countForWin; columnCounter++)
                {
                    // first diagonal
                    if (
                        _tables[rowCounter, columnCounter] == player &&
                        _tables[rowCounter+1, columnCounter+1] == player &&
                        _tables[rowCounter+2, columnCounter+2] == player
                    ) {
                        hasWinner = true;
                    }

                    // second diagonal
                    if (
                        _tables[rowCounter, columnCounter+2] == player &&
                        _tables[rowCounter+1, columnCounter+1] == player &&
                        _tables[rowCounter+2, columnCounter] == player
                    ) {
                        hasWinner = true;
                    }
                }
            }

            return hasWinner;
        }

        private async void GoToMainPage(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MainPage());
        }
    }
}