Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros.BECotizador

Public Class daProductoAlquiler
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
            cmdSql.CommandText = objBBDD.StoreProcedure.uspProductoAlquilerBuscarId
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
                        Dim _ValorNeto As String = dr.GetOrdinal("ValorNeto")
                        Dim _IdMonedaValorNeto As String = dr.GetOrdinal("IdMonedaValorNeto")
                        Dim _NombreEstado As String = dr.GetOrdinal("NombreEstado")

                        Dim _CodTipoAlquiler As String = dr.GetOrdinal("CodTipoAlquiler")
                        Dim _DesTipoAlquiler As String = dr.GetOrdinal("DesTipoAlquiler")
                        Dim _CodTipoPago As String = dr.GetOrdinal("CodTipoPago")
                        Dim _DesTipoPago As String = dr.GetOrdinal("DesTipoPago")
                        Dim _CodTipoFacturacion As String = dr.GetOrdinal("CodTipoFacturacion")
                        Dim _DesTipoFacturacion As String = dr.GetOrdinal("DesTipoFacturacion")
                        Dim _CodMesAlquilar As String = dr.GetOrdinal("CodMesAlquilar")
                        Dim _DesMesAlquilar As String = dr.GetOrdinal("DesMesAlquilar") 
                        Dim _beProductoAlquiler As New beProductoAlquiler

                        If Not dr.IsDBNull(_IdProducto) Then .IdProducto = dr.GetValue(_IdProducto).ToString()
                        If Not dr.IsDBNull(_IdProducto) Then _beProductoAlquiler.IdProducto = dr.GetValue(_IdProducto).ToString()
                        If Not dr.IsDBNull(_IdCotizacion) Then .IdCotizacion = dr.GetValue(_IdCotizacion).ToString()
                        If Not dr.IsDBNull(_IdPosicionSAP) Then .IdPosicion = dr.GetValue(_IdPosicionSAP).ToString()
                        If Not dr.IsDBNull(_IdProductoSAP) Then .IdProductoSap = dr.GetValue(_IdProductoSAP).ToString()
                        If Not dr.IsDBNull(_TipoProducto) Then .TipoProducto = dr.GetValue(_TipoProducto).ToString()
                        If Not dr.IsDBNull(_Descripcion) Then .Descripcion = dr.GetValue(_Descripcion).ToString()
                        If Not dr.IsDBNull(_ValorUnitario) Then .ValorUnitario = dr.GetValue(_ValorUnitario).ToString()
                        If Not dr.IsDBNull(_IdMonedaValorUnitario) Then .IdMonedaValorUnitario = dr.GetValue(_IdMonedaValorUnitario).ToString()
                        If Not dr.IsDBNull(_Cantidad) Then .Cantidad = dr.GetValue(_Cantidad).ToString()
                        If Not dr.IsDBNull(_Unidad) Then .Unidad = dr.GetValue(_Unidad).ToString()
                        If Not dr.IsDBNull(_ValorNeto) Then .ValorNeto = dr.GetValue(_ValorNeto).ToString()
                        If Not dr.IsDBNull(_IdMonedaValorNeto) Then .IdMonedaValorNeto = dr.GetValue(_IdMonedaValorNeto).ToString()
                        If Not dr.IsDBNull(_NombreEstado) Then .NombreEstado = dr.GetValue(_NombreEstado).ToString()

                        If Not dr.IsDBNull(_CodTipoAlquiler) Then _beProductoAlquiler.CodTipoAlquiler = dr.GetValue(_CodTipoAlquiler).ToString()
                        If Not dr.IsDBNull(_DesTipoAlquiler) Then _beProductoAlquiler.DesTipoAlquiler = dr.GetValue(_DesTipoAlquiler).ToString()
                        If Not dr.IsDBNull(_CodTipoPago) Then _beProductoAlquiler.CodTipoPago = dr.GetValue(_CodTipoPago).ToString()
                        If Not dr.IsDBNull(_DesTipoPago) Then _beProductoAlquiler.DesTipoPago = dr.GetValue(_DesTipoPago).ToString()
                        If Not dr.IsDBNull(_CodTipoFacturacion) Then _beProductoAlquiler.CodTipoFacturacion = dr.GetValue(_CodTipoFacturacion).ToString()
                        If Not dr.IsDBNull(_DesTipoFacturacion) Then _beProductoAlquiler.DesTipoFacturacion = dr.GetValue(_DesTipoFacturacion).ToString()
                        If Not dr.IsDBNull(_CodMesAlquilar) Then _beProductoAlquiler.CodMesAlquilar = dr.GetValue(_CodMesAlquilar).ToString()
                        If Not dr.IsDBNull(_DesMesAlquilar) Then _beProductoAlquiler.DesMesAlquilar = dr.GetValue(_DesMesAlquilar).ToString()
                        .beProductoAlquiler = _beProductoAlquiler
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

End Class
