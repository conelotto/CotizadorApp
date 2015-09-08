<Serializable()>
Public Class beAprobador
     
    Public Property IdAprobador() As String 
    Public Property IdCorporacion() As String 
    Public Property IdCompañia() As String 
    Public Property Aprobador() As String 
    Public Property Estado() As String 
    Public Property UsuarioCreacion() As String 
    Public Property FechaCreacion() As String 
    Public Property UsuarioModificacion() As String 
    Public Property FechaModificacion() As String 
    Public Sub New()
        IdAprobador = String.Empty
        IdCorporacion = String.Empty
        IdCompañia = String.Empty
        Aprobador = String.Empty
        Estado = String.Empty
        UsuarioCreacion = String.Empty
        FechaCreacion = String.Empty
        UsuarioModificacion = String.Empty
        FechaModificacion = String.Empty
    End Sub

End Class