<Serializable()>
Public Class beValidacion

    Public Property respuesta() As Integer 
    Public Property validacion() As Boolean 
    Public Property mensaje() As String 
    Public Property usuario() As String 
    Public Property flag() As String 
    Public Property cadenaAleatoria() As String 
    Public Sub New()
        _usuario = String.Empty
        _mensaje = String.Empty
        _validacion = False
        _respuesta = -1
        _flag = String.Empty
        _cadenaAleatoria = String.Empty
    End Sub

End Class
