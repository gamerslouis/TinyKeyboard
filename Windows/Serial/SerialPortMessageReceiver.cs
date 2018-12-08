using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace TinyKeyboard
{
    class SerialPortMessageReceiver : IDisposable
    {
        private SerialPort comport;

        // Serial port name
        public string name { get { return comport.PortName; } }

        public event EventHandler<byte> SerialPortMessageReceived;

        public Exception lastError;

        // Thread for Mainloop to listen to port message
        private System.Threading.Thread thread;

        public SerialPortMessageReceiver(SerialPort comport)
        {
            this.comport = comport;
            thread = new System.Threading.Thread(MainLoop);
            thread.IsBackground = true;
        }

        public bool StartRead()
        {
            try
            {
                if (!comport.IsOpen)
                {
                    comport.Open();
                }
                thread.Start();
                return true;
            }
            catch (Exception e)
            {
                this.lastError = e;
                return false;
            }

        }

        public void EndRead()
        {
            if (comport.IsOpen)
            {
                comport.Close();
            }
            thread.Abort();
        }

        private void MainLoop()
        {
            while (true)
            {
                if (comport.BytesToRead > 0)
                {
                    var key = ReadMessage();
                    SerialPortMessageReceived?.Invoke(this, key);
                }
                else System.Threading.Thread.Sleep(10);
            }
        }

        private byte ReadMessage()
        {
            byte[] buffer = new byte[1];
            comport.Read(buffer, 0, buffer.Length);
            System.Diagnostics.Debug.WriteLine(buffer[0].ToString());
            return buffer[0];
        }

        #region IDisposable Support
        private bool disposedValue = false; // 偵測多餘的呼叫

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    comport.Close();
                    SerialPortMessageReceived = null;
                    // TODO: 處置受控狀態 (受控物件)。
                }

                // TODO: 釋放非受控資源 (非受控物件) 並覆寫下方的完成項。
                // TODO: 將大型欄位設為 null。

                disposedValue = true;
            }
        }

        // TODO: 僅當上方的 Dispose(bool disposing) 具有會釋放非受控資源的程式碼時，才覆寫完成項。
        // ~SerialPortMessage() {
        //   // 請勿變更這個程式碼。請將清除程式碼放入上方的 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 加入這個程式碼的目的在正確實作可處置的模式。
        public void Dispose()
        {
            // 請勿變更這個程式碼。請將清除程式碼放入上方的 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果上方的完成項已被覆寫，即取消下行的註解狀態。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
