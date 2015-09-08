<Serializable()>
Public Class beQuery
    Private _codigo As String
    Private _descripcion As String
    Private _query As String
    Private _cadenaCampos As String
    Private _cadenaCondiciones As String
    Private _estado As String

    Public Property codigo() As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Public Property descripcion() As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
        End Set
    End Property

    Public Property cadenaCampos() As String
        Get
            Return _cadenaCampos
        End Get
        Set(ByVal value As String)
            _cadenaCampos = value
        End Set
    End Property

    Public Property cadenaCondiciones() As String
        Get
            Return _cadenaCondiciones
        End Get
        Set(ByVal value As String)
            _cadenaCondiciones = value
        End Set
    End Property

    Public Property query() As String
        Get
            Return _query
        End Get
        Set(ByVal value As String)
            _query = value
        End Set
    End Property

    Public Property estado() As String
        Get
            Return _estado
        End Get
        Set(ByVal value As String)
            _estado = value
        End Set
    End Property

    Public Sub New()
        _codigo = String.Empty
        _descripcion = String.Empty
        _query = String.Empty
        _cadenaCampos = String.Empty
        _cadenaCondiciones = String.Empty
        _estado = String.Empty
    End Sub
End Class
