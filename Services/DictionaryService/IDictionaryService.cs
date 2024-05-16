using Libra.Models;

namespace Libra.Services.DictionaryService
{
    public interface IDictionaryService
    {
        List<DictionaryData> GetDictionary();
        bool AddDefinitionToWord(string word, DictionaryData definition);
        bool RemoveDefinitionFromWord(string word);
        bool ChangeDefinitionOfWord(string word,string definition);
    }
}
