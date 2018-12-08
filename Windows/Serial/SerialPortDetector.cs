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
        private string[] _serialPortNames;

        private ManagementEventWatcher arrival;

        private ManagementEventWatcher removal;

        public SerialPortDetector()
        {
            _serialPortNames = GetAvailableSerialPortNames();
            DeviceChangesMonitorInit();
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
        
        // Start linsten to port change event  
        public void Start()
        {
            arrival.Start();
            removal.Start();
        }

        // Stop linsten to port change event
        public void Stop()
        {
            arrival.Stop();
            removal.Stop();
        }

        // Event Handlers for port change evnet
        public event EventHandler<PortsChangedArgs> PortsChanged;

        public string[] GetAvailableSerialPortNames()
        {
            return SerialPort.GetPortNames();
        }

        private void DeviceChangesMonitorInit()
        {

            var deviceArrivalQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
            var deviceRemovalQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3");

            arrival = new ManagementEventWatcher(deviceArrivalQuery);
            removal = new ManagementEventWatcher(deviceRemovalQuery);

            arrival.EventArrived += (o, args) => RaisePortsChangedIfNecessary(EventType.Insertion);
            removal.EventArrived += (sender, eventArgs) => RaisePortsChangedIfNecessary(EventType.Removal);
        }

        private void RaisePortsChangedIfNecessary(EventType eventType)
        {
            lock (_serialPortNames)
            {
                var availableSerialPorts = GetAvailableSerialPortNames();
                if (!_serialPortNames.SequenceEqual(availableSerialPorts))
                {
                    _serialPortNames = availableSerialPorts;
                    PortsChanged?.Invoke(this, new PortsChangedArgs(eventType, _serialPortNames));
                }
            }
        }
    }
}

public enum EventType
{
    Insertion, // Connect to pc
    Removal, // Remove from pc
}

public class PortsChangedArgs : EventArgs
{
    public PortsChangedArgs(EventType eventType, string[] serialPortNames)
    {
        EventType = eventType;
        SerialPortNames = serialPortNames;
    }

    public string[] SerialPortNames { get; }

    public EventType EventType { get; }
}

