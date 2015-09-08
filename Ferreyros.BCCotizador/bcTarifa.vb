Imports Ferreyros.DACotizador
Imports Ferreyros.BECotizador
Imports log4net

Public Class bcTarifa

    Private _daTarifa As daTarifa = Nothing
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(bcTarifa))

    Public Sub ListarTarifas(ByVal oConexion As String, _
                                ByVal oTarifa As beTarifa, _
                                ByRef oValidacion As beValidacion, _
                                ByRef oDetalle As List(Of beTarifa))

        Try
            _daTarifa = New daTarifa
            _daTarifa.ListarTarifas(oConexion, oTarifa, oValidacion, oDetalle)
        Catch ex As Exception
            oValidacion.mensaje = ex.Message
            log.Error(oValidacion.cadenaAleatoria + ": " + oValidacion.mensaje)
        End Try

    End Sub

    Public Sub MantenimientoTarifas(ByVal oConexion As String, ByVal oTarifa As beTarifa, ByRef oValidacion As beValidacion)

        Try
            _daTarifa = New daTarifa

            If oValidacion.flag = "3" Then GoTo Acceder

            With oTarifa
                If Not IsNumeric(.kitRepuestos) Then .kitRepuestos = 0
                If Not IsNumeric(.servicioContratado) Then .servicioContratado = 0
                If Not IsNumeric(.fluidos) Then .fluidos = 0
                If Not IsNumeric(.eventosNueva) Then .eventosNueva = 1
                .total = CDbl(.kitRepuestos) + CDbl(.servicioContratado) + CDbl(.fluidos)
                .kitRepuestosT = CDbl(.kitRepuestos) * CDbl(.eventosNueva)
                .fluidosT = CDbl(.eventosNueva) * CDbl(.fluidos)
                .servicioContratadoT = CDbl(.servicioContratado) * CDbl(.eventosNueva)
                .totalT = CDbl(.kitRepuestosT) + CDbl(.fluidosT) + CDbl(.servicioContratadoT)
            End With

Acceder:
            _daTarifa.MantenimientoTarifas(oConexion, oTarifa, oValidacion)
        Catch ex As Exception
            oValidacion.mensaje = ex.Message.ToString
            log.Error(oValidacion.cadenaAleatoria + ": " + oValidacion.mensaje)
        End Try

    End Sub

    Public Sub ListarDetallePartes(ByVal oConexion As String, _
                              ByVal oTarifa As beTarifa, _
                              ByRef oValidacion As beValidacion, _
                              ByRef oDetalle As List(Of beTarifa))

        Try
            _daTarifa = New daTarifa
            _daTarifa.ListarDetallePartes(oConexion, oTarifa, oValidacion, oDetalle)
        Catch ex As Exception
            oValidacion.mensaje = ex.Message.ToString
            log.Error(oValidacion.cadenaAleatoria + ": " + oValidacion.mensaje)
        End Try

    End Sub

    Public Sub MantenimientoDetallePartes(ByVal oConexion As String, _
                        ByVal oPartes As beTarifa, _
                        ByRef oValidacion As beValidacion)

        Try
            _daTarifa = New daTarifa
            _daTarifa.MantenimientoDetallePartes(oConexion, oPartes, oValidacion)
        Catch ex As Exception
            oValidacion.mensaje = ex.Message.ToString
            log.Error(oValidacion.cadenaAleatoria + ": " + oValidacion.mensaje)
        End Try

    End Sub

    Public Sub ListarModBaseCodPlanPrefPM(ByVal oConexion As String, _
                                       ByVal oTarifa As beTarifa, _
                           ByRef oValidacion As beValidacion, _
                           ByRef oDetalle As List(Of beTarifa))

        Try
            _daTarifa = New daTarifa
            _daTarifa.ListarModBaseCodPlanPrefPM(oConexion, oTarifa, oValidacion, oDetalle)
        Catch ex As Exception
            oValidacion.mensaje = ex.Message
        End Try

    End Sub

    Public Sub ListarModeloCodPlanEvento(ByVal oConexion As String, _
                                    ByVal oTarifa As beTarifa, _
                        ByRef oValidacion As beValidacion, _
                        ByRef oDetalle As List(Of beTarifa))

        Try
            _daTarifa = New daTarifa
            _daTarifa.ListarModeloCodPlanEvento(oConexion, oTarifa, oValidacion, oDetalle)
        Catch ex As Exception
            oValidacion.mensaje = ex.Message
        End Try

    End Sub

End Class
