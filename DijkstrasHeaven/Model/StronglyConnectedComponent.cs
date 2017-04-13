using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DijkstrasHeaven.Model
{
    public class StronglyConnectedComponent
    {
        public static bool Find(Graph refGraph)
        {
            //inicjalizacja zmiennych
            Graph graph = refGraph;
            Stack<Node> S = new Stack<Node>();
            int cn = 0;
            int[] C = new int[graph.Nodes.Count];

            //zerowanie tablicy indeksów grafów
            for (int i = 0; i < graph.Nodes.Count; i++)
                C[graph.Nodes[i].ID] = 0;

            //zaczynamy od każdego wierzchołka po kolei
            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                if (C[graph.Nodes[i].ID] > 0)
                    continue;

                cn++;
                S.Push(graph.Nodes[i]);
                C[graph.Nodes[i].ID] = cn;

                while (S.Count > 0)
                {
                    Node v = S.Pop();
                    //znalezienie sąsiadów
                    List<Node> neighbours = new List<Node>();
                    for (int j = 0; j < graph.Connections.Count; j++)
                    {
                        if (graph.Connections[j].Node1.ID == v.ID)
                        {
                            neighbours.Add(graph.Connections[j].Node2);
                        }
                        else if (graph.Connections[j].Node2.ID == v.ID)
                        {
                            neighbours.Add(graph.Connections[j].Node1);
                        }
                    }

                    //sprawdzanie czy sąsiedzi byli już odwiedzeni
                    for (int j = 0; j < neighbours.Count; j++)
                    {
                        if (C[neighbours[j].ID] > 0)
                            continue;

                        S.Push(neighbours[j]);
                        C[neighbours[j].ID] = cn;
                    }
                }

                //jeżeli graf spójny to koniec
                if (C.Count(x => x == cn) == graph.Nodes.Count)
                    return true;
            }

            return false;
        }
    }
}
