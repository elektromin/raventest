namespace raventest
{
    using Raven.Client.Document;
    using Raven.Client.Linq;
    using System.Collections.Generic;
    using System.Linq;

    class Program
    {
        private static DocumentStore docStore;
        static void Main(string[] args)
        {
            docStore = new DocumentStore { Url = "http://localhost:8080/", DefaultDatabase = "raventest"};
            docStore.Initialize();

            //var id = CreateSong(new Song { Title = "We Can Work It Out", Artist = "Chaka Khan", Producer = "Arif Mardin" });
            //var song = LoadSong(id);

            var songs = FindSongsByArtist("Chaka Khan");
            foreach (var s in songs)
            {
                System.Console.WriteLine("Title: " + s.Title + ", Artist: " + s.Artist + ", Producer: " + s.Producer);
            }

            System.Threading.Thread.Sleep(5000);
        }

        private static List<Song> FindSongsByArtist(string artist)
        {
            using (var session = docStore.OpenSession())
            {
                return session.Query<Song>().Where(x => x.Artist == artist).ToList();
            }
        }

        private static object LoadSong(string id)
        {
            using (var session = docStore.OpenSession())
            {
                var song = session.Load<Song>(id);
                System.Console.WriteLine("Title: " + song.Title);
                return song;
            }
        }

        private static string CreateSong(Song song)
        {
            using (var session = docStore.OpenSession())
            {
                var entity = song;
                session.Store(entity);
                session.SaveChanges();
                System.Console.WriteLine("Created entity id: " + entity.Id);
                return entity.Id;
            }
        }
    }

    class Song
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string Producer { get; set; }
    }
}
