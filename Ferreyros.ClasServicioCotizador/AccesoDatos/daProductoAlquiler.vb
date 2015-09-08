Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras

Public Class daProductoAlquiler
    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty
    Private uConfig As New Utiles.uConfiguracion

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function Insertar(ByVal cnnSql As SqlConnection, ByVal obeProducto As beProductoAlquiler, _
                              Optional ByVal trSql As SqlTransaction = Nothing) As Boolean
        blnValido = False
        strError = String.Empty
        Try
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand
            If trSql IsNot Nothing Then
                cmdSql.Transaction = trSql
            End If
            cmdSql.CommandText = "uspServicioProductoAlquilerInsertar"
            cmdSql.CommandType = CommandType.StoredProcedure
            With obeProducto
                cmdSql.Parameters.Add(uData.CreaParametro("@IdProducto", .IdProducto, SqlDbType.Int, ParameterDirection.InputOutput))
                cmdSql.Parameters.Add(uData.CreaParametro("@CodTipoAlquiler ", SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@DesTipoAlquiler", .DesTipoAlquiler, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@CodTipoPago", .CodTipoPago, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@DesTipoPago", .DesTipoPago, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@CodTipoFacturacion", .CodTipoFacturacion, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@DesTipoFacturacion", .DesTipoFacturacion, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@CodMesAlquilar", .CodMesAlquilar, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@DesMesAlquilar", .DesMesAlquilar, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@AnioFabricacion", .AnioFabricacion, SqlDbType.VarChar, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@Garantia", .Garantia, SqlDbType.VarChar, 50))
                cmdSql.Parameters.Add(uData.CreaParametro("@Condicion", .Condicion, SqlDbType.VarChar, 50))
                cmdSql.Parameters.Add(uData.CreaParametro("@PlazoEntrega", .PlazoEntrega, SqlDbType.VarChar, 50))
                cmdSql.Parameters.Add(uData.CreaParametro("@Orden", .Orden, SqlDbType.VarChar, 50))
                cmdSql.Parameters.Add(uData.CreaParametro("@Serie", .Serie, SqlDbType.VarChar, 50))
                cmdSql.Parameters.Add(uData.CreaParametro("@Horas", .Horas, SqlDbType.VarChar, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@CodClasificacion", .CodClasificacion, SqlDbType.VarChar, 50))

                cmdSql.Parameters.Add(uData.CreaParametro("@CodigoTipoArrendamiento", .CodigoTipoArrendamiento, SqlDbType.VarChar, 5))
                cmdSql.Parameters.Add(uData.CreaParametro("@DescripcionTipoArrendamiento", .DescripcionTipoArrendamiento, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@CodigoTipoAlquiler", .CodigoTipoAlquiler, SqlDbType.VarChar, 5))
                cmdSql.Parameters.Add(uData.CreaParametro("@DescripcionTipoAlquiler", .DescripcionTipoAlquiler, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@FechaInicioArrendamiento", .FechaInicioArrendamiento, SqlDbType.VarChar, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@NroMaquinasMeses", .NroMaquinasMeses, SqlDbType.VarChar, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@ValorAdicionalHora", .ValorAdicionalHora, SqlDbType.VarChar, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@HorasUsoMensual", .HorasUsoMensual, SqlDbType.VarChar, 10))
                'campos adicionales para leasing
                cmdSql.Parameters.Add(uData.CreaParametro("@LeasingValorMensual", .LeasingValorMensual, SqlDbType.VarChar, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@LeasingMeses", .LeasingMeses, SqlDbType.VarChar, 10))

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
