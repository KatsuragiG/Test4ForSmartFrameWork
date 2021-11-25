using System.Collections.Generic;

namespace TradeStops.Common.DataStructures
{
    public class Trie<T> : ITrie<T>
    {
        private TrieNode<T> root = new TrieNode<T>();

        public void Add(string word, T entity)
        {
            root.AddChild(word.ToUpper(), 0, word.Length, entity);
        }

        public List<T> Search(string word)
        {
            var keys = SearchTrie(root, word.ToUpper());

            return keys;
        }

        public List<T> SearchExact(string word)
        {
            var wordInUpper = word.ToUpper();

            var node = root;
            var length = wordInUpper.Length;

            for (int i = 0; i < length; i++)
            {
                if (node.Childs != null)
                {
                    if (!node.Childs.TryGetValue(wordInUpper[i], out node))
                    {
                        return null;
                    }
                }
            }

            return node.EntityKey;
        }

        private static List<T> SearchTrie(TrieNode<T> rootNode, string word)
        {
            var keys = new List<T>();
            var node = rootNode;
            var length = word.Length;

            for (int i = 0; i < length; i++)
            {
                if (node.Childs != null)
                {
                    if (!node.Childs.TryGetValue(word[i], out node))
                    {
                        return keys;
                    }
                }
                else
                {
                    return keys;
                }
            }

            if (length == 1)
            {
                // make cached search for fast performance on short query strings.
                node.ValuesDeepCached(keys);
            }
            else
            {
                node.ValuesDeep(keys);
            }

            return keys;
        }
    }
}