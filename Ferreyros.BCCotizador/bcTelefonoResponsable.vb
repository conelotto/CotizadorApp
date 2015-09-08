Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador

Public Class bcTelefonoResponsable
    Private dTelefonoResponsable As daTelefonoResponsable = Nothing
    Private p_strError As String

    Public ReadOnly Property ErrorDes() As String
        Get
            Return p_strError
        End Get
    End Property

    Public Function TelefonoResponsableListar(ByVal eConexion As String, ByVal eTelefonoResponsable As beTelefonoResponsable, ByRef lResult As List(Of beTelefonoResponsable)) As Boolean

        Dim bolExito As Boolean = False
        Try
            dTelefonoResponsable = New daTelefonoResponsable
            Using CnnSql As New SqlConnection(eConexion)
                CnnSql.Open()
                If Not dTelefonoResponsable.TelefonoResponsableListar(CnnSql, eTelefonoResponsable, lResult) Then
                    Throw New Exception(dTelefonoResponsable.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString
        End Try

        Return bolExito
    End Function
End Class

