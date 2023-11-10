using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Actualizador
{
    public partial class Actualizador : Form
    {

        public Actualizador()
        {
            InitializeComponent();
        }
        private void PlActualizar()
        {
            foreach (Process proceso in Process.GetProcesses())
            {
                if (proceso.ProcessName == "ModuloConexionesRemotas")
                {
                    proceso.Kill();
                }
            }
        }
        private void PlIniciarAplicacion()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = txtRuta.Text+ "\\ModuloConexionesRemotas.exe";
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            Process processTemp = new Process();
            processTemp.StartInfo = startInfo;
            processTemp.EnableRaisingEvents = true;
            try
            {
                processTemp.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            this.btnActualizar.Enabled = false;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 3;
            progressBar1.Value = 1;
            PlActualizar();
            Thread.Sleep(6000);
            WebClient mywebClient = new WebClient();
            mywebClient.DownloadFile("http://repositorio.appsmercadodeimportaciones.com/SistemaConexionesRemotas/ModuloConexionesRemotas.exe",""+txtRuta.Text+"\\ModuloConexionesRemotas.exe");
            mywebClient.DownloadFile("http://repositorio.appsmercadodeimportaciones.com/SistemaConexionesRemotas/ModuloConexionesRemotas.exe.config","" + txtRuta.Text + "\\ModuloConexionesRemotas.exe.config");
            progressBar1.Value = 2;
            mywebClient.DownloadFile("http://repositorio.appsmercadodeimportaciones.com/SistemaConexionesRemotas/MySql.Data.dll", "" + txtRuta.Text + "\\MySql.Data.dll");
            progressBar1.Value = 3;
            this.btnActualizar.Enabled = true;
            MessageBox.Show("Modulo actualizado correctamente.", "Remoto", MessageBoxButtons.OK, MessageBoxIcon.Information);
            PlIniciarAplicacion();
            progressBar1.Value = 0;
            this.Dispose();
        }

        private void btnBuscarRuta_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtRuta.Text = fbd.SelectedPath;
        }

        private void Actualizador_Load(object sender, EventArgs e)
        {
            txtRuta.Text = Directory.GetCurrentDirectory();
        }
    }
}
