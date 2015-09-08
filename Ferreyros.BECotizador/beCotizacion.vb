<Serializable()>
Public Class beCotizacion
     
    Public Property IdCotizacion() As String = String.Empty
    Public Property IdCotizacionSap() As String = String.Empty
    Public Property IdCorporacion() As String = String.Empty
    Public Property IdCompania() As String = String.Empty
    Public Property IdSolicitante() As String = String.Empty
    Public Property DescripSolicitante() As String = String.Empty
    Public Property CodigoMercadoSolicitante() As String = String.Empty
    Public Property RUCSolicitante() As String = String.Empty
    Public Property DNISolicitante() As String = String.Empty
    Public Property IdPersonaResponsable() As String = String.Empty
    Public Property DescripResponsable() As String = String.Empty
    Public Property OficinaResponsable() As String = String.Empty
    Public Property CargoResponsable() As String = String.Empty
    Public Property EmailResponsable() As String = String.Empty
    Public Property TelefonoResponsable() As String = String.Empty
    Public Property AnexoTelefonoResponsable() As String = String.Empty
    Public Property FechaInicioValidez() As String = String.Empty
    Public Property FechaFinalValidez() As String = String.Empty
    Public Property FechaPrecio() As String = String.Empty
    Public Property FechaEstimadaFacturacion() As String = String.Empty
    Public Property NumeroOportunidad() As String = String.Empty
    Public Property ItemOportunidad() As String = String.Empty
    Public Property Version() As String = String.Empty
    Public Property ValorTipoCambio() As String = String.Empty
    Public Property MonedaValorNeto() As String = String.Empty
    Public Property MonedaValorImpuesto() As String = String.Empty
    Public Property MonedaValorBruto() As String = String.Empty
    Public Property ValorNeto() As String = String.Empty
    Public Property ValorImpuesto() As String = String.Empty
    Public Property ValorBruto() As String = String.Empty
    Public Property NombreEstado() As String = String.Empty
    Public Property FlatEliminado() As String = String.Empty
    Public Property UsuarioCreacion() As String = String.Empty
    Public Property FechaCreacion() As String = String.Empty
    Public Property UsuarioModificacion() As String = String.Empty
    Public Property FechaModificacion() As String = String.Empty
    Public Property RolUsuario() As String = String.Empty
     
    Public Property ListaProducto() As New List(Of beProducto)
    'Public Property ListabeProductoPrime() As New List(Of beProductoPrime)
    Public Property ListabeCotizacionContacto() As New List(Of beCotizacionContacto)
    Public Property ListaSolucionCombinada() As New List(Of beSolucionCombinada)

End Class
