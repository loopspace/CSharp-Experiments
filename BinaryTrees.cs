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
	    
	    INode<string> node;
	    string label;

	    Console.WriteLine("Enter the graph's nodes:");
	    label = Console.ReadLine();

	    while (label != "")
	    {
		node = new Node<string>(label);
		graph.AddNode(node);
		label = Console.ReadLine();
	    }
	    Console.WriteLine("Enter the graph's edges:");
	    label = Console.ReadLine();
	    
	    string[] delimiters = { " ", "->", "-" };
	    string[] edge;
	    INode<string> start;
	    INode<string> end;
	    
	    while (label != "")
	    {
		edge = label.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);

		start = graph.GetNodeByLabel(edge[0]);
		end = graph.GetNodeByLabel(edge[1]);

		if (start == null) {
		    start = new Node<string>(edge[0]);
		    graph.AddNode(start);
		}

		if (end == null) {
		    end = new Node<string>(edge[1]);
		    graph.AddNode(end);
		}

		graph.AddEdge(start, end, label.Contains(">"));
		label = Console.ReadLine();
	    }

	    if (graph.IsTree()) {
		Console.WriteLine("Your graph is a tree");
	    }
	    else
	    {
		Console.WriteLine("Your graph is not a tree");
	    }

	    if (graph.IsConnected()) {
		Console.WriteLine("Your graph is connected");
	    }
	    else
	    {
		Console.WriteLine("Your graph has {0} components", graph.ComponentsCount());
	    }

	    
	    
	    Console.WriteLine("Enter the starting node for the searches:");
	    label = Console.ReadLine();

	    start = graph.GetNodeByLabel(label);
	    List<INode<string>> visited = graph.DepthFrom(start);
	    List<string> labels = new List<string>();

	    for (int i = 0; i < visited.Count; i++) {
		node = visited[i];
		labels.Add(node.GetLabel());
	    }
	    Console.WriteLine("Depth first  : {0}", String.Join(", ", labels));

	    visited = graph.BreadthFrom(start);
	    labels = new List<string>();
	    for (int i = 0; i < visited.Count; i++) {
		node = visited[i];
		labels.Add(node.GetLabel());
	    }
	    Console.WriteLine("Breadth first: {0}", String.Join(", ", labels));
	}
    }
}

namespace GraphStructures
{

    public interface INode<T>
    {
	void  AddEdge(INode<T> node);
	void  RemoveEdge(INode<T> node);
	INode<T> GetEdge(int i);
	int   CountEdges();
	
	void  VisitRecursively(ref List<INode<T>> visited);
	
	void  Mark();
	bool  IsMarked();
	void  Clear();

	T     GetLabel ();
//	int   CompareLabel (T label);
//	bool  LabelIs (T label);
    }
    
    public class Node<T> : INode<T>
    {
	private List<INode<T>> edges = new List<INode<T>>();
	private bool marked { get; set; }
	private T contents { get; set; }

	public Node (T c) {
	    contents = c;
	}
	
	public void AddEdge(INode<T> node) {
	    if (!edges.Contains(node)) {
		edges.Add(node);
	    }
	}

	public void RemoveEdge(INode<T> node) {
	    edges.Remove(node);
	}

	public INode<T> GetEdge (int i) {
	    return edges[i];
	}

	public int CountEdges() {
	    return edges.Count;
	}
	
	/*
	  Depth-first is recursive
	 */
	public void VisitRecursively(ref List<INode<T>> visited) {
	    marked = true;
	    visited.Add(this);
	    for (int i = 0; i < edges.Count; i++) {
		if (!edges[i].IsMarked()) {
		    edges[i].VisitRecursively(ref visited);
		}
	    }
	}

	public void Mark() {
	    marked = true;
	}

	public bool IsMarked() {
	    return marked;
	}
	
	public void Clear() {
	    marked = false;
	}

	public T GetLabel()
	{
	    return contents;
	}
    }

    public class Graph<T>
    {
	private List<INode<T>> nodes = new List<INode<T>>();
	private List<List<INode<T>>> components = new List<List<INode<T>>>();
	private bool tree = true;
	
	public void AddNode(INode<T> node) {
	    nodes.Add(node);
	    components.Add(new List<INode<T>> { node });
	}

	public void AddEdge(INode<T> start, INode<T> end, bool directed) {
	    int startComponent = -1;
	    int endComponent = -2;
	    for (int i = 0; i < components.Count; i++) {
		if (components[i].Contains(start)) {
		    startComponent = i;
		}
		if (components[i].Contains(end)) {
		    endComponent = i;
		}
	    }

	    if (startComponent == endComponent) {
		tree = false;
	    }
	    else
	    {

	   	for (int i = 0; i < components[endComponent].Count; i++) {
		    components[startComponent].Add(components[endComponent][i]);
	    	}
	    	components.Remove(components[endComponent]);
	    }
	    start.AddEdge(end);
	    if (!directed) {
		end.AddEdge(start);
	    }

	}

	public bool IsTree() {
	    return tree;
	}

	public bool IsConnected() {
	    return components.Count == 1;
	}

	public int ComponentsCount() {
	    return components.Count;
	}
	
	public INode<T> GetNodeByLabel (T label) {
	    foreach (INode<T> node in nodes)
	    {
		if (EqualityComparer<T>.Default.Equals(node.GetLabel() , label)) {
		    return node;
		}
	    }
	    return null;
	}

	public bool ContainsNodeByLabel (T label) {
	    foreach (INode<T> node in nodes)
	    {
		if (EqualityComparer<T>.Default.Equals(node.GetLabel() , label)) {
		    return true;
		}
	    }
	    return false;
	}
	
	public void ClearAll() {
	    foreach (INode<T> node in nodes)
	    {
		node.Clear();
	    }
	}

	public List<INode<T>> VisitAll() {
	    ClearAll();
	    List<INode<T>> visits = new List<INode<T>>();
	    nodes[0].VisitRecursively(ref visits);
	    ClearAll();
	    return visits;
	}

	public List<INode<T>> DepthFrom(INode<T> start) {
	    ClearAll();
	    List<INode<T>> visits = new List<INode<T>>();
	    Stack<INode<T>> stack = new Stack<INode<T>>();
	    stack.Push(start);
	    INode<T> current;
	    
	    while (stack.Count > 0) {
		current = stack.Pop();

		if (!current.IsMarked())
		{
		    visits.Add(current);
		    current.Mark();
		    for (int i = current.CountEdges() - 1; i >= 0; i--) {
			if (!current.GetEdge(i).IsMarked()) {
			    stack.Push(current.GetEdge(i));
			}
		    }
		}
		
	    };
	    ClearAll();
	    return visits;
	}

	public List<INode<T>> BreadthFrom(INode<T> start) {
	    ClearAll();
	    List<INode<T>> visits = new List<INode<T>>();
	    Queue<INode<T>> queue = new Queue<INode<T>>();
	    queue.Enqueue(start);
	    INode<T> current;
	    
	    while (queue.Count > 0) {
		current = queue.Dequeue();
		if (!current.IsMarked())
		{
		    visits.Add(current);
		    current.Mark();
		    for (int i = 0; i < current.CountEdges(); i++) {
			if (!current.GetEdge(i).IsMarked()) {
			    queue.Enqueue(current.GetEdge(i));
			}
		    }
		}
	    };
	    ClearAll();
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
