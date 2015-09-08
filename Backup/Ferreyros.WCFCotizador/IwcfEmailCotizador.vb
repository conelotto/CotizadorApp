Imports Ferreyros.ClasServicioCotizador
Imports System.ServiceModel

' NOTA: puede usar el comando "Cambiar nombre" del menú contextual para cambiar el nombre de interfaz "IwcfEmailCotizador" en el código y en el archivo de configuración a la vez.
<ServiceContract()>
Public Interface IwcfEmailCotizador

    <OperationContract()>
    Function EnviarEmail(ByVal beEmail As beEmail, ByRef ErrorDescripcion As String) As Boolean

End Interface
