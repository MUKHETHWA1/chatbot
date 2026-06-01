using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE
{
    public class ChatbotEngine
    {
        private ResponseManager _responseManager;
        private MemoryManager _memoryManager;
        private SentimentAnalyzer _sentimentAnalyzer;
        private string _currentTopic;
        private string _lastResponse;
        private string _lastDetectedSentiment;

        public ChatbotEngine()
        {
            _responseManager = new ResponseManager();
            _memoryManager = new MemoryManager();
            _sentimentAnalyzer = new SentimentAnalyzer();
            _currentTopic = "";
            _lastResponse = "";
            _lastDetectedSentiment = "neutral";
        }

        public string GetResponse(string userInput)
        {
            string lowerInput = userInput.ToLower().Trim();

            // Detect sentiment
            _lastDetectedSentiment = _sentimentAnalyzer.DetectSentiment(lowerInput);

            // Handle name input first time
            if (!_memoryManager.HasName() && !IsNameAlreadyAsked())
            {
                _memoryManager.SetName(userInput);
                return GetPersonalizedResponse($"Nice to meet you, {_memoryManager.GetName()}! " +
                    "I can help you with cybersecurity topics like passwords, scams, privacy, and phishing. What would you like to know?");
            }

            // Handle follow-up requests
            if (IsFollowUpRequest(lowerInput))
            {
                if (!string.IsNullOrEmpty(_currentTopic))
                {
                    return _responseManager.GetRandomResponseForTopic(_currentTopic);
                }
                return "What topic would you like me to tell you more about? Try asking about passwords, scams, privacy, or phishing.";
            }
            string topics;

            // Check for topic preference memory
            if (lowerInput.Contains("interested in") || lowerInput.Contains("my favourite topic is") || lowerInput.Contains("i like"))
            {
                 topics = ExtractTopic(lowerInput);
                if (!string.IsNullOrEmpty(topics))
                {
                    _memoryManager.SetFavoriteTopic(topics);
                    return GetPersonalizedResponse($"Great! I'll remember that you're interested in {topics}. " +
                        _responseManager.GetResponseForTopic(topics));
                }
            }

            // Check for sentiment-based empathetic responses
            if (_lastDetectedSentiment == "worried" || _lastDetectedSentiment == "frustrated")
            {
                string extractedTopic = ExtractTopicFromInput(lowerInput);
                string topicResponse = "";
                if (extractedTopic != null)
                {
                    topicResponse = _responseManager.GetResponseForTopic(extractedTopic);
                }
                else
                {
                    topicResponse = _responseManager.GetResponseForTopic("general");
                }
                return GetEmpatheticResponse(_lastDetectedSentiment) + " " + topicResponse;
            }

            // Get regular response
            string topic = _responseManager.GetTopicFromInput(lowerInput);
            if (!string.IsNullOrEmpty(topic))
            {
                _currentTopic = topic;
                string response = _responseManager.GetResponseForTopic(topic);
                _lastResponse = response;
                return GetPersonalizedResponse(response);
            }

            // Default/error handling
            return GetPersonalizedResponse("I'm not sure I understand. Can you try rephrasing? " +
                "You can ask me about passwords, scams, privacy, phishing, or just say 'another tip' for more information.");
        }

        private bool IsNameAlreadyAsked()
        {
            return _memoryManager.HasName();
        }

        private bool IsFollowUpRequest(string input)
        {
            string[] followUpKeywords = {
                "another tip", "tell me more", "explain more",
                "more info", "continue", "more please", "another"
            };

            foreach (var keyword in followUpKeywords)
            {
                if (input.Contains(keyword))
                    return true;
            }
            return false;
        }

        private string ExtractTopic(string input)
        {
            if (input.Contains("password")) return "password";
            if (input.Contains("scam")) return "scam";
            if (input.Contains("privacy")) return "privacy";
            if (input.Contains("phish")) return "phishing";
            return null;
        }

        private string ExtractTopicFromInput(string input)
        {
            return ExtractTopic(input);
        }

        private string GetPersonalizedResponse(string response)
        {
            string name = _memoryManager.GetName();
            if (!string.IsNullOrEmpty(name))
            {
                response = response.Replace("you", name);
                if (!response.Contains(name) && response.Length > 10)
                {
                    string firstChar = response.Substring(0, 1).ToUpper();
                    string rest = response.Substring(1).ToLower();
                    return $"{name}, {firstChar}{rest}";
                }
            }

            string favoriteTopic = _memoryManager.GetFavoriteTopic();
            if (!string.IsNullOrEmpty(favoriteTopic) && !response.Contains(favoriteTopic))
            {
                Random rand = new Random();
                if (rand.Next(5) == 0) // 20% chance to remind about fav topic
                {
                    response += $" As someone interested in {favoriteTopic}, you might find that particularly helpful.";
                }
            }

            return response;
        }

        private string GetEmpatheticResponse(string sentiment)
        {
            var empatheticResponses = new Dictionary<string, string[]>();
            empatheticResponses.Add("worried", new[] {
                "It's completely understandable to feel worried about online security.",
                "I understand your concern. Many people feel this way about cybersecurity.",
                "Your worry is valid. Let me help you feel more secure."
            });
            empatheticResponses.Add("frustrated", new[] {
                "I hear your frustration. Cybersecurity can feel overwhelming sometimes.",
                "I understand it can be frustrating. Let me help simplify this for you.",
                "Take a deep breath. I'm here to make this easier for you."
            });

            if (empatheticResponses.ContainsKey(sentiment))
            {
                var responses = empatheticResponses[sentiment];
                Random rand = new Random();
                return responses[rand.Next(responses.Length)];
            }

            return "I'm here to help you stay safe online.";
        }

        public string GetLastDetectedSentiment()
        {
            return _lastDetectedSentiment;
        }

        public string GetMemoryInfo()
        {
            return _memoryManager.GetMemorySummary();
        }
    }
}
