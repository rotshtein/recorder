using System;
using Gtk;
using System.IO;
using System.Configuration;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using NLog;



public partial class MainWindow: Gtk.Window
{
	private static Logger logger = LogManager.GetCurrentClassLogger();

	public double Rate = 14e6;
	public double UsefulBW;
	string FileName;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();

		//filechooserbutton1.Action = FileChooserAction.Save;
		combobox3.Active = 0;
		entry3.Sensitive = false;


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
			entry3.Sensitive = false;
		} else {
			entry3.Sensitive = true;
		}
		//throw new NotImplementedException ();
	}

	protected void OnButton2Clicked (object sender, EventArgs e)
	{
		//Read Center Frequency

		if (combobox1.Active == 1) {

			MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Spectrum Measurement needs Central Frequency Specified");
			msdSame.Title="Error";
			ResponseType tp = (Gtk.ResponseType)msdSame.Run();       
			msdSame.Destroy();
			return;
		}

		string sCentralFreq = entry1.Text;
		bool Error = false;
		double CentralFreq = 0;
		try{
			
			CentralFreq = Convert.ToDouble (sCentralFreq);
		}

		catch
		{
			Error = true;
		}

		if (!Error) {

			if ((CentralFreq < 950) || (CentralFreq > 2150)) {
				Error = true;
			}
		}

		if (Error) {
		
			MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Central Frequency should be specified in MHz, in the range 950-2150");
			msdSame.Title="Error";
			ResponseType tp = (Gtk.ResponseType)msdSame.Run();       
			msdSame.Destroy();
			return;

		
		}
		CentralFreq *= 1e6;


		// Read Gain
		double Gain = -1; //AGC
		Error = false;

		if (combobox3.Active == 1) {

			//Manual
			string sGain = entry3.Text;
			try{

				Gain = Convert.ToDouble (sGain);
			}

			catch
			{
				Error = true;
			}

			if (!Error) {

				if ((Gain < 0) || (Gain > 30.5)) {
					Error = true;
				}
			}
		}


		if (Error) {

			MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Gain setting should be set to either AGC or Manual gain in the range 0-30.5");
			msdSame.Title="Error";
			ResponseType tp = (Gtk.ResponseType)msdSame.Run();       
			msdSame.Destroy();
			return;


		}


		Recorder.winSpectrum spectrum = new Recorder.winSpectrum ();
		string Filename = spectrum.GetMessurment (CentralFreq, Rate, Gain, "/home/x300/spectrum.dat");
		spectrum.ShowSpectrum (Filename);

	}

	protected void OnButton3Clicked (object sender, EventArgs e)
	{

		FileName = "";
		
		Gtk.FileChooserDialog filechooser =
			new Gtk.FileChooserDialog("Choose the file to save",
				this,
				FileChooserAction.Save,
				"Cancel",ResponseType.Cancel,
				"Save",ResponseType.Accept);

		FileName = "";
		filechooser.Show ();
		if (filechooser.Run() == (int)ResponseType.Accept) 
		{
			FileName = filechooser.Filename;
		}

		filechooser.Destroy();


		string FileOnly =  System.IO.Path.GetFileName(FileName);
		string Path = System.IO.Path.GetDirectoryName(FileName);
		string FileWithoutExt = System.IO.Path.GetFileNameWithoutExtension(FileName);


		string[] FileList = System.IO.Directory.GetFiles(Path, FileWithoutExt+"*", System.IO.SearchOption.TopDirectoryOnly);

		bool Fexist = false;
		if (FileList.Length > 0)
			Fexist = true;

		if (Fexist) {
		
			MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "File Exists. Overwrite?");
			msdSame.Title="File Exists";
			ResponseType tp = (Gtk.ResponseType)msdSame.Run(); 
				
			msdSame.Destroy();

			if (tp.ToString() == "No") {
				return;
			}	

		}


		entry5.Text = FileName;


	}

	protected void OnButton1Clicked (object sender, EventArgs e)
	{
		string sVal1 = entry1.Text;
		bool Error1 = false;
		double Val1 = 0;
		try {

			Val1 = Convert.ToDouble (sVal1);
		} catch {
			Error1 = true;
		}


		string sVal2 = entry2.Text;
		bool Error2 = false;
		double Val2 = 0;
		try {

			Val2 = Convert.ToDouble (sVal2);
		} catch {
			Error2 = true;
		}

		double CentralFreq = 0.0, RecBW = 0.0;
		double LowFreq = 0.0, HighFreq = 0.0;

		if (combobox1.Active == 0) {



			//Center, BW



			if (!Error1) {

				if ((Val1 < 950) || (Val1 > 2150)) {
					Error1 = true;
				}
			}

			if (Error1) {

				MessageDialog msdSame = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Central Frequency should be specified in MHz, in the range 950-2150");
				msdSame.Title = "Error";
				ResponseType tp = (Gtk.ResponseType)msdSame.Run ();       
				msdSame.Destroy ();
				return;


			}
			CentralFreq = Val1 * 1e6;

			if (!Error2) {

				RecBW = Val2 * 1e6;

				LowFreq = CentralFreq - 0.5 * RecBW;
				HighFreq = CentralFreq + 0.5 * RecBW;

				if ((LowFreq < 950e6) || (HighFreq > 2150e6)) {
					Error2 = true;
				}

			}


			if (Error2) {

				MessageDialog msdSame = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Bandwidth should be specified in MHz, so the lower and upper Frequencies will be in range 950 - 2150 MHz");
				msdSame.Title = "Error";
				ResponseType tp = (Gtk.ResponseType)msdSame.Run ();       
				msdSame.Destroy ();
				return;


			}


		} else {

			//Lower, Upper


			if (!Error1) {

				if ((Val1 < 950)) {
					Error1 = true;
				} 
			}

			if (Error1) {

				MessageDialog msdSame = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Lower Frequency should be specified in MHz, in the range 950-2150");
				msdSame.Title = "Error";
				ResponseType tp = (Gtk.ResponseType)msdSame.Run ();       
				msdSame.Destroy ();
				return;


			}
			LowFreq = Val1 * 1e6;

			if (!Error2) {

				HighFreq = Val2 * 1e6;


				if ((Val2 > 2150)) {
					Error2 = true;
				}

			}


			if (Error2) {

				MessageDialog msdSame = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Upper Frequency should be specified in MHz, in the range 950-2150");
				msdSame.Title = "Error";
				ResponseType tp = (Gtk.ResponseType)msdSame.Run ();       
				msdSame.Destroy ();
				return;


			}

			if (LowFreq > HighFreq) {

				MessageDialog msdSame = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Upper and Lower Frequency mismatch ");
				msdSame.Title = "Error";
				ResponseType tp = (Gtk.ResponseType)msdSame.Run ();       
				msdSame.Destroy ();
				return;


			}

		}

		UsefulBW = 0.75 * Rate;
		double dNumSessions = Math.Ceiling ((HighFreq - LowFreq)/UsefulBW);
		int NumSessions = Convert.ToInt32 (dNumSessions);

		//Convert everything to Samples

		string sVal = entry4.Text;
		double Val = 0;
		Error1 = false;
		try {
			
			Val = Convert.ToDouble (sVal);
		} catch {
			Error1 = true;
		}

		if (!Error1) {
			if (Val <= 0)
				Error1 = true;
		}
			

		if (Error1) {

			MessageDialog msdSame = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Please enter a positive size for file recording ");
			msdSame.Title = "Error";
			ResponseType tp = (Gtk.ResponseType)msdSame.Run ();       
			msdSame.Destroy ();
			return;

		}

		double dNumSamples=0;
		switch (combobox4.Active)
		{
			//Time
		case 0:
			dNumSamples = Math.Ceiling (Val * Rate * 1e-3);
			break;
		case 1:
			dNumSamples = Val * 1e6;
			break;
		case 2: 
			dNumSamples = 1073741824 * Val * 0.25;
			break;
		}

		if (NumSessions > 1) {
			double dTotalSize = dNumSamples * 4 / (1073741824.0) * NumSessions;
			MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Total File Size "+Math.Round(dTotalSize, 2).ToString("#.00")+ " Gb. Continue?");
			msdSame.Title="File Size Warning";
			ResponseType tp = (Gtk.ResponseType)msdSame.Run(); 

			msdSame.Destroy();

			if (tp.ToString() == "No") {
				return;
			}	


		}


		if (FileName == null) {

			MessageDialog msdSame = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Please specify a file name for recording ");
			msdSame.Title = "Error";
			ResponseType tp = (Gtk.ResponseType)msdSame.Run ();       
			msdSame.Destroy ();
			return;
		}

		string FileOnly =  System.IO.Path.GetFileName(FileName);
		string Path = System.IO.Path.GetDirectoryName(FileName);
		string FileWithoutExt = System.IO.Path.GetFileNameWithoutExtension(FileName);
		string Extn = System.IO.Path.GetExtension(FileName);

		// Read Gain
		double Gain = -1; //AGC
		Error1 = false;

		if (combobox3.Active == 1) {

			//Manual
			string sGain = entry3.Text;
			try{

				Gain = Convert.ToDouble (sGain);
			}

			catch
			{
				Error1 = true;
			}

			if (!Error1) {

				if ((Gain < 0) || (Gain > 30.5)) {
					Error1 = true;
				}
			}
		}


		if (Error1) {

			MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Gain setting should be set to either AGC or Manual gain in the range 0-30.5");
			msdSame.Title="Error";
			ResponseType tp = (Gtk.ResponseType)msdSame.Run();       
			msdSame.Destroy();
			return;


		}

		Int64 NumSamples = Convert.ToInt64 (dNumSamples);

		double f0 = CentralFreq;
		string CurrFile = FileName;
		string	spectru_exe = ConfigurationManager.AppSettings["Spectrum"];
		if (!string.IsNullOrEmpty (spectru_exe) && File.Exists (spectru_exe)) {
			for (int ii = 0; ii < NumSessions; ii++) {
				if (NumSessions > 1) {
					f0 = LowFreq + UsefulBW*(0.5 + ii);
					CurrFile = Path +"/"+ FileWithoutExt + ii.ToString("000")  + Extn;
				}
				Process p = new Process ();
				p.StartInfo.UseShellExecute = false;
				p.StartInfo.RedirectStandardOutput = true;
				p.StartInfo.RedirectStandardError = true;
				p.StartInfo.FileName = spectru_exe;
				p.StartInfo.Arguments = "--mode record --freq " + f0.ToString () + " --rate " + Rate.ToString () + " --file " + CurrFile + " --nsamps " + NumSamples.ToString ();
				try {
					if (p.Start ()) {
						string output = p.StandardOutput.ReadToEnd ();
						string err = p.StandardError.ReadToEnd ();
						while (!p.HasExited) {
							output = p.StandardOutput.ReadToEnd ();	
						}
						//p.WaitForExit (1000 * 60);
						string error = p.StandardError.ReadToEnd ();	
					}
				} catch (Exception ex) 
				{
					logger.Error (ex, "Failed to start the specrtum process");
				}
			}
		}

		MessageDialog msdSame1 = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Close, "Finished");
		msdSame1.Title="Info";
		ResponseType tp1 = (Gtk.ResponseType)msdSame1.Run();       
		msdSame1.Destroy();
		return;

	}

	protected void OnRadiobutton1Clicked (object sender, EventArgs e)
	{
		int rb = 0;
		if (radiobutton1.Active) {
			rb = 1;
		}
		else {
			rb = 2;
		}

	}


}
