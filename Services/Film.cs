
namespace MyApp.Services
{
    internal class Film
    {
        private int id;
        public string name;
        internal bool isWatched;
        protected double rating;
        protected internal char letterRating;
        public Film(int id, string name, bool isWatched, double rating, char letterRating)
        {
            this.id = id;
            this.name = name;
            this.isWatched = isWatched;
            this.rating = rating;
            this.letterRating = letterRating;
        }
        public void WriteFullRating()
        {
            Console.WriteLine($"This movies has {rating} points and totally {letterRating} rating");
        }

        protected int GetId()
        {
            return id;
        }

        public void WriteInfo(Film film)
        {
            string isWatchedMessage = isWatched ? "is watched" : "is not watched";
            string preferToWatchMessage = isWatched ? "watch next one" : "watch it at any time tou want";
            Console.WriteLine($"{name} {isWatchedMessage}, {preferToWatchMessage}. You also can watch {film.name} ");
        }
    }
}
