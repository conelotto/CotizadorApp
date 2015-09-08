Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros.BECotizador

Public Class daProductoPrime
    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function BuscarId(ByVal cnnSql As SqlConnection, ByRef obeProducto As beProducto) As Boolean
        Try
            blnValido = False
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = objBBDD.StoreProcedure.uspProductoPrimeBuscarId
            With obeProducto
                cmdSql.Parameters.Add(uData.CreaParametro("@IdProducto", obeProducto.IdProducto, SqlDbType.Int))
                Dim dr As SqlDataReader = cmdSql.ExecuteReader()
                If dr.HasRows Then
                    dr.Read()
                    With obeProducto
                        Dim _IdProducto As String = dr.GetOrdinal("IdProducto")
                        Dim _IdCotizacion As String = dr.GetOrdinal("IdCotizacion")
                        Dim _IdPosicionSAP As String = dr.GetOrdinal("IdPosicionSAP")
                        Dim _IdProductoSAP As String = dr.GetOrdinal("IdProductoSAP")
                        Dim _TipoProducto As String = dr.GetOrdinal("TipoProducto")
                        Dim _Descripcion As String = dr.GetOrdinal("Descripcion")
                        Dim _ValorUnitario As String = dr.GetOrdinal("ValorUnitario")
                        Dim _IdMonedaValorUnitario As String = dr.GetOrdinal("IdMonedaValorUnitario")
                        Dim _Cantidad As String = dr.GetOrdinal("Cantidad")
                        Dim _Unidad As String = dr.GetOrdinal("Unidad")
                        Dim _ValorLista As String = dr.GetOrdinal("ValorLista")
                        Dim _IdMonedaValorNeto As String = dr.GetOrdinal("IdMonedaValorNeto")
                        Dim _NombreEstado As String = dr.GetOrdinal("NombreEstado")
                        Dim _FechaEstimCierre As String = dr.GetOrdinal("FechaEstimCierre")
                        Dim _PlazoEntregaEstim As String = dr.GetOrdinal("PlazoEntregaEstim")
                        Dim _CodigoFormaPago As String = dr.GetOrdinal("CodigoFormaPago")
                        Dim _FormaPago As String = dr.GetOrdinal("FormaPago")
                        Dim _FlatIncluyeRecompra As String = dr.GetOrdinal("FlatIncluyeRecompra")
                        Dim _FlatIncluyeCLC As String = dr.GetOrdinal("FlatIncluyeCLC")
                        Dim _PromHorasMensualUso As String = dr.GetOrdinal("PromHorasMensualUso")
                        Dim _beProductoPrime As New beProductoPrime

                        If Not dr.IsDBNull(_IdProducto) Then .IdProducto = dr.GetValue(_IdProducto).ToString()
                        If Not dr.IsDBNull(_IdProducto) Then _beProductoPrime.IdProducto = dr.GetValue(_IdProducto).ToString()
                        If Not dr.IsDBNull(_IdCotizacion) Then .IdCotizacion = dr.GetValue(_IdCotizacion).ToString()
                        If Not dr.IsDBNull(_IdPosicionSAP) Then .IdPosicion = dr.GetValue(_IdPosicionSAP).ToString()
                        If Not dr.IsDBNull(_IdProductoSAP) Then .IdProductoSap = dr.GetValue(_IdProductoSAP).ToString()
                        If Not dr.IsDBNull(_TipoProducto) Then .TipoProducto = dr.GetValue(_TipoProducto).ToString()
                        If Not dr.IsDBNull(_Descripcion) Then .Descripcion = dr.GetValue(_Descripcion).ToString()
                        If Not dr.IsDBNull(_ValorUnitario) Then .ValorUnitario = dr.GetValue(_ValorUnitario).ToString()
                        If Not dr.IsDBNull(_IdMonedaValorUnitario) Then .IdMonedaValorUnitario = dr.GetValue(_IdMonedaValorUnitario).ToString()
                        If Not dr.IsDBNull(_Cantidad) Then .Cantidad = dr.GetValue(_Cantidad).ToString()
                        If Not dr.IsDBNull(_Unidad) Then .Unidad = dr.GetValue(_Unidad).ToString()
                        If Not dr.IsDBNull(_ValorLista) Then .ValorLista = dr.GetValue(_ValorLista).ToString()
                        If Not dr.IsDBNull(_IdMonedaValorNeto) Then .IdMonedaValorNeto = dr.GetValue(_IdMonedaValorNeto).ToString()
                        If Not dr.IsDBNull(_NombreEstado) Then .NombreEstado = dr.GetValue(_NombreEstado).ToString()

                        If Not dr.IsDBNull(_FechaEstimCierre) Then _beProductoPrime.FechaEstimCierre = dr.GetValue(_FechaEstimCierre).ToString()
                        If Not dr.IsDBNull(_PlazoEntregaEstim) Then _beProductoPrime.PlazoEntregaEstim = dr.GetValue(_PlazoEntregaEstim).ToString()
                        If Not dr.IsDBNull(_CodigoFormaPago) Then _beProductoPrime.CodigoFormaPago = dr.GetValue(_CodigoFormaPago).ToString()
                        If Not dr.IsDBNull(_FormaPago) Then _beProductoPrime.FormaPago = dr.GetValue(_FormaPago).ToString()
                        If Not dr.IsDBNull(_FlatIncluyeRecompra) Then _beProductoPrime.FlatIncluyeRecompra = dr.GetValue(_FlatIncluyeRecompra).ToString()
                        If Not dr.IsDBNull(_FlatIncluyeCLC) Then _beProductoPrime.FlatIncluyeCLC = dr.GetValue(_FlatIncluyeCLC).ToString()
                        If Not dr.IsDBNull(_PromHorasMensualUso) Then _beProductoPrime.PromHorasMensualUso = dr.GetValue(_PromHorasMensualUso).ToString()
                        .beProductoPrime = _beProductoPrime
                        blnValido = True
                    End With

                Else
                    blnValido = False
                End If
            End With

        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function Eliminar(ByVal cnnSql As SqlConnection, ByRef obeProductoPrime As beProductoPrime) As Boolean
        blnValido = False
        Dim cmd As SqlCommand = cnnSql.CreateCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = objBBDD.StoreProcedure.uspProductoPrimeEliminar

        Try
            With obeProductoPrime
                cmd.Parameters.Add(uData.CreaParametro("@IdProducto", .IdProducto, SqlDbType.Int))
                cmd.ExecuteNonQuery()
                blnValido = True
            End With
        Catch ex As Exception
            blnValido = False
        End Try

        Return blnValido
    End Function

End Class
