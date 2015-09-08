<Serializable()>
Public Class beProducto

    Public Property IdProducto() As String
    Public Property IdCotizacion() As String
    Public Property IdCotizacionSap() As String
    Public Property IdPosicion() As String
    Public Property IdProductoSap() As String
    Public Property TipoProducto() As String
    Public Property Descripcion() As String
    Public Property ValorUnitario() As String
    Public Property IdMonedaValorUnitario() As String
    Public Property Cantidad() As String
    Public Property Unidad() As String
    Public Property ValorNeto() As String
    Public Property IdMonedaValorNeto() As String
    Public Property CostoProducto As String
    Public Property MonedaCostoProducto As String
    Public Property MonedaCotizacion As String
    Public Property ValorLista As String
    Public Property ValorReal As String
    Public Property PorcDescuento As String
    Public Property DescuentoImp As String
    Public Property Flete As String
    Public Property ValorVenta As String
    Public Property PorcImpuesto As String
    Public Property ValorImpuesto As String
    Public Property PrecioVentaFinal As String
    Public Property IdEstado() As String
    Public Property NombreEstado() As String
    Public Property CodigoLinea() As String
    Public Property NombreLinea() As String
    Public Property CodigoFamilia() As String
    Public Property FlatModificadoMemoria() As Boolean = False ' para indicar si se modifico en memoria
    Public Property ProductoCSA() As New beProductoCSA
    Public Property beProductoPrime As New beProductoPrime
    Public Property beProductoAlquiler() As New beProductoAlquiler
    Public Property beProductoSolucionCombinada() As New beProductoSolucionCombinada

    Public Property IdTarifaRS() As String = 0

    '12/06 CS
    Public Property LugarEntrega() As String
End Class
