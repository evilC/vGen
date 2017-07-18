using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using vGenInterfaceWrap;

public class vGenBasicWrapper
{
    public _vJoy vJoy = new _vJoy();
    public _vXbox vXbox = new _vXbox();
    public static vGen vGenWrapper = new vGenInterfaceWrap.vGen();

    ~vGenBasicWrapper()
    {
        vJoy.RelinquishAll();
        vXbox.RelinquishAll();
    }

    public class _vJoy : Joystick
    {
        public override DevType DeviceType { get { return DevType.vJoy; } }

        public _vJoy()
        {
            Devices = new vGenDevice[8];
            bool exists;
            for (uint i = 0; i < 8; i++)
            {
                exists = false;
                vGenWrapper.isDevExist(i+1, DevType.vJoy, ref exists);
                if (exists)
                {
                    Devices[i] = new vGenDevice(DeviceType, i+1);
                }
                // else? Leave array item null?
            }
        }
    }

    public class _vXbox : Joystick
    {
        public override DevType DeviceType { get { return DevType.vXbox; } }

        public _vXbox()
        {
            Devices = new vGenDevice[4];
            for (uint i = 0; i < 4; i++)
            {
                Devices[i] = new vGenDevice(DeviceType, i);
            }
        }
    }

    public abstract class Joystick
    {
        public vGenDevice[] Devices { get; set; }
        public abstract DevType DeviceType { get; }

        public void RelinquishAll()
        {
            for (var i = 0; i < Devices.Length; i++)
            {
                if (Devices[i] != null)
                {
                    Devices[i].Relinquish();
                }
            }
        }

        public vGenDevice Device(uint id)
        {
            try
            {
                return Devices[id - 1];
            }
            catch
            {
                return null;
            }
        }
    }

    public class vGenDevice
    {
        private DevType DeviceType;
        private bool IsAcquired = false;
        private int hDev = 0;
        private uint DeviceId;
        private bool IsXboxType;

        public vGenDevice(DevType devType, uint id)
        {
            DeviceType = devType;
            DeviceId = id;
            IsXboxType = DeviceType == DevType.vXbox;
        }

        ~vGenDevice()
        {
            if (IsAcquired)
            {
                Relinquish();
            }
        }

        public bool Acquire()
        {
            var id = (IsXboxType ? DeviceId + 1 : DeviceId);
            var ret = vGenWrapper.AcquireDev(id, DeviceType, ref hDev);
            // vGen quirk - if xBox type and you Acquire (plug in) then send update, the update never happens...
            // ... also, trying to change the axis to that value later does not update the axis (it must be cached and ignoring)
            if (IsXboxType)
            {
                Thread.Sleep(1000);
            }
            IsAcquired = (ret == 0);
            return IsAcquired;
        }

        public bool Relinquish()
        {
            var ret = vGenWrapper.RelinquishDev(hDev);
            if (ret == 0)
            {
                IsAcquired = false;
            }
            return (ret == 0);
        }

        public bool SetAxis(uint axis, float value)
        {
            if (!IsAcquired && !Acquire())
            {
                return false;
            }
            var ret = vGenWrapper.SetDevAxis(hDev, axis, value);
            return (ret == 0);
        }

        public bool SetButton(uint button, bool value)
        {
            if (!IsAcquired && !Acquire())
            {
                return false;
            }
            var ret = vGenWrapper.SetDevButton(hDev, button, value);
            return (ret == 0);
        }

        public bool SetPov(uint pov, float value)
        {
            if (!IsAcquired && !Acquire())
            {
                return false;
            }
            var ret = vGenWrapper.SetDevPov(hDev, pov, value);
            return (ret == 0);
        }

    }
}
