using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE
{
    public class MemoryManager
    {
        private string _userName;
        private string _favoriteTopic;
        private List<string> _discussedTopics;
        private Dictionary<string, string> _userPreferences;
        private DateTime _conversationStartTime;
        private int _messageCount;

        public MemoryManager()
        {
            _discussedTopics = new List<string>();
            _userPreferences = new Dictionary<string, string>();
            _conversationStartTime = DateTime.Now;
            _messageCount = 0;
        }

        public void SetName(string name)
        {
            _userName = name.Trim();
            _messageCount++;
        }

        public string GetName()
        {
            return _userName;
        }

        public bool HasName()
        {
            return !string.IsNullOrEmpty(_userName);
        }

        public void SetFavoriteTopic(string topic)
        {
            _favoriteTopic = topic;
            AddDiscussedTopic(topic);
        }

        public string GetFavoriteTopic()
        {
            return _favoriteTopic;
        }

        public void AddDiscussedTopic(string topic)
        {
            if (!_discussedTopics.Contains(topic))
            {
                _discussedTopics.Add(topic);
            }
        }

        public List<string> GetDiscussedTopics()
        {
            return new List<string>(_discussedTopics);
        }

        public void SetUserPreference(string key, string value)
        {
            if (_userPreferences.ContainsKey(key))
            {
                _userPreferences[key] = value;
            }
            else
            {
                _userPreferences.Add(key, value);
            }
        }

        public string GetUserPreference(string key)
        {
            return _userPreferences.ContainsKey(key) ? _userPreferences[key] : null;
        }

        public void IncrementMessageCount()
        {
            _messageCount++;
        }

        public int GetMessageCount()
        {
            return _messageCount;
        }

        public TimeSpan GetConversationDuration()
        {
            return DateTime.Now - _conversationStartTime;
        }

        public string GetMemorySummary()
        {
            string summary = "📝 ";

            if (!string.IsNullOrEmpty(_userName))
            {
                summary += $"User: {_userName} | ";
            }

            if (!string.IsNullOrEmpty(_favoriteTopic))
            {
                summary += $"⭐ Fav topic: {_favoriteTopic} | ";
            }

            if (_discussedTopics.Count > 0)
            {
                summary += $"📚 Discussed: {string.Join(", ", _discussedTopics)} | ";
            }

            summary += $"💬 Messages: {_messageCount}";

            if (string.IsNullOrEmpty(_userName) && string.IsNullOrEmpty(_favoriteTopic))
            {
                summary = "📝 No user data stored yet - start a conversation!";
            }

            return summary;
        }

        public void ClearMemory()
        {
            _userName = null;
            _favoriteTopic = null;
            _discussedTopics.Clear();
            _userPreferences.Clear();
        }
    }
}
