Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras
Public Class daCotizacionContacto

    Private uData As New Utiles.Datos
    Private Command As SqlCommand = Nothing

    Public Sub InsertarCotizacionContacto(ByVal Connection As SqlConnection, ByVal Transaction As SqlTransaction, _
                                          ByVal IdCotizacion As Integer, ByVal Usuario As String, _
                                          ByVal CotizacionContacto As beCotizacionContacto)

        If Connection Is Nothing OrElse Transaction Is Nothing OrElse CotizacionContacto Is Nothing Then
            Return
        End If

        Command = Connection.CreateCommand
        Command.Transaction = Transaction
        Command.CommandText = "uspServicioCotizacionContactoInsertar"
        Command.CommandType = CommandType.StoredProcedure

        With Command.Parameters
            .Add(uData.CreaParametro("@IdCotizacion", IdCotizacion, SqlDbType.Int, 15))
            .Add(uData.CreaParametro("@Nombres", CotizacionContacto.Nombres, SqlDbType.VarChar, 150))
            .Add(uData.CreaParametro("@Direccion", CotizacionContacto.Direccion, SqlDbType.VarChar, 150))
            .Add(uData.CreaParametro("@Email", CotizacionContacto.Email, SqlDbType.VarChar, 150))
            .Add(uData.CreaParametro("@Cargo", CotizacionContacto.Cargo, SqlDbType.VarChar, 150))
            .Add(uData.CreaParametro("@Telefono", CotizacionContacto.Telefono, SqlDbType.VarChar, 50))
            .Add(uData.CreaParametro("@Usuario", Usuario, SqlDbType.VarChar, 15))
        End With

        Command.ExecuteNonQuery()


    End Sub

End Class

