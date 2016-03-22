using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artists
{
    class Program
    {
        static void Main(string[] args)
        {
            var artists = CreateArtists();
            var albums = CreateAlbums();
            var songs = CreateSongs();

            // Koj artist ima najdolgo ime?
            var artistWithLongestName = artists.OrderByDescending(a => a.Name.Length).First();

            Console.WriteLine("Artist so najdolgo ime: " + artistWithLongestName.Name);

            // Koj artist prv izdal album?
            var artistWhoFirstPublishedAlbum =
                (from ar in artists
                join al in albums
                    on ar.ArtistID equals al.ArtistID
                orderby al.YearPublished
                select ar).First();

            Console.WriteLine("Prv izdal album: " + artistWhoFirstPublishedAlbum.Name);

            // Koj artist posleden izdal album?
            var artistWhoLastPublishedAlbum =
                (from ar in artists
                 join al in albums
                     on ar.ArtistID equals al.ArtistID
                 orderby al.YearPublished descending
                 select ar).First();

            Console.WriteLine("Posleden izdaden album: " + artistWhoLastPublishedAlbum.Name);

            // Koj artist ima najmnogu albumi?
            var artistWithMostAlbums =
                (from ar in artists
                 join al in albums
                     on ar.ArtistID equals al.ArtistID into allArtistAlbums
                 let artistAlbumCount = allArtistAlbums.Count()
                 orderby artistAlbumCount descending
                 select ar).First();

             Console.WriteLine("Najmnogu albumi: " + artistWithMostAlbums.Name);

            // Koj artist ima najdolga pesna?
            var artistWithLongestSong =
                (from ar in artists
                join al in albums on ar.ArtistID equals al.ArtistID
                join so in songs on al.AlbumID equals so.AlbumID
                orderby so.Length.TotalSeconds descending
                select ar).First();

             Console.WriteLine("Najdolga pesna ima " + artistWithLongestSong.Name);

            // Koj artist ima najmnogu pesni?
            var artistWithMostSongs =
                (from ar in artists
                 join al in albums on ar.ArtistID equals al.ArtistID
                 join so in songs on al.AlbumID equals so.AlbumID into allSongsForAlbum
                 orderby allSongsForAlbum.Count() descending
                 select ar).First();

             Console.WriteLine("Najmnogu pesni ima: " + artistWithMostSongs.Name);

            // Koj artist ima album so najmnogu pesni?
            var albumWithMostSongs =
                (from ar in artists
                 join al in albums on ar.ArtistID equals al.ArtistID
                 join so in songs on al.AlbumID equals so.AlbumID into allSongsForAlbum
                 orderby allSongsForAlbum.Count() descending
                 select ar).First();

             Console.WriteLine("Artist so album so najmnogu pesni: " + albumWithMostSongs.Name);

            // Koj artist ima najdolgo vkupno vremetraenje na pesnite?
            var artistWithBiggestTotalSongLength =
                (from ar in artists
                 join al in albums on ar.ArtistID equals al.ArtistID
                 join so in songs on al.AlbumID equals so.AlbumID into allSongsForAlbum
                 let totalSongsLength = allSongsForAlbum.Sum(s => s.Length.TotalSeconds)
                 orderby totalSongsLength descending
                 select ar).First();

            Console.WriteLine("Najdolgo vkupno vremetraenje na pesni: " + artistWithBiggestTotalSongLength.Name);

            // Vo koja godina ima izdadeno najmnogu albumi?
            var yearWithMostAlbumsReleased =
                (from al in albums
                 group al by al.YearPublished into albumsByYearPublished
                 orderby albumsByYearPublished.Count() descending
                 select albumsByYearPublished.Key).First();

            Console.WriteLine("Godina so najveke albumi: " + yearWithMostAlbumsReleased);

            // Dali ima artist koj izdal poveke od eden album vo ista godina?
            var anyArtistWithMoreThanOneAlbumInSameYear =
                 (from ar in artists
                 join al in albums on ar.ArtistID equals al.ArtistID into allAlbumsByArtist
                 let albumsByYear = allAlbumsByArtist.GroupBy(a => a.YearPublished)
                 where albumsByYear.Any(items => items.Count() > 1)
                 select ar).FirstOrDefault();

            Console.WriteLine(anyArtistWithMoreThanOneAlbumInSameYear == null ? "Nema artist poveke od eden album vo ista godina" : "Postoi artist so poveke od eden album vo ista godina");

            // Kolku e prosecnoto vremetraenje na pesna?
            var averageSongDuration = songs.Average(s => s.Length.TotalSeconds);
            Console.WriteLine("Prosecno vremetranje na pesna: " + new TimeSpan(0, 0, (int)averageSongDuration));

            // Kolku e prosecnoto vremetraenje na album?
            var averageAlbumDuration =
                from al in albums
                join so in songs on al.AlbumID equals so.AlbumID into totalSongsForAlbum
                let AverageSongsDuration = totalSongsForAlbum.Select(s => s.Length.TotalSeconds).Average()
                select new { albumName = al.Name, AverageSongsDuration };

            foreach (var album in averageAlbumDuration)
            {
                Console.Write("Album: " + album.albumName + ", ");
                Console.WriteLine("Prosecno vremetraenje: " + new TimeSpan(0, 0, (int)album.AverageSongsDuration));
            }
        }



        static List<Artist> CreateArtists()
        {
            return new List<Artist>
            {
                new Artist { ArtistID = 0, Name = "Gurgen" },
                new Artist {ArtistID = 1, Name = "Felix" },
                new Artist { ArtistID = 2, Name = "Oluwakanyinsola" },
            };
        }

        static List<Album> CreateAlbums()
        {
            return new List<Album>
            {
                new Album { AlbumID = 0, ArtistID = 0, Name = "Ambition Around Satellites", YearPublished = 2011 },
                new Album { AlbumID = 1, ArtistID = 0, Name = "The Foray To Assailings", YearPublished = 1992 },
                new Album { AlbumID = 2, ArtistID = 1, Name = "Son Sky Bane Of Muscle", YearPublished = 1959 },
                new Album { AlbumID = 3, ArtistID = 1, Name = "Mesa Jane 901", YearPublished = 2011 },
                new Album { AlbumID = 4, ArtistID = 2, Name = "Pyramid Around Valley", YearPublished = 2115 },
                new Album { AlbumID = 5, ArtistID = 2, Name = "Clamor Planets ", YearPublished = 2011 },
                new Album { AlbumID = 6, ArtistID = 3, Name = "The Continental Gentleman", YearPublished = 2009 },
                new Album { AlbumID = 7, ArtistID = 1, Name = "The Continental Gentleman", YearPublished = 2010 },
                new Album { AlbumID = 8, ArtistID = 1, Name = "The Continental Gentleman", YearPublished = 2015 },
                new Album { AlbumID = 9, ArtistID = 1, Name = "The Continental Gentleman", YearPublished = 2011 },
            };
        }

        static List<Song> CreateSongs()
        {
            return new List<Song>
            {
                new Song { SongID = 0, AlbumID = 0, Length = new TimeSpan(0, 3, 4), Name = "Creativity Is Sin" },
                new Song { SongID = 1, AlbumID = 2, Length = new TimeSpan(0, 2, 50), Name = "Automatic Persuasion" },
                new Song { SongID = 2, AlbumID = 2, Length = new TimeSpan(0, 3, 41), Name = "Don't Touch The Crusade" },
                new Song { SongID = 3, AlbumID = 2, Length = new TimeSpan(0, 3, 14), Name = "Whole Lotta Firecracker" },
                new Song { SongID = 4, AlbumID = 1, Length = new TimeSpan(0, 2, 11), Name = "Cuteness Peepshow" },
                new Song { SongID = 5, AlbumID = 2, Length = new TimeSpan(0, 2, 56), Name = "Toxic Soldier" },
                new Song { SongID = 6, AlbumID = 6, Length = new TimeSpan(0, 1, 4), Name = "Suburban Alley" },
                new Song { SongID = 7, AlbumID = 0, Length = new TimeSpan(0, 3, 5), Name = "I Demand Experience" },
                new Song { SongID = 8, AlbumID = 1, Length = new TimeSpan(0, 3, 22), Name = "100 Dollar Slap" },
                new Song { SongID = 9, AlbumID = 2, Length = new TimeSpan(0, 3, 14), Name = "Stories Of Pretence" },
                new Song { SongID = 10, AlbumID = 3, Length = new TimeSpan(0, 2, 11), Name = "No Excuse For Lust" },
                new Song { SongID = 11, AlbumID = 4, Length = new TimeSpan(0, 3, 31), Name = "Dying Strength" },
                new Song { SongID = 12, AlbumID = 5, Length = new TimeSpan(0, 3, 19), Name = "Wicked Car" },
                new Song { SongID = 13, AlbumID = 6, Length = new TimeSpan(0, 2, 9), Name = "Silent Sinner" },
                new Song { SongID = 14, AlbumID = 7, Length = new TimeSpan(0, 1, 41), Name = "Eternal Bitch" },
                new Song { SongID = 15, AlbumID = 8, Length = new TimeSpan(0, 4, 18), Name = "The Missing Look" },
                new Song { SongID = 16, AlbumID = 9, Length = new TimeSpan(0, 3, 22), Name = "100 Dollar Slap" },
                new Song { SongID = 17, AlbumID = 5, Length = new TimeSpan(0, 3, 14), Name = "Stories Of Pretence" },
                new Song { SongID = 18, AlbumID = 6, Length = new TimeSpan(0, 2, 11), Name = "No Excuse For Lust" },
                new Song { SongID = 19, AlbumID = 7, Length = new TimeSpan(0, 3, 31), Name = "Dying Strength" },
                new Song { SongID = 20, AlbumID = 8, Length = new TimeSpan(0, 3, 19), Name = "Wicked Car" },
                new Song { SongID = 21, AlbumID = 9, Length = new TimeSpan(0, 2, 9), Name = "Silent Sinner" },
                new Song { SongID = 22, AlbumID = 5, Length = new TimeSpan(0, 1, 41), Name = "Eternal Bitch" },
                new Song { SongID = 23, AlbumID = 6, Length = new TimeSpan(0, 4, 18), Name = "The Missing Look" },
            };
        }
    }
}
