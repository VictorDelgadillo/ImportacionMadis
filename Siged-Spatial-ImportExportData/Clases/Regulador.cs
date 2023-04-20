using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Siged_Spatial_ImportExportData
{
    public static class Regulador
    {
        public static void importaReguladores(string pDivision, string pZona, string circuito, NpgsqlCommand command, int UTM)
        { 
            DataTable dt, dt2 = new DataTable();
            Conexion conn = new Conexion();
            
            double[] pointsl;

            decimal px, py;
            string StrSQL;

            /* OBTENEMOS LOS DATOS POR transformador
            =================================================== */

            string coordenada, loc;

            string sql = "";

            decimal rotacion;

            DateTime fecha_insercion = new DateTime();
            DateTime fecha_actualizacion = new DateTime();

            StrSQL = "select a.nx as x, a.ny as y, a.identificador, capac, a.loc, a.observaciones, a.fec_ins, a.fec_act " +
                    "FROM regulador_m a, linea_lp_m b, deflex_lp_m d " +
                    "WHERE a.div='" + pDivision + "' and a.zona='" + pZona + "' and ((a.nx=b.x2 and a.ny=b.y2) or (a.nx=b.x1 and a.ny=b.y1)) " +
                    "and b.div=a.div and b.zona=a.zona and circuito = '" + circuito + "' " +
                    "and d.lx1=b.x1 and d.ly1=b.y1 and d.lx2=b.x2 and d.ly2=b.y2 and a.x=d.x and a.y=d.y";
            dt = conn.obtenerDT(StrSQL, pDivision);
            
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    //identifi = row["identificador"].ToString();
                    if (row["fec_ins"] != DBNull.Value)
                        fecha_insercion = Convert.ToDateTime(row["fec_ins"]);
                    //else
                      //  fecha_insercion = Convert.ToDateTime("1900-01-01");
                    if (row["fec_act"] != DBNull.Value)
                        fecha_actualizacion = Convert.ToDateTime(row["fec_act"]);
                    //else
                      //  fecha_actualizacion = fecha_insercion;
                
                    if (row["loc"] != DBNull.Value)
                    {
                        loc = row["loc"].ToString();
                        rotacion = ((int)Convert.ToChar(loc) - 65);
                        rotacion = rotacion / 30 * 180;
                    
                    }
                    else
                        rotacion = 0;
                                
                    px = (decimal)row["x"];
                    py = (decimal)row["y"];
                                
                    pointsl = Converter.UTMXYToLatLon((double)px, (double)py, UTM, false);
                
                    px = (decimal)Converter.RadToDeg(pointsl[0]);
                    py = (decimal)Converter.RadToDeg(pointsl[1]);
                
                    coordenada = String.Format("﻿ST_GeomFromText('POINT({0} {1})', 4326)", py, px);
                    sql = "INSERT INTO regulador (idsiged, division, zona, circuito, capacidad, rotacion, observaciones, fecha_insercion, fecha_actualizacion, coordenada) " +
                        "values ('" + row[2].ToString().Trim() + "', '" + pDivision + "','" + pZona + "','" + circuito + "',  " + row["capac"].ToString() + ", " + rotacion + ", '" + row["observaciones"].ToString().Trim() + "', " +
                        "to_date('" + fecha_insercion + "','YY-MM-DD'),to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ") ﻿RETURNING id";

                    sql = sql.Replace("\ufeff", " ");
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                                        
                } // Fin TRY
                catch (Exception msg)
                {
                    // something went wrong, and you wanna know why
                    Console.WriteLine(msg.ToString());
                }
                //postes.Add(new Poste(pDivis    ion, pZona, nombrezona, circuito, identificador, numero, (double)px, (double)py, material, Convert.ToInt16(row["altura"]), Convert.ToInt16(row["resistencia"]), fecha_ins, cantidad));
            } // fin de foreach datarows

        } // fin de postes

    }// fin de la clase Poste
}
