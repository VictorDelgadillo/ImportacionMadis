using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siged_Spatial_ImportExportData
{
    public static class CuchillaAt
    {

        public static void importaCuchilla(string pDivision, string pZona, NpgsqlCommand command, int UTM)
        {
            DataTable dt = new DataTable();
            Conexion conn = new Conexion();

            double[] pointsl;

            decimal cx, cy;
            string StrSQL;

            /* CUCHILLAS ALTA TENSION
            =================================================== */

            StrSQL = "SELECT * FROM m_cuchillas WHERE div='"+pDivision + "' AND zona='"+ pZona +"'";

            dt = conn.obtenerDT(StrSQL, pDivision);

            string division, zona,linea,num_econ,marca,tipo_cuch,tipo_uso,tipo_const,tipo_mont,observaciones,coordenada;

          
            Int16 idmadis,num_torre,consecutivo;
            //DateTime fecha_inst = new DateTime();
            DateTime fecha_ins = new DateTime();
            //  DateTime fechamed = new DateTime();

            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    idmadis = Convert.ToInt16(row["num_cuchilla"]);
                    num_torre = Convert.ToInt16(row["num_torre"]);
                    consecutivo = Convert.ToInt16(row["consecutivo"]);
                    linea = row["circuito"].ToString();
                    division = row["div"].ToString();
                    zona = row["zona"].ToString();
                    num_econ = row["num_econ"].ToString();

                    marca = row["marca"].ToString();
                    tipo_cuch = row["tipo_cuch"].ToString();
                    tipo_uso = row["tipo_uso"].ToString();

                    tipo_const = row["tipo_const"].ToString();
                    tipo_mont = row["tipo_mont"].ToString();

                    observaciones = row["obs"].ToString();

               
                    cx = (decimal)row["cx"];
                    cy = (decimal)row["cy"];

                    pointsl = Converter.UTMXYToLatLon((double)cx, (double)cy, UTM, false);

                    cx = (decimal)Converter.RadToDeg(pointsl[0]);
                    cy = (decimal)Converter.RadToDeg(pointsl[1]);

                    coordenada = String.Format("﻿ST_GeomFromText('POINT({0} {1})', 4326)", cy, cx);
                    string sql = "INSERT INTO cuchilla_at (idmadis, num_torre, division, zona, linea, " +
                    " consecutivo,num_econ,marca,tipo_cuch,tipo_uso,tipo_const,tipo_mont, " +
                    " observaciones,coordenada) " +
                    String.Format(" Values ({0}, {1}, '{2}', '{3}', '{4}', " +
                    " {5}, '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', " +
                    " '{12}',{13})",

                    idmadis, num_torre, division, zona, linea,
                    consecutivo, num_econ, marca, tipo_cuch, tipo_uso, tipo_const, tipo_mont,
                    observaciones,coordenada) + " ﻿RETURNING id";
                    sql = sql.Replace("\ufeff", " ");

                    // INSERTA CUCHILLA
                    command.CommandText = sql;
                    //  command.ExecuteNonQuery();

                    // REGRESA EL ID DE LA CUCHILLA INSERTADA
                    Int64 id = Convert.ToInt64(command.ExecuteScalar());
                    // INSERTA EL CONSECUTIVO POR CIRCUITO
                    //sql = "INSERT INTO torre_cons (idtorre,consecutivo,linea) " +
                    //String.Format("Values ({0}, {1} , '{2}')", id, consecutivo, linea);
                    //command.CommandText = sql;
                    //command.ExecuteNonQuery();

                }
                catch (Exception msg)
                {
                    // something went wrong, and you wanna know why
                    Console.WriteLine(msg.ToString());
                }

            } // fin de foreach datarows

        } // fin de TORRES
    }
}
