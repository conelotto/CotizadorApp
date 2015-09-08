<Serializable()>
Public Class beMarcadorCotizacion

    Public Property IdMarcadorCotizacion() As String = String.Empty
    Public Property IdArchivoConfiguracion() As String = String.Empty
    Public Property NombreMarcadorCotizacion() As String = String.Empty
    Public Property NombreMarcador() As String = String.Empty
 
    Public Sub New()
        IdMarcadorCotizacion = ""
        IdArchivoConfiguracion = ""
        NombreMarcadorCotizacion = ""
        NombreMarcador = ""
    End Sub


End Class
