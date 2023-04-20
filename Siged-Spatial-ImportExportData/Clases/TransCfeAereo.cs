using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Siged_Spatial_ImportExportData
{
    public class TransCfeAereo
    {
        public void importaTransCfeAereo(string pDivision, string pZona, string circuito, NpgsqlCommand command,int UTM)
        {
            DataTable dt, dt2 = new DataTable();
            Conexion conn = new Conexion();
          
            double[] pointsl;

            decimal px, py;
            string StrSQL;

            /* OBTENEMOS LOS DATOS POR transformador
            =================================================== */

            string loc, autoproteg, coordenada, fases, fas, fases_b, tconex, ne1, ne2, ne3, idtr;
            decimal rotacion, cap_total, cap;
            Int16 ntrans;

            DateTime fecha_insercion = new DateTime();
            DateTime fecha_actualizacion = new DateTime();

            StrSQL = "SELECT a.x, a.y, a.identificador, ntrans, ne1, ne2, ne3, total, cfa, cfb, cfc, tconex, fases_con, delec," +
                     "a.loc, autoproteg, fec_ins, a.fec_act, observaciones " +
                    "FROM trans_cfe_m a, nodo_lp_m b WHERE b.div='" + pDivision + "' AND b.zona='" + pZona + "' AND circuito = '" + circuito + "' " +
                    "AND a.x=b.x and a.y=b.y";
            dt = conn.obtenerDT(StrSQL, pDivision);
            
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    idtr = row["identificador"].ToString();
                    if (row["fec_ins"] != DBNull.Value)
                        fecha_insercion = Convert.ToDateTime(row["fec_ins"]);
                    //else
                      //  fecha_insercion = Convert.ToDateTime("1900-01-01");
                    if (row["fec_act"] != DBNull.Value)
                        fecha_actualizacion = Convert.ToDateTime(row["fec_act"]);
                    //else
                      //  fecha_actualizacion = fecha_insercion;

                    autoproteg = (row["autoproteg"].ToString() == "S") ? "S" : "N";

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
                    cap_total=(decimal)row["total"];

                    if (fases.Contains("111"))
                      fases_b = "ABC";
                    else if (fases.Contains("110"))
                      if (tconex.Contains("DA"))
                        fases_b = "ABC";
                      else
                        fases_b="AB";
                    else if (fases.Contains("101"))
                        if (tconex.Contains("DA"))
                            fases_b = "ABC";
                        else
                            fases_b = "AC";
                    else if (fases.Contains("011"))
                        if (tconex.Contains("DA"))
                            fases_b = "ABC";
                        else
                            fases_b = "BC";
                    else if (fases.Contains("100"))
                        if (tconex.Contains("MD"))
                            fases_b = "AB";
                        else
                            fases_b = "A";
                    else if (fases.Contains("010"))
                        if (tconex.Contains("MD"))
                            fases_b = "BC";
                        else
                            fases_b = "B";
                    else 
                        if (tconex.Contains("MD"))
                            fases_b = "AC";
                        else
                            fases_b = "C";
                                
                    px = (decimal)row["x"];
                    py = (decimal)row["y"];
                                
                    pointsl = Converter.UTMXYToLatLon((double)px, (double)py, UTM, false);

                    px = (decimal)Converter.RadToDeg(pointsl[0]);
                    py = (decimal)Converter.RadToDeg(pointsl[1]);
                                    
                    coordenada = String.Format("﻿ST_GeomFromText('POINT({0} {1})', 4326)", py, px);
                     string sql = "INSERT INTO banco_transd (idsiged, division, zona, circuito, num_trans, cap_total, tipo_conexion, fases, num_banco, rotacion, " +
                    "fecha_insercion, fecha_actualizacion, observaciones, coordenada) " +
                    String.Format("Values ('{0}', '{1}' , '{2}' , '{3}' , {4} , {5} ,'{6}','{7}','{8}',{9}, " +
                    "to_date('{10}','YY-MM-DD') , to_date('{11}','YY-MM-DD') , '{12}',{13})",
                    idtr,
                    pDivision,
                    pZona,
                    circuito,
                    ntrans,
                    cap_total,
                    tconex,
                    fases_b,
                    row["delec"].ToString(),
                    rotacion,
                    fecha_insercion,
                    fecha_actualizacion,
                    row["observaciones"].ToString(),
                    coordenada) + " ﻿RETURNING id";
                    sql = sql.Replace("\ufeff", " ");
                    // INSERTA POSTE dfjdsgfrg
                    command.CommandText = sql;
                    // REGRESA EL ID DEL POSTE INSERTADO ggrgrgt
                    Int64 id = Convert.ToInt64(command.ExecuteScalar());
                     
                    // INSERTA LOS TRANSFORMADORES DEL BANCO
                                        
                    if (tconex.Contains("DE") || tconex.Contains("YE") || tconex.Contains("YA") || tconex.Contains("YZ") || tconex.Contains("TE")) 
                    {
                        if (ntrans == 1)
                        {
                            
                            fas = "ABC";
                            cap = cap_total;
                            if (row["ne1"] != DBNull.Value)
                                ne1 = row["ne1"].ToString().Trim();
                            else if (row["ne2"] != DBNull.Value)
                                ne1 = row["ne2"].ToString().Trim();
                            else if (row["ne3"] != DBNull.Value)
                                ne1 = row["ne3"].ToString().Trim();
                            else
                                ne1 = "";

                            sql = "INSERT INTO trcfe_a (idbanco, economico, fases, capacidad, autoprotegido) " +
                            String.Format("Values ({0}, '{1}' , '{2}', {3}, '{4}')", id, ne1, fas, cap, autoproteg);
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            cap = (decimal)row["cfa"];
                            //ne  = row["ne1"].ToString().Trim();
                            fas = "A";

                            if (row["ne1"] != DBNull.Value)
                              ne1 = row["ne1"].ToString().Trim();
                            else
                              ne1 = "";

                            sql = "INSERT INTO trcfe_a (idbanco, economico, fases, capacidad, autoprotegido) " +
                            String.Format("Values ({0}, '{1}' , '{2}', {3}, '{4}')", id, ne1, fas, cap, autoproteg);
                            command.CommandText = sql;
                            command.ExecuteNonQuery();

                            cap = (decimal)row["cfb"];
                            fas = "B";
                            if (row["ne2"] != DBNull.Value)
                                ne2 = row["ne2"].ToString().Trim();
                            else
                                ne2 = "";

                            sql = "INSERT INTO trcfe_a (idbanco, economico, fases, capacidad, autoprotegido) " +
                            String.Format("Values ({0}, '{1}' , '{2}', {3}, '{4}')", id, ne2, fas, cap, autoproteg);
                            command.CommandText = sql;
                            command.ExecuteNonQuery();

                            cap = (decimal)row["cfc"];
                            fas = "C";
                            if (row["ne3"] != DBNull.Value)
                                ne3 = row["ne3"].ToString().Trim();
                            else
                                ne3 = "";

                            sql = "INSERT INTO trcfe_a (idbanco, economico, fases, capacidad, autoprotegido) " +
                            String.Format("Values ({0}, '{1}' , '{2}', {3}, '{4}')", id, ne3, fas, cap, autoproteg);
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

                        if (row["ne1"] != DBNull.Value)
                            ne1 = row["ne1"].ToString().Trim();
                        else if (row["ne2"] != DBNull.Value)
                            ne1 = row["ne2"].ToString().Trim();
                        else if (row["ne3"] != DBNull.Value)
                            ne1 = row["ne3"].ToString().Trim();
                        else
                            ne1 = "";

                        sql = "INSERT INTO trcfe_a (idbanco, economico, fases, capacidad, autoprotegido) " +
                            String.Format("Values ({0}, '{1}' , '{2}', {3}, '{4}')", id, ne1, fas, cap, autoproteg);
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                    else if (tconex.Contains("DA") || tconex.Contains("YI"))
                    {
                        if (row["ne1"] != DBNull.Value)
                        {
                            ne1 = row["ne1"].ToString().Trim();
                            if (row["ne2"] != DBNull.Value)
                                ne2 = row["ne2"].ToString().Trim();
                            else if (row["ne3"] != DBNull.Value)
                                ne2 = row["ne3"].ToString().Trim();
                            else
                                ne2 = "";
                        }
                        else if (row["ne2"] != DBNull.Value)
                        {
                            ne1 = row["ne2"].ToString().Trim();
                            if (row["ne3"] != DBNull.Value)
                                ne2 = row["ne3"].ToString().Trim();
                            else
                                ne2 = "";
                        }
                        else if (row["ne3"] != DBNull.Value)
                        {
                            ne1 = row["ne3"].ToString().Trim();
                            ne2 = "";
                        }
                        else
                        {
                            ne1 = "";
                            ne2 = "";
                        }

                        if (fases.Substring(0, 1).Contains("1"))
                        {
                            cap = (decimal)row["cfa"];
                            //ne  = row["ne1"].ToString().Trim();
                            if (tconex.Contains("DA"))
                                fas = "AB";
                            else
                                fas = "A";

                            sql = "INSERT INTO trcfe_a (idbanco, economico, fases, capacidad, autoprotegido) " +
                            String.Format("Values ({0}, '{1}' , '{2}', {3}, '{4}')", id, ne1, fas, cap, autoproteg);
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

                            sql = "INSERT INTO trcfe_a (idbanco, economico, fases, capacidad, autoprotegido) " +
                            String.Format("Values ({0}, '{1}' , '{2}', {3}, '{4}')", id, ne2, fas, cap, autoproteg);
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
                            sql = "INSERT INTO trcfe_a (idbanco, economico, fases, capacidad, autoprotegido) " +
                            String.Format("Values ({0}, '{1}' , '{2}', {3}, '{4}')", id, ne1, fas, cap, autoproteg);
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
                            
                            sql = "INSERT INTO trcfe_a (idbanco, economico, fases, capacidad, autoprotegido) " +
                            String.Format("Values ({0}, '{1}' , '{2}', {3}, '{4}')", id, ne2, fas, cap, autoproteg);
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }
                    
                    } // FIN DE IF ELSES DE TIPOS DE CONEXION

                    StrSQL = "select unico as id, x1,y1,x2,y2, 0 as x, 0 as y, -1 as orden,  identificador as idtr, 'NA' as idtrans, 'A' tipo, orden_fases fases," +
                        "mat_con_fases mat_fases, cal_con_fases cal_fases,mat_con_neutro mat_neutro, cal_con_neutro cal_neutro, fec_ins as fecha_insercion," +
                        " fec_act as fecha_actualizacion, observaciones from linea_ls_m " +
                        "where div = '" + pDivision + "' and zona = '" + pZona + "' and identificador = '" + idtr + "' " +
                        "union " +
                        "select unico as id, x1,y1,x2,y2, x, y, orden,  identificador as idtr, 'NA' as idtrans, 'A' tipo, orden_fases fases," +
                        "mat_con_fases mat_fases, cal_con_fases cal_fases,mat_con_neutro mat_neutro, cal_con_neutro cal_neutro, fec_ins as fecha_insercion," +
                        " fec_act as fecha_actualizacion, observaciones from linea_ls_m l, deflex_ls_m d " +
                        "where l.div = '" + pDivision + "' and l.zona = '" + pZona + "' and l.identificador = '" + idtr + "' " +
                        "and lx1=x1 and ly1=y1 and lx2=x2 and ly2=y2 order by 1, 8";
                    
                    dt = conn.obtenerDT(StrSQL, pDivision);

                    //lineas = new List<LineaPrimaria>();

                    List<List<double>> deflexiones = null;
                    List<double> deflexion = null;
                    int unico = 0;
                    int unicoA = 0;
                    int unicoB = 0;
                    int orden;
                    bool lnueva = true;

                    //string obs;
                    
                    string puntos = "";

                    for (int renglon = 0; renglon < dt.Rows.Count; renglon++)
                    {

                        if (lnueva == true)
                        {
                            puntos = "LINESTRING(";
                            deflexiones = new List<List<double>>();
                            deflexion = new List<double>();
                            pointsl = Converter.UTMXYToLatLon(Convert.ToDouble(dt.Rows[renglon]["x1"]), Convert.ToDouble(dt.Rows[renglon]["y1"]), UTM, false);

                            deflexion.Add((double)Converter.RadToDeg(pointsl[1]));
                            deflexion.Add((double)Converter.RadToDeg(pointsl[0]));
                            deflexiones.Add(deflexion);
                            lnueva = false;
                        }

                        unico = Convert.ToInt32(dt.Rows[renglon]["id"]);
                        unicoA = Convert.ToInt32(dt.Rows[renglon]["id"]);
                        orden = Convert.ToInt32(dt.Rows[renglon]["orden"]);

                        if (renglon < dt.Rows.Count - 1)
                        {
                            unicoB = Convert.ToInt32(dt.Rows[renglon + 1]["id"]);
                        }
                        else
                        {
                            unicoB = 0;
                        }

                        if (unicoA == unicoB)
                        {
                            if (orden != -1)
                            {
                                deflexion = new List<double>();
                                pointsl = Converter.UTMXYToLatLon(Convert.ToDouble(dt.Rows[renglon]["x"]), Convert.ToDouble(dt.Rows[renglon]["y"]), UTM, false);
                                deflexion.Add((double)Converter.RadToDeg(pointsl[1]));
                                deflexion.Add((double)Converter.RadToDeg(pointsl[0]));
                                deflexiones.Add(deflexion);
                            }
                        }
                        else
                        {
                            if (orden != -1)
                            {
                                deflexion = new List<double>();
                                pointsl = Converter.UTMXYToLatLon(Convert.ToDouble(dt.Rows[renglon]["x"]), Convert.ToDouble(dt.Rows[renglon]["y"]), UTM, false);
                                deflexion.Add((double)Converter.RadToDeg(pointsl[1]));
                                deflexion.Add((double)Converter.RadToDeg(pointsl[0]));
                                deflexiones.Add(deflexion);
                            }
                            deflexion = new List<double>();
                            pointsl = Converter.UTMXYToLatLon(Convert.ToDouble(dt.Rows[renglon]["x2"]), Convert.ToDouble(dt.Rows[renglon]["y2"]), UTM, false);
                            deflexion.Add((double)Converter.RadToDeg(pointsl[1]));
                            deflexion.Add((double)Converter.RadToDeg(pointsl[0]));
                            deflexiones.Add(deflexion);
                            for (int i = 0; i < deflexiones.Count(); i++)
                            {
                                if (i == deflexiones.Count() - 1)
                                {
                                    puntos += deflexiones[i][0] + " " + deflexiones[i][1] + ")";
                                }
                                else
                                {
                                    puntos += deflexiones[i][0] + " " + deflexiones[i][1] + ",";
                                }

                            }

                            if (!Convert.IsDBNull(dt.Rows[renglon]["fecha_insercion"]))
                            {
                                fecha_insercion = Convert.ToDateTime(dt.Rows[renglon]["fecha_insercion"]);
                            }

                            if (!Convert.IsDBNull(dt.Rows[renglon]["fecha_actualizacion"]))
                            {
                                fecha_actualizacion = Convert.ToDateTime(dt.Rows[renglon]["fecha_actualizacion"]);
                            }
                            
                            string coor = "﻿ST_GeomFromText('" + puntos.ToString().Trim() + "',4326)";
                            coor = coor.Replace("\ufeff", " ");

                            sql = "INSERT INTO linea_bta (idsiged,division, zona, circuito, idtransf, material_fases,calibre_fases,material_neutro, calibre_neutro, orden_fases,fecha_insercion,fecha_actualizacion,observaciones,coordenadas) " +
                                  "Values (" + unico + ",'" + pDivision + "', '" + pZona + "' , '" + circuito + "' , " + id.ToString() + " , '" + dt.Rows[renglon]["mat_fases"].ToString().Trim() + "' , '" + dt.Rows[renglon]["cal_fases"].ToString().Trim() + "' , '" + dt.Rows[renglon]["mat_neutro"].ToString().Trim() + "' , '" + dt.Rows[renglon]["cal_neutro"].ToString().Trim() + "' , '" + dt.Rows[renglon]["fases"].ToString().Trim() + "',to_date('" + fecha_insercion + "','YY-MM-DD'),to_date('" + fecha_actualizacion + "','YY-MM-DD'),'" + dt.Rows[renglon]["observaciones"].ToString().Trim() + "'," + coor.TrimStart() + ")";

                                // INSERTA LINEA BTA
                                command.CommandText = sql;
                                // REGRESA EL ID DE LA LINEA INSERTADA
                                Int64 idlbta = Convert.ToInt64(command.ExecuteScalar());
                            
                            puntos = "";
                            lnueva = true;
                        }

                    }  // Fin For Lineas BTA

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
