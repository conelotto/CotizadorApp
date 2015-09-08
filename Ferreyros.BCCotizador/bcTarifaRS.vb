Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador
Imports System.Data.SqlClient
Public Class bcTarifaRS

    Private dProducto As daProducto = Nothing
    Private p_strError As String

    Public ReadOnly Property ErrorDes() As String
        Get
            Return p_strError
        End Get
    End Property

    Public Function BuscarCombinacionLlave(ByVal strConexion As String, ByVal obeDatoGeneral As beDatoGeneral, ByRef ListabeTarifaRS As List(Of beTarifaRS)) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaTarifaRS As New daTarifaRS
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaTarifaRS.BuscarCombinacionLlave(CnnSql, obeDatoGeneral, ListabeTarifaRS) Then
                    Throw New Exception(odaTarifaRS.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString
        End Try
        Return bolExito
    End Function
End Class
