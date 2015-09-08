Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador

Public Class bcProductoAdicional
    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function BuscarIdProducto(ByVal strConexion As String, ByVal obeProductoAdicional As beProductoAdicional, ByRef ListaProductoAdicional As List(Of beProductoAdicional)) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaProductoAdicional As New daProductoAdicional
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaProductoAdicional.BuscarIdProducto(CnnSql, obeProductoAdicional, ListaProductoAdicional) Then
                    Throw New Exception(odaProductoAdicional.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function

End Class
