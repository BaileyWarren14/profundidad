using System;
using System.Collections.Generic;

public class Node
{
    public string State { get; set; }
    public List<Node> Neighbors { get; set; }

    public Node(string state)
    {
        State = state;
        Neighbors = new List<Node>();
    }

    public void AddNeighbor(Node neighbor)
    {
        Neighbors.Add(neighbor);
    }
}

public class Graph
{
    public List<Node> Nodes { get; set; }

    public Graph()
    {
        Nodes = new List<Node>();
    }

    public void AddNode(Node node)
    {
        Nodes.Add(node);
    }

    public void AddEdge(Node from, Node to)
    {
        from.AddNeighbor(to);
    }

    public void DisplayGraph()
    {
        foreach (var node in Nodes)
        {
            Console.Write($"Nodo {node.State} está conectado a: ");
            foreach (var neighbor in node.Neighbors)
            {
                Console.Write($"{neighbor.State} ");
            }
            Console.WriteLine();
        }
    }
}

public class DFS
{
    public static List<Node> DepthFirstSearch(Node start, Node goal)
    {
        var stack = new Stack<Node>();
        var visited = new HashSet<Node>();
        var path = new Dictionary<Node, Node>();

        stack.Push(start);
        visited.Add(start);
        path[start] = null;

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            if (current == goal)
            {
                return ReconstructPath(path, goal);
            }

            foreach (var neighbor in current.Neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    stack.Push(neighbor);
                    path[neighbor] = current;
                }
            }
        }

        return null; // No se encontró una ruta
    }

    private static List<Node> ReconstructPath(Dictionary<Node, Node> path, Node goal)
    {
        var result = new List<Node>();
        var current = goal;

        while (current != null)
        {
            result.Add(current);
            current = path[current];
        }

        result.Reverse();
        return result;
    }
}

public class Program
{
    public static void Main()
    {
        var graph = new Graph();
        var nodes = new Dictionary<string, Node>();

        Console.WriteLine("Bienvenido al programa de búsqueda en profundidad (DFS).");

        // Agregar nodos
        while (true)
        {
            Console.Write("Ingrese el nombre de un nodo (o escriba 'fin' para terminar): ");
            string nodeName = Console.ReadLine();

            if (nodeName.ToLower() == "fin")
                break;

            if (!nodes.ContainsKey(nodeName))
            {
                var newNode = new Node(nodeName);
                nodes[nodeName] = newNode;
                graph.AddNode(newNode);
                Console.WriteLine($"Nodo '{nodeName}' agregado.");
            }
            else
            {
                Console.WriteLine($"El nodo '{nodeName}' ya existe.");
            }
        }

        // Agregar conexiones
        while (true)
        {
            Console.Write("Ingrese la conexión (formato: 'A B' para conectar A -> B, o escriba 'fin' para terminar): ");
            string input = Console.ReadLine();

            if (input.ToLower() == "fin")
                break;

            string[] parts = input.Split(' ');
            if (parts.Length == 2 && nodes.ContainsKey(parts[0]) && nodes.ContainsKey(parts[1]))
            {
                graph.AddEdge(nodes[parts[0]], nodes[parts[1]]);
                Console.WriteLine($"Conexión '{parts[0]} -> {parts[1]}' agregada.");
            }
            else
            {
                Console.WriteLine("Entrada inválida. Asegúrese de que ambos nodos existan.");
            }
        }

        // Mostrar el grafo
        Console.WriteLine("\nGrafo actual:");
        graph.DisplayGraph();

        // Seleccionar nodo de inicio y objetivo
        Console.Write("\nIngrese el nodo de inicio: ");
        string startNodeName = Console.ReadLine();
        Console.Write("Ingrese el nodo objetivo: ");
        string goalNodeName = Console.ReadLine();

        if (nodes.ContainsKey(startNodeName) && nodes.ContainsKey(goalNodeName))
        {
            var startNode = nodes[startNodeName];
            var goalNode = nodes[goalNodeName];

            // Ejecutar DFS
            var path = DFS.DepthFirstSearch(startNode, goalNode);

            if (path != null)
            {
                Console.WriteLine("\nRuta encontrada:");
                foreach (var node in path)
                {
                    Console.Write($"{node.State} ");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("\nNo se encontró una ruta entre los nodos especificados.");
            }
        }
        else
        {
            Console.WriteLine("Uno o ambos nodos no existen en el grafo.");
        }
    }
}