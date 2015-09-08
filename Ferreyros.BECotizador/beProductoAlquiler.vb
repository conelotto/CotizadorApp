<Serializable()>
Public Class beProductoAlquiler

    Public Property IdProducto() As String = String.Empty
    Public Property CodTipoAlquiler() As String = String.Empty
    Public Property DesTipoAlquiler() As String = String.Empty
    Public Property CodTipoPago() As String = String.Empty
    Public Property DesTipoPago() As String = String.Empty
    Public Property CodTipoFacturacion() As String = String.Empty
    Public Property DesTipoFacturacion() As String = String.Empty
    Public Property CodMesAlquilar() As String = String.Empty
    Public Property DesMesAlquilar() As String = String.Empty
    Public Property AnioFabricacion() As String = String.Empty
    Public Property Garantia() As String = String.Empty
    Public Property Condicion() As String = String.Empty
    Public Property PlazoEntrega() As String = String.Empty
    Public Property Orden() As String = String.Empty
    Public Property Serie() As String = String.Empty
    Public Property Horas() As String = String.Empty
    Public Property LeasingValorMensual() As String = String.Empty
    Public Property LeasingMeses() As String = String.Empty
    Public Property CodClasificacion() As String = String.Empty
    Public Property UsuarioCreacion() As String = String.Empty

    Public Property ListabeProductoAdicional() As New List(Of beProductoAdicional)
    Public Property ListabeProductoAccesorio() As New List(Of beProductoAccesorio)
End Class
