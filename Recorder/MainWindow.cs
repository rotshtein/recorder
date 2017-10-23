using System;
using Gtk;
using NPlot;
using NPlot.Bitmap;
using NPlot.Web;
using NPlot.Windows;
//using System.Drawing;
using Gdk;
using System.Drawing;
using System.IO;

public partial class MainWindow: Gtk.Window
{
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();

		filechooserbutton1.Action = FileChooserAction.Save;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnCombobox1Changed (object sender, EventArgs e)
	{
		if (combobox1.ActiveText == "Center [MHz]") {
			combobox2.Active = 0;
		} else {

			combobox2.Active = 1;

		}
		//throw new NotImplementedException ();
	}

	protected void OnCombobox2Changed (object sender, EventArgs e)
	{

		if (combobox2.Active == 0) {
			combobox1.Active = 0;
		} else {

			combobox1.Active = 1;

		}
		//throw new NotImplementedException ();
	}

	protected void OnCombobox3Changed (object sender, EventArgs e)
	{
		if (combobox3.Active == 0) {
		//	entry3.Sensitive = false;
		} else {
		//	entry3.Sensitive = true;
		}
		//throw new NotImplementedException ();
	}

	protected void OnButton2Clicked (object sender, EventArgs e)
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
