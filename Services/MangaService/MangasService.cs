namespace MyApp.Services.MangasService
{
    public class MangasService : IMangasService
    {
        private readonly HashSet<string> mangas = new HashSet<string>
        {
            "ShingekiNoKyojin",
            "JujutsuKaisen",
            "Naruto",
            "Bleach",
            "KageNoJitsuryokushaNiNaritakute!",
            "TateNoYuushaNoNariagari",
            "BokuDakeGaInaiMachi",
            "TenkiNoKo",
            "YuukokuNoMoriarty"
        };
        private Random rand = new Random();

        public string GetRandomManga()
        {
            int randomIndex = rand.Next(mangas.Count);
            return mangas.ElementAt(randomIndex);
        }

        public bool AddManga(string manga)
        {
            if (!mangas.Contains(manga))
            {
                mangas.Add(manga);
                return true;
            }
            return false;
        }

        public bool RemoveManga(int id)
        {
            string mangaToRemove = GetMangaById(id);
            if (mangaToRemove != null)
            {
                mangas.Remove(mangaToRemove);
                return true;
            }
            return false;
        }

        public bool ReplaceManga(int id, string newManga)
        {
            if (id >= 0 && id < mangas.Count && !mangas.Contains(newManga))
            {
                List<string> mangasList = mangas.ToList();
                mangasList[id] = newManga;
                mangas.Clear();
                mangas.UnionWith(mangasList);
                return true;
            }
            return false;
        }
        public HashSet<string> RetrieveMangas()
        {
            return mangas;
        }

        public string GetMangaById(int id)
        {
            if (id >= 0 && id < mangas.Count)
            {
                List<string> mangasList = mangas.ToList();
                return mangasList[id];
            }
            return null;
        }
    }
}
