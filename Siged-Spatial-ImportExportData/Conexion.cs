using System;
using System.Data;
using IBM.Data.Informix;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Siged_Spatial_ImportExportData
{
    public class Conexion
    {
        public IfxCommand cmd;
        public IfxConnection conn;
        public IfxDataAdapter da;
        //string sqlIsolation = "set isolation to dirty read";

        public OdbcCommand cmdODBC;
        public OdbcConnection connODBC;
        public OdbcDataAdapter daODBC;

        private DataSet ds = new DataSet();
        //
        // TODO: Add constructor logic here
        //
        public Conexion()
        {
        }



        public void Abrir(String division)
        {
            try
            {
              
                cmd = new IfxCommand();
                conn = new IfxConnection(obtenerCadena(division));

                conn.Open();

                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 100000000;
                cmd.CommandText = "set isolation to dirty read";
                cmd.ExecuteNonQuery();
              
            }
            catch (Exception ex)
            {
            }
        }

        public void AbrirODBC(String Servidor)
        {
            try
            {
                cmdODBC = new OdbcCommand();
                connODBC = new OdbcConnection(obtenerCadena(Servidor));

                connODBC.Open();

                cmdODBC.Connection = connODBC;

                cmdODBC.CommandType = CommandType.Text;

            }
            catch (Exception ex)
            {
            }
        }

        public void Cerrar()
        {
            try
            {
                conn.Close();
                conn.Dispose();
                da.Dispose();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        public void CerrarOdbc()
        {
            try
            {
                connODBC.Close();
                connODBC.Dispose();
                daODBC.Dispose();
                cmdODBC.Dispose();
            }
            catch (Exception ex)
            {
            }
        }


        public string obtenerCadena(String division)
        {
            string _connString = "";
            switch (division)
            {
                case "DA":
                    _connString = "Host=10.4.6.3;Service=1526;Server=bajacalifornia_net;Database=sigedw;User Id=sigedw;Password=sigedw;Pooling=true;Max Pool Size=200;Enlist=true";
                    break;
                case "DB":
                    // _connString = "Host=10.4.13.3;Service=1526;Server=noroeste_net;Database=sigedw;User Id=consulta;Password=consulta;Pooling=true;Max Pool Size=200;Enlist=true";
                    // _connString = "Server=noroeste_net;Database=sigedw;User ID=consulta;Password=consulta;Persist Security Info=True;Authentication=Server;Pooling=false;Protocol=onsoctcp;Host=dbsun.db0.cfemex.com;Service=1526";
                    _connString = "dsn=SIE_ODBC;UID=sigedw;PWD=wG5B4xy";

                    break;
                case "DC":
                    _connString = "Host=10.4.14.3;Service=1526;Server=norte_net;Database=sigedw;User Id=sigedw;Password=sigedw;Pooling=true;Max Pool Size=200;Enlist=true";
                    break;
                case "DD":
                    _connString = "Host=10.4.22.200;Service=1526;Server=golfonorte_net;Database=sigedw;User Id=sigedw;Password=sigedw;Pooling=true;Max Pool Size=200;Enlist=true";
                    break;
                case "DP":
                    _connString = "Host=10.4.12.3;Service=1526;Server=bajio_net;Database=sigedw;User Id=sigedw;Password=sigedw;Pooling=true;Max Pool Size=200;Enlist=true";
                    break;
                case "DX":
                    _connString = "Host=10.4.15.3;Service=1526;Server=jalisco_net;Database=sigedw;User Id=sigedw;Password=sigedw;Pooling=true;Max Pool Size=200;Enlist=true";
                    break;
                case "DU":
                    _connString = "Host=10.4.11.3;Service=1526;Server=golfocentro_net;Database=sigedw;User Id=sigedw;Password=sigedw;Pooling=true;Max Pool Size=200;Enlist=true";
                    break;
                case "DF":
                    _connString = "Host=10.4.8.3;Service=1526;Server=centrooccidente_net;Database=sigedw;User Id=sigedw;Password=sigedw;Pooling=true;Max Pool Size=200;Enlist=true";
                    break;
                case "DL":
                    _connString = "Host=10.4.59.3;Service=1526;Server=vmnorte_net;Database=sigedw;User Id=sigedw;Password=sigedw;Pooling=true;Max Pool Size=200;Enlist=true";
                    break;
                case "DM":
                    _connString = "Host=10.4.57.3;Service=1526;Server=vmcentro_net;Database=sigedw;User Id=sigedw;Password=sigedw;Pooling=true;Max Pool Size=200;Enlist=true";
                    break;
                case "DN":
                    _connString = "Host=10.4.61.3;Service=1526;Server=vmsur_net;Database=sigedw;User Id=sigedw;Password=sigedw;Pooling=true;Max Pool Size=200;Enlist=true";
                    break;
                case "DG":
                    _connString = "Host=10.4.10.3;Service=1526;Server=centrosur_net;Database=sigedw;User Id=sigedw;Password=sigedw;Pooling=true;Max Pool Size=200;Enlist=true";
                    break;
                case "DV":
                    _connString = "Host=10.4.9.3;Service=1526;Server=centrooriente_net;Database=sigedw;User Id=sigedw;Password=ppwhkx25R;Pooling=true;Max Pool Size=200;Enlist=true";
                    break;
                case "DJ":
                    _connString = "Host=10.4.16.3;Service=1526;Server=oriente_net;Database=sigedw;User Id=sigedw;Password=sigedw;Pooling=true;Max Pool Size=200;Enlist=true";
                    break;
                case "DK":
                    _connString = "Host=10.4.18.3;Service=1526;Server=sureste_net;Database=sigedw;User Id=sigedw;Password=sigedw;Pooling=true;Max Pool Size=200;Enlist=true";
                    break;
                case "DW":
                    _connString = "Host=10.4.17.3;Service=1526;Server=peninsular_net;Database=sigedw;User Id=sigedw;Password=sigedw;Pooling=true;Max Pool Size=200;Enlist=true";
                    break;
                default:
                    _connString = "";
                    break;
            }
            return _connString;
        }

        public DataTable obtenerDTresp(String query,String division)
        {
                       
            Abrir(division);
           
            da = new IfxDataAdapter(query, conn);
            da.SelectCommand.CommandTimeout = 100000000;
            DataSet ds = new DataSet();
            da.Fill(ds);
            Cerrar();

            return ds.Tables[0];
        }

        public DataTable obtenerDT(String query, String division)
        {

            AbrirODBC(division);

            daODBC = new OdbcDataAdapter(query, connODBC);
            daODBC.SelectCommand.CommandTimeout = 100000000;
            DataSet ds = new DataSet();
            daODBC.Fill(ds);
            CerrarOdbc();

            return ds.Tables[0];
        }

        public bool ejecutarQry(String query)
        {
            try
            {
                Abrir("siged");
                cmd = new IfxCommand(query, conn);
                cmd.CommandTimeout = 10000000;
                cmd.ExecuteNonQuery();
              
                Cerrar();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ejecutarProcedure(String query)
        {
            try
            {
                Abrir("siged");
                cmd = new IfxCommand(query, conn);
                cmd.CommandTimeout = 10000000;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataTable obtenerDTsiad(String query)
        {

            Abrir("siad");
            da = new IfxDataAdapter(query, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Cerrar();

            return ds.Tables[0];
        }

        public bool ejecutarQrysiad(String query)
        {
            try
            {
                Abrir("siad");
                cmd = new IfxCommand(query, conn);
                cmd.ExecuteNonQuery();
                Cerrar();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataTable obtenerDTmysql(String query)
        {

            Abrir("mysql");
            da = new IfxDataAdapter(query, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Cerrar();

            return ds.Tables[0];
        }

        public bool ejecutarQryMysql(String query)
        {
            try
            {
                Abrir("mysql");
                cmd = new IfxCommand(query, conn);
                cmd.ExecuteNonQuery();
                Cerrar();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
