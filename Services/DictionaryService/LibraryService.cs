using LitHub.DTOs;
using LitHub.Models;
using LitHub.Services.BooksService;

namespace LitHub.Services.LibraryService
{
    public class LibraryService : ILibraryService
    {
        private readonly IBooksService _booksService;
        private static List<LibraryData> _booksWithDefinitions = new ();
        public LibraryService(IBooksService booksService)
        {
            _booksService = booksService;
            InitBooks(_booksService.RetrieveBooks().ToList());
        }
        public bool AddDefinitionToBook(string book, LibraryData definition)
        {
            if (string.IsNullOrWhiteSpace(book) || string.IsNullOrWhiteSpace(definition.Definition))
                return false;

            LibraryData existingBook = _booksWithDefinitions.FirstOrDefault(w => w.Book == book);
            if (existingBook != null)
            {
                existingBook.Definition = definition.Definition;
                existingBook.Valid = definition.Valid;
            }
            else
            {
                var libr = new LibraryData() { Valid=definition.Valid, Definition = definition.Definition, Book = book };
                _booksWithDefinitions.Add(libr);
                _booksService.AddBook(book);
            }

            return true;
        }

        public List<LibraryData> GetLibrary()
        {
            return _booksWithDefinitions;
        }

        public bool ChangeDefinitionOfBook(string book, string newDefinition)
        {
            if (string.IsNullOrWhiteSpace(book) || string.IsNullOrWhiteSpace(newDefinition))
                return false;

            var existingBook = _booksWithDefinitions.FirstOrDefault(w => w.Book == book);
            if (existingBook != null)
            {
                existingBook.Valid = true;
                existingBook.Definition = newDefinition;
                return true;
            }

            return false;
        }

        public bool RemoveDefinitionFromBook(string book)
        {
            if (string.IsNullOrWhiteSpace(book))
                return false;

            var existingBook = _booksWithDefinitions.FirstOrDefault(w => w.Book == book);
            if (existingBook != null)
            {
                existingBook.Definition = null;
                existingBook.Valid = false;
                return true;
            }

            return false;
        }

        private void InitBooks(List<string> books)
        {

            foreach (var book in books)
            {
                if (!_booksWithDefinitions.Any(w => w.Book == book))
                {
                    string definition = null;
                    var libr = new LibraryData() { Valid = false, Definition = definition, Book = book };
                    _booksWithDefinitions.Add(libr);
                }
            }
        }
    }
}
