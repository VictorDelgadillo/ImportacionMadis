using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Siged_Spatial_ImportExportData
{
    public partial class MainForm : Form
    {
   
        Conexion conn = new Conexion();
        public static int UTM = 0;
        public static double[] pointsl;
        public static double[] pointsd;

        public MainForm()
        {
            InitializeComponent();
        }
        
        private void btnExportarImportar_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            Conexion connIfx = new Conexion();
            lblMensaje.Text = "Importando!!!!!";
            string division = "";
          //  string zona = txtZona.Text;
            string circuito,zona;
            string StrSQL;
            //circuitos[0] = txtSe.Text;
            LineaPrimaria linea = new LineaPrimaria();
            Poste poste = new Poste();
            TransCfeAereo trcfea = new TransCfeAereo();

            Division item = (Division)cbDivision.SelectedItem;
            division = item.Value.ToString();

            string connstring = String.Format("Server={0};Port={1};" +
                  "User Id={2};Password={3};Database={4};",
                  "10.4.22.81", "5432", "postgres",
                  "manager", "postgres");

            // Conexion a Postgres
            NpgsqlConnection conn = new NpgsqlConnection(connstring);
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = conn;
            conn.Open();
                         
            //StrSQL = "SELECT ciare, cisub, cicir FROM siad:circuitos WHERE cidiv='" + division + "' and ciare like '%' AND cisub like '%' and cists = 'S' and cistatus = 'O'" ; // and cicir='04010' 
             
           //StrSQL = "SELECT ciare, cisub, cicir FROM siad:circuitos WHERE cidiv='" + division + "' and ciare = '07' AND " +
           //            "cisub = 'TPO' and cicir = '04050' and cists='S' and cistatus='O' order by 1,2,3";

            //  MessageBox.Show(item.Value.ToString());
            // not in ('AME', 'BOQ')
            //StrSQL = "SELECT ciare, cisub, cicir FROM siad:circuitos WHERE cidiv='" + division + "' and ciare = '03' AND " +
            //             "cisub = 'COO' and cists='S' and cistatus='O' order by 1,2,3"; // and cicir='04010' 

            //StrSQL = "SELECT ciare, cisub, cicir FROM siad:circuitos WHERE cidiv='" + division + "' and ciare = '01' AND " +
            //      "cisub = 'HLC' and cicir='04050' and cists='S' and cistatus='O' order by 1,2,3"; // and cicir='04010' 

            StrSQL = "SELECT ciare, cisub, cicir FROM siad:circuitos WHERE cidiv='" + division + "' and ciare like '%' AND " +
                       "cisub like '%' and cists='S' and cistatus='O' order by 1,2,3"; // and cicir='04010' 
            
            //StrSQL = "SELECT ciare, cisub, cicir FROM siad:circuitos WHERE cidiv='" + division + "' and ciare like '%' AND " +
            //           "cisub like '%' and cists='S' and cistatus='O' order by 1,2,3"; // and cicir='04010' 

            //StrSQL = "SELECT ciare, cisub, cicir FROM siad:circuitos WHERE cidiv='" + division + "' and ciare = '01' AND " +
            //          "cisub = 'HLC' and cicir='04020' and cists='S' and cistatus='O' order by 1,2,3"; // and cicir='04010' 

        //   StrSQL = "SELECT ciare, cisub, cicir FROM siad:circuitos WHERE cidiv='" + division + "' and ciare = '01' AND " +
          //              "cisub = 'HLC' and cicir='04090' and cists='S' and cistatus='O' order by 1,2,3"; // and cicir='04010' 
           
          //  MessageBox.Show(item.Value.ToString());

            dt = connIfx.obtenerDT(StrSQL, division);
            foreach (DataRow row in dt.Rows)
            {
                circuito = row["cisub"].ToString() + row["cicir"].ToString();
                zona = row["ciare"].ToString();

                StrSQL = "SELECT utm FROM m_utm WHERE ardiv='" + division + "' AND arare='" + zona + "'";
                dt = connIfx.obtenerDT(StrSQL, division);

                foreach (DataRow r in dt.Rows)
                {
                    UTM = Convert.ToInt16(r["utm"]);
                }
                
                if (chbAlimentadores.Checked)
                {
                    Alimentador.importaAlimentadores(division, zona, circuito, command, UTM);
                }

                if (chbLineaPrimaria.Checked)
                {
                    linea.importaLineasMta(division, zona , circuito, command, UTM);
                }
                
                if (chbPostes.Checked)
                {
                    poste.importaPostes(division, zona, circuito, command, UTM);
                }
                if (chbTransformadores.Checked) {
                    trcfea.importaTransCfeAereo(division, zona, circuito, command, UTM);
                }
                if (chbCapacitores.Checked)
                {
                    Capacitor.importaCapacitores(division, zona, circuito, command, UTM);
                }
                if (chbDesconectadores.Checked)
                {
                    Desconectador.importaDesconectadores(division, zona, circuito, command, UTM);
                }

                if (chbRestauradores.Checked)
                {
                    Restaurador.importaRestauradores(division, zona, circuito, command, UTM);
                }

                if (chbFusibles.Checked)
                {
                    Fusible.importaFusibles(division, zona, circuito, command, UTM);
                }

                if (chbReguladores.Checked)
                {
                    Regulador.importaReguladores(division, zona, circuito, command, UTM);
                }

                if (chbTransfParticularesA.Checked)
                {
                    TransPartAereo.importaTransPartAereo(division, zona, circuito, command, UTM);
                }
                if (chbLinea_mts.Checked)
                {
                    Linea_mts.importa(division, zona, circuito, command, UTM);
                }

                if (chbTransfCfeSubt.Checked)
                {
                     TransfCfeSubterraneo.importa(division, zona, circuito, command, UTM);
                }

                if (chbRegistrosMt.Checked)
                {
                    RegistroMt.importaRegistrosMt(division, zona, circuito, command, UTM);
                }

                if (chbTransicionesBT.Checked)
                {
                    TransicionBTSubterranea.importa(division, zona, circuito, command, UTM);
                }

                if (chbRegistrosBT.Checked)
                {
                    RegistroBt.importaRegistrosBt(division, zona, circuito, command, UTM);
                }

                if (chbTransicionesMT.Checked)
                {
                    TransicionMT.importaTransicionesMT(division, zona, circuito, command, UTM);
                }

                if (chbAcometidaMT.Checked)
                {
                    Acometida_mts.importa(division, zona, circuito, command, UTM);
                }

                if (chbSeccionadores.Checked)
                {
                  //  SeccionadorSubt.importaSeccionadorSubt(division, zona, circuito, command, UTM);
                }

                // Linea_mts.importa(division, row["ciare"].ToString(), circuito, command);


            } // Fin de Foreach (string pCircuito in circuitos) 


            lblMensaje.Text = "Importación terminada!!";          
            conn.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cbDivision.DisplayMember = "Text";
            cbDivision.ValueMember = "Value";

            cbDivision.Items.Add(new Division("DA", "BAJA CALIFORNIA"));
            cbDivision.Items.Add(new Division("DB", "NOROESTE"));
            cbDivision.Items.Add(new Division("DC", "NORTE"));
            cbDivision.Items.Add(new Division("DD", "GOLFO NORTE"));
            cbDivision.Items.Add(new Division("DP", "BAJIO"));
            cbDivision.Items.Add(new Division("DX", "JALISCO"));
            cbDivision.Items.Add(new Division("DU", "GOLFO CENTRO"));
            cbDivision.Items.Add(new Division("DF", "CENTRO OCCIDENTE"));
            cbDivision.Items.Add(new Division("DL", "VALLE MEXICO NORTE"));
            cbDivision.Items.Add(new Division("DM", "VALLE MEXICO CENTRO"));
            cbDivision.Items.Add(new Division("DN", "VALLE MEXICO SUR"));
            cbDivision.Items.Add(new Division("DG", "CENTRO SUR"));
            cbDivision.Items.Add(new Division("DV", "CENTRO ORIENTE"));
            cbDivision.Items.Add(new Division("DJ", "ORIENTE"));
            cbDivision.Items.Add(new Division("DK", "SURESTE"));
            cbDivision.Items.Add(new Division("DW", "PENINSULAR"));          
            cbDivision.SelectedIndex = 0;
           

        }

        private void cbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get zonas 
            //Division item = (Division)cbDivision.SelectedItem;
            //string division = item.Value.ToString();
            //Zona.getZonas(division);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void chbSeccionadores_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnExportarImportarMadis_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            Conexion connIfx = new Conexion();
            Division item = (Division)cbDivision.SelectedItem;
            String division = item.Value.ToString();

            string connstring = String.Format("Server={0};Port={1};" +
                  "User Id={2};Password={3};Database={4};",
                  "10.4.22.81", "5432", "9eakj",
                  "$9j54944690*", "postgres");

            // Conexion a Postgres
            NpgsqlConnection conn = new NpgsqlConnection(connstring);
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = conn;
            conn.Open();

            string zona;

          // string StrSQL =" select arare, arnom from siad: areasres where ardiv = '"+division+"'  and arare<> '**' and arare<> '* ' and arare <> '00 '  order by arare";
            string StrSQL = " select arare, arnom from siad: areasres where ardiv = 'DB'   and arare ='05'  order by arare";


            dt = connIfx.obtenerDT(StrSQL, division);
            foreach (DataRow row in dt.Rows)
            {
                zona = row["arare"].ToString();

                StrSQL = "SELECT DISTINCT(utm) FROM m_utm WHERE ardiv='" + division + "' AND arare='" + zona + "'";
                dt = connIfx.obtenerDT(StrSQL, division);

                foreach (DataRow r in dt.Rows)
                {
                    UTM = Convert.ToInt16(r["utm"]);
                }
                if(UTM > 0)
                {
                    if (chbTorres.Checked){
                        Torre.importaTorres(division, zona, command, UTM);
                    }
                    if (chbLineasAT.Checked){
                        LineaAltaTension.importaLineasAta(division, zona, command, UTM);
                    }

                    if (chbLineasATSubt.Checked)
                    {
                        LineaAltaTensionSubt.importaLineasAtsubt(division, zona, command, UTM);
                    }

                    if (chbCuchillaSubt.Checked)
                    {
                        CuchillaAt.importaCuchilla(division, zona, command, UTM);                      
                    }

                    if (chbRegistrosSubt.Checked)
                    {
                        RegistroAt.importaRegistrosAt(division, zona, command, UTM);
                    }

                     if (chbTransicionesSubt.Checked)
                    {
                        TransicionAt.importaTransicionesAt(division, zona, command, UTM);
                    }
                }
            }

            lblMensaje.Text = "El proceso de importacion a Madis ha terminado";


        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
