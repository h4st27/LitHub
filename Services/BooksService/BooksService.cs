namespace MyApp.Services.BooksService
{
    public class BooksService : IBooksService
    {
        private string pattern = "^[a-zA-Z]+$";
        private HashSet<string> books = new HashSet<string>
        {
            "HarryPotter", "LordOfTheRings", "TheBladeItself", "Witcher", "It",
            "IfWeWereVillains", "ALittleLife", "TheLittlePrince", "Lookingfor Alaska", "AtomicHabits",
            "Barbie", "HappyPlace", "BeHappy", "HaveAGoodTime", "EverythingIKnowAboutLove"
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
