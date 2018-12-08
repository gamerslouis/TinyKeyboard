using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyKeyboard
{
    class CenterControl
    {
        public ProfileContainer profileContainer = new ProfileContainer();
        private MessageScheduler messageScheduler = new MessageScheduler();
        private SerialPortDetector serialPortDetector = new SerialPortDetector();
        public SerialPortMessageReceiver serialPortMessage { get; set; }

        public Form1 form;

        public CenterControl()
        {
            form = new Form1(new CenterControlGUIMessage(this));

            if(!System.IO.File.Exists(GlobalSetting.ProfileLocation))
            {
                profileContainer.Create();
                if(!profileContainer.Save())
                {
                    System.Windows.Forms.MessageBox.Show("錯誤"
                    , "重建" + GlobalSetting.ProfileLocation + "失敗"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Error);
                    throw new CannotUnloadAppDomainException();
                }
            }

            else if (!profileContainer.Load())
            {
                System.Windows.Forms.MessageBox.Show("錯誤"
                    , GlobalSetting.ProfileLocation + "無法開啟或失敗"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Error);
                throw new CannotUnloadAppDomainException();
            }

            serialPortDetector.PortsChanged += PortsChanged;
            messageScheduler.ProfileChanged += ProfileChanged;

            // Apply the default profile in the profile file
            messageScheduler.handlers
                = MakeHandlers(profileContainer.jSONProfiles[profileContainer.CurrentProfileIndex]);

            // Check if the device have connected before application start
            var truePortName = ScanPorts();
            if(truePortName !="")
            {
                SetSerialPortMessage(truePortName);
            }
        }

        //Trigger when user change Profile on Keyboard
        private void ProfileChanged(object sender, int e)
        {
            if (e < profileContainer.jSONProfiles.Length)
            {
                profileContainer.CurrentProfileIndex = e;
                messageScheduler.handlers
                = MakeHandlers(profileContainer.jSONProfiles[profileContainer.CurrentProfileIndex]);
            }
        }

        public void reloadProfile()
        {
            ProfileChanged(null, profileContainer.CurrentProfileIndex);
        }

        //Check If KeyBoard is Online,init serialMessage
        public string ScanPorts()
        {
            foreach (var portName in serialPortDetector.GetAvailableSerialPortNames())
            {
                if (portName == GlobalSetting.ArduinoName)
                {
                    return portName;
                }
            }
            return "";
        }

        //Use COM name to init SerialPortMessageObject and Start Listen
        private void SetSerialPortMessage(string port)
        {
            if (serialPortMessage != null) serialPortMessage.Dispose();
            serialPortMessage = new SerialPortMessageReceiver(new System.IO.Ports.SerialPort(port));
            serialPortMessage.SerialPortMessageReceived += messageScheduler.ScheduleFuction;
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
                foreach (var port in e.SerialPortNames)
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
                var x = ModeFactory.Get(profile.jSONModes[i].Name);
                handlers[i] = x.CreateHanlder(profile.jSONModes[i].Set);
            }
            return handlers;
        }
    }
    public class CenterControlInitFailException : Exception { }
}