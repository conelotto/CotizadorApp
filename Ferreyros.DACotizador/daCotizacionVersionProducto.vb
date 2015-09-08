Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.Utiles.Estructuras
Public Class daCotizacionVersionProducto

    Private blnValido As Boolean = False
    Private uData As New Utiles.Datos
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function BuscarIdCotizacionVersion(ByVal cnnSql As SqlConnection, ByVal obeCotizacionVersionProducto As beCotizacionVersionProducto, ByRef ListabeCotizacionVersionProducto As List(Of beCotizacionVersionProducto)) As Boolean
        Try
            blnValido = False
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = objBBDD.StoreProcedure.UspCotizacionVersionProductoBuscarIdCotVers

            cmdSql.Parameters.Add(uData.CreaParametro("@IdCotizacionVersion", obeCotizacionVersionProducto.IdCotizacionVersion, SqlDbType.Int))

            Using dr As SqlDataReader = cmdSql.ExecuteReader

                Dim _IdCotizacionVersionProducto As String = dr.GetOrdinal("IdCotizacionVersionProducto")
                Dim _IdCotizacionVersion As String = dr.GetOrdinal("IdCotizacionVersion")
                Dim _CodigoProducto As String = dr.GetOrdinal("CodigoProducto")
                Dim _NombreProducto As String = dr.GetOrdinal("NombreProducto")


                Dim ebeCotizacionVersionProducto As beCotizacionVersionProducto = Nothing

                While dr.Read()
                    ebeCotizacionVersionProducto = New beCotizacionVersionProducto

                    With ebeCotizacionVersionProducto
                        If Not dr.IsDBNull(_IdCotizacionVersionProducto) Then .IdCotizacionVersionProducto = dr.GetValue(_IdCotizacionVersionProducto).ToString()
                        If Not dr.IsDBNull(_IdCotizacionVersion) Then .IdCotizacionVersion = dr.GetValue(_IdCotizacionVersion).ToString()
                        If Not dr.IsDBNull(_CodigoProducto) Then .CodigoProducto = dr.GetValue(_CodigoProducto).ToString()
                        If Not dr.IsDBNull(_NombreProducto) Then .NombreProducto = dr.GetValue(_NombreProducto).ToString()

                    End With
                    ListabeCotizacionVersionProducto.Add(ebeCotizacionVersionProducto)
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
