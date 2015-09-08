Imports Ferreyros.ClasServicioCotizador
Imports System.ServiceModel

' NOTE: You can use the "Rename" command on the context menu to change the interface name "IwcfCotizador" in both code and config file together.

<ServiceContract()>
Public Interface IwcfCotizador
    <OperationContract()>
    Function InsertarCotizacion(ByVal oclsDataCotizacion As beCotizacion, ByRef UrlResult As String, ByRef ErrorDescripcion As String) As Boolean

End Interface



