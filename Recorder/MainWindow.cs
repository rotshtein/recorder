using System;
using Gtk;
using NPlot;
using NPlot.Bitmap;
using NPlot.Web;
using NPlot.Windows;
//using System.Drawing;

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
		float[] x = new float[1000];
		float[] y = new float[1000];

		for (int ii = 0; ii < 1000; ii++) {
			x[ii] = ii;
			y[ii] = 2 * x [ii] - 1;
		}
	/*
		//throw new NotImplementedException ();
		NPlot.Bitmap.PlotSurface2npD npSurface = new NPlot.Bitmap.PlotSurface2D(1000,1000);
		NPlot.LinePlot npPlot1 = new NPlot.LinePlot();
		npSurface.Clear ();
		npSurface.Title = "Demo1";
		npSurface.BackColor = Color.White;
		npSurface.TitleFont = TitleFont;

		npPlot1.AbscissaData = x;
		npPlot1.DataSource = y;
		npPlot1.Color = Color.Blue;
		npPlot1.Label = "Timeseries 1";
		npSurface.Add (npPlot1, NPlot.PlotSurface2D.XAxisPosition.Bottom, NPlot.PlotSurface2D.YAxisPosition.Left);

		npSurface.XAxis1.Label = "Timestamp";
		npSurface.XAxis1.LabelFont = AxisFont;
		npSurface.XAxis1.TickTextFont = TickFont;
		npSurface.XAxis1.NumberFormat = "yyyy-MM-dd HH:mm";
		npSurface.XAxis1.TicksLabelAngle = 90;
		npSurface.XAxis1.TickTextNextToAxis = True;
		npSurface.XAxis1.FlipTicksLabel = True;
		npSurface.XAxis1.LabelOffset = 110;
		npSurface.XAxis1.LabelOffsetAbsolute = True;


			//'Prepare left Y axis (a Plot MUST first be assigned to the left Y axis):
		npSurface.YAxis1.Label = "Value";
		npSurface.YAxis1.LabelFont = AxisFont;
		npSurface.YAxis1.TickTextFont = TickFont;
		npSurface.YAxis1.NumberFormat = "{0:####0.0}";

		npSurface.Refresh ();

		*/
	}
}
