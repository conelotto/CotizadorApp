Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador

Public Class bcProductoAlquiler
    Private p_strError As String
    Public Function BuscarId(ByVal strConexion As String, ByRef obeProducto As beProducto) As Boolean
        Dim bolExito As Boolean = False 
        Try
            Dim odaProductoAlquiler As New daProductoAlquiler
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaProductoAlquiler.BuscarId(CnnSql, obeProducto) Then
                    Throw New Exception(odaProductoAlquiler.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function
End Class
