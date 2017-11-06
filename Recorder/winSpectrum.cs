using System;
using NPlot;
using System.IO;
using Gdk;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Configuration;
using NLog;
using Gtk;

namespace Recorder
{
	public partial class winSpectrum : Gtk.Window
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		string spectru_exe = null;
		public winSpectrum () :
		base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
			spectru_exe = ConfigurationManager.AppSettings["Spectrum"];
		}

		void OnFileSave(object sender, EventArgs args)
		{
		}

		protected void CloseWindow (object sender, EventArgs e)
		{
			this.Destroy ();
		}


		public string GetMessurment(double f0, double Rate, double Gain, string Filename="/home/x300/spectrum.dat")
		{
			if (!string.IsNullOrEmpty (spectru_exe) && File.Exists (spectru_exe)) 
			{
				Process p = new Process ();
				p.StartInfo.UseShellExecute = false;
				p.StartInfo.RedirectStandardOutput = true;
				p.StartInfo.RedirectStandardError = true;
				p.StartInfo.FileName = spectru_exe;
				p.StartInfo.Arguments = "--mode spec --freq " + f0.ToString() + " --rate " + Rate.ToString() + " --file " + Filename;
				try
				{
					if (p.Start ()) 
					{
						string output = p.StandardOutput.ReadToEnd ();
						string err = p.StandardError.ReadToEnd ();
						while (!p.HasExited) 
						{
							output = p.StandardOutput.ReadToEnd ();	
						}
						//p.WaitForExit (1000 * 60);
						string error = p.StandardError.ReadToEnd ();	
						return Filename;
					}
				}
				catch (Exception ex) 
				{
					logger.Error (ex, "Failed to start spectrum process");
				}
				return null;
			}
			return null;
		}

		public void ShowSpectrum(string Filename)
		{
			LinePlot lp = new LinePlot ();
			PointPlot pp = new PointPlot ();


			//throw new NotImplementedException ();

			NPlot.Bitmap.PlotSurface2D npSurface = new NPlot.Bitmap.PlotSurface2D(1000,1000);
			//NPlot.Windows.PlotSurface2D npSurface = new NPlot.Windows.PlotSurface2D();
			Tuple<float[], float[]> Data = ReadFile(Filename);
			if (Data == null)
				return;
			
			lp.AbscissaData = Data.Item1;
			lp.DataSource = Data.Item2;
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

		private Tuple<float[], float[]> ReadFile(string Filename)
		{
			try
			{
				;

				BinaryReader br = new BinaryReader (File.OpenRead (Filename));
				List<float> x = new List<float>();
				List<float> y = new List<float>();
				while (br.BaseStream.Position != br.BaseStream.Length) {
					x.Add ((float)br.ReadSingle());
					y.Add ((float)br.ReadSingle());
				}
				Tuple<float[], float[]> Data = new Tuple<float[], float[]>(x.ToArray (),y.ToArray ());
				return Data;
			}
			catch (Exception ex) 
			{
				logger.Error (ex, "Failed to read spectrum data from file. Filename=" + Filename);
				return null;
			}
		}

	}
}

