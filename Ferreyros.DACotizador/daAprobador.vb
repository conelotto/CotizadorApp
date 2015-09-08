Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros.BECotizador
Imports log4net

Public Class daAprobador

    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(daAprobador))

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function Anular(ByVal cnnSql As SqlConnection, ByVal eAprobador As beAprobador, _
    Optional ByVal trnSql As SqlTransaction = Nothing) As Boolean
        blnValido = False
        strError = String.Empty
        Try
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand
            If trnSql IsNot Nothing Then
                cmdSql.Transaction = trnSql
            End If
            cmdSql.CommandText = objBBDD.StoreProcedure.AprobadorAnular
            cmdSql.CommandType = CommandType.StoredProcedure
            With eAprobador
                cmdSql.Parameters.Add(uData.CreaParametro("@IdAprobador", .IdAprobador, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@UsuarioModificacion", .UsuarioModificacion, SqlDbType.VarChar, 10))
                cmdSql.ExecuteNonQuery()
            End With
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Sub ListarAprobador(ByVal eConexion As String, ByVal eAprobador As beAprobador, ByRef eValidacion As beValidacion, ByRef eDetalle As List(Of beAprobador))

        If String.IsNullOrEmpty(eConexion) OrElse eAprobador Is Nothing Then
            Return
        End If

        Try
            Using cnnSql As New SqlConnection(eConexion)
                cnnSql.Open()
                Dim cmdSql As SqlCommand = cnnSql.CreateCommand
                cmdSql.CommandText = objBBDD.StoreProcedure.AprobadorBuscar
                cmdSql.CommandType = CommandType.StoredProcedure
                With eAprobador
                    cmdSql.Parameters.Add(uData.CreaParametro("@IdCorporacion", .IdCorporacion, SqlDbType.VarChar, 15))
                    cmdSql.Parameters.Add(uData.CreaParametro("@IdCompañia", .IdCompañia, SqlDbType.VarChar, 15))
                    cmdSql.Parameters.Add(uData.CreaParametro("@Aprobador", .Aprobador, SqlDbType.VarChar, 50))
                    cmdSql.Parameters.Add(uData.CreaParametro("@Estado", .Estado, SqlDbType.VarChar, 1))
                End With
                Using dr As IDataReader = cmdSql.ExecuteReader
                    Dim _IdAprobador As Integer = dr.GetOrdinal("IdAprobador")
                    Dim _IdCorporacion As Integer = dr.GetOrdinal("IdCorporacion")
                    Dim _IdCompañia As Integer = dr.GetOrdinal("IdCompañia")
                    Dim _Aprobador As Integer = dr.GetOrdinal("Aprobador")
                    Dim _Estado As Integer = dr.GetOrdinal("Estado")
                    Dim Rpt As beAprobador = Nothing
                    While dr.Read
                        Rpt = New beAprobador
                        If Not dr.IsDBNull(_IdAprobador) Then Rpt.IdAprobador = dr.GetValue(_IdAprobador)
                        If Not dr.IsDBNull(_IdCorporacion) Then Rpt.IdCorporacion = dr.GetValue(_IdCorporacion)
                        If Not dr.IsDBNull(_IdCompañia) Then Rpt.IdCompañia = dr.GetValue(_IdCompañia)
                        If Not dr.IsDBNull(_Aprobador) Then Rpt.Aprobador = dr.GetValue(_Aprobador)
                        If Not dr.IsDBNull(_Estado) Then Rpt.Estado = dr.GetValue(_Estado)
                        eDetalle.Add(Rpt)
                    End While
                    dr.Close()
                End Using
            End Using
            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.mensaje = ex.Message
            log.Error(eValidacion.cadenaAleatoria + ": " + eValidacion.mensaje)
        End Try

    End Sub

    Public Sub MantenimientoAprobador(ByVal eConexion As String, ByRef eAprobador As beAprobador, ByRef eDetalle As List(Of beAprobadorUsuario), ByRef eValidacion As beValidacion)

        If String.IsNullOrEmpty(eConexion) Then
            Return
        End If

        Dim transaction As SqlTransaction = Nothing
        Dim conexion As SqlConnection = Nothing

        Try
            conexion = New SqlConnection(eConexion)
            conexion.Open()
            transaction = conexion.BeginTransaction
            Dim cmdSql As SqlCommand = conexion.CreateCommand
            cmdSql.CommandText = objBBDD.StoreProcedure.AprobadorMantenimiento
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.Parameters.Add(uData.CreaParametro("@Tipo", eValidacion.flag, SqlDbType.Int, 10))
            cmdSql.Parameters.Add(uData.CreaParametro("@IdCorporacion", eAprobador.IdCorporacion, SqlDbType.VarChar, 15))
            cmdSql.Parameters.Add(uData.CreaParametro("@IdCompañia", eAprobador.IdCompañia, SqlDbType.VarChar, 15))
            cmdSql.Parameters.Add(uData.CreaParametro("@IdAprobador", eAprobador.IdAprobador, SqlDbType.Int, 15, ParameterDirection.InputOutput))
            cmdSql.Parameters.Add(uData.CreaParametro("@Aprobador", eAprobador.Aprobador, SqlDbType.VarChar, 50))
            cmdSql.Parameters.Add(uData.CreaParametro("@Usuario", eValidacion.usuario, SqlDbType.VarChar, 15))
            cmdSql.Parameters.Add(uData.CreaParametro("@Respuesta", Nothing, SqlDbType.Int, 15, ParameterDirection.Output))
            cmdSql.Parameters.Add(uData.CreaParametro("@Mensaje", Nothing, SqlDbType.VarChar, 100, ParameterDirection.Output))
            cmdSql.Transaction = transaction
            cmdSql.ExecuteNonQuery()
            eValidacion.respuesta = cmdSql.Parameters("@Respuesta").Value
            eValidacion.mensaje = cmdSql.Parameters("@Mensaje").Value

            If eValidacion.respuesta <> 0 Then
                Throw New Exception(eValidacion.mensaje)
            End If
            If eValidacion.flag = 1 Then
                eAprobador.IdAprobador = cmdSql.Parameters("@IdAprobador").Value
            End If
            For Each Rpt In eDetalle
                Dim comand As SqlCommand = conexion.CreateCommand
                comand.CommandText = objBBDD.StoreProcedure.AprobadorUsuarioInsertar
                comand.CommandType = CommandType.StoredProcedure
                comand.Parameters.Add(uData.CreaParametro("@IdAprobador", eAprobador.IdAprobador, SqlDbType.VarChar, 10))
                comand.Parameters.Add(uData.CreaParametro("@MatriculaUsuario", Rpt.MatriculaUsuario, SqlDbType.VarChar, 4))
                comand.Parameters.Add(uData.CreaParametro("@NombreUsuario", Rpt.NombreUsuario, SqlDbType.VarChar, 130))
                comand.Parameters.Add(uData.CreaParametro("@CorreoUsuario", Rpt.CorreoUsuario, SqlDbType.VarChar, 100))
                comand.Parameters.Add(uData.CreaParametro("@UsuarioCreacion", eValidacion.usuario, SqlDbType.VarChar, 15))
                comand.Transaction = transaction
                comand.ExecuteNonQuery()
            Next

            transaction.Commit()
            eValidacion.validacion = True
        Catch ex As Exception
            transaction.Rollback()
            eValidacion.mensaje = ex.Message.ToString
            log.Error(eValidacion.cadenaAleatoria + ": " + eValidacion.mensaje)
        Finally
            If conexion.State = ConnectionState.Open Then conexion.Close()
        End Try

    End Sub

End Class
