using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Siged_Spatial_ImportExportData
{
    public static class TransPartAereo
    {
        public static void importaTransPartAereo(string pDivision, string pZona, string circuito, NpgsqlCommand command,int UTM)
        {
            DataTable dt, dt2 = new DataTable();
            Conexion conn = new Conexion();
          
            double[] pointsl;

            decimal px, py;
            string StrSQL, sql;

            /* OBTENEMOS LOS DATOS POR transformador
            =================================================== */

            string loc, coordenada, fases, fas, tconex, tconex2, idtr;
            decimal rotacion, cap;
            Int16 ntrans;

            DateTime fecha_insercion = new DateTime();
            DateTime fecha_actualizacion = new DateTime();

            StrSQL = "SELECT a.x, a.y, a.identificador, ntrans, total, cfa, cfb, cfc, tconex, fases_con, a.loc, tipo[1,1] as tipo, fec_ins, a.fec_act, observaciones " +
                    "FROM trans_par_m a, nodo_lp_m b WHERE b.div='" + pDivision + "' AND b.zona='" + pZona + "' AND circuito = '" + circuito + "' " +
                    "AND a.x=b.x and a.y=b.y";
            dt = conn.obtenerDT(StrSQL, pDivision);
            
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    idtr = row["identificador"].ToString().Trim();
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
                        rotacion = (decimal)((((int)Convert.ToChar(loc) - 65) / 30) * 180);
                    }
                    else
                        rotacion = 0;
                
                    ntrans=(Int16)row["ntrans"];
                    fases = row["fases_con"].ToString();

                    tconex = row["tconex"].ToString();
                                
                    px = (decimal)row["x"];
                    py = (decimal)row["y"];
                                
                    pointsl = Converter.UTMXYToLatLon((double)px, (double)py, UTM, false);

                    px = (decimal)Converter.RadToDeg(pointsl[0]);
                    py = (decimal)Converter.RadToDeg(pointsl[1]);
                                
                    coordenada = String.Format("﻿ST_GeomFromText('POINT({0} {1})', 4326)", py, px);
                                        
                    if (tconex.Contains("DE") || tconex.Contains("YE") || tconex.Contains("YA") || tconex.Contains("YZ") || tconex.Contains("TE")) 
                    {
                        if (ntrans == 1)
                        {
                            fas = "ABC";
                            sql = "INSERT INTO trpar_a (idsiged, division, zona, circuito, capacidad, tipo_conexion, fases, rotacion, tipo, observaciones, fecha_insercion, fecha_actualizacion, coordenada ) " +
                            "values ('" + idtr + "', '" + pDivision + "', '" + pZona + "', '" + circuito + "', " + row["total"].ToString() + ", '" + tconex + "', '" + fas + "', " +
                            "" + rotacion.ToString() + ",  '" + row[11].ToString() + "', '" + row["observaciones"].ToString().Trim() + "', to_date('" + fecha_insercion + "','YY-MM-DD'), " +
                            "to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ")";
                            sql = sql.Replace("\ufeff", " ");
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            cap = (decimal)row["cfa"];
                            //ne  = row["ne1"].ToString().Trim();
                            fas = "A";

                            tconex2="ME";
                            sql = "INSERT INTO trpar_a (idsiged, division, zona, circuito, capacidad, tipo_conexion, fases, rotacion, tipo, observaciones, fecha_insercion, fecha_actualizacion, coordenada ) " +
                            "values ('" + idtr + "', '" + pDivision + "', '" + pZona + "', '" + circuito + "', " + cap.ToString() + ", '" + tconex2 + "', '" + fas + "', " +
                            "" + rotacion.ToString() + ",  '" + row[11].ToString() + "', '" + row["observaciones"].ToString().Trim() + "', to_date('" + fecha_insercion + "','YY-MM-DD'), " +
                            "to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ")";
                            sql = sql.Replace("\ufeff", " ");
                            command.CommandText = sql;
                            command.ExecuteNonQuery();

                            cap = (decimal)row["cfb"];
                            fas = "B";

                            rotacion = rotacion + 120;

                            sql = "INSERT INTO trpar_a (idsiged, division, zona, circuito, capacidad, tipo_conexion, fases, rotacion, tipo, observaciones, fecha_insercion, fecha_actualizacion, coordenada ) " +
                            "values ('" + idtr + "', '" + pDivision + "', '" + pZona + "', '" + circuito + "', " + cap.ToString() + ", '" + tconex2 + "', '" + fas + "', " +
                            "" + rotacion.ToString() + ",  '" + row[11].ToString() + "', '" + row["observaciones"].ToString().Trim() + "', to_date('" + fecha_insercion + "','YY-MM-DD'), " +
                            "to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ")";
                            sql = sql.Replace("\ufeff", " ");
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                                                        
                            cap = (decimal)row["cfc"];
                            fas = "C";

                            rotacion = rotacion + 120;
                            
                            sql = "INSERT INTO trpar_a (idsiged, division, zona, circuito, capacidad, tipo_conexion, fases, rotacion, tipo, observaciones, fecha_insercion, fecha_actualizacion, coordenada ) " +
                            "values ('" + idtr + "', '" + pDivision + "', '" + pZona + "', '" + circuito + "', " + cap.ToString() + ", '" + tconex2 + "', '" + fas + "', " +
                            "" + rotacion.ToString() + ",  '" + row[11].ToString() + "', '" + row["observaciones"].ToString().Trim() + "', to_date('" + fecha_insercion + "','YY-MM-DD'), " +
                            "to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ")";
                            sql = sql.Replace("\ufeff", " ");
                            command.CommandText = sql;
                            command.ExecuteNonQuery();

                        
                        }
                                                
                    }
                    else if (tconex.Contains("MD") || tconex.Contains("ME"))
                    {
                        if (fases.Substring(0, 1).Contains("1"))
                        {
                            cap = (decimal)row["cfa"];
                            //ne = row["ne1"].ToString().Trim();
                            if (tconex.Contains("MD"))
                                fas = "AB";
                            else
                                fas = "A";
                        }
                        else if (fases.Substring(1, 1).Contains("1"))
                        {
                            cap = (decimal)row["cfb"];
                            //ne = row["ne2"].ToString().Trim();
                            if (tconex.Contains("MD"))
                                fas = "BC";
                            else
                                fas = "B";
                        }
                        else
                        {
                            cap = (decimal)row["cfc"];
                            //ne = row["ne3"].ToString().Trim();
                            if (tconex.Contains("MD"))
                                fas = "AC";
                            else
                                fas = "C";
                        }
                        
                        sql = "INSERT INTO trpar_a (idsiged, division, zona, circuito, capacidad, tipo_conexion, fases, rotacion, tipo, observaciones, fecha_insercion, fecha_actualizacion, coordenada ) " +
                            "values ('" + idtr + "', '" + pDivision + "', '" + pZona + "', '" + circuito + "', " + cap.ToString() + ", '" + tconex + "', '" + fas + "', " +
                            "" + rotacion.ToString() + ",  '" + row[11].ToString() + "', '" + row["observaciones"].ToString().Trim() + "', to_date('" + fecha_insercion + "','YY-MM-DD'), " +
                            "to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ")";
                        sql = sql.Replace("\ufeff", " ");
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                    else if (tconex.Contains("DA") || tconex.Contains("YI"))
                    {
                        if (tconex.Contains("DA"))
                            tconex2 = "MD";
                        else
                            tconex2 = "ME";
                        
                        if (fases.Substring(0, 1).Contains("1"))
                        {
                            cap = (decimal)row["cfa"];
                            //ne  = row["ne1"].ToString().Trim();
                            if (tconex.Contains("DA"))
                                fas = "AB";
                            else
                                fas = "A";
                            
                            sql = "INSERT INTO trpar_a (idsiged, division, zona, circuito, capacidad, tipo_conexion, fases, rotacion, tipo, observaciones, fecha_insercion, fecha_actualizacion, coordenada ) " +
                            "values ('" + idtr + "', '" + pDivision + "', '" + pZona + "', '" + circuito + "', " + cap.ToString() + ", '" + tconex2 + "', '" + fas + "', " +
                            "" + rotacion.ToString() + ",  '" + row[11].ToString() + "', '" + row["observaciones"].ToString().Trim() + "', to_date('" + fecha_insercion + "','YY-MM-DD'), " +
                            "to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ")";
                            sql = sql.Replace("\ufeff", " ");
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                            
                            if (fases.Substring(1, 1).Contains("1"))
                            {
                                cap = (decimal)row["cfb"];
                                if (tconex.Contains("DA"))
                                    fas = "BC";
                                else
                                    fas = "B";
                            }
                            else
                            {
                                cap = (decimal)row["cfc"];
                                if (tconex.Contains("MD"))
                                    fas = "AC";
                                else
                                    fas = "C";
                            }

                            rotacion = rotacion + 180;

                            sql = "INSERT INTO trpar_a (idsiged, division, zona, circuito, capacidad, tipo_conexion, fases, rotacion, tipo, observaciones, fecha_insercion, fecha_actualizacion, coordenada ) " +
                            "values ('" + idtr + "', '" + pDivision + "', '" + pZona + "', '" + circuito + "', " + cap.ToString() + ", '" + tconex2 + "', '" + fas + "', " +
                            "" + rotacion.ToString() + ",  '" + row[11].ToString() + "', '" + row["observaciones"].ToString().Trim() + "', to_date('" + fecha_insercion + "','YY-MM-DD'), " +
                            "to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ")";
                            sql = sql.Replace("\ufeff", " ");
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }
                        else if (fases.Substring(1, 1).Contains("1"))
                        {
                            cap = (decimal)row["cfb"];
                            if (tconex.Contains("DA"))
                                fas = "BC";
                            else
                                fas = "B";

                            sql = "INSERT INTO trpar_a (idsiged, division, zona, circuito, capacidad, tipo_conexion, fases, rotacion, tipo, observaciones, fecha_insercion, fecha_actualizacion, coordenada ) " +
                            "values ('" + idtr + "', '" + pDivision + "', '" + pZona + "', '" + circuito + "', " + cap.ToString() + ", '" + tconex2 + "', '" + fas + "', " +
                            "" + rotacion.ToString() + ",  '" + row[11].ToString() + "', '" + row["observaciones"].ToString().Trim() + "', to_date('" + fecha_insercion + "','YY-MM-DD'), " +
                            "to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ")";
                            sql = sql.Replace("\ufeff", " ");
                            command.CommandText = sql;
                            command.ExecuteNonQuery();

                            if (fases.Substring(2, 1).Contains("1"))
                            {
                                cap = (decimal)row["cfc"];
                                if (tconex.Contains("DA"))
                                    fas = "AC";
                                else
                                    fas = "C";
                            }

                            sql = "INSERT INTO trpar_a (idsiged, division, zona, circuito, capacidad, tipo_conexion, fases, rotacion, tipo, observaciones, fecha_insercion, fecha_actualizacion, coordenada ) " +
                            "values ('" + idtr + "', '" + pDivision + "', '" + pZona + "', '" + circuito + "', " + cap.ToString() + ", '" + tconex2 + "', '" + fas + "', " +
                            "" + rotacion.ToString() + ",  '" + row[11].ToString() + "', '" + row["observaciones"].ToString().Trim() + "', to_date('" + fecha_insercion + "','YY-MM-DD'), " +
                            "to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ")";
                            sql = sql.Replace("\ufeff", " ");
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }
                    
                    } // FIN DE IF ELSES DE TIPOS DE CONEXION
                                       

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
