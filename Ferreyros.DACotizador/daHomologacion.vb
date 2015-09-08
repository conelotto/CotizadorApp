Imports System.Data.SqlClient
Imports Ferreyros.BECotizador

Public Class daHomologacion

    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function Insertar(ByVal cnnSql As SqlConnection, ByVal obeHomologacion As beHomologacion, Optional ByVal trSql As SqlTransaction = Nothing) As Boolean
        blnValido = False
        strError = String.Empty
        Try
            Dim cmd As SqlCommand = cnnSql.CreateCommand()
            If Not IsNothing(trSql) Then
                cmd.Transaction = trSql
            End If
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = objBBDD.StoreProcedure.uspHomologacionInsertar
            With obeHomologacion
                cmd.Parameters.Add(uData.CreaParametro("@IdHomologacion", .IdHomologacion, SqlDbType.BigInt, ParameterDirection.InputOutput))
                cmd.Parameters.Add(uData.CreaParametro("@Tabla", .Tabla, SqlDbType.VarChar, 50))
                cmd.Parameters.Add(uData.CreaParametro("@Descripcion", .Descripcion, SqlDbType.VarChar, 100))
                cmd.Parameters.Add(uData.CreaParametro("@ValorSap", .ValorSap, SqlDbType.VarChar, 100))
                cmd.Parameters.Add(uData.CreaParametro("@ValorCotizador", .ValorCotizador, SqlDbType.VarChar, 100))

                cmd.ExecuteNonQuery()
                .IdHomologacion = cmd.Parameters("IdHomologacion").Value.ToString()
                blnValido = True
            End With

        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function Actualizar(ByVal cnnSql As SqlConnection, ByVal obeHomologacion As beHomologacion, Optional ByVal trSql As SqlTransaction = Nothing)
        blnValido = True
        strError = String.Empty

        Try
            Dim cmd As SqlCommand = cnnSql.CreateCommand()
            If Not IsNothing(trSql) Then
                cmd.Transaction = trSql
            End If

            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = objBBDD.StoreProcedure.uspHomologacionActualizar
            With obeHomologacion
                cmd.Parameters.Add(uData.CreaParametro("@IdHomologacion", .IdHomologacion, SqlDbType.BigInt))
                cmd.Parameters.Add(uData.CreaParametro("@Tabla", .Tabla, SqlDbType.VarChar, 50))
                cmd.Parameters.Add(uData.CreaParametro("@Descripcion", .Descripcion, SqlDbType.VarChar, 100))
                cmd.Parameters.Add(uData.CreaParametro("@ValorSap", .ValorSap, SqlDbType.VarChar, 100))
                cmd.Parameters.Add(uData.CreaParametro("@ValorCotizador", .ValorCotizador, SqlDbType.VarChar, 100))
            End With
            cmd.ExecuteNonQuery()
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function Eliminar(ByVal cnnSql As SqlConnection, ByVal obeHomologacion As beHomologacion, ByRef eValidacion As beValidacion) As Boolean
        blnValido = False
        strError = String.Empty
        Try
            Dim cmd As SqlCommand = cnnSql.CreateCommand()

            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = objBBDD.StoreProcedure.uspHomologacionEliminar
            With obeHomologacion
                cmd.Parameters.Add(uData.CreaParametro("@IdHomologacion", .IdHomologacion, SqlDbType.BigInt, ParameterDirection.Input))
                cmd.Parameters.Add(uData.CreaParametro("@Result", Nothing, SqlDbType.VarChar, 50, ParameterDirection.Output))

                cmd.ExecuteNonQuery()
                Dim Resultado As String = cmd.Parameters("@Result").Value.ToString()
                If Resultado = 1 Then
                    blnValido = True
                    eValidacion.validacion = True
                End If
            End With

        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function BuscarTabla(ByVal cnnSql As SqlConnection, ByVal obeHomologacion As beHomologacion, ByRef ListaHomologacion As List(Of beHomologacion)) As Boolean
        Try
            blnValido = False
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = objBBDD.StoreProcedure.uspHomologacionBuscarTabla

            cmdSql.Parameters.Add(uData.CreaParametro("@Tabla", obeHomologacion.Tabla, SqlDbType.VarChar, 50))

            Using dr As SqlDataReader = cmdSql.ExecuteReader 
                Dim _IdHomologacion As String = dr.GetOrdinal("IdHomologacion")
                Dim _Tabla As String = dr.GetOrdinal("Tabla")
                Dim _Descripcion As String = dr.GetOrdinal("Descripcion")
                Dim _ValorSap As String = dr.GetOrdinal("ValorSap")
                Dim _ValorCotizador As String = dr.GetOrdinal("ValorCotizador")

                Dim ebeHomologacionNew As beHomologacion = Nothing

                While dr.Read()
                    ebeHomologacionNew = New beHomologacion

                    With ebeHomologacionNew
                        If Not dr.IsDBNull(_IdHomologacion) Then .IdHomologacion = dr.GetValue(_IdHomologacion).ToString
                        If Not dr.IsDBNull(_Tabla) Then .Tabla = dr.GetValue(_Tabla).ToString()
                        If Not dr.IsDBNull(_Descripcion) Then .Descripcion = dr.GetValue(_Descripcion).ToString()
                        If Not dr.IsDBNull(_ValorSap) Then .ValorSap = dr.GetValue(_ValorSap).ToString()
                        If Not dr.IsDBNull(_ValorCotizador) Then .ValorCotizador = dr.GetValue(_ValorCotizador).ToString()
                    End With
                    ListaHomologacion.Add(ebeHomologacionNew)
                End While
                dr.Close()
            End Using
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function Listar(ByVal cnnSql As SqlConnection, ByRef ListaHomologacion As List(Of beHomologacion)) As Boolean
        Try
            blnValido = False
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = objBBDD.StoreProcedure.uspHomologacionListar
            'cmdSql.Parameters.Add(uData.CreaParametro("@Tabla", obeHomologacion.Tabla, SqlDbType.VarChar, 50))

            Using dr As SqlDataReader = cmdSql.ExecuteReader
                Dim _IdHomologacion As String = dr.GetOrdinal("IdHomologacion")
                Dim _Tabla As String = dr.GetOrdinal("Tabla")
                Dim _Descripcion As String = dr.GetOrdinal("Descripcion")
                Dim _ValorSap As String = dr.GetOrdinal("ValorSap")
                Dim _ValorCotizador As String = dr.GetOrdinal("ValorCotizador")

                Dim ebeHomologacionNew As beHomologacion = Nothing

                While dr.Read()
                    ebeHomologacionNew = New beHomologacion

                    With ebeHomologacionNew
                        If Not dr.IsDBNull(_IdHomologacion) Then .IdHomologacion = dr.GetValue(_IdHomologacion).ToString
                        If Not dr.IsDBNull(_Tabla) Then .Tabla = dr.GetValue(_Tabla).ToString()
                        If Not dr.IsDBNull(_Descripcion) Then .Descripcion = dr.GetValue(_Descripcion).ToString()
                        If Not dr.IsDBNull(_ValorSap) Then .ValorSap = dr.GetValue(_ValorSap).ToString()
                        If Not dr.IsDBNull(_ValorCotizador) Then .ValorCotizador = dr.GetValue(_ValorCotizador).ToString()
                    End With
                    ListaHomologacion.Add(ebeHomologacionNew)
                End While
                dr.Close()
            End Using
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function BuscarTablaSC(ByVal cnnSql As SqlConnection, ByVal obeHomologacion As beHomologacion, ByRef ListaHomologacion As List(Of beHomologacion)) As Boolean
        Dim blnValido As Boolean = False
        strError = String.Empty
        Try
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = "uspHomologacionBuscarTabla_SC"

            cmdSql.Parameters.Add(uData.CreaParametro("@Tabla", obeHomologacion.Tabla, SqlDbType.VarChar, 50))
            cmdSql.Parameters.Add(uData.CreaParametro("@ValorSAP", obeHomologacion.ValorSap, SqlDbType.VarChar, 100))

            Using dr As SqlDataReader = cmdSql.ExecuteReader
                Dim _IdHomologacion As String = dr.GetOrdinal("IdHomologacion")
                Dim _Tabla As String = dr.GetOrdinal("Tabla")
                Dim _Descripcion As String = dr.GetOrdinal("Descripcion")
                Dim _ValorSap As String = dr.GetOrdinal("ValorSap")
                Dim _ValorCotizador As String = dr.GetOrdinal("ValorCotizador")

                Dim ebeHomologacionNew As beHomologacion = Nothing

                While dr.Read()
                    ebeHomologacionNew = New beHomologacion

                    With ebeHomologacionNew
                        If Not dr.IsDBNull(_IdHomologacion) Then .IdHomologacion = dr.GetValue(_IdHomologacion).ToString
                        If Not dr.IsDBNull(_Tabla) Then .Tabla = dr.GetValue(_Tabla).ToString()
                        If Not dr.IsDBNull(_Descripcion) Then .Descripcion = dr.GetValue(_Descripcion).ToString()
                        If Not dr.IsDBNull(_ValorSap) Then .ValorSap = dr.GetValue(_ValorSap).ToString()
                        If Not dr.IsDBNull(_ValorCotizador) Then .ValorCotizador = dr.GetValue(_ValorCotizador).ToString()
                    End With
                    ListaHomologacion.Add(ebeHomologacionNew)
                End While
                dr.Close()
            End Using
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

End Class
