using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

public partial class AnimForm : Form {
    private Timer timer = new Timer();
    private BufferedGraphics bufferedGraphics;

    private int size = 5;
    private int gridsize = 100;
    private bool[,] grid = new bool[100,100];
    private bool[,] newgrid = new bool[100,100];


    public AnimForm() {
	Width = (gridsize+10)*size;
	Height = (gridsize+10)*size;
        BufferedGraphicsContext context = BufferedGraphicsManager.Current;
        context.MaximumBuffer = new Size( this.Width + 1, this.Height + 1 );
        bufferedGraphics = context.Allocate( this.CreateGraphics(),
        new Rectangle( 0, 0, this.Width, this.Height) );
        timer.Enabled = true;
        timer.Tick += OnTimer;
        timer.Interval = 20; // 50 images per second.
        timer.Start();

	Random rnd = new Random();
	for (int i = 0; i < gridsize; i++) {
	    for (int j = 0; j < gridsize; j++) {
		if (rnd.Next(0,9) < 3) {
//		if (i + j < 30) {
		    grid[i,j] = true;
		}
		else
		{
		    grid[i,j] = false;
		}
	    }
	}
    }

    private void OnTimer( object sender, System.EventArgs e ) {

	int c;
	for (int i = 0; i < gridsize; i++) {
	    for (int j = 0; j < gridsize; j++) {
		newgrid[i,j] = grid[i,j];
	    }
	}
	    
	for (int i = 0; i < gridsize; i++) {
	    for (int j = 0; j < gridsize; j++) {
		c = 0;
		for (int k = -1; k < 2; k++) {
		    for (int l = -1; l < 2; l++) {
			if (k != 0 || l != 0) {
			    if (grid[ (i + k + gridsize)%gridsize, (j + l + gridsize)%gridsize]) {
				c++;
			    }
			}
		    }
		}
		if (grid[i,j])
		{ // Alive
		    if (c != 2 && c != 3)
		    { // Die
			newgrid[i,j] = false;
		    }
		}
		else
		{ // Dead
		    if (c == 3)
		    { // Born
			newgrid[i,j] = true;
		    }
		}
	    }
	}

	for (int i = 0; i < gridsize; i++) {
	    for (int j = 0; j < gridsize; j++) {
		grid[i,j] = newgrid[i,j];
	    }
	}
	
        Graphics g = bufferedGraphics.Graphics;
        g.Clear( Color.White );

	Pen p = new Pen(Color.Gray, 1);
	for (int i = 0; i <= gridsize; i++) {
	    g.DrawLine( p, size + i*size, size, size + i*size, size + gridsize*size);
	    g.DrawLine( p, size, size + i*size, size + gridsize*size, size + i*size);
	}
	for (int i = 0; i < gridsize; i++) {
	    for (int j = 0; j < gridsize; j++) {
		if (grid[i,j]) {
		    g.FillRectangle( Brushes.Black, size + i*size, size + j*size, size, size );
		}
	    }
	}
        bufferedGraphics.Render( Graphics.FromHwnd( this.Handle ) );
    }

    [System.STAThread]
    public static void Main() {
	Application.Run( new AnimForm() );
    }
}
