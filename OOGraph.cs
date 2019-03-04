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
	    int distance;
	    List<Edge> path;
	    Node current;
	    string[] delimiters = { " ", "->", "-" };
	    string[] edgelabel;
	    List<Node> visited;
	    List<string> labels = new List<string>();

	    Console.WriteLine("Enter the graph's nodes:");
	    label = Console.ReadLine();

	    while (label != "")
	    {
		graph.AddNodeByLabel(label);
		label = Console.ReadLine();
	    }
	    Console.WriteLine("Enter the graph's edges:");
	    label = Console.ReadLine();
	    
	    while (label != "")
	    {
		edgelabel = label.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);

		graph.AddEdgeBetween(edgelabel[0], edgelabel[1], int.Parse(edgelabel[2]));
		
		label = Console.ReadLine();
	    }
	    
	    Console.WriteLine("Enter the starting node for the searches:");
	    label = Console.ReadLine();

	    visited = graph.DepthFrom(label);
	    labels = new List<string>();

	    for (int i = 0; i < visited.Count; i++) {
		node = visited[i];
		labels.Add(node.Name);
	    }
	    Console.WriteLine("Depth first  : {0}", String.Join(", ", labels));

	    visited = graph.BreadthFrom(label);
	    labels = new List<string>();

	    for (int i = 0; i < visited.Count; i++) {
		node = visited[i];
		labels.Add(node.Name);
	    }
	    Console.WriteLine("Breadth first: {0}", String.Join(", ", labels));

	    Console.WriteLine("Enter the nodes for the shortest path:");
	    label = Console.ReadLine();
	    edgelabel = label.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);

	    labels = new List<string>();
	    distance = 0;
	    path = graph.PathBetween(edgelabel[0], edgelabel[1]);
	    current = graph.GetNodeByLabel(edgelabel[0]);
	    labels.Add(current.Name);
	    for (int i = path.Count - 1; i >= 0; i--) {
		labels.Add(path[i].GetOtherEnd(current).Name);
		current = path[i].GetOtherEnd(current);
		distance += path[i].Length;
	    }

	    Console.WriteLine("Shortest path has length {0}: {1}", distance, String.Join(" -> ", labels));
	    
	}
    }
}

namespace GraphStructures
{
    
    public class Node: IEnumerable<Edge>
    {
	private List<Edge> Edges = new List<Edge>();
	public bool Visited { get; set; }
	public int Weight { get; private set; }
	public string Name { get; private set; }
	public Edge Path { get; private set; }

	public Node (string c) {
	    Name = c;
	}

	public void Reset() {
	    Visited = false;
	    Weight = int.MaxValue;
	    Path = null;
	}

	public void AddEdge(Edge e)
	{
	    if (! Edges.Contains(e))
	    {
		Edges.Add(e);
	    }
	}

	public void UpdateWeight(Edge e)
	{
	    Node other = e.GetOtherEnd(this);
	    if (other.Weight + e.Length < Weight)
	    {
		Weight = other.Weight + e.Length;
		Path = e;
	    }
	}

	public void ZeroWeight() {
	    Weight = 0;
	}

	public void VisitRecursively(ref List<Node> v) {
	    Node node;
	    Visited = true;
	    v.Add(this);
	    for (int i = 0; i < Edges.Count; i++) {
		node = Edges[i].GetOtherEnd(this);
		if (!node.Visited) {
		    node.VisitRecursively(ref v);
		}
	    }
	}

	public IEnumerator<Edge> GetEnumerator()
	{
	    for (int i = 0; i < Edges.Count; i++)
	    {
		yield return Edges[i];
	    }
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
	    return this.GetEnumerator();
	}

	IEnumerator<Edge> IEnumerable<Edge>.GetEnumerator()
	{
	    return GetEnumerator();
	}

	public IEnumerable<Edge> Forward
	{
	    get {
		return this;
	    }
	}

	public IEnumerable<Edge> Reverse
	{
	    get {
		for (int i = Edges.Count - 1; i >= 0; i--)
		{
		    yield return Edges[i];
		}
	    }
	}

    }

    public class Edge
    {
	private Node[] Ends = new Node[2];
	public int Length {get; private set;}

	public Edge (Node s, Node e, int l)
	{
	    Ends[0] = s;
	    Ends[1] = e;
	    Length = l;
	    s.AddEdge(this);
	    e.AddEdge(this);
	}

	public Node GetOtherEnd(Node s)
	{
	    if (Ends[0] == s)
	    {
		return Ends[1];
	    }
	    else if (Ends[1] == s)
	    {
		return Ends[0];
	    }
	    else
	    {
		return null;
	    }
	}
    }
    
    public class Graph
    {
	private Dictionary<string,Node> nodes = new Dictionary<string,Node>();
	private List<Edge> edges = new List<Edge>();

	public bool AddNode(Node node)
	{
	    string label = node.Name;
	    if (!nodes.ContainsKey(label))
	    {
		nodes.Add(label,node);
		return true;
	    }
	    return false;
	}
	
	public Node AddNodeByLabel(string label) {
	    if (nodes.ContainsKey(label))
	    {
		return null;
	    }
	    else
	    {
		Node node = new Node(label);
		nodes.Add(label,node);
		return node;
	    }
	}
	
	public Node GetNodeByLabel (string label) {
	    if (nodes.ContainsKey(label))
	    {
		return nodes[label];
	    }
	    else
	    {
		return null;
	    }
	}

	public bool ContainsNodeByLabel (string label) {
	    return nodes.ContainsKey(label);
	}

	public Edge AddEdgeBetween (string s, string e, int l)
	{
	    Node sn;
	    Node en;
	    if (nodes.ContainsKey(s))
	    {
		sn = GetNodeByLabel(s);
	    }
	    else
	    {
		sn = new Node(s);
	    }
	    if (nodes.ContainsKey(e))
	    {
		en = GetNodeByLabel(e);
	    }
	    else
	    {
		en = new Node(e);
	    }

	    Edge edge = new Edge(sn,en,l);
	    edges.Add(edge);
	    return edge;
	}
	
	public void ResetAll() {
	    foreach (KeyValuePair<string,Node> entry in nodes)
	    {
		entry.Value.Reset();
	    }
	}

	public List<Node> VisitAllFrom(string s) {
	    Node start = GetNodeByLabel(s);
	    ResetAll();
	    List<Node> visits = new List<Node>();
	    start.VisitRecursively(ref visits);
	    ResetAll();
	    return visits;
	}

	public List<Node> DepthFrom(string s) {
	    Node start = GetNodeByLabel(s);
	    ResetAll();
	    List<Node> visits = new List<Node>();
	    Stack<Node> stack = new Stack<Node>();
	    stack.Push(start);
	    Node current;
	    
	    while (stack.Count > 0) {
		current = stack.Pop();

		if (!current.Visited)
		{
		    visits.Add(current);
		    current.Visited = true;
		    foreach (Edge e in current.Reverse)
		    {
			if (!e.GetOtherEnd(current).Visited) {
			    stack.Push(e.GetOtherEnd(current));
			}
		    }
		}
	    };
	    ResetAll();
	    return visits;
	}

	public List<Node> BreadthFrom(string s) {
	    Node start = GetNodeByLabel(s);
	    
	    ResetAll();
	    List<Node> visits = new List<Node>();
	    Queue<Node> queue = new Queue<Node>();
	    queue.Enqueue(start);
	    Node current;
	    
	    while (queue.Count > 0) {
		current = queue.Dequeue();
		if (!current.Visited)
		{
		    visits.Add(current);
		    current.Visited = true;
		    foreach (Edge e in current.Forward)
		    {
			if (!e.GetOtherEnd(current).Visited) {
			    queue.Enqueue(e.GetOtherEnd(current));
			}
		    }
		}
	    }
	    ResetAll();
	    return visits;
	}

	public Node FindNearest() {
	    int weight = int.MaxValue;
	    Node node = null;
	    foreach (KeyValuePair<string,Node> entry in nodes) {
		if (!entry.Value.Visited && entry.Value.Weight < weight)
		{
		    node = entry.Value;
		    weight = entry.Value.Weight;
		}
	    }
	    return node;
	}

	public List<Edge> PathBetween(string s, string e) {
	    Node start = GetNodeByLabel(s);
	    Node end = GetNodeByLabel(e);
	    
	    ResetAll();
	    List<Edge> path = new List<Edge>();
	    Node current;
	    Node other;
	    start.ZeroWeight();
	    current = FindNearest();

	    while (current != null)
	    {
		current.Visited = true;
		foreach (Edge edge in current) {
		    other = edge.GetOtherEnd(current);
		    other.UpdateWeight(edge);
		}

		current = FindNearest();
	    }
	    current = end;
	    while (current.Path != null) {
		path.Add(current.Path);
		current = current.Path.GetOtherEnd(current);
	    }
	    ResetAll();

	    return path;
	}
    }
}
