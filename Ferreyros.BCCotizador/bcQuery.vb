Imports Ferreyros.DACotizador
Imports Ferreyros.BECotizador
Imports log4net
Public Class bcQuery

    Private _daQuery As daQuery = Nothing
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(bcTarifa))

    Public Sub MantenimientoQuerys(ByVal oConexion As String, ByVal oQuery As beQuery, ByRef oValidacion As beValidacion)

        Try
            _daQuery = New daQuery
            _daQuery.MantenimientoQuerys(oConexion, oQuery, oValidacion)

        Catch ex As Exception
            oValidacion.mensaje = ex.Message.ToString
            log.Error(oValidacion.cadenaAleatoria + ": " + oValidacion.mensaje)
        End Try

    End Sub

    Public Sub listarComboQuery(ByVal eConexion As String, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beQuery))

        Try
            _daQuery = New daQuery
            _daQuery.listarComboQuery(eConexion, eValidacion, lResult)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub
End Class
