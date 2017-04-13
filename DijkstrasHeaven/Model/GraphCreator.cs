using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstrasHeaven.Model
{
    public static class GraphCreator
    {
        public static Graph CreateFromMatrix(int[,] MatrixInt)
        {
            int Dimension = MatrixInt.GetLength(0);
            Graph fromMatrix = new Graph();

            for (int i = 0; i < Dimension; i++)
                fromMatrix.Nodes.Add(new Node() { ID = i });
            for (int i = 0; i < Dimension; i++)
            {
                for (int j = i + 1; j < Dimension; j++)
                {
                    if (MatrixInt[i, j] == 1)
                    {
                        fromMatrix.Connections.Add(new Connection { Node1 = fromMatrix.Nodes[i], Node2 = fromMatrix.Nodes[j] });
                    }
                }
            }
            return fromMatrix;
        }

        public static Graph CreateFullGraph(int Nodes = 0)
        {
            Graph fullGraph = new Graph();

            //Dodanie wierzchołków
            for (int i = 0; i < Nodes; i++)
                fullGraph.Nodes.Add(new Node() { ID = i });

            //Dodanie połączeń między wierzchołkami
            for (int i = 0; i < Nodes; i++)
            {
                for (int j = i + 1; j < Nodes; j++)
                {
                    Connection connection = new Connection();
                    connection.Node1 = fullGraph.Nodes.FirstOrDefault(x => x.ID == i);
                    connection.Node2 = fullGraph.Nodes.FirstOrDefault(x => x.ID == j);
                    fullGraph.Connections.Add(connection);
                }
            }

            return fullGraph;
        }
        public static Graph createConsistentGraph(int Nodes)
        {
            
            while (true)
            {
                Random rand = new Random();
                int CounterOfConnections = 0;
                int n = nodes;
                double b = 0.5;
                //tworze losowy graf o 10 wierzchołkach i stopniu 0.5
                int[,] result = new int[n, n];
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        result[i, j] = 0;
                for (int i = 1; i < n; i++)
                    for (int j = 0; j < i; j++)
                        if (rand.NextDouble() < b)
                        {
                            result[i, j] = 1;
                            result[j, i] = 1;
                            CounterOfConnections++;
                        }
                //zmieniam graf aby był eulerowski

                for (int i = 0; i < n - 1; i++)
                {
                    int deg = 0;
                    for (int j = 0; j < n; j++)
                        if (result[i, j] > 0)
                            deg++;
                    //check if degree is even
                    if (deg % 2 != 0)
                    {
                        int x = rand.Next(n - i - 1) + i + 1;
                        if (result[i, x] > 0)
                        {
                            result[i, x] = 0;
                            result[x, i] = 0;
                            CounterOfConnections--;
                        }
                        else
                        {
                            result[i, x] = 1;
                            result[x, i] = 1;
                            CounterOfConnections++;
                        }
                    }
                }
                 if (CounterOfConnections != 0)
                {
                      return CreateFromMatrix(result);
                }
            }     
        }
    }
}
