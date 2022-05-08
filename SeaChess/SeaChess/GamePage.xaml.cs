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
        private const int ROWS_COUNT = 5;
        private const int COLUMNS_COUNT = 5;
        private const string FIRST_SYMBOL = "O";
        private const string SECOND_SYMBOL = "X";
        private string[,] values = new string[ROWS_COUNT, COLUMNS_COUNT]; 
        private string firstPlayer { get; set; }
        private string secondPlayer { get; set; }
        private string currentPlayer { get; set; }
        public GamePage(string firstPlayer, string secondPlayer)
        {
            this.firstPlayer = firstPlayer;
            this.secondPlayer = secondPlayer;
            InitializeComponent();

            chooseRandomPlayer();
            renderBoard();
        }

        private void chooseRandomPlayer()
        {

            Random r = new Random();
            string[] players = new string[] { firstPlayer, secondPlayer };
            string randomPlayer = players[r.Next(0, players.Length)];
            setCurrentPlayer(randomPlayer);
        }

        private void setCurrentPlayer(string player)
        {
            this.currentPlayer = player;
            currentPlayerLabel.Text = "Now is " + currentPlayer;
        }

        private void switchPlayers()
        {
            if (currentPlayer == firstPlayer)
            {
                setCurrentPlayer(secondPlayer);
            } else
            {
                setCurrentPlayer(firstPlayer);
            }
        }

        private void renderBoard()
        {
            Grid grid = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 0
            };

            grid.RowDefinitions = new RowDefinitionCollection();
            grid.ColumnDefinitions = new ColumnDefinitionCollection();

            for (int rowCounter = 0; rowCounter < ROWS_COUNT; rowCounter++)
            {

                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int columnCounter = 0; columnCounter < COLUMNS_COUNT; columnCounter++)
            {

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int rowCounter = 0; rowCounter < ROWS_COUNT; rowCounter++)
            {
                for (int columnCounter = 0; columnCounter < COLUMNS_COUNT; columnCounter++)
                {
                    Tile button = new Tile
                    {
                        Text = "",
                        FontSize = 30,
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        BackgroundColor = Color.AliceBlue,
                        BorderColor = Color.Gray,
                        row = rowCounter,
                        column = columnCounter
                    };

                    button.Clicked += onClickTile;

                    grid.Children.Add(button, rowCounter, columnCounter);
                }
            }
            
            boardUi.Content = grid;
        }

        private async void onClickTile(object sender, EventArgs e)
        {
            var tile = (sender as Tile);
            if (!String.IsNullOrEmpty(tile.Text)){
                return;
            }

            if (currentPlayer == firstPlayer)
            {
                tile.Text = FIRST_SYMBOL;
                values[tile.row, tile.column] = FIRST_SYMBOL;
            } else
            {
                tile.Text = SECOND_SYMBOL;
                values[tile.row, tile.column] = SECOND_SYMBOL;
            }

            var winner = checkWinner(FIRST_SYMBOL) ? firstPlayer : checkWinner(SECOND_SYMBOL) ? secondPlayer : null;
            if (winner != null)
            {
                await DisplayAlert("Game end", winner + " is winner", "New game");
                renderBoard();
                for (int rowCounter = 0; rowCounter < ROWS_COUNT; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < COLUMNS_COUNT; columnCounter++)
                    {
                        values[rowCounter, columnCounter] = "";
                    }
                }
            } else
            {
                switchPlayers();
            }
            
        }

        private bool checkWinner(string player)
        {
            var hasWinner = false;
            var minimumCount = 3;

            // check columns
            for (int rowCounter = 0; rowCounter < ROWS_COUNT; rowCounter++)
            {
                
                for (int columnCounter = 0; columnCounter <= COLUMNS_COUNT - minimumCount; columnCounter++)
                {
                    if (values[rowCounter, columnCounter] == player &&
                        values[rowCounter, columnCounter+1] == player &&
                        values[rowCounter, columnCounter+2] == player)
                    {
                        hasWinner=true;
                    }
                }
            }

            // check rows
            for (int rowCounter = 0; rowCounter <= ROWS_COUNT - minimumCount; rowCounter++)
            {
                for (int columnCounter = 0; columnCounter < COLUMNS_COUNT; columnCounter++)
                {
                    if (values[rowCounter, columnCounter] == player &&
                        values[rowCounter+1, columnCounter] == player &&
                        values[rowCounter+2, columnCounter] == player)
                    {
                        hasWinner = true;
                    }
                }
            }

            // check diagonals
            for (int rowCounter = 0; rowCounter <= ROWS_COUNT - minimumCount; rowCounter++)
            {
                for (int columnCounter = 0; columnCounter <= COLUMNS_COUNT - minimumCount; columnCounter++)
                {
                    // first diagonal
                    if (values[rowCounter, columnCounter] == player &&
                        values[rowCounter+1, columnCounter+1] == player &&
                        values[rowCounter+2, columnCounter+2] == player)
                    {
                        hasWinner = true;
                    }

                    // second diagonal
                    if (values[rowCounter, columnCounter+2] == player &&
                        values[rowCounter+1, columnCounter+1] == player &&
                        values[rowCounter+2, columnCounter] == player)
                    {
                        hasWinner = true;
                    }
                }
            }

            return hasWinner;
        }
    }
}