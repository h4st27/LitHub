using MyApp.Models;

namespace MyApp.Services.DictionaryService
{
    public interface IDictionaryService
    {
        List<DictionaryData> GetDictionary();
        bool AddDefinitionToWord(string word, DictionaryData definition);
        bool RemoveDefinitionFromWord(string word);
        bool ChangeDefinitionOfWord(string word,string definition);
    }
}
