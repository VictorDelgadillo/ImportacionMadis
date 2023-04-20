using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Siged_Spatial_ImportExportData
{
    public static class Desconectador
    {
        public static void importaDesconectadores(string pDivision, string pZona, string circuito, NpgsqlCommand command,int UTM)
        {
            DataTable dt, dt2 = new DataTable();
            Conexion conn = new Conexion();
          
            double[] pointsl;

            decimal px, py;
            string StrSQL;

            /* OBTENEMOS LOS DATOS POR transformador
            =================================================== */

            string coordenada, ne, utr, loc;

            string sql = "";

            string fases;

            int idlineasiged;
            int idlineapg;

            decimal rotacion;

            DateTime fecha_insercion = new DateTime();
            DateTime fecha_actualizacion = new DateTime();
            
            StrSQL = "select b.unico as idlinea, a.nx as x, a.ny as y, a.identificador, ne, a.fases_con, capacidad, a.loc, sym, estado, a.fec_ins, a.fec_act, a.observaciones, utr " +
                    "FROM desconectador_m a, linea_lp_m b, deflex_lp_m d " +
                    "WHERE a.div='" + pDivision + "' and a.zona='" + pZona + "' and ((a.nx=b.x2 and a.ny=b.y2) or (a.nx=b.x1 and a.ny=b.y1)) " +
                    "and b.div=a.div and b.zona=a.zona and circuito = '" + circuito + "' " +
                    "and d.lx1=b.x1 and d.ly1=b.y1 and d.lx2=b.x2 and d.ly2=b.y2 and d.x=a.x and d.y=a.y ";
            dt = conn.obtenerDT(StrSQL, pDivision);
            
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    idlineasiged = (int)row["idlinea"];
                    //identifi = row["identificador"].ToString();
                    if (row["fec_ins"] != DBNull.Value)
                        fecha_insercion = Convert.ToDateTime(row["fec_ins"]);
                    //else
                      //  fecha_insercion = Convert.ToDateTime("1900-01-01");
                    if (row["fec_act"] != DBNull.Value)
                        fecha_actualizacion = Convert.ToDateTime(row["fec_act"]);
                    //else
                      //  fecha_actualizacion = fecha_insercion;

                    utr = (row["utr"].ToString() == "1") ? "S" : "N";

                    if (row["loc"] != DBNull.Value)
                    {
                        loc = row["loc"].ToString();
                        rotacion = ((int)Convert.ToChar(loc) - 65);
                        rotacion = rotacion / 30 * 180;
                    
                    }
                    else
                        rotacion = 0;
                
                    fases = row["fases_con"].ToString();

                    if (fases.Contains("111"))
                      fases = "ABC";
                    else if (fases.Contains("110"))
                      fases = "AB";
                    else if (fases.Contains("101"))
                      fases = "AC";
                    else if (fases.Contains("011"))
                      fases = "BC";
                    else if (fases.Contains("100"))
                      fases = "A";
                    else if (fases.Contains("010"))
                      fases = "B";
                    else 
                      fases = "C";
                                
                    px = (decimal)row["x"];
                    py = (decimal)row["y"];
                                
                    pointsl = Converter.UTMXYToLatLon((double)px, (double)py, UTM, false);

                    px = (decimal)Converter.RadToDeg(pointsl[0]);
                    py = (decimal)Converter.RadToDeg(pointsl[1]);
                                
                    coordenada = String.Format("﻿ST_GeomFromText('POINT({0} {1})', 4326)", py, px);
                    
                    sql = "INSERT INTO desconectador (idsiged, division, zona, circuito, economico, fases_con, capacidad, estado, rotacion, id_tipo, observaciones, utr, fecha_insercion, fecha_actualizacion, coordenada) " +
                        "values ('" + row[3].ToString().Trim() + "', '" + pDivision + "','" + pZona + "','" + circuito + "', '" + row["ne"].ToString().Trim() + "', '" + fases + "'," + row[6].ToString().Trim() + ",'" + row[9].ToString().Trim() + "', " + rotacion + ", '" + row["sym"].ToString().Trim() + "', " +
                        "'" + row["observaciones"] + "', '" + utr + "' ,to_date('" + fecha_insercion + "','YY-MM-DD'),to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ") ﻿RETURNING id";

                    sql = sql.Replace("\ufeff", " ");
                    // INSERTA POSTE dfjdsgfrg
                    command.CommandText = sql;
                    // REGRESA EL ID DEL POSTE INSERTADO ggrgrgt
                    Int64 id = Convert.ToInt64(command.ExecuteScalar());
                     
                    // INSERTA EL DESCONECTADOR Y SU REFERENCIA DE LA LINEA
                    
                    //NpgsqlConnection conn2 = new NpgsqlConnection("Server=10.4.22.81;User Id=postgres;Password=manager;Database=postgres; Port=5432;");
                    //conn2.Open();

                    // Define a query returning a single row result set
                    //NpgsqlCommand command2 = new NpgsqlCommand("select id from linea_mta where division= '" + pDivision + "' and zona='" + pZona + "' and idsiged=" + idlineasiged.ToString(), conn2);

                    //NpgsqlDataReader dr = command2.ExecuteReader();
                                       
                    
                    // Output rows
                    //while (dr.Read())
                      //  idlineapg = (int)dr[0];

                    //conn2.Close();

                    // Execute the query and obtain the value of the first column of the first row
                    //idlineapg = (Int64)command.ExecuteScalar();

                    sql = "select id from linea_mta where division= '" + pDivision + "' and zona='" + pZona + "' and idsiged=" + idlineasiged.ToString();

                    sql = sql.Replace("\ufeff", " ");
                    command.CommandText = sql;
                    // REGRESA EL ID DEL POSTE INSERTADO ggrgrgt
                    idlineapg = (int)command.ExecuteScalar();

                    sql = "INSERT INTO secc_linea (tipo, idsecc, tipolinea, idlinea, division, zona) " +
                           "values ('DES', " + id.ToString() + ", 'AER', " + idlineapg.ToString() + ", '" + pDivision + "', '" + pZona + "')";
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
