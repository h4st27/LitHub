namespace MyApp.Services.JokesService
{
    public class JokesService : IJokesService
    {
        private readonly HashSet<string> jokes = new HashSet<string>
        {
            "Why don't scientists trust atoms? Because they make up everything!",
            "What do you call cheese that isn't yours? Nacho cheese!",
            "Why did the scarecrow win an award? Because he was outstanding in his field!",
            "I told my wife she was drawing her eyebrows too high. She looked surprised.",
            "I'm reading a book on anti-gravity. It's impossible to put down!",
            "What did one hat say to the other? You stay here, I'll go on ahead.",
            "Why couldn't the bicycle stand up by itself? It was two-tired!",
            "I'm on a whiskey diet. I've lost three days already!",
            "Why did the tomato turn red? Because it saw the salad dressing!",
            "What do you get when you cross a snowman and a vampire? Frostbite!"
        };
        private Random rand = new Random();

        public string GetRandomJoke()
        {
            int randomIndex = rand.Next(jokes.Count);
            return jokes.ElementAt(randomIndex);
        }

        public bool AddJoke(string joke)
        {
            if (!jokes.Contains(joke))
            {
                jokes.Add(joke);
                return true;
            }
            return false;
        }

        public bool RemoveJoke(int id)
        {
            string jokeToRemove = GetJokeById(id);
            if (jokeToRemove != null)
            {
                jokes.Remove(jokeToRemove);
                return true;
            }
            return false;
        }

        public bool ReplaceJoke(int id, string newJoke)
        {
            if (id >= 0 && id < jokes.Count && !jokes.Contains(newJoke))
            {
                List<string> jokesList = jokes.ToList();
                jokesList[id] = newJoke;
                jokes.Clear();
                jokes.UnionWith(jokesList);
                return true;
            }
            return false;
        }
        public HashSet<string> RetrieveJokes()
        {
            return jokes;
        }

        public string GetJokeById(int id)
        {
            if (id >= 0 && id < jokes.Count)
            {
                List<string> jokesList = jokes.ToList();
                return jokesList[id];
            }
            return null;
        }
    }
}
