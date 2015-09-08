Public Class JQGridItem

    Private _id As Long
    Public Property ID() As Long
        Get
            Return _id
        End Get
        Set(ByVal value As Long)
            _id = value
        End Set
    End Property

    Private _row As List(Of String)
    Public Property Row() As List(Of String)
        Get
            Return _row
        End Get
        Set(ByVal value As List(Of String))
            _row = value
        End Set
    End Property

    Public Sub New(ByVal pId As Long, ByVal pRow As List(Of String))
        _id = pId
        _row = pRow
    End Sub

End Class
