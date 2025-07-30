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
    public class CategoriasMetodos
    {
        private static CategoriasMetodos _instancia = null;

        public CategoriasMetodos()
        {

        }

        public static CategoriasMetodos Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CategoriasMetodos();
                }
                return _instancia;
            }
        }
        public List<Categorias> Listar()
        {
            List<Categorias> oCategorias = new List<Categorias>();

            using (SqlConnection oCnn = new SqlConnection(Conexion.Bd))
            {
                oCnn.Open();
                SqlCommand cmd = new SqlCommand("sp_consultaCategorias", oCnn);
                cmd.CommandType = CommandType.StoredProcedure;



                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    oCategorias.Add(new Categorias()
                    {
                        IdCategoria = Convert.ToInt32(dr["IdCategoria"].ToString()),
                        Descripcion = dr["Descripcion"].ToString(),
                        Estatus = Convert.ToBoolean(dr["Estatus"].ToString()),
                        Imagen = dr["Imagen"] != DBNull.Value ? (byte[])dr["Imagen"] : null,
                        ImagenBase64 = dr["Imagen"] != DBNull.Value ? Convert.ToBase64String((byte[])dr["Imagen"]) : null
                    });
                }
                dr.Close();

                return oCategorias;
            }
        }
        public bool Registrar(Categorias oCategoria)
        {
            bool respuesta = false;
            using (SqlConnection oCnn = new SqlConnection(Conexion.Bd))
            {
                try
                {
                    oCnn.Open();
                    string sInsertar = "INSERT INTO CATEGORIAS (Descripcion, Imagen, Estatus) VALUES (@Descripcion, @Imagen, @Estatus)";
                    SqlCommand cmd = new SqlCommand(sInsertar, oCnn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@Descripcion", oCategoria.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Estatus", oCategoria.Estatus);

                    if (!string.IsNullOrEmpty(oCategoria.ImagenBase64))
                    {
                        try
                        {
                            byte[] imagenBytes = Convert.FromBase64String(oCategoria.ImagenBase64);
                            cmd.Parameters.AddWithValue("@Imagen", imagenBytes);
                        }
                        catch (FormatException ex)
                        {
                            throw new Exception("La imagen enviada no es un base64 válido.", ex);
                        }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Imagen", DBNull.Value);
                    }

                    int rowsAffected = cmd.ExecuteNonQuery();
                    respuesta = rowsAffected > 0; ;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("SQL Exception: " + ex.Message);
                    if (ex.InnerException != null)
                        System.Diagnostics.Debug.WriteLine("Inner: " + ex.InnerException.Message);
                    respuesta = false;
                    throw new Exception("Error in Registrar: " + ex.Message, ex);
                }
            }
            return respuesta;
        }
        public bool Modificar(Categorias oCategoria)
        {
            bool respuesta = false;
            using (SqlConnection oCnn = new SqlConnection(Conexion.Bd))
            {
                try
                {
                    oCnn.Open();

                    SqlCommand cmd = new SqlCommand("sp_ModificaCategorias", oCnn);
                    cmd.Parameters.AddWithValue("@IdCategoria", oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("@Descripcion", oCategoria.Descripcion);

                    if (!string.IsNullOrEmpty(oCategoria.ImagenBase64))
                    {
                        byte[] imagenBytes = Convert.FromBase64String(oCategoria.ImagenBase64);
                        cmd.Parameters.AddWithValue("@Imagen", imagenBytes);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Imagen", DBNull.Value);
                    }

                    cmd.Parameters.AddWithValue("@Estatus", oCategoria.Estatus);

                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                }
                catch (Exception ex)
                {
                    respuesta = false;
                    throw new Exception("Error in Modificar: " + ex.Message, ex);
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
                    string sBorrar = "UPDATE CATEGORIAS SET Estatus = 'False' FROM CATEGORIAS WHERE IdCategoria = @A1";

                    SqlCommand cmd = new SqlCommand(sBorrar, oCnn);
                    cmd.Parameters.AddWithValue("@A1", Id);
                    cmd.CommandType = CommandType.Text;

                    int rowsAffected = cmd.ExecuteNonQuery();
                    respuesta = rowsAffected > 0;  // Cambié la lógica aquí
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
    }
}
