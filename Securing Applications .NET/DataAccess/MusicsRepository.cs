using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DataAccess
{
    public class MusicsRepository: ConnectionClass
    {
        public MusicsRepository(bool isAdmin):base(isAdmin)
        { }


        #region Select
        public Music GetMusic(int id)
        {
            return Entity.Musics.SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<Music> GetMusicWithID(Guid id)
        {
            return Entity.Musics.Where(x => x.User_fk == id);
        }

        public IQueryable<Music> GetMusics()
        {
            return Entity.Musics;

        }

        #endregion

        #region Insert

        public void AddMusic(Music m)
        {
            Entity.Musics.Add(m);
            Entity.SaveChanges();
        }

        #endregion

        #region Delete
        public void DeleteMusic(Music m)
        {
            Entity.Musics.Remove(m);
            Entity.SaveChanges();
        }

        #endregion





    }
}
