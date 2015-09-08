Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador

Public Class bcProductoAccesorio
    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function BuscarIdProducto(ByVal strConexion As String, ByVal obeProductoAccesorio As beProductoAccesorio, ByRef ListaProductoAccesorio As List(Of beProductoAccesorio)) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaProductoAccesorio As New daProductoAccesorio
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaProductoAccesorio.BuscarIdProducto(CnnSql, obeProductoAccesorio, ListaProductoAccesorio) Then
                    Throw New Exception(odaProductoAccesorio.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function

End Class
