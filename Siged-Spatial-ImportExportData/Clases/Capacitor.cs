using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siged_Spatial_ImportExportData
{
    public static class Capacitor
    {
       
        public static void importaCapacitores(string pDivision, string pZona, string Pcircuito, NpgsqlCommand command,int UTM)
        {
            DataTable dt = new DataTable();
            Conexion conn = new Conexion();
           
            double[] pointsl;

            decimal px, py;
            string StrSQL;

            /* CAPACITORES
            =================================================== */
            
            StrSQL =" SELECT c.div as division, c.zona, ne, c.loc, c.identificador, tipo_banco, capacidad_t, tipo_conex, tipo_Control, observaciones, c.x , c.y, c.fec_ins as fecha_insercion, c.fec_act as fecha_actualizacion  " +
                    " FROM capacitor_m c, nodo_lp_m n " +
                    " WHERE n.div='" + pDivision + "' and n.zona='" + pZona + "' and circuito= '" + Pcircuito + "' and c.x = n.x and c.y = n.y";
            dt = conn.obtenerDT(StrSQL, pDivision);

            string division, zona, zonaNombre, numeco, identificador, tipobanco, tipoconex, tipocontrol, loc, coordenada, observaciones;
            double capacidad;
            decimal rotacion;
            
            DateTime fecha_insercion = new DateTime();
            DateTime fecha_actualizacion = new DateTime();

            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    division = row["division"].ToString();
                    zona = row["zona"].ToString();
                    zonaNombre = row["arnom"].ToString().Trim();
                    numeco = row["ne"].ToString().Trim();
                    identificador = row["identificador"].ToString().Trim();
                    tipobanco = row["tipo_banco"].ToString().Trim();
                    capacidad = Convert.ToInt64(row["capacidad_t"]);
                    tipoconex = row["tipo_conex"].ToString().Trim();
                    tipocontrol = row["tipo_control"].ToString().Trim();

                    if (row["fecha_insercion"] != DBNull.Value)
                    {
                        fecha_insercion = Convert.ToDateTime(row["fecha_insercion"]);
                    }
                    if (row["fecha_actualizacion"] != DBNull.Value)
                    {
                        fecha_actualizacion = Convert.ToDateTime(row["fecha_actualizacion"]);
                    }
                    observaciones = row["observaciones"].ToString();

                    if (row["loc"] != DBNull.Value)
                    {
                        loc = row["loc"].ToString();
                        rotacion = ((int)Convert.ToChar(loc) - 65);
                        rotacion = rotacion / 30 * 180;

                    }
                    else
                        rotacion = 0;

                    //if (row["loc"] != DBNull.Value)
                    //{
                    //    loc = row["loc"].ToString();
                    //    rotacion = (decimal)((((int)Convert.ToChar(loc) - 65) / 30) * 180);
                    //}
                    //else
                    //    rotacion = 0;

                    px = (decimal)row["x"];
                    py = (decimal)row["y"];

                    pointsl = Converter.UTMXYToLatLon((double)px, (double)py, UTM, false);

                    px = (decimal)Converter.RadToDeg(pointsl[0]);
                    py = (decimal)Converter.RadToDeg(pointsl[1]);
                
                    coordenada = String.Format("﻿ST_GeomFromText('POINT({0} {1})', 4326)", py, px);
                    string sql = "INSERT INTO capacitor (idsiged, division, zona, circuito, economico, tipo_banco, capacidad, tipo_conexion, rotacion, tipo_control, fecha_insercion, fecha_actualizacion, observaciones, coordenada) " +
                    String.Format("Values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, '{7}', {8}, '{9}', to_date('{10}','YY-MM-DD'), to_date('{11}','YY-MM-DD'), '{12}', {13} )",
                    identificador,
                    division,
                    zona,
                    Pcircuito,
                    numeco,
                    tipobanco,
                    capacidad,
                    tipoconex,
                    rotacion,
                    tipocontrol,
                    fecha_insercion,
                    fecha_actualizacion,
                    observaciones,
                    coordenada) + " ﻿RETURNING id";
                    sql = sql.Replace("\ufeff", " ");
                    
                    // INSERTA CAPACITOR
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                    
                }
                catch (Exception msg)
                {
                    // something went wrong, and you wanna know why
                    Console.WriteLine(msg.ToString());
                }
                
            } // fin de foreach datarows

        } // fin de capacitores
    }
}
