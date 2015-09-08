<Serializable()>
Public Class beAprobadorUsuario
     
    Public Property IdAprobador() As String 
    Public Property MatriculaUsuario() As String 
    Public Property NombreUsuario() As String 
    Public Property CorreoUsuario() As String 
    Public Property Estado() As String 
    Public Property UsuarioCreacion() As String 
    Public Property FechaCreacion() As String 
    Public Property UsuarioModificacion() As String 
    Public Property FechaModificacion() As String 

    Public Sub New()
        IdAprobador = String.Empty
        MatriculaUsuario = String.Empty
        NombreUsuario = String.Empty
        CorreoUsuario = String.Empty
        Estado = String.Empty
        UsuarioCreacion = String.Empty
        FechaCreacion = String.Empty
        UsuarioModificacion = String.Empty
        FechaModificacion = String.Empty
    End Sub
End Class
