using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Search.Lucene
{
    public class LuceneBackend : ISearchBackend
    {
        //TODO Add config settings if necessary for initializing the objects
        //use the options pattern and DI
        public IIndex GetIndex()
        {
            //TODO Create and initialize an instance of LeceneIndex
            throw new NotImplementedException();
        }

        public ISearcher GetSearcher()
        {
            //TODO Create and initialize an instance of LeceneSeracher
            throw new NotImplementedException();
        }
    }
}
