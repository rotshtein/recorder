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

        double _central_frequncy = 0;
        double _rate = 0;
        string spectru_exe = null;
        MemoryStream bitmapStream = new MemoryStream();
        Process _spectrumProcess = null;
        String _fileName = null;


        public winSpectrum() : base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            spectru_exe = ConfigurationManager.AppSettings["Spectrum"];
        }

        void OnFileSave(object sender, EventArgs args)
        {
        }

        protected void CloseWindow(object sender, EventArgs e)
        {
            this.Destroy();
        }


        public Process GetMessurment(double f0, double Rate, double Gain, string Filename = "/home/x300/spectrum.dat")
        {
            if (!string.IsNullOrEmpty(spectru_exe) && File.Exists(spectru_exe))
            {
                try
                {
                    _fileName = Filename;
                    _central_frequncy = f0;
                    _rate = Rate;

                    _spectrumProcess = new Process();
                    _spectrumProcess.StartInfo.UseShellExecute = true;
                    //_spectrumProcess.StartInfo.RedirectStandardOutput = true;
                    //_spectrumProcess.StartInfo.RedirectStandardError = true;
                    _spectrumProcess.StartInfo.FileName = spectru_exe;
                    _spectrumProcess.StartInfo.Arguments = "--mode spec --freq " + f0.ToString() + " --rate " + Rate.ToString() + " --file " + Filename;
                    _spectrumProcess.Start();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Failed to start spectrum process");
                    return null;
                }
                return _spectrumProcess;
            }
            return null;
        }

        public void ShowSpectrum()
        {
            ShowSpectrum(_fileName);
        }

		public void ShowSpectrum(string Filename)
		{
            try
            {
                while (!_spectrumProcess.HasExited)
                {
                    Thread.Sleep(50);
                }

            }
            catch (Exception ex) 
            {
                logger.Error (ex, "Failed to get spectrum results");
                return;
            }
			LinePlot lp = new LinePlot();
			PointPlot pp = new PointPlot();
			NPlot.Bitmap.PlotSurface2D npSurface = new NPlot.Bitmap.PlotSurface2D(1000, 1000);

			try
            {
                Tuple<float[], float[]> Data = ReadFile(Filename);
                if (Data == null)
                    return;
                lp.AbscissaData = Data.Item1;
                lp.DataSource = Data.Item2;
                lp.Color = System.Drawing.Color.Green;
                npSurface.Add(lp);
                npSurface.XAxis1.Label = "Frequncy [Hz]";
                npSurface.YAxis1.Label = "Power [db]";
                npSurface.Title = "Central Frequncy = " + (_central_frequncy / 1e6).ToString() + " MHz - Bandwidth = " + (_rate / 2 / 1e6).ToString() + " MHz";
                npSurface.BackColor = System.Drawing.Color.White;

                npSurface.Refresh();
            
                MemoryStream ms = new MemoryStream();
				npSurface.Bitmap.Save(ms,System.Drawing.Imaging.ImageFormat.Png);

				ms.Position = 0;
                Gtk.Application.Invoke(delegate
                {
                    Pixbuf p = new Gdk.Pixbuf(ms);

                    image.Pixbuf = p;
                });
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

