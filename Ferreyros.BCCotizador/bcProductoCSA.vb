Imports Ferreyros.DACotizador
Imports Ferreyros.BECotizador

Public Class bcProductoCSA

    Private dProductoCSA As daProductoCSA = Nothing

    Public Sub ProductoCSAListar(ByVal eConexion As String, ByVal Cotizacion As beCotizacion, ByRef Producto As beProducto, ByRef ProductoCSA As beProductoCSA, ByRef eValidacion As beValidacion)

        Try
            dProductoCSA = New daProductoCSA
            dProductoCSA.ProductoCSAListar(eConexion, Cotizacion, Producto, ProductoCSA, eValidacion)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub

End Class
