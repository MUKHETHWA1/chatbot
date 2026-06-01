using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProgPOE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChatbotEngine _chatbot;
        private string _audioPath;

        public MainWindow()
        {
            InitializeComponent();
            InitializeChatbot();
            PlayVoiceGreeting();
            DisplayBotMessage("Hello! I'm your Cybersecurity Awareness Bot. What's your name?", "🤖");
        }

        private void InitializeChatbot()
        {
            _chatbot = new ChatbotEngine();
            _audioPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio", "greeting.wav");
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                if (System.IO.File.Exists(_audioPath))
                {
                    using (var player = new SoundPlayer(_audioPath))
                    {
                        player.Play();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Voice greeting error: {ex.Message}");
            }
        }

        private void AppendMessage(string sender, string message, Brush color)
        {
            var paragraph = new Paragraph();

            var senderRun = new Run($"{sender}: ") { FontWeight = FontWeights.Bold, Foreground = color };
            var messageRun = new Run(message) { Foreground = Brushes.White };

            paragraph.Inlines.Add(senderRun);
            paragraph.Inlines.Add(messageRun);
            paragraph.Margin = new Thickness(0, 5, 0, 5);

            ChatDisplay.Document.Blocks.Add(paragraph);
            ChatScrollViewer.ScrollToEnd();
        }

        private void DisplayUserMessage(string message)
        {
            AppendMessage("👤 You", message, Brushes.LightBlue);
        }

        private void DisplayBotMessage(string message, string prefix = "🤖 Bot")
        {
            AppendMessage(prefix, message, Brushes.LightGreen);

            // Update status bar sentiment display
            var sentiment = _chatbot.GetLastDetectedSentiment();
            UpdateSentimentStatus(sentiment);
            UpdateMemoryStatus();
        }

        private void UpdateSentimentStatus(string sentiment)
        {
            string icon = "😐";
            if (sentiment == "worried") icon = "😟";
            else if (sentiment == "frustrated") icon = "😤";
            else if (sentiment == "curious") icon = "🤔";
            else if (sentiment == "happy") icon = "😊";

            SentimentStatus.Text = $"{icon} Sentiment: {(sentiment ?? "Neutral")}";

            // Change status color based on sentiment
            if (sentiment == "worried" || sentiment == "frustrated")
                SentimentStatus.Foreground = Brushes.Orange;
            else if (sentiment == "curious")
                SentimentStatus.Foreground = Brushes.LightBlue;
            else
                SentimentStatus.Foreground = Brushes.LightGreen;
        }

        private void UpdateMemoryStatus()
        {
            var memory = _chatbot.GetMemoryInfo();
            MemoryStatus.Text = memory;
            StatusText.Text = "✅ Bot is ready";
        }

        private void ProcessUserInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                DisplayBotMessage("Please enter a message. I'm here to help you with cybersecurity!", "⚠️ Bot");
                return;
            }

            DisplayUserMessage(input);

            var response = _chatbot.GetResponse(input);
            DisplayBotMessage(response);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var input = UserInput.Text.Trim();
            ProcessUserInput(input);
            UserInput.Clear();
            UserInput.Focus();
        }

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendButton_Click(sender, e);
            }
        }

        private void QuickTip_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var clickedTopic = button.Content.ToString();
                ProcessUserInput(clickedTopic);
            }
        }
    }
}
