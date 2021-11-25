using System.Collections.Generic;

namespace TradeStops.Common.DataStructures
{
    public class TrieNode<T>
    {
        private List<T> cachedKeys;

        private List<T> entityKey;

        public List<T> EntityKey => entityKey;

        public Dictionary<char, TrieNode<T>> Childs { get; private set;  } // todo for Nikita: remove {get set} and add comment with explanation

        public void AddChild(string word, int position, int length, T entity)
        {
            if (position >= length)
            {
                if (this.entityKey == null)
                {
                    this.entityKey = new List<T>(1);
                }

                this.entityKey.Add(entity);

                return;
            }

            var key = word[position];

            if (this.Childs == null)
            {
                this.Childs = new Dictionary<char, TrieNode<T>>();
            }

            TrieNode<T> childNode;

            if (!this.Childs.TryGetValue(key, out childNode))
            {
                childNode = new TrieNode<T>();
                this.Childs[key] = childNode;
            }

            childNode.AddChild(word, ++position, length, entity);
        }

        public void ValuesDeep(List<T> results)
        {
            if (entityKey != null)
            {
                results.AddRange(entityKey);
            }

            if (this.Childs == null)
            {
                return;
            }

            foreach (var child in this.Childs)
            {
                child.Value.ValuesDeep(results);
            }
        }

        public void ValuesDeepCached(List<T> results)
        {
            if (cachedKeys != null)
            {
                results.AddRange(cachedKeys);

                return;
            }

            if (this.entityKey != null)
            {
                results.AddRange(this.entityKey);
            }

            if (this.Childs == null)
            {
                return;
            }

            foreach (var child in this.Childs)
            {
                // it's possible to call here ValuesDeepCached for even more fast searches,
                // but in that case all child nodes will use their own cache too = more memory consumation.
                child.Value.ValuesDeep(results);
            }

            this.cachedKeys = new List<T>(results);
        }
    }
}