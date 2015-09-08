Imports System.Data.SqlClient

Public Class bcHomologacion
    Private p_strError As String = String.Empty

    Public Function BuscarTabla(ByVal strConexion As String, ByVal obeHomologacion As beHomologacion, ByRef ListabeHomologacion As List(Of beHomologacion)) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaHomologacion As New daHomologacion
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaHomologacion.BuscarTabla(CnnSql, obeHomologacion, ListabeHomologacion) Then
                    Throw New Exception(odaHomologacion.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function
End Class
