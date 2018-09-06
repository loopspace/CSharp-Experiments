using System;
using System.Collections.Generic;

class Nim
{
    static void Main()
    {
	List<int> Piles = new List<int>();
	string inputstr;
	int inputnum;
	int gstate = 0;
	int gtotal = 0;
	int gmove;

	Console.WriteLine("Enter the number of coins in each pile, with a blank line to finish.");

	do
	{
	    inputstr = Console.ReadLine();
	    if (inputstr != "")
	    {
		inputnum = int.Parse(inputstr);
		Piles.Add(inputnum);
		gstate ^= inputnum;
		gtotal += inputnum;
	    }
	}
	while ( inputstr != "");

	gmove = 2*clz(gstate) - gstate;
	
	Console.WriteLine("The game state is " + gstate);
	Console.WriteLine("The total number of tokens is " + gtotal);
	Console.WriteLine("The first move is to remove " + gmove + " tokens from a pile");
	
    }
    
    static int clz(int v) {
	int n=64,c=1;
	while (n != 0) {
	    n>>=1;
	    if (v>>n != 0)
	    {
		c<<=n;
		v>>=n;
	    }
	}
	return c;
    }
}

