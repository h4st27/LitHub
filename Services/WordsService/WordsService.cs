using System.Net;
using System.Text.RegularExpressions;

namespace MyApp.Services.WordsService
{
    public class WordsService:IWordsService
    {
        string pattern = "^[a-zA-Z]+$";
        static private List<string> words = new List<string>
        {
            "beautiful", "happy", "exciting", "creative", "wonderful",
            "amazing", "fantastic", "awesome", "brilliant", "joyful",
            "lovely", "delightful", "fun", "inspiring", "peaceful"
        };
        public string GetRandomWord()
        {
            Random rand = new Random();
            int randomIndex = rand.Next(words.Count);
            return words[randomIndex];
        }
        public bool AddWord(string word)
        {
            if (!words.Contains(word.Trim().ToLower()))
            {
                words.Add(word.Trim().ToLower());
                return true;
            }
            return false;
        }
        public bool RemoveWord(string word)
        {
            return words.Remove(word.Trim().ToLower());
        }
        public bool ReplaceWord(string oldWord, string newWord)
        {
            int index = words.IndexOf(oldWord.Trim().ToLower());
            if (index != -1)
            {
                words[index] = newWord.Trim().ToLower();
                return true;
            }
            return false;
        }
        public bool ValidateWord(string word)
        {
            string trimmedWord = word.Trim().ToLower();
            if (Regex.IsMatch(trimmedWord, pattern) && trimmedWord.Length != 0)
            {
                return true;
            }
            return false;
        }
        public List<string> RetrieveWords()
        {
            return words;
        }
    }
}
