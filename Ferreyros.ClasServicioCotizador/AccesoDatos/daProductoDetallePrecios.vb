Imports System.Data.SqlClient
Public Class daProductoDetallePrecios
    Private uData As New Utiles.Datos
    Private uConfig As New Utiles.uConfiguracion
    Private strError As String = String.Empty
    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function Insertar(ByVal cnnSql As SqlConnection, ByVal obeProductoDetallePrecios As beProductoDetallePrecios _
                             , Optional ByVal trSql As SqlTransaction = Nothing) As Boolean
        Dim boolReturn As Boolean = False
        Try
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            If trSql IsNot Nothing Then
                cmdSql.Transaction = trSql
            End If
            cmdSql.CommandText = "uspServicioProductoCaracteristicaInsertar"
            cmdSql.CommandType = CommandType.StoredProcedure

            With obeProductoDetallePrecios
                cmdSql.Parameters.Add(uData.CreaParametro("@IdDetallePrecio", .IdDetallePrecio, SqlDbType.Int, ParameterDirection.InputOutput))
                cmdSql.Parameters.Add(uData.CreaParametro("@IdProducto", uConfig.fc_ConvertirEntero(.IdProducto), SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@Codigo", .Codigo, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@Estado", .Estado, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@ElementoPrecio", .ElementoPrecio, SqlDbType.VarChar, 50))
                cmdSql.Parameters.Add(uData.CreaParametro("@Precio", uConfig.fc_ConvertirDouble(.Precio), SqlDbType.Float, 15))
                cmdSql.Parameters.Add(uData.CreaParametro("@Unidad", .Unidad, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@Cantidad", uConfig.fc_ConvertirEntero(.Cantidad), SqlDbType.Int, 15))
                cmdSql.Parameters.Add(uData.CreaParametro("@UM", .UM, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@ValorFinal", uConfig.fc_ConvertirDouble(.ValorFinal), SqlDbType.Float, 15))
                cmdSql.Parameters.Add(uData.CreaParametro("@Moneda", .Moneda, SqlDbType.VarChar, 20))

                cmdSql.ExecuteNonQuery()
                .IdDetallePrecio = cmdSql.Parameters("@IdDetallePrecio").Value
                boolReturn = True
            End With

        Catch ex As Exception
            strError = ex.Message
        End Try
        Return boolReturn
    End Function

End Class
