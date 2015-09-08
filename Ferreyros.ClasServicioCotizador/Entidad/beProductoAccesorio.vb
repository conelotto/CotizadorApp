<Serializable()>
Public Class beProductoAccesorio

    Public Property IdProductoAccesorio() As String
    Public Property IdProducto() As String
    Public Property IdAccesorio() As String
    Public Property CodigoProductoAccesorio() As String
    Public Property NombreProductoAccesorio() As String
    Public Property Cantidad() As String
    Public Property UnidadMedida() As String
    Public Property ValorLista() As String = String.Empty
    Public Property MonedaValorLista() As String = String.Empty
    Public Property FlatMostrarEspTecnica() As String = "0"
    Public Property FlatEliminado() As String = String.Empty
    Public Property UsuarioCreacion() As String = String.Empty

End Class
