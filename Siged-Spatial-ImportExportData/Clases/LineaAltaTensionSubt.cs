using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siged_Spatial_ImportExportData
{
    public static class LineaAltaTensionSubt
    {

        public static void importaLineasAtsubt(string pDivision, string pZona,  NpgsqlCommand command, int UTM)
        {
            DataTable dt = new DataTable();
            Conexion conn = new Conexion();

            double[] pointsl;
            string StrSQL;
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
            StrSQL = "SELECT l.num_linea as unico,l.xi as x1,l.yi as y1,l.xf as x2,l.yf as y2,NVL(dx, 0) AS x,NVL(dy, 0) AS y, NVL(orden, -1) AS orden, " +
                    "l.div,l.zona,l.linea,calibre_li,material_li,calibre_li,material_li, " +
                    "long, observaciones, fibra, l.fecha, " +
                    "aislamiento_li, " +
                              "material_n,calibre_n,id_ductos,num_vias,diametro,material,tipo_construccion,long,observaciones, " +
                              "fibra,fibra_marca,fibra_tipocable,fibra_tipofibra,fibra_num,fibra_tiposujecion "+
                    "FROM m_sub_lineas l, outer m_sub_deflexiones d " +
                    "WHERE l.div = '" +pDivision+"' AND l.zona = '"+pZona+ "' " +
                    "AND l.num_linea = d.num_linea  order by l.num_linea, orden  ";


            dt = conn.obtenerDT(StrSQL, pDivision);

          

            string division, zona, nomlinea, calibre_li, material_li,observaciones,fibra;

            decimal longi;

            Int64 idmadis;
          
            DateTime fecha_ins = new DateTime();




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

                unico = Convert.ToInt32(dt.Rows[renglon]["unico"]);
                unicoA = Convert.ToInt32(dt.Rows[renglon]["unico"]);
                orden = Convert.ToInt32(dt.Rows[renglon]["orden"]);

                if (renglon < dt.Rows.Count - 1)
                {
                    unicoB = Convert.ToInt32(dt.Rows[renglon + 1]["unico"]);
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


                    nomlinea = dt.Rows[renglon]["linea"].ToString();
                    division = dt.Rows[renglon]["div"].ToString();
                    zona = dt.Rows[renglon]["zona"].ToString();
                    nomlinea = dt.Rows[renglon]["linea"].ToString();
                    calibre_li = dt.Rows[renglon]["calibre_li"].ToString();
                    material_li = dt.Rows[renglon]["material_li"].ToString();

                    string aislamiento_li = dt.Rows[renglon]["aislamiento_li"].ToString();
                    string material_n = dt.Rows[renglon]["material_n"].ToString();
                    string calibre_n = dt.Rows[renglon]["calibre_n"].ToString();

                    int  id_ductos = Convert.ToInt32(dt.Rows[renglon]["id_ductos"]);

                    int num_vias = Convert.ToInt32(dt.Rows[renglon]["num_vias"]);
                    decimal diametro = Convert.ToDecimal(dt.Rows[renglon]["diametro"]);
                    string material = dt.Rows[renglon]["material"].ToString();
                    string tipo_construccion = dt.Rows[renglon]["tipo_construccion"].ToString();

                    observaciones = dt.Rows[renglon]["observaciones"].ToString();
                    fibra = dt.Rows[renglon]["fibra"].ToString();

                    longi = Convert.ToDecimal(dt.Rows[renglon]["long"]);


                    idmadis = Convert.ToInt64(dt.Rows[renglon]["unico"]);

                    string fibra_marca = dt.Rows[renglon]["fibra_marca"].ToString();
                    string fibra_tipocable = dt.Rows[renglon]["fibra_tipocable"].ToString();
                    string fibra_tipo = dt.Rows[renglon]["fibra_tipofibra"].ToString();

                    int fibra_num = 0;
                    if (dt.Rows[renglon]["fibra_num"] != DBNull.Value)
                    {
                        fibra_num = Convert.ToInt16(dt.Rows[renglon]["fibra_num"]);
                    }



                   

                    string fibra_tiposujecion = dt.Rows[renglon]["fibra_tiposujecion"].ToString();






                    if (!Convert.IsDBNull(dt.Rows[renglon]["fecha"]))
                    {
                        fecha_ins = Convert.ToDateTime(dt.Rows[renglon]["fecha"]);
                    }

                    string coor = "﻿ST_GeomFromText('" + puntos.ToString().Trim() + "',4326)";
                    coor = coor.Replace("\ufeff", " ");

                        sql = "INSERT INTO linea_ats (idmadis,division, zona, linea,calibre_li,material_li,aislamiento_li, " +
                              "material_n,calibre_n,id_ductos,num_vias,diametro,material,tipo_construccion,long,observaciones, " +
                              "fibra,fecha_inst,fibra_marca,fibra_tipocable,fibra_tipo,fibra_num,fibra_tiposujecion,coordenadas) Values (" + 
                               unico + ",'" + pDivision + "', '" + pZona + "' , '" + nomlinea + "' , '" + calibre_li + "' , '" + material_li + "' , '" + aislamiento_li + 
                              "' , '" + material_n + "' , '" + calibre_n + "' , " + id_ductos +  ", " + num_vias + "," + diametro + ",'" + material+ "','" + tipo_construccion + "',"+ longi + ",'"+ observaciones + "','"+
                              fibra + "',to_date('" + fecha_ins + "','YY-MM-DD'),'" + fibra_marca  + "','" + fibra_tipocable + "','" + fibra_tipo + "'," + fibra_num + ",'" + fibra_tiposujecion + "'," + coor.TrimStart() + ")";
                   
                    // Hace Insert
                    try
                    {
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
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

        }
    }
}
