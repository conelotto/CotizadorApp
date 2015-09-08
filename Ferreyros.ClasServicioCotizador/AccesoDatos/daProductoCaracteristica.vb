Imports System.Data.SqlClient
Public Class daProductoCaracteristica
    Private uData As New Utiles.Datos
    Private uConfig As New Utiles.uConfiguracion
    Private strError As String = String.Empty
    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function Insertar(ByVal cnnSql As SqlConnection, ByVal obeProductoCaracteristica As beProductoCaracteristica _
                             , Optional ByVal trSql As SqlTransaction = Nothing) As Boolean
        Dim boolReturn As Boolean = False
        Try
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            If trSql IsNot Nothing Then
                cmdSql.Transaction = trSql
            End If
            cmdSql.CommandText = "uspServicioProductoCaracteristicaInsertar"
            cmdSql.CommandType = CommandType.StoredProcedure

            With obeProductoCaracteristica
                cmdSql.Parameters.Add(uData.CreaParametro("@IdProductoCaracteristica", .IdProductoCaracteristica, SqlDbType.BigInt, ParameterDirection.InputOutput))
                cmdSql.Parameters.Add(uData.CreaParametro("@IdProducto", uConfig.fc_ConvertirEntero(.IdProducto), SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@Posicion", .Posicion, SqlDbType.VarChar, 5))
                cmdSql.Parameters.Add(uData.CreaParametro("@CodigoAtributo", .CodigoAtributo, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@DescripcionAtributo", .DescripcionAtributo, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@ValorAtributo", .ValorAtributo, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@CodigoUnidadMedida", .CodigoUnidadMedida, SqlDbType.VarChar, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@NombreUnidadMedida", .NombreUnidadMedida, SqlDbType.VarChar, 50))
                cmdSql.Parameters.Add(uData.CreaParametro("@UsuarioCreacion", .UsuarioCreacion, SqlDbType.VarChar, 15))

                cmdSql.ExecuteNonQuery()
                .IdProductoCaracteristica = cmdSql.Parameters("@IdProductoCaracteristica").Value
                boolReturn = True
            End With

        Catch ex As Exception
            strError = ex.Message
        End Try
        Return boolReturn
    End Function

End Class
