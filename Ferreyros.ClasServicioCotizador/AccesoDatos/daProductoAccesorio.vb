Imports System.Data
Imports System.Data.SqlClient


Public Class daProductoAccesorio

    Private uData As New Utiles.Datos
    Private uConfig As New Utiles.uConfiguracion
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function Insertar(ByVal cnnSql As SqlConnection, ByVal obeProductoAccesorio As beProductoAccesorio _
                             , Optional ByVal trSql As SqlTransaction = Nothing) As Boolean
        Dim boolReturn As Boolean = False
        Try
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            If trSql IsNot Nothing Then
                cmdSql.Transaction = trSql
            End If

            cmdSql.CommandText = "uspServicioProductoAccesorioInsertar"
            cmdSql.CommandType = CommandType.StoredProcedure
            With obeProductoAccesorio
                cmdSql.Parameters.Add(uData.CreaParametro("@IdProductoAccesorio", .IdProductoAccesorio, SqlDbType.VarChar, 20, ParameterDirection.InputOutput))
                cmdSql.Parameters.Add(uData.CreaParametro("@IdProducto", uConfig.fc_ConvertirEntero(.IdProducto), SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@IdAccesorio", .IdAccesorio, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@CodigoProductoAccesorio", .CodigoProductoAccesorio, SqlDbType.VarChar, 30))
                cmdSql.Parameters.Add(uData.CreaParametro("@NombreProductoAccesorio", .NombreProductoAccesorio, SqlDbType.VarChar, 200))
                cmdSql.Parameters.Add(uData.CreaParametro("@Cantidad", uConfig.fc_ConvertirEntero(.Cantidad), SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@UnidadMedida", .UnidadMedida, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@ValorLista", uConfig.fc_ConvertirDouble(.ValorLista), SqlDbType.Decimal))
                cmdSql.Parameters.Add(uData.CreaParametro("@MonedaValorLista", .MonedaValorLista, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@FlatMostrarEspTecnica", uConfig.fc_ConvertirBoolean(.FlatMostrarEspTecnica), SqlDbType.Bit))
                cmdSql.Parameters.Add(uData.CreaParametro("@UsuarioCreacion", .UsuarioCreacion, SqlDbType.VarChar, 15))

                cmdSql.ExecuteNonQuery()
                .IdProductoAccesorio = cmdSql.Parameters("@IdProductoAccesorio").Value
                boolReturn = True
            End With
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return boolReturn
    End Function
End Class
