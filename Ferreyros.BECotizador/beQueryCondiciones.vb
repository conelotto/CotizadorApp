<Serializable()>
Public Class beQueryCondiciones
    Private _codigo As String
    Private _codQuery As String
    Private _condiciones As String

    Public Property codigo() As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Public Property codQuery() As String
        Get
            Return _codQuery
        End Get
        Set(ByVal value As String)
            _codQuery = value
        End Set
    End Property

    Public Property condiciones() As String
        Get
            Return _condiciones
        End Get
        Set(ByVal value As String)
            _condiciones = value
        End Set
    End Property

    Public Sub New()
        _codigo = String.Empty
        _codQuery = String.Empty
        _condiciones = String.Empty
    End Sub
End Class
