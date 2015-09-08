Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.Utiles.Estructuras
Public Class daTelefonoResponsable
    Private blnValido As Boolean = False
    Private uData As New Utiles.Datos
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property
    Public Function TelefonoResponsableListar(ByVal cnnSql As SqlConnection, ByVal obeTelefonoResponsable As beTelefonoResponsable, ByRef ListaTelefonoResponsable As List(Of beTelefonoResponsable)) As Boolean
        Try
            blnValido = False
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = "uspTelefonoResponsableListar"
            cmdSql.Parameters.Add(uData.CreaParametro("@IdCotizacion", obeTelefonoResponsable.IdCotizacion, SqlDbType.Int, 15))

            Using dr As SqlDataReader = cmdSql.ExecuteReader

                Dim _CodTipoTelefono As String = dr.GetOrdinal("CodTipoTelefono")
                Dim _NroTelefono As String = dr.GetOrdinal("NroTelefono")
                Dim _Anexo As String = dr.GetOrdinal("Anexo")

                Dim ebeTelefonoResponsable As beTelefonoResponsable = Nothing

                While dr.Read()
                    ebeTelefonoResponsable = New beTelefonoResponsable

                    With ebeTelefonoResponsable
                        If Not dr.IsDBNull(_CodTipoTelefono) Then .CodTipoTelefono = dr.GetValue(_CodTipoTelefono).ToString()
                        If Not dr.IsDBNull(_NroTelefono) Then .NroTelefono = dr.GetValue(_NroTelefono).ToString()
                        If Not dr.IsDBNull(_Anexo) Then .Anexo = dr.GetValue(_Anexo).ToString()
                    End With
                    ListaTelefonoResponsable.Add(ebeTelefonoResponsable)
                End While
                dr.Close()
            End Using
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function
End Class
