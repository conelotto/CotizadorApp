Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador

Public Class bcProductoPrime
    Private p_strError As String

    Public Function BuscarId(ByVal strConexion As String, ByRef obeProducto As beProducto) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaProductoPrime As New daProductoPrime
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaProductoPrime.BuscarId(CnnSql, obeProducto) Then
                    Throw New Exception(odaProductoPrime.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function

    Public Function Eliminar(ByVal strConexion As String, ByVal obeProductoPrime As beProductoPrime) As Boolean
        Dim blnValido As Boolean = False
        Dim odaProductoPrime As New daProductoPrime
        Using cnnSql As New SqlConnection(strConexion)
            Try
                cnnSql.Open()
                If odaProductoPrime.Eliminar(cnnSql, obeProductoPrime) Then
                    blnValido = True
                Else
                    Throw New Exception(odaProductoPrime.ErrorDes)
                End If
            Catch ex As Exception
                blnValido = False
                p_strError = ex.Message.ToString()
            End Try
        End Using

        Return blnValido
    End Function
End Class
