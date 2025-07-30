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
    public class MarcasMetodos
    {
        private static MarcasMetodos _instancia = null;

        public MarcasMetodos()
        {

        }

        public static MarcasMetodos Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new MarcasMetodos();
                }
                return _instancia;
            }
        }
        public List<Marcas> Listar()
        {
            List<Marcas> oMarcas = new List<Marcas>();

            using (SqlConnection oCnn = new SqlConnection(Conexion.Bd))
            {
                oCnn.Open();
                SqlCommand cmd = new SqlCommand("sp_consultaMarcas", oCnn);
                cmd.CommandType = CommandType.StoredProcedure;

                

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    oMarcas.Add(new Marcas()
                    {
                        IdMarca = Convert.ToInt32(dr["IdMarca"].ToString()),
                        Descripcion = dr["Descripcion"].ToString(),
                        Imagen = dr["Imagen"] != DBNull.Value ? (byte[])dr["Imagen"] : null,
                        Estatus = Convert.ToBoolean(dr["Estatus"].ToString()),
                        ImagenBase64 = dr["Imagen"] != DBNull.Value ? Convert.ToBase64String((byte[])dr["Imagen"]) : null
                    });
                }
                dr.Close();

                return oMarcas;
            }
        }
        public bool Registrar(Marcas oMarcas)
        {
            bool respuesta = false;
            using (SqlConnection oCnn = new SqlConnection(Conexion.Bd))
            {
                try
                {
                    oCnn.Open();
                    string sInsertar = "INSERT INTO MARCAS (Descripcion, Imagen, Estatus) VALUES (@Descripcion, @Imagen, @Estatus)";
                    SqlCommand cmd = new SqlCommand(sInsertar, oCnn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@Descripcion", oMarcas.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Estatus", oMarcas.Estatus);

                    if (!string.IsNullOrEmpty(oMarcas.ImagenBase64))
                    {
                        try
                        {
                            byte[] imagenBytes = Convert.FromBase64String(oMarcas.ImagenBase64);
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
                    respuesta = rowsAffected > 0;
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
        public bool Modificar(Marcas oMarcas)
        {
            bool respuesta = false;
            using (SqlConnection oCnn = new SqlConnection(Conexion.Bd))
            {
                try
                {
                    oCnn.Open();

                    SqlCommand cmd = new SqlCommand("sp_ModificaMarcas", oCnn);
                    cmd.Parameters.AddWithValue("@IdMarca", oMarcas.IdMarca);
                    cmd.Parameters.AddWithValue("@Descripcion", oMarcas.Descripcion);

                    if (!string.IsNullOrEmpty(oMarcas.ImagenBase64))
                    {
                        byte[] imagenBytes = Convert.FromBase64String(oMarcas.ImagenBase64);
                        cmd.Parameters.AddWithValue("@Imagen", imagenBytes);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Imagen", DBNull.Value);
                    }

                    cmd.Parameters.AddWithValue("@Estatus", oMarcas.Estatus);

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
                    string sBorrar = "UPDATE MARCAS SET Estatus = 0 WHERE IdMarca = @IdMarca";
                    SqlCommand cmd = new SqlCommand(sBorrar, oCnn);
                    cmd.Parameters.AddWithValue("@IdMarca", Id);
                    cmd.CommandType = CommandType.Text;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    respuesta = rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    respuesta = false;
                    throw new Exception("Error in Eliminar: " + ex.Message, ex);
                }
            }
            return respuesta;
        }
    }
}