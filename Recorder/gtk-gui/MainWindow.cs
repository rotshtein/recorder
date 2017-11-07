
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.Fixed fixed1;
	
	private global::Gtk.Button btnSpectrum;
	
	private global::Gtk.ComboBox cmbRecordingSize;
	
	private global::Gtk.Button btnFile;
	
	private global::Gtk.Entry txtFilename;
	
	private global::Gtk.CheckButton chkLoop;
	
	private global::Gtk.Entry txtRecordingSize;
	
	private global::Gtk.Label label3;
	
	private global::Gtk.ComboBox cmbAGC;
	
	private global::Gtk.Label label2;
	
	private global::Gtk.ComboBox cmbBandwidth;
	
	private global::Gtk.ComboBox cmbFrequencyMode;
	
	private global::Gtk.Label label1;
	
	private global::Gtk.RadioButton radRecord;
	
	private global::Gtk.RadioButton radTransmit;
	
	private global::Gtk.Entry txtAGC;
	
	private global::Gtk.Entry txtBandwidth;
	
	private global::Gtk.Entry txtFrequency;
	
	private global::Gtk.Button btnRecord;
	
	private global::Gtk.Button btnTransmit;
	
	private global::Gtk.Button button5;
	
	private global::Gtk.Image imgConnectivityLed;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.WidthRequest = 500;
		this.HeightRequest = 500;
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("Recorder");
		this.Icon = global::Stetic.IconLoader.LoadIcon (this, "stock_save", global::Gtk.IconSize.Menu);
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		this.Resizable = false;
		this.AllowGrow = false;
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.fixed1 = new global::Gtk.Fixed ();
		this.fixed1.WidthRequest = 500;
		this.fixed1.HeightRequest = 500;
		this.fixed1.HasWindow = false;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.btnSpectrum = new global::Gtk.Button ();
		this.btnSpectrum.CanFocus = true;
		this.btnSpectrum.Name = "btnSpectrum";
		this.btnSpectrum.UseUnderline = true;
		this.btnSpectrum.Label = global::Mono.Unix.Catalog.GetString ("Spectrum");
		this.fixed1.Add (this.btnSpectrum);
		global::Gtk.Fixed.FixedChild w1 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.btnSpectrum]));
		w1.X = 55;
		w1.Y = 330;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.cmbRecordingSize = global::Gtk.ComboBox.NewText ();
		this.cmbRecordingSize.AppendText (global::Mono.Unix.Catalog.GetString ("Time [msec]"));
		this.cmbRecordingSize.AppendText (global::Mono.Unix.Catalog.GetString ("Samples [x10^6]"));
		this.cmbRecordingSize.AppendText (global::Mono.Unix.Catalog.GetString ("File Size [GB]"));
		this.cmbRecordingSize.Name = "cmbRecordingSize";
		this.cmbRecordingSize.Active = 1;
		this.fixed1.Add (this.cmbRecordingSize);
		global::Gtk.Fixed.FixedChild w2 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.cmbRecordingSize]));
		w2.X = 55;
		w2.Y = 280;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.btnFile = new global::Gtk.Button ();
		this.btnFile.CanFocus = true;
		this.btnFile.Name = "btnFile";
		this.btnFile.UseUnderline = true;
		this.btnFile.Label = global::Mono.Unix.Catalog.GetString ("Specify File");
		this.fixed1.Add (this.btnFile);
		global::Gtk.Fixed.FixedChild w3 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.btnFile]));
		w3.X = 55;
		w3.Y = 380;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.txtFilename = new global::Gtk.Entry ();
		this.txtFilename.CanFocus = true;
		this.txtFilename.Name = "txtFilename";
		this.txtFilename.IsEditable = true;
		this.txtFilename.WidthChars = 30;
		this.txtFilename.InvisibleChar = '•';
		this.fixed1.Add (this.txtFilename);
		global::Gtk.Fixed.FixedChild w4 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.txtFilename]));
		w4.X = 189;
		w4.Y = 382;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.chkLoop = new global::Gtk.CheckButton ();
		this.chkLoop.CanFocus = true;
		this.chkLoop.Name = "chkLoop";
		this.chkLoop.Label = global::Mono.Unix.Catalog.GetString ("Loop");
		this.chkLoop.DrawIndicator = true;
		this.chkLoop.UseUnderline = true;
		this.fixed1.Add (this.chkLoop);
		global::Gtk.Fixed.FixedChild w5 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.chkLoop]));
		w5.X = 377;
		w5.Y = 330;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.txtRecordingSize = new global::Gtk.Entry ();
		this.txtRecordingSize.CanFocus = true;
		this.txtRecordingSize.Name = "txtRecordingSize";
		this.txtRecordingSize.IsEditable = true;
		this.txtRecordingSize.InvisibleChar = '●';
		this.fixed1.Add (this.txtRecordingSize);
		global::Gtk.Fixed.FixedChild w6 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.txtRecordingSize]));
		w6.X = 255;
		w6.Y = 282;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.label3 = new global::Gtk.Label ();
		this.label3.Name = "label3";
		this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("Record Limit");
		this.fixed1.Add (this.label3);
		global::Gtk.Fixed.FixedChild w7 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.label3]));
		w7.X = 55;
		w7.Y = 250;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.cmbAGC = global::Gtk.ComboBox.NewText ();
		this.cmbAGC.AppendText (global::Mono.Unix.Catalog.GetString ("Automatic"));
		this.cmbAGC.AppendText (global::Mono.Unix.Catalog.GetString ("Manual"));
		this.cmbAGC.Name = "cmbAGC";
		this.cmbAGC.Active = 0;
		this.fixed1.Add (this.cmbAGC);
		global::Gtk.Fixed.FixedChild w8 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.cmbAGC]));
		w8.X = 55;
		w8.Y = 200;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.label2 = new global::Gtk.Label ();
		this.label2.Name = "label2";
		this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("AGC:");
		this.fixed1.Add (this.label2);
		global::Gtk.Fixed.FixedChild w9 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.label2]));
		w9.X = 55;
		w9.Y = 170;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.cmbBandwidth = global::Gtk.ComboBox.NewText ();
		this.cmbBandwidth.AppendText (global::Mono.Unix.Catalog.GetString ("Bandwidth [MHz]"));
		this.cmbBandwidth.AppendText (global::Mono.Unix.Catalog.GetString ("Upper Freq [MHz]"));
		this.cmbBandwidth.Name = "cmbBandwidth";
		this.cmbBandwidth.Active = 0;
		this.fixed1.Add (this.cmbBandwidth);
		global::Gtk.Fixed.FixedChild w10 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.cmbBandwidth]));
		w10.X = 55;
		w10.Y = 120;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.cmbFrequencyMode = global::Gtk.ComboBox.NewText ();
		this.cmbFrequencyMode.AppendText (global::Mono.Unix.Catalog.GetString ("Center [MHz]"));
		this.cmbFrequencyMode.AppendText (global::Mono.Unix.Catalog.GetString ("Lower Freq [MHz]"));
		this.cmbFrequencyMode.Name = "cmbFrequencyMode";
		this.cmbFrequencyMode.Active = 0;
		this.fixed1.Add (this.cmbFrequencyMode);
		global::Gtk.Fixed.FixedChild w11 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.cmbFrequencyMode]));
		w11.X = 55;
		w11.Y = 70;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.label1 = new global::Gtk.Label ();
		this.label1.Name = "label1";
		this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Frequency Specifications");
		this.fixed1.Add (this.label1);
		global::Gtk.Fixed.FixedChild w12 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.label1]));
		w12.X = 55;
		w12.Y = 40;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.radRecord = new global::Gtk.RadioButton (global::Mono.Unix.Catalog.GetString ("Record"));
		this.radRecord.CanFocus = true;
		this.radRecord.Name = "radRecord";
		this.radRecord.DrawIndicator = true;
		this.radRecord.UseUnderline = true;
		this.radRecord.Group = new global::GLib.SList (global::System.IntPtr.Zero);
		this.fixed1.Add (this.radRecord);
		global::Gtk.Fixed.FixedChild w13 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.radRecord]));
		w13.X = 55;
		w13.Y = 10;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.radTransmit = new global::Gtk.RadioButton (global::Mono.Unix.Catalog.GetString ("Transmit"));
		this.radTransmit.CanFocus = true;
		this.radTransmit.Name = "radTransmit";
		this.radTransmit.DrawIndicator = true;
		this.radTransmit.UseUnderline = true;
		this.radTransmit.Group = this.radRecord.Group;
		this.fixed1.Add (this.radTransmit);
		global::Gtk.Fixed.FixedChild w14 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.radTransmit]));
		w14.X = 280;
		w14.Y = 10;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.txtAGC = new global::Gtk.Entry ();
		this.txtAGC.CanFocus = true;
		this.txtAGC.Name = "txtAGC";
		this.txtAGC.IsEditable = true;
		this.txtAGC.InvisibleChar = '•';
		this.fixed1.Add (this.txtAGC);
		global::Gtk.Fixed.FixedChild w15 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.txtAGC]));
		w15.X = 250;
		w15.Y = 202;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.txtBandwidth = new global::Gtk.Entry ();
		this.txtBandwidth.CanFocus = true;
		this.txtBandwidth.Name = "txtBandwidth";
		this.txtBandwidth.IsEditable = true;
		this.txtBandwidth.InvisibleChar = '●';
		this.fixed1.Add (this.txtBandwidth);
		global::Gtk.Fixed.FixedChild w16 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.txtBandwidth]));
		w16.X = 250;
		w16.Y = 122;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.txtFrequency = new global::Gtk.Entry ();
		this.txtFrequency.CanFocus = true;
		this.txtFrequency.Name = "txtFrequency";
		this.txtFrequency.IsEditable = true;
		this.txtFrequency.InvisibleChar = '●';
		this.fixed1.Add (this.txtFrequency);
		global::Gtk.Fixed.FixedChild w17 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.txtFrequency]));
		w17.X = 250;
		w17.Y = 72;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.btnRecord = new global::Gtk.Button ();
		this.btnRecord.CanFocus = true;
		this.btnRecord.Name = "btnRecord";
		this.btnRecord.UseUnderline = true;
		this.btnRecord.Label = global::Mono.Unix.Catalog.GetString ("Record");
		this.fixed1.Add (this.btnRecord);
		global::Gtk.Fixed.FixedChild w18 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.btnRecord]));
		w18.X = 180;
		w18.Y = 330;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.btnTransmit = new global::Gtk.Button ();
		this.btnTransmit.CanFocus = true;
		this.btnTransmit.Name = "btnTransmit";
		this.btnTransmit.UseUnderline = true;
		this.btnTransmit.Label = global::Mono.Unix.Catalog.GetString ("Transmit");
		this.fixed1.Add (this.btnTransmit);
		global::Gtk.Fixed.FixedChild w19 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.btnTransmit]));
		w19.X = 289;
		w19.Y = 330;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.button5 = new global::Gtk.Button ();
		this.button5.CanFocus = true;
		this.button5.Name = "button5";
		this.button5.UseUnderline = true;
		this.button5.Label = global::Mono.Unix.Catalog.GetString ("Stop");
		this.fixed1.Add (this.button5);
		global::Gtk.Fixed.FixedChild w20 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.button5]));
		w20.X = 212;
		w20.Y = 455;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.imgConnectivityLed = new global::Gtk.Image ();
		this.imgConnectivityLed.Name = "imgConnectivityLed";
		this.fixed1.Add (this.imgConnectivityLed);
		global::Gtk.Fixed.FixedChild w21 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.imgConnectivityLed]));
		w21.X = 434;
		w21.Y = 12;
		this.Add (this.fixed1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 3041;
		this.DefaultHeight = 1000;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.btnSpectrum.Clicked += new global::System.EventHandler (this.OnButton2Clicked);
		this.btnFile.Clicked += new global::System.EventHandler (this.OnButton3Clicked);
		this.radRecord.Clicked += new global::System.EventHandler (this.OnRadiobutton1Clicked);
		this.btnRecord.Clicked += new global::System.EventHandler (this.OnButton1Clicked);
	}
}
