Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras

Public Class daProducto

    Private uConfig As New Utiles.uConfiguracion
    Private uData As New Utiles.Datos
    Private Command As SqlCommand = Nothing
    Private _IdProducto As Integer = 0
    Public Property IdProducto() As Integer
        Get
            Return _IdProducto
        End Get
        Set(ByVal value As Integer)
            _IdProducto = value
        End Set
    End Property

    Public Sub InsertarProducto(ByVal Connection As SqlConnection, ByVal Transaction As SqlTransaction, _
                                ByVal IdCotizacion As Integer, ByVal Usuario As String, _
                                ByVal Producto As beProducto)

        Command = Connection.CreateCommand
        Command.Transaction = Transaction
        Command.CommandText = "uspServicioProductoInsertar"
        Command.CommandType = CommandType.StoredProcedure

        With Command.Parameters
            .Add(uData.CreaParametro("@IdCotizacion", IdCotizacion, SqlDbType.Int, 15))
            .Add(uData.CreaParametro("@IdPosicionSAP", Producto.IdPosicion, SqlDbType.VarChar, 20))
            .Add(uData.CreaParametro("@IdProductoSAP", Producto.IdProducto, SqlDbType.VarChar, 20))
            .Add(uData.CreaParametro("@TipoProducto", Producto.TipoProducto, SqlDbType.VarChar, 10))
            .Add(uData.CreaParametro("@Descripcion", Producto.Descripcion, SqlDbType.VarChar, 100))

            Dim dblValorUnitario As Double = 0
            Try
                Select Case Producto.TipoProducto
                    Case clsGeneral.TipoProducto.PRIME
                        dblValorUnitario = uConfig.fc_ConvertirDouble(Producto.ValorLista) / uConfig.fc_ConvertirEntero(Producto.Cantidad)
                        Exit Select
                    Case clsGeneral.TipoProducto.CSA
                        If uConfig.fc_ConvertirDouble(Producto.ValorLista) > uConfig.fc_ConvertirDouble(Producto.ValorReal) Then
                            dblValorUnitario = uConfig.fc_ConvertirDouble(Producto.ValorLista)
                        Else
                            dblValorUnitario = uConfig.fc_ConvertirDouble(Producto.ValorReal)
                        End If
                        Exit Select
                    Case clsGeneral.TipoProducto.ACCESORIO
                        dblValorUnitario = uConfig.fc_ConvertirDouble(Producto.ValorLista) / uConfig.fc_ConvertirEntero(Producto.Cantidad)
                        Exit Select
                    Case clsGeneral.TipoProducto.ALQUILER
                        dblValorUnitario = uConfig.fc_ConvertirDouble(Producto.ValorLista) / uConfig.fc_ConvertirEntero(Producto.Cantidad)
                        Exit Select
                End Select
            Catch ex As Exception

            End Try


            .Add(uData.CreaParametro("@ValorUnitario", dblValorUnitario, SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@IdMonedaValorUnitario", Producto.IdMonedaValorUnitario, SqlDbType.VarChar, 10))
            .Add(uData.CreaParametro("@Cantidad", uConfig.fc_ConvertirEntero(Producto.Cantidad), SqlDbType.Int, 15))
            .Add(uData.CreaParametro("@Unidad", Producto.Unidad, SqlDbType.VarChar, 10))

            'Cambiado para corregir envio desde sap
            '.Add(uData.CreaParametro("@ValorNeto", uConfig.fc_ConvertirDouble(Producto.ValorNeto), SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@ValorNeto", uConfig.fc_ConvertirDouble(Producto.ValorVenta), SqlDbType.Float, 15))

            .Add(uData.CreaParametro("@IdMonedaValorNeto", Producto.IdMonedaValorNeto, SqlDbType.VarChar, 10))

            .Add(uData.CreaParametro("@CostoProducto", uConfig.fc_ConvertirDouble(Producto.CostoProducto), SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@MonedaCostoProducto", Producto.MonedaCostoProducto, SqlDbType.VarChar, 50))
            .Add(uData.CreaParametro("@MonedaCotizacion", Producto.MonedaCotizacion, SqlDbType.VarChar, 50))
            .Add(uData.CreaParametro("@ValorLista", uConfig.fc_ConvertirDouble(Producto.ValorLista), SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@ValorReal", uConfig.fc_ConvertirDouble(Producto.ValorReal), SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@PorcDescuento", uConfig.fc_ConvertirDouble(Producto.PorcDescuento), SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@DescuentoImp", uConfig.fc_ConvertirDouble(Producto.DescuentoImp), SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@Flete", uConfig.fc_ConvertirDouble(Producto.Flete), SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@ValorVenta", uConfig.fc_ConvertirDouble(Producto.ValorVenta), SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@PorcImpuesto", uConfig.fc_ConvertirDouble(Producto.PorcImpuesto), SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@ValorImpuesto", uConfig.fc_ConvertirDouble(Producto.ValorImpuesto), SqlDbType.Float, 15))
            .Add(uData.CreaParametro("@PrecioVentaFinal", uConfig.fc_ConvertirDouble(Producto.PrecioVentaFinal), SqlDbType.Float, 15))

            .Add(uData.CreaParametro("@IdEstado", Producto.IdEstado, SqlDbType.VarChar, 2))
            .Add(uData.CreaParametro("@NombreEstado", Producto.NombreEstado, SqlDbType.VarChar, 50))

            .Add(uData.CreaParametro("@DescripcionModelo", Producto.DescripcionModelo, SqlDbType.VarChar, 150))
            .Add(uData.CreaParametro("@IdMarca", Producto.IdMarca, SqlDbType.VarChar, 15))
            .Add(uData.CreaParametro("@NombreMarca", Producto.NombreMarca, SqlDbType.VarChar, 150))
            .Add(uData.CreaParametro("@Otros", Producto.Otros, SqlDbType.VarChar, 300))
            .Add(uData.CreaParametro("@DescripGarantia", Producto.DescripGarantia, SqlDbType.VarChar, 100))

            .Add(uData.CreaParametro("@CodigoFamilia", Producto.CodigoFamilia, SqlDbType.VarChar, 20))
            .Add(uData.CreaParametro("@DescripcionFamilia", Producto.DescripcionFamilia, SqlDbType.VarChar, 100))

            .Add(uData.CreaParametro("@CodigoFormaPago", Producto.CodigoFormaPago, SqlDbType.VarChar, 5))
            .Add(uData.CreaParametro("@DescripcionFormaPago", Producto.DescripFormaPago, SqlDbType.VarChar, 200))

            .Add(uData.CreaParametro("@CodigoLinea", Producto.CodigoLinea, SqlDbType.VarChar, 20))
            .Add(uData.CreaParametro("@NombreLinea", Producto.DescripcionFamilia, SqlDbType.VarChar, 100))

            .Add(uData.CreaParametro("@Usuario", Usuario, SqlDbType.VarChar, 15))
            '12/06 CS
            .Add(uData.CreaParametro("@LugarEntrega", Producto.LugarEntrega, SqlDbType.VarChar, 200))
            '28/08
            .Add(uData.CreaParametro("@DisponibilidadMaquina", Producto.DisponibilidadMaquina, SqlDbType.VarChar, 50))
            '03/09
            .Add(uData.CreaParametro("@CodigoDisponibilidadMaquina", Producto.CodigoDisponibilidadMaquina, SqlDbType.Char, 2))

            .Add(uData.CreaParametro("@IdProducto", Nothing, SqlDbType.Int, 15, ParameterDirection.Output))
        End With

        Command.ExecuteNonQuery()
        IdProducto = Command.Parameters("@IdProducto").Value

        If Not (IdProducto > 0) Then
            Throw New Exception("No se pudo insertar el Producto")
        End If

    End Sub

End Class
