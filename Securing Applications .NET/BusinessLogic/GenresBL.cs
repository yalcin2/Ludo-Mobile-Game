using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess;

namespace BusinessLogic
{
    public class GenresBL
    {
        public IQueryable<Genre> GetGenres()
        {
            return new GenresRepository().GetGenres();
        }
    }
}
