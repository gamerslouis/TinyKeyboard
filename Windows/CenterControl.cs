using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyKeyboard
{
    class CenterControl
    {
        public ProfileContainer profileContainer;
        MessageScheduler messageScheduler;
        SerialPortDetector serialPortDetector;
        public SerialPortMessage serialPortMessage { get; set; }

        Form1 form;
        System.Windows.Forms.NotifyIcon notifyIcon;

        public CenterControl()
        {
            messageScheduler = new MessageScheduler();
            serialPortDetector = new SerialPortDetector();
            profileContainer = new ProfileContainer();

            if (!profileContainer.Load())
            {
                System.Windows.Forms.MessageBox.Show("錯誤"
                    , "無法開啟或重建" + GlobalSetting.ProfileLocation
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Error);
                throw new CannotUnloadAppDomainException();
            }

            serialPortDetector.PortsChanged += PortsChanged;
            form.VisibleChanged += Form_VisibleChanged;
            messageScheduler.ProfileChanged += ProfileChanged;

            messageScheduler.handlers
                = MakeHandlers(profileContainer.jSONProfiles[profileContainer.ProfileIndex]);

            ScanPorts();

            notifyIcon.Visible = true;
            notifyIcon.Text = "TinyKeyboard";
            notifyIcon.Click += notifyIconClick;
            var cms = new System.Windows.Forms.MenuItem[1];
            cms[1] = new System.Windows.Forms.MenuItem("Exit", (sender, e) =>
             {
                 System.Windows.Forms.Application.Exit();
             });
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(cms);


        }

        //Show notify when form closed
        private void Form_VisibleChanged(object sender, EventArgs e)
        {
            if (((Form1)sender).Visible == false)
            {
                notifyIcon.Visible = true;
            }
        }

        //Show form and hide notify when notify clicked
        private void notifyIconClick(object sender, EventArgs e)
        {
            form.Show();
            notifyIcon.Visible = false;
        }

        //Trigger when user change Profile on Keyboard
        private void ProfileChanged(object sender, int e)
        {
            e--;
            if (e < profileContainer.jSONProfiles.Length)
            {
                profileContainer.ProfileIndex = e;
                messageScheduler.handlers
                = MakeHandlers(profileContainer.jSONProfiles[profileContainer.ProfileIndex]);
            }
        }

        //Check If KeyBoard is Online,init serialMessage
        public void ScanPorts()
        {
            foreach (var port in serialPortDetector._serialPorts)
            {
                if (port == GlobalSetting.ArduinoName)
                {
                    SetSerialPortMessage(port);
                }
            }
        }

        //Use COM name to init SerialPortMessageObject and Start Listen
        private void SetSerialPortMessage(string port)
        {
            if (serialPortMessage != null) serialPortMessage.Dispose();
            serialPortMessage = new SerialPortMessage(new System.IO.Ports.SerialPort(port));
            serialPortMessage.SerialPortReceived += messageScheduler.ScheduleFuction;
            serialPortMessage.StartRead();
        }

        //Catch event from detector if COM is connect or dis connect from pc
        private void PortsChanged(object sender, PortsChangedArgs e)
        {
            if (e.EventType == EventType.Insertion)
            {
                if (serialPortMessage == null)
                {
                    ScanPorts();
                }
            }
            else
            {
                serialPortMessage.EndRead();
                var foundFlag = false;
                foreach (var port in e.SerialPorts)
                {
                    if (port == serialPortMessage.name)
                    {
                        foundFlag = true;
                        break;
                    }
                }

                if (!foundFlag)
                {
                    serialPortMessage.Dispose();
                    serialPortMessage = null;
                }
                else
                {
                    serialPortMessage.StartRead();
                }
            }
        }

        //Make handlers for scheduler from profiles
        private Handler.IHandler[] MakeHandlers(JSONProfile profile)
        {
            Handler.IHandler[] handlers = new Handler.IHandler[GlobalSetting.KeyMaxNumber];
            for (int i = 0; i < handlers.Length; i++)
            {
                handlers[i] = ModeFactory.Get(profile.jSONModes[i].Name).CreateHanlder(profile.jSONModes[i].Set);
            }
            return handlers;
        }
    }
    public class CenterControlInitFailException : Exception { }
}
