Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros.DACotizador
Imports Ferreyros.BECotizador
Imports log4net
Public Class bcAprobadorUsuario

    Private pr_strError As String
    Public ReadOnly Property ErrorDes() As String
        Get
            Return pr_strError
        End Get
    End Property

    Public Function Buscar(ByVal strConexion As String, ByVal eAprobadorUsuario As beAprobadorUsuario, ByRef dstAprobadorUsuario As DataSet) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim oAprobadorUsuario As New daAprobadorUsuario
            Using cnnSql As New SqlConnection(strConexion)
                cnnSql.Open()
                If Not oAprobadorUsuario.Buscar(cnnSql, eAprobadorUsuario, dstAprobadorUsuario) Then
                    Throw New Exception(oAprobadorUsuario.ErrorDes)
                End If
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function

    Public Function ExisteUsuarioEnOtroAprobador(ByVal strConexion As String, ByVal oAprobadorUsuario As bcAprobadorUsuario, _
                                                 ByVal eAprobadorUsuario As beAprobadorUsuario) As Boolean
        Dim dstAprobadorUsuarioAux As New DataSet
        Dim blnResultado As Boolean = False
        oAprobadorUsuario.BuscarPorMatricula(strConexion, eAprobadorUsuario, dstAprobadorUsuarioAux)
        If dstAprobadorUsuarioAux IsNot Nothing Then
            If dstAprobadorUsuarioAux.Tables(0).Rows.Count > 1 Then
                blnResultado = True
                pr_strError = eAprobadorUsuario.MatriculaUsuario & "|" & eAprobadorUsuario.NombreUsuario & " ya ha sido asignado a otro aprobador"
            End If
        End If
        dstAprobadorUsuarioAux = Nothing
        Return blnResultado
    End Function

    Private Function BuscarPorMatricula(ByVal strConexion As String, ByVal eAprobadorUsuario As beAprobadorUsuario, _
                                        ByRef dstAprobadorUsuario As DataSet) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim oAprobadorUsuario As New daAprobadorUsuario
            Using cnnSql As New SqlConnection(strConexion)
                cnnSql.Open()
                If Not oAprobadorUsuario.BuscarPorMatricula(cnnSql, eAprobadorUsuario, dstAprobadorUsuario) Then
                    Throw New Exception(oAprobadorUsuario.ErrorDes)
                End If
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function

End Class
