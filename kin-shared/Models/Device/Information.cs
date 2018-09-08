using System.Reflection;

namespace Kin.Shared.Models.Device
{
    public class Information
    {
        public string XDeviceId { get; }
        public string XDeviceManufacturer { get; }
        public string XDeviceModel { get; }
        public string XOs { get; }
        public string XSdkVersion { get; }


        public Information(string xDeviceId = "KINTIPBOT", string xDeviceModel = "Samsung9+",string xDeviceManufacturer = "Samsung", string xOs = "Android")
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            AssemblyName assemblyName = assembly.GetName();
            string assemblyVersion = assembly.ImageRuntimeVersion;

            XDeviceId = xDeviceId;
            XDeviceModel = xDeviceModel;
            XDeviceManufacturer = xDeviceManufacturer;
            XOs = xOs;
            XSdkVersion = $"{assemblyName}-{assemblyVersion}";
        }
    }
}