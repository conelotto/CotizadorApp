Imports System.Data.SqlClient
Imports Ferreyros.BECotizador

Public Class daProductoAdicional
    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property
    Public Function BuscarIdProducto(ByVal cnnSql As SqlConnection, ByVal obeProductoAdicional As beProductoAdicional, ByRef ListaProductoAdicional As List(Of beProductoAdicional)) As Boolean
        Try
            blnValido = False
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = objBBDD.StoreProcedure.uspProductoAdicionalBuscarIdProducto

            cmdSql.Parameters.Add(uData.CreaParametro("@IdProducto", obeProductoAdicional.IdProducto, SqlDbType.Int))

            Using dr As SqlDataReader = cmdSql.ExecuteReader
                Dim _IdProductoAdicional As String = dr.GetOrdinal("IdProductoAdicional")
                Dim _CodigoProductoAdicional As String = dr.GetOrdinal("CodigoProductoAdicional")
                Dim _NombreProdutoAdicional As String = dr.GetOrdinal("NombreProdutoAdicional")
                Dim _Cantidad As String = dr.GetOrdinal("Cantidad")
                Dim _UnidadMedida As String = dr.GetOrdinal("UnidadMedida")
                Dim _FlatMostrarEspTecnica As String = dr.GetOrdinal("FlatMostrarEspTecnica")

                Dim ebeProductoAdicional As beProductoAdicional = Nothing

                While dr.Read()
                    ebeProductoAdicional = New beProductoAdicional

                    With ebeProductoAdicional
                        If Not dr.IsDBNull(_IdProductoAdicional) Then .IdProductoAdicional = dr.GetValue(_IdProductoAdicional).ToString
                        If Not dr.IsDBNull(_CodigoProductoAdicional) Then .CodigoProductoAdicional = dr.GetValue(_CodigoProductoAdicional).ToString()
                        If Not dr.IsDBNull(_NombreProdutoAdicional) Then .NombreProdutoAdicional = dr.GetValue(_NombreProdutoAdicional).ToString()
                        If Not dr.IsDBNull(_Cantidad) Then .Cantidad = dr.GetValue(_Cantidad).ToString()
                        If Not dr.IsDBNull(_UnidadMedida) Then .UnidadMedida = dr.GetValue(_UnidadMedida).ToString()
                        If Not dr.IsDBNull(_FlatMostrarEspTecnica) Then .FlatMostrarEspTecnica = dr.GetValue(_FlatMostrarEspTecnica).ToString()
                    End With
                    ListaProductoAdicional.Add(ebeProductoAdicional)
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
