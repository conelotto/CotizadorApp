Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports log4net

Public Class daQuery

    Private uData As New Utiles.Datos
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(daTarifa))

    Public Sub MantenimientoQuerys(ByVal oConexion As String, _
                            ByVal oQuery As beQuery, _
                            ByRef oValidacion As beValidacion)

        If String.IsNullOrEmpty(oConexion) OrElse oQuery Is Nothing Then
            Return
        End If

        Try
            Using cnnSql As New SqlConnection(oConexion)
                cnnSql.Open()
                Dim cmdSql As SqlCommand = cnnSql.CreateCommand
                cmdSql.CommandText = objBBDD.StoreProcedure.uspQueryMantenimiento
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.Parameters.Add(uData.CreaParametro("@TIPO", oValidacion.flag, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@CODIGO", oQuery.codigo, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@DESCRIPCION", oQuery.descripcion, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@QUERY", oQuery.query, SqlDbType.VarChar, -1))
                cmdSql.Parameters.Add(uData.CreaParametro("@CADENACAMPOS", oQuery.cadenaCampos, SqlDbType.VarChar, -1))
                cmdSql.Parameters.Add(uData.CreaParametro("@CADENACONDICIONES", oQuery.cadenaCondiciones, SqlDbType.VarChar, -1))
                cmdSql.Parameters.Add(uData.CreaParametro("@USUARIO", oValidacion.usuario, SqlDbType.VarChar, 50))
                cmdSql.Parameters.Add(uData.CreaParametro("@RETURN", String.Empty, SqlDbType.Int, , ParameterDirection.ReturnValue))

                cmdSql.ExecuteNonQuery()

                oValidacion.respuesta = CType(cmdSql.Parameters("@Return").Value, Integer)
                If oValidacion.respuesta = 0 Then oValidacion.validacion = True

            End Using

        Catch ex As Exception
            oValidacion.mensaje = ex.Message
            log.Error(oValidacion.cadenaAleatoria + ": " + oValidacion.mensaje)
        End Try

    End Sub

    Public Sub listarComboQuery(ByVal eConexion As String, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beQuery))

        If String.IsNullOrEmpty(eConexion) OrElse eConexion Is Nothing Then
            Return
        End If

        Try
            Using conexion As New SqlConnection(eConexion)
                conexion.Open()
                Dim comand As New SqlCommand
                comand = conexion.CreateCommand
                comand.CommandText = "uspQueryListar"
                comand.CommandType = CommandType.StoredProcedure
                Using dr As IDataReader = comand.ExecuteReader
                    Dim _Codigo As Integer = dr.GetOrdinal("id")
                    Dim _Descripcion As Integer = dr.GetOrdinal("Descripcion")

                    While dr.Read
                        Dim Query As New beQuery
                        If Not dr.IsDBNull(_Codigo) Then Query.codigo = dr.GetValue(_Codigo)
                        If Not dr.IsDBNull(_Descripcion) Then Query.descripcion = dr.GetValue(_Descripcion)
                        lResult.Add(Query)
                    End While

                End Using
            End Using
            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString

        End Try

    End Sub
End Class
