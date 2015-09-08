Imports System.Data.SqlClient
Imports Ferreyros.BECotizador

Public Class daProductoAlquilerTarifa
    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property
    Public Function BuscarIdProducto(ByVal cnnSql As SqlConnection, ByVal obeProductoAlquilerTarifa As beProductoAlquilerTarifa, ByRef ListaProductoAlquilerTarifa As List(Of beProductoAlquilerTarifa)) As Boolean
        Try
            blnValido = False
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = objBBDD.StoreProcedure.uspProductoAlquilerTarifaBuscarIdProducto

            cmdSql.Parameters.Add(uData.CreaParametro("@IdProducto", obeProductoAlquilerTarifa.IdProducto, SqlDbType.Int))

            Using dr As SqlDataReader = cmdSql.ExecuteReader
                Dim _IdProducto As String = dr.GetOrdinal("IdProducto")
                Dim _ValorEscala As String = dr.GetOrdinal("ValorEscala")
                Dim _Importe As String = dr.GetOrdinal("Importe")
                Dim _Moneda As String = dr.GetOrdinal("Moneda")
                Dim _UnidadMedidaPrecio As String = dr.GetOrdinal("UnidadMedidaPrecio")
                Dim _CodigoUnidadMedida As String = dr.GetOrdinal("CodigoUnidadMedida")

                Dim ebeProductoAlquilerTarifa As beProductoAlquilerTarifa = Nothing

                While dr.Read()
                    ebeProductoAlquilerTarifa = New beProductoAlquilerTarifa

                    With ebeProductoAlquilerTarifa
                        If Not dr.IsDBNull(_IdProducto) Then .IdProducto = dr.GetValue(_IdProducto).ToString
                        If Not dr.IsDBNull(_ValorEscala) Then .ValorEscala = dr.GetValue(_ValorEscala).ToString()
                        If Not dr.IsDBNull(_Importe) Then .Importe = dr.GetValue(_Importe).ToString()
                        If Not dr.IsDBNull(_Moneda) Then .Moneda = dr.GetValue(_Moneda).ToString()
                        If Not dr.IsDBNull(_UnidadMedidaPrecio) Then .UnidadMedidaPrecio = dr.GetValue(_UnidadMedidaPrecio).ToString()
                        If Not dr.IsDBNull(_CodigoUnidadMedida) Then .CodigoUnidadMedida = dr.GetValue(_CodigoUnidadMedida).ToString()
                    End With
                    ListaProductoAlquilerTarifa.Add(ebeProductoAlquilerTarifa)
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
