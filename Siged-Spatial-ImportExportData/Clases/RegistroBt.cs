using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Siged_Spatial_ImportExportData
{
    public static class RegistroBt
    {
        public static void inserta_juegocon(string calibre, string tipo, Int16 cant, Int64 idreg, NpgsqlCommand command)
        {
            string sql;
            sql = "INSERT INTO registro_bt_juegocon (idregistro, calibre, tipo, cantidad) " +
                           "values (" + idreg.ToString() + ", '" + calibre + "', '" + tipo + "', " + cant.ToString() + ")";
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }

        public static void importaRegistrosBt(string pDivision, string pZona, string circuito, NpgsqlCommand command,int UTM)
        {
            DataTable dt, dt2 = new DataTable();
            Conexion conn = new Conexion();
          
            double[] pointsl;

            decimal px, py;
            string StrSQL;

            /* OBTENEMOS LOS DATOS POR transformador
            =================================================== */

            string coordenada, urbana, result, juegocontipo;
            Int64 idsiged;
            Int64 idreg;

            Int32 cons;

            string sql = "";
            Int16 varilla, conmultbt_4, conmultbt_6, conmultbt_8, conmultbt_10, rotacion, cantjuegocon;

            DateTime fecha_insercion = new DateTime();
            DateTime fecha_actualizacion = new DateTime();

            StrSQL = "select id, n.num, x, y, tiporegistro, rotacion, varillatierra, conmultbt_4, conmultbt_6, conmultbt_8, conmultbt_10, observaciones, " +
                "urbana, fec_ins, fec_act, juegocontipo, juegoconcal2, juegoconcal4, juegoconcal6, juegoconcal1_0, juegoconcal3_0, juegoconcal250, " +
                "juegoconcal40 from sub_regbt, sub_regbt_num n WHERE div='" + pDivision + "' and zona='" + pZona + "' and circuito = '" + circuito + "' and idreg=id";
            dt = conn.obtenerDT(StrSQL, pDivision);
            
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                
                    idsiged = (int)row["id"];
                    result = Regex.Replace(row["num"].ToString(), @"[^\d]", "");
                    if (result == "")
                    {
                        cons = 0;
                    }
                    else
                    {
                        cons = Convert.ToInt32(result);
                    }

                    if (row["fec_ins"] != DBNull.Value)
                        fecha_insercion = Convert.ToDateTime(row["fec_ins"]);
                    //else
                      //  fecha_insercion = Convert.ToDateTime("1900-01-01");
                    if (row["fec_act"] != DBNull.Value)
                        fecha_actualizacion = Convert.ToDateTime(row["fec_act"]);
                    //else
                      //  fecha_actualizacion = fecha_insercion;

                    urbana = (row["urbana"].ToString().ToUpper() == "S") ? "S" : "N";

                    px = (decimal)row["x"];
                    py = (decimal)row["y"];
                                
                    pointsl = Converter.UTMXYToLatLon((double)px, (double)py, UTM, false);

                    px = (decimal)Converter.RadToDeg(pointsl[0]);
                    py = (decimal)Converter.RadToDeg(pointsl[1]);

                    coordenada = String.Format("﻿ST_GeomFromText('POINT({0} {1})', 4326)", py, px);

                    sql = "select id from registro_bt where division ='" + pDivision + "' and zona='" + pZona + "' and idsiged=" + idsiged.ToString();

                    sql = sql.Replace("\ufeff", " ");
                    command.CommandText = sql;
                    // REGRESA EL ID DEL POSTE INSERTADO ggrgrgt

                    NpgsqlDataReader dr = command.ExecuteReader();

                    idreg = 0;

                    while (dr.Read())
                        idreg = (int)dr[0];
                    dr.Close();

                    if (idreg == 0)
                    {
                        
                        if (row["conmultbt_4"] != DBNull.Value)
                            conmultbt_4 = (Int16)row["conmultbt_4"];
                        else
                            conmultbt_4 = 0;

                        if (row["conmultbt_6"] != DBNull.Value)
                            conmultbt_6 = (Int16)row["conmultbt_6"];
                        else
                            conmultbt_6 = 0;

                        if (row["conmultbt_8"] != DBNull.Value)
                            conmultbt_8 = (Int16)row["conmultbt_8"];
                        else
                            conmultbt_8 = 0;

                        if (row["conmultbt_10"] != DBNull.Value)
                            conmultbt_10 = (Int16)row["conmultbt_10"];
                        else
                            conmultbt_10 = 0;

                        if (row["varillatierra"] != DBNull.Value)
                            varilla = (Int16)row["varillatierra"];
                        else
                            varilla = 0;

                        if (row["rotacion"] != DBNull.Value)
                        {
                            rotacion = (Int16)(row["rotacion"]);
                            if (rotacion < -360 || rotacion > 360)
                                rotacion = 0;
                        }
                        else
                            rotacion = 0;
                        
                        sql = "INSERT INTO registro_bt (idsiged, division, zona, tiporegistro, rotacion, conmultbt_4, conmultbt_6, conmultbt_8, " +
                          "conmultbt_10, varillatierra, observaciones, urbana, fecha_insercion, fecha_actualizacion, coordenada) values " +
                          "(" + idsiged + ", '" + pDivision + "','" + pZona + "', '" + row["tiporegistro"].ToString().Trim() + "', " + rotacion.ToString() + ", " +
                          "" + conmultbt_4.ToString() + ", " + conmultbt_6.ToString() + ", " + conmultbt_8.ToString() + ", " + conmultbt_10.ToString() + ", " + varilla.ToString() + " , " +
                          "'" + row["observaciones"] + "', '" + urbana + "' ,to_date('" + fecha_insercion + "','YY-MM-DD'),to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ") ﻿RETURNING id";

                        sql = sql.Replace("\ufeff", " ");
                        // INSERTA POSTE dfjdsgfrg
                        command.CommandText = sql;
                        // REGRESA EL ID DEL POSTE INSERTADO ggrgrgt
                        idreg = Convert.ToInt64(command.ExecuteScalar());

                        if (row["juegocontipo"] != DBNull.Value)
                            juegocontipo = row["juegocontipo"].ToString();
                        else
                            juegocontipo = "Removible";

                        if (row["juegoconcal2"] != DBNull.Value)
                        {
                            cantjuegocon = (Int16)row["juegoconcal2"];
                            if (cantjuegocon > 0)
                                inserta_juegocon("02",juegocontipo,cantjuegocon,idreg, command);
                        }
                        if (row["juegoconcal4"] != DBNull.Value)
                        {
                            cantjuegocon = (Int16)row["juegoconcal4"];
                            if (cantjuegocon > 0)
                                inserta_juegocon("04",juegocontipo,cantjuegocon,idreg, command);
                        }
                        if (row["juegoconcal6"] != DBNull.Value)
                        {
                            cantjuegocon = (Int16)row["juegoconcal6"];
                            if (cantjuegocon > 0)
                                inserta_juegocon("06", juegocontipo, cantjuegocon, idreg, command);
                        }
                        if (row["juegoconcal1_0"] != DBNull.Value)
                        {
                            cantjuegocon = (Int16)row["juegoconcal1_0"];
                            if (cantjuegocon > 0)
                                inserta_juegocon("1/0", juegocontipo, cantjuegocon, idreg, command);
                        }
                        if (row["juegoconcal3_0"] != DBNull.Value)
                        {
                            cantjuegocon = (Int16)row["juegoconcal3_0"];
                            if (cantjuegocon > 0)
                                inserta_juegocon("3/0", juegocontipo, cantjuegocon, idreg, command);
                        }
                        if (row["juegoconcal40"] != DBNull.Value)
                        {
                            cantjuegocon = (Int16)row["juegoconcal40"];
                            if (cantjuegocon > 0)
                                inserta_juegocon("4/0", juegocontipo, cantjuegocon, idreg, command);
                        }
                        if (row["juegoconcal250"] != DBNull.Value)
                        {
                            cantjuegocon = (Int16)row["juegoconcal250"];
                            if (cantjuegocon > 0)
                                inserta_juegocon("350", juegocontipo, cantjuegocon, idreg, command);
                        }

                    }
                      
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
                    
                    sql = "INSERT INTO registro_bt_cons (idregistro, circuito, consecutivo) " +
                           "values (" + idreg.ToString() + ", '" + circuito + "', " + cons + ")";
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
