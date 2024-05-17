namespace LitHub.Services.BooksService
{
    public class BooksService : IBooksService
    {
        private string pattern = "^[a-zA-Z]+$";
        private HashSet<string> books = new HashSet<string>
        {
            "Harry Potter", "Lord of the Rings", "The Blade Itself", "Witcher", "It",
            "If We Were Villains", "A Little Life", "The Little Prince", "Looking for Alaska", "Atomic Habits",
            "Barbie", "Happy Place", "Be Happy", "Have a Good Time", "Everything I Know About Love"
        };
        private Random rand = new Random();

        public string GetRandomBook()
        {
            int randomIndex = rand.Next(books.Count);
            return books.ElementAt(randomIndex);
        }

        public bool AddBook(string book)
        {
            if (!books.Contains(book))
            {
                books.Add(book);
                return true;
            }
            return false;
        }

        public bool RemoveBook(string book)
        {
            return books.Remove(book);
        }

        public bool ReplaceBook(string oldBook, string newBook)
        {
            if (books.Contains(oldBook))
            {
                books.Remove(oldBook);
                books.Add(newBook);
                return true;
            }
            return false;
        }

        public HashSet<string> RetrieveBooks()
        {
            return books;
        }
    }
}
