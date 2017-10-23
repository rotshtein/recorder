using System;
using Gtk;
using System.IO;

public partial class MainWindow: Gtk.Window
{
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

		Recorder.winSpectrum spectrum = new Recorder.winSpectrum ();
		spectrum.ShowSpectrum ();

	}

	protected void OnButton3Clicked (object sender, EventArgs e)
	{
		Gtk.FileChooserDialog filechooser =
			new Gtk.FileChooserDialog("Choose the file to open",
				this,
				FileChooserAction.Open,
				"Cancel",ResponseType.Cancel,
				"Open",ResponseType.Accept);

		if (filechooser.Run() == (int)ResponseType.Accept) 
		{
			System.IO.FileStream file = System.IO.File.OpenRead(filechooser.Filename);
			file.Close();
		}

		filechooser.Destroy();

	}
}
