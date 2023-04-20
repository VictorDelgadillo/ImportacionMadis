using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siged_Spatial_ImportExportData
{
    static class TransfCfeSubterraneo
    {
        public static void importa(string pDivision, string pZona, string pCircuito, NpgsqlCommand command, int UTM)
        {
            DataTable dt = new DataTable();
            Conexion conn = new Conexion();

            double[] pointsl;

            decimal px, py;
            String StrSQL;



            /* OBTENEMOS LOS TRANSFORMADORES CFE SUBTERRANEOS
            =================================================== */

            StrSQL = "select t.id as idsiged,area as numbanco,rotacion,economico,tipotransformador as tipo_base,t.faseo as tipo_transformador,t.fases, "+
                     "potencia as capacidad, t.observaciones,t.fec_ins as fecha_insercion,t.fec_act as fecha_actualizacion,x, y " +
                     "from sub_trcfe t, sub_linmt l where t.idlm = l.id and div = '"+pDivision+"' and zona = '"+pZona+"'  AND circuito = '"+ pCircuito + "' ";

            dt = conn.obtenerDT(StrSQL, pDivision);


            Int64 idsiged, rotacion = 0;
            string  numbanco="", economico = "", tipo_base = "", tipo_transformador = "", fases = "", capacidad = "", observaciones = "", coordenada = "";
          
            DateTime fecha_insercion = new DateTime();
            DateTime fecha_actualizacion = new DateTime();
            //String result;
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    idsiged = Convert.ToInt64(row["idsiged"]);
             
                    if (row["rotacion"] != DBNull.Value)
                    {
                        rotacion = Convert.ToInt64(row["rotacion"]);
                    }
               
                         
                    if (row["fecha_insercion"] != DBNull.Value)
                    {
                        fecha_insercion = Convert.ToDateTime(row["fecha_insercion"]);
                    }
                    if (row["fecha_actualizacion"] != DBNull.Value)
                    {
                        fecha_actualizacion = Convert.ToDateTime(row["fecha_actualizacion"]);
                    }

                    if (!Convert.IsDBNull(row["numbanco"]))
                    {
                        numbanco = row["numbanco"].ToString();
                    }

                    if (!Convert.IsDBNull(row["economico"]))
                    {
                        economico = row["economico"].ToString();
                    }

                    if (!Convert.IsDBNull(row["tipo_base"]))
                    {
                        tipo_base = row["tipo_base"].ToString();
                    }

                    if (!Convert.IsDBNull(row["tipo_transformador"]))
                    {
                        tipo_transformador = row["tipo_transformador"].ToString();
                    }

                    if (!Convert.IsDBNull(row["fases"]))
                    {
                        fases = row["fases"].ToString();
                    }
                    if (!Convert.IsDBNull(row["capacidad"]))
                    {
                        capacidad = row["capacidad"].ToString();
                    }

                    observaciones = row["observaciones"].ToString();
               
                    px = (decimal)row["x"];
                    py = (decimal)row["y"];

                    pointsl = Converter.UTMXYToLatLon((double)px, (double)py, UTM, false);

                    px = (decimal)Converter.RadToDeg(pointsl[0]);
                    py = (decimal)Converter.RadToDeg(pointsl[1]);

                    coordenada = String.Format("﻿ST_GeomFromText('POINT({0} {1})', 4326)", py, px);
                    string sql = "INSERT INTO trcfe_sub (idsiged, division, zona , circuito , numbanco , rotacion , economico,tipo_base,tipo_transformador,fases,capacidad,observaciones, " +
                    "fecha_insercion, fecha_actualizacion,coordenada) " +
                    String.Format("Values ({0}, '{1}' , '{2}' , '{3}' , '{4}' , {5} ,'{6}','{7}','{8}','{9}','{10}','{11}', to_date('{12}','YY-MM-DD') , to_date('{13}','YY-MM-DD'), {14} )",
                    idsiged,
                    pDivision,
                    pZona,
                    pCircuito,
                    numbanco,
                    rotacion,
                    economico,
                    tipo_base,
                    tipo_transformador,
                    fases,
                    capacidad,
                    observaciones,                 
                    fecha_insercion,
                    fecha_actualizacion,                
                    coordenada) + " ﻿RETURNING id";
                    sql = sql.Replace("\ufeff", " ");
                    // INSERTA POSTE
                    command.CommandText = sql;
                    // REGRES EL ID DEL POSTE INSERTADO
                    Int64 id = Convert.ToInt64(command.ExecuteScalar());

                    // INSERTAMOS LA LINEA DE BAJA TENSION SUBTERRANEOS DEPENDIENTE DEL TR

                    StrSQL = "select a.id, x1,y1,x2,y2, 0 as x, 0 as y, -1 as orden, a.circuito as cicuito_bt, a.ccs calibre_fases, a.mcs material_fases, a.aislamiento, tipo, a.ccn calibre_neutro, a.mcn material_neutro, " +
                             "a.aislaneutro aislamiento_neutro, diametroducto, materialducto, numviasducto, a.ductoreserva,nivelesducto, a.tipoduc tipoducto, a.lineaductos, "+
                             "a.fases,a.observaciones,a.urbana,a.ductomixto,a.fec_ins fecha_insercion, a.fec_act fecha_actualizacion "+
                             "from sub_linbt a " +
                             "where  a.idtr = "+ idsiged +  " " +
                             "union "+
                             "select a.id, x1,y1,x2,y2,  x,  y, orden, a.circuito as cicuito_bt, a.ccs calibre_fases, a.mcs material_fases, a.aislamiento, tipo, a.ccn calibre_neutro, a.mcn material_neutro, " +
                             "a.aislaneutro aislamiento_neutro, diametroducto, materialducto, numviasducto, a.ductoreserva,nivelesducto, a.tipoduc tipoducto, a.lineaductos, " +
                             "a.fases,a.observaciones,a.urbana,a.ductomixto,a.fec_ins fecha_insercion, a.fec_act fecha_actualizacion "+
                             "from sub_defbt s,sub_linbt a  "+
                             "where  a.idtr = "+ idsiged + "  and s.idlb=a.id " +
                             "order by 1,8";

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

                            sql = "INSERT INTO linea_bts (division, zona, circuito, id_transformador,circuito_bt,calibre_fases,material_fases, aislamiento, " +
                                                         " tipo,calibre_neutro,material_neutro, aislamiento_neutro,diametroducto,materialducto,numviasducto,ductoreserva,nivelesducto, " +
                                                         " tipoducto,lineaductos,fases,observaciones,urbana,ductomixto,fecha_insercion,fecha_actualizacion,coordenadas) " +
                                  "Values ('" + pDivision + "', '" + pZona + "' , '" + pCircuito + "' , " + id + " , " + Convert.ToInt16(dt.Rows[renglon]["cicuito_bt"]) + " , '" + dt.Rows[renglon]["calibre_fases"].ToString().Trim() + "' , '" + dt.Rows[renglon]["material_fases"].ToString().Trim() + "' , '" + dt.Rows[renglon]["aislamiento"].ToString().Trim() + "' , '" + dt.Rows[renglon]["tipo"].ToString().Trim() + "','" + dt.Rows[renglon]["calibre_neutro"].ToString().Trim() + "', '" + dt.Rows[renglon]["material_neutro"].ToString().Trim() + "','" + dt.Rows[renglon]["aislamiento_neutro"].ToString().Trim() + "'," + Convert.ToInt16(dt.Rows[renglon]["diametroducto"]) + ",'" + dt.Rows[renglon]["materialducto"].ToString().Trim() + "'," + Convert.ToInt16(dt.Rows[renglon]["numviasducto"]) + "," + Convert.ToInt16(dt.Rows[renglon]["ductoreserva"]) + "," + Convert.ToInt16(dt.Rows[renglon]["nivelesducto"]) + ",'" + dt.Rows[renglon]["tipoducto"].ToString().Trim() + "'," + Convert.ToInt16(dt.Rows[renglon]["lineaductos"]) + ",'" + dt.Rows[renglon]["fases"].ToString().Trim() + "','" + dt.Rows[renglon]["observaciones"].ToString().Trim() + "', '" + dt.Rows[renglon]["urbana"].ToString().Trim() + "','" + dt.Rows[renglon]["ductomixto"].ToString().Trim() + "',to_date('" + fecha_insercion + "','YY-MM-DD'),to_date('" + fecha_actualizacion + "','YY-MM-DD')," + coor.TrimStart() + ")";

                            // INSERTA LINEA BTA
                            command.CommandText = sql;
                            // REGRESA EL ID DE LA LINEA INSERTADA
                            Int64 idlbta = Convert.ToInt64(command.ExecuteScalar());

                            puntos = "";
                            lnueva = true;
                        }

                    }  // Fin For Lineas BTSUBTERRANEAS






                } // FIN DEL TRY
                catch (Exception msg)
                {
                    // something went wrong, and you wanna know why
                    Console.WriteLine(msg.ToString());
                }
                //postes.Add(new Poste(pDivision, pZona, nombrezona, circuito, identificador, numero, (double)px, (double)py, material, Convert.ToInt16(row["altura"]), Convert.ToInt16(row["resistencia"]), fecha_ins, cantidad));
            } // fin de foreach datarows

        } // fin de postes
    }
}
