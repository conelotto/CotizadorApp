Imports System.Data.SqlClient
Imports Ferreyros.BECotizador

Public Class daCotizacionVersion

    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property
    Public Function Insertar(ByVal cnnSql As SqlConnection, ByVal obeCotizacionVersion As beCotizacionVersion, Optional ByVal trSql As SqlTransaction = Nothing) As Boolean
        blnValido = False
        strError = String.Empty
        Try
            Using cmd As SqlCommand = cnnSql.CreateCommand()
                If Not IsNothing(trSql) Then
                    cmd.Transaction = trSql
                End If
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = objBBDD.StoreProcedure.uspCotizacionVersionInsertar
                With obeCotizacionVersion
                    cmd.Parameters.Add(uData.CreaParametro("@IdCotizacionVersion", .IdCotizacionVersion, SqlDbType.VarChar, 20, ParameterDirection.InputOutput))
                    cmd.Parameters.Add(uData.CreaParametro("@IdCotizacion", .IdCotizacion, SqlDbType.Int))
                    cmd.Parameters.Add(uData.CreaParametro("@NumVersion", .NumVersion, SqlDbType.VarChar, 5))
                    cmd.Parameters.Add(uData.CreaParametro("@NombreArchivo", .NombreArchivo, SqlDbType.VarChar, 100))
                    cmd.Parameters.Add(uData.CreaParametro("@UsuarioCreacion", .UsuarioCreacion, SqlDbType.VarChar, 20))
                    cmd.Parameters.Add(uData.CreaParametro("@TieneDetalleParte", .TieneDetallePartes, SqlDbType.TinyInt))
                    cmd.ExecuteNonQuery()
                    .IdCotizacionVersion = cmd.Parameters("@IdCotizacionVersion").Value.ToString()
                    blnValido = True
                End With
            End Using
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function BuscarIdCotizacionSap(ByVal cnnSql As SqlConnection, ByRef obeCotizacionVersion As beCotizacionVersion, ByRef lista As List(Of beCotizacionVersion)) As Boolean
        Try
            blnValido = False
            Using cmdSql As SqlCommand = cnnSql.CreateCommand()
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.CommandText = objBBDD.StoreProcedure.uspCotizacionVersionBuscarIdCotizacionSap

                cmdSql.Parameters.Add(uData.CreaParametro("@IdCotizacionSap", obeCotizacionVersion.IdCotizacionSap, SqlDbType.VarChar, 20))

                Using dr As SqlDataReader = cmdSql.ExecuteReader

                    Dim _IdCotizacionVersion As String = dr.GetOrdinal("IdCotizacionVersion")
                    Dim _IdCotizacion As String = dr.GetOrdinal("IdCotizacion")
                    Dim _IdCotizacionSap As String = dr.GetOrdinal("IdCotizacionSap")
                    Dim _DescripcionProducto As String = dr.GetOrdinal("DescripcionProducto")
                    Dim _NumVersion As String = dr.GetOrdinal("NumVersion")
                    Dim _Fecha As String = dr.GetOrdinal("Fecha")
                    Dim _ValorNegociado As String = dr.GetOrdinal("ValorNegociado")
                    Dim _Moneda As String = dr.GetOrdinal("Moneda")
                    Dim _NombreArchivo As String = dr.GetOrdinal("NombreArchivo")
                    Dim _TieneDetalleParte As String = dr.GetOrdinal("TieneDetalleParte")

                    Dim ebeCotizacionVersion As beCotizacionVersion = Nothing

                    While dr.Read()
                        ebeCotizacionVersion = New beCotizacionVersion

                        With ebeCotizacionVersion
                            If Not dr.IsDBNull(_IdCotizacionVersion) Then .IdCotizacionVersion = dr.GetValue(_IdCotizacionVersion).ToString()
                            If Not dr.IsDBNull(_IdCotizacion) Then .IdCotizacion = dr.GetValue(_IdCotizacion).ToString()
                            If Not dr.IsDBNull(_IdCotizacionSap) Then .IdCotizacionSap = dr.GetValue(_IdCotizacionSap).ToString()
                            If Not dr.IsDBNull(_DescripcionProducto) Then .DescripcionProducto = dr.GetValue(_DescripcionProducto).ToString()
                            If Not dr.IsDBNull(_NumVersion) Then .NumVersion = dr.GetValue(_NumVersion).ToString()
                            If Not dr.IsDBNull(_Fecha) Then .Fecha = dr.GetValue(_Fecha).ToString()
                            If Not dr.IsDBNull(_ValorNegociado) Then .ValorNegociado = dr.GetValue(_ValorNegociado).ToString()
                            If Not dr.IsDBNull(_Moneda) Then .Moneda = dr.GetValue(_Moneda).ToString()
                            If Not dr.IsDBNull(_NombreArchivo) Then .NombreArchivo = dr.GetValue(_NombreArchivo).ToString()
                            If Not dr.IsDBNull(_TieneDetalleParte) Then .TieneDetallePartes = dr.GetValue(_TieneDetalleParte).ToString()
                        End With
                        lista.Add(ebeCotizacionVersion)
                    End While
                    dr.Close()
                End Using
                blnValido = True
            End Using
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function CrearVersion(ByVal cnnSql As SqlConnection, ByVal obeCotizacionVersion As beCotizacionVersion) As beCotizacionVersion
        Dim ebeCotizacionVersion As beCotizacionVersion

        Try

            Using cmdSql As SqlCommand = cnnSql.CreateCommand()

                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.CommandText = objBBDD.StoreProcedure.UspCotizacionVersionCrearVersion

                cmdSql.Parameters.Add(uData.CreaParametro("@IdCotizacionSap", obeCotizacionVersion.IdCotizacionSap, SqlDbType.VarChar, 20))

                Using dr As SqlDataReader = cmdSql.ExecuteReader
                    Dim _NumVersion As String = dr.GetOrdinal("NumVersion")

                    While dr.Read()
                        ebeCotizacionVersion = New beCotizacionVersion
                        ebeCotizacionVersion.IdCotizacion = obeCotizacionVersion.IdCotizacion
                        ebeCotizacionVersion.IdCotizacionSap = obeCotizacionVersion.IdCotizacionSap
                        With ebeCotizacionVersion
                            If Not dr.IsDBNull(_NumVersion) Then .NumVersion = dr.GetValue(_NumVersion).ToString()
                        End With
                    End While
                    dr.Close()
                End Using
                blnValido = True
            End Using
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return ebeCotizacionVersion
    End Function

    Public Function ActualizarArchivo(ByVal cnnSql As SqlConnection, ByVal obeCotizacionVersion As beCotizacionVersion, ByRef eValidacion As beValidacion) As Boolean
        Dim ebeCotizacionVersion As beCotizacionVersion
        Dim boolRetorno As Boolean = False
        Try

            Using cmdSql As SqlCommand = cnnSql.CreateCommand()

                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.CommandText = objBBDD.StoreProcedure.uspCotizacionVersionActualizarArchivo

                cmdSql.Parameters.Add(uData.CreaParametro("@IdCotizacionVersion", obeCotizacionVersion.IdCotizacionVersion, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@NombreArchivo", obeCotizacionVersion.NombreArchivo, SqlDbType.VarChar, 100))

                Using dr As SqlDataReader = cmdSql.ExecuteReader
                    Dim _IdCotizacionVersion As String = dr.GetOrdinal("IdCotizacionVersion")
                    Dim _NombreArchivo As String = dr.GetOrdinal("NombreArchivo")

                    While dr.Read()
                        ebeCotizacionVersion = New beCotizacionVersion
                        With ebeCotizacionVersion
                            If Not dr.IsDBNull(_IdCotizacionVersion) Then .IdCotizacionVersion = dr.GetValue(_IdCotizacionVersion).ToString()
                            If Not dr.IsDBNull(_NombreArchivo) Then .NombreArchivo = dr.GetValue(_NombreArchivo).ToString()
                        End With
                    End While
                    dr.Close()
                End Using
                boolRetorno = True
                eValidacion.validacion = True
            End Using
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return boolRetorno
    End Function
End Class
