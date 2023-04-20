using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Siged_Spatial_ImportExportData
{

    public static class LineaBajaTension
    {
           
        public static void importaLineasBta(string pDivision, string pZona, string pCircuito, NpgsqlCommand command)
        {
            DataTable dt = new DataTable();
            Conexion conn = new Conexion();
            int UTM = 0;
            double[] pointsl;
    
            string StrSQL = "SELECT utm FROM m_utm WHERE ardiv='" + pDivision + "' AND arare='" + pZona + "'";
            dt = conn.obtenerDT(StrSQL, pDivision);

            foreach (DataRow row in dt.Rows)
            {
                UTM = Convert.ToInt16(row["utm"]);
            }

            //   List<CircuitoJson> listCircuitos = new List<CircuitoJson>();

            List<LineaPrimaria> lineas = null;
            List<List<double>> deflexiones = null;
            List<double> deflexion = null;
            int unico = 0;
            int unicoA = 0;
            int unicoB = 0;
            int orden;
            bool lnueva = true;

            string sql = "";


            String puntos;
            LineaPrimaria linea = null;
            DateTime fecha_insercion = new DateTime(1999, 01, 01);
            DateTime fecha_actualizacion = new DateTime(1999, 01, 01);


            puntos = "";
            // StrSQL = "SELECT 'A' AS tipolinea, b.circuito, unico, x1, y1, x2, y2, NVL(x, 0) AS x,NVL(y, 0) AS y, NVL(orden, -1) AS orden, descripcion, descripcion[6, 7] AS calibrefases, descripcion[8, 9] AS materialfases, descripcion[10, 11] AS calibreneutro, descripcion[12, 13] AS materialneutro, orden_fases AS orden_fases, x1, y1, x2, y2, ubica, CASE WHEN troncal = '0' THEN 'Ramal' WHEN troncal = '1' THEN 'Troncal' END AS troncal, b.descripcion || '' AS d2, b.long AS longitud,c.arnom AS zona, 'NA' AS tipo, 'NA' AS nivel, 0 AS aislamiento, 0 AS amperes FROM linea_lp_m b, OUTER deflex_lp_m a,siad: areasres c WHERE b.div = '" + pDivision + "' AND b.zona = '" + pZona + "' AND x1 = lx1 AND y1 = ly1 AND x2 = lx2 AND y2 = ly2 AND b.circuito = '" + pCircuito + "' AND c.arare = b.zona AND c.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito, a.id AS unico, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2, NVL(b.x, 0) AS x, NVL(b.y, 0) AS y, - 1 AS orden, 'NA' AS descripcion, ccp AS calibrefases, mcp AS materialfases, cn AS calibreneutro, mcn AS materialneutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' AS d2, a.long AS longitud, e.arnom AS zona, a.tipocp AS tipo, a.nivelcp AS nivel, NVL(a.aislamiento, 0) AS aislamiento, a.amperes FROM sub_linmt a, sub_transmt b,sub_regmt c, siad:areasres e WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idtrans = b.id AND a.idreg2 = c.id AND a.idlm is null AND e.arare = b.zona AND e.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito,a.id AS UNICO, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2, d.x AS x, d.y AS y, orden, 'NA' AS descripcion, ccp AS calibrefases, mcp AS materialfases, cn AS calibreneutro, mcn AS materialneutro, a.fASes as orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' as d2,a.long AS longitud,e.arnom AS zona , a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes FROM sub_linmt a, sub_transmt b,sub_regmt c, sub_defmt d, siad: areASres e WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idtrans = b.id AND a.idreg2 = c.id AND a.idlm is null AND d.idlm = a.id AND e.arare = b.zona AND e.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito,a.id AS UNICO, c.x + b.dx AS x1, c.y + b.dy AS y1, d.x + a.dx AS x2, d.y + a.dy AS y2, 0 AS x, 0 AS y, - 1 AS orden, 'NA' AS descripcion, a.ccp AS calibrefases, a.mcp AS materialfases, a.cn AS calibreneutro, a.mcn AS materialneutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' as d2, a.long AS longitud, e.arnom AS zona , a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes FROM sub_linmt a, sub_linmt b,sub_regmt c, sub_regmt d, siad: areASres e WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idlm = b.id AND b.idreg2 = c.id AND a.idreg2 = d.id AND e.arare = b.zona AND e.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito,a.id AS UNICO, c.x + b.dx AS x1, c.y + b.dy AS y1, d.x + a.dx AS x2, d.y + a.dy AS y2, e.x AS x, e.y AS y, orden, 'NA' AS descripcion, a.ccp AS calibrefases, a.mcp AS materialfASes, a.cn AS calibreneutro, a.mcn AS materialneutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' AS d2, a.long AS longitud, f.arnom AS zona , a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes FROM sub_linmt a, sub_linmt b,sub_regmt c, sub_regmt d, sub_defmt e, siad:areASres f WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idlm = b.id AND b.idreg2 = c.id AND a.idreg2 = d.id AND e.idlm = a.id AND f.arare = b.zona AND f.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito,a.id AS UNICO, c.x + b.dx AS x1, c.y + b.dy AS y1, d.x + a.dx AS x2, d.y + a.dy AS y2, 0 AS x, 0 AS y, -1 AS orden, 'NA' AS descripcion, a.ccp AS calibrefASes, a.mcp AS materialfASes, a.cn AS calibreneutro, a.mcn AS materialneutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' as d2, a.long AS longitud, e.arnom AS zona, a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes FROM sub_linmt a, sub_linmt b,sub_regmt c, sub_transmt d, siad: areASres e WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idlm = b.id AND b.idreg2 = c.id AND a.idtrans = d.id AND a.idreg2 is null AND e.arare = b.zona AND e.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito,a.id AS UNICO, c.x + b.dx AS x1, c.y + b.dy AS y1, d.x + a.dx AS x2, d.y + a.dy AS y2, e.x AS x, e.y AS y, orden, 'NA' AS descripcion, a.ccp AS calibrefases, a.mcp AS materialfases, a.cn AS calibreneutro, a.mcn AS materialneutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' AS d2, a.long AS longitud, f.arnom AS zona, a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento, a.amperes FROM sub_linmt a, sub_linmt b,sub_regmt c, sub_transmt d, sub_defmt e, siad:areASres f WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idlm = b.id AND b.idreg2 = c.id AND a.idtrans = d.id AND a.idreg2 is null AND e.idlm = a.id AND f.arare = b.zona AND f.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito, a.id AS UNICO, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2, 0 AS x, 0 AS y, -1 AS orden, 'NA' AS descripcion, a.ccp AS calibrefases, a.mcp AS materialfases, a.cn AS calibreneutro, a.mcn AS materialneutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' AS d2, a.long AS longitud, e.arnom AS zona , a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes FROM sub_linmt a, nodo_fuente b, sub_regmt c, siad:areASres e WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idfuente = b.identificador AND a.idreg2 = c.id AND e.arare = b.zona AND e.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito,a.id, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2, d.x AS x, d.y AS y, orden, 'NA' AS descripcion, a.ccp AS calibrefASes, a.mcp AS materialfases, a.cn AS calibreneutro, a.mcn AS materialneutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' AS d2, a.long AS longitud, e.arnom AS zona , a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes FROM sub_linmt a, nodo_fuente b, sub_regmt c, sub_defmt d, siad: areASres e WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idfuente = b.identificador AND a.idreg2 = c.id AND d.idlm = a.id AND e.arare = b.zona AND e.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito,a.id AS UNICO, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2, 0 AS x, 0 AS y, - 1 AS orden, 'NA' AS descripcion, a.ccp AS calibrefASes, a.mcp AS materialfASes, a.cn AS calibreneutro, a.mcn AS materialneutro, a.fASes AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' AS d2, a.long AS longitud, e.arnom AS zona, a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes FROM sub_linmt a, nodo_fuente b, sub_transmt c, siad:areASres e WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idfuente = b.identificador AND a.idtrans = c.id AND idreg2 is null AND e.arare = b.zona AND e.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito, a.id AS UNICO, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2, d.x AS x, d.y AS y, orden, 'NA' AS descripcion, a.ccp AS calibrefASes, a.mcp AS materialfASes, a.cn AS calibreneutro, a.mcn AS materialneutro, a.fASes AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' AS d2, a.long AS longitud, e.arnom AS zona , a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes FROM sub_linmt a, nodo_fuente b, sub_transmt c, sub_defmt d, siad: areASres e WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idfuente = b.identificador AND a.idtrans = c.id AND idreg2 is null AND d.idlm = a.id AND e.arare = b.zona AND e.arare = '" + pZona + "' ORDER BY 1,3,10";
            StrSQL =    "select c.unico as id, x1,y1,x2,y2, 0 as x, 0 as y, -1 as orden,  a.identificador as idtr, 'NA' as idtrans, 'A' tipo, orden_fases fases," +
                        "mat_con_fases mat_fases, cal_con_fases cal_fases,mat_con_neutro mat_neutro, cal_con_neutro cal_neutro,c.fec_ins as fecha_insercion,"+
                        "c.fec_act as fecha_actualizacion,c.observaciones " +
                        "from trans_cfe_m a, nodo_lp_m b, linea_ls_m c "+
                        "where b.div = '"+pDivision+ "' and b.zona = '" + pZona + "' and circuito = '" + pCircuito + "' and a.div = b.div and a.zona = b.zona and a.x = b.x and a.y = b.y " +
                        "and c.identificador = a.identificador and c.div = a.div and c.zona = a.zona "+
                        "union "+
                        "select c.unico as id, x1,y1,x2,y2, d.x as x, d.y as y, orden,  a.identificador as idtr, 'NA' as idtrans, 'A' tipo, orden_fases fases,"+
                        "mat_con_fases mat_fases, cal_con_fases cal_fases, mat_con_neutro mat_neutro, cal_con_neutro cal_neutro,c.fec_ins as fecha_insercion,"+
                        "c.fec_act as fecha_actualizacion,c.observaciones "+
                        "from trans_cfe_m a, nodo_lp_m b, linea_ls_m c, deflex_ls_m d "+
                        "where b.div = '" + pDivision + "' and b.zona = '" + pZona + "' and circuito = '" + pCircuito + "' and a.div = b.div and a.zona = b.zona and a.x = b.x and a.y = b.y " +
                        "and c.identificador = a.identificador and c.div = a.div and c.zona = a.zona and lx1 = x1 and ly1 = y1 and lx2 = x2 and ly2 = y2 "+
                        "and d.div = c.div and d.zona = c.zona ";


            dt = conn.obtenerDT(StrSQL, pDivision);

            lineas = new List<LineaPrimaria>();


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

                    sql = "INSERT INTO linea_bta (idsiged,division, zona, circuito, idtrans, material_fases,calibre_fases,material_neutro, calibre_neutro, orden_fases,fecha_insercion,fecha_actualizacion,observaciones,coordenadas) "+
                          "Values (" + unico + ",'" + pDivision + "', '" + pZona + "' , '" + pCircuito + "' , " + Convert.ToInt16(dt.Rows[renglon]["idtr"]) + " , '" + dt.Rows[renglon]["mat_fases"].ToString().Trim() + "' , '" + dt.Rows[renglon]["cal_fases"].ToString().Trim() + "' , '" + dt.Rows[renglon]["mat_neutro"].ToString().Trim() + "' , '" + dt.Rows[renglon]["cal_neutro"].ToString().Trim() + "' , '" + dt.Rows[renglon]["fases"].ToString().Trim() + "',to_date('" + fecha_insercion + "','YY-MM-DD'),to_date('" + fecha_actualizacion + "','YY-MM-DD'),'" + dt.Rows[renglon]["observaciones"].ToString().Trim() + "'," + coor.TrimStart() + ")";
                   
                    try
                    {
                        // INSERTA LINEA BTA
                        command.CommandText = sql;
                        // REGRESA EL ID DE LA LINEA INSERTADA
                        Int64 id = Convert.ToInt64(command.ExecuteScalar());
                    }
                    catch (Exception msg)
                    {
                        // something went wrong, and you wanna know why
                        Console.WriteLine(msg.ToString());

                    }

                    puntos = "";
                    lnueva = true;
                }

            }


        } // fin del metodo

    }
}