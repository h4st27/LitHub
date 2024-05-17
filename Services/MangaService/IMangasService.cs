namespace MyApp.Services.MangasService
{
    public interface IMangasService
    {
        string GetRandomManga();
        bool AddManga(string manga);
        bool ReplaceManga(int id, string newManga);
        bool RemoveManga(int id);
        HashSet<string> RetrieveMangas();
        string GetMangaById(int id);
    }
}
