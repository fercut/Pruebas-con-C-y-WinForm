using System;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    public partial class PROGRAMA_FERMIN : Form
    {
        public PROGRAMA_FERMIN()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            salida.Text = string.Empty; // es igual que ""
            //salida.Text = Environment.OSVersion.ToString();
            ObtenerUsuarioEquipo();
            ObtenerUnidades();
            ObtenerIPs();
            ComprobarCarga();
            ObtenerRAM();

        }
        private void ObtenerUsuarioEquipo()
        {
            string usuario = SystemInformation.UserName;
            string dominio = SystemInformation.UserDomainName;

            salida.Text = "Usuario: " + usuario + Environment.NewLine 
                + "Dominio/Equipo: " + dominio + Environment.OSVersion.ToString();
        }
        private void ObtenerUnidades()
        {
            DriveInfo[] drives = DriveInfo
                .GetDrives()
                .Where(disco => disco.DriveType == DriveType.Fixed)
                .ToArray();
            foreach (DriveInfo drive in drives)
            {
                double espacioLibre = drive.TotalFreeSpace;
                double espacioTotal = drive.TotalSize;

                double espacioLibrePorcentaje = (espacioLibre / espacioTotal) * 100;

                salida.Text += Environment.NewLine + drive.Name + ": " + espacioLibrePorcentaje + " % " 
                    ;
            }
        }
        private void ObtenerIPs()
        {
            IPAddress[] direcciones =
                Dns.GetHostAddresses(Dns.GetHostName())
                .Where(a => !a.IsIPv6LinkLocal)
                .ToArray();

            foreach (IPAddress direccion in direcciones)
            {
                salida.Text += Environment.NewLine + "IP: " + direccion.ToString();
            }
        }
        private void ComprobarCarga()
        {
            Type pw = typeof(PowerStatus);
            PropertyInfo[] propiedades = pw.GetProperties();
            object valor = propiedades[0].GetValue(SystemInformation.PowerStatus, null);

            salida.Text += Environment.NewLine + valor.ToString() + Environment.NewLine 
                + "El Sistema Operativo es: " + Environment.OSVersion.ToString();
        }
        private void ObtenerRAM()
        {
            ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");

            ManagementObjectSearcher managementeObject = new ManagementObjectSearcher(objectQuery);

            ManagementObjectCollection collection = managementeObject.Get();

            foreach(ManagementObject elemento in collection)
            {
                decimal memoriaTotal = 
                    Math.Round(Convert.ToDecimal(elemento["TotalVisibleMemorySize"]) / (1024 * 1024), 2);

                decimal memoriaLibre =
                    Math.Round(Convert.ToDecimal(elemento["FreePhysicalMemory"]) / (1024 * 1024), 2);

                salida.Text += Environment.NewLine + "Memoria total: " + memoriaTotal + " GB";
                salida.Text += Environment.NewLine + "Memoria libre: " + memoriaLibre + " GB";
                string arquitectura = elemento["OSArchitecture"].ToString();

                salida.Text += Environment.NewLine + arquitectura;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}