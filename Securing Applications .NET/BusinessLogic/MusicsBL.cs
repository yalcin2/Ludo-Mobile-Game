using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess;

namespace BusinessLogic
{
    public class MusicsBL
    {
        public IQueryable<Music> GetMusics()
        {
            return new MusicsRepository(false).GetMusics();
        }

        public Music GetMusic(int id)
        {
            return new MusicsRepository(false).GetMusic(id);
        }

        public IQueryable<Music> GetMusicWithID(Guid id)
        {
            return new MusicsRepository(false).GetMusicWithID(id);
        }

        public void AddMusic(string name, string description, int genre, Guid user_id, string musicPath, string signature)
        {
            Music m = new Music();
            m.Name = name;
            m.Description = description;
            m.Genre_fk = genre;
            m.User_fk = user_id;
            m.Signature = signature;

            if (string.IsNullOrEmpty(musicPath) == false)
                m.MusicPath = musicPath;

            new MusicsRepository(true).AddMusic(m);
        }

        public void DeleteMusic(int id)
        {
            MusicsRepository ir = new MusicsRepository(true);

            var myMusic = ir.GetMusic(id);
            if (myMusic != null)
            {
                ir.DeleteMusic(myMusic);
            }
        }
    }
}
