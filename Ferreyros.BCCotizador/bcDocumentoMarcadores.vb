Imports Ferreyros.DACotizador
Imports Ferreyros.BECotizador

Public Class bcDocumentoMarcadores

    Private dDocumentoMarcadores As daDocumentoMarcadores = Nothing
    Private p_strError As String

    Public ReadOnly Property ErrorDes() As String
        Get
            Return p_strError
        End Get
    End Property

    Public Sub DocumentoMarcadoresListar(ByVal eConexion As String, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beDocumentoMarcadores))

        Try
            dDocumentoMarcadores = New daDocumentoMarcadores
            dDocumentoMarcadores.DocumentoMarcadoresListar(eConexion, eValidacion, lResult)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub

    Public Sub MantenimientoDocumentoMarcadores(ByVal oConexion As String, ByVal eDocumentoMarcadores As beDocumentoMarcadores, ByRef oValidacion As beValidacion)

        Try
            dDocumentoMarcadores = New daDocumentoMarcadores
            dDocumentoMarcadores.MantenimientoDocumentoMarcadores(oConexion, eDocumentoMarcadores, oValidacion)

        Catch ex As Exception
            oValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub
End Class
