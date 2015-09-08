<Serializable()>
Public Class beProducto

    Public Property IdPosicion() As String
    Public Property IdProducto() As String
    Public Property TipoProducto() As String 
    Public Property Descripcion() As String 
    Public Property DescripcionModelo() As String 
    Public Property ValorUnitario() As String 
    Public Property IdMonedaValorUnitario() As String 
    Public Property Cantidad() As String 
    Public Property Unidad() As String 
    Public Property ValorNeto() As String 
    Public Property IdMonedaValorNeto() As String 
    Public Property CostoProducto() As String 
    Public Property MonedaCostoProducto() As String 
    Public Property MonedaCotizacion() As String 
    Public Property ValorLista() As String 
    Public Property ValorReal() As String 
    Public Property PorcDescuento() As String 
    Public Property DescuentoImp() As String 
    Public Property Flete() As String 
    Public Property ValorVenta() As String 
    Public Property PorcImpuesto() As String 
    Public Property ValorImpuesto() As String 
    Public Property PrecioVentaFinal() As String 
    Public Property IdEstado() As String 
    Public Property NombreEstado() As String 
    Public Property IdMarca() As String 
    Public Property NombreMarca() As String 
    Public Property DescripGarantia() As String 
    Public Property Otros() As String 
    Public Property CodigoFamilia() As String 
    Public Property DescripcionFamilia() As String
    Public Property CodigoFormaPago() As String
    Public Property DescripFormaPago() As String
    Public Property CodigoLinea() As String
    Public Property NombreLinea() As String
    '12/06
    Public Property LugarEntrega() As String
    '--
    '28/08
    Public Property DisponibilidadMaquina() As String
    '--
    '03/09
    Public Property CodigoDisponibilidadMaquina() As String
    '--
    Public Property ProductoCSA() As beProductoCSA = New beProductoCSA
    Public Property ProductoPrime() As beProductoPrime = New beProductoPrime
    Public Property beProductoAlquiler() As beProductoAlquiler = New beProductoAlquiler
    Public Property beProductoSolucionCombinada() As beProductoSolucionCombinada = New beProductoSolucionCombinada
    Public Property ListaProductoCaracteristica() As List(Of beProductoCaracteristica) = New List(Of beProductoCaracteristica)
    Public Property ListaProductoDetallePrecios() As List(Of beProductoDetallePrecios) = New List(Of beProductoDetallePrecios)
     
End Class
