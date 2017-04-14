using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstrasHeaven.Model
{
    class Dijkstra
    {
        public static string ShortestPaths(Graph graph, Node selectedNode, out int[] d)
        {
            List<int> Q = new List<int>();

            d = new int[graph.Nodes.Count];
            int[] p = new int[graph.Nodes.Count];
            for (int i = 0; i < graph.Nodes.Count; ++i)
            {
                Q.Add(i);
                d[i] = Int32.MaxValue;
                p[i] = -1;
            }

            d[selectedNode.ID] = 0;

            while (Q.Count > 0)
            {
                //Szukam najmniejszej wartości w tablicy d
                int minValue = Int32.MaxValue, minIndex = Int32.MaxValue;
                for (int i = 0; i < d.Count(); ++i)
                    if (minValue > d[i] && Q.Count(x => x == i) == 1)
                    {
                        minValue = d[i];
                        minIndex = i;
                    }

                //Usuwam wierzcholek o najmniejszym d z Q
                Q.Remove(minIndex);

                //znalezienie sąsiadów
                List<Node> neighbours = new List<Node>();
                for (int j = 0; j < graph.Connections.Count; j++)
                {
                    if (graph.Connections[j].Node1.ID == minIndex)
                    {
                        neighbours.Add(graph.Connections[j].Node2);
                    }
                    else if (graph.Connections[j].Node2.ID == minIndex)
                    {
                        neighbours.Add(graph.Connections[j].Node1);
                    }
                }

                for (int j = 0; j < neighbours.Count; j++)
                {
                    //sprawdzenie czy wierzcholek jest w Q
                    if (Q.Count(x => x == neighbours[j].ID) == 0)
                        continue;

                    //obliczenie wagi krawedzi
                    int weight = graph.Connections.Single(x => (x.Node1.ID == minIndex && x.Node2.ID == neighbours[j].ID)
                    || (x.Node1.ID == neighbours[j].ID && x.Node2.ID == minIndex)).Weight;

                    if (d[neighbours[j].ID] > d[minIndex] + weight)
                    {
                        d[neighbours[j].ID] = d[minIndex] + weight;
                        p[neighbours[j].ID] = minIndex;
                    }
                }
            }

            string result = "";
            for (int i = 0; i < d.Count(); ++i)
            {
                if (i == selectedNode.ID)
                    continue;

                result += "Odległość do " + i + ": " + d[i] + ". Trasa: ";

                int previous = p[i];
                List<int> trace = new List<int>();
                while (previous != -1)
                {
                    trace.Add(previous);
                    previous = p[previous];
                }
                trace.Reverse();

                foreach (int item in trace)
                    result += item + "->";

                result += Environment.NewLine;
            }

            return result;
        }
        public static int [][] MatrixOfShortestsPaths(Graph graph)
        {
            int [] temp;
            int[][] final = new int[graph.Nodes.Count][];
            foreach (Node node in graph.Nodes)
            {
                temp = new int[graph.Nodes.Count];
                ShortestPaths(graph, node, out temp);
                final[node.ID] = temp;
            }
            return final;
        }
        public static Node MinCenterOfGraph(Graph graph)
        {
            int Count = graph.Nodes.Count;
            int[][] final = MatrixOfShortestsPaths(graph);
            //na początku zakładam że najkrótsza droga wynosi Count*10
            //czyli tyle ile najwięcej może wylosować algorytm
            int indexOfNode = 0;
            int ShortestPath = Count*10;
            for(int i = 0;i<Count;i++)
            {
                int temp=0;
                for(int j=0;j<Count;j++)
                {
                    temp += final[i][j];
                }
                if (temp < ShortestPath)
                {
                    indexOfNode = i;
                    ShortestPath = temp;
                }
            }
            foreach (Node node in graph.Nodes)
                if (node.ID == indexOfNode)
                    return node;
            return null;
        }
        public static Node MinMaxCenterOfGraph(Graph graph)
        {
            int Count = graph.Nodes.Count;
            int[][] final = MatrixOfShortestsPaths(graph);
            //na początku zakładam że najkrótsza droga wynosi 10
            //czyli tyle ile najwięcej może wylosować algorytm
            int indexOfNode = 0;
            int MinMaxPath = 10;
            for (int i = 0; i < Count; i++)
            {
                int temp = 0;
                for (int j = 0; j < Count; j++)
                {
                    if (temp < final[i][j])
                        temp= final[i][j];
                }
                if (temp < MinMaxPath)
                {
                    indexOfNode = i;
                    MinMaxPath = temp;
                }
            }
            foreach (Node node in graph.Nodes)
                if (node.ID == indexOfNode)
                    return node;
            return null;
        }
        public static string WriteMatrix(int [][] Matrix)
        {
            string finalString = "";
            foreach (int[] array in Matrix)
            {
                foreach (int number in array)
                    finalString = finalString + number + " ";
                finalString = finalString + "\n";
            }
            return finalString;
        }
    }
}