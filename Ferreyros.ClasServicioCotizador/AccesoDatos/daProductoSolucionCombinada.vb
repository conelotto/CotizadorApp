Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras

Public Class daProductoSolucionCombinada

    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty
    Private uConfig As New Utiles.uConfiguracion

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property
    Public Function Insertar(ByVal cnnSql As SqlConnection, ByVal obeProductoSolucionCombinada As beProductoPrime, _
                              Optional ByVal trSql As SqlTransaction = Nothing) As Boolean
        blnValido = False
        strError = String.Empty
        Try
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand
            If trSql IsNot Nothing Then
                cmdSql.Transaction = trSql
            End If
            cmdSql.CommandText = "uspServicioProductoSolucionCombinadaInsertar"
            cmdSql.CommandType = CommandType.StoredProcedure
            'With obeProductoSolucionCombinada
            '    cmdSql.Parameters.Add(uData.CreaParametro("@IdProducto", .IdProducto, SqlDbType.Int, ParameterDirection.InputOutput))
            '    cmdSql.Parameters.Add(uData.CreaParametro("@Duracion ", uConfig.fc_ConvertirEntero(.Duracion), SqlDbType.Int))
            '    cmdSql.Parameters.Add(uData.CreaParametro("@PlazoRenovacion", uConfig.fc_ConvertirEntero(.PlazoRenovacion), SqlDbType.Int))
            '    cmdSql.Parameters.Add(uData.CreaParametro("@FechaInicioContrato", uConfig.fc_ConvertirFecha(.FechaInicioContrato), SqlDbType.Date))
            '    cmdSql.Parameters.Add(uData.CreaParametro("@FechaEstimadaCierre", uConfig.fc_ConvertirFecha(.FechaEstimadaCierre), SqlDbType.Date))
            '    cmdSql.Parameters.Add(uData.CreaParametro("@ParticipacionVendedor1", uConfig.fc_ConvertirEntero(.ParticipacionVendedor1), SqlDbType.Int))
            '    cmdSql.Parameters.Add(uData.CreaParametro("@ParticipacionVendedor2", uConfig.fc_ConvertirEntero(.ParticipacionVendedor2), SqlDbType.Int)) 

            '    cmdSql.Parameters.Add(uData.CreaParametro("@UsuarioCreacion", .UsuarioCreacion, SqlDbType.VarChar, 15))
            '    cmdSql.ExecuteNonQuery()

            '    .IdProducto = cmdSql.Parameters("@IdProducto").Value
            '    blnValido = True
            'End With
            With obeProductoSolucionCombinada
                cmdSql.Parameters.Add(uData.CreaParametro("@IdProducto", .IdProducto, SqlDbType.Int, ParameterDirection.InputOutput))
                cmdSql.Parameters.Add(uData.CreaParametro("@FechaEstimCierre ", uConfig.fc_ConvertirFecha(.FechaEstimCierre), SqlDbType.Date))
                cmdSql.Parameters.Add(uData.CreaParametro("@PlazoEntregaEstim", .PlazoEntregaEstim, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@CodigoFormaPago", .CodigoFormaPago, SqlDbType.VarChar, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@FormaPago", .FormaPago, SqlDbType.VarChar, 200))
                cmdSql.Parameters.Add(uData.CreaParametro("@FlatIncluyeRecompra", uConfig.fc_ConvertirBoolean(.FlatIncluyeRecompra), SqlDbType.Bit))
                cmdSql.Parameters.Add(uData.CreaParametro("@FlatIncluyeCLC", uConfig.fc_ConvertirBoolean(.FlatIncluyeCLC), SqlDbType.Bit))
                cmdSql.Parameters.Add(uData.CreaParametro("@PromHorasMensualUso", uConfig.fc_ConvertirDouble(.PromHorasMensualUso), SqlDbType.Decimal))
                cmdSql.Parameters.Add(uData.CreaParametro("@AnioFabricacion", .AnioFabricacion, SqlDbType.VarChar, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@Garantia", .Garantia, SqlDbType.VarChar, 50))
                cmdSql.Parameters.Add(uData.CreaParametro("@Condicion", .Condicion, SqlDbType.VarChar, 50))
                cmdSql.Parameters.Add(uData.CreaParametro("@PlazoEntrega", .PlazoEntrega, SqlDbType.VarChar, 50))
                cmdSql.Parameters.Add(uData.CreaParametro("@Orden", .Orden, SqlDbType.VarChar, 50))
                cmdSql.Parameters.Add(uData.CreaParametro("@Serie", .Serie, SqlDbType.VarChar, 50))
                cmdSql.Parameters.Add(uData.CreaParametro("@Horas", .Horas, SqlDbType.VarChar, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@CodClasificacion", .CodClasificacion, SqlDbType.VarChar, 50))
                cmdSql.Parameters.Add(uData.CreaParametro("@Interlote", .Interlote, SqlDbType.VarChar, 5))

                cmdSql.Parameters.Add(uData.CreaParametro("@UsuarioCreacion", .UsuarioCreacion, SqlDbType.VarChar, 15))
                cmdSql.ExecuteNonQuery()

                .IdProducto = cmdSql.Parameters("@IdProducto").Value
                blnValido = True
            End With
        Catch ex As Exception
            strError = ex.Message
        End Try

        Return blnValido
    End Function

End Class
