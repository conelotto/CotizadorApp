Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras

Public Class daProductoAdicional

    Private uConfig As New Utiles.uConfiguracion
    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty


    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function Insertar(ByVal cnnSql As SqlConnection, ByVal obeProductoAdicional As beProductoAdicional, _
                              Optional ByVal trSql As SqlTransaction = Nothing) As Boolean
        blnValido = False
        strError = String.Empty
        Try
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand
            If trSql IsNot Nothing Then
                cmdSql.Transaction = trSql
            End If
            cmdSql.CommandText = "uspServicioProductoAdicionalInsertar"
            cmdSql.CommandType = CommandType.StoredProcedure
            With obeProductoAdicional
                cmdSql.Parameters.Add(uData.CreaParametro("@IdProductoAdicional", .IdProducto, SqlDbType.VarChar, 20, ParameterDirection.InputOutput))
                cmdSql.Parameters.Add(uData.CreaParametro("@IdProducto", .IdProducto, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@IdAdicional", .IdAdicional, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@CodigoProductoAdicional", .CodigoProductoAdicional, SqlDbType.VarChar, 30))
                cmdSql.Parameters.Add(uData.CreaParametro("@NombreProdutoAdicional", .NombreProdutoAdicional, SqlDbType.VarChar, 200))
                cmdSql.Parameters.Add(uData.CreaParametro("@Cantidad", uConfig.fc_ConvertirEntero(.Cantidad), SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@UnidadMedida", .UnidadMedida, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@ValorCosto", uConfig.fc_ConvertirDouble(.ValorCosto), SqlDbType.Decimal))
                cmdSql.Parameters.Add(uData.CreaParametro("@ValorLista", uConfig.fc_ConvertirDouble(.ValorLista), SqlDbType.Decimal))
                cmdSql.Parameters.Add(uData.CreaParametro("@MonedaValorCosto", .MonedaValorCosto, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@MonedaValorLista", .MonedaValorLista, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@TipoAdicional", .TipoAdicional, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@FlatMostrarEspTecnica", uConfig.fc_ConvertirBoolean(.FlatMostrarEspTecnica), SqlDbType.Bit))
                cmdSql.Parameters.Add(uData.CreaParametro("@UsuarioCreacion", .UsuarioCreacion, SqlDbType.VarChar, 15))

                cmdSql.ExecuteNonQuery()
                .IdProductoAdicional = cmdSql.Parameters("@IdProductoAdicional").Value
                blnValido = True
            End With
        Catch ex As Exception
            strError = ex.Message
        End Try

        Return blnValido
    End Function

End Class
