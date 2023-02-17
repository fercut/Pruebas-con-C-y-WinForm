using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            salida.Text = string.Empty; // es igual que ""
            ObtenerUsuarioEquipo();
            ObtenerUnidades();
            ObtenerIPs();
            ComprobarCarga();
        }
        private void ObtenerUsuarioEquipo()
        {
            string usuario = SystemInformation.UserName;
            string dominio = SystemInformation.UserDomainName;

            salida.Text = "Usuario: " + usuario + Environment.NewLine + "Dominio/Equipo: " + dominio;
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

                salida.Text += Environment.NewLine + drive.Name + ": " + espacioLibrePorcentaje + " % " + Environment.NewLine;
            }
        }
        private void ObtenerIPs()
        {
            IPAddress[] direcciones =
                Dns.GetHostAddresses(Dns.GetHostName())
                .Where(a => !a.IsIPv6LinkLocal)
                .ToArray();

            foreach(IPAddress direccion in direcciones)
            {
                salida.Text += Environment.NewLine + "IP: " + direccion.ToString();
            }
        }
        private void ComprobarCarga()
        {
            Type pw = typeof(PowerStatus);
            PropertyInfo[] propiedades = pw.GetProperties();
            object valor = propiedades[0].GetValue(SystemInformation.PowerStatus, null);

            salida.Text += Environment.NewLine + valor.ToString();
        }
    }
}