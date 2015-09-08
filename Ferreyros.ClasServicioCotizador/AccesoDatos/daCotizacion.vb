Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras

Public Class daCotizacion

    Private uData As New Utiles.Datos
    Private uConfig As New Utiles.uConfiguracion
    Private Command As SqlCommand = Nothing
    Private _IdCotizacion As Integer
    Public Property IdCotizacion() As Integer
        Get
            Return _IdCotizacion
        End Get
        Set(ByVal value As Integer)
            _IdCotizacion = value
        End Set
    End Property
    Private _Usuario As String
    Public Property Usuario() As String
        Get
            Return _Usuario
        End Get
        Set(ByVal value As String)
            _Usuario = value
        End Set
    End Property

    Public Sub InsertarCotizacion(ByVal Connection As SqlConnection, ByVal Transaction As SqlTransaction, ByVal Cotizacion As beCotizacion)

        Command = Connection.CreateCommand
        Command.Transaction = Transaction
        Command.CommandText = "uspServicioCotizacionInsertar"
        Command.CommandType = CommandType.StoredProcedure

        With Command.Parameters
            .Add(uData.CreaParametro("@IdCotizacionSAP", Cotizacion.IdCotizacion, SqlDbType.VarChar, 20))
            .Add(uData.CreaParametro("@IdCorporacion", Cotizacion.IdCorporacion, SqlDbType.VarChar, 15))
            .Add(uData.CreaParametro("@IdCompania", Cotizacion.IdCompanhia, SqlDbType.VarChar, 15))
            .Add(uData.CreaParametro("@IdSolicitante", Cotizacion.IdSolicitante, SqlDbType.VarChar, 15))
            .Add(uData.CreaParametro("@DescripSolicitante", Cotizacion.DescripSolicitante, SqlDbType.VarChar, 150))
            .Add(uData.CreaParametro("@CodigoMercadoSolicitante", Cotizacion.CodigoMercadoSolicitante, SqlDbType.VarChar, 10))
            .Add(uData.CreaParametro("@RUCSolicitante", Cotizacion.RUCSolicitante, SqlDbType.VarChar, 15))
            .Add(uData.CreaParametro("@DNISolicitante", Cotizacion.DNISolicitante, SqlDbType.VarChar, 15))
            .Add(uData.CreaParametro("@IdPersonaResponsable", Cotizacion.IdPersonaResponsable, SqlDbType.VarChar, 15))
            .Add(uData.CreaParametro("@DescripResponsable", Cotizacion.DescripResponsable, SqlDbType.VarChar, 150))
            .Add(uData.CreaParametro("@OficinaResponsable", Cotizacion.OficinaResponsable, SqlDbType.VarChar, 150))
            .Add(uData.CreaParametro("@CargoResponsable", Cotizacion.CargoResponsable, SqlDbType.VarChar, 150))
            .Add(uData.CreaParametro("@EmailResponsable", Cotizacion.EmailResponsable, SqlDbType.VarChar, 150))
            .Add(uData.CreaParametro("@TelefonoResponsable", Cotizacion.TelefonoResponsable, SqlDbType.VarChar, 50))
            .Add(uData.CreaParametro("@AnexoTelefonoResponsable", Cotizacion.AnexoTelefonoResponsable, SqlDbType.VarChar, 30))
            .Add(uData.CreaParametro("@FechaInicioValidez", uConfig.fc_ConvertirFecha(Cotizacion.FechaInicioValidez), SqlDbType.Date, 10))
            .Add(uData.CreaParametro("@FechaFinalValidez", uConfig.fc_ConvertirFecha(Cotizacion.FechaFinalValidez), SqlDbType.Date, 10))
            .Add(uData.CreaParametro("@FechaPrecio", uConfig.fc_ConvertirFecha(Cotizacion.FechaPrecio), SqlDbType.Date, 10))
            .Add(uData.CreaParametro("@FechaEstimadaFacturacion", uConfig.fc_ConvertirFecha(Cotizacion.FechaEstimadaFacturacion), SqlDbType.Date, 10))
            .Add(uData.CreaParametro("@NumeroOportunidad", Cotizacion.NumeroOportunidad, SqlDbType.VarChar, 20))
            .Add(uData.CreaParametro("@ItemOportunidad", Cotizacion.ItemOportunidad, SqlDbType.VarChar, 20))
            .Add(uData.CreaParametro("@Version", uConfig.fc_ConvertirEntero(Cotizacion.Version), SqlDbType.Int, 15))

            ''Cambiado para corregir envio desde sap(aveces envia el valor en Negativo)
            'Dim dblvalorTipoCambio As Double = 0
            'dblvalorTipoCambio = uConfig.fc_ConvertirDouble(Cotizacion.ValorTipoCambio)
            'If dblvalorTipoCambio < 0 Then
            '    dblvalorTipoCambio = (-1) * dblvalorTipoCambio
            'End If

            .Add(uData.CreaParametro("@ValorTipoCambio", uConfig.fc_ConvertirDouble(Cotizacion.ValorTipoCambio), SqlDbType.Float, 15))

            .Add(uData.CreaParametro("@MonedaValorNeto", Cotizacion.MonedaValorNeto, SqlDbType.VarChar, 10))
            .Add(uData.CreaParametro("@MonedaValorImpuesto", Cotizacion.MonedaValorImpuesto, SqlDbType.VarChar, 10))
            .Add(uData.CreaParametro("@MonedaValorBruto", Cotizacion.MonedaValorBruto, SqlDbType.VarChar, 10))
             
            'Cambiado para corregir envio desde sap
            '.Add(uData.CreaParametro("@ValorNeto", uConfig.fc_ConvertirDouble(Cotizacion.ValorNeto), SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@ValorNeto", uConfig.fc_ConvertirDouble(Cotizacion.TotalValorVenta), SqlDbType.Float, 15))

            'ValorNeto = ValorBruto - ValorImpuesto
            .Add(uData.CreaParametro("@ValorImpuesto", uConfig.fc_ConvertirDouble(Cotizacion.ValorImpuesto), SqlDbType.Float, 15))

            'Cambiado para corregir envio desde sa
            '.Add(uData.CreaParametro("@ValorBruto", uConfig.fc_ConvertirDouble(Cotizacion.ValorBruto), SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@ValorBruto", uConfig.fc_ConvertirDouble(Cotizacion.TotalPrecioVentaFinal), SqlDbType.Float, 15))

            .Add(uData.CreaParametro("@NombreEstado", Cotizacion.NombreEstado, SqlDbType.VarChar, 50))
            .Add(uData.CreaParametro("@TotalValorLista", uConfig.fc_ConvertirDouble(Cotizacion.TotalValorLista), SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@TotalValorReal", uConfig.fc_ConvertirDouble(Cotizacion.TotalValorReal), SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@TotalDescuentoPorc", uConfig.fc_ConvertirDouble(Cotizacion.TotalDescuentoPorc), SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@TotalDescuentoImp", uConfig.fc_ConvertirDouble(Cotizacion.TotalDescuentoImp), SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@TotalFlete", uConfig.fc_ConvertirDouble(Cotizacion.TotalFlete), SqlDbType.Float, 15))

            .Add(uData.CreaParametro("@TotalValorVenta", uConfig.fc_ConvertirDouble(Cotizacion.TotalValorVenta), SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@TotalValorImpuesto", uConfig.fc_ConvertirDouble(Cotizacion.TotalValorImpuesto), SqlDbType.Float, 15))
             .Add(uData.CreaParametro("@TotalPrecioVentaFinal", uConfig.fc_ConvertirDouble(Cotizacion.TotalPrecioVentaFinal), SqlDbType.Float, 15))

            .Add(uData.CreaParametro("@IdResponsableServicio", Cotizacion.IdResponsableServicio, SqlDbType.VarChar, 15))
            .Add(uData.CreaParametro("@NombreResponsableServicio", Cotizacion.NombreResponsableServicio, SqlDbType.VarChar, 200))
            .Add(uData.CreaParametro("@IdSupervisorServicio", Cotizacion.IdSupervisorServicio, SqlDbType.VarChar, 15))
            .Add(uData.CreaParametro("@NombreSupervisorServicio", Cotizacion.NombreSupervisorServicio, SqlDbType.VarChar, 200))
            .Add(uData.CreaParametro("@Otros", Cotizacion.Otros, SqlDbType.VarChar, 300))

            .Add(uData.CreaParametro("@Usuario", Cotizacion.Usuario, SqlDbType.VarChar, 15))
            .Add(uData.CreaParametro("@RolUsuario", Cotizacion.RolUsuario, SqlDbType.VarChar, 50))

            .Add(uData.CreaParametro("@IdCotizacion", Nothing, SqlDbType.Int, 15, ParameterDirection.Output))
        End With

        Command.ExecuteNonQuery()
        IdCotizacion = Command.Parameters("@IdCotizacion").Value

        If Not (IdCotizacion > 0) Then
            Throw New Exception("No se pudo insertar la Cotización SAP")
        Else
            Usuario = Cotizacion.Usuario
        End If

    End Sub

End Class
