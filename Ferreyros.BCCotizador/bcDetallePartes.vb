Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador
Public Class bcDetallePartes
    Private daDetallePartes As daDetallePartes = Nothing
    Public Sub BuscarLlave(ByVal eConexion As String, ByVal beDetallePartes As beDetallePartes, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beDetallePartes))

        Try
            daDetallePartes = New daDetallePartes
            daDetallePartes.BuscarLlave(eConexion, beDetallePartes, eValidacion, lResult)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub
End Class
