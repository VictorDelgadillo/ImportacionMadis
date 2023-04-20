using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siged_Spatial_ImportExportData
{
    class Zona
    {
        public string Id;
        public string Nombre;

        public Zona(string id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        public override string ToString()
        {
            return Nombre;
        }


        public static List<Zona> getZonas(String division)
        {
            List<Zona> zonas = new List<Zona>();

            //string query = "SELECT division,nombre FROM divisiones WHERE SUBSTRING(division,1,1)='D' ORDER BY division";
            //string divi = "";
            //string nom = "";
            //// FUNCION DE LA CLASE DBConect PARA ABRIR LA CONEXION
            //if (DBConnect.OpenConnection("sisnae_d0") == true)
            //{

            //    MySqlCommand cmd = new MySqlCommand(query, DBConnect.connection);

            //    MySqlDataReader dataReader = cmd.ExecuteReader();

            //    while (dataReader.Read())
            //    {
            //        divi = (string)dataReader["division"];
            //        nom = (string)dataReader["nombre"];
            //        divisiones.Add(new Division(divi, nom));

            //    }
            //    dataReader.Close();
            //    DBConnect.CloseConnection();
            //    divisiones.Add(new Division("dev", "PRUEBA"));


            //}
            //else
            //{
            //    return null;
            //}

            return zonas;
        }
    }
}
