using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

        public void FindCycles()
        {
            var visited = new HashSet<T>();
            var stack = new HashSet<T>();
            var path = new List<T>();
            var printedCycles = new HashSet<string>();

            foreach (var vertex in _adjacencyList.Keys)
            {
                FindCyclesUtil(vertex, visited, stack, path, printedCycles);
            }
        }

        private bool FindCyclesUtil(T current, HashSet<T> visited, HashSet<T> stack, List<T> path, HashSet<string> printedCycles)
        {
            if (stack.Contains(current))
            {
                int index = path.IndexOf(current);
                if (index != -1)
                {
                    var cycle = path.Skip(index).ToList();
                    var processes = cycle.Where(x => x.ToString().StartsWith("P")).ToList();
                    var resources = cycle.Where(x => x.ToString().StartsWith("R")).ToList();
                    var formattedCycle = string.Join(" ", processes.Concat(resources));

                    if (!printedCycles.Contains(formattedCycle))
                    {
                        printedCycles.Add(formattedCycle);
                    }
                }
                return true;
            }

            if (visited.Contains(current))
            {
                return false;
            }

            visited.Add(current);
            stack.Add(current);
            path.Add(current);

            if (_adjacencyList.ContainsKey(current))
            {
                foreach (var neighbor in _adjacencyList[current])
                {
                    if (FindCyclesUtil(neighbor, visited, stack, path, printedCycles))
                    {
                        return true;
                    }
                }
            }

            stack.Remove(current);
            path.Remove(current);
            return false;
        }
    }
}
