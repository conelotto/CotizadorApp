Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros.DACotizador
Imports Ferreyros.BECotizador

Public Class bcUsuario

    Private pr_strError As String

    Public ReadOnly Property ErrorDes() As String
        Get
            Return pr_strError
        End Get
    End Property

    Public Function Buscar(ByVal strConexion As String, ByVal Compania As String, ByVal ValorBusqueda As String, _
                           ByRef dstAprobador As DataSet) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim oUsuario As New daUsuario
            Using cnnSql As New SqlConnection(strConexion)
                cnnSql.Open()
                If Not oUsuario.Buscar(cnnSql, Compania, ValorBusqueda, dstAprobador) Then
                    Throw New Exception(oUsuario.ErrorDes)
                End If
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function

    Public Function Buscar(ByVal strConexion As String, ByVal eAprobadorUsuario As beAprobadorUsuario, _
                           ByRef dtrAprobadorUsuario As DataRow) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim oUsuario As New daUsuario
            Using cnnSql As New SqlConnection(strConexion)
                cnnSql.Open()
                If Not oUsuario.Buscar(cnnSql, eAprobadorUsuario, dtrAprobadorUsuario) Then
                    Throw New Exception(oUsuario.ErrorDes)
                End If
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function
End Class
