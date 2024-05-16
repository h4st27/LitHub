using MyApp.DTOs;
using MyApp.Models;
using MyApp.Services.WordsService;

namespace MyApp.Services.DictionaryService
{
    public class DictionaryService : IDictionaryService
    {
        private readonly IWordsService _wordsService;
        private static List<DictionaryData> _wordsWithDefinitions = new ();
        public DictionaryService(IWordsService wordsService)
        {
            _wordsService = wordsService;
            InitWords(_wordsService.RetrieveWords().ToList());
        }
        public bool AddDefinitionToWord(string word, DictionaryData definition)
        {
            if (string.IsNullOrWhiteSpace(word) || string.IsNullOrWhiteSpace(definition.Definition))
                return false;

            DictionaryData existingWord = _wordsWithDefinitions.FirstOrDefault(w => w.Word == word);
            if (existingWord != null)
            {
                existingWord.Definition = definition.Definition;
                existingWord.Valid = definition.Valid;
            }
            else
            {
                var dict = new DictionaryData() { Valid=definition.Valid, Definition = definition.Definition, Word = word };
                _wordsWithDefinitions.Add(dict);
                _wordsService.AddWord(word);
            }

            return true;
        }

        public List<DictionaryData> GetDictionary()
        {
            return _wordsWithDefinitions;
        }

        public bool ChangeDefinitionOfWord(string word, string newDefinition)
        {
            if (string.IsNullOrWhiteSpace(word) || string.IsNullOrWhiteSpace(newDefinition))
                return false;

            var existingWord = _wordsWithDefinitions.FirstOrDefault(w => w.Word == word);
            if (existingWord != null)
            {
                existingWord.Valid = true;
                existingWord.Definition = newDefinition;
                return true;
            }

            return false;
        }

        public bool RemoveDefinitionFromWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return false;

            var existingWord = _wordsWithDefinitions.FirstOrDefault(w => w.Word == word);
            if (existingWord != null)
            {
                existingWord.Definition = null;
                existingWord.Valid = false;
                return true;
            }

            return false;
        }

        private void InitWords(List<string> words)
        {

            foreach (var word in words)
            {
                if (!_wordsWithDefinitions.Any(w => w.Word == word))
                {
                    string definition = null;
                    var dict = new DictionaryData() { Valid = false, Definition = definition, Word = word };
                    _wordsWithDefinitions.Add(dict);
                }
            }
        }
    }
}
