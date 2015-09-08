Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros.BECotizador

Public Class daTablaMaestra

    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty
    Private IdSeccion As String = String.Empty
    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function BuscarId(ByVal cnnSql As SqlConnection, ByVal eTablaMaestra As beTablaMaestra, _
                           ByRef dtrTablaMaestra As DataRow, Optional ByVal trnSql As SqlTransaction = Nothing) As Boolean
        blnValido = False
        strError = String.Empty
        Try
            Dim dstTablaMaestra As New DataSet
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand
            If trnSql IsNot Nothing Then
                cmdSql.Transaction = trnSql
            End If
            cmdSql.CommandText = "uspTablaMaestraBuscarId"
            cmdSql.CommandType = CommandType.StoredProcedure
            With eTablaMaestra
                cmdSql.Parameters.Add(uData.CreaParametro("@IdTablaMaestra", .IdTablaMaestra, SqlDbType.Int))
                Dim adpSql As New SqlDataAdapter(cmdSql)
                adpSql.Fill(dstTablaMaestra, "TablaMaestra")
                If Not dstTablaMaestra.Tables(0).Rows.Count.Equals(0) Then
                    dtrTablaMaestra = dstTablaMaestra.Tables(0).Rows(0)
                End If
            End With
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function BuscarGrupo(ByVal cnnSql As SqlConnection, ByVal eTablaMaestra As beTablaMaestra, _
                           ByRef dstTablaMaestra As DataSet, Optional ByVal trnSql As SqlTransaction = Nothing) As Boolean
        blnValido = False
        strError = String.Empty
        Try
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand
            If trnSql IsNot Nothing Then
                cmdSql.Transaction = trnSql
            End If
            cmdSql.CommandText = objBBDD.StoreProcedure.TablaMaestraBuscarGrupo
            cmdSql.CommandType = CommandType.StoredProcedure
            With eTablaMaestra
                cmdSql.Parameters.Add(uData.CreaParametro("@Grupo", .Grupo, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@Estado", .Estado, SqlDbType.VarChar, 1))
                Dim adpSql As New SqlDataAdapter(cmdSql)
                If dstTablaMaestra Is Nothing Then
                    dstTablaMaestra = New DataSet
                End If
                adpSql.Fill(dstTablaMaestra, eTablaMaestra.Grupo)
            End With
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Sub ListarSeccionesCotizaciones(ByVal eConexion As String, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beTablaMaestra))

        Dim conexion As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing

        Try
            conexion = New SqlConnection(eConexion)
            conexion.Open()
            cmd = conexion.CreateCommand
            cmd.CommandText = objBBDD.StoreProcedure.SeccionesCotizacionListar
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add(uData.CreaParametro("@Tipo", eValidacion.flag, SqlDbType.Int))
            If eValidacion.flag = "1" Then
                Using Reader As IDataReader = cmd.ExecuteReader
                    Dim _IdSeccion As Integer = Reader.GetOrdinal("IdSeccion")
                    Dim _Nombre As Integer = Reader.GetOrdinal("Nombre")
                    Dim _PosicionInicial As Integer = Reader.GetOrdinal("PosicionInicial")
                    Dim _Opcional As Integer = Reader.GetOrdinal("Opcional")
                    Dim _CambioPosicion As Integer = Reader.GetOrdinal("CambioPosicion")
                    Dim TablaMaestra As beTablaMaestra = Nothing
                    While Reader.Read
                        TablaMaestra = New beTablaMaestra
                        If Not Reader.IsDBNull(_IdSeccion) Then TablaMaestra.IdSeccion = Reader.GetValue(_IdSeccion)
                        If Not Reader.IsDBNull(_Nombre) Then TablaMaestra.Nombre = Reader.GetValue(_Nombre)
                        If Not Reader.IsDBNull(_PosicionInicial) Then TablaMaestra.PosicionInicial = Reader.GetValue(_PosicionInicial)
                        If Not Reader.IsDBNull(_Opcional) Then TablaMaestra.Opcional = Reader.GetValue(_Opcional)
                        If Not Reader.IsDBNull(_CambioPosicion) Then TablaMaestra.CambioPosicion = Reader.GetValue(_CambioPosicion)
                        lResult.Add(TablaMaestra)
                    End While
                End Using
            End If
            If eValidacion.flag = "2" Then
                Using Reader As IDataReader = cmd.ExecuteReader
                    Dim _IdCriterio As Integer = Reader.GetOrdinal("IdCriterio")
                    Dim _NombreCriterio As Integer = Reader.GetOrdinal("NombreCriterio")
                    Dim TablaMaestra As beTablaMaestra = Nothing
                    While Reader.Read
                        TablaMaestra = New beTablaMaestra
                        If Not Reader.IsDBNull(_IdCriterio) Then TablaMaestra.IdSeccion = Reader.GetValue(_IdCriterio)
                        If Not Reader.IsDBNull(_NombreCriterio) Then TablaMaestra.Nombre = Reader.GetValue(_NombreCriterio)
                        lResult.Add(TablaMaestra)
                    End While
                End Using
            End If
            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        Finally
            If conexion.State = ConnectionState.Open Then conexion.Close()
        End Try

    End Sub

    Public Sub MantenimientoSeccion(ByVal eConexion As String, ByVal eTablaMaestra As beTablaMaestra, ByVal l_SeccionCriterio As List(Of beTablaMaestra), ByRef eValidacion As beValidacion)

        Dim conexion As SqlConnection = Nothing
        Dim cmdSeccion As SqlCommand = Nothing
        Dim cmdSeccionCriterio As SqlCommand = Nothing
        Dim transaction As SqlTransaction = Nothing
        Dim IdSeccion As String = String.Empty
        Dim Listado As List(Of beTablaMaestra) = Nothing
        Dim Prioridad As Integer

        Try
            conexion = New SqlConnection(eConexion)
            conexion.Open()
            transaction = conexion.BeginTransaction
            '-----------------------------------------------------------------------------------------'
            cmdSeccion = conexion.CreateCommand
            cmdSeccion.CommandText = objBBDD.StoreProcedure.SeccionMantenimiento
            cmdSeccion.CommandType = CommandType.StoredProcedure
            cmdSeccion.Parameters.Add(uData.CreaParametro("@TipoOperacion", eValidacion.flag, SqlDbType.Int))
            cmdSeccion.Parameters.Add(uData.CreaParametro("@IdSeccion", eTablaMaestra.IdSeccion, SqlDbType.Int, 10, ParameterDirection.InputOutput))
            cmdSeccion.Parameters.Add(uData.CreaParametro("@Nombre", eTablaMaestra.Nombre, SqlDbType.VarChar, 100))
            cmdSeccion.Parameters.Add(uData.CreaParametro("@PosicionInicial", eTablaMaestra.PosicionInicial, SqlDbType.Int))
            cmdSeccion.Parameters.Add(uData.CreaParametro("@Opcional", eTablaMaestra.Opcional, SqlDbType.Bit, 5))
            cmdSeccion.Parameters.Add(uData.CreaParametro("@CambioPosicion", eTablaMaestra.CambioPosicion, SqlDbType.Bit, 5))
            cmdSeccion.Parameters.Add(uData.CreaParametro("@Usuario", eValidacion.usuario, SqlDbType.VarChar, 15))
            cmdSeccion.Transaction = transaction
            cmdSeccion.ExecuteNonQuery()
            IdSeccion = cmdSeccion.Parameters("@IdSeccion").Value
            Prioridad = 0
            For Each Rpt As beTablaMaestra In l_SeccionCriterio
                Prioridad += 1
                cmdSeccionCriterio = conexion.CreateCommand
                cmdSeccionCriterio.CommandText = objBBDD.StoreProcedure.SeccionCriterioInsertar
                cmdSeccionCriterio.CommandType = CommandType.StoredProcedure
                cmdSeccionCriterio.Parameters.Add(uData.CreaParametro("@IdSeccion", IdSeccion, SqlDbType.Int))
                cmdSeccionCriterio.Parameters.Add(uData.CreaParametro("@IdCriterio", Rpt.IdCriterio, SqlDbType.Int))
                cmdSeccionCriterio.Parameters.Add(uData.CreaParametro("@Prioridad", Prioridad, SqlDbType.Int))
                cmdSeccionCriterio.Parameters.Add(uData.CreaParametro("@Usuario", eValidacion.usuario, SqlDbType.VarChar, 15))
                cmdSeccionCriterio.Transaction = transaction
                cmdSeccionCriterio.ExecuteNonQuery()
            Next
            transaction.Commit()
            eValidacion.validacion = True
        Catch ex As Exception
            transaction.Rollback()
            eValidacion.mensaje = ex.Message.ToString
        Finally
            If conexion.State = ConnectionState.Open Then conexion.Close()
        End Try

    End Sub

    Public Sub MantenimientoCriterio(ByVal eConexion As String, ByVal eTablaMaestra As beTablaMaestra, ByRef eValidacion As beValidacion)

        Dim conexion As SqlConnection = Nothing
        Dim cmdCriterio As SqlCommand = Nothing
        Dim transaction As SqlTransaction = Nothing

        Try
            conexion = New SqlConnection(eConexion)
            conexion.Open()
            transaction = conexion.BeginTransaction
            '-----------------------------------------------------------------------------------------'
            cmdCriterio = conexion.CreateCommand
            cmdCriterio.CommandText = objBBDD.StoreProcedure.CriterioMantenimiento
            cmdCriterio.CommandType = CommandType.StoredProcedure
            cmdCriterio.Parameters.Add(uData.CreaParametro("@TipoOperacion", eValidacion.flag, SqlDbType.Int, 10))
            cmdCriterio.Parameters.Add(uData.CreaParametro("@IdCriterio", eTablaMaestra.IdSeccion, SqlDbType.Int, 10))
            cmdCriterio.Parameters.Add(uData.CreaParametro("@NombreCriterio", eTablaMaestra.Nombre, SqlDbType.VarChar, 100))
            cmdCriterio.Transaction = transaction
            cmdCriterio.ExecuteNonQuery()
            transaction.Commit()
            eValidacion.validacion = True
        Catch ex As Exception
            transaction.Rollback()
            eValidacion.mensaje = ex.Message.ToString
        Finally
            If conexion.State = ConnectionState.Open Then conexion.Close()
        End Try

    End Sub
    Public Sub ListarCriteriosPorSeccion(ByVal strConexion As String, ByVal eValidacion As beValidacion, ByRef lResult As List(Of beTablaMaestra))
        Dim conexion As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim Listado As List(Of beTablaMaestra) = Nothing

        Try
            conexion = New SqlConnection(strConexion)
            conexion.Open()
            cmd = conexion.CreateCommand
            cmd.CommandText = objBBDD.StoreProcedure.uspSeccionCriterioBuscarIdSeccion
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add(uData.CreaParametro("@IdSeccion", eValidacion.flag, SqlDbType.Int))
            Using Reader As IDataReader = cmd.ExecuteReader
                Dim _IdSeccionCriterio As String = Reader.GetOrdinal("IdSeccionCriterio")
                Dim _IdSeccion As String = Reader.GetOrdinal("IdSeccion")
                Dim _IdCriterio As String = Reader.GetOrdinal("IdCriterio")
                Dim _Nombre As String = Reader.GetOrdinal("Nombre")
                Dim _Prioridad As String = Reader.GetOrdinal("Prioridad")
                Dim _CodigoSeccion As String = Reader.GetOrdinal("CodigoSeccion")


                Dim TablaMaestra As beTablaMaestra = Nothing
                While Reader.Read
                    TablaMaestra = New beTablaMaestra
                    If Not Reader.IsDBNull(_IdSeccionCriterio) Then TablaMaestra.IdSeccionCriterio = Reader.GetValue(_IdSeccionCriterio)
                    If Not Reader.IsDBNull(_IdSeccion) Then TablaMaestra.IdSeccion = Reader.GetValue(_IdSeccion)
                    If Not Reader.IsDBNull(_IdCriterio) Then TablaMaestra.IdCriterio = Reader.GetValue(_IdCriterio)
                    If Not Reader.IsDBNull(_Nombre) Then TablaMaestra.Nombre = Reader.GetValue(_Nombre)
                    If Not Reader.IsDBNull(_Prioridad) Then TablaMaestra.Prioridad = Reader.GetValue(_Prioridad)
                    If Not Reader.IsDBNull(_CodigoSeccion) Then TablaMaestra.CodigoSeccion = Reader.GetValue(_CodigoSeccion).ToString()
                    lResult.Add(TablaMaestra)
                End While
            End Using
            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        Finally
            If conexion.State = ConnectionState.Open Then conexion.Close()
        End Try
    End Sub

    Public Sub MantenimientoListaSeccion(ByVal eConexion As String, ByVal l_ListaSeccion As List(Of beTablaMaestra), ByRef eValidacion As beValidacion)

        Dim conexion As SqlConnection = Nothing
        Dim cmdLimpiar As SqlCommand = Nothing
        Dim cmdInsertar As SqlCommand = Nothing
        Dim transaction As SqlTransaction = Nothing
   
        Try
            conexion = New SqlConnection(eConexion)
            conexion.Open()
            transaction = conexion.BeginTransaction
            '-----------------------------------------------------------------------------------------'
            cmdLimpiar = conexion.CreateCommand
            cmdLimpiar.CommandText = objBBDD.StoreProcedure.ListaSeccionLimpiar
            cmdLimpiar.CommandType = CommandType.StoredProcedure
            cmdLimpiar.Parameters.Add(uData.CreaParametro("@Usuario", eValidacion.usuario, SqlDbType.VarChar, 15))
            cmdLimpiar.Transaction = transaction
            cmdLimpiar.ExecuteNonQuery()
            For Each Rpt As beTablaMaestra In l_ListaSeccion
                cmdInsertar = conexion.CreateCommand
                cmdInsertar.CommandText = objBBDD.StoreProcedure.ListaSeccionInsertar
                cmdInsertar.CommandType = CommandType.StoredProcedure
                cmdInsertar.Parameters.Add(uData.CreaParametro("@IdSeccion", Rpt.IdSeccion, SqlDbType.Int, 10))
                cmdInsertar.Parameters.Add(uData.CreaParametro("@PosicionInicial", Rpt.PosicionInicial, SqlDbType.Int, 10))
                cmdInsertar.Parameters.Add(uData.CreaParametro("@Imprimir", Rpt.Imprimir, SqlDbType.Char, 2))
                cmdInsertar.Parameters.Add(uData.CreaParametro("@Usuario", eValidacion.usuario, SqlDbType.VarChar, 15))
                cmdInsertar.Transaction = transaction
                cmdInsertar.ExecuteNonQuery()
            Next
            transaction.Commit()
            eValidacion.validacion = True
        Catch ex As Exception
            transaction.Rollback()
            eValidacion.mensaje = ex.Message.ToString
        Finally
            If conexion.State = ConnectionState.Open Then conexion.Close()
        End Try

    End Sub

    Public Sub ListarSeccionesxUsuario(ByVal eConexion As String, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beTablaMaestra))

        Dim conexion As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing

        Try
            conexion = New SqlConnection(eConexion)
            conexion.Open()
            cmd = conexion.CreateCommand
            cmd.CommandText = objBBDD.StoreProcedure.ListaSeccionBusqueda
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add(uData.CreaParametro("@Usuario", eValidacion.usuario, SqlDbType.VarChar, 15))
            Using Reader As IDataReader = cmd.ExecuteReader
                Dim _IdSeccion As Integer = Reader.GetOrdinal("IdSeccion")
                Dim _Nombre As Integer = Reader.GetOrdinal("Nombre")
                Dim _PosicionInicial As Integer = Reader.GetOrdinal("PosicionInicial")
                Dim _Opcional As Integer = Reader.GetOrdinal("Opcional")
                Dim _Imprimir As Integer = Reader.GetOrdinal("Imprimir")
                Dim _CambioPosicion As Integer = Reader.GetOrdinal("CambioPosicion")
                Dim _CodigoSeccion As Integer = Reader.GetOrdinal("CodigoSeccion")
                Dim TablaMaestra As beTablaMaestra = Nothing
                While Reader.Read
                    TablaMaestra = New beTablaMaestra
                    If Not Reader.IsDBNull(_IdSeccion) Then TablaMaestra.IdSeccion = Reader.GetValue(_IdSeccion)
                    If Not Reader.IsDBNull(_Nombre) Then TablaMaestra.Nombre = Reader.GetValue(_Nombre)
                    If Not Reader.IsDBNull(_PosicionInicial) Then TablaMaestra.PosicionInicial = Reader.GetValue(_PosicionInicial)
                    If Not Reader.IsDBNull(_Opcional) Then TablaMaestra.Opcional = Reader.GetValue(_Opcional)
                    If Not Reader.IsDBNull(_Imprimir) Then TablaMaestra.Imprimir = Reader.GetValue(_Imprimir)
                    If Not Reader.IsDBNull(_CambioPosicion) Then TablaMaestra.CambioPosicion = Reader.GetValue(_CambioPosicion)
                    If Not Reader.IsDBNull(_CodigoSeccion) Then TablaMaestra.Codigo = Reader.GetValue(_CodigoSeccion)
                    lResult.Add(TablaMaestra)
                End While
            End Using
            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        Finally
            If conexion.State = ConnectionState.Open Then conexion.Close()
        End Try

    End Sub


    Public Sub MarcadorListar(ByVal eConexion As String, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beMarcador))

        Dim conexion As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing

        Try
            conexion = New SqlConnection(eConexion)
            conexion.Open()
            cmd = conexion.CreateCommand
            cmd.CommandText = objBBDD.StoreProcedure.uspMarcadorListar
            cmd.CommandType = CommandType.StoredProcedure

            Using Reader As IDataReader = cmd.ExecuteReader
                Dim _IdMarcador As Integer = Reader.GetOrdinal("IdMarcador")
                Dim _NombreMarcador As String = Reader.GetOrdinal("NombreMarcador")
                Dim TablaMaestra As beMarcador = Nothing
                While Reader.Read
                    TablaMaestra = New beMarcador
                    If Not Reader.IsDBNull(_IdMarcador) Then TablaMaestra.IdMarcador = Reader.GetValue(_IdMarcador)
                    If Not Reader.IsDBNull(_NombreMarcador) Then TablaMaestra.NombreMarcador = Reader.GetValue(_NombreMarcador)
                    lResult.Add(TablaMaestra)
                End While
            End Using
            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        Finally
            If conexion.State = ConnectionState.Open Then conexion.Close()
        End Try

    End Sub

    Public Function MarcadorCotizacionInsertar(ByVal cnnSql As SqlConnection, ByVal obeMarcadorCotizacion As beMarcadorCotizacion, Optional ByVal trSql As SqlTransaction = Nothing) As Boolean
        blnValido = False
        strError = String.Empty
        Try
            Dim cmd As SqlCommand = cnnSql.CreateCommand()
            If Not IsNothing(trSql) Then
                cmd.Transaction = trSql
            End If
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = objBBDD.StoreProcedure.uspMarcadorCotizacionInsertar
            With obeMarcadorCotizacion
                cmd.Parameters.Add(uData.CreaParametro("@IdMarcadorCotizacion", .IdMarcadorCotizacion, SqlDbType.Int, ParameterDirection.InputOutput))
                cmd.Parameters.Add(uData.CreaParametro("@IdArchivoConfiguracion", .IdArchivoConfiguracion, SqlDbType.Int))
                cmd.Parameters.Add(uData.CreaParametro("@NombreMarcadorCotizacion", .NombreMarcadorCotizacion, SqlDbType.VarChar, 100))
                cmd.Parameters.Add(uData.CreaParametro("@NombreMarcador", .NombreMarcador, SqlDbType.VarChar, 100))

                cmd.ExecuteNonQuery()
                .IdMarcadorCotizacion = cmd.Parameters("@IdMarcadorCotizacion").Value.ToString()
                blnValido = True
            End With

        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function MarcadorCotizacionActualizar(ByVal cnnSql As SqlConnection, ByVal obeMarcadorCotizacion As beMarcadorCotizacion, Optional ByVal trSql As SqlTransaction = Nothing)
        blnValido = True
        strError = String.Empty

        Try
            Dim cmd As SqlCommand = cnnSql.CreateCommand()
            If Not IsNothing(trSql) Then
                cmd.Transaction = trSql
            End If

            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = objBBDD.StoreProcedure.uspMarcadorCotizacionActualizar
            With obeMarcadorCotizacion
                cmd.Parameters.Add(uData.CreaParametro("@IdMarcadorCotizacion", .IdMarcadorCotizacion, SqlDbType.Int))
                cmd.Parameters.Add(uData.CreaParametro("@IdArchivoConfiguracion", .IdArchivoConfiguracion, SqlDbType.Int))
                cmd.Parameters.Add(uData.CreaParametro("@NombreMarcadorCotizacion", .NombreMarcadorCotizacion, SqlDbType.VarChar, 100))
                cmd.Parameters.Add(uData.CreaParametro("@NombreMarcador", .NombreMarcador, SqlDbType.VarChar, 100))

            End With
            cmd.ExecuteNonQuery()
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function
    Public Sub MarcadorBuscarIdArchivoConfig(ByVal eConexion As String, ByRef ebeMarcadorCotizacion As beMarcadorCotizacion, ByRef lResult As List(Of beMarcadorCotizacion))
         
            Using conexion As SqlConnection = New SqlConnection(eConexion)
                conexion.Open()
                Using cmd As SqlCommand = conexion.CreateCommand
                    cmd.CommandText = objBBDD.StoreProcedure.uspMarcadorCotizacionBuscarIdArchivo
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add(uData.CreaParametro("@IdArchivoConfiguracion", ebeMarcadorCotizacion.IdArchivoConfiguracion, SqlDbType.Int))
                    Using Reader As IDataReader = cmd.ExecuteReader
                        Dim _IdMarcadorCotizacion As Integer = Reader.GetOrdinal("IdMarcadorCotizacion")
                        Dim _IdArchivoConfiguracion As Integer = Reader.GetOrdinal("IdArchivoConfiguracion")
                        Dim _NombreMarcadorCotizacion As Integer = Reader.GetOrdinal("NombreMarcadorCotizacion")
                        Dim _NombreMarcador As Integer = Reader.GetOrdinal("NombreMarcador")
                        Dim obeMarcadorCotizacion As beMarcadorCotizacion = Nothing
                        While Reader.Read
                            obeMarcadorCotizacion = New beMarcadorCotizacion
                            If Not Reader.IsDBNull(_IdMarcadorCotizacion) Then obeMarcadorCotizacion.IdMarcadorCotizacion = Reader.GetValue(_IdMarcadorCotizacion)
                            If Not Reader.IsDBNull(_IdArchivoConfiguracion) Then obeMarcadorCotizacion.IdArchivoConfiguracion = Reader.GetValue(_IdArchivoConfiguracion)
                            If Not Reader.IsDBNull(_NombreMarcadorCotizacion) Then obeMarcadorCotizacion.NombreMarcadorCotizacion = Reader.GetValue(_NombreMarcadorCotizacion)
                            If Not Reader.IsDBNull(_NombreMarcador) Then obeMarcadorCotizacion.NombreMarcador = Reader.GetValue(_NombreMarcador)
                            lResult.Add(obeMarcadorCotizacion)
                        End While
                    End Using
                    'eValidacion.validacion = True
                End Using
            End Using 

    End Sub

End Class