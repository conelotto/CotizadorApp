Imports Ferreyros.DACotizador
Imports Ferreyros.BECotizador
Imports log4net

Public Class bcLineaResponsable
    Private _daLineaResponsable As daLineaResponsable = Nothing
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(bcTarifa))

    Public Sub LineaResponsableMant(ByVal oConexion As String, ByVal oLineaResponsable As beLineaResponsable, ByRef oValidacion As beValidacion)

        Try
            _daLineaResponsable = New daLineaResponsable
            _daLineaResponsable.LineaResponsableMant(oConexion, oLineaResponsable, oValidacion)

        Catch ex As Exception
            oValidacion.mensaje = ex.Message.ToString
            log.Error(oValidacion.cadenaAleatoria + ": " + oValidacion.mensaje)
        End Try

    End Sub

    Public Sub listarCombo(ByVal eConexion As String, ByVal eLineaResponsable As beLineaResponsable, _
                           ByRef eValidacion As beValidacion, ByRef lResult As List(Of beLineaResponsable))

        Try
            _daLineaResponsable = New daLineaResponsable
            _daLineaResponsable.listarCombo(eConexion, eLineaResponsable, eValidacion, lResult)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub

    Public Sub LineaResponsableListar(ByVal eConexion As String, ByRef eValidacion As beValidacion, _
                                      ByRef lResult As List(Of beLineaResponsable))

        Try
            _daLineaResponsable = New daLineaResponsable
            _daLineaResponsable.LineaResponsableListar(eConexion, eValidacion, lResult)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub

End Class
