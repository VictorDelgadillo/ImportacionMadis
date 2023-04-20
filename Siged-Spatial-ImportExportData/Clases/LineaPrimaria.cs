using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Siged_Spatial_ImportExportData
{

    public class LineaPrimaria
    {
        public string division { get; set; }
        public string zona { get; set; }
        public string circuito { get; set; }
        public string est_predominante { get; set; }
        public string calibre_fases { get; set; }
        public string material_fases { get; set; }
        public string calibre_neutro { get; set; }
        public string material_neutro { get; set; }
        public string fases_conectadas { get; set; }
        public string ubicacion { get; set; }
        public string orden_fases { get; set; }
        public DateTime fecha_insercion { get; set; }
        public DateTime fecha_actualizacion { get; set; }
        public string observaciones { get; set; }
        public string acometida { get; set; }
        public string troncal { get; set; }
        public string coordenadas { get; set; }

        public LineaPrimaria()
        {

        }
        public LineaPrimaria(string division, string zona, string circuito, string est_predominante, string calibre_fases, string material_fases, string calibre_neutro, string material_neutro, string fases_conectadas, string ubicacion, string orden_fases, DateTime fecha_insercion, DateTime fecha_actualizacion, string observaciones, string acometida, string troncal, string coordenadas)
        {
            this.division = division;
            this.zona = zona;
            this.circuito = circuito;
            this.est_predominante = est_predominante;
            this.calibre_fases = calibre_fases;
            this.material_fases = material_fases;
            this.calibre_neutro = calibre_neutro;
            this.material_neutro = material_neutro;
            this.fases_conectadas = fases_conectadas;
            this.ubicacion = ubicacion;
            this.orden_fases = orden_fases;
            this.fecha_insercion = fecha_insercion;
            this.fecha_actualizacion = fecha_actualizacion;
            this.observaciones = observaciones;
            this.acometida = acometida;
            this.troncal = troncal;
            this.coordenadas = coordenadas;
        }

        public void importaLineasMta(string pDivision, string pZona, string circuito, NpgsqlCommand command, int UTM)
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
            StrSQL = "SELECT unico, x1, y1, x2, y2, ubica, NVL(x, 0) AS x,NVL(y, 0) AS y, NVL(orden, -1) AS orden, " +
                     "b.div,b.zona,b.circuito,b.descripcion,  SUBSTR(b.descripcion,4,2) as est_predominante, SUBSTR(b.descripcion,6,2) as calibre_fases, " +
                     "SUBSTR(b.descripcion, 8, 2) as material_fases, SUBSTR(b.descripcion, 10, 2) as calibre_neutro, SUBSTR(b.descripcion, 12, 2) as material_neutro,b.fases_con as fases_conectadas,b.ubica as ubicacion, " +
                     "b.orden_fases,b.fec_ins as fecha_insercion,b.fec_act as fecha_actualizacion, " +
                     "b.observaciones,b.acometida,b.troncal " +
                     "FROM linea_lp_m b, OUTER deflex_lp_m a " +
                     "WHERE b.div = '" + pDivision + "' AND b.zona = '" + pZona + "' AND circuito= '" + circuito + "' " +
                     "AND lx1=x1 AND ly1=y1 AND lx2=x2 AND ly2=y2 order by unico, orden ";


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

                    if (!Convert.IsDBNull(dt.Rows[renglon]["fecha_insercion"]))
                    {
                        fecha_insercion = Convert.ToDateTime(dt.Rows[renglon]["fecha_insercion"]);
                    }

                    if (!Convert.IsDBNull(dt.Rows[renglon]["fecha_actualizacion"]))
                    {
                        fecha_actualizacion = Convert.ToDateTime(dt.Rows[renglon]["fecha_actualizacion"]);
                    }

                    linea = new LineaPrimaria(pDivision,
                                              pZona,
                                              dt.Rows[renglon]["circuito"].ToString().Trim(),
                                              dt.Rows[renglon]["est_predominante"].ToString().Trim(),
                                              dt.Rows[renglon]["calibre_fases"].ToString(),
                                              dt.Rows[renglon]["material_fases"].ToString(),
                                              dt.Rows[renglon]["calibre_neutro"].ToString(),
                                              dt.Rows[renglon]["material_neutro"].ToString(),
                                              dt.Rows[renglon]["fases_conectadas"].ToString(),
                                              dt.Rows[renglon]["ubicacion"].ToString().Trim(),
                                              dt.Rows[renglon]["orden_fases"].ToString().Trim(),
                                              fecha_insercion,
                                              fecha_actualizacion,
                                              dt.Rows[renglon]["observaciones"].ToString().Trim(),
                                              dt.Rows[renglon]["acometida"].ToString().Trim(),
                                              dt.Rows[renglon]["troncal"].ToString().Trim(),
                                              puntos.ToString().Trim());

                    string coor = "﻿ST_GeomFromText('" + linea.coordenadas.ToString().Trim() + "',4326)";
                    coor = coor.Replace("\ufeff", " ");

                    // VALIDAMOS SI EL CAMPO ACOEMTIDA ES 1 INSERTAMOS LA LINEA EN LA NUEVA TABLA acometidamta DE POSTGRES
                    if (linea.acometida == "1")
                    {
                        sql = "INSERT INTO acometida_mta (idsiged,division, zona, circuito, calibre_fases, material_fases, orden_fases,fecha_insercion,fecha_actualizacion,observaciones,coordenadas) " +
                              "Values (" + unico + ",'" + linea.division.ToString().Trim() + "', '" + linea.zona.ToString().Trim() + "' , '" + linea.circuito.ToString().Trim() + "' , '" + linea.calibre_fases.ToString().Trim() + "' , '" + linea.material_fases.ToString().Trim() + "','" + linea.orden_fases.TrimEnd() + "',to_date('" + linea.fecha_insercion + "','YY-MM-DD'),to_date('" + linea.fecha_actualizacion + "','YY-MM-DD'),'" + linea.observaciones.TrimEnd() + "'," + coor.TrimStart() + ")";
                    }
                    else
                    {
                        sql = "INSERT INTO linea_mta (idsiged,division, zona, circuito, est_predominante, calibre_fases, material_fases, calibre_neutro, material_neutro,ubicacion,orden_fases,fecha_insercion,fecha_actualizacion,observaciones,troncal,coordenadas) Values (" + unico + ",'" + linea.division.ToString().Trim() + "', '" + linea.zona.ToString().Trim() + "' , '" + linea.circuito.ToString().Trim() + "' , '" + linea.est_predominante.ToString().Trim() + "' , '" + linea.calibre_fases.ToString().Trim() + "' , '" + linea.material_fases.ToString().Trim() + "' , '" + linea.calibre_neutro.ToString().Trim() + "' , '" + linea.material_neutro.ToString().Trim() + "' , '" + linea.ubicacion + "', '" + linea.orden_fases.TrimEnd() + "',to_date('" + linea.fecha_insercion + "','YY-MM-DD'),to_date('" + linea.fecha_actualizacion + "','YY-MM-DD'),'" + linea.observaciones.TrimEnd() + "','" + linea.troncal.TrimEnd() + "'," + coor.TrimStart() + ")";
                    }
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
           
        } // fin del metodo

    }
}