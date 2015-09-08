Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras

Public Class daProductoCSA

    Private uData As New Utiles.Datos
    Private uConfig As New Utiles.uConfiguracion
    Private Command As SqlCommand = Nothing
    
    Private _IdProductoCSA As Integer
    Public Property IdProductoCSA() As Integer
        Get
            Return _IdProductoCSA
        End Get
        Set(ByVal value As Integer)
            _IdProductoCSA = value
        End Set
    End Property

    Public Sub InsertarProductoCSA(ByVal Connection As SqlConnection, ByVal Transaction As SqlTransaction, _
                                   ByVal IdProducto As Integer, ByVal Usuario As String, _
                                   ByVal ProductoCSA As beProductoCSA)

        Command = Connection.CreateCommand
        Command.Transaction = Transaction
        Command.CommandText = "uspServicioProductoCSAInsertar"
        Command.CommandType = CommandType.StoredProcedure

        With Command.Parameters
            .Add(uData.CreaParametro("@IdProducto", IdProducto, SqlDbType.Int, 15))
            .Add(uData.CreaParametro("@ClaseCsa", ProductoCSA.ClaseCsa, SqlDbType.Char, 1))
            .Add(uData.CreaParametro("@TipoCotizacion", ProductoCSA.TipoCotizacion, SqlDbType.Char, 1))
            .Add(uData.CreaParametro("@IdTipoCSA", ProductoCSA.IdTipoCSA, SqlDbType.VarChar, 100))
            .Add(uData.CreaParametro("@IdPlan", ProductoCSA.IdPlan, SqlDbType.VarChar, 100))
            .Add(uData.CreaParametro("@DescripcionPlan", ProductoCSA.DescripcionPlan, SqlDbType.VarChar, 100))
            .Add(uData.CreaParametro("@IdUnidadDuracion", ProductoCSA.IdUnidadDuracion, SqlDbType.VarChar, 10))
            .Add(uData.CreaParametro("@Duracion", uConfig.fc_ConvertirEntero(ProductoCSA.Duracion), SqlDbType.Int, 15))
            .Add(uData.CreaParametro("@Tiempo", ProductoCSA.Tiempo, SqlDbType.VarChar, 20))
            .Add(uData.CreaParametro("@IdUnidadPlazoRenovacion", ProductoCSA.IdUnidadPlazoRenovacion, SqlDbType.VarChar, 10))
            .Add(uData.CreaParametro("@PlazoRenovacion", uConfig.fc_ConvertirEntero(ProductoCSA.PlazoRenovacion), SqlDbType.Int, 15))
            .Add(uData.CreaParametro("@IncluyeFluidos", uConfig.fc_ConvertirBoolean(ProductoCSA.IncluyeFluidos), SqlDbType.Bit, 10))
            .Add(uData.CreaParametro("@IncluyeDetallePartes", uConfig.fc_ConvertirBoolean(ProductoCSA.IncluyeDetallePartes), SqlDbType.Bit, 10))
            .Add(uData.CreaParametro("@FechaInicioContrato", uConfig.fc_ConvertirFecha(ProductoCSA.FechaInicioContrato), SqlDbType.Date, 10))
            .Add(uData.CreaParametro("@FechaEstimadaCierre", uConfig.fc_ConvertirFecha(ProductoCSA.FechaEstimadaCierre), SqlDbType.Date, 10))
            .Add(uData.CreaParametro("@ParticipacionVendedor1", uConfig.fc_ConvertirEntero(ProductoCSA.ParticipacionVendedor1), SqlDbType.Int, 15))
            .Add(uData.CreaParametro("@ParticipacionVendedor2", uConfig.fc_ConvertirEntero(ProductoCSA.ParticipacionVendedor2), SqlDbType.Int, 15))
            .Add(uData.CreaParametro("@Usuario", Usuario, SqlDbType.VarChar, 15))
        End With

        Command.ExecuteNonQuery()

    End Sub



End Class
