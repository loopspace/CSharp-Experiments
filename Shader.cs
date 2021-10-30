using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK.Input;

namespace OpenGLStart
{
	public class SimpleWindow : GameWindow
	{
		public SimpleWindow()
			: base(400, 300)
		{
		}

		protected override void OnUpdateFrame (FrameEventArgs e)
		{
			base.OnUpdateFrame (e);
		}

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			// draw something simple using OpenGL

			// ...

			GL.Begin(BeginMode.Triangles);

			// red apex
			GL.Color3(1.0, 0.0, 0.0);
			GL.Vertex2(-1.0, -1.0);

			// green apex
			GL.Color3(0.0, 1.0, 0.0);
			GL.Vertex2(1.0, -1.0);

			// blue apex
			GL.Color3(0.0, 0.0, 1.0);
			GL.Vertex2(0, 1.0);

			GL.End();
			this.SwapBuffers();

		}

		protected override void OnLoad (EventArgs e)
		{
		    //			GL.ClearColor(Color4.RoyalBlue);
		}

		protected override void OnResize (EventArgs e)
		{
			GL.Viewport(0, 0, Width, Height);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
			base.OnResize (e);

		}

		[STAThread]
		public static void Main(string[] args)
		{
			using (var SimpleWindow1 = new SimpleWindow())
			{
				SimpleWindow1.Run();
			}
		}
	}
}
