using P0006.Models;
using P0006.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace P0006.Metodos
{
    public class TiposMetodos
    {
        private static TiposMetodos _instancia = null;

        public TiposMetodos()
        {
            
        }

        public static TiposMetodos Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new TiposMetodos();
                }
                return _instancia;
            }
        }
        
        public List<Tipos>Listar()
        { 
            List<Tipos> oListaTipos = new List<Tipos>();

            using (SqlConnection oCnn = new SqlConnection(Conexion.Bd))
            {
                SqlCommand cmd = new SqlCommand("sp_consultaTipos", oCnn);
                cmd.CommandType = CommandType.StoredProcedure;

                oCnn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    oListaTipos.Add(new Tipos()
                    {
                        IdTipo = Convert.ToInt32(dr["IDTipo"].ToString()),
                        Descripcion = dr["Descripcion"].ToString(),
                        Estatus = Convert.ToBoolean(dr["Estatus"].ToString()),
                        Imagen = dr["Imagen"] != DBNull.Value ? (byte[])dr["Imagen"] : null,
                        ImagenBase64 = dr["Imagen"] != DBNull.Value ? Convert.ToBase64String((byte[])dr["Imagen"]) : null
                    });
                }
                dr.Close();

                return oListaTipos;
            }
        }
        public bool Registrar(Tipos oTipo)
        {
            bool respuesta = false;
            using (SqlConnection oCnn = new SqlConnection(Conexion.Bd))
            {
                try 
                { 
                    SqlCommand cmd = new SqlCommand("sp_insertaTipos", oCnn);
                    cmd.Parameters.AddWithValue("@Descripcion", oTipo.Descripcion);

                    if (!string.IsNullOrEmpty(oTipo.ImagenBase64))
                    {
                        byte[] imagenBytes = Convert.FromBase64String(oTipo.ImagenBase64);
                        cmd.Parameters.AddWithValue("@Imagen", imagenBytes);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Imagen", DBNull.Value);
                    }

                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
        public bool Modificar(Tipos oTipo)
        {
            bool respuesta = false;
            using (SqlConnection oCnn = new SqlConnection(Conexion.Bd))
            {
                try
                {
                    oCnn.Open();

                    SqlCommand cmd = new SqlCommand("sp_ModificaTipos", oCnn);
                    cmd.Parameters.AddWithValue("@IdTipo", oTipo.IdTipo);
                    cmd.Parameters.AddWithValue("@Descripcion", oTipo.Descripcion);
                    cmd.Parameters.AddWithValue("@Estatus", oTipo.Estatus);

                    // Corrección principal: verificar si hay imagen nueva
                    if (!string.IsNullOrEmpty(oTipo.ImagenBase64) && !string.IsNullOrWhiteSpace(oTipo.ImagenBase64))
                    {
                        try
                        {
                            byte[] imagenBytes = Convert.FromBase64String(oTipo.ImagenBase64);
                            cmd.Parameters.AddWithValue("@Imagen", imagenBytes);
                        }
                        catch (FormatException)
                        {
                            // Si hay error en la conversión, no actualizar imagen
                            cmd.Parameters.AddWithValue("@Imagen", DBNull.Value);
                        }
                    }
                    else
                    {
                        // No hay imagen nueva, pasar NULL para que el SP no actualice la imagen
                        cmd.Parameters.AddWithValue("@Imagen", DBNull.Value);
                    }

                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error en Modificar: {ex.Message}");
                    respuesta = false;
                }
            }
            return respuesta;
        }
        public bool Eliminar(int Id)
        {
            bool respuesta = false;
            using (SqlConnection oCnn = new SqlConnection(Conexion.Bd))
            {
                try
                {
                    oCnn.Open();
                    string sBorrar = "UPDATE TIPOS SET Estatus = 0 WHERE IdTipo = @A1";  // Cambié 'False' por 0

                    SqlCommand cmd = new SqlCommand(sBorrar, oCnn);
                    cmd.Parameters.AddWithValue("@A1", Id);
                    cmd.CommandType = CommandType.Text;

                    int rowsAffected = cmd.ExecuteNonQuery();
                    respuesta = rowsAffected > 0;  // Cambié la lógica aquí
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error en Eliminar: {ex.Message}");
                    respuesta = false;
                }
            }
            return respuesta;
        }
    }
}