using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyKeyboard
{
    class CenterControl
    {
        ProfileContainer profileContainer;
        MessageScheduler messageScheduler;
        SerialPortDetector serialPortDetector;
        SerialPortMessage serialPortMessage;

        public CenterControl()
        {
            messageScheduler = new MessageScheduler();
            serialPortDetector = new SerialPortDetector();
            profileContainer = new ProfileContainer();

            profileContainer.Load();
            messageScheduler.ProfileChanged += ProfileChanged;

            messageScheduler.handlers 
                = MakeHandlers(profileContainer.jSONProfiles[profileContainer.ProfileIndex]); 

            ///
            //profileload
            ///
            serialPortDetector.PortsChanged += PortsChanged;

            foreach (var port in serialPortDetector._serialPorts)
            {
                if (port == GlobalSetting.ArduinoName)
                {
                    SetSerialPortMessage(port);
                }
            }

        }

        private void ProfileChanged(object sender, int e)
        {
            e--;
            if(e<profileContainer.jSONProfiles.Length)
            {
                profileContainer.ProfileIndex = e;
                messageScheduler.handlers
                = MakeHandlers(profileContainer.jSONProfiles[profileContainer.ProfileIndex]);
            }
        }

        private void SetSerialPortMessage(string port)
        {
            serialPortMessage = new SerialPortMessage(new System.IO.Ports.SerialPort(port));
            serialPortMessage.SerialPortReceived += messageScheduler.ScheduleFuction;
        }

        private void PortsChanged(object sender, PortsChangedArgs e)
        {
            if (e.EventType == EventType.Insertion)
            {
                if (serialPortMessage == null)
                {
                    foreach (var port in e.SerialPorts)
                    {
                        if (port == GlobalSetting.ArduinoName)
                        {
                            SetSerialPortMessage(port);
                        }
                    }
                }
            }
            else
            {
                var foundFlag = false;
                foreach (var port in e.SerialPorts)
                {
                    if (port == serialPortMessage.name)
                    {
                        serialPortMessage.Dispose();
                        serialPortMessage = null;
                    }
                }
            }
        }

        private Handler.IHandler[] MakeHandlers(JSONProfile profile)
        {
            Handler.IHandler[] handlers = new Handler.IHandler[GlobalSetting.KeyMaxNumber];
            for (int i = 0; i < handlers.Length; i++)
            {
                handlers[i] = ModeFactory.Get(profile.jSONModes[i].Name).CreateHanlder(profile.jSONModes[i].Set);
            }
            return handlers;
        }

        //ui,notify init
        //ui profile set commu
    }
}
