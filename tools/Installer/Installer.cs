using Microsoft.Win32;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MockingjayApp.Installer
{
    [RunInstaller(true)]
    public class Installer : System.Configuration.Install.Installer
    {
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                RegistryKey myKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Environment", true);
                if (myKey != null)
                {
                    var old = myKey.GetValue("Path");
                    myKey.SetValue("Path", old + ";" + path, RegistryValueKind.String);
                    myKey.Close();
                }
            }
        }
    }
}
