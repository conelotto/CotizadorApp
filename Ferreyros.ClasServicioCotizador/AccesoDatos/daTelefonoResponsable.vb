Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras

Public Class daTelefonoResponsable

    Private uData As New Utiles.Datos
    Private Command As SqlCommand = Nothing

    Public Sub InsertarTelefonoResponsable(ByVal Connection As SqlConnection, ByVal Transaction As SqlTransaction, _
                                          ByVal IdCotizacion As Integer, ByVal TelefonoResponsable As beTelefonoResponsable)

        If Connection Is Nothing OrElse Transaction Is Nothing OrElse TelefonoResponsable Is Nothing Then
            Return
        End If

        Command = Connection.CreateCommand
        Command.Transaction = Transaction
        Command.CommandText = "uspServicioTelefonoResponsableInsertar"
        Command.CommandType = CommandType.StoredProcedure

        With Command.Parameters
            .Add(uData.CreaParametro("@IdCotizacion", IdCotizacion, SqlDbType.Int, 15))
            .Add(uData.CreaParametro("@CodTipoTelefono", TelefonoResponsable.CodTipoTelefono, SqlDbType.VarChar, 20))
            .Add(uData.CreaParametro("@NroTelefono", TelefonoResponsable.NroTelefono, SqlDbType.VarChar, 20))
            .Add(uData.CreaParametro("@Anexo", TelefonoResponsable.Anexo, SqlDbType.VarChar, 20))
        End With

        Command.ExecuteNonQuery()


    End Sub

End Class


