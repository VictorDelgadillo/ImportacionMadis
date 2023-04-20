using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siged_Spatial_ImportExportData
{
    public static class RegistroAt
    {

        public static void importaRegistrosAt(string pDivision, string pZona, NpgsqlCommand command, int UTM)
        {
            DataTable dt = new DataTable();
            Conexion conn = new Conexion();

            double[] pointsl;

            decimal rx, ry;
            string StrSQL;

            /* REGISTROS SUBTERRANEOS ALTA TENSION
            =================================================== */

            StrSQL =" SELECT * " +
                    " FROM m_sub_registros r " +
                    " INNER JOIN m_sub_conseregistros c ON r.num_reg=c.num_reg " +
                    " WHERE  c.div='" + pDivision + "' and c.zona='"+ pZona +"'  order by consecutivo,linea";

            dt = conn.obtenerDT(StrSQL, pDivision);

            string division, zona, utilizacion, tipo_reg, tipo_emp, soporteria, observaciones, marca, caja,linea, coordenada;

            decimal altura, altura_real, resori, respos, resact, profelect, resterreno, angulo;
            Int64 num_reg, cant_lineas, cant_emp, num_vias, num_empalmes, consecutivo;
            //DateTime fecha_inst = new DateTime();
            DateTime fecha_inst = new DateTime();
            //  DateTime fechamed = new DateTime();

            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    num_reg = Convert.ToInt32(row["num_reg"]);
                    consecutivo = Convert.ToInt16(row["consecutivo"]);
                    linea = row["linea"].ToString();
                    division = row["div"].ToString();
                    zona = row["zona"].ToString();
                    cant_lineas = Convert.ToInt16(row["cant_circ"]);
                    utilizacion = row["utilizacion"].ToString();
                    tipo_reg = row["tipo_reg"].ToString();
                    cant_emp = Convert.ToInt16(row["cant_emp"]);

                    tipo_emp = row["tipo_emp"].ToString();
                    soporteria = row["soporteria"].ToString();
                    observaciones = row["observaciones"].ToString();

                    if (row["marca"] != DBNull.Value)
                    {
                        marca = row["marca"].ToString();
                    }else
                    {
                        marca = "";
                    }
                       
                    if (row["num_vias"] != DBNull.Value)
                    {
                        num_vias = Convert.ToInt16(row["num_vias"]);
                    }
                    else
                    {
                        num_vias = 0;
                    }

                    if (row["num_empalmes"] != DBNull.Value)
                    {
                        num_empalmes = Convert.ToInt16(row["num_empalmes"]);
                    }
                    else
                    {
                        num_empalmes = 0;
                    }

                    if (row["caja"] != DBNull.Value)
                    {
                        caja = row["caja"].ToString();
                    }
                    else
                    {
                        caja = "N";
                    }


                    if (row["fecha"] != DBNull.Value)
                    {
                        fecha_inst = Convert.ToDateTime(row["fecha"]);
                    }

                 
                    rx = (decimal)row["rx"];
                    ry = (decimal)row["ry"];

                    pointsl = Converter.UTMXYToLatLon((double)rx, (double)ry, UTM, false);

                    rx = (decimal)Converter.RadToDeg(pointsl[0]);
                    ry = (decimal)Converter.RadToDeg(pointsl[1]);

                    coordenada = String.Format("﻿ST_GeomFromText('POINT({0} {1})', 4326)", ry, rx);
                    string sql = "INSERT INTO registro_at (idmadis, cant_lin,utilizacion,tipo_reg,cant_emp, " +
                                 "soporteria,marca,num_vias,num_empalmes,caja," +
                                 "observaciones,fecha_ins,coordenada) " +
                    String.Format(" Values ({0}, {1}, '{2}', '{3}', {4}, " +
                                  " '{5}', '{6}', {7}, {8}, '{9}', '{10}',to_date('{11}','YY-MM-DD'),{12})",
                                num_reg, cant_lineas, utilizacion,tipo_reg,cant_emp,
                                soporteria,marca, num_vias,num_empalmes,caja,
                                observaciones, fecha_inst, coordenada) + " ﻿RETURNING id";
                                sql = sql.Replace("\ufeff", " ");

                    // INSERTA REGISTRO SUBTERRANEO
                    command.CommandText = sql;
                    //  command.ExecuteNonQuery();

                    // REGRESA EL ID DEL REGISTRO INSERTADO
                    Int64 idreg = Convert.ToInt64(command.ExecuteScalar());
                    // INSERTA EL CONSECUTIVO POR CIRCUITO
                    sql = "INSERT INTO registro_at_cons (idreg,consecutivo,linea,div,zona,fecha_ins) " +
                    String.Format("Values ({0}, {1} , '{2}' , '{3}' , '{4}' , to_date('{5}','YY-MM-DD'))", idreg, consecutivo, linea, division, zona, fecha_inst);
                    command.CommandText = sql;
                    command.ExecuteNonQuery();

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
