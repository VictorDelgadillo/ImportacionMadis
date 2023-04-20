using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Siged_Spatial_ImportExportData
{
    public class Poste
    {
        public int Id { get; set; }
        public string Division { get; set; }
        public string Zona { get; set; }
        public string Nombrezona { get; set; }
        public string Circuito { get; set; }
        public Int64 Identificador { get; set; }
        public Int64 Cantidad { get; set; }
        public string Numero { get; set; }
        // public Coordenada Coordenadas { get; set; }
        public Double Latitud { get; set; }
        public Double Longitud { get; set; }
        public String Material { get; set; }
        public Int64 Altura { get; set; }
        public Int64 Resistencia { get; set; }
        public DateTime FechaInstalacion { get; set; }


        public Poste()
        {
        }

        public Poste(string division, string zona, string nombrezona, string circuito, int identificador, String numero, Double latitud, Double longitud, String material, Int64 altura, Int64 resistencia, DateTime fechaInstalacion, int cantidad)
        {
            Division = division;
            Zona = zona;
            Nombrezona = nombrezona;
            Circuito = circuito;
            Identificador = identificador;
            Numero = numero;
            Latitud = latitud;
            Longitud = longitud;
            Material = material;
            Altura = altura;
            Resistencia = resistencia;
            FechaInstalacion = fechaInstalacion;
            Cantidad = cantidad;
        }


        public void importaPostes(string pDivision, string pZona, string Pcircuito, NpgsqlCommand command,int UTM)
        {
            DataTable dt = new DataTable();
            Conexion conn = new Conexion();
          
            double[] pointsl;

            decimal px, py;
            String StrSQL;



            /* OBTENEMOS LOS DATOS POR POSTE
            =================================================== */

            StrSQL = "SELECT div as division,zona,identificador as circuito,x, y, unico,num_campo, material, altura, resistencia,cant_postes as cantidad_postes, fecha_ins as fecha_instalacion,fec_ins as fecha_insercion, fec_act as fecha_actualizacion, observaciones, urbana as uor,particular " +
            "FROM poste_m a WHERE div='" + pDivision + "' AND zona='" + pZona + "' AND identificador='" + Pcircuito + "' ";
         
           
            dt = conn.obtenerDT(StrSQL, pDivision);


            Int64 unico, altura, resistencia, cantidad_postes, num_campo, cantidad;
            string division, zona, material, uor, particular, observaciones, coordenada, circuito, niv_tension, tipo;
            DateTime fecha_instalacion = new DateTime();
            DateTime fecha_insercion = new DateTime();
            DateTime fecha_actualizacion = new DateTime();
            String result;
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    unico = Convert.ToInt64(row["unico"]);
                    division = row["division"].ToString();
                    zona = row["zona"].ToString();
                    if (row["altura"] != DBNull.Value){
                        altura = Convert.ToInt64(row["altura"]);
                    }
                    else
                    {
                        altura = 0;
                    }
                    if (row["resistencia"] != DBNull.Value)
                    {
                        resistencia = Convert.ToInt64(row["resistencia"]);
                    }
                    else
                    {
                        resistencia = 0;
                    }
                   
                    material = row["material"].ToString();
                    if (row["cantidad_postes"] != DBNull.Value)
                    {
                        cantidad_postes = Convert.ToInt64(row["cantidad_postes"]);
                    }
                    else
                    {
                        cantidad_postes = 0;
                    }
                  
                  //  num_campo = Convert.ToInt64(row["num_campo"]);
                    result = Regex.Replace(row["num_campo"].ToString(), @"[^\d]", "");
                    if(result == "") {
                        num_campo = 0;
                    }
                    else
                    {
                        num_campo = Convert.ToInt64(result);
                    }
               
                    //circuito = row["circuito"].ToString();
                    if (row["fecha_instalacion"] != DBNull.Value)
                    {
                        fecha_instalacion = Convert.ToDateTime(row["fecha_instalacion"]);
                    }
                    if (row["fecha_insercion"] != DBNull.Value)
                    {
                        fecha_insercion = Convert.ToDateTime(row["fecha_insercion"]);
                    }
                    if (row["fecha_actualizacion"] != DBNull.Value)
                    {
                        fecha_actualizacion = Convert.ToDateTime(row["fecha_actualizacion"]);
                    }
                    observaciones = row["observaciones"].ToString();
                    uor = (row["uor"].ToString() == "1") ? "S" : "N";
                    particular = (row["particular"].ToString() == "1") ? "S" : "N";
                    px = (decimal)row["x"];
                    py = (decimal)row["y"];

                    pointsl = Converter.UTMXYToLatLon((double)px, (double)py, UTM, false);

                    px = (decimal)Converter.RadToDeg(pointsl[0]);
                    py = (decimal)Converter.RadToDeg(pointsl[1]);
                                
                    coordenada = String.Format("﻿ST_GeomFromText('POINT({0} {1})', 4326)", py, px);
                    string sql = "INSERT INTO poste (idsiged, division, zona , altura , resistencia , material , cantidad_postes, fecha_instalacion , " +
                    "fecha_insercion, fecha_actualizacion,observaciones,urbana,particular,coordenada) " +
                    String.Format("Values ({0}, '{1}' , '{2}' , {3} , {4} , '{5}' ,'{6}', to_date('{7}','YY-MM-DD') , to_date('{8}','YY-MM-DD') , to_date('{9}','YY-MM-DD'), '{10}', '{11}', '{12}', {13} )",
                    unico,
                    division,
                    zona,
                    altura,
                    resistencia,
                    material,
                    cantidad_postes,
                    fecha_instalacion,
                    fecha_insercion,
                    fecha_actualizacion,
                    observaciones,
                    uor,
                    particular,
                    coordenada) + " ﻿RETURNING id";
                    sql = sql.Replace("\ufeff", " ");
                    // INSERTA POSTE
                    command.CommandText = sql;
                
                    // REGRES EL ID DEL POSTE INSERTADO
                    Int64 id = Convert.ToInt64(command.ExecuteScalar());
                    // INSERTA EL CONSECUTIVO POR CIRCUITO
                    sql = "INSERT INTO poste_cons (idposte, circuito, consecutivo) " +
                    String.Format("Values ({0}, '{1}' , '{2}')", id, Pcircuito, num_campo);
                    command.CommandText = sql;
                    command.ExecuteNonQuery();

                    // ESTRUCTURAS
                    // OBTENEMOS LAS ESTRUCTURAS DEL POSTE PARA INSERTARLAS NE L ANUEVA TABLA poste_estruct CON SU ID DE POSTGRES
                    sql = "SELECT niv_tension, tipo,cantidad FROM estructura_m WHERE " +
                    String.Format("x={0} and y={1}", (decimal)row["x"], (decimal)row["y"]);
                   
                    dt = conn.obtenerDT(sql, pDivision);
                    // POR CADA UNA DE SUS ESTRUCTURAS LAS INSERTAMOS EN SU RESPECTIVA TABLA
                    foreach (DataRow r in dt.Rows)
                    {
                        niv_tension = r["niv_tension"].ToString();
                        tipo = r["tipo"].ToString();
                        cantidad = Convert.ToInt64(r["cantidad"].ToString());
                        sql = "INSERT INTO poste_estruct (idposte, tipo, cantidad,niv_tension) " +
                        String.Format("Values ({0}, '{1}' , '{2}', '{3}')", id, tipo, cantidad, niv_tension);
                        command.CommandText = sql;
                        command.ExecuteNonQuery();

                    }

                    // INSERTAMOS RETENIDAS

                    sql = "SELECT unico,tipo,x,y,px,py,ax,ay FROM retenida_m WHERE " +
                    String.Format("px={0} and py={1}", (decimal)row["x"], (decimal)row["y"]);
                    dt = conn.obtenerDT(sql, pDivision);
                    // POR CADA UNA DE SUS RETENIDAS LAS INSERTAMOS EN SU RESPECTIVA TABLA
                    foreach (DataRow r in dt.Rows)
                    {
                        //niv_tension = r["niv_tension"].ToString();
                        unico = Convert.ToInt64(r["unico"]);
                        tipo = r["tipo"].ToString();
                        // cantidad = Convert.ToInt64(r["cantidad"].ToString());
                        px = (decimal)r["x"];
                        py = (decimal)r["y"];

                        pointsl = Converter.UTMXYToLatLon((double)px, (double)py, UTM, false);

                        px = (decimal)Converter.RadToDeg(pointsl[0]);
                        py = (decimal)Converter.RadToDeg(pointsl[1]);

                        Double angulo = Math.Atan2(Convert.ToDouble(r["y"]) - Convert.ToDouble(r["ay"]), Convert.ToDouble(r["x"]) - Convert.ToDouble(r["ax"]));
                        Double angulo2 = Converter.RadToDeg(angulo);
                        coordenada = String.Format("﻿ST_GeomFromText('POINT({0} {1})', 4326)", py, px);
                        sql = "INSERT INTO retenida (idsiged,idposte, division,zona,circuito, tipo, rotacion, coordenada) " +
                        String.Format("Values ({0}, {1} , '{2}' , '{3}', '{4}','{5}' , '{6}', {7})", unico, id, division, zona, Pcircuito.Trim(), tipo, Convert.ToInt16(angulo2), coordenada);
                        sql = sql.Replace("\ufeff", " ");
                        command.CommandText = sql;
                        command.ExecuteNonQuery();

                    }

                }
                catch (Exception msg)
                {
                    // something went wrong, and you wanna know why
                    Console.WriteLine(msg.ToString());
                }
                //postes.Add(new Poste(pDivision, pZona, nombrezona, circuito, identificador, numero, (double)px, (double)py, material, Convert.ToInt16(row["altura"]), Convert.ToInt16(row["resistencia"]), fecha_ins, cantidad));
            } // fin de foreach datarows

        } // fin de postes



    }// fin de la clase Poste
}
