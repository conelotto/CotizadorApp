Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports log4net

Public Class daLineaResponsable
    Private uData As New Utiles.Datos
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(daTarifa))

    Public Sub LineaResponsableMant(ByVal oConexion As String, _
                            ByVal oLineaResponsable As beLineaResponsable, _
                            ByRef oValidacion As beValidacion)

        If String.IsNullOrEmpty(oConexion) OrElse oLineaResponsable Is Nothing Then
            Return
        End If

        Try
            Using cnnSql As New SqlConnection(oConexion)
                cnnSql.Open()
                Dim cmdSql As SqlCommand = cnnSql.CreateCommand
                cmdSql.CommandText = "uspLineaResponsableMant"
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.Parameters.Add(uData.CreaParametro("@TIPO", oValidacion.flag, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@CODIGO", oLineaResponsable.Codigo, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@USUARIO", oLineaResponsable.Usuario, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@LINEA", oLineaResponsable.Linea, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@RETURN", String.Empty, SqlDbType.Int, , ParameterDirection.ReturnValue))

                cmdSql.ExecuteNonQuery()

                oValidacion.respuesta = CType(cmdSql.Parameters("@Return").Value, Integer)

                If oValidacion.respuesta = 0 Then
                    oValidacion.validacion = False
                Else
                    oValidacion.validacion = True
                End If

            End Using

        Catch ex As Exception
            oValidacion.mensaje = ex.Message
            log.Error(oValidacion.cadenaAleatoria + ": " + oValidacion.mensaje)
        End Try

    End Sub
    Public Sub listarCombo(ByVal eConexion As String, ByVal eLineaResponsable As beLineaResponsable, _
                           ByRef eValidacion As beValidacion, ByRef lResult As List(Of beLineaResponsable))

        If String.IsNullOrEmpty(eConexion) OrElse eConexion Is Nothing Then
            Return
        End If

        Try
            Using conexion As New SqlConnection(eConexion)
                conexion.Open()
                Dim cmdSql As SqlCommand = conexion.CreateCommand
                cmdSql.CommandText = "uspLineaResponsableMant"
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.Parameters.Add(uData.CreaParametro("@TIPO", eValidacion.flag, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@CODIGO", eLineaResponsable.Codigo, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@USUARIO", eLineaResponsable.Usuario, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@LINEA", eLineaResponsable.Linea, SqlDbType.Int))
                Using dr As IDataReader = cmdSql.ExecuteReader
                    Dim _Codigo As Integer = dr.GetOrdinal("id")
                    Dim _Descripcion As Integer = dr.GetOrdinal("Descripcion")

                    While dr.Read
                        Dim LineaResponsable As New beLineaResponsable
                        If Not dr.IsDBNull(_Codigo) Then LineaResponsable.Linea = dr.GetValue(_Codigo)
                        If Not dr.IsDBNull(_Descripcion) Then LineaResponsable.DescripcionLinea = dr.GetValue(_Descripcion)
                        lResult.Add(LineaResponsable)
                    End While

                End Using
            End Using
            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString

        End Try

    End Sub

    Public Sub LineaResponsableListar(ByVal eConexion As String, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beLineaResponsable))

        Try
            Using conexion As New SqlConnection(eConexion)
                conexion.Open()
                Dim command As New SqlCommand
                command = conexion.CreateCommand
                command.CommandText = "uspLineaResponsableListar"
                command.CommandType = CommandType.StoredProcedure
                Using dr As IDataReader = command.ExecuteReader
                    Dim _Codigo As Integer = dr.GetOrdinal("IdResponsableLinea")
                    Dim _Usuario As Integer = dr.GetOrdinal("Usuario")
                    Dim _IdLinea As Integer = dr.GetOrdinal("IdLinea")
                    Dim _CodigoLinea As Integer = dr.GetOrdinal("CodigoLinea")
                    Dim _DescripcionLinea As Integer = dr.GetOrdinal("DescripcionLinea")

                    While dr.Read
                        Dim ResponsableLinea As New beLineaResponsable
                        If Not dr.IsDBNull(_Codigo) Then ResponsableLinea.Codigo = dr.GetValue(_Codigo)
                        If Not dr.IsDBNull(_Usuario) Then ResponsableLinea.Usuario = dr.GetValue(_Usuario)
                        If Not dr.IsDBNull(_IdLinea) Then ResponsableLinea.Linea = dr.GetValue(_IdLinea)
                        If Not dr.IsDBNull(_CodigoLinea) Then ResponsableLinea.CodigoLinea = dr.GetValue(_CodigoLinea)
                        If Not dr.IsDBNull(_DescripcionLinea) Then ResponsableLinea.DescripcionLinea = dr.GetValue(_DescripcionLinea)

                        lResult.Add(ResponsableLinea)
                    End While
                End Using
            End Using

            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub

End Class
