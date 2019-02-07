using System;
using System.Collections.Generic;
using GraphStructures;

namespace MainProgram
{
    class Program
    {
	static void Main(string[] args)
	{
	    Graph<string> graph = new Graph<string>();
	    string label;
	    Dictionary<string,Node<string>> nodes = new Dictionary<string,Node<string>>();
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

	    while (label != "")
	    {
		edge = label.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);
		if (!nodes.ContainsKey(edge[0])) {
		    nodes.Add(edge[0], graph.addNodeByLabel(edge[0]));
		}
		if (!nodes.ContainsKey(edge[1])) {
		    nodes.Add(edge[1], graph.addNodeByLabel(edge[1]));
		}

		graph.addEdge(nodes[edge[0]], nodes[edge[1]], label.Contains(">"));
		label = Console.ReadLine();
	    }

	    List<Node<string>> visited = graph.visitAll();
	    for (int i = 0; i < visited.Count; i++) {
		Console.WriteLine(visited[i].contents);
	    }
	}
    }
}

namespace GraphStructures
{
    
    public class Node<T>
    {
	private List<Node<T>> edges = new List<Node<T>>();
	public T contents { get; set; }
	public bool marked { get; set; }

	public Node(T c) {
	    contents = c;
	}
	
	public void addEdge(Node<T> node) {
	    if (!edges.Contains(node)) {
		edges.Add(node);
	    }
	}

	public void removeEdge(Node<T> node) {
	    edges.Remove(node);
	}

	public void visit() {
	    marked = true;
	}

	/*
	  Depth-first is recursive
	 */
	public void visitRecurse(ref List<Node<T>> visited) {
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

    public class Graph<T>
    {
	private List<Node<T>> nodes = new List<Node<T>>();
	
	public void addNode(Node<T> node) {
	    nodes.Add(node);
	}

	public void addEdge(Node<T> start, Node<T> end, bool directed) {
	    start.addEdge(end);
	    if (!directed) {
		end.addEdge(start);
	    }
	}

	public Node<T> addNodeByLabel(T label) {
	    Node<T> node = new Node<T>(label);
	    nodes.Add(node);
	    return node;
	}

	public void clearAll() {
	    nodes.ForEach(delegate(Node<T> node) {
		    node.clear();
		});
	}

	public List<Node<T>> visitAll() {
	    clearAll();
	    List<Node<T>> visits = new List<Node<T>>();
	    nodes[0].visitRecurse(ref visits);
	    clearAll();
	    return visits;
	}
    }

    public class Tree<T> : Graph<T>
    {
	
    }

    public class BinaryTree<T> : Tree<T>
    {
	
    }

}
