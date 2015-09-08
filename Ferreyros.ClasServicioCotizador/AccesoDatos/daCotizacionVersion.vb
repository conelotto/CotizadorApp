
Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras


Public Class daCotizacionVersion
    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty
    Private uConfig As New Utiles.uConfiguracion
    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function BuscarIdCotizacionSap(ByVal cnnSql As SqlConnection,ByRef obeCotizacionVersion As beCotizacionVersion, ByRef lista As List(Of beCotizacionVersion)) As Boolean
        Try
            blnValido = False
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = "uspServicioCotizacionVersionBuscar"

            cmdSql.Parameters.Add(uData.CreaParametro("@IdCotizacionSap", obeCotizacionVersion.IdCotizacionSap, SqlDbType.VarChar, 20))

            Using dr As SqlDataReader = cmdSql.ExecuteReader

                Dim _IdCotizacionVersion As String = dr.GetOrdinal("IdCotizacionVersion")
                Dim _IdCotizacion As String = dr.GetOrdinal("IdCotizacion")
                Dim _IdCotizacionSap As String = dr.GetOrdinal("IdCotizacionSap")
                Dim _DescripcionProducto As String = dr.GetOrdinal("DescripcionProducto")
                Dim _NumVersion As String = dr.GetOrdinal("NumVersion")
                Dim _Fecha As String = dr.GetOrdinal("Fecha")
                Dim _ValorNegociado As String = dr.GetOrdinal("ValorNegociado")
                Dim _Moneda As String = dr.GetOrdinal("Moneda")
                Dim _NombreArchivo As String = dr.GetOrdinal("NombreArchivo")
                Dim _TieneDetalleParte As String = dr.GetOrdinal("TieneDetalleParte")

 

                Dim ebeCotizacionVersion As beCotizacionVersion = Nothing

                While dr.Read()
                    ebeCotizacionVersion = New beCotizacionVersion

                    With ebeCotizacionVersion
                        If Not dr.IsDBNull(_IdCotizacionVersion) Then .IdCotizacionVersion = dr.GetValue(_IdCotizacionVersion).ToString()
                        If Not dr.IsDBNull(_IdCotizacion) Then .IdCotizacion = dr.GetValue(_IdCotizacion).ToString()
                        If Not dr.IsDBNull(_IdCotizacionSap) Then .IdCotizacionSap = dr.GetValue(_IdCotizacionSap).ToString()
                        If Not dr.IsDBNull(_DescripcionProducto) Then .DescripcionProducto = dr.GetValue(_DescripcionProducto).ToString()
                        If Not dr.IsDBNull(_NumVersion) Then .NumVersion = dr.GetValue(_NumVersion).ToString()
                        If Not dr.IsDBNull(_Fecha) Then .Fecha = dr.GetValue(_Fecha).ToString()
                        If Not dr.IsDBNull(_ValorNegociado) Then .ValorNegociado = dr.GetValue(_ValorNegociado).ToString()
                        If Not dr.IsDBNull(_Moneda) Then .Moneda = dr.GetValue(_Moneda).ToString()
                        If Not dr.IsDBNull(_NombreArchivo) Then .NombreArchivo = dr.GetValue(_NombreArchivo).ToString()
                        If Not dr.IsDBNull(_TieneDetalleParte) Then .TieneDetalleParte = dr.GetValue(_TieneDetalleParte).ToString()
                    End With
                    lista.Add(ebeCotizacionVersion)
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
