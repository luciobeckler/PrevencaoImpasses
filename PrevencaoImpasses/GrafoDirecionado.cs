using System;
using System.Collections.Generic;

namespace PrevencaoImpasses
{
    public class GrafoDirecionado<T>
    {
        private readonly Dictionary<T, List<T>> _listaAdjacencia;

        public GrafoDirecionado()
        {
            _listaAdjacencia = new Dictionary<T, List<T>>();
        }

        public void AdicionarVertice(T vertice)
        {
            if (!_listaAdjacencia.ContainsKey(vertice))
            {
                _listaAdjacencia[vertice] = new List<T>();
            }
        }

        public void AdicionarAresta(T de, T para)
        {
            if (!_listaAdjacencia.ContainsKey(de))
            {
                AdicionarVertice(de);
            }
            if (!_listaAdjacencia.ContainsKey(para))
            {
                AdicionarVertice(para);
            }
            _listaAdjacencia[de].Add(para);
        }

        public void Exibir()
        {
            foreach (var vertice in _listaAdjacencia)
            {
                Console.Write(vertice.Key + " -> ");
                Console.WriteLine(string.Join(", ", vertice.Value));
            }
        }

        public List<string> EncontraCiclos()
        {
            var visitados = new HashSet<T>();
            var pilha = new HashSet<T>();
            var ciclos = new List<string>();

            foreach (var vertice in _listaAdjacencia.Keys)
            {
                var caminho = new List<T>();
                if (EncontraCiclosUtil(vertice, visitados, pilha, caminho, ciclos))
                {
                    continue;
                }
            }

            return ciclos;
        }

        private bool EncontraCiclosUtil(T vertice, HashSet<T> visitados, HashSet<T> pilha, List<T> caminho, List<string> ciclos)
        {
            if (pilha.Contains(vertice))
            {
                int index = caminho.IndexOf(vertice);
                if (index != -1)
                {
                    var ciclo = caminho.GetRange(index, caminho.Count - index);
                    ciclos.Add(string.Join(" ", ciclo));
                }
                return true;
            }

            if (visitados.Contains(vertice))
            {
                return false;
            }

            visitados.Add(vertice);
            pilha.Add(vertice);
            caminho.Add(vertice);

            if (_listaAdjacencia.ContainsKey(vertice))
            {
                foreach (var vizinho in _listaAdjacencia[vertice])
                {
                    if (EncontraCiclosUtil(vizinho, visitados, pilha, caminho, ciclos))
                    {
                        return true;
                    }
                }
            }

            pilha.Remove(vertice);
            caminho.RemoveAt(caminho.Count - 1);

            return false;
        }

        public bool TemAresta(T de, T para)
        {
            return _listaAdjacencia.ContainsKey(de) && _listaAdjacencia[de].Contains(para);
        }
    }
}
