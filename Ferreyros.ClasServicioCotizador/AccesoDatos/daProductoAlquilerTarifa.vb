Imports System.Data.SqlClient
Public Class daProductoAlquilerTarifa

    Private uData As New Utiles.Datos
    Private uConfig As New Utiles.uConfiguracion
    Private strError As String = String.Empty
    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function Insertar(ByVal cnnSql As SqlConnection, ByVal obeProductoAlquilerTarifa As beProductoAlquilerTarifa _
                             , Optional ByVal trSql As SqlTransaction = Nothing) As Boolean
        Dim boolReturn As Boolean = False
        Try
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            If trSql IsNot Nothing Then
                cmdSql.Transaction = trSql
            End If
            cmdSql.CommandText = "uspServicioProductoAlquilerTarifaInsertar"
            cmdSql.CommandType = CommandType.StoredProcedure

            With obeProductoAlquilerTarifa
                cmdSql.Parameters.Add(uData.CreaParametro("@IdProductoAlquilerTarifa", .IdProductoAlquilerTarifa, SqlDbType.BigInt, ParameterDirection.InputOutput))
                cmdSql.Parameters.Add(uData.CreaParametro("@IdProducto", uConfig.fc_ConvertirEntero(.IdProducto), SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@ValorEscala", .ValorEscala, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@Importe", uConfig.fc_ConvertirDouble(.Importe), SqlDbType.Decimal))
                cmdSql.Parameters.Add(uData.CreaParametro("@Moneda", .Moneda, SqlDbType.VarChar, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@UnidadMedidaPrecio", .UnidadMedidaPrecio, SqlDbType.VarChar, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@CodigoUnidadMedida", .CodigoUnidadMedida, SqlDbType.VarChar, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@UsuarioCreacion", .UsuarioCreacion, SqlDbType.VarChar, 15))

                cmdSql.ExecuteNonQuery()
                .IdProductoAlquilerTarifa = cmdSql.Parameters("@IdProductoAlquilerTarifa").Value
                boolReturn = True
            End With

        Catch ex As Exception
            strError = ex.Message
        End Try
        Return boolReturn
    End Function
End Class
