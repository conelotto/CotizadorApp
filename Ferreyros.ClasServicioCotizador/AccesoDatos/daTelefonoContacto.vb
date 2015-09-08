Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras

Public Class daTelefonoContacto

    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    'Public Function Insertar(ByVal cnnSql As SqlConnection, ByVal Cotizacion As beCotizacion, ByVal CotizacionContacto As beCotizacionContacto, ByVal obeTelefonoContacto As beTelefonoContacto, _
    '                          Optional ByVal trSql As SqlTransaction = Nothing) As Boolean
    '    blnValido = False
    '    strError = String.Empty
    '    Try

    '        Dim cmdSql As SqlCommand = cnnSql.CreateCommand
    '        If trSql IsNot Nothing Then
    '            cmdSql.Transaction = trSql
    '        End If
    '        cmdSql.CommandText = "uspServicioTelefonoContactoInsertar"
    '        cmdSql.CommandType = CommandType.StoredProcedure
    '        With obeTelefonoContacto
    '            cmdSql.Parameters.Add(uData.CreaParametro("@IdCotizacionContacto", CotizacionContacto.IdCotizacionContacto, SqlDbType.Int, 15))
    '            cmdSql.Parameters.Add(uData.CreaParametro("@CodigoTipoTelefono", .CodigoTipoTelefono, SqlDbType.VarChar, 10))
    '            cmdSql.Parameters.Add(uData.CreaParametro("@NumeroTelefono", .NumeroTelefono, SqlDbType.VarChar, 50))
    '            cmdSql.Parameters.Add(uData.CreaParametro("@Anexo", .Anexo, SqlDbType.VarChar, 15))
    '            cmdSql.Parameters.Add(uData.CreaParametro("@UsuarioCreacion", Cotizacion.UsuarioCreacion, SqlDbType.VarChar, 15))
    '            cmdSql.ExecuteNonQuery()
    '            blnValido = True
    '        End With
    '    Catch ex As Exception
    '        strError = ex.Message
    '    End Try

    '    Return blnValido
    'End Function

End Class
