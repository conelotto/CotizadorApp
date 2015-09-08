Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros.BECotizador

Public Class daArchivoConfiguracion

    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property
    Public Function Insertar(ByVal cnnSql As SqlConnection, ByVal obeArchivoConfiguracion As beArchivoConfiguracion, Optional ByVal trSql As SqlTransaction = Nothing) As Boolean
        blnValido = False
        strError = String.Empty
        Try
            Using cmd As SqlCommand = cnnSql.CreateCommand()
                If Not IsNothing(trSql) Then
                    cmd.Transaction = trSql
                End If
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = objBBDD.StoreProcedure.uspArchivoConfiguracionInsertar
                With obeArchivoConfiguracion
                    cmd.Parameters.Add(uData.CreaParametro("@IdArchivoConfiguracion", .IdArchivoConfiguracion, SqlDbType.Int, ParameterDirection.InputOutput))
                    cmd.Parameters.Add(uData.CreaParametro("@IdSeccionCriterio", .IdSeccionCriterio, SqlDbType.Int))
                    cmd.Parameters.Add(uData.CreaParametro("@IdSubSeccionCriterio", .IdSubSeccionCriterio, SqlDbType.Int))
                    cmd.Parameters.Add(uData.CreaParametro("@Tipo", .Tipo, SqlDbType.VarChar, 50))
                    cmd.Parameters.Add(uData.CreaParametro("@Codigo", .Codigo, SqlDbType.VarChar, 50))
                    cmd.Parameters.Add(uData.CreaParametro("@Nombre", .Nombre, SqlDbType.VarChar, 100))
                    cmd.Parameters.Add(uData.CreaParametro("@Archivo", .Archivo, SqlDbType.VarChar, 200))
                    cmd.Parameters.Add(uData.CreaParametro("@Valor", .Valor, SqlDbType.VarChar, 500))
                    cmd.Parameters.Add(uData.CreaParametro("@Usuario", .UsuarioCreacion, SqlDbType.VarChar, 100))

                    cmd.ExecuteNonQuery()
                    .IdArchivoConfiguracion = cmd.Parameters("@IdArchivoConfiguracion").Value.ToString()
                    blnValido = True
                End With
            End Using
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function Actualizar(ByVal cnnSql As SqlConnection, ByVal obeArchivoConfiguracion As beArchivoConfiguracion, Optional ByVal trSql As SqlTransaction = Nothing)
        blnValido = True
        strError = String.Empty

        Try
            Using cmd As SqlCommand = cnnSql.CreateCommand()
                If Not IsNothing(trSql) Then
                    cmd.Transaction = trSql
                End If

                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = objBBDD.StoreProcedure.uspArchivoConfiguracionActualizar
                With obeArchivoConfiguracion
                    cmd.Parameters.Add(uData.CreaParametro("@IdArchivoConfiguracion", .IdArchivoConfiguracion, SqlDbType.Int))
                    cmd.Parameters.Add(uData.CreaParametro("@IdSeccionCriterio", .IdSeccionCriterio, SqlDbType.Int))
                    cmd.Parameters.Add(uData.CreaParametro("@IdSubSeccionCriterio", .IdSubSeccionCriterio, SqlDbType.Int))
                    cmd.Parameters.Add(uData.CreaParametro("@Tipo", .Tipo, SqlDbType.VarChar, 50))
                    cmd.Parameters.Add(uData.CreaParametro("@Codigo", .Codigo, SqlDbType.VarChar, 50))
                    cmd.Parameters.Add(uData.CreaParametro("@Nombre", .Nombre, SqlDbType.VarChar, 100))
                    cmd.Parameters.Add(uData.CreaParametro("@Archivo", .Archivo, SqlDbType.VarChar, 200))
                    cmd.Parameters.Add(uData.CreaParametro("@Valor", .Valor, SqlDbType.VarChar, 500))
                    cmd.Parameters.Add(uData.CreaParametro("@Usuario", .UsuarioModificacion, SqlDbType.VarChar, 100))
                End With
                cmd.ExecuteNonQuery()
                blnValido = True
            End Using
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function Eliminar(ByVal cnnSql As SqlConnection, ByVal obeArchivoConfiguracion As beArchivoConfiguracion, ByRef eValidacion As beValidacion) As Boolean
        blnValido = False
        strError = String.Empty
        Try
            Using cmd As SqlCommand = cnnSql.CreateCommand()

                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = objBBDD.StoreProcedure.uspArchivoConfiguracionEliminar
                With obeArchivoConfiguracion
                    cmd.Parameters.Add(uData.CreaParametro("@IdArchivoConfiguracion", .IdArchivoConfiguracion, SqlDbType.Int, ParameterDirection.Input))
                    cmd.Parameters.Add(uData.CreaParametro("@Result", Nothing, SqlDbType.VarChar, 50, ParameterDirection.Output))

                    cmd.ExecuteNonQuery()
                    Dim Resultado As String = cmd.Parameters("@Result").Value.ToString()
                    If Resultado = 1 Then
                        blnValido = True
                        eValidacion.validacion = True
                    End If
                End With
            End Using
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function BuscarPorCriterio(ByVal cnnSql As SqlConnection, ByVal obeArchivoConfiguracion As beArchivoConfiguracion, ByRef lista As List(Of beArchivoConfiguracion)) As Boolean
        Try
            blnValido = False
            Using cmdSql As SqlCommand = cnnSql.CreateCommand()
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.CommandText = objBBDD.StoreProcedure.uspArchivoConfiguracionBuscarPorCriterio

                cmdSql.Parameters.Add(uData.CreaParametro("@Tipo", obeArchivoConfiguracion.Tipo, SqlDbType.VarChar, 50))
                cmdSql.Parameters.Add(uData.CreaParametro("@IdSeccionCriterio", obeArchivoConfiguracion.IdSeccionCriterio, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@IdSubSeccionCriterio", obeArchivoConfiguracion.IdSubSeccionCriterio, SqlDbType.Int))
                Using Reader As IDataReader = cmdSql.ExecuteReader
                    Dim _IdArchivoConfiguracion As String = Reader.GetOrdinal("IdArchivoConfiguracion")
                    Dim _IdSeccionCriterio As String = Reader.GetOrdinal("IdSeccionCriterio")
                    Dim _IdSubSeccionCriterio As String = Reader.GetOrdinal("IdSubSeccionCriterio")
                    Dim _Tipo As String = Reader.GetOrdinal("Tipo")
                    Dim _Codigo As String = Reader.GetOrdinal("Codigo")
                    Dim _Nombre As String = Reader.GetOrdinal("Nombre")
                    Dim _Archivo As String = Reader.GetOrdinal("Archivo")
                    Dim _Valor As String = Reader.GetOrdinal("Valor")


                    While Reader.Read
                        Dim ebeArchivoConfiguracion As New beArchivoConfiguracion
                        With ebeArchivoConfiguracion
                            If Not Reader.IsDBNull(_IdArchivoConfiguracion) Then .IdArchivoConfiguracion = Reader.GetValue(_IdArchivoConfiguracion).ToString()
                            If Not Reader.IsDBNull(_IdSeccionCriterio) Then .IdSeccionCriterio = Reader.GetValue(_IdSeccionCriterio).ToString()
                            If Not Reader.IsDBNull(_IdSubSeccionCriterio) Then .IdSubSeccionCriterio = Reader.GetValue(_IdSubSeccionCriterio).ToString()
                            If Not Reader.IsDBNull(_Tipo) Then .Tipo = Reader.GetValue(_Tipo)
                            If Not Reader.IsDBNull(_Codigo) Then .Codigo = Reader.GetValue(_Codigo).ToString()
                            If Not Reader.IsDBNull(_Nombre) Then .Nombre = Reader.GetValue(_Nombre).ToString()
                            If Not Reader.IsDBNull(_Archivo) Then .Archivo = Reader.GetValue(_Archivo).ToString()
                            If Not Reader.IsDBNull(_Valor) Then .Valor = Reader.GetValue(_Valor).ToString()

                        End With
                        lista.Add(ebeArchivoConfiguracion)
                    End While
                    blnValido = True
                End Using
            End Using
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function BuscarId(ByVal cnnSql As SqlConnection, ByRef obeArchivoConfiguracion As beArchivoConfiguracion) As Boolean
        Try
            blnValido = False
            Using cmdSql As SqlCommand = cnnSql.CreateCommand()
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.CommandText = objBBDD.StoreProcedure.uspArchivoConfiguracionBuscarId
                With obeArchivoConfiguracion
                    cmdSql.Parameters.Add(uData.CreaParametro("@IdArchivoConfiguracion", obeArchivoConfiguracion.IdArchivoConfiguracion, SqlDbType.Int))
                    Dim dr As SqlDataReader = cmdSql.ExecuteReader()
                    If dr.HasRows Then
                        dr.Read()
                        With obeArchivoConfiguracion
                            Dim _IdArchivoConfiguracion As String = dr.GetOrdinal("IdArchivoConfiguracion")
                            Dim _IdSeccionCriterio As String = dr.GetOrdinal("IdSeccionCriterio")
                            Dim _IdSubSeccionCriterio As String = dr.GetOrdinal("IdSubSeccionCriterio")
                            Dim _Tipo As String = dr.GetOrdinal("Tipo")
                            Dim _Codigo As String = dr.GetOrdinal("Codigo")
                            Dim _Nombre As String = dr.GetOrdinal("Nombre")
                            Dim _Archivo As String = dr.GetOrdinal("Archivo")
                            Dim _Valor As String = dr.GetOrdinal("Valor")

                            If Not dr.IsDBNull(_IdArchivoConfiguracion) Then .IdArchivoConfiguracion = dr.GetValue(_IdArchivoConfiguracion).ToString()
                            If Not dr.IsDBNull(_IdSeccionCriterio) Then .IdSeccionCriterio = dr.GetValue(_IdSeccionCriterio).ToString()
                            If Not dr.IsDBNull(_IdSubSeccionCriterio) Then .IdSubSeccionCriterio = dr.GetValue(_IdSubSeccionCriterio).ToString()
                            If Not dr.IsDBNull(_Tipo) Then .Tipo = dr.GetValue(_Tipo)
                            If Not dr.IsDBNull(_Codigo) Then .Codigo = dr.GetValue(_Codigo).ToString()
                            If Not dr.IsDBNull(_Nombre) Then .Nombre = dr.GetValue(_Nombre).ToString()
                            If Not dr.IsDBNull(_Archivo) Then .Archivo = dr.GetValue(_Archivo).ToString()
                            If Not dr.IsDBNull(_Valor) Then .Valor = dr.GetValue(_Valor)

                        End With
                    Else
                        blnValido = False
                    End If
                End With
            End Using
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function BuscarArchivoSeccion(ByVal cnnSql As SqlConnection, ByVal obeTablaMaestra As beTablaMaestra, ByRef lista As DataTable) As Boolean
        Try
            blnValido = False
            Using cmdSql As SqlCommand = cnnSql.CreateCommand()
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.CommandText = objBBDD.StoreProcedure.uspArchivoConfiguracionBuscarArchivo

                cmdSql.Parameters.Add(uData.CreaParametro("@IdSeccion", obeTablaMaestra.IdSeccion, SqlDbType.VarChar, 500))
                cmdSql.Parameters.Add(uData.CreaParametro("@IdCotizacion", obeTablaMaestra.IdTablaMaestra, SqlDbType.Int))
                Dim da As New SqlDataAdapter(cmdSql)
                da.Fill(lista)
            End Using

        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function
    Public Function BuscarArchivoProductoGeneral(ByVal cnnSql As SqlConnection, ByVal obeTablaMaestra As beTablaMaestra, ByRef lista As DataTable) As Boolean
        Try
            blnValido = False
            Using cmdSql As SqlCommand = cnnSql.CreateCommand()
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.CommandText = objBBDD.StoreProcedure.uspArchivoConfiguracionBuscarGeneral

                cmdSql.Parameters.Add(uData.CreaParametro("@Codigo", obeTablaMaestra.Codigo, SqlDbType.VarChar, 1000))
                cmdSql.Parameters.Add(uData.CreaParametro("@TipoConsulta", obeTablaMaestra.Nombre, SqlDbType.VarChar, 50))
                Dim da As New SqlDataAdapter(cmdSql)
                da.Fill(lista)
                blnValido = True
            End Using
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function
    Public Function BuscarArchivoProducto(ByVal cnnSql As SqlConnection, ByVal obeTablaMaestra As beTablaMaestra, ByRef lista As DataTable) As Boolean
        Try
            blnValido = False
            Using cmdSql As SqlCommand = cnnSql.CreateCommand()
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.CommandText = objBBDD.StoreProcedure.uspArchivoConfiguracionBuscarArchivoProd

                cmdSql.Parameters.Add(uData.CreaParametro("@IdSeccionCriterio", obeTablaMaestra.IdSeccionCriterio, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@IdCotizacion", obeTablaMaestra.IdTablaMaestra, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@TipoSeccion", obeTablaMaestra.Nombre, SqlDbType.VarChar, 50))
                Dim da As New SqlDataAdapter(cmdSql)
                da.Fill(lista)
            End Using
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function
    Public Function BuscarIdSeccionCriterio(ByVal cnnSql As SqlConnection, ByVal obeTablaMaestra As beTablaMaestra, ByRef lista As DataTable) As Boolean
        Try
            blnValido = False
            Using cmdSql As SqlCommand = cnnSql.CreateCommand()
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.CommandText = objBBDD.StoreProcedure.uspArchivoConfiguracionBuscarIdSeccionCriterio

                cmdSql.Parameters.Add(uData.CreaParametro("@IdSeccionCriterio", obeTablaMaestra.IdSeccionCriterio, SqlDbType.Int))
                Dim da As New SqlDataAdapter(cmdSql)
                da.Fill(lista)
            End Using
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function
    Public Function BuscarCodigoYSeccion(ByVal cnnSql As SqlConnection, ByRef obeArchivoConfiguracion As beArchivoConfiguracion, ByRef listaArchivoConfiguracion As List(Of beArchivoConfiguracion)) As Boolean
        Try
            blnValido = False
            Using cmdSql As SqlCommand = cnnSql.CreateCommand()
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.CommandText = objBBDD.StoreProcedure.uspArchivoConfiguracionBuscarCodigoYSeccion
                With obeArchivoConfiguracion
                    cmdSql.Parameters.Add(uData.CreaParametro("@Codigo", obeArchivoConfiguracion.Codigo, SqlDbType.VarChar, 50))
                    cmdSql.Parameters.Add(uData.CreaParametro("@IdSeccion", obeArchivoConfiguracion.CodigoSeccion, SqlDbType.Int))
                    Dim dr As SqlDataReader = cmdSql.ExecuteReader()
                    If dr.HasRows Then
                        While dr.Read
                            Dim ebeArchivoConfiguracion As New beArchivoConfiguracion
                            With ebeArchivoConfiguracion
                                Dim _IdArchivoConfiguracion As String = dr.GetOrdinal("IdArchivoConfiguracion")
                                Dim _IdSeccionCriterio As String = dr.GetOrdinal("IdSeccionCriterio")
                                Dim _IdSubSeccionCriterio As String = dr.GetOrdinal("IdSubSeccionCriterio")
                                Dim _Tipo As String = dr.GetOrdinal("Tipo")
                                Dim _Codigo As String = dr.GetOrdinal("Codigo")
                                Dim _Nombre As String = dr.GetOrdinal("Nombre")
                                Dim _Archivo As String = dr.GetOrdinal("Archivo")
                                Dim _Valor As String = dr.GetOrdinal("Valor")
                                Dim _NombreSeccion As String = dr.GetOrdinal("NombreSeccion")

                                If Not dr.IsDBNull(_IdArchivoConfiguracion) Then .IdArchivoConfiguracion = dr.GetValue(_IdArchivoConfiguracion).ToString()
                                If Not dr.IsDBNull(_IdSeccionCriterio) Then .IdSeccionCriterio = dr.GetValue(_IdSeccionCriterio).ToString()
                                If Not dr.IsDBNull(_IdSubSeccionCriterio) Then .IdSubSeccionCriterio = dr.GetValue(_IdSubSeccionCriterio).ToString()
                                If Not dr.IsDBNull(_Tipo) Then .Tipo = dr.GetValue(_Tipo)
                                If Not dr.IsDBNull(_Codigo) Then .Codigo = dr.GetValue(_Codigo).ToString()
                                If Not dr.IsDBNull(_Nombre) Then .Nombre = dr.GetValue(_Nombre).ToString()
                                If Not dr.IsDBNull(_Archivo) Then .Archivo = dr.GetValue(_Archivo).ToString()
                                If Not dr.IsDBNull(_Valor) Then .Valor = dr.GetValue(_Valor)
                                If Not dr.IsDBNull(_NombreSeccion) Then .NombreSeccion = dr.GetValue(_NombreSeccion)
                                listaArchivoConfiguracion.Add(ebeArchivoConfiguracion)
                            End With
                        End While
                    Else
                        blnValido = False
                    End If
                End With
            End Using
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function
End Class
