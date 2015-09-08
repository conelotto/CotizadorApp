Imports System.Data.SqlClient
Imports Ferreyros.BECotizador

Public Class daDocumentoMarcadores

    Private uData As New Utiles.Datos

    Public Sub DocumentoMarcadoresListar(ByVal eConexion As String, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beDocumentoMarcadores))

        Try
            Using conexion As New SqlConnection(eConexion)
                conexion.Open()
                Dim comand As New SqlCommand
                comand = conexion.CreateCommand
                comand.CommandText = "uspDocumentoMarcadoresListar"
                comand.CommandType = CommandType.StoredProcedure
                Using dr As IDataReader = comand.ExecuteReader
                    Dim _IdMarcador As Integer = dr.GetOrdinal("IdMarcador")
                    Dim _Descripcion As Integer = dr.GetOrdinal("Descripcion")
                    Dim _IdCampo As Integer = dr.GetOrdinal("IdCampo")
                    Dim _Query As Integer = dr.GetOrdinal("Query")
                    Dim _IdTipo As Integer = dr.GetOrdinal("IdTipo")
                    Dim _Tipo As Integer = dr.GetOrdinal("Tipo")

                    While dr.Read
                        Dim DocumentoMarcadores As New beDocumentoMarcadores
                        If Not dr.IsDBNull(_IdMarcador) Then DocumentoMarcadores.Codigo = dr.GetValue(_IdMarcador)
                        If Not dr.IsDBNull(_Descripcion) Then DocumentoMarcadores.Descripcion = dr.GetValue(_Descripcion)
                        If Not dr.IsDBNull(_IdCampo) Then DocumentoMarcadores.IdCampo = dr.GetValue(_IdCampo)
                        If Not dr.IsDBNull(_Query) Then DocumentoMarcadores.DescripcionCampo = dr.GetValue(_Query)
                        If Not dr.IsDBNull(_IdTipo) Then DocumentoMarcadores.IdTipo = dr.GetValue(_IdTipo)
                        If Not dr.IsDBNull(_Tipo) Then DocumentoMarcadores.DescripcionTipo = dr.GetValue(_Tipo)

                        lResult.Add(DocumentoMarcadores)
                    End While
                End Using
            End Using
            
            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub

    Public Sub MantenimientoDocumentoMarcadores(ByVal oConexion As String, _
                           ByVal eDocumentoMarcadores As beDocumentoMarcadores, _
                           ByRef eValidacion As beValidacion)

        If String.IsNullOrEmpty(oConexion) OrElse eDocumentoMarcadores Is Nothing Then
            Return
        End If

        Try
            Using cnnSql As New SqlConnection(oConexion)
                cnnSql.Open()
                Dim cmdSql As SqlCommand = cnnSql.CreateCommand
                cmdSql.CommandText = "uspDocumentoMarcadoresMant"
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.Parameters.Add(uData.CreaParametro("@TIPO", eValidacion.flag, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@CODIGO", eDocumentoMarcadores.codigo, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@DESCRIPCION", eDocumentoMarcadores.Descripcion, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@IDCAMPO", eDocumentoMarcadores.IdCampo, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@IDTIPO", eDocumentoMarcadores.IdTipo, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@USUARIO", eValidacion.usuario, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@RETURN", String.Empty, SqlDbType.Int, , ParameterDirection.ReturnValue))

                cmdSql.ExecuteNonQuery()

                eValidacion.respuesta = CType(cmdSql.Parameters("@Return").Value, Integer)
                If eValidacion.respuesta = 0 Then eValidacion.validacion = True

            End Using

        Catch ex As Exception
            eValidacion.mensaje = ex.Message
        End Try

    End Sub
End Class
