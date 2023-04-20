using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Siged_Spatial_ImportExportData
{
    public static class TransicionMT
    {
        public static void importaTransicionesMT(string pDivision, string pZona, string circuito, NpgsqlCommand command,int UTM)
        {
            DataTable dt, dt2 = new DataTable();
            Conexion conn = new Conexion();

            double[] pointsl;

            decimal px, py;
            string StrSQL;

            /* OBTENEMOS LOS DATOS POR transformador
            =================================================== */

            string coordenada, num, fases;

            string sql = "";

            Int64 idsiged, idlmts, idlmta;
            Int64 idlmtapg, idlmtspg;

            decimal rotacion;
            Int16 cantapartarrayo, varillatierra;

            DateTime fecha_insercion = new DateTime();
            DateTime fecha_actualizacion = new DateTime();
          
            StrSQL = "select id, x, y, num, rotacion, diametroducto, tipoducto, varillatierra, cantapartarrayo, tipoapartarrayo, mcovapartarrayo, " +
                "tipoterminal, observaciones, fec_ins, fec_act, tipoproteccion, capproteccion, economico[1,5], idlm, idlp FROM sub_transmt " +
                "WHERE div='" + pDivision + "' and zona='" + pZona + "' and circuito = '" + circuito + "'"; //in (14708,20406,21013,22986,23439,23441)
            dt = conn.obtenerDT(StrSQL, pDivision);

            string diamducto, tipoducto, observaciones, tipoterminal, tipoapartarrayo, mcovapartarrayo;

            foreach (DataRow row in dt.Rows)
            {
                try
                {
                     idsiged = (int)row["id"];
                    if (row["idlm"] != DBNull.Value)
                    {
                        idlmts = Convert.ToInt64(row["idlm"].ToString());
                    }
                    else
                        idlmts = 0;

                    if (row["idlp"] != DBNull.Value)
                    {
                        idlmta = Convert.ToInt64(row["idlp"].ToString());
                    }
                    else
                        idlmta = 0;

                    if (idlmta > 0 && idlmts > 0)
                    {
                        idlmta = 0;
                        idlmts = 0;
                    }

                    if (row["num"] != DBNull.Value)
                        num = row["num"].ToString();
                    else
                        num = "";

                    if (row["rotacion"] != DBNull.Value)
                    {
                        rotacion = (Int16)(row["rotacion"]);
                        if (rotacion < -360 || rotacion > 360)
                            rotacion = 0;
                    }
                    else
                        rotacion = 0;

                    //if (row["diametroducto"] != DBNull.Value)
                    //    diamducto = row["diametroducto"].ToString();
                    //else
                    //    diamducto = "";

                    diamducto = Regex.Replace(row["diametroducto"].ToString(), @"[^\d]", "");
                    if (diamducto == "")
                        diamducto = "2";
                
                    if (row["tipoducto"] != DBNull.Value)
                        tipoducto = row["tipoducto"].ToString();
                    else
                        tipoducto = "";

                    if (row["varillatierra"] != DBNull.Value)
                        varillatierra = (Int16)row["varillatierra"];
                    else
                        varillatierra = 0;

                    if (row["cantapartarrayo"] != DBNull.Value)
                        cantapartarrayo = (Int16)row["cantapartarrayo"];
                    else
                        cantapartarrayo = 0;

                    if (row["tipoapartarrayo"] != DBNull.Value)
                        tipoapartarrayo = row["tipoapartarrayo"].ToString();
                    else
                        tipoapartarrayo = "";

                    if (row["mcovapartarrayo"] != DBNull.Value)
                        mcovapartarrayo = row["mcovapartarrayo"].ToString();
                    else
                        mcovapartarrayo = "0";

                    if (row["tipoterminal"] != DBNull.Value)
                        tipoterminal = row["tipoterminal"].ToString();
                    else
                        tipoterminal = "";

                    if (row["observaciones"] != DBNull.Value)
                        observaciones = row["observaciones"].ToString();
                    else
                        observaciones = "";

                    if (row["fec_ins"] != DBNull.Value)
                        fecha_insercion = Convert.ToDateTime(row["fec_ins"]);
                
                    if (row["fec_act"] != DBNull.Value)
                        fecha_actualizacion = Convert.ToDateTime(row["fec_act"]);                

                    px = (decimal)row["x"];
                    py = (decimal)row["y"];
                                
                    pointsl = Converter.UTMXYToLatLon((double)px, (double)py, UTM, false);

                    px = (decimal)Converter.RadToDeg(pointsl[0]);
                    py = (decimal)Converter.RadToDeg(pointsl[1]);

                    fases = "ABC";

                    NpgsqlDataReader dr;
                    idlmtapg = 0;
                    idlmtspg = 0;
                    coordenada = String.Format("﻿ST_GeomFromText('POINT({0} {1})', 4326)", py, px);
                    if (idlmta > 0 || idlmts > 0)
                    {
                        if (idlmta>0)
                        {
                            sql = "select id, orden_fases from linea_mta where division= '" + pDivision + "' and zona='" + pZona + "' and idsiged=" + idlmta.ToString() + "";
                            sql = sql.Replace("\ufeff", " ");
                            command.CommandText = sql;

                            dr = command.ExecuteReader();
                            while (dr.Read())
                            {
                                idlmtapg = Convert.ToInt64(dr[0].ToString());
                                fases = dr[1].ToString();
                            }
                        }
                        else
                        {
                            sql = "select id, orden_fases from linea_mts where division= '" + pDivision + "' and zona='" + pZona + "' and idsiged=" + idlmts.ToString() + "";
                            sql = sql.Replace("\ufeff", " ");
                            command.CommandText = sql;
                            dr = command.ExecuteReader();
                            while (dr.Read())
                            {
                                idlmtspg = Convert.ToInt64(dr[0].ToString());
                                fases = dr[1].ToString();
                            }
                        }
                        dr.Close();
                    }

                    if (idlmtapg == 0 && idlmtspg == 0)
                    {
                        sql = "select st_distancesphere(" + coordenada + ", ST_EndPoint(l.coordenadas::geometry)) as dist, l.id, l.orden_fases " +
                            "from linea_mta l where l.division= '" + pDivision + "' and l.zona='" + pZona + "' and l.circuito='" + circuito + "' " +
                            "and st_distancesphere(" + coordenada + ", ST_EndPoint(l.coordenadas::geometry))<1 order by 1 desc";
                        sql = sql.Replace("\ufeff", " ");
                        command.CommandText = sql;
                        dr = command.ExecuteReader();

                        //fases = "ABC";
                        //decimal dist;
                        while (dr.Read())
                        {
                            //dist = Convert.ToDecimal(dr[0].ToString());
                            idlmtapg = Convert.ToInt64(dr[1].ToString());
                            fases = dr[2].ToString();
                        }
                        dr.Close();

                        if (idlmtapg == 0)
                        {
                            sql = "select st_distancesphere(" + coordenada + ", ST_EndPoint(l.coordenadas::geometry)) as dist, l.id, l.orden_fases " +
                            "from linea_mts l where l.division= '" + pDivision + "' and l.zona='" + pZona + "' and l.circuito='" + circuito + "' " +
                            "and st_distancesphere(" + coordenada + ", ST_EndPoint(l.coordenadas::geometry))<1 order by 1 desc";
                            sql = sql.Replace("\ufeff", " ");
                            command.CommandText = sql;
                            dr = command.ExecuteReader();

                            //fases = "ABC";

                            while (dr.Read())
                            {
                                //dist = Convert.ToDecimal(dr[0].ToString());
                                idlmtspg = Convert.ToInt64(dr[1].ToString());
                                fases = dr[2].ToString();
                            }
                            dr.Close();
                                
                        }
                        
                    }

                    sql = "INSERT INTO transicion_MT (idsiged, idlmta, idlmts, division, zona, circuito, numero, rotacion, diametro_ducto, tipo_ducto, varillatierra, " +
                        "cant_aparta, tipo_aparta, mcov_aparta, tipo_terminal, observaciones, fecha_insercion, fecha_actualizacion, coordenada) " +
                    "values (" + idsiged + ", " + idlmtapg + ", " + idlmtspg + ", '" + pDivision + "','" + pZona + "','" + circuito + "', '" + num + "', " + rotacion.ToString() + ", " +
                    "'" + diamducto + "','" + tipoducto + "', " + varillatierra + ", " + cantapartarrayo.ToString() + ", '" + tipoapartarrayo + "',  " +
                    "" + mcovapartarrayo + ", '" + tipoterminal + "' ,'" + row["observaciones"] + "', to_date('" + fecha_insercion + "','YY-MM-DD'), " +
                    "to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ") ﻿RETURNING id";

                    sql = sql.Replace("\ufeff", " ");
                    // INSERTA POSTE dfjdsgfrg
                    command.CommandText = sql;
                    // REGRESA EL ID DEL POSTE INSERTADO ggrgrgt
                    Int64 idtransmt = Convert.ToInt64(command.ExecuteScalar());

                    if (row["tipoproteccion"] != DBNull.Value)
                    {
                        string tipoprot, tiposeccstr, tipolinea;

                        tipolinea = "";

                        Int64[] lineassecc = {0,0,0,0,0};
                            
                        tipoprot = row["tipoproteccion"].ToString().ToUpper();

                        if (tipoprot.Contains("COG") || tipoprot.Contains("COGC") || tipoprot.Contains("CNAVAJA") || tipoprot.Contains("COP") || tipoprot.Contains("CCF") || tipoprot.Contains("RESTAURADOR"))
                        {
                            Int16 capprot;
                            if (row["capproteccion"] != DBNull.Value)
                                capprot = (Int16)row["capproteccion"];
                            else
                                capprot = 0;
                            string economico, tiposecc, utr;

                            if (row["economico"] != DBNull.Value)
                            {
                                economico = row["economico"].ToString().ToUpper();
                                if (economico == "NO ACT" || economico == "NOACT")
                                    economico = "";
                            }
                            else
                                economico = "";

                            StrSQL = "select count(u.identificador) as utr FROM utr_m u, sub_transmt t, nodo_lp_m n " +
                                    "WHERE u.div='" + pDivision + "' and u.zona='" + pZona + "' and u.x=" + row["x"].ToString() + " and u.y=" + row["y"].ToString() + " and " +
                                    "t.div=u.div and t.zona=u.zona and t.circuito='" + circuito + "' and t.x= u.x and t.y=u.y and n.circuito=t.circuito";
                            dt2 = conn.obtenerDT(StrSQL, pDivision);

                            utr = (dt2.Rows[0]["utr"].ToString() == "1") ? "S" : "N";

                            string fasesSecc="";

                            if (fases.Contains('A'))
                            {
                                fasesSecc = fasesSecc + 'A';
                            }
                            if (fases.Contains('B'))
                            {
                                fasesSecc = fasesSecc + 'B';
                            }
                            if (fases.Contains('C'))
                            {
                                fasesSecc = fasesSecc + 'C';
                            }

                            if (tipoprot.Contains("COG") || tipoprot.Contains("COGC") || tipoprot.Contains("CNAVAJA") || tipoprot.Contains("COP") || tipoprot.Contains("SECCPOSTE"))
                            {
                                if (tipoprot.Contains("COG"))
                                    tiposecc = "2";
                                else if (tipoprot.Contains("COGC"))
                                    tiposecc = "1";
                                else if (tipoprot.Contains("CNAVAJA") || tipoprot.Contains("COP"))
                                    tiposecc = "3";
                                else
                                    tiposecc = "4";

                                tiposeccstr="DES";
                                sql = "INSERT INTO desconectador (idsiged, division, zona, circuito, economico, fases_con, capacidad, estado, rotacion, id_tipo, observaciones, utr, fecha_insercion, fecha_actualizacion, coordenada) " +
                                "values (0, '" + pDivision + "','" + pZona + "','" + circuito + "', '" + economico + "', '" + fasesSecc + "'," + capprot.ToString() + ",'NC', 0, '" + tiposecc + "', " +
                                "'SECC DE TRANSICION', '" + utr + "' ,to_date('" + fecha_insercion + "','YY-MM-DD'),to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ") ﻿RETURNING id";
                            }
                            else if (tipoprot.Contains("CCF"))
                            {
                                tiposeccstr="FUS";
                                sql = "INSERT INTO fusible (idsiged, division, zona, circuito, economico, fases, capacidad, estado, rotacion, tipo,  tresdisparos, observaciones, fecha_insercion, fecha_actualizacion, coordenada) " +
                                "values ('0', '" + pDivision + "','" + pZona + "','" + circuito + "', '" + economico + "', '" + fasesSecc + "', " + capprot.ToString() + ", 'NC', 0, 'K','N', " +
                                "'FUSIBLE DE TRANSICION', to_date('" + fecha_insercion + "','YY-MM-DD'),to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ") ﻿RETURNING id";
                            }
                            else // Restaurador
                            {
                                tiposeccstr="RES";
                                sql = "INSERT INTO restaurador (idsiged, division, zona, circuito, marca, tipo, estado, fases, capintamp, economico, rotacion, utr, observaciones, fecha_insercion, fecha_actualizacion, coordenada) " +
                                "values ('0', '" + pDivision + "','" + pZona + "','" + circuito + "', '', 'ELECTRONICO', 'NC' , 'ABC'," + capprot.ToString() + ",'" + economico + "', " + rotacion + ", '" + utr + "',  " +
                                "'RESTAURADOR DE TRANSICION', to_date('" + fecha_insercion + "','YY-MM-DD'),to_date('" + fecha_actualizacion + "','YY-MM-DD'), " + coordenada.TrimStart() + ") ﻿RETURNING id";
                            }

                            sql = sql.Replace("\ufeff", " ");
                            // INSERTA POSTE dfjdsgfrg
                            command.CommandText = sql;
                            // REGRESA EL ID DEL POSTE INSERTADO ggrgrgt
                            Int64 idsecc = Convert.ToInt64(command.ExecuteScalar());
                                
                            if (idlmtapg > 0)
                            {
                                sql = "select st_distancesphere(t.coordenada, ST_StartPoint(l.coordenadas::geometry)) as dist,l.id " +
                                "from linea_mts l, transicion_mt t " +
                                "where l.division= '" + pDivision + "' and l.zona='" + pZona + "' and l.circuito='" + circuito + "' " +
                                "and st_distancesphere(t.coordenada, ST_StartPoint(l.coordenadas::geometry))<1 and t.id=" + idtransmt.ToString() + " " +
                                "order by 1 desc";

                                sql = sql.Replace("\ufeff", " ");
                                command.CommandText = sql;
                                dr = command.ExecuteReader();
                                    
                                idlmtspg = 0;
                                int i = 0;
                                while (dr.Read() && i < 5)
                                {
                                    tipolinea = "SUB";
                                    idlmtspg = Convert.ToInt64(dr[1].ToString());
                                    lineassecc[i] = idlmtspg;
                                    i = i + 1;
                                }
                                dr.Close();

                                // BUSCAR ACOMETIDAS

                                if (lineassecc[0] == 0)
                                {
                                    sql = "select st_distancesphere(t.coordenada, ST_StartPoint(l.coordenadas::geometry)) as dist,l.id, l.idtrans, t.idsiged " +
                                    "from acometida_mts l, transicion_mt t " +
                                    "where l.division= '" + pDivision + "' and l.zona='" + pZona + "' and l.circuito='" + circuito + "' " +
                                    "and st_distancesphere(t.coordenada, ST_StartPoint(l.coordenadas::geometry))<1 and t.id=" + idtransmt.ToString() + " " +
                                    "order by 1 desc";

                                    sql = sql.Replace("\ufeff", " ");
                                    command.CommandText = sql;
                                    dr = command.ExecuteReader();

                                    idlmtspg = 0;
                                    i = 0;
                                    while (dr.Read() && i < 5)
                                    {
                                        if (Convert.ToInt64(dr[2].ToString()) == Convert.ToInt64(dr[3].ToString()))
                                        {
                                            tipolinea = "ACO";
                                            idlmtspg = Convert.ToInt64(dr[1].ToString());
                                            lineassecc[i] = idlmtspg;
                                        }
                                        i = i + 1;
                                    }
                                    dr.Close();
                                
                                }
                                foreach (int j in lineassecc)
                                {
                                    if (j > 0)
                                    {
                                        sql = "INSERT INTO secc_linea (tipo, idsecc, tipolinea, idlinea, division, zona) " +
                                                "values ('"+ tiposeccstr +"', " + idsecc.ToString() + ", '"+ tipolinea +"', " + j.ToString() + ", '" + pDivision + "', '" + pZona + "')";

                                        sql = sql.Replace("\ufeff", " ");
                                        command.CommandText = sql;
                                        command.ExecuteNonQuery();
                                        if (tipolinea=="SUB")
                                            sql = "update linea_mts set idtrans="+ idtransmt +" where id=" + j.ToString();
                                        else
                                            sql = "update acometida_mts set idtrans=" + idtransmt + " where id=" + j.ToString();
                                        sql = sql.Replace("\ufeff", " ");
                                        command.CommandText = sql;
                                        command.ExecuteNonQuery();
                                    }
                                }
                            }
                            else if (idlmtspg > 0)
                            {
                                sql = "select st_distancesphere(t.coordenada, ST_StartPoint(l.coordenadas::geometry)) as dist,l.id " +
                                "from linea_mta l, transicion_mt t " +
                                "where l.division= '" + pDivision + "' and l.zona='" + pZona + "' and l.circuito='" + circuito + "' " +
                                "and st_distancesphere(t.coordenada, ST_StartPoint(l.coordenadas::geometry))<1 and t.id=" + idtransmt.ToString() + " " +
                                "order by 1 desc";

                                sql = sql.Replace("\ufeff", " ");
                                command.CommandText = sql;
                                dr = command.ExecuteReader();

                                idlmtapg = 0;
                                int i = 0;
                                while (dr.Read() && i < 5)
                                {
                                    idlmtapg = Convert.ToInt64(dr[1].ToString());
                                    lineassecc[i] = idlmtapg;
                                    i = i + 1;
                                }
                                dr.Close();

                                foreach (int j in lineassecc)
                                {
                                    if (j > 0)
                                    {
                                        sql = "INSERT INTO secc_linea (tipo, idsecc, tipolinea, idlinea, division, zona) " +
                                                "values ('"+ tiposeccstr +"', " + idsecc.ToString() + ", 'AER', " + j.ToString() + ", '" + pDivision + "', '" + pZona + "')";

                                        sql = sql.Replace("\ufeff", " ");
                                        command.CommandText = sql;
                                        command.ExecuteNonQuery();
                                    }
                                }
                            }

                        }
                        
                    }
                                        
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
