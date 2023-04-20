using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siged_Spatial_ImportExportData
{
   public static class Linea_mts
    {

        public static void importa(string pDivision, string pZona, string pCircuito, NpgsqlCommand command, int UTM)
        {
            DataTable dt = new DataTable();
            Conexion conn = new Conexion();
         
            double[] pointsl;
            //double[] pointsd;

            List<List<double>> deflexiones = null;
            List<double> deflexion = null;
            int unico = 0;
            int unicoA = 0;
            int unicoB = 0;
            int orden;
            Int64 aislamiento = 0;
            bool lnueva = true;

            string sql = "";
            string StrSQL;


           String puntos;
            //LineaPrimaria linea = null;
            DateTime fecha_insercion = new DateTime(1999, 01, 01);
            DateTime fecha_actualizacion = new DateTime(1999, 01, 01);


            puntos = "";
            // StrSQL = "SELECT 'A' AS tipolinea, b.circuito, unico, x1, y1, x2, y2, NVL(x, 0) AS x,NVL(y, 0) AS y, NVL(orden, -1) AS orden, descripcion, descripcion[6, 7] AS calibrefases, descripcion[8, 9] AS materialfases, descripcion[10, 11] AS calibreneutro, descripcion[12, 13] AS materialneutro, orden_fases AS orden_fases, x1, y1, x2, y2, ubica, CASE WHEN troncal = '0' THEN 'Ramal' WHEN troncal = '1' THEN 'Troncal' END AS troncal, b.descripcion || '' AS d2, b.long AS longitud,c.arnom AS zona, 'NA' AS tipo, 'NA' AS nivel, 0 AS aislamiento, 0 AS amperes FROM linea_lp_m b, OUTER deflex_lp_m a,siad: areasres c WHERE b.div = '" + pDivision + "' AND b.zona = '" + pZona + "' AND x1 = lx1 AND y1 = ly1 AND x2 = lx2 AND y2 = ly2 AND b.circuito = '" + pCircuito + "' AND c.arare = b.zona AND c.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito, a.id AS unico, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2, NVL(b.x, 0) AS x, NVL(b.y, 0) AS y, - 1 AS orden, 'NA' AS descripcion, ccp AS calibrefases, mcp AS materialfases, cn AS calibreneutro, mcn AS materialneutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' AS d2, a.long AS longitud, e.arnom AS zona, a.tipocp AS tipo, a.nivelcp AS nivel, NVL(a.aislamiento, 0) AS aislamiento, a.amperes FROM sub_linmt a, sub_transmt b,sub_regmt c, siad:areasres e WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idtrans = b.id AND a.idreg2 = c.id AND a.idlm is null AND e.arare = b.zona AND e.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito,a.id AS UNICO, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2, d.x AS x, d.y AS y, orden, 'NA' AS descripcion, ccp AS calibrefases, mcp AS materialfases, cn AS calibreneutro, mcn AS materialneutro, a.fASes as orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' as d2,a.long AS longitud,e.arnom AS zona , a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes FROM sub_linmt a, sub_transmt b,sub_regmt c, sub_defmt d, siad: areASres e WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idtrans = b.id AND a.idreg2 = c.id AND a.idlm is null AND d.idlm = a.id AND e.arare = b.zona AND e.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito,a.id AS UNICO, c.x + b.dx AS x1, c.y + b.dy AS y1, d.x + a.dx AS x2, d.y + a.dy AS y2, 0 AS x, 0 AS y, - 1 AS orden, 'NA' AS descripcion, a.ccp AS calibrefases, a.mcp AS materialfases, a.cn AS calibreneutro, a.mcn AS materialneutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' as d2, a.long AS longitud, e.arnom AS zona , a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes FROM sub_linmt a, sub_linmt b,sub_regmt c, sub_regmt d, siad: areASres e WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idlm = b.id AND b.idreg2 = c.id AND a.idreg2 = d.id AND e.arare = b.zona AND e.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito,a.id AS UNICO, c.x + b.dx AS x1, c.y + b.dy AS y1, d.x + a.dx AS x2, d.y + a.dy AS y2, e.x AS x, e.y AS y, orden, 'NA' AS descripcion, a.ccp AS calibrefases, a.mcp AS materialfASes, a.cn AS calibreneutro, a.mcn AS materialneutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' AS d2, a.long AS longitud, f.arnom AS zona , a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes FROM sub_linmt a, sub_linmt b,sub_regmt c, sub_regmt d, sub_defmt e, siad:areASres f WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idlm = b.id AND b.idreg2 = c.id AND a.idreg2 = d.id AND e.idlm = a.id AND f.arare = b.zona AND f.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito,a.id AS UNICO, c.x + b.dx AS x1, c.y + b.dy AS y1, d.x + a.dx AS x2, d.y + a.dy AS y2, 0 AS x, 0 AS y, -1 AS orden, 'NA' AS descripcion, a.ccp AS calibrefASes, a.mcp AS materialfASes, a.cn AS calibreneutro, a.mcn AS materialneutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' as d2, a.long AS longitud, e.arnom AS zona, a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes FROM sub_linmt a, sub_linmt b,sub_regmt c, sub_transmt d, siad: areASres e WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idlm = b.id AND b.idreg2 = c.id AND a.idtrans = d.id AND a.idreg2 is null AND e.arare = b.zona AND e.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito,a.id AS UNICO, c.x + b.dx AS x1, c.y + b.dy AS y1, d.x + a.dx AS x2, d.y + a.dy AS y2, e.x AS x, e.y AS y, orden, 'NA' AS descripcion, a.ccp AS calibrefases, a.mcp AS materialfases, a.cn AS calibreneutro, a.mcn AS materialneutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' AS d2, a.long AS longitud, f.arnom AS zona, a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento, a.amperes FROM sub_linmt a, sub_linmt b,sub_regmt c, sub_transmt d, sub_defmt e, siad:areASres f WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idlm = b.id AND b.idreg2 = c.id AND a.idtrans = d.id AND a.idreg2 is null AND e.idlm = a.id AND f.arare = b.zona AND f.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito, a.id AS UNICO, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2, 0 AS x, 0 AS y, -1 AS orden, 'NA' AS descripcion, a.ccp AS calibrefases, a.mcp AS materialfases, a.cn AS calibreneutro, a.mcn AS materialneutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' AS d2, a.long AS longitud, e.arnom AS zona , a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes FROM sub_linmt a, nodo_fuente b, sub_regmt c, siad:areASres e WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idfuente = b.identificador AND a.idreg2 = c.id AND e.arare = b.zona AND e.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito,a.id, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2, d.x AS x, d.y AS y, orden, 'NA' AS descripcion, a.ccp AS calibrefASes, a.mcp AS materialfases, a.cn AS calibreneutro, a.mcn AS materialneutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' AS d2, a.long AS longitud, e.arnom AS zona , a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes FROM sub_linmt a, nodo_fuente b, sub_regmt c, sub_defmt d, siad: areASres e WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idfuente = b.identificador AND a.idreg2 = c.id AND d.idlm = a.id AND e.arare = b.zona AND e.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito,a.id AS UNICO, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2, 0 AS x, 0 AS y, - 1 AS orden, 'NA' AS descripcion, a.ccp AS calibrefASes, a.mcp AS materialfASes, a.cn AS calibreneutro, a.mcn AS materialneutro, a.fASes AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' AS d2, a.long AS longitud, e.arnom AS zona, a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes FROM sub_linmt a, nodo_fuente b, sub_transmt c, siad:areASres e WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idfuente = b.identificador AND a.idtrans = c.id AND idreg2 is null AND e.arare = b.zona AND e.arare = '" + pZona + "' UNION SELECT 'S' AS tipolinea, a.circuito, a.id AS UNICO, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2, d.x AS x, d.y AS y, orden, 'NA' AS descripcion, a.ccp AS calibrefASes, a.mcp AS materialfASes, a.cn AS calibreneutro, a.mcn AS materialneutro, a.fASes AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' AS d2, a.long AS longitud, e.arnom AS zona , a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes FROM sub_linmt a, nodo_fuente b, sub_transmt c, sub_defmt d, siad: areASres e WHERE a.div = '" + pDivision + "' AND a.zona = '" + pZona + "' AND a.circuito = '" + pCircuito + "' AND a.idfuente = b.identificador AND a.idtrans = c.id AND idreg2 is null AND d.idlm = a.id AND e.arare = b.zona AND e.arare = '" + pZona + "' ORDER BY 1,3,10";
            StrSQL = "SELECT a.id AS unico, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2, NVL(b.x, 0) AS x, NVL(b.y, 0) AS y, -1 AS orden, " +
            "'NA' AS descripcion, ccp AS calibre_fases, mcp AS material_fases, cn AS calibre_neutro, mcn AS material_neutro, a.fases AS orden_fases, 0 AS x1, " +
            "0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica,'NA' AS troncal, 'NA' AS d2, a.long AS longitud, a.tipocp AS tipo, a.nivelcp AS nivel, " +
            "NVL(a.aislamiento, 0) AS aislamiento,a.amperes as sistema ,a.concentrico,a.diametroductos,a.materialductos,a.viasductos,a.tipoduc,a.ductoreserva, " +
            "a.disposicion,a.niveles,a.lineaductos ,a.conectividadi,a.conectividadf,a.observaciones,a.phd,a.urbana,a.fec_ins as fecha_insercion, " +
            "a.fec_act as fecha_actualizacion FROM sub_linmt a, sub_transmt b,sub_regmt c " +
            "WHERE a.div = '"+pDivision+"' AND a.zona = '"+pZona+"' AND a.circuito = '"+pCircuito+"' AND a.idtrans = b.id AND a.idreg2 = c.id AND a.idlm is null " +
            "UNION " +
            "SELECT a.id AS UNICO, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2, d.x AS x, d.y AS y, orden, 'NA' AS descripcion, ccp AS calibre_fases, " +
            "mcp AS material_fases, cn AS calibre_neutro, mcn AS material_neutro, a.fASes as orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, " +
            "'NA' AS troncal, 'NA' as d2, a.long AS longitud, a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes as sistema ,a.concentrico, " +
            "a.diametroductos,a.materialductos,a.viasductos,a.tipoduc,a.ductoreserva,a.disposicion,a.niveles,a.lineaductos ,a.conectividadi,a.conectividadf, " +
            "a.observaciones,a.phd,a.urbana,a.fec_ins as fecha_insercion,a.fec_act as fecha_actualizacion FROM sub_linmt a, sub_transmt b,sub_regmt c, sub_defmt d " +
            "WHERE a.div = '"+pDivision+"' AND a.zona = '"+pZona+"' AND a.circuito = '"+pCircuito+"' AND a.idtrans = b.id AND a.idreg2 = c.id AND a.idlm is null " +
            "AND d.idlm = a.id " +
            "UNION " +
            "SELECT a.id AS UNICO, c.x + b.dx AS x1, c.y + b.dy AS y1, d.x + a.dx AS x2, d.y + a.dy AS y2, 0 AS x, 0 AS y, -1 AS orden, " +
            "'NA' AS descripcion, a.ccp AS calibre_fases, a.mcp AS material_fases, a.cn AS calibre_neutro, a.mcn AS material_neutro, a.fases AS orden_fases, " +
            "0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' as d2,a.long AS longitud, a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento, " +
            "a.amperes as sistema ,a.concentrico,a.diametroductos,a.materialductos,a.viasductos,a.tipoduc,a.ductoreserva,a.disposicion,a.niveles,a.lineaductos , " +
            "a.conectividadi,a.conectividadf,a.observaciones,a.phd,a.urbana,a.fec_ins as fecha_insercion,a.fec_act as fecha_actualizacion " +
            "FROM sub_linmt a, sub_linmt b,sub_regmt c, sub_regmt d WHERE a.div = '"+pDivision+"' AND a.zona = '"+pZona+"' AND a.circuito = '"+pCircuito+"' " +
            "AND a.idlm = b.id AND b.idreg2 = c.id AND a.idreg2 = d.id " +
            "UNION " +
            "SELECT a.id AS UNICO, c.x + b.dx AS x1, c.y + b.dy AS y1, d.x + a.dx AS x2, d.y + a.dy AS y2, e.x AS x, e.y AS y, orden, 'NA' AS descripcion, " +
            "a.ccp AS calibre_fases, a.mcp AS material_fases, a.cn AS calibre_neutro, a.mcn AS material_neutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, " +
            "0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' AS d2, a.long AS longitud, a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes as sistema, " +
            "a.concentrico,a.diametroductos,a.materialductos,a.viasductos,a.tipoduc,a.ductoreserva,a.disposicion,a.niveles,a.lineaductos ,a.conectividadi, " +
            "a.conectividadf,a.observaciones,a.phd,a.urbana,a.fec_ins as fecha_insercion,a.fec_act as fecha_actualizacion " +
            "FROM sub_linmt a, sub_linmt b,sub_regmt c, sub_regmt d, sub_defmt e WHERE a.div = '"+pDivision+"' AND a.zona = '"+pZona+"' AND " +
            "a.circuito = '"+pCircuito+"' AND a.idlm = b.id AND b.idreg2 = c.id AND a.idreg2 = d.id AND e.idlm = a.id " +
            "UNION " +
            "SELECT a.id AS UNICO, c.x + b.dx AS x1,c.y + b.dy AS y1, d.x + a.dx AS x2, d.y + a.dy AS y2, 0 AS x, 0 AS y, -1 AS orden, 'NA' AS descripcion, " +
            "a.ccp AS calibre_fases, a.mcp AS material_fases, a.cn AS calibre_neutro, a.mcn AS material_neutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, " +
            "0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' as d2, a.long AS longitud, a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes as sistema, " +
            "a.concentrico,a.diametroductos,a.materialductos,a.viasductos,a.tipoduc,a.ductoreserva,a.disposicion,a.niveles,a.lineaductos ,a.conectividadi, " +
            "a.conectividadf,a.observaciones,a.phd,a.urbana,a.fec_ins as fecha_insercion,a.fec_act as fecha_actualizacion " +
            "FROM sub_linmt a, sub_linmt b,sub_regmt c, sub_transmt d WHERE a.div = '"+pDivision+"' AND a.zona = '"+pZona+"' AND a.circuito = '"+pCircuito+"' " +
            "AND a.idlm = b.id AND b.idreg2 = c.id AND a.idtrans = d.id AND a.idreg2 is null " + 
            "UNION " +
            "SELECT a.id AS UNICO,c.x + b.dx AS x1, c.y + b.dy AS y1, " +
            "d.x + a.dx AS x2, d.y + a.dy AS y2, e.x AS x, e.y AS y, orden, 'NA' AS descripcion, a.ccp AS calibre_fases, a.mcp AS material_fases, " +
            "a.cn AS calibre_neutro, a.mcn AS material_neutro, a.fases AS orden_fases,0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal, 'NA' AS d2, " +
            "a.long AS longitud,a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento, a.amperes as sistema ,a.concentrico,a.diametroductos,a.materialductos, " +
            "a.viasductos,a.tipoduc,a.ductoreserva,a.disposicion,a.niveles,a.lineaductos ,a.conectividadi,a.conectividadf,a.observaciones,a.phd,a.urbana, " +
            "a.fec_ins as fecha_insercion,a.fec_act as fecha_actualizacion FROM sub_linmt a, sub_linmt b,sub_regmt c, sub_transmt d, sub_defmt e " +
            "WHERE a.div = '"+pDivision+"' AND a.zona = '"+pZona+"' AND a.circuito = '"+pCircuito+"' AND a.idlm = b.id AND b.idreg2 = c.id AND a.idtrans = d.id  " +
            "AND a.idreg2 is null AND e.idlm = a.id " +
            "UNION " +
            "SELECT a.id AS UNICO, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2,0 AS x, 0 AS y, -1 AS orden, 'NA' AS descripcion, " +
            "a.ccp AS calibre_fases, a.mcp AS material_fases,a.cn AS calibre_neutro, a.mcn AS material_neutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, " +
            "0 AS y2,'N' AS ubica, 'NA' AS troncal, 'NA' AS d2, a.long AS longitud, a.tipocp AS tipo, a.nivelcp AS nivel, a.aislamiento,a.amperes as sistema, " +
            "a.concentrico,a.diametroductos,a.materialductos,a.viasductos,a.tipoduc,a.ductoreserva,a.disposicion,a.niveles,a.lineaductos ,a.conectividadi, " +
            "a.conectividadf,a.observaciones,a.phd,a.urbana,a.fec_ins as fecha_insercion,a.fec_act as fecha_actualizacion FROM sub_linmt a, nodo_fuente b, " +
            "sub_regmt c WHERE a.div = '"+pDivision+"' AND a.zona = '"+pZona+"' AND a.circuito = '"+pCircuito+"' AND a.idfuente = b.identificador AND a.idreg2 = c.id " +
            "UNION " +
            "SELECT a.id, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2, d.x AS x, d.y AS y,orden, 'NA' AS descripcion, a.ccp AS calibre_fases, " +
            "a.mcp AS material_fases, a.cn AS calibre_neutro, a.mcn AS material_neutro, a.fases AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, " +
            "'NA' AS troncal,'NA' AS d2, a.long AS longitud, a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes as sistema ,a.concentrico, " +
            "a.diametroductos,a.materialductos,a.viasductos,a.tipoduc,a.ductoreserva,a.disposicion,a.niveles,a.lineaductos ,a.conectividadi,a.conectividadf, " +
            "a.observaciones,a.phd,a.urbana,a.fec_ins as fecha_insercion,a.fec_act as fecha_actualizacion FROM sub_linmt a, nodo_fuente b, sub_regmt c, sub_defmt d " +
            "WHERE a.div = '"+pDivision+"' AND a.zona = '"+pZona+"' AND a.circuito = '"+pCircuito+"' AND a.idfuente = b.identificador AND a.idreg2 = c.id AND d.idlm = a.id " +
            "UNION " +
            "SELECT a.id AS UNICO, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2, 0 AS x, 0 AS y, -1 AS orden, 'NA' AS descripcion, " +
            "a.ccp AS calibre_fases, a.mcp AS material_fases, a.cn AS calibre_neutro, a.mcn AS material_neutro, a.fASes AS orden_fases, 0 AS x1, 0 AS y1,0 AS x2, " +
            "0 AS y2, 'N' AS ubica, 'NA' AS troncal,'NA' AS d2, a.long AS longitud, a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento,a.amperes as sistema, " +
            "a.concentrico,a.diametroductos,a.materialductos,a.viasductos,a.tipoduc,a.ductoreserva,a.disposicion,a.niveles,a.lineaductos ,a.conectividadi, " +
            "a.conectividadf,a.observaciones,a.phd,a.urbana,a.fec_ins as fecha_insercion,a.fec_act as fecha_actualizacion FROM sub_linmt a, nodo_fuente b, " +
            "sub_transmt c WHERE a.div = '"+pDivision+"' AND a.zona = '"+pZona+"' AND a.circuito = '"+pCircuito+"' AND a.idfuente = b.identificador AND " +
            "a.idtrans = c.id AND idreg2 is null " +
            "UNION " +
            "SELECT a.id AS UNICO, b.x AS x1, b.y AS y1, c.x + dx AS x2, c.y + dy AS y2, d.x AS x, d.y AS y, orden, " +
            "'NA' AS descripcion, a.ccp AS calibre_fases, a.mcp AS material_fases, a.cn AS calibre_neutro, a.mcn AS material_neutro, a.fASes AS orden_fases, " +
            "0 AS x1, 0 AS y1,0 AS x2, 0 AS y2, 'N' AS ubica, 'NA' AS troncal,'NA' AS d2, a.long AS longitud, a.tipocp AS tipo,a.nivelcp AS nivel, a.aislamiento, " +
            "a.amperes as sistema ,a.concentrico,a.diametroductos,a.materialductos,a.viasductos,a.tipoduc,a.ductoreserva,a.disposicion,a.niveles,a.lineaductos, " +
            "a.conectividadi,a.conectividadf,a.observaciones,a.phd,a.urbana,a.fec_ins as fecha_insercion,a.fec_act as fecha_actualizacion FROM sub_linmt a, " +
            "nodo_fuente b, sub_transmt c, sub_defmt d WHERE a.div = '"+pDivision+"' AND a.zona = '"+pZona+"' AND a.circuito = '"+pCircuito+"' " +
            "AND a.idfuente = b.identificador AND a.idtrans = c.id AND idreg2 is null AND d.idlm = a.id ORDER BY 1,8";
            dt = conn.obtenerDT(StrSQL, pDivision);

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
                    string calibre_fases = "";
                    if (!Convert.IsDBNull(dt.Rows[renglon]["calibre_fases"]))
                    {
                         calibre_fases = dt.Rows[renglon]["calibre_fases"].ToString();
                    }
                    string material_fases = "";
                    if (!Convert.IsDBNull(dt.Rows[renglon]["material_fases"]))
                    {
                        material_fases = dt.Rows[renglon]["material_fases"].ToString();
                    }
                    string calibre_neutro = "";
                    if (!Convert.IsDBNull(dt.Rows[renglon]["calibre_neutro"]))
                    {
                        calibre_neutro = dt.Rows[renglon]["calibre_neutro"].ToString();
                    }
                    string material_neutro = "";
                    if (!Convert.IsDBNull(dt.Rows[renglon]["material_neutro"]))
                    {
                        material_neutro = dt.Rows[renglon]["material_neutro"].ToString();
                    }

                    string nivel = "";
                    if (!Convert.IsDBNull(dt.Rows[renglon]["nivel"]))
                    {
                        nivel = dt.Rows[renglon]["nivel"].ToString();
                    }

                    string concentrico = "";
                    if (!Convert.IsDBNull(dt.Rows[renglon]["concentrico"]))
                    {
                        concentrico = dt.Rows[renglon]["concentrico"].ToString();
                    }

                    string sistema = "";
                    if (!Convert.IsDBNull(dt.Rows[renglon]["sistema"]))
                    {
                        sistema = dt.Rows[renglon]["sistema"].ToString();
                    }

                    string diametroductos = "";
                    if (!Convert.IsDBNull(dt.Rows[renglon]["diametroductos"]))
                    {
                        diametroductos = dt.Rows[renglon]["diametroductos"].ToString();
                    }

                    string materialductos = "";
                    if (!Convert.IsDBNull(dt.Rows[renglon]["materialductos"]))
                    {
                        materialductos = dt.Rows[renglon]["materialductos"].ToString();
                    }

                    string viasductos = "";
                    if (!Convert.IsDBNull(dt.Rows[renglon]["viasductos"]))
                    {
                        viasductos = dt.Rows[renglon]["viasductos"].ToString();
                    }


                    string tipoduc = "";
                    if (!Convert.IsDBNull(dt.Rows[renglon]["tipoduc"]))
                    {
                        tipoduc = dt.Rows[renglon]["tipoduc"].ToString();
                    }

                    string ductoreserva = "";
                    if (!Convert.IsDBNull(dt.Rows[renglon]["ductoreserva"]))
                    {
                        ductoreserva = dt.Rows[renglon]["ductoreserva"].ToString();
                    }

                    string disposicion = "NA";
                    if (!Convert.IsDBNull(dt.Rows[renglon]["disposicion"]))
                    {
                        disposicion = dt.Rows[renglon]["disposicion"].ToString();
                    }

                    string orden_fases = "";
                    if (!Convert.IsDBNull(dt.Rows[renglon]["orden_fases"]))
                    {
                        orden_fases = dt.Rows[renglon]["orden_fases"].ToString();
                    }


                    Int64 niveles = 0;
                    if (!Convert.IsDBNull(dt.Rows[renglon]["niveles"]))
                    {
                        niveles = Convert.ToInt64(dt.Rows[renglon]["niveles"]);
                    }

                    Int64 lineaductos = 0;
                    if (!Convert.IsDBNull(dt.Rows[renglon]["lineaductos"]))
                    {
                        lineaductos = Convert.ToInt64(dt.Rows[renglon]["lineaductos"]);
                    }
                    
                    if (!Convert.IsDBNull(dt.Rows[renglon]["aislamiento"]))
                    {
                        aislamiento = Convert.ToInt16(dt.Rows[renglon]["aislamiento"]);
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
              
                    sql = "INSERT INTO linea_mts (idsiged,division, zona, circuito,orden_fases,calibre_fases, material_fases, calibre_neutro, material_neutro,aislamiento,nivel_aislamiento, " + 
                          "concentrico,sistema,diametroductos,materialductos,viasductos,tipoducto,ductoreserva,disposicion,niveles,lineaductos,conectadaini,conectadafin,observaciones, " +
                          "phd,urbana,fecha_insercion,fecha_actualizacion,coordenadas) Values "+ 
                          "(" + unico + ",'" + pDivision + "', '" + pZona + "' , '" + pCircuito + "' , '" + orden_fases + "' ,'" + calibre_fases + "' , '" + material_fases + "' , '" + calibre_neutro + "' , '" + material_neutro + "' , " + aislamiento + " , '" + nivel + "', '" +concentrico + "','" + sistema + "', '" +diametroductos + "', '" + materialductos + "', " + viasductos + ", '" + tipoduc + "', '" + ductoreserva + "', '" + disposicion + "', " + niveles + ", " + lineaductos + ", '" + dt.Rows[renglon]["conectividadi"].ToString().Trim() + "', '" + dt.Rows[renglon]["conectividadf"].ToString().Trim() + "', '" + dt.Rows[renglon]["observaciones"].ToString().Trim() + "', '" + dt.Rows[renglon]["phd"].ToString().Trim() + "', '" + dt.Rows[renglon]["urbana"].ToString().Trim() + "',to_date('" + fecha_insercion + "','YY-MM-DD'),to_date('" + fecha_actualizacion + "','YY-MM-DD')," +  coor.TrimStart() + ")";
                  
                    // Hace Insert
                    try
                    {
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                    catch (Exception msg)
                    {
                       
                        Console.WriteLine(msg.ToString());

                    }

                    puntos = "";
                    lnueva = true;
                }

            }
        }  // fin del método importa
    }
}
