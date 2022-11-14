using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO.Ports;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using LibreHardwareMonitor.Hardware;
using LibreHardwareMonitor.UI;
using System.Xml.Linq;

namespace LibreHardwareMonitor.Utilities
{
    public class Serial
    {
        string _Port;
        public string Port
        {
            get { return _Port; }
            set
            {
                _Port = value;
                Properties.Default.ComName = _Port;
                Properties.Default.Save();

            }
        }
        SerialPort serial;
        public Serial()
        {
            int Number = Properties.Default.BaudRate;
            Port = Properties.Default.ComName;
            serial = new SerialPort(string.IsNullOrEmpty(Port) ? "COM1" : Port, Number);

        }
        public bool Open()
        {
            if (serial.IsOpen)
            {
                try { serial.Close(); }
                catch { }
            }

            serial.PortName = Port;

            try
            {
                serial.Open();
            }
            catch (IOException e)
            {
                return false;
            }

            return true;
        }

        public void Close()
        {
            if (serial.IsOpen)
            {
                serial.Close();
            }
        }

        public bool isOpen()
        {
            return serial.IsOpen;
        }

        public void Write(byte[] data)
        {
            if (serial.IsOpen)
            {
                try
                {
                    serial.Write(data, 0, data.Length);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка отправки данных в COM-порт:\r\n\r\n" + ex.Message);

                    try { serial.Close(); }
                    catch { }

                    Open();
                }
            }
        }

        public void collectAndWrite(Computer computer, PersistentSettings settings, UpdateVisitor updateVisitor)
        {
            List<float> data = this.collectData(computer, settings, updateVisitor);

            string tmp = string.Join(";", data.Select(T => T.ToString()).ToArray());

            this.Write(Encoding.ASCII.GetBytes(tmp));
            this.Write(Encoding.ASCII.GetBytes("E"));
        }

        public List<float> collectData(Computer computer, PersistentSettings settings, UpdateVisitor updateVisitor)
        {
            computer.Accept(updateVisitor);

            List<float> data = new List<float>
            {
                (int)_getSensorValue(computer, HardwareType.Cpu, SensorType.Temperature, "Core Average"),
                (int)_getSensorValue(computer, HardwareType.GpuAmd, SensorType.Temperature, "GPU Core"),
                (int)_getAvgSensorsValue(computer, HardwareType.Motherboard, SensorType.Temperature),
                (int)_getAvgSensorsValue(computer, HardwareType.Storage, SensorType.Temperature),
                (int)_getSensorValue(computer, HardwareType.Cpu, SensorType.Load, "CPU Total"),
                (int)_getSensorValue(computer, HardwareType.GpuAmd, SensorType.Load, "GPU Core"),
                (int)_getSensorValue(computer, HardwareType.Memory, SensorType.Load, "Memory"),
                (int)_getSensorValue(computer, HardwareType.GpuAmd, SensorType.Load, "GPU Memory"),
                (int)_getSensorValue(computer, HardwareType.GpuAmd, SensorType.Load, "Fullscreen FPS"),
            };

            return data;
        }

        public string[] getPortNames()
        {
            return SerialPort.GetPortNames();
        }

        private float _getSensorValue(Computer computer, HardwareType hardwareType, SensorType sensorType, string name)
        {
            int n = 0;
            float p = -1;
            var elements = computer.Hardware.Where(x => x.HardwareType == hardwareType).ToArray();
            if (elements.Count() > 0)
            {
                foreach (var hardware in elements)
                {
                    var values = hardware.Sensors.Where(x => x.Name == name && x.SensorType == sensorType).ToArray();
                    if (values.Count() != 0)
                    {
                        n++;
                        p = values.Average(values => values.Value.Value);
                    }
                }
            }


            return n > 0 ? p / n : 0;
        }

        private float _getAvgSensorsValue(Computer computer, HardwareType hardwareType, SensorType sensorType)
        {
            var elements = computer.Hardware.Where(x => x.HardwareType == hardwareType).ToArray();
            if (elements.Any())
            {
                int n = 0;
                float t = 0;
                foreach (var hardware in elements)
                {
                    var values = hardware.Sensors.Where(x => x.SensorType == sensorType).ToArray();
                    if (values.Any())
                    {
                        t += values.Average(x => x.Value.Value);
                        n++;
                    }

                    foreach (var sh in hardware.SubHardware)
                    {
                        values = sh.Sensors.Where(x => x.SensorType == sensorType).ToArray();
                        if (values.Any())
                        {
                            t += values.Average(x => x.Value.Value);
                            n++;
                        }
                    }
                }

                if (n > 0)
                {
                    return t / (float)n;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
