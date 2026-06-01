using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE
{
    public class ResponseManager
    {
        private Dictionary<string, List<string>> _keywordResponses;
        private Dictionary<string, List<string>> _randomResponsePools;
        private Dictionary<string, string[]> _keywordMapping;

        public ResponseManager()
        {
            InitializeKeywordResponses();
            InitializeRandomResponsePools();
            InitializeKeywordMapping();
        }

        private void InitializeKeywordResponses()
        {
            _keywordResponses = new Dictionary<string, List<string>>
            {
                { "password", new List<string> {
                    "🔐 Use strong, unique passwords for each account - at least 12 characters with a mix of letters, numbers, and symbols!",
                    "🔑 Never share your passwords with anyone, and consider using a password manager to keep track of them securely.",
                    "🛡️ Enable two-factor authentication whenever possible - it adds an extra layer of security beyond just your password."
                }},
                { "scam", new List<string> {
                    "⚠️ If something sounds too good to be true, it probably is! Always verify unexpected offers through official channels.",
                    "📞 Never give personal information over the phone to unsolicited callers. Hang up and call back using a verified number.",
                    "💸 Be wary of urgent payment requests. Scammers create false urgency to bypass your better judgment."
                }},
                { "privacy", new List<string> {
                    "👁️ Review your social media privacy settings regularly - you might be sharing more than you realize!",
                    "🔒 Use a VPN on public Wi-Fi to encrypt your internet traffic and protect your personal information.",
                    "📧 Be selective about what personal information you share online. Once it's out there, it's hard to take back."
                }},
                { "phishing", new List<string> {
                    "🎣 Always check the sender's email address carefully - phishers often use addresses that look almost legitimate.",
                    "🔗 Hover over links before clicking to see the actual URL. If it looks suspicious, don't click!",
                    "📎 Be cautious of unexpected attachments, even from known senders. Their account might have been compromised."
                }},
                { "general", new List<string> {
                    "Keep your software and operating systems updated - security patches fix known vulnerabilities.",
                    "Back up your important data regularly to an external drive or cloud service.",
                    "Use antivirus software and run regular scans to detect and remove malware.",
                    "Be cautious about what you download and only use official app stores.",
                    "Log out of accounts when you're done, especially on shared computers."
                }}
            };
        }

        private void InitializeRandomResponsePools()
        {
            _randomResponsePools = new Dictionary<string, List<string>>
            {
                { "phishing", new List<string> {
                    "🎣 Phishing emails often create a sense of urgency. Take a moment to verify before acting!",
                    "🔍 Look for spelling and grammar mistakes - legitimate companies rarely make these errors.",
                    "📱 If an email asks you to click a link to update payment info, go directly to the website instead.",
                    "🛡️ Report phishing attempts to the legitimate company being impersonated and to anti-phishing organizations."
                }},
                { "password", new List<string> {
                    "🔐 Avoid using personal information like birthdays or pet names in your passwords.",
                    "🔄 Change your passwords immediately if you suspect any account has been compromised.",
                    "📝 Consider using passphrases - a sequence of random words is both secure and memorable.",
                    "🚫 Never use the same password across multiple sites. If one gets breached, all are at risk."
                }},
                { "scam", new List<string> {
                    "💡 Research companies before making purchases, especially if you found them through ads or emails.",
                    "📄 Keep records of your transactions and communications in case you need to report fraud.",
                    "🔨 Trust your instincts - if something feels off, it probably is. Don't let pressure make you ignore red flags."
                }}
            };
        }

        private void InitializeKeywordMapping()
        {
            _keywordMapping = new Dictionary<string, string[]>
            {
                { "password", new[] { "password", "pass", "login", "credentials", "account security", "strong password" } },
                { "scam", new[] { "scam", "fraud", "con", "deception", "rip off", "scheme" } },
                { "privacy", new[] { "privacy", "private", "data", "personal info", "information security", "tracking" } },
                { "phishing", new[] { "phish", "phishing", "fake email", "email scam", "spoof" } }
            };
        }

        public string GetTopicFromInput(string input)
        {
            input = input.ToLower();

            foreach (var mapping in _keywordMapping)
            {
                foreach (var keyword in mapping.Value)
                {
                    if (input.Contains(keyword))
                    {
                        return mapping.Key;
                    }
                }
            }

            return null;
        }

        public string GetResponseForTopic(string topic)
        {
            topic = topic.ToLower();

            if (_keywordResponses.ContainsKey(topic) && _keywordResponses[topic].Count > 0)
            {
                var responses = _keywordResponses[topic];
                return responses[new Random().Next(responses.Count)];
            }

            if (_keywordResponses.ContainsKey("general"))
            {
                var generalResponses = _keywordResponses["general"];
                return generalResponses[new Random().Next(generalResponses.Count)];
            }

            return "I can help you with cybersecurity topics like password safety, avoiding scams, protecting your privacy, and spotting phishing attempts.";
        }

        public string GetRandomResponseForTopic(string topic)
        {
            topic = topic.ToLower();

            if (_randomResponsePools.ContainsKey(topic) && _randomResponsePools[topic].Count > 0)
            {
                var responses = _randomResponsePools[topic];
                return responses[new Random().Next(responses.Count)];
            }

            // Fall back to regular responses if random pool not available
            return GetResponseForTopic(topic);
        }
    }
}
