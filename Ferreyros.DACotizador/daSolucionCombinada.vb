Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.Utiles.Estructuras

Public Class daSolucionCombinada

    Private blnValido As Boolean = False
    Private uData As New Utiles.Datos
    Private strError As String = String.Empty

    Public Sub SolucionCombinadaListar(ByVal eConexion As String, ByVal eCotizacion As beCotizacion, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beSolucionCombinada))

        Dim conexion As SqlConnection = Nothing
        Dim comand As SqlCommand = Nothing
        Dim SolucionCombinada As beSolucionCombinada = Nothing
        Try
            conexion = New SqlConnection(eConexion)
            conexion.Open()
            comand = conexion.CreateCommand
            comand.CommandText = "uspSolucionCombinadaListar"
            comand.CommandType = CommandType.StoredProcedure
            comand.Parameters.Add(uData.CreaParametro("@IdCotizacionSap", eCotizacion.IdCotizacionSap, SqlDbType.VarChar, 20))
            Using dr As IDataReader = comand.ExecuteReader
                Dim _IdCotizacion As Integer = dr.GetOrdinal("IdCotizacion")
                Dim _IdProducto As Integer = dr.GetOrdinal("IdProducto")
                Dim _IdPosicion As Integer = dr.GetOrdinal("IdPosicion")
                Dim _IdTarifa As Integer = dr.GetOrdinal("IdTarifa")
                Dim _LLave As Integer = dr.GetOrdinal("LLave")
                Dim _Valor As Integer = dr.GetOrdinal("Valor")

                While dr.Read
                    SolucionCombinada = New beSolucionCombinada
                    If Not dr.IsDBNull(_IdCotizacion) Then SolucionCombinada.IdCotizacion = dr.GetValue(_IdCotizacion)
                    If Not dr.IsDBNull(_IdProducto) Then SolucionCombinada.IdProducto = dr.GetValue(_IdProducto)
                    If Not dr.IsDBNull(_IdPosicion) Then SolucionCombinada.IdPosicion = dr.GetValue(_IdPosicion)
                    If Not dr.IsDBNull(_IdTarifa) Then SolucionCombinada.IdTarifa = dr.GetValue(_IdTarifa)
                    If Not dr.IsDBNull(_LLave) Then SolucionCombinada.LLave = dr.GetValue(_LLave)
                    If Not dr.IsDBNull(_Valor) Then SolucionCombinada.Valor = dr.GetValue(_Valor)

                    lResult.Add(SolucionCombinada)
                End While
            End Using
            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        Finally
            If conexion.State = ConnectionState.Open Then conexion.Close()
        End Try

    End Sub
End Class
