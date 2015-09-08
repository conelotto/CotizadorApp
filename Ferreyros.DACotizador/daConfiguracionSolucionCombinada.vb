Imports System.Data.SqlClient
Imports Ferreyros.BECotizador

Public Class daConfiguracionSolucionCombinada

    Private uData As New Utiles.Datos

    Public Sub MantenimientoConfiguracionSolComb(ByVal oConexion As String, _
                           ByVal eConfiguracionSolucionCombinada As beConfiguracionSolucionCombinada, _
                           ByRef eValidacion As beValidacion)

        If String.IsNullOrEmpty(oConexion) OrElse eConfiguracionSolucionCombinada Is Nothing Then
            Return
        End If

        Try
            Using cnnSql As New SqlConnection(oConexion)
                cnnSql.Open()
                Dim cmdSql As SqlCommand = cnnSql.CreateCommand
                cmdSql.CommandText = "uspConfiguracionSolCombMant"
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.Parameters.Add(uData.CreaParametro("@TIPO", eValidacion.flag, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@IDCONFIGSC", eConfiguracionSolucionCombinada.IdConfiguracionSC, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@IDLINEA", eConfiguracionSolucionCombinada.CodLinea, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@IDLLAVE", eConfiguracionSolucionCombinada.CodLLave, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@DEPENDENCIA", eConfiguracionSolucionCombinada.LLaveDependencia, SqlDbType.Int))
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
