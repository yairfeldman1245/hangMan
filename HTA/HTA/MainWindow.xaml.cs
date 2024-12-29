using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace HTA
{
    public partial class MainWindow : Window
    {
        private List<string> wordList = new List<string> { "PLATE", "CUP", "WATER" };
        private string selectedWord;
        private List<char> guessedLetters = new List<char>();
        private int currentFails = 0;
        private const int maxFails = 6;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            currentFails = 0;
            guessedLetters.Clear();
            selectedWord = wordList[new Random().Next(wordList.Count)];
            UpdateWordDisplay();
            UpdateHangmanImage();
            CreateLetterButtons();
        }

        private void UpdateWordDisplay()
        {
            WordContainer.Children.Clear();
            foreach (char letter in selectedWord)
            {
                TextBlock textBlock = new TextBlock
                {
                    Text = guessedLetters.Contains(letter) ? letter.ToString() : "_",
                    FontSize = 36,
                    Width = 40,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(5)
                };
                WordContainer.Children.Add(textBlock);
            }
        }

        private void UpdateHangmanImage()
        {
            string imagePath = $"images/man{currentFails}.jpg";
            if (System.IO.File.Exists(imagePath))
            {
                ManImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            }
        }

        private void CreateLetterButtons()
        {
            ButtonContainer.Children.Clear();
            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                Button button = new Button
                {
                    Content = letter.ToString(),
                    Width = 40,
                    Height = 40,
                    Margin = new Thickness(5)
                };
                button.Click += (s, e) => GuessLetter(letter, button);
                ButtonContainer.Children.Add(button);
            }
        }

        private void GuessLetter(char letter, Button button)
        {
            button.IsEnabled = false;

            if (selectedWord.Contains(letter))
            {
                guessedLetters.Add(letter);
                UpdateWordDisplay();
                if (selectedWord.All(c => guessedLetters.Contains(c)))
                {
                    MessageBox.Show("Congratulations! You've guessed the word!");
                    DisableAllButtons();
                }
            }
            else
            {
                currentFails++;
                UpdateHangmanImage();
                if (currentFails >= maxFails)
                {
                    MessageBox.Show($"Game Over! The word was: {selectedWord}");
                    DisableAllButtons();
                }
            }
        }

        private void DisableAllButtons()
        {
            foreach (Button button in ButtonContainer.Children.OfType<Button>())
            {
                button.IsEnabled = false;
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame();
        }

        private void ShowSolutionButton_Click(object sender, RoutedEventArgs e)
        {
            guessedLetters = selectedWord.ToCharArray().ToList();
            UpdateWordDisplay();
            DisableAllButtons();
        }
    }
}
