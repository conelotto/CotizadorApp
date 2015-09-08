Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador
Imports System.Data.SqlClient
Public Class bcSolucionCombinada
    Private dSolucionCombinada As daSolucionCombinada = Nothing
    Private p_strError As String

    Public ReadOnly Property ErrorDes() As String
        Get
            Return p_strError
        End Get
    End Property
    Public Sub SolucionCombinadaListar(ByVal eConexion As String, ByVal eCotizacion As beCotizacion, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beSolucionCombinada))

        Try
            dSolucionCombinada = New daSolucionCombinada
            dSolucionCombinada.SolucionCombinadaListar(eConexion, eCotizacion, eValidacion, lResult)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub
End Class
