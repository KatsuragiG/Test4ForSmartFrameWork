using System.Collections.Generic;

namespace TradeStops.Common.DataStructures
{
    public interface ITrie<T>
    {
        void Add(string word, T entity);
        List<T> Search(string word);

        List<T> SearchExact(string word);
    }
}
