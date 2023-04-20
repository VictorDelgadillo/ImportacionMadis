using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Siged_Spatial_ImportExportData
{
    public static class RegistroMt
    {

        public static void inserta_junction(string tipo, string amp, Int16 cant, Int64 idreg, NpgsqlCommand command)
        {
            string sql;
            sql = "INSERT INTO registro_mt_junction (idregistro, tipo, amperes,  cantidad) " +
                           "values (" + idreg.ToString() + ", '" + tipo + "', '" + amp + "', " + cant.ToString() + ")";
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }

        public static void importaRegistrosMt(string pDivision, string pZona, string circuito, NpgsqlCommand command,int UTM)
        {
            DataTable dt, dt2 = new DataTable();
            Conexion conn = new Conexion();
          
            double[] pointsl;

            decimal px, py;
            string StrSQL;

            /* OBTENEMOS LOS DATOS POR transformador
            =================================================== */

            string coordenada, urbana, result, tipojunction, ampjunctionStr, cantjunctionStr;
            int idsiged;
            int idreg;

            Int64 idreg2;
            Int32 cons;

            string sql = "";
            Int16 cantctc200, cantctc600, cantctcf, capctcf, varilla, cantjunction;

            DateTime fecha_insercion = new DateTime();
            DateTime fecha_actualizacion = new DateTime();

            StrSQL = "select id, num, x, y, tiporegistro, rotacion, conectadortipocodo200, conectadortipocodo600, conectadortipocodo200cf, conectadortipocodofusible, " +
                "varillatierra, urbana, fec_ins, fec_act, observaciones,  dmmtcantidad, " +
                "tipodmmt_1, ampsdmmt_1, cantdmmt_1, tipodmmt_2, ampsdmmt_2, cantdmmt_2, tipodmmt_3, ampsdmmt_3, cantdmmt_3, tipodmmt_4, ampsdmmt_4, cantdmmt_4, " +
                "tipodmmt_5, ampsdmmt_5, cantdmmt_5, tipodmmt_6, ampsdmmt_6, cantdmmt_6, tipodmmt_7, ampsdmmt_7, cantdmmt_7, tipodmmt_8, ampsdmmt_8, cantdmmt_8, " +
                "tipodmmt_9, ampsdmmt_9, cantdmmt_9 FROM sub_regmt, sub_regmt_num n " +
                "WHERE div='" + pDivision + "' and zona='" + pZona + "' and circuito = '" + circuito + "' and idreg=id";
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

                    sql = "select id from registro_mt where division ='" + pDivision + "' and zona='" + pZona + "' and idsiged=" + idsiged.ToString();

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
                        
                        if (row["conectadortipocodo200"] != DBNull.Value)
                        {
                            cantctc200 = (Int16)row["conectadortipocodo200"];
                        }
                        else
                        {
                            cantctc200 = 0;
                        }

                        if (row["conectadortipocodo600"] != DBNull.Value)
                        {
                            cantctc600 = (Int16)row["conectadortipocodo600"];
                        }
                        else
                        {
                            cantctc600 = 0;
                        }

                        if (row["conectadortipocodo200cf"] != DBNull.Value)
                        {
                            cantctcf = (Int16)row["conectadortipocodo200cf"];
                        }
                        else
                        {
                            cantctcf = 0;
                        }

                        if (row["conectadortipocodofusible"] != DBNull.Value)
                        {
                            capctcf = (Int16)row["conectadortipocodofusible"];
                        }
                        else
                        {
                            capctcf = 0;
                        }

                        if (row["varillatierra"] != DBNull.Value)
                        {
                            varilla = (Int16)row["varillatierra"];
                        }
                        else
                        {
                            varilla = 0;
                        }
                        
                        sql = "INSERT INTO registro_mt (idsiged, division, zona, tipo_registro, rotacion, cant_conec_codo_200, cant_conec_codo_600, cant_conec_codo_200cf, " +
                          "cap_conec_codo_fusible, varilla_tierra, observaciones, urbana, fecha_insercion, fecha_actualizacion, coordenada) values " +
                          "(" + idsiged + ", '" + pDivision + "','" + pZona + "', '" + row["tiporegistro"].ToString().Trim() + "', " + row["rotacion"].ToString() + ", " +
                          "" + cantctc200.ToString() + ", " + cantctc600.ToString() + ", " + cantctcf.ToString() + ", " + capctcf.ToString() + ", " + varilla.ToString() + " , " +
                          "'" + row["observaciones"] + "', '" + urbana + "' ,to_date('" + fecha_insercion + "','YY-MM-DD'),to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ") ﻿RETURNING id";

                        sql = sql.Replace("\ufeff", " ");
                        // INSERTA POSTE dfjdsgfrg
                        command.CommandText = sql;
                        // REGRESA EL ID DEL POSTE INSERTADO ggrgrgt
                        idreg2 = Convert.ToInt64(command.ExecuteScalar());

                        idreg = (int)idreg2;

                        tipojunction = row["tipodmmt_1"].ToString();
                        ampjunctionStr = row["ampsdmmt_1"].ToString();
                        cantjunctionStr = row["cantdmmt_1"].ToString();

                        result = Regex.Replace(row["cantdmmt_1"].ToString(), @"[^\d]", "");
                        if (result == "")
                        {
                            cantjunction = 0;
                        }
                        else
                        {
                            cantjunction = Convert.ToInt16(result);
                        }

                        if (tipojunction.Contains("J2") || tipojunction.Contains("J3") || tipojunction.Contains("J4") || tipojunction.Contains("J5") || tipojunction.Contains("J6"))
                        {
                            if ( cantjunction > 0 )
                                if (ampjunctionStr.Contains("200") || ampjunctionStr.Contains("600") || ampjunctionStr.Contains("2/1") || ampjunctionStr.Contains("2/2") || ampjunctionStr.Contains("4/2") || ampjunctionStr.Contains("3/2") )
                                    inserta_junction(tipojunction, ampjunctionStr, cantjunction, idreg, command);
                            
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
                    
                    sql = "INSERT INTO registro_mt_cons (idregistro, circuito, consecutivo) " +
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
