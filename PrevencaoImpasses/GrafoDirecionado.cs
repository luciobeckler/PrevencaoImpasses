using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrevencaoImpasses
{
    public class DirectedGraph<T>
    {
        private readonly Dictionary<T, List<T>> _adjacencyList;

        public DirectedGraph()
        {
            _adjacencyList = new Dictionary<T, List<T>>();
        }

        public void AddVertex(T vertex)
        {
            if (!_adjacencyList.ContainsKey(vertex))
            {
                _adjacencyList[vertex] = new List<T>();
            }
        }

        public void AddEdge(T from, T to)
        {
            if (!_adjacencyList.ContainsKey(from))
            {
                AddVertex(from);
            }
            if (!_adjacencyList.ContainsKey(to))
            {
                AddVertex(to);
            }
            _adjacencyList[from].Add(to);
        }

        public void Display()
        {
            foreach (var vertex in _adjacencyList)
            {
                Console.Write(vertex.Key + " -> ");
                Console.WriteLine(string.Join(", ", vertex.Value));
            }
        }

        public bool HasEdge(T from, T to)
        {
            return _adjacencyList.ContainsKey(from) && _adjacencyList[from].Contains(to);
        }
    }
}
