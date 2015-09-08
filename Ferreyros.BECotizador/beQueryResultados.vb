<Serializable()>
Public Class beQueryResultados
    Private _codigo As String
    Private _codQuery As String
    Private _campo As String

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

    Public Property campo() As String
        Get
            Return _campo
        End Get
        Set(ByVal value As String)
            _campo = value
        End Set
    End Property

    Public Sub New()
        _codigo = String.Empty
        _codQuery = String.Empty
        _campo = String.Empty
    End Sub
End Class
