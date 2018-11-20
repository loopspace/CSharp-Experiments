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
	    string label;
	    Dictionary<string,Node> nodes = new Dictionary<string,Node>();
	    Console.WriteLine("Enter the graph's nodes:");
	    label = Console.ReadLine();
	    while (label != "")
	    {
		nodes.Add(label,graph.addNodeByLabel(label));
		label = Console.ReadLine();
	    }
	    Console.WriteLine("Enter the graph's edges:");
	    label = Console.ReadLine();
	    string[] delimiters = { " ", "->", "-" };
	    string[] edge;
	    bool directed;
	    while (label != "")
	    {
		edge = label.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);
		if (!nodes.ContainsKey(edge[0])) {
		    nodes.Add(edge[0], graph.addNodeByLabel(edge[0]));
		}
		if (!nodes.ContainsKey(edge[1])) {
		    nodes.Add(edge[1], graph.addNodeByLabel(edge[1]));
		}
		if (label.Contains(">")) {
		    directed = true;
		} else {
		    directed = false;
		}
		graph.addEdge(nodes[edge[0]], nodes[edge[1]], directed);
		label = Console.ReadLine();
	    }

	    List<Node> visited = graph.visitAll();
	    for (int i = 0; i < visited.Count; i++) {
		Console.WriteLine(visited[i].contents);
	    }
	}
    }
}

namespace GraphStructures
{

    public class Node
    {
	private List<Node> edges = new List<Node>();
	public string contents { get; set; }
	public bool marked { get; set; }
	
	public Node(string c) {
	    contents = c;
	}
	
	public void addEdge(Node node) {
	    if (!edges.Contains(node)) {
		edges.Add(node);
	    }
	}

	public void removeEdge(Node node) {
	    edges.Remove(node);
	}

	public void visit() {
	    marked = true;
	}

	public void visitRecurse(ref List<Node> visited) {
	    marked = true;
	    visited.Add(this);
	    for (int i = 0; i < edges.Count; i++) {
		if (!edges[i].marked) {
		    edges[i].visitRecurse(ref visited);
		}
	    }
	}

	public void clear() {
	    marked = false;
	}
    }
    
    public class Graph
    {
	private List<Node> nodes = new List<Node>();

	public void addNode(Node node) {
	    nodes.Add(node);
	}

	public void addEdge(Node start, Node end, bool directed) {
	    start.addEdge(end);
	    if (!directed) {
		end.addEdge(start);
	    }
	}

	public Node addNodeByLabel(string label) {
	    Node node = new Node(label);
	    nodes.Add(node);
	    return node;
	}

	public void clearAll() {
	    nodes.ForEach(delegate(Node node) {
		    node.clear();
		});
	}

	public List<Node> visitAll() {
	    clearAll();
	    List<Node> visits = new List<Node>();
	    Node node = nodes[0];
	    node.visitRecurse(ref visits);
	    clearAll();
	    return visits;
	}
    }

    public class Tree : Graph
    {
	
    }

    public class BinaryTree : Tree
    {
	
    }

}
