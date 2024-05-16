namespace MyApp.Services.WordsService
{
    public interface IWordsService
    {
        bool ValidateWord(string word);
        string GetRandomWord();
        bool AddWord(string word);
        bool ReplaceWord(string oldWord, string newWord);
        bool RemoveWord(string word);
        List<string> RetrieveWords();
    }
}
