Public Class EstructuraDatos
    Public _valor1 As String = String.Empty
    Public _valor2 As String = String.Empty
    Public _valor3 As String = String.Empty
    Public _valor4 As String = String.Empty
    Public _valor5 As String = String.Empty

    Public Property valor1() As String
        Get
            Return _valor1
        End Get
        Set(value As String)
            _valor1 = value
        End Set
    End Property

    Public Property valor2() As String
        Get
            Return _valor2
        End Get
        Set(value As String)
            _valor2 = value
        End Set
    End Property

    Public Property valor3() As String
        Get
            Return _valor3
        End Get
        Set(value As String)
            _valor3 = value
        End Set
    End Property

    Public Property valor4() As String
        Get
            Return _valor4
        End Get
        Set(value As String)
            _valor4 = value
        End Set
    End Property

    Public Property valor5() As String
        Get
            Return _valor5
        End Get
        Set(value As String)
            _valor5 = value
        End Set
    End Property

    Public Structure s_GridResult

        Dim page As Integer           ' Current page of data that is being viewed/requested
        Dim total As Integer          ' Total number of pages avaialble to view in the entire list
        Dim records As Integer         ' Number of records available in the rows[] array
        Dim rows() As s_RowData     ' Rows of data
    End Structure


    Public Structure s_RowData

        Dim id As Integer               ' ItemID value for the row
        Dim cell() As String        ' Array of strings that hold the field values for the given row
    End Structure

    Public Structure combo
        Dim id As Integer               ' ItemID value for the row
        Dim Descripcion As String        ' Array of strings that hold the field values for the given row
    End Structure

End Class
