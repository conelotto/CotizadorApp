Imports Ferreyros.DACotizador
Imports Ferreyros.BECotizador

Public Class bcMaquinaria

    Private dMaquinaria As daMaquinaria = Nothing


    Public Sub MaquinariaListar(ByVal eConexion As String, ByVal Cotizacion As beCotizacion, ByVal Producto As beProducto, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beMaquinaria))

        Try
            dMaquinaria = New daMaquinaria
            dMaquinaria.MaquinariaListar(eConexion, Cotizacion, Producto, eValidacion, lResult)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub

End Class
