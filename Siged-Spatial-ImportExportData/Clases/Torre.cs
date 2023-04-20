using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siged_Spatial_ImportExportData
{
    public static class Torre
    {
       
        public static void importaTorres(string pDivision, string pZona, NpgsqlCommand command,int UTM)
        {
            DataTable dt = new DataTable();
            Conexion conn = new Conexion();
           
            double[] pointsl;

            decimal tx, ty;
            string StrSQL;

            /* TORRES ALTA TENSION
            =================================================== */
            
            StrSQL =" SELECT me.num_torre,consecutivo,circuito,mc.div,mc.zona,me.cant_circ,tipo_torre, " +
                    " material,terreno,tipo_aislante,aisla_cade,uso,tipo_estruc,altura,altura_real, " +
                    " utr,fecha_inst,contaminacion,senalizacion,amort,desionizadores,bayonetas,protec_inc,protec_cat, " +
                    " muro,helipuerto,motooperador,apartarrayos,cant_apart,retenida,pararrayos,resori,respos,resact, " +
                    " sistierra,profelect,resterreno,fechamed,antiaves,cant_antiaves,accesso_cri,observaciones,angulo, " +
                    " a,me.fecha,foto,tx,ty  " +
                    " FROM m_estructuras me " +
                    " INNER JOIN m_conseestruct mc ON me.num_torre= mc.num_torre " +
                    " WHERE  mc.zona='"+pZona+"' order by consecutivo,circuito";

            dt = conn.obtenerDT(StrSQL, pDivision);

            string division, zona, tipo_torre,material, terreno, tipo_aislante, uso, tipo_estruc,utr, 
                   contaminacion, senalizacion, amort,  protec_inc, protec_cat,
                   muro,helipuerto,motooperador,apartarrayos, coordenada, observaciones, retenida, pararrayos,
                   sistierra, antiaves, accesso_cri,foto,linea, fecha_inst, fechamed;
         
            decimal altura,altura_real, resori, respos, resact, profelect, resterreno,angulo;
            Int16 idmadis,cant_lineas, aisla_cade, bayonetas, desionizadores, cant_apart, cant_antiaves,consecutivo;
            //DateTime fecha_inst = new DateTime();
            DateTime fecha = new DateTime();
          //  DateTime fechamed = new DateTime();

            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    idmadis = Convert.ToInt16(row["num_torre"]);
                    consecutivo = Convert.ToInt16(row["consecutivo"]);
                    linea = row["circuito"].ToString();
                    division = row["div"].ToString();
                    zona = row["zona"].ToString();
                    cant_lineas = Convert.ToInt16(row["cant_circ"]);
                    tipo_torre = row["tipo_torre"].ToString();
                    material = row["material"].ToString();
                    terreno = row["terreno"].ToString();
                    aisla_cade = Convert.ToInt16(row["aisla_cade"]);
                    tipo_aislante = row["tipo_aislante"].ToString();
                    uso = row["uso"].ToString();
                    tipo_estruc = row["tipo_estruc"].ToString();

                    altura = Convert.ToDecimal(row["altura"]);
                    altura_real = Convert.ToDecimal(row["altura_real"]);
                    utr = row["utr"].ToString();

                    fecha_inst = row["fecha_inst"].ToString();

                    //if (row["fecha_inst"] != DBNull.Value)
                    //{
                    //    fecha_inst = Convert.ToDateTime(row["fecha_inst"]);
                    //}

                    contaminacion = row["contaminacion"].ToString();

                    senalizacion = row["senalizacion"].ToString();

                    amort = row["amort"].ToString();
                    desionizadores = Convert.ToInt16(row["desionizadores"]);
                    bayonetas = Convert.ToInt16(row["bayonetas"]);

                    protec_inc = row["protec_inc"].ToString();

                    protec_cat = row["protec_cat"].ToString();
                    muro = row["muro"].ToString();
                    helipuerto = row["helipuerto"].ToString();

                    motooperador = row["motooperador"].ToString();

                    apartarrayos = row["apartarrayos"].ToString();

                    muro = row["muro"].ToString();

                    cant_apart = Convert.ToInt16(row["cant_apart"]);

                    retenida = row["retenida"].ToString();
                    pararrayos = row["pararrayos"].ToString();

                    resori = Convert.ToDecimal(row["resori"]);
                    respos = Convert.ToDecimal(row["respos"]);
                    resact = Convert.ToDecimal(row["resact"]);
                    profelect = Convert.ToDecimal(row["profelect"]);

                    sistierra = row["sistierra"].ToString();
                    resterreno = Convert.ToDecimal(row["resterreno"]);

                    fechamed = row["fechamed"].ToString();

                    //if (row["fechamed"] != DBNull.Value)
                    //{
                    //    fechamed = Convert.ToDateTime(row["fechamed"]);
                    //}

                    antiaves = row["antiaves"].ToString();

                    cant_antiaves = Convert.ToInt16(row["cant_antiaves"]);

                    accesso_cri = row["accesso_cri"].ToString();


                    observaciones = row["observaciones"].ToString();
                    angulo = Convert.ToDecimal(row["angulo"]);

                    if (row["fecha"] != DBNull.Value)
                    {
                        fecha = Convert.ToDateTime(row["fecha"]);
                    }
                    foto = row["foto"].ToString();


                    //if (row["loc"] != DBNull.Value)
                    //{
                    //    loc = row["loc"].ToString();
                    //    rotacion = ((int)Convert.ToChar(loc) - 65);
                    //    rotacion = rotacion / 30 * 180;

                    //}
                    //else
                    //    rotacion = 0;


                    tx = (decimal)row["tx"];
                    ty = (decimal)row["ty"];

                    pointsl = Converter.UTMXYToLatLon((double)tx, (double)ty, UTM, false);

                    tx = (decimal)Converter.RadToDeg(pointsl[0]);
                    ty = (decimal)Converter.RadToDeg(pointsl[1]);
                
                    coordenada = String.Format("﻿ST_GeomFromText('POINT({0} {1})', 4326)", ty, tx);
                    string sql = "INSERT INTO torre (idmadis, division, zona, cant_lineas,tipo_torre,material,terreno,tipo_aislante,aisla_cade,uso,tipo_estruc,altura,altura_real, " +
                    " utr,fecha_inst,contaminacion,senalizacion,amort,desionizadores,bayonetas,protec_inc,protec_cat, " +
                    " muro,helipuerto,motooperador,apartarrayos,cant_apart,retenida,pararrayos,resori,respos,resact, " +
                    " sistierra,profelect,resterreno,fechamed,antiaves,cant_antiaves,acceso_cri, observaciones, angulo,fecha,foto,coordenada) " +
                    String.Format(" Values ({0}, '{1}', '{2}', {3}, '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', {11}, {12}," +
                                  " '{13}','{14}','{15}','{16}','{17}',{18},{19},'{20}','{21}'," +
                                  " '{22}','{23}','{24}','{25}',{26},'{27}','{28}',{29},{30},{31}," +
                                  " '{32}', {33},{34},'{35}','{36}',{37},'{38}','{39}',{40},to_date('{41}','YY-MM-DD'),'{42}',{43})",

                    idmadis,division,zona,cant_lineas,tipo_torre, material, terreno, tipo_aislante, aisla_cade, uso, tipo_estruc, altura, altura_real,
                    utr,fecha_inst,contaminacion,senalizacion,amort,desionizadores,bayonetas,protec_inc,protec_cat, 
                    muro,helipuerto,motooperador,apartarrayos,cant_apart,retenida,pararrayos,resori,respos,resact, 
                    sistierra,profelect,resterreno,fechamed,antiaves,cant_antiaves,accesso_cri,observaciones,angulo,fecha,foto,coordenada) + " ﻿RETURNING id";
                    sql = sql.Replace("\ufeff", " ");
                    
                    // INSERTA TORRE
                    command.CommandText = sql;
                  //  command.ExecuteNonQuery();

                    // REGRESA EL ID DE LA TOORE INSERTADA
                    Int64 id = Convert.ToInt64(command.ExecuteScalar());
                    // INSERTA EL CONSECUTIVO POR CIRCUITO
                    sql = "INSERT INTO torre_cons (idtorre,consecutivo,linea) " +
                    String.Format("Values ({0}, {1} , '{2}')", id, consecutivo, linea);
                    command.CommandText = sql;
                    command.ExecuteNonQuery();

                }
                catch (Exception msg)
                {
                    // something went wrong, and you wanna know why
                    Console.WriteLine(msg.ToString());
                }
                
            } // fin de foreach datarows

        } // fin de TORRES
    }
}
