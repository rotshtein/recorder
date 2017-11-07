using System;
using Gtk;
using System.IO;
using System.Configuration;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using NLog;
using System.Net.NetworkInformation;
using Gdk;



public partial class MainWindow: Gtk.Window
{
	private static Logger logger = LogManager.GetCurrentClassLogger();

	public double Rate = 14e6;
	public double UsefulBW;
	string FileName;
	bool _connectionStatus = false;
	bool RecordMode = true;
	string _device_ip;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();

		//filechooserbutton1.Action = FileChooserAction.Save;
		cmbAGC.Active = 0;
		txtAGC.Sensitive = false;
		_device_ip = ConfigurationManager.AppSettings["device_ip"];
		if (string.IsNullOrEmpty (_device_ip))
		{
			_device_ip = "127.0.0.1";
		}
		lblDeviceId.Text = _device_ip;
		logger.Info ("Start");
		logger.Debug ("Test debug message");
		CheckConnectivity();
		Timer t = new Timer (TimerCheckConnectivity, 5, 0, 2500);
	}

	public void SwitchtoTx()
	{
		cmbFrequencyMode.Active = 0;
		cmbAGC.Active = 1;
		cmbRecordingSize.Sensitive = false;
		btnRecord.Sensitive = false;
		btnTransmit.Sensitive = true;
		btnSpectrum.Sensitive = false;
		chkLoop.Sensitive = true;
		label2.Text = "Tx Gain";
		txtFilename.Text = "";
		FileName = "";
	}

	public void SwitchtoRx()
	{
		cmbFrequencyMode.Active = 0;
		cmbAGC.Active = 0;
		cmbRecordingSize.Sensitive = true;
		btnSpectrum.Sensitive = true;
		btnRecord.Sensitive = true;
		btnTransmit.Sensitive = false;
		btnSpectrum.Sensitive = true;
		chkLoop.Sensitive = false;
		label2.Text = "AGC";
		txtFilename.Text = "";
		FileName = "";
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnFrequencyModeChanged(object sender, EventArgs e)
	{
		if (cmbFrequencyMode.ActiveText == "Center [MHz]")
		{
			lblBandwith.Text = "Bandwidth";
		}
		else
		{
			lblBandwith.Text = "High Frequency";
		}
		logger.Trace("Change Center Frequency");
	}

	protected void OncmbBandwidthChanged (object sender, EventArgs e)
	{
	/*
		if (cmbBandwidth.Active == 0) {
			cmbFrequencyMode.Active = 0;
		} else {

			cmbFrequencyMode.Active = 1;

		}
		logger.Trace ("Change Center Frequency");*/
	}

	protected void OncmbAGChanged(object sender, EventArgs e)
	{
		if (!RecordMode)
		{
			cmbAGC.Active = 1;
		}
		if (cmbAGC.Active == 0) 
		{
			txtAGC.Sensitive = false;
		} else 
		{
			txtAGC.Sensitive = true;
		}
		logger.Trace ("Change Center Frequency");
	}

	protected void OnbtnSpectrumClicked (object sender, EventArgs e)
	{
		//Read Center Frequency

		if (cmbFrequencyMode.Active == 1) {

			MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Spectrum Measurement needs Central Frequency Specified");
			msdSame.Title="Error";
			ResponseType tp = (Gtk.ResponseType)msdSame.Run();       
			msdSame.Destroy();
			return;
		}

		string sCentralFreq = txtFrequency.Text;
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

		if (cmbAGC.Active == 1) {

			//Manual
			string sGain = txtAGC.Text;
			try
			{
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


		if (Error) 
		{
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

	protected void OnbtnFileClicked(object sender, EventArgs e)
	{

		FileName = "";
		if (RecordMode == true)
		{
			Gtk.FileChooserDialog filechooser =
				new Gtk.FileChooserDialog("Choose the file to save",
					this,
					FileChooserAction.Save,
					"Cancel", ResponseType.Cancel,
					"Save", ResponseType.Accept);

			FileName = "";
			filechooser.Show();
			if (filechooser.Run() == (int)ResponseType.Accept)
			{
				FileName = filechooser.Filename;
			}
		
			filechooser.Destroy();


			string FileOnly = System.IO.Path.GetFileName(FileName);
			string Path = System.IO.Path.GetDirectoryName(FileName);
			string FileWithoutExt = System.IO.Path.GetFileNameWithoutExtension(FileName);


			string[] FileList = System.IO.Directory.GetFiles(Path, FileWithoutExt + "*", System.IO.SearchOption.TopDirectoryOnly);

			bool Fexist = false;
			if (FileList.Length > 0)
				Fexist = true;

			if (Fexist)
			{
				MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "File Exists. Overwrite?");
				msdSame.Title = "File Exists";
				ResponseType tp = (Gtk.ResponseType)msdSame.Run(); 
				
				msdSame.Destroy();

				if (tp.ToString() == "No")
				{
					return;
				}	
			}

		}
		else
		{
			Gtk.FileChooserDialog filechooser =
				new Gtk.FileChooserDialog ("Choose the file to transmit",
					this,
					FileChooserAction.Open,
					"Cancel", ResponseType.Cancel,
					"Open", ResponseType.Accept);

			FileName = "";
			filechooser.Show ();
			if (filechooser.Run () == (int)ResponseType.Accept) {
				FileName = filechooser.Filename;
			}

			filechooser.Destroy ();

		}
		txtFilename.Text = FileName;
	}

	protected void OnbtnRecordClicked (object sender, EventArgs e)
	{
		string sVal1 = txtFrequency.Text;
		bool Error1 = false;
		double Val1 = 0;
		try {

			Val1 = Convert.ToDouble (sVal1);
		} catch {
			Error1 = true;
		}


		string sVal2 = txtBandwidth.Text;
		bool Error2 = false;
		double Val2 = 0;
		try {

			Val2 = Convert.ToDouble (sVal2);
		} catch {
			Error2 = true;
		}

		double CentralFreq = 0.0, RecBW = 0.0;
		double LowFreq = 0.0, HighFreq = 0.0;

		if (cmbFrequencyMode.Active == 0) {



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

		string sVal = txtRecordingSize.Text;
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
		switch (cmbRecordingSize.Active)
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

		if (cmbAGC.Active == 1) {

			//Manual
			string sGain = txtAGC.Text;
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
					logger.Error (ex, "Failed to start spectrum process");
				}
			}
		}

		MessageDialog msdSame1 = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Close, "Finished");
		msdSame1.Title="Info";
		ResponseType tp1 = (Gtk.ResponseType)msdSame1.Run();       
		msdSame1.Destroy();
		return;

	}

	protected void OnRecoredModeClicked (object sender, EventArgs e)
	{
		RecordMode = radRecord.Active;
		SwitchtoRx();
	}

	protected void OnTransmitModeClicked(object sender, EventArgs e)
	{
		RecordMode = false;;
		SwitchtoTx();
	}


	private void TimerCheckConnectivity(object o) 
	{
		if (CheckConnectivity () != _connectionStatus) 
		{
			_connectionStatus = !_connectionStatus;
			Pixbuf led = _connectionStatus ? new Gdk.Pixbuf ("green-led.png") : new Gdk.Pixbuf ("red-led.png");
			imgConnectivityLed.Pixbuf = led;
			imgConnectivityLed.Show ();
		}
	}


	private bool CheckConnectivity()
	{
		return CheckConnectivity(_device_ip);
	}

	private bool CheckConnectivity(string host)
	{
		try 
		{ 
			Ping myPing = new Ping();
			byte[] buffer = new byte[32];
			int timeout = 1000;
			PingOptions pingOptions = new PingOptions();
			PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
			return (reply.Status == IPStatus.Success);
		}
		catch (Exception) 
		{
			return false;
		}
	}


	protected void OnRecordTransmitChanged(object sender, EventArgs e)
	{
		RecordMode = radRecord.Active;
	}


}
