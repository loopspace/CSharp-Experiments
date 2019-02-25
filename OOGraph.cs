using System;
using System.Collections.Generic;
using GraphStructures;

namespace MainProgram
{
    class Program
    {
	static void Main(string[] args)
	{
	    Graph graph = new Graph();
	    
	    Node node;
	    string label;

	    Console.WriteLine("Enter the graph's nodes:");
	    label = Console.ReadLine();

	    while (label != "")
	    {
		node = new Node(label);
		graph.nodes.Add(node);
		label = Console.ReadLine();
	    }
	    Console.WriteLine("Enter the graph's edges:");
	    label = Console.ReadLine();
	    
	    string[] delimiters = { " ", "->", "-" };
	    string[] edgelabel;
	    Edge edge;
	    Node start;
	    Node end;
	    
	    while (label != "")
	    {
		edgelabel = label.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);

		start = graph.GetNodeByLabel(edgelabel[0]);
		end = graph.GetNodeByLabel(edgelabel[1]);

		if (start == null) {
		    start = new Node(edgelabel[0]);
		    graph.nodes.Add(start);
		}

		if (end == null) {
		    end = new Node(edgelabel[1]);
		    graph.nodes.Add(end);
		}

		edge = new Edge();
		edge.start = start;
		edge.end = end;
		start.edges.Add(edge);
		end.edges.Add(edge);

		if (edgelabel.Length > 2)
		{
		    edge.weight = int.Parse(edgelabel[2]);
		}
		
		graph.edges.Add(edge);
		label = Console.ReadLine();
	    }

	    
	    Console.WriteLine("Enter the starting node for the searches:");
	    label = Console.ReadLine();

	    start = graph.GetNodeByLabel(label);
	    List<Node> visited = graph.DepthFrom(start);
	    List<string> labels = new List<string>();

	    for (int i = 0; i < visited.Count; i++) {
		node = visited[i];
		labels.Add(node.contents);
	    }
	    Console.WriteLine("Depth first  : {0}", String.Join(", ", labels));

	    visited = graph.BreadthFrom(start);
	    labels = new List<string>();
	    for (int i = 0; i < visited.Count; i++) {
		node = visited[i];
		labels.Add(node.contents);
	    }
	    Console.WriteLine("Breadth first: {0}", String.Join(", ", labels));

	    Console.WriteLine("Enter the nodes for the shortest path:");
	    label = Console.ReadLine();
	    edgelabel = label.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);

	    start = graph.GetNodeByLabel(edgelabel[0]);
	    end = graph.GetNodeByLabel(edgelabel[1]);
	    labels = new List<string>();
	    int distance = 0;

	    List<Edge> path = graph.PathBetween(start, end);
	    for (int i = path.Count - 1; i >= 0; i--) {
		labels.Add(path[i].start.contents);
		distance += path[i].weight;
	    }
	    labels.Add(path[0].end.contents);
	    Console.WriteLine("Shortest path has length {0}: {1}", distance, String.Join(" -> ", labels));
	    
	}
    }
}

namespace GraphStructures
{
    
    public class Node
    {
	public List<Edge> edges = new List<Edge>();
	public bool marked { get; set; }
	public int weight { get; set; }
	public string contents { get; set; }
	public Edge path { get; set; }

	public Node (string c) {
	    contents = c;
	}

	public void VisitRecursively(ref List<Node> visited) {
	    marked = true;
	    visited.Add(this);
	    for (int i = 0; i < edges.Count; i++) {
		if (!edges[i].end.marked) {
		    edges[i].end.VisitRecursively(ref visited);
		}
	    }
	}

    }

    public class Edge
    {
	public Node start {get; set;}
	public Node end {get; set;}
	public bool marked {get; set;}
	public int weight {get; set;}
    }
    
    public class Graph
    {
	public List<Node> nodes = new List<Node>();
	public List<Edge> edges = new List<Edge>();
	
	public Node GetNodeByLabel (string label) {
	    foreach (Node node in nodes)
	    {
		if (node.contents == label) {
		    return node;
		}
	    }
	    return null;
	}

	public bool ContainsNodeByLabel (string label) {
	    foreach (Node node in nodes)
	    {
		if (node.contents == label) {
		    return true;
		}
	    }
	    return false;
	}
	
	public void ClearAll() {
	    foreach (Node node in nodes)
	    {
		node.marked = false;
		node.weight = int.MaxValue;
		node.path = null;
	    }
	}

	public List<Node> VisitAll() {
	    ClearAll();
	    List<Node> visits = new List<Node>();
	    nodes[0].VisitRecursively(ref visits);
	    ClearAll();
	    return visits;
	}

	public List<Node> DepthFrom(Node start) {
	    ClearAll();
	    List<Node> visits = new List<Node>();
	    Stack<Node> stack = new Stack<Node>();
	    stack.Push(start);
	    Node current;
	    
	    while (stack.Count > 0) {
		current = stack.Pop();

		if (!current.marked)
		{
		    visits.Add(current);
		    current.marked = true;
		    for (int i = current.edges.Count - 1; i >= 0; i--) {
			if (!current.edges[i].end.marked) {
			    stack.Push(current.edges[i].end);
			}
		    }
		}
	    };
	    ClearAll();
	    return visits;
	}

	public List<Node> BreadthFrom(Node start) {
	    ClearAll();
	    List<Node> visits = new List<Node>();
	    Queue<Node> queue = new Queue<Node>();
	    queue.Enqueue(start);
	    Node current;
	    
	    while (queue.Count > 0) {
		current = queue.Dequeue();
		if (!current.marked)
		{
		    visits.Add(current);
		    current.marked = true;
		    for (int i = 0; i < current.edges.Count; i++) {
			if (!current.edges[i].end.marked) {
			    queue.Enqueue(current.edges[i].end);
			}
		    }
		}
	    };
	    ClearAll();
	    return visits;
	}

	public Node FindNearest() {
	    int weight = int.MaxValue;
	    Node node = nodes[0];
	    for (int i = 0; i < nodes.Count; i++) {
		if (!nodes[i].marked && nodes[i].weight < weight)
		{
		    node = nodes[i];
		    weight = nodes[i].weight;
		}
	    }
	    return node;
	}

	public List<Edge> PathBetween(Node start, Node end) {
	    ClearAll();
	    List<Edge> path = new List<Edge>();
	    Node current;
	    start.weight = 0;

	    for (int k = 0; k < nodes.Count; k++) {
		current = FindNearest();
		current.marked = true;
		for (int i = 0; i < current.edges.Count; i++) {
		    if (current.weight + current.edges[i].weight < current.edges[i].end.weight) {
			current.edges[i].end.weight = current.weight + current.edges[i].weight;
			current.edges[i].end.path = current.edges[i];
		    }
		}
	    }
	    current = end;
	    while (current.path != null) {
		path.Add(current.path);
		current = current.path.start;
	    }
	    ClearAll();
	    return path;
	}
    }
}
