using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Siged_Spatial_ImportExportData
{
    public static class SeccionadorSubt
    {
        public static void inserta_vias(Int64 idsecc, int numvia, string estado, int amperes,  NpgsqlCommand command)
        {
            string sql;
            sql = "INSERT INTO seccionador_vias (idsecc, numvia, estado, amperes) " +
                           "values (" + idsecc.ToString() + ", '" + numvia.ToString() + "', '" + estado + "', " + amperes.ToString() + ")";
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }

        public static void importaSeccionadorSubt(string pDivision, string pZona, string circuito, NpgsqlCommand command,int UTM)
        {
            DataTable dt, dt2 = new DataTable();
            Conexion conn = new Conexion();
          
            double[] pointsl;

            decimal px, py;
            string StrSQL;

            /* OBTENEMOS LOS DATOS POR transformador
            =================================================== */

            string coordenada, economico, utr, tipotransfer, tipobase, medioaislante, proteccion, observaciones, estado;
            Int64 idsiged, idsecc;
            
            string sql = "";
            Int16 rotacion, vias;
            int amperes, i;

            DateTime fecha_insercion = new DateTime();
            DateTime fecha_actualizacion = new DateTime();

            StrSQL = "select s.id, l.id as idlm,  x, y, rotacion, economico, vias, utr[1,1], transferencia, tipo, caracteristica, proteccion, s.observaciones, " +
                "s.fec_ins, s.fec_act, estado1v[1,1], amperes1v, estado2v[1,1], amperes2v, estado3v[1,1], amperes3v, estado4v[1,1], amperes4v, estado5v[1,1],  " +
                "amperes5v, estado6v[1,1], amperes6v, estado7v[1,1], amperes7v, estado8v[1,1], amperes8v from sub_secc s, sub_linmt l " +
                "WHERE div='" + pDivision + "' and zona='" + pZona + "' and circuito = '" + circuito + "' and s.idlm=l.id";
            dt = conn.obtenerDT(StrSQL, pDivision);
            
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    idsiged = (int)row["id"];
                    if (row["rotacion"] != DBNull.Value)
                    {
                        rotacion = (Int16)(row["rotacion"]);
                        if (rotacion < -360 || rotacion > 360)
                            rotacion = 0;
                    }
                    else
                        rotacion = 0;
                    
                    economico = Regex.Replace(row["economico"].ToString(), @"[^\d]", "");
                    if (row["vias"] != DBNull.Value)
                        vias = Convert.ToInt16(row["vias"].ToString());
                    else
                        vias = 3;
                    
                    utr = (row["utr"].ToString().ToUpper() == "S") ? "S" : "N";

                    tipotransfer = row["transferencia"].ToString().ToUpper();    
                    tipobase = row["tipo"].ToString().ToUpper();    
                    medioaislante = row["caracteristica"].ToString().ToUpper();
                    proteccion = row["proteccion"].ToString().ToUpper();
                    observaciones = row["observaciones"].ToString().ToUpper();
              
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
                    
                    sql = "INSERT INTO seccionador_subt (idsiged, division, zona, circuito, rotacion, economico, vias, utr, tipotransf, tipobase, medioaisla, proteccion, " +
                    "observaciones, fecha_insercion, fecha_actualizacion, coordenada) values " +
                    "(" + idsiged + ", '" + pDivision + "','" + pZona + "', '" + circuito + "', " + rotacion.ToString() + ", '" + economico + "', " + vias.ToString() + 
                    ", '" + utr + "', '" + tipotransfer + "', '" + tipobase + "', '" + medioaislante + "', '" + proteccion + "' ,'" + observaciones + "', " +
                    "to_date('" + fecha_insercion + "','YY-MM-DD'),to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ") ﻿RETURNING id";

                    sql = sql.Replace("\ufeff", " ");
                    // INSERTA POSTE dfjdsgfrg
                    command.CommandText = sql;
                    // REGRESA EL ID DEL POSTE INSERTADO ggrgrgt
                    idsecc = Convert.ToInt64(command.ExecuteScalar());
                        

                    if (row[15] != DBNull.Value)
                    {
                        estado = (row[15].ToString() == "A") ? "NA" : "NC";
                        amperes = (row[16].ToString() == "600") ? 600 : 200;
                        inserta_vias(idsecc, 1, estado, amperes, command);
                    }
                    if (row[17] != DBNull.Value)
                    {
                        estado = (row[17].ToString() == "A") ? "NA" : "NC";
                        amperes = (row[18].ToString() == "600") ? 600 : 200;
                        inserta_vias(idsecc, 2, estado, amperes, command);
                    }
                        
                    int ind=19;
                    i=3;
                    while(i <= vias)
                    {
                        estado = (row[ind].ToString() == "A") ? "NA" : "NC";
                        amperes = (row[ind+1].ToString() == "600") ? 600 : 200;
                        inserta_vias(idsecc, i, estado, amperes, command);
                        ind = ind + 2;
                        i = i + 1;
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
