using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Contracts;
using System.IO.Ports;
using System.Management;


namespace TinyKeyboard
{
    class SerialPortDetector
    {
        public string[] _serialPorts { get; protected set; }

        private ManagementEventWatcher arrival;

        private ManagementEventWatcher removal;

        public SerialPortDetector()
        {
            _serialPorts = GetAvailableSerialPorts();
            MonitorDeviceChanges();
        }

        /// <summary>
        /// If this method isn't called, an InvalidComObjectException will be thrown (like below):
        /// System.Runtime.InteropServices.InvalidComObjectException was unhandled
        ///Message=COM object that has been separated from its underlying RCW cannot be used.
        ///Source=mscorlib
        ///StackTrace:
        ///     at System.StubHelpers.StubHelpers.StubRegisterRCW(Object pThis, IntPtr pThread)
        ///     at System.Management.IWbemServices.CancelAsyncCall_(IWbemObjectSink pSink)
        ///     at System.Management.SinkForEventQuery.Cancel()
        ///     at System.Management.ManagementEventWatcher.Stop()
        ///     at System.Management.ManagementEventWatcher.Finalize()
        ///InnerException: 
        /// </summary>
        public void CleanUp()
        {
            arrival.Stop();
            removal.Stop();
        }

        public event EventHandler<PortsChangedArgs> PortsChanged;

        private void MonitorDeviceChanges()
        {
            try
            {
                var deviceArrivalQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
                var deviceRemovalQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3");

                arrival = new ManagementEventWatcher(deviceArrivalQuery);
                removal = new ManagementEventWatcher(deviceRemovalQuery);

                arrival.EventArrived += (o, args) => RaisePortsChangedIfNecessary(EventType.Insertion);
                removal.EventArrived += (sender, eventArgs) => RaisePortsChangedIfNecessary(EventType.Removal);

                // Start listening for events
                arrival.Start();
                removal.Start();
            }
            catch (ManagementException err)
            {

            }
        }

        private void RaisePortsChangedIfNecessary(EventType eventType)
        {
            lock (_serialPorts)
            {
                var availableSerialPorts = GetAvailableSerialPorts();
                if (!_serialPorts.SequenceEqual(availableSerialPorts))
                {
                    _serialPorts = availableSerialPorts;
                    PortsChanged?.Invoke(this, new PortsChangedArgs(eventType, _serialPorts));
                }
            }
        }

        public string[] GetAvailableSerialPorts()
        {
            return SerialPort.GetPortNames();
        }
    }
}

public enum EventType
{
    Insertion,
    Removal,
}

public class PortsChangedArgs : EventArgs
{
    public PortsChangedArgs(EventType eventType, string[] serialPorts)
    {
        EventType = eventType;
        SerialPorts = serialPorts;
    }

    public string[] SerialPorts { get; }

    public EventType EventType { get; }
}

