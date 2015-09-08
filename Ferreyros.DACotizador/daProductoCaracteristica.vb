Imports System.Data.SqlClient
Imports Ferreyros.BECotizador

Public Class daProductoCaracteristica
    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property
    Public Function BuscarIdProducto(ByVal cnnSql As SqlConnection, ByVal obeProductoCaracteristica As beProductoCaracteristica, ByRef ListabeProductoCaracteristica As List(Of beProductoCaracteristica)) As Boolean
        Try
            blnValido = False
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = objBBDD.StoreProcedure.uspProductoCaracteristicaBuscarIdProducto

            cmdSql.Parameters.Add(uData.CreaParametro("@IdProducto", obeProductoCaracteristica.IdProducto, SqlDbType.Int))

            Using dr As SqlDataReader = cmdSql.ExecuteReader
                Dim _IdProducto As String = dr.GetOrdinal("IdProducto")
                Dim _Posicion As String = dr.GetOrdinal("Posicion")
                Dim _CodigoAtributo As String = dr.GetOrdinal("CodigoAtributo")
                Dim _DescripcionAtributo As String = dr.GetOrdinal("DescripcionAtributo")
                Dim _ValorAtributo As String = dr.GetOrdinal("ValorAtributo")
                Dim _CodigoUnidadMedida As String = dr.GetOrdinal("CodigoUnidadMedida")
                Dim _NombreUnidadMedida As String = dr.GetOrdinal("NombreUnidadMedida")

                Dim ebeProductoCaracteristica As beProductoCaracteristica = Nothing

                While dr.Read()
                    ebeProductoCaracteristica = New beProductoCaracteristica

                    With ebeProductoCaracteristica
                        If Not dr.IsDBNull(_IdProducto) Then .IdProducto = dr.GetValue(_IdProducto).ToString
                        If Not dr.IsDBNull(_Posicion) Then .Posicion = dr.GetValue(_Posicion).ToString()
                        If Not dr.IsDBNull(_CodigoAtributo) Then .CodigoAtributo = dr.GetValue(_CodigoAtributo).ToString()
                        If Not dr.IsDBNull(_DescripcionAtributo) Then .DescripcionAtributo = dr.GetValue(_DescripcionAtributo).ToString()
                        If Not dr.IsDBNull(_ValorAtributo) Then .ValorAtributo = dr.GetValue(_ValorAtributo).ToString()
                        If Not dr.IsDBNull(_CodigoUnidadMedida) Then .CodigoUnidadMedida = dr.GetValue(_CodigoUnidadMedida).ToString()
                        If Not dr.IsDBNull(_NombreUnidadMedida) Then .NombreUnidadMedida = dr.GetValue(_NombreUnidadMedida).ToString()
                    End With
                    ListabeProductoCaracteristica.Add(ebeProductoCaracteristica)
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
