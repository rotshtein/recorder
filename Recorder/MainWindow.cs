
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
using System.Threading.Tasks;



public partial class MainWindow : Gtk.Window
{
    private static Logger logger = LogManager.GetCurrentClassLogger();

    public double Rate = 184.32e6;
    public double UsefulBW;
    string FileName;
    bool _connectionStatus = true;
    bool RecordMode = true;
    string _device_ip;
    //uint con = 0;
    Process _currentProcess;
    Task _recordTask = null;
    Timer connectivityTimer = null;

	public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
        try
        {
            this.Title = "RF Recorder 1.0";
            //filechooserbutton1.Action = FileChooserAction.Save;
            cmbAGC.Active = 0;
            txtAGC.Sensitive = false;
            try
            {
                _device_ip = ConfigurationManager.AppSettings["device_ip"];
                if (string.IsNullOrEmpty(_device_ip))
                {
                    _device_ip = "127.0.0.1";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Can't get device ip address. using 127.0.0.1");
            }

            try
            {
                Rate = double.Parse(ConfigurationManager.AppSettings["Rate"]);
                if (Rate == 0)
                {
                    Rate = 184.32e6;
                }
            }
            catch (Exception ex)
            {
                Rate = 184.32e6;
                logger.Error(ex, "Can't get Rate. using 184.32M");
            }


            lblDeviceId.Text = _device_ip;
            logger.Info("Start");
            CheckConnectivity();
            connectivityTimer = new Timer(TimerCheckConnectivity, 5, 0, 2500);

            SwitchtoRx();
            statusbar.Push(statusbar.GetContextId("Hello"), "Hello");
            statusbar.ShowNow();

            statusbar.SetSizeRequest(fixed1.WidthRequest, 20);
        }
		catch (Exception ex)
		{
			logger.Error(ex);
		}

	}

    public void SwitchtoTx()
    {
        try
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
            txtBandwidth.Sensitive = false;
            txtRecordingSize.Sensitive = false;
            FileName = "";
        }
		catch (Exception ex)
		{
			logger.Error(ex);
		}
	}

    public void SwitchtoRx()
    {
        try
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
            txtRecordingSize.Sensitive = true;
            txtFilename.Text = "";
            FileName = "";
            txtBandwidth.Sensitive = true;
        }
		catch (Exception ex)
		{
			logger.Error(ex);
		}


	}

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnFrequencyModeChanged(object sender, EventArgs e)
    {
        try
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
		catch (Exception ex)
		{
			logger.Error(ex);
		}

	}

    protected void OnCmbAGCChanged(object sender, EventArgs e)
    {
        try
        {
            if (!RecordMode)
            {
                cmbAGC.Active = 1;
            }
            if (cmbAGC.Active == 0)
            {
                txtAGC.Sensitive = false;
            }
            else
            {
                txtAGC.Sensitive = true;
            }
            logger.Trace("Change Center Frequency");
        }
        catch (Exception ex)
        {
            logger.Error(ex);
        }
    }

    protected void OnbtnSpectrumClicked(object sender, EventArgs e)
    {
        Spectrum();
    }
	private async Task<bool> Spectrum()
	{
		var result = await SpectrumTask();
		return result;
	}

    private Task<bool> SpectrumTask()
    {
        return Task.Run(() =>
        {
            try
            {
                if (_connectionStatus == false)
                {
                    Gtk.Application.Invoke(delegate
                    {
                        MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "USRP not connected");
                        msdSame.Title = "Error";
                        ResponseType tp = (Gtk.ResponseType)msdSame.Run();
                        msdSame.Destroy();

                    });
                    return false;
                }
                    //Read Center Frequency
                if (cmbFrequencyMode.Active == 1)
                {
                    Gtk.Application.Invoke(delegate
                    {
                        MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Spectrum Measurement needs Central Frequency Specified");
                        msdSame.Title = "Error";
                        ResponseType tp = (Gtk.ResponseType)msdSame.Run();
                        msdSame.Destroy();
                    });
                    return false;
                }

                string sCentralFreq = txtFrequency.Text;
                bool Error = false;
                double CentralFreq = 0;
                if (string.IsNullOrEmpty(sCentralFreq))
                {
                    Error = true;
                }
                else
                {
                    try
                    {
                        CentralFreq = Convert.ToDouble(sCentralFreq);
                    }

                    catch
                    {
                        Error = true;
                    }
                }
                if (!Error)
                {

                    if ((CentralFreq < 950) || (CentralFreq > 2150))
                    {
                        Error = true;
                    }
                }

                if (Error)
                {
                    Gtk.Application.Invoke(delegate
                    {
                        MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Central Frequency should be specified in MHz, in the range 950-2150");
                        msdSame.Title = "Error";
                        ResponseType tp = (Gtk.ResponseType)msdSame.Run();
                        msdSame.Destroy();
                    });
                    return false;
                }
                CentralFreq *= 1e6;


                    // Read Gain
                    double Gain = -1; //AGC
                    Error = false;

                if (cmbAGC.Active == 1)
                {
                        //Manual
                        string sGain = txtAGC.Text;
                    try
                    {
                        Gain = Convert.ToDouble(sGain);
                    }
                    catch
                    {
                        Error = true;
                    }

                    if (!Error)
                    {

                        if ((Gain < 0) || (Gain > 30.5))
                        {
                            Error = true;
                        }
                    }
                }


                if (Error)
                {
                    Gtk.Application.Invoke(delegate
                    {
                        MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Gain setting should be set to either AGC or Manual gain in the range 0-30.5");
                        msdSame.Title = "Error";
                        ResponseType tp = (Gtk.ResponseType)msdSame.Run();
                        msdSame.Destroy();
                    });
                    return false;
                }
                string Filename = "/home/x300/spectrum.dat";
                ShowStatusMessage("Fetching Specrtum...");
                Recorder.winSpectrum spectrum = new Recorder.winSpectrum();
                _currentProcess = spectrum.GetMessurment(CentralFreq, Rate, Gain, Filename);
                spectrum.ShowSpectrum();
                ShowStatusMessage("Specrtum Ready");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error getting spectrum graph");
            }
            return true;
        });
    }

    protected void OnBtnFileClicked(object sender, EventArgs e)
    {
        try
        {
            FileName = "";
            if (RecordMode == true)
            {
                Gtk.FileChooserDialog filechooser = null;

                try
                {
                    filechooser = new Gtk.FileChooserDialog("Choose the file to save",
                            this,
                            FileChooserAction.Save,
                            "Cancel", ResponseType.Cancel,
                            "Save", ResponseType.Accept);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Choose receive File");
                }
                FileName = "";
                filechooser.Show();
                if (filechooser.Run() == (int)ResponseType.Accept)
                {
                    FileName = filechooser.Filename;
                }

                filechooser.Destroy();


                string FileOnly = System.IO.Path.GetFileName(FileName);
                string path = System.IO.Path.GetDirectoryName(FileName);
                string FileWithoutExt = System.IO.Path.GetFileNameWithoutExtension(FileName);


                string[] FileList = System.IO.Directory.GetFiles(path, FileWithoutExt + "*", System.IO.SearchOption.TopDirectoryOnly);

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
                Gtk.FileChooserDialog filechooser = null;
                try
                {
                    filechooser = new Gtk.FileChooserDialog("Choose the file to transmit",
                        this,
                        FileChooserAction.Open,
                        "Cancel", ResponseType.Cancel,
                        "Open", ResponseType.Accept);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Choose transmit File");
                }
                FileName = "";
                filechooser.Show();
                if (filechooser.Run() == (int)ResponseType.Accept)
                {
                    FileName = filechooser.Filename;
                }

                filechooser.Destroy();

            }
            txtFilename.Text = FileName;
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error while choosing file");
        }
    }

    protected void OnbtnRecordClicked(object sender, EventArgs e)
    {
        _recordTask = Recorder();
    }

    private async Task<bool> Recorder()
    {
        var result = await ReceiveThread();
        return result;
    }

    public Task<bool> ReceiveThread()
    {
        return Task.Run(() =>
        {
            if (_connectionStatus == false)
            {
                Gtk.Application.Invoke(delegate
                {
                    MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "USRP not connected");
                    msdSame.Title = "Error";
                    ResponseType tp = (Gtk.ResponseType)msdSame.Run();
                    msdSame.Destroy();
                });
                return false;
            }

            Console.WriteLine("Starting ....");

        string sVal1 = txtFrequency.Text;
        bool Error1 = false;
        double Val1 = 0;
        try
        {
            Val1 = Convert.ToDouble(sVal1);
        }
        catch
        {
            Error1 = true;
        }

        string sVal2 = txtBandwidth.Text;
        bool Error2 = false;
        double Val2 = 0;
        try
        {
            Val2 = Convert.ToDouble(sVal2);
        }
        catch
        {
            Error2 = true;
        }

        double CentralFreq = 0.0, RecBW = 0.0;
        double LowFreq = 0.0, HighFreq = 0.0;

        if (cmbFrequencyMode.Active == 0)
        {
            //Center, BW
            if (!Error1)
            {
                if ((Val1 < 950) || (Val1 > 2150))
                {
                    Error1 = true;
                }
            }

            if (Error1)
            {
                    Gtk.Application.Invoke(delegate
                    {
                        MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Central Frequency should be specified in MHz, in the range 950-2150");
                        msdSame.Title = "Error";
                        ResponseType tp = (Gtk.ResponseType)msdSame.Run();
                        msdSame.Destroy();
                    });
                return false;
            }
            CentralFreq = Val1 * 1e6;

            if (!Error2)
            {
                RecBW = Val2 * 1e6;

                LowFreq = CentralFreq - 0.5 * RecBW;
                HighFreq = CentralFreq + 0.5 * RecBW;

                if ((LowFreq < 950e6) || (HighFreq > 2150e6))
                {
                    Error2 = true;
                }
            }


            if (Error2)
            {
                    Gtk.Application.Invoke(delegate
                    {
                        MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Bandwidth should be specified in MHz, so the lower and upper Frequencies will be in range 950 - 2150 MHz");
                        msdSame.Title = "Error";
                        ResponseType tp = (Gtk.ResponseType)msdSame.Run();
                        msdSame.Destroy();
                    });
                return false;
            }
        }
        else
        {
            //Lower, Upper
            if (!Error1)
            {
                if ((Val1 < 950))
                {
                    Error1 = true;
                }
            }

            if (Error1)
            {
                    Gtk.Application.Invoke(delegate
                    {
                        MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Lower Frequency should be specified in MHz, in the range 950-2150");
                        msdSame.Title = "Error";
                        ResponseType tp = (Gtk.ResponseType)msdSame.Run();
                        msdSame.Destroy();
                    });
                return false;
            }
            LowFreq = Val1 * 1e6;

            if (!Error2)
            {
                HighFreq = Val2 * 1e6;


                if ((Val2 > 2150))
                {
                    Error2 = true;
                }
            }


            if (Error2)
            {
                    Gtk.Application.Invoke(delegate
                    {
                        MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Upper Frequency should be specified in MHz, in the range 950-2150");
                        msdSame.Title = "Error";
                        ResponseType tp = (Gtk.ResponseType)msdSame.Run();
                        msdSame.Destroy();
                    });
                return false;
            }

            if (LowFreq > HighFreq)
            {
                    Gtk.Application.Invoke(delegate
                    {
                        MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Upper and Lower Frequency mismatch ");
                        msdSame.Title = "Error";
                        ResponseType tp = (Gtk.ResponseType)msdSame.Run();
                        msdSame.Destroy();
                    });
                return false;
            }
        }

        UsefulBW = 0.75 * Rate;
        double dNumSessions = Math.Ceiling((HighFreq - LowFreq) / UsefulBW);
        int NumSessions = Convert.ToInt32(dNumSessions);

        //Convert everything to Samples

        string sVal = txtRecordingSize.Text;
        double Val = 0;
        Error1 = false;
        try
        {
            Val = Convert.ToDouble(sVal);
        }
        catch
        {
            Error1 = true;
        }

        if (!Error1)
        {
            if (Val <= 0)
                Error1 = true;
        }

        if (Error1)
        {
                Gtk.Application.Invoke(delegate
                {
                    MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Please enter a positive size for file recording ");
                    msdSame.Title = "Error";
                    ResponseType tp = (Gtk.ResponseType)msdSame.Run();
                    msdSame.Destroy();
                });
            return false;
        }

        #region Recording Size
        double dNumSamples = 0;
        switch (cmbRecordingSize.Active)
        {
            case 0: //Time
                dNumSamples = Math.Ceiling(Val * Rate);
                break;

            case 1:
                dNumSamples = Val * 1e6;
                break;

            case 2:
                dNumSamples = 1073741824 * Val * 0.25;
                break;
        }
        #endregion

        if (NumSessions > 1)
        {
            double dTotalSize = dNumSamples * 4 / (1073741824.0) * NumSessions;
                Gtk.Application.Invoke(delegate
                {
                    MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Total File Size " + Math.Round(dTotalSize, 2).ToString("#.00") + " Gb. Continue?");
                    msdSame.Title = "File Size Warning";
                    ResponseType tp = (Gtk.ResponseType)msdSame.Run(); 
                    msdSame.Destroy();
                    if (tp.ToString() == "No")
                    {
                        return;
                    }
                });
        }

            if (FileName == null)
            {
                Gtk.Application.Invoke(delegate
                {
                MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Please specify a file name for recording ");
                msdSame.Title = "Error";
                ResponseType tp = (Gtk.ResponseType)msdSame.Run();
                msdSame.Destroy();
                });
            return false;
        }

        string FileOnly = System.IO.Path.GetFileName(FileName);
        string path = System.IO.Path.GetDirectoryName(FileName);
        string FileWithoutExt = System.IO.Path.GetFileNameWithoutExtension(FileName);
        string Extn = System.IO.Path.GetExtension(FileName);

        // Read Gain
        double Gain = -1; //AGC
        Error1 = false;

        if (cmbAGC.Active == 1)
        {
            //Manual
            string sGain = txtAGC.Text;
            try
            {
                Gain = Convert.ToDouble(sGain);
            }
            catch
            {
                Error1 = true;
            }

            if (!Error1)
            {
                if ((Gain < 0) || (Gain > 30.5))
                {
                    Error1 = true;
                }
            }
        }


        if (Error1)
        {
                Gtk.Application.Invoke(delegate
                {
                    MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Gain setting should be set to either AGC or Manual gain in the range 0-30.5");
                    msdSame.Title = "Error";
                    ResponseType tp = (Gtk.ResponseType)msdSame.Run();
                    msdSame.Destroy();
                   
                }); 
                return false;
        }

        Int64 NumSamples = Convert.ToInt64(dNumSamples);

        double f0 = CentralFreq;
        string CurrFile = FileName;
        string record_exe = ConfigurationManager.AppSettings["Record"];
            if (!string.IsNullOrEmpty(record_exe) && File.Exists(record_exe))
            {
                for (int ii = 0; ii < NumSessions; ii++)
                {
                    if (NumSessions > 1)
                    {
                        f0 = LowFreq + UsefulBW * (0.5 + ii);
                        CurrFile = path + "/" + FileWithoutExt + ii.ToString("000") + Extn;
                    }
                    KillRunningProcess();
                    _currentProcess = new Process();
                    _currentProcess.StartInfo.UseShellExecute = true;
                    _currentProcess.StartInfo.FileName = record_exe;
                    _currentProcess.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(record_exe);
                    _currentProcess.StartInfo.Arguments = "--mode record --freq " + f0.ToString() + " --rate " + Rate.ToString() + " --gain " + Gain.ToString() + " --file " + CurrFile + " --nsamps " + NumSamples.ToString();
					string StatusFile = _currentProcess.StartInfo.WorkingDirectory + "/Statuses.txt";
					try
					{
						if (File.Exists(StatusFile))
						{
							File.Delete(StatusFile);
						}
					}
					catch (Exception ex)
					{
						logger.Error(ex, "Error deleting status file");
					}
					try
                    {
                        bool r = _currentProcess.Start();
                        {
                            int rows = 0;
                            while (!_currentProcess.HasExited)
                            {
                                rows = UpdateStatusFromFile(rows, StatusFile);
                                Thread.Sleep(10);
							}
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Failed to start spectrum process");
                    }
                    //_currentProcess = null;
                }
            }
            Gtk.Application.Invoke(delegate
            {
                MessageDialog msdSame1 = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Close, "Finished");
                msdSame1.Title = "Info";
                //msdSame1.Response += (object o, ResponseArgs args) => { };
                ResponseType tp1 = (Gtk.ResponseType)msdSame1.Run();
                msdSame1.Destroy();
                try
                {
                    if (_currentProcess != null)
                    {
                        int i = _currentProcess.Id;
                        _currentProcess.Kill();
                        _currentProcess = null;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Finished - Dialog message error");
                }
            });
            //t.Wait();
            return true;
        });
    }



    protected void OnRecoredModeClicked (object sender, EventArgs e)
    {
        RecordMode = radRecord.Active;
        if (RecordMode)
            SwitchtoRx ();
        else
            SwitchtoTx ();
    }

	protected void OnTransmitModeClicked(object sender, EventArgs e)
    {
        RecordMode = false;;
        SwitchtoTx();
    }

    private void TimerCheckConnectivity(object o) 
    {
        try
        {
            if (CheckConnectivity() != _connectionStatus)
            {
                _connectionStatus = !_connectionStatus;
				/*Gtk.Application.Invoke(delegate
				{
                    Pixbuf led = _connectionStatus ? new Gdk.Pixbuf(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("green-led.png")) :
                        new Gdk.Pixbuf(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("red-led.png"));
                    imgConnectivityLed.Pixbuf = led;
                    imgConnectivityLed.Show();
                });*/
            }
			Gtk.Application.Invoke(delegate
			{
				Pixbuf led = _connectionStatus ? new Gdk.Pixbuf(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("green-led.png")) :
					new Gdk.Pixbuf(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("red-led.png"));
				imgConnectivityLed.Pixbuf = led;
				imgConnectivityLed.Show();
			});
            //_connectionStatus = true;
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Check connectivity error");
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
        catch (Exception ex) 
        {
            logger.Error(ex, "Error pinging to the x300"); 
            return false;
        }
    }


    protected void OnRecordTransmitChanged(object sender, EventArgs e)
    {
        RecordMode = radRecord.Active;
    }

	protected void OnBtnTransmitClicked(object sender, EventArgs e)
	{
		Transmit();
	}
	private async Task<bool> Transmit()
	{
		var result = await TransmitTask();
		return result;
	}

    protected Task<bool> TransmitTask()
    {
        return Task.Run(() =>
        {
            try
            {
                if (_connectionStatus == false)
                {
                    Gtk.Application.Invoke(delegate
                    {
                        MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "USRP not connected");
                        msdSame.Title = "Error";
                        ResponseType tp = (Gtk.ResponseType)msdSame.Run();
                        msdSame.Destroy();
                    });
                    return false;
                }


                string sVal1 = txtFrequency.Text;
                bool Error1 = false;
                double Val1 = 0;
                try
                {
                    Val1 = Convert.ToDouble(sVal1);
                }
                catch
                {
                    Error1 = true;
                }

                double CentralFreq = 0.0;

                //Center, BW
                if (!Error1)
                {
                    if ((Val1 < 950) || (Val1 > 2150))
                    {
                        Error1 = true;
                    }
                }

                if (Error1)
                {
                    Gtk.Application.Invoke(delegate
                    {
                        MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Central Frequency should be specified in MHz, in the range 950-2150");
                        msdSame.Title = "Error";
                        ResponseType tp = (Gtk.ResponseType)msdSame.Run();
                        msdSame.Destroy();
                    });
                    return false;
                }
                CentralFreq = Val1 * 1e6;

                if (FileName == null)
                {
                    Gtk.Application.Invoke(delegate
                    {
                        MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Please specify a file for playing ");
                        msdSame.Title = "Error";
                        ResponseType tp = (Gtk.ResponseType)msdSame.Run();
                        msdSame.Destroy();
                    });
                    return false;
                }

                string FileOnly = System.IO.Path.GetFileName(FileName);
                string path = System.IO.Path.GetDirectoryName(FileName);
                string FileWithoutExt = System.IO.Path.GetFileNameWithoutExtension(FileName);
                string Extn = System.IO.Path.GetExtension(FileName);

                double Gain = -1.0;

                string sGain = txtAGC.Text;
                try
                {
                    Gain = Convert.ToDouble(sGain);
                }
                catch
                {
                    Error1 = true;
                }

                if (!Error1)
                {
                    if ((Gain < 0) || (Gain > 30.5))
                    {
                        Error1 = true;
                    }
                }

                if (Error1)
                {
                    Gtk.Application.Invoke(delegate
                    {
                        MessageDialog msdSame = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Gain setting should be set to Manual gain in the range 0-30.5");
                        msdSame.Title = "Error";
                        ResponseType tp = (Gtk.ResponseType)msdSame.Run();
                        msdSame.Destroy();
                    });
                    return false;
                }

                double f0 = CentralFreq;
                string CurrFile = FileName;
                string transmit_exe = ConfigurationManager.AppSettings["Transmit"];
                ShowStatusMessage("Setting transmiter");
                if (!string.IsNullOrEmpty(transmit_exe) && File.Exists(transmit_exe))
                {
                    KillRunningProcess();
                    _currentProcess = new Process();
                    _currentProcess.StartInfo.UseShellExecute = false;
                    _currentProcess.StartInfo.FileName = transmit_exe;
                    _currentProcess.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(transmit_exe);
                    string LoopMode = "";
                    if (chkLoop.Active)
                        LoopMode = " --loop";
                    Console.WriteLine("--mode play --freq " + f0.ToString() + " --rate " + Rate.ToString() + " --gain " + Gain.ToString() + " --file " + CurrFile + LoopMode);
                    _currentProcess.StartInfo.Arguments = "--mode play --freq " + f0.ToString() + " --rate " + Rate.ToString() + " --gain " + Gain.ToString() + " --file " + CurrFile + LoopMode;
					string StatusFile = _currentProcess.StartInfo.WorkingDirectory + "/Statuses.txt";
                    try
                    {
                        if (File.Exists(StatusFile))
                        {
                            File.Delete(StatusFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Error deleting status file");
                    }

					try
                    {

						if (_currentProcess.Start())
                        {
                            ShowStatusMessage("Transmiting...");

                            int rows = 0;

                            while (!_currentProcess.HasExited)
                            {
                                rows = UpdateStatusFromFile(rows, StatusFile);
                                Thread.Sleep(10);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Failed to start spectrum process");
                    }
                    _currentProcess = null;
                }
            }

            catch (Exception ex)
            {
                logger.Error(ex, "Transmit error");
            }
            return true;
        });
	}

    private int UpdateStatusFromFile(int rows, string Filename)
    {
        try
        {
            using (StreamReader sr = new StreamReader(Filename))
            {
                int i = 0;
                while (sr.Peek() >= 0) // reading the live data if exists
                {
                    string str = sr.ReadLine();
                    if (i++ >= rows)
                    {
                        if (str != null)
                        { 
                            ShowStatusMessage(str);
                            rows += 1;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error updating status from file");
        }
        return rows;
    }


    protected void OnRadRecordClicked (object sender, EventArgs e)
    {
        OnRecoredModeClicked(sender,e);
    }

    protected void OnRadTransmitClicked (object sender, EventArgs e)
    {
        OnTransmitModeClicked(sender,e);
    }

    protected void OnBbtnStopClicked(object sender, EventArgs e)
    {
        //ShowStatusMessage("This is the" + con++ + " time");
        try
        {
            ShowStatusMessage("Button <stop> pressed");
            if (_currentProcess != null)
            {
                _currentProcess.Kill();
                while (_currentProcess != null && !_currentProcess.HasExited )
                {

                }
                ShowStatusMessage(_currentProcess.ProcessName + " Has terminated.");
                logger.Warn(_currentProcess.ProcessName + " Has terminated.");
                //_currentProcess = null;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error while kill process");
        }
    }

    private void ShowStatusMessage(string msg)
    {
        try
        {
            Gtk.Application.Invoke(delegate
            {
                uint content_id = statusbar.GetContextId(msg);
                statusbar.Push(content_id, msg);
            });
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Show message error");
        }
    }

    private bool KillRunningProcess()
    {
        try
        {
            if (_currentProcess != null)
            {
                _currentProcess.Kill();
                while (!_currentProcess.HasExited)
                {

                }

                ShowStatusMessage(_currentProcess.ProcessName + " Has terminated.");
                logger.Warn(_currentProcess.ProcessName + " Has terminated.");
                _currentProcess = null;
                return true;
            }

        }
        catch (Exception ex) 
        {
            logger.Error(ex, "Kill process error");
        }
        return false;
    }
}
