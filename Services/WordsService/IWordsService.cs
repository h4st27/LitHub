namespace MyApp.Services.WordsService
{
    public interface IWordsService
    {
        string GetRandomWord();
        bool AddWord(string word);
        bool ReplaceWord(string oldWord, string newWord);
        bool RemoveWord(string word);
        HashSet<string> RetrieveWords();
    }
}
