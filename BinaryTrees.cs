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
	    Graph<int> intgraph = new Graph<int>();
	    for (int i = 0; i < 10; i++) {
		intgraph.addNodeByLabel(i);
	    }
	    string label;
	    string firstNodeLabel;
	    Node<string> firstNode;
	    Dictionary<string,Node<string>> nodes = new Dictionary<string,Node<string>>();
	    Console.WriteLine("Enter the graph's nodes:");
	    label = Console.ReadLine();
	    firstNodeLabel = label;
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

	    firstNode = graph.GetNodeByLabel(firstNodeLabel);
	    List<Node<string>> visited = graph.depthFrom(firstNode);
	    List<string> labels = new List<string>();
	    for (int i = 0; i < visited.Count; i++) {
		labels.Add(visited[i].contents);
	    }
	    Console.WriteLine("Depth first: {0}", String.Join(", ", labels));

	    visited = graph.breadthFrom(firstNode);
	    labels = new List<string>();
	    for (int i = 0; i < visited.Count; i++) {
		labels.Add(visited[i].contents);
	    }
	    Console.WriteLine("Breadth first: {0}", String.Join(", ", labels));
	}
    }
}

namespace GraphStructures
{
    
    public class Node<T>
    {
	public List<Node<T>> edges = new List<Node<T>>();
	public T contents { get; set; }
	public bool marked { get; set; }

	public Node(T c) {
	    contents = c;
	}

	public bool compareLabel(T c) {
	    return EqualityComparer<T>.Default.Equals(this.contents,c);
	}
	
	public void addEdge(Node<T> node) {
	    if (!edges.Contains(node)) {
		edges.Add(node);
	    }
	}

	public void removeEdge(Node<T> node) {
	    edges.Remove(node);
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

	public Node<T> GetNodeByLabel(T label) {
	    foreach (Node<T> node in nodes)
	    {
		if (node.compareLabel(label)) {
		    return node;
		}
	    }
	    return null;
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

	public List<Node<T>> depthFrom(Node<T> start) {
	    clearAll();
	    List<Node<T>> visits = new List<Node<T>>();
	    Stack<Node<T>> stack = new Stack<Node<T>>();
	    stack.Push(start);
	    Node<T> current;
	    while (stack.Count > 0) {
		current = stack.Pop();
		if (!current.marked)
		{
		    visits.Add(current);
		    current.marked = true;
		    for (int i = current.edges.Count - 1; i >= 0; i--) {
			if (!current.edges[i].marked) {
			    stack.Push(current.edges[i]);
			}
		    }
		}
		
	    };
	    return visits;
	}

	public List<Node<T>> breadthFrom(Node<T> start) {
	    clearAll();
	    List<Node<T>> visits = new List<Node<T>>();
	    Queue<Node<T>> queue = new Queue<Node<T>>();
	    queue.Enqueue(start);
	    Node<T> current;
	    while (queue.Count > 0) {
		current = queue.Dequeue();
		if (!current.marked)
		{
		    visits.Add(current);
		    current.marked = true;
		    for (int i = 0; i < current.edges.Count; i++) {
			if (!current.edges[i].marked) {
			    queue.Enqueue(current.edges[i]);
			}
		    }
		}
	    };
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
