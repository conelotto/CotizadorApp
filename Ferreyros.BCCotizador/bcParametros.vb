Imports Ferreyros.DACotizador
Imports Ferreyros.BECotizador

Public Class bcParametros

    Private oDataParametros As daParametros = Nothing

    Public Sub ListarDetalleCsa(ByVal oConexion As String, _
                                ByVal oMaquinaria As beMaquinaria, _
                                ByRef oValidacion As beValidacion, _
                                ByRef oDetalle As List(Of beMaquinaria))

        Try
            oDataParametros = New daParametros
            oDataParametros.ListarDetalleCsa(oConexion, oMaquinaria, oValidacion, oDetalle)
        Catch ex As Exception
            oValidacion.mensaje = ex.Message
        End Try

    End Sub

    Public Sub ListarNroSerie(ByVal oConexion As String, ByVal oLibreria As String, ByVal oMaquinaria As beMaquinaria, ByRef oValidacion As beValidacion, ByRef oDetalle As List(Of beMaquinaria))

        Try
            oDataParametros = New daParametros
            oDataParametros.ListarNroSerie(oConexion, oLibreria, oMaquinaria, oValidacion, oDetalle)
        Catch ex As Exception
            oValidacion.mensaje = ex.Message
        End Try

    End Sub

End Class
