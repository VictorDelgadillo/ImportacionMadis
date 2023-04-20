using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Siged_Spatial_ImportExportData
{
    public static  class Alimentador
    {
        public static void importaAlimentadores(string pDivision, string pZona, string circuito, NpgsqlCommand command,int UTM)
        {
            DataTable dt = new DataTable();
            Conexion conn = new Conexion();
          
            double[] pointsl;

            decimal px, py;
            string StrSQL, sql;

            /* OBTENEMOS LOS DATOS POR transformador
            =================================================== */

            string coordenada, idsiged;
                        
            DateTime fecha_insercion = new DateTime();
            DateTime fecha_actualizacion = new DateTime();

            StrSQL = "SELECT x, y, identificador, descripcion, max_reac, min_reac, max_cto, emerg, color, fec_ins, fec_act  " +
                    "FROM nodo_fuente WHERE div='" + pDivision + "' AND zona='" + pZona + "' AND circuito = '" + circuito + "' ";
            dt = conn.obtenerDT(StrSQL, pDivision);
            
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    idsiged = row["identificador"].ToString();
                    if (row["fec_ins"] != DBNull.Value)
                        fecha_insercion = Convert.ToDateTime(row["fec_ins"]);
                    //else
                      //  fecha_insercion = Convert.ToDateTime("1900-01-01");
                    if (row["fec_act"] != DBNull.Value)
                        fecha_actualizacion = Convert.ToDateTime(row["fec_act"]);
                    //else
                      //  fecha_actualizacion = fecha_insercion;
                                                
                    px = (decimal)row["x"];
                    py = (decimal)row["y"];
                                
                    pointsl = Converter.UTMXYToLatLon((double)px, (double)py, UTM, false);

                    px = (decimal)Converter.RadToDeg(pointsl[0]);
                    py = (decimal)Converter.RadToDeg(pointsl[1]);

                    coordenada = String.Format("﻿ST_GeomFromText('POINT({0} {1})', 4326)", py, px);
                    sql = "INSERT INTO alimentador (idsiged, division, zona, circuito, descripcion, max_cap_react, min_cap_react, cap_maxima, cap_emergencia, color, fecha_insercion, fecha_actualizacion, coordenada) " +
                        "values ('" + idsiged + "', '" + pDivision + "','" + pZona + "','" + circuito + "', '" + row["descripcion"].ToString().Trim() + "', " + row["max_reac"].ToString() + ", " + row["min_reac"].ToString() + ", " +
                        " " + row["max_cto"].ToString() + ", " + row["emerg"].ToString() + ", " + row["color"].ToString() + ", to_date('" + fecha_insercion + "','YY-MM-DD'),to_date('" + fecha_actualizacion + "','YY-MM-DD'), " +
                        "" + coordenada.TrimStart() + ") ﻿RETURNING id";

                    sql = sql.Replace("\ufeff", " ");
                    // INSERTA POSTE dfjdsgfrg
                    command.CommandText = sql;
                    // REGRESA EL ID DEL POSTE INSERTADO ggrgrgt
                    //Int64 id = Convert.ToInt64(command.ExecuteScalar());
                    command.ExecuteNonQuery();
                    
                } // Fin TRY
                catch (Exception msg)
                {
                    // something went wrong, and you wanna know why
                    Console.WriteLine(msg.ToString());
                }
                //postes.Add(new Poste(pDivision, pZona, nombrezona, circuito, identificador, numero, (double)px, (double)py, material, Convert.ToInt16(row["altura"]), Convert.ToInt16(row["resistencia"]), fecha_ins, cantidad));
            } // fin de foreach datarows

        } // fin de postes

    }// fin de la clase Poste
}
