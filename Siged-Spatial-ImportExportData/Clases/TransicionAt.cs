using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siged_Spatial_ImportExportData
{
    public static class TransicionAt
    {

        public static void importaTransicionesAt(string pDivision, string pZona, NpgsqlCommand command, int UTM)
        {
            DataTable dt = new DataTable();
            Conexion conn = new Conexion();

            double[] pointsl;

            decimal tx, ty;
            string StrSQL;

            /* TRANSICIONES  ALTA TENSION
            =================================================== */

            StrSQL = " SELECT * " +
                    " FROM m_sub_transicion " +
                    " WHERE  div='" + pDivision + "' and zona='" + pZona + "'";

            dt = conn.obtenerDT(StrSQL, pDivision);

            string division, zona, tipo_trans, linea, tipo_terminal, material_terminal, marca_terminal, tipo_aparta, observaciones,coordenada;
        
            Int16 num_trans,num_linea,fases,cant_terminales,cant_aparta,num_linea_sub;
            //DateTime fecha_inst = new DateTime();
            DateTime fecha_inst = new DateTime();
            //  DateTime fechamed = new DateTime();

            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    num_trans = Convert.ToInt16(row["num_trans"]);
                    if (row["num_trans"] != DBNull.Value)
                    {
                        num_trans = Convert.ToInt16(row["num_trans"]);
                    }else
                    {
                        num_trans = 0;
                    }

                    if (row["num_linea"] != DBNull.Value)
                    {
                        num_linea = Convert.ToInt16(row["num_linea"]);
                    }
                    else {
                        num_linea = 0;
                    }

                    if (row["num_linea_sub"] != DBNull.Value)
                    {
                        num_linea_sub = Convert.ToInt16(row["num_linea_sub"]);
                    }
                    else {
                        num_linea_sub = 0;
                    }
                    linea = row["linea"].ToString();
                    division = row["div"].ToString();
                    zona = row["zona"].ToString();
                    tipo_trans = row["tipo_trans"].ToString();
                    fases = Convert.ToInt16(row["fases"]);
                    tipo_terminal = row["tipo_terminal"].ToString();

                    if (row["tipo_aparta"] != DBNull.Value)
                    {
                        tipo_aparta = row["tipo_aparta"].ToString();
                    }else
                    {
                        tipo_aparta = "";
                    }
                    cant_aparta = Convert.ToInt16(row["cant_aparta"]);
                    observaciones = row["observaciones"].ToString();

                    material_terminal = row["material_terminal"].ToString();
                    cant_terminales = Convert.ToInt16(row["cant_terminales"]);
                    marca_terminal  = row["marca_terminal"].ToString();




                    if (row["fecha"] != DBNull.Value)
                    {
                        fecha_inst = Convert.ToDateTime(row["fecha"]);
                    }


                    tx = (decimal)row["trx"];
                    ty = (decimal)row["try"];

                    pointsl = Converter.UTMXYToLatLon((double)tx, (double)ty, UTM, false);

                    tx = (decimal)Converter.RadToDeg(pointsl[0]);
                    ty = (decimal)Converter.RadToDeg(pointsl[1]);

                    coordenada = String.Format("﻿ST_GeomFromText('POINT({0} {1})', 4326)", ty, tx);
                    string sql = "INSERT INTO transicion_at (idmadis, num_linea , num_linea_sub, division,zona, "+
                                 "linea,fases,tipo_terminal,material_terminal,cant_terminales,marca_terminal, " +
                                 "tipo_aparta,cant_aparta,observaciones,fecha_ins,coordenada) " +
                    String.Format(" Values ({0}, {1}, {2}, '{3}', '{4}', " +
                                  " '{5}', {6}, '{7}', '{8}', {9}, '{10}'," +
                                  " '{11}', {12}, '{13}', to_date('{14}','YY-MM-DD'),{15})",
                                num_trans, num_linea, num_linea_sub, division, zona,
                                linea, fases, tipo_terminal, material_terminal, cant_terminales,marca_terminal,
                                tipo_aparta, cant_aparta, observaciones, fecha_inst, coordenada) + " ﻿RETURNING id";
                    sql = sql.Replace("\ufeff", " ");

                    // INSERTA LA TRANSCION
                    command.CommandText = sql;
                    //  command.ExecuteNonQuery();

                    // REGRESA EL ID DE LA TRANSCION
                    Int64 idreg = Convert.ToInt64(command.ExecuteScalar());
                   

                }
                catch (Exception msg)
                {
                    // something went wrong, and you wanna know why
                    Console.WriteLine(msg.ToString());
                }

            } // fin de foreach datarows

        } // fin de TRANSICION
    }
}
