<Serializable()>
Public Class beValidacion

    Private _respuesta As Integer
    Public Property respuesta() As Integer
        Get
            Return _respuesta
        End Get
        Set(ByVal value As Integer)
            _respuesta = value
        End Set
    End Property

    Private _validacion As Boolean
    Public Property validacion() As Boolean
        Get
            Return _validacion
        End Get
        Set(ByVal value As Boolean)
            _validacion = value
        End Set
    End Property

    Private _mensaje As String
    Public Property mensaje() As String
        Get
            Return _mensaje
        End Get
        Set(ByVal value As String)
            _mensaje = value
        End Set
    End Property

    Private _usuario As String
    Public Property usuario() As String
        Get
            Return _usuario
        End Get
        Set(ByVal value As String)
            _usuario = value
        End Set
    End Property

    Private _flag As String
    Public Property flag() As String
        Get
            Return _flag
        End Get
        Set(ByVal value As String)
            _flag = value
        End Set
    End Property

    Private _pageSize As String
    Public Property pageSize() As String
        Get
            Return _pageSize
        End Get
        Set(ByVal value As String)
            _pageSize = value
        End Set
    End Property

    Private _currentPage As String
    Public Property currentPage() As String
        Get
            Return _currentPage
        End Get
        Set(ByVal value As String)
            _currentPage = value
        End Set
    End Property

    Private _sortColumn As String
    Public Property sortColumn() As String
        Get
            Return _sortColumn
        End Get
        Set(ByVal value As String)
            _sortColumn = value
        End Set
    End Property

    Private _sortOrder As String
    Public Property sortOrder() As String
        Get
            Return _sortOrder
        End Get
        Set(ByVal value As String)
            _sortOrder = value
        End Set
    End Property

    Private _recordCount As String
    Public Property recordCount() As String
        Get
            Return _recordCount
        End Get
        Set(ByVal value As String)
            _recordCount = value
        End Set
    End Property

    Private _pageCount As String
    Public Property pageCount() As String
        Get
            Return _pageCount
        End Get
        Set(ByVal value As String)
            _pageCount = value
        End Set
    End Property

    Private _cadenaAleatoria As String
    Public Property cadenaAleatoria() As String
        Get
            Return _cadenaAleatoria
        End Get
        Set(ByVal value As String)
            _cadenaAleatoria = value
        End Set
    End Property

    Private _cadenaLog4Net As String
    Public Property cadenaLog4Net() As String
        Get
            Return _cadenaLog4Net
        End Get
        Set(ByVal value As String)
            _cadenaLog4Net = value
        End Set
    End Property

    Public Sub New()
        _cadenaLog4Net = String.Empty
        _cadenaAleatoria = String.Empty
        _usuario = String.Empty
        _mensaje = String.Empty
        _validacion = False
        _respuesta = -1
        _flag = String.Empty
        _pageSize = String.Empty
        _currentPage = String.Empty
        _sortOrder = String.Empty
        _sortColumn = String.Empty
    End Sub

End Class
