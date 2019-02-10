# Experiments in C-Sharp

The programs here are my experiments in learning C#.  They aren't
particularly aimed at programming something special, but rather are
aimed at learning a technique.

This README is for recording things that I've learnt.

Also part of my learning is my [cryptography project](https://github.com/loopspace/CSharp-Cryptography).

## Compiling with mono

I'm using both a Windows machine (with Visual Studio) and a Linux
machine (with mono).  I can never remember exactly which command is
used for compiling with mono.

For compiling a single `cs` file:

~~~
mono-csc Program.cs
~~~

For compiling a project:

~~~
xbuild Project.sln
~~~

The executables run fine by themselves without needing to be called
via `mono` (as some documentation suggests they might need to be).

## Types

One of the biggest things I'm having to get used to is types.  I'm not
used to having to declare types at the outset and not be able to
change my mind.  The `BinaryTrees` program illustrates one of my
difficulties with this.

I wanted to implement a class for graphs.  I thought this would be a
good example of learning what OO programming looked like in C# since
there was plenty of scope for inheritence.  My current system has the
following structure:

* A node class
* A graph class

A graph contains as part of its structure a list of its nodes.
Currently, edges are specified on the node level in that each node has
a list of other nodes that it is connected to (later, I may alter this
so that edges are their own type which would allow for constructions
like complementary graphs with the same set of vertices; for now, this
is enough).

A node has, as hinted above, a list of other nodes that it is
connected to (directed edges can be handled this way, but not weighted
ones; the current implementation can only handle one edge to a given
node).  It also has an attribute which is its *contents* or *label*.
The issue is that I want to be able to deal with nodes which can have
different types of contents, specifically (but not exclusively) `int`
and `string`.  It took a bit of playing around with the `<T>` type
specifier, but I think I've gotten this working.  In the end, it
appears that the obvious thing works -- but it did take a few
iterations to see what the "obvious thing" actually was:

> Every `Node` and `Graph` declaration is adorned with the `<T>` generic
type identifier.

I can then define a particular instance via:

~~~
Graph<string> strgraph = new Graph<string>();
Graph<int> intgraph = new Graph<int>();
~~~

