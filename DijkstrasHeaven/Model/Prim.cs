using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstrasHeaven.Model
{
    class Prim
    {

        public static bool Algorithm(Graph myGraphToCheck)
        {
            // MST - minimalne drzewo rozpinajace 

            bool isExistMST = false; // zmienna przechowujaca, czy znaleziono MST
            List<Connection> importantBranches = new List<Connection>(); //lista krawedzi rozpinajacych MST

            if (myGraphToCheck.Nodes.Count > 0 && isEveryNodeConnected(myGraphToCheck)) // sprawdzam rowniez, czy istnieja wierzcholki-odludki
            {
                List<int> visited = new List<int>(); //lista wierzcholkow dodanych do MST
                List<Connection> neighbours = new List<Connection>(); //lista sasiadujacych krawedzi, dostepnych z wierzcholkow w liscie visited


                Random rnd = new Random();
                int v = rnd.Next(0, myGraphToCheck.Nodes.Count - 1);  //losuje wierzcholek od ktorego zaczne
                visited.Add(v); //dodaje go do wierzcholkow drzewa

                while (visited.Count < myGraphToCheck.Nodes.Count)  // dopoki MST nie zawiera wszystkich wierzcholkow:
                {

                    for (int i = 0; i < myGraphToCheck.Connections.Count; i++)  // szukam osiagalnych krawedzi z dodanych wierzcholkow
                    {
                        if (visited.Contains(myGraphToCheck.Connections[i].Node1.ID) && !visited.Contains(myGraphToCheck.Connections[i].Node2.ID))  //opcja 1: gdy nasze aktualne MST zawiera wierzcholek Node1, ale nie zawiera Node2
                        {
                            neighbours.Add(myGraphToCheck.Connections[i]);
                            continue;
                        }
                        else if (visited.Contains(myGraphToCheck.Connections[i].Node2.ID) && !visited.Contains(myGraphToCheck.Connections[i].Node1.ID)) //opcja 2: gdy nasze aktualne MST zawiera wierzcholek Node2, ale nie zawiera Node1
                        {
                            neighbours.Add(myGraphToCheck.Connections[i]);
                            continue;
                        }
                    }
                    if (neighbours.Count > 0)
                    {

                        var sortedList = neighbours.OrderBy(q => q.Weight).ToList(); // sortuje liste sasiadujacych krawedzi po wadze od najmniejszej do najwiekszej

                        if (visited.Contains(sortedList[0].Node1.ID))
                        {
                            visited.Add(sortedList[0].Node2.ID); // dodaje nieodwiedzony wierzcholek
                        }
                        else if (visited.Contains(sortedList[0].Node2.ID))
                        {
                            visited.Add(sortedList[0].Node1.ID); // dodaje nieodwiedzony wierzcholek
                        }
                        if (visited.Count == myGraphToCheck.Nodes.Count)
                        {
                            isExistMST = true; // znaleziono MST
                        }

                        importantBranches.Add(sortedList[0]);

                    }

                    neighbours.Clear();

                }
            }

            if (isExistMST)
            {
                myGraphToCheck.Connections.Clear();
                for (int i = 0; i < importantBranches.Count; ++i)
                {
                    myGraphToCheck.AddConnection(importantBranches[i]);
                }
            }

            return isExistMST;
        }

        public static bool isEveryNodeConnected(Graph myGraphToCheck)  //sprawdzam, czy wszystkie wierzcholki sa polaczone z grafem (czy nie sa odludkami)
        {
            List<int> connectedNodes = new List<int>(); // lista polaczonych wierzcholkow
            for (int i = 0; i < myGraphToCheck.Connections.Count; ++i)
            {
                if (!connectedNodes.Contains(myGraphToCheck.Connections[i].Node1.ID))
                {
                    connectedNodes.Add(myGraphToCheck.Connections[i].Node1.ID);
                }
                if (!connectedNodes.Contains(myGraphToCheck.Connections[i].Node2.ID))
                {
                    connectedNodes.Add(myGraphToCheck.Connections[i].Node2.ID);
                }
            }

            if (connectedNodes.Count == myGraphToCheck.Nodes.Count)
                return true;
            return false;
        }

    }
}
