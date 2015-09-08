Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador
Imports System.Data.SqlClient

Public Class bcCotizacionContacto
    Private p_strError As String
    Private dCotizacionContacto As daCotizacionContacto = Nothing

    Public ReadOnly Property ErrorDes() As String
        Get
            Return p_strError
        End Get
    End Property
    Public Sub CotizacionContactoListar(ByVal eConexion As String, ByVal eCotizacion As beCotizacion, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beCotizacionContacto))

        Try
            dCotizacionContacto = New daCotizacionContacto
            dCotizacionContacto.CotizacionContactoListar(eConexion, eCotizacion, eValidacion, lResult)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub

    Public Function BuscarIdCotizacion(ByVal strConexion As String, ByVal obeCotizacionContacto As beCotizacionContacto, ByRef ListabeCotizacionContacto As List(Of beCotizacionContacto)) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaCotizacionContacto As New daCotizacionContacto
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaCotizacionContacto.BuscarIdCotizacion(CnnSql, obeCotizacionContacto, ListabeCotizacionContacto) Then
                    Throw New Exception(odaCotizacionContacto.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString
        End Try
        Return bolExito
    End Function

End Class
