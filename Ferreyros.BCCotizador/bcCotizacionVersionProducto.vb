Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador
Imports System.Data.SqlClient
Public Class bcCotizacionVersionProducto
    Private dProducto As daProducto = Nothing
    Private p_strError As String

    Public ReadOnly Property ErrorDes() As String
        Get
            Return p_strError
        End Get
    End Property
    Public Function BuscarIdCotizacionVersion(ByVal strConexion As String, ByVal obeCotizacionVersionProducto As beCotizacionVersionProducto, ByRef ListabeCotizacionVersionProducto As List(Of beCotizacionVersionProducto)) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaCotizacionVersionProducto As New daCotizacionVersionProducto
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaCotizacionVersionProducto.BuscarIdCotizacionVersion(CnnSql, obeCotizacionVersionProducto, ListabeCotizacionVersionProducto) Then
                    Throw New Exception(odaCotizacionVersionProducto.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString
        End Try
        Return bolExito
    End Function
End Class
