Imports Ferreyros.DACotizador
Imports Ferreyros.BECotizador

Public Class bcConfiguracionSolucionCombinada
    Private aConfiguracionSolucionCombinada As daConfiguracionSolucionCombinada = Nothing
    Private p_strError As String

    Public ReadOnly Property ErrorDes() As String
        Get
            Return p_strError
        End Get
    End Property

    Public Sub MantenimientoConfigSolComb(ByVal oConexion As String, ByVal eConfiguracionSolucionCombinada As beConfiguracionSolucionCombinada, ByRef oValidacion As beValidacion)

        Try
            aConfiguracionSolucionCombinada = New daConfiguracionSolucionCombinada
            aConfiguracionSolucionCombinada.MantenimientoConfiguracionSolComb(oConexion, eConfiguracionSolucionCombinada, oValidacion)

        Catch ex As Exception
            oValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub

End Class
