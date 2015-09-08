Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras

Public Class daDetalleRecompra

    Private uConfig As New Utiles.uConfiguracion
    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function Insertar(ByVal cnnSql As SqlConnection, ByVal obeDetalleRecompra As beDetalleRecompra, _
                              Optional ByVal trSql As SqlTransaction = Nothing) As Boolean
        blnValido = False
        strError = String.Empty
        Try
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand
            If trSql IsNot Nothing Then
                cmdSql.Transaction = trSql
            End If
            cmdSql.CommandText = "uspServicioDetalleRecompraInsertar"
            cmdSql.CommandType = CommandType.StoredProcedure
            With obeDetalleRecompra
                cmdSql.Parameters.Add(uData.CreaParametro("@IdDetalleRecompra", .IdDetalleRecompra, SqlDbType.VarChar, 20, ParameterDirection.InputOutput))
                cmdSql.Parameters.Add(uData.CreaParametro("@IdProducto", uConfig.fc_ConvertirEntero(.IdProducto), SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@Numero", uConfig.fc_ConvertirEntero(.Numero), SqlDbType.TinyInt))
                cmdSql.Parameters.Add(uData.CreaParametro("@NumeroHoras", uConfig.fc_ConvertirEntero(.NumeroHoras), SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@NumeroMeses", uConfig.fc_ConvertirEntero(.NumeroMeses), SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@MontoRecompra", uConfig.fc_ConvertirDouble(.MontoRecompra), SqlDbType.Decimal))
                cmdSql.Parameters.Add(uData.CreaParametro("@UsuarioCreacion", .UsuarioCreacion, SqlDbType.VarChar, 15))
                cmdSql.ExecuteNonQuery()
                .IdDetalleRecompra = cmdSql.Parameters("@IdDetalleRecompra").Value
                blnValido = True
            End With
        Catch ex As Exception
            strError = ex.Message
        End Try

        Return blnValido
    End Function

End Class
