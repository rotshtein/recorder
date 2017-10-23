using System;
using NPlot;
using System.IO;
using Gdk;

namespace Recorder
{
	public partial class winSpectrum : Gtk.Window
	{
		public winSpectrum () :
		base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}

		public void ShowSpectrum()
		{
			LinePlot lp = new LinePlot ();
			PointPlot pp = new PointPlot ();
			float[] x = new float[1000];
			float[] y = new float[1000];

			for (int ii = 0; ii < 1000; ii++) {
				x[ii] = ii;
				y[ii] = 2 * x [ii] - 1;
			}

			//throw new NotImplementedException ();

			NPlot.Bitmap.PlotSurface2D npSurface = new NPlot.Bitmap.PlotSurface2D(1000,1000);
			//NPlot.Windows.PlotSurface2D npSurface = new NPlot.Windows.PlotSurface2D();
			lp.AbscissaData = x;
			lp.DataSource = y;
			lp.Color =  System.Drawing.Color.Green;
			npSurface.Add (lp);
			npSurface.XAxis1.Label = "X-Axis";
			npSurface.YAxis1.Label = "Y-Axis";
			npSurface.Title = "Demo1";
			npSurface.BackColor = System.Drawing.Color.White;

			npSurface.Refresh ();

			MemoryStream ms = new MemoryStream();
			try
			{
				npSurface.Bitmap.Save(ms,System.Drawing.Imaging.ImageFormat.Png);

				ms.Position = 0;
				Pixbuf p = new Gdk.Pixbuf (ms);

				image.Pixbuf = p;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}

