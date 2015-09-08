Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador

Public Class bcLlave
    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private p_strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return p_strError
        End Get
    End Property
    Public Function BuscarCodigoLinea(ByVal strConexion As String, ByVal idProducto As String, ByVal obeLinea As beLinea, ByRef ListabeLlave As List(Of beLlave)) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaLlave As New daLlave
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaLlave.BuscarCodigoLinea(CnnSql, idProducto, obeLinea, ListabeLlave) Then
                    Throw New Exception(odaLlave.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString
        End Try
        Return bolExito
    End Function
End Class
