namespace Libra.Services.WordsService
{
    public class WordsService : IWordsService
    {
        private string pattern = "^[a-zA-Z]+$";
        private HashSet<string> words = new HashSet<string>
        {
            "beautiful", "happy", "exciting", "creative", "wonderful",
            "amazing", "fantastic", "awesome", "brilliant", "joyful",
            "lovely", "delightful", "fun", "inspiring", "peaceful"
        };
        private Random rand = new Random();

        public string GetRandomWord()
        {
            int randomIndex = rand.Next(words.Count);
            return words.ElementAt(randomIndex);
        }

        public bool AddWord(string word)
        {
            if (!words.Contains(word))
            {
                words.Add(word);
                return true;
            }
            return false;
        }

        public bool RemoveWord(string word)
        {
            return words.Remove(word);
        }

        public bool ReplaceWord(string oldWord, string newWord)
        {
            if (words.Contains(oldWord))
            {
                words.Remove(oldWord);
                words.Add(newWord);
                return true;
            }
            return false;
        }

        public HashSet<string> RetrieveWords()
        {
            return words;
        }
    }
}
