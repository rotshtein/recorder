
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.Fixed fixed1;
	
	private global::Gtk.ComboBox combobox1;
	
	private global::Gtk.Entry entry1;
	
	private global::Gtk.Entry entry2;
	
	private global::Gtk.ComboBox combobox2;
	
	private global::Gtk.ComboBox combobox4;
	
	private global::Gtk.Entry entry4;
	
	private global::Gtk.Label label3;
	
	private global::Gtk.Label label2;
	
	private global::Gtk.ComboBox combobox3;
	
	private global::Gtk.Button button1;
	
	private global::Gtk.FileChooserButton filechooserbutton1;
	
	private global::Gtk.Button button2;
	
	private global::Gtk.Label label1;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("Recorder");
		this.Icon = global::Stetic.IconLoader.LoadIcon (this, "stock_save", global::Gtk.IconSize.Menu);
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		this.BorderWidth = ((uint)(2));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.fixed1 = new global::Gtk.Fixed ();
		this.fixed1.Name = "fixed1";
		this.fixed1.HasWindow = false;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.combobox1 = global::Gtk.ComboBox.NewText ();
		this.combobox1.AppendText (global::Mono.Unix.Catalog.GetString ("Center [MHz]"));
		this.combobox1.AppendText (global::Mono.Unix.Catalog.GetString ("Lower Freq [MHz]"));
		this.combobox1.Name = "combobox1";
		this.combobox1.Active = 0;
		this.fixed1.Add (this.combobox1);
		global::Gtk.Fixed.FixedChild w1 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.combobox1]));
		w1.X = 70;
		w1.Y = 70;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.entry1 = new global::Gtk.Entry ();
		this.entry1.CanFocus = true;
		this.entry1.Name = "entry1";
		this.entry1.IsEditable = true;
		this.entry1.InvisibleChar = '●';
		this.fixed1.Add (this.entry1);
		global::Gtk.Fixed.FixedChild w2 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.entry1]));
		w2.X = 225;
		w2.Y = 72;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.entry2 = new global::Gtk.Entry ();
		this.entry2.CanFocus = true;
		this.entry2.Name = "entry2";
		this.entry2.IsEditable = true;
		this.entry2.InvisibleChar = '●';
		this.fixed1.Add (this.entry2);
		global::Gtk.Fixed.FixedChild w3 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.entry2]));
		w3.X = 225;
		w3.Y = 105;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.combobox2 = global::Gtk.ComboBox.NewText ();
		this.combobox2.AppendText (global::Mono.Unix.Catalog.GetString ("Bandwidth [MHz]"));
		this.combobox2.AppendText (global::Mono.Unix.Catalog.GetString ("Upper Freq [MHz]"));
		this.combobox2.Name = "combobox2";
		this.combobox2.Active = 0;
		this.fixed1.Add (this.combobox2);
		global::Gtk.Fixed.FixedChild w4 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.combobox2]));
		w4.X = 70;
		w4.Y = 104;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.combobox4 = global::Gtk.ComboBox.NewText ();
		this.combobox4.AppendText (global::Mono.Unix.Catalog.GetString ("Time [msec]"));
		this.combobox4.AppendText (global::Mono.Unix.Catalog.GetString ("Samples [x10^6]"));
		this.combobox4.AppendText (global::Mono.Unix.Catalog.GetString ("Total File Size [GB]"));
		this.combobox4.Name = "combobox4";
		this.combobox4.Active = 0;
		this.fixed1.Add (this.combobox4);
		global::Gtk.Fixed.FixedChild w5 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.combobox4]));
		w5.X = 70;
		w5.Y = 223;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.entry4 = new global::Gtk.Entry ();
		this.entry4.CanFocus = true;
		this.entry4.Name = "entry4";
		this.entry4.IsEditable = true;
		this.entry4.InvisibleChar = '●';
		this.fixed1.Add (this.entry4);
		global::Gtk.Fixed.FixedChild w6 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.entry4]));
		w6.X = 225;
		w6.Y = 223;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.label3 = new global::Gtk.Label ();
		this.label3.Name = "label3";
		this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("Record Limit");
		this.fixed1.Add (this.label3);
		global::Gtk.Fixed.FixedChild w7 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.label3]));
		w7.X = 55;
		w7.Y = 199;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.label2 = new global::Gtk.Label ();
		this.label2.Name = "label2";
		this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("AGC:");
		this.fixed1.Add (this.label2);
		global::Gtk.Fixed.FixedChild w8 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.label2]));
		w8.X = 55;
		w8.Y = 138;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.combobox3 = global::Gtk.ComboBox.NewText ();
		this.combobox3.AppendText (global::Mono.Unix.Catalog.GetString ("Automatic"));
		this.combobox3.AppendText (global::Mono.Unix.Catalog.GetString ("Manual"));
		this.combobox3.Name = "combobox3";
		this.combobox3.Active = 0;
		this.fixed1.Add (this.combobox3);
		global::Gtk.Fixed.FixedChild w9 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.combobox3]));
		w9.X = 70;
		w9.Y = 158;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.button1 = new global::Gtk.Button ();
		this.button1.CanFocus = true;
		this.button1.Name = "button1";
		this.button1.UseUnderline = true;
		this.button1.Label = global::Mono.Unix.Catalog.GetString ("Record");
		this.fixed1.Add (this.button1);
		global::Gtk.Fixed.FixedChild w10 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.button1]));
		w10.X = 335;
		w10.Y = 270;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.filechooserbutton1 = new global::Gtk.FileChooserButton (global::Mono.Unix.Catalog.GetString ("Select a File"), ((global::Gtk.FileChooserAction)(0)));
		this.filechooserbutton1.Name = "filechooserbutton1";
		this.filechooserbutton1.DoOverwriteConfirmation = true;
		this.fixed1.Add (this.filechooserbutton1);
		global::Gtk.Fixed.FixedChild w11 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.filechooserbutton1]));
		w11.X = 65;
		w11.Y = 270;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.button2 = new global::Gtk.Button ();
		this.button2.CanFocus = true;
		this.button2.Name = "button2";
		this.button2.UseUnderline = true;
		this.button2.Label = global::Mono.Unix.Catalog.GetString ("Spectrum");
		this.fixed1.Add (this.button2);
		global::Gtk.Fixed.FixedChild w12 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.button2]));
		w12.X = 145;
		w12.Y = 270;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.label1 = new global::Gtk.Label ();
		this.label1.Name = "label1";
		this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Frequency Specifications");
		this.fixed1.Add (this.label1);
		global::Gtk.Fixed.FixedChild w13 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.label1]));
		w13.X = 57;
		w13.Y = 49;
		this.Add (this.fixed1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 403;
		this.DefaultHeight = 311;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.combobox1.Changed += new global::System.EventHandler (this.OnCombobox1Changed);
		this.combobox2.Changed += new global::System.EventHandler (this.OnCombobox2Changed);
		this.combobox3.Changed += new global::System.EventHandler (this.OnCombobox3Changed);
		this.button2.Clicked += new global::System.EventHandler (this.OnButton2Clicked);
	}
}
