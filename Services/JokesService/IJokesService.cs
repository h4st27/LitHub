namespace MyApp.Services.JokesService
{
    public interface IJokesService
    {
        string GetRandomJoke();
        bool AddJoke(string joke);
        bool ReplaceJoke(int id, string newJoke);
        bool RemoveJoke(int id);
        HashSet<string> RetrieveJokes();
        string GetJokeById(int id);
    }
}
