using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE
{
    public class SentimentAnalyzer
    {
        private Dictionary<string, string[]> _sentimentKeywords;

        public SentimentAnalyzer()
        {
            InitializeSentimentKeywords();
        }

        private void InitializeSentimentKeywords()
        {
            _sentimentKeywords = new Dictionary<string, string[]>
            {
                { "worried", new[] {
                    "worried", "concerned", "nervous", "anxious", "scared",
                    "afraid", "unsafe", "vulnerable", "paranoid", "fear",
                    "what if", "i'm afraid", "i'm scared", "help i"
                }},
                { "frustrated", new[] {
                    "frustrated", "annoyed", "angry", "mad", "tired of",
                    "sick of", "hate", "too hard", "difficult", "confusing",
                    "complicated", "overwhelming", "why is this", "can't"
                }},
                { "curious", new[] {
                    "curious", "interested", "want to learn", "tell me about",
                    "how do i", "what is", "explain", "teach me", "learn",
                    "i want to know", "can you show", "how to"
                }},
                { "happy", new[] {
                    "happy", "great", "good", "excellent", "awesome",
                    "thank you", "helpful", "useful", "love", "like this",
                    "wonderful", "fantastic", "amazing"
                }}
            };
        }

        public string DetectSentiment(string input)
        {
            input = input.ToLower();

            foreach (var sentiment in _sentimentKeywords)
            {
                foreach (var keyword in sentiment.Value)
                {
                    if (input.Contains(keyword))
                    {
                        return sentiment.Key;
                    }
                }
            }

            return "neutral";
        }

        public bool IsNegativeSentiment(string sentiment)
        {
            return sentiment == "worried" || sentiment == "frustrated";
        }

        public bool IsPositiveSentiment(string sentiment)
        {
            return sentiment == "curious" || sentiment == "happy";
        }

        public string GetSentimentEmoji(string sentiment)
        {
            if (sentiment == "worried") return "😟";
            if (sentiment == "frustrated") return "😤";
            if (sentiment == "curious") return "🤔";
            if (sentiment == "happy") return "😊";
            return "😐";
        }
    }
}
