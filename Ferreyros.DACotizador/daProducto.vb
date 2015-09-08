Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.Utiles.Estructuras

Public Class daProducto

    Private blnValido As Boolean = False
    Private uData As New Utiles.Datos
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Sub ProductoListar(ByVal eConexion As String, ByVal eCotizacion As beCotizacion, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beProducto))

        Dim conexion As SqlConnection = Nothing
        Dim comand As SqlCommand = Nothing
        Dim Producto As beProducto = Nothing
        Try
            conexion = New SqlConnection(eConexion)
            conexion.Open()
            comand = conexion.CreateCommand
            comand.CommandText = objBBDD.StoreProcedure.ProductoListar
            comand.CommandType = CommandType.StoredProcedure
            comand.Parameters.Add(uData.CreaParametro("@IdCotizacionSap", eCotizacion.IdCotizacionSap, SqlDbType.VarChar, 20))
            Using dr As IDataReader = comand.ExecuteReader
                Dim _IdProducto As Integer = dr.GetOrdinal("IdProducto")
                Dim _IdPosicionSAP As Integer = dr.GetOrdinal("IdPosicionSAP")
                Dim _IdProductoSAP As Integer = dr.GetOrdinal("IdProductoSAP")
                Dim _TipoProducto As Integer = dr.GetOrdinal("TipoProducto")
                Dim _Descripcion As Integer = dr.GetOrdinal("Descripcion")
                Dim _ValorUnitario As Decimal = dr.GetOrdinal("ValorUnitario")
                Dim _IdMonedaValorUnitario As Integer = dr.GetOrdinal("IdMonedaValorUnitario")
                Dim _Cantidad As Integer = dr.GetOrdinal("Cantidad")
                Dim _Unidad As Integer = dr.GetOrdinal("Unidad")
                Dim _ValorNeto As Decimal = dr.GetOrdinal("ValorNeto")
                Dim _IdMonedaValorNeto As Integer = dr.GetOrdinal("IdMonedaValorNeto")
                Dim _NombreEstado As Integer = dr.GetOrdinal("NombreEstado")
                Dim _ValorImpuesto As Decimal = dr.GetOrdinal("ValorImpuesto")

                Dim _ValorLista As Decimal = dr.GetOrdinal("ValorLista")
                Dim _ValorReal As Decimal = dr.GetOrdinal("ValoReal")
                Dim _PorcDescuento As Decimal = dr.GetOrdinal("PorcDescuento")
                Dim _DescuentoImp As Decimal = dr.GetOrdinal("DescuentoImp")
                Dim _Flete As Decimal = dr.GetOrdinal("Flete")
                Dim _ValorVenta As Decimal = dr.GetOrdinal("ValorVenta")
                Dim _PorcImpuesto As Decimal = dr.GetOrdinal("PorcImpuesto")
                Dim _PrecioVentaFinal As Decimal = dr.GetOrdinal("PrecioVentaFinal")
                Dim _CodigoLinea As String = dr.GetOrdinal("CodigoLinea")
                Dim _NombreLinea As String = dr.GetOrdinal("NombreLinea")
                '18/09
                Dim _CodigoFamilia As String = dr.GetOrdinal("CodigoFamilia")
                '--
                '12/06
                Dim _LugarEntrega As String = dr.GetOrdinal("LugarEntrega")
                '--

                While dr.Read
                    Producto = New beProducto
                    If Not dr.IsDBNull(_IdProducto) Then Producto.IdProducto = dr.GetValue(_IdProducto)
                    If Not dr.IsDBNull(_IdPosicionSAP) Then Producto.IdPosicion = dr.GetValue(_IdPosicionSAP)
                    If Not dr.IsDBNull(_IdProductoSAP) Then Producto.IdProductoSap = dr.GetValue(_IdProductoSAP)
                    If Not dr.IsDBNull(_TipoProducto) Then Producto.TipoProducto = dr.GetValue(_TipoProducto)
                    If Not dr.IsDBNull(_Descripcion) Then Producto.Descripcion = dr.GetValue(_Descripcion)
                    If Not dr.IsDBNull(_ValorUnitario) Then Producto.ValorUnitario = dr.GetValue(_ValorUnitario)
                    If Not dr.IsDBNull(_IdMonedaValorUnitario) Then Producto.IdMonedaValorUnitario = dr.GetValue(_IdMonedaValorUnitario)
                    If Not dr.IsDBNull(_Cantidad) Then Producto.Cantidad = dr.GetValue(_Cantidad)
                    If Not dr.IsDBNull(_Unidad) Then Producto.Unidad = dr.GetValue(_Unidad)

                    If Not dr.IsDBNull(_ValorNeto) Then Producto.ValorNeto = dr.GetValue(_ValorNeto)

                    If Not dr.IsDBNull(_IdMonedaValorNeto) Then Producto.IdMonedaValorNeto = dr.GetValue(_IdMonedaValorNeto)
                    If Not dr.IsDBNull(_NombreEstado) Then Producto.NombreEstado = dr.GetValue(_NombreEstado)
                    If Not dr.IsDBNull(_ValorImpuesto) Then Producto.ValorImpuesto = dr.GetValue(_ValorImpuesto)

                    If Not dr.IsDBNull(_ValorLista) Then Producto.ValorLista = dr.GetValue(_ValorLista)
                    If Not dr.IsDBNull(_ValorReal) Then Producto.ValorReal = dr.GetValue(_ValorReal)
                    If Not dr.IsDBNull(_PorcDescuento) Then Producto.PorcDescuento = dr.GetValue(_PorcDescuento)
                    If Not dr.IsDBNull(_DescuentoImp) Then Producto.DescuentoImp = dr.GetValue(_DescuentoImp)
                    If Not dr.IsDBNull(_Flete) Then Producto.Flete = dr.GetValue(_Flete)
                    If Not dr.IsDBNull(_ValorVenta) Then Producto.ValorVenta = dr.GetValue(_ValorVenta)
                    If Not dr.IsDBNull(_PorcImpuesto) Then Producto.PorcImpuesto = dr.GetValue(_PorcImpuesto)
                    If Not dr.IsDBNull(_PrecioVentaFinal) Then Producto.PrecioVentaFinal = dr.GetValue(_PrecioVentaFinal)

                    If Not dr.IsDBNull(_CodigoLinea) Then Producto.CodigoLinea = dr.GetValue(_CodigoLinea)
                    If Not dr.IsDBNull(_NombreLinea) Then Producto.NombreLinea = dr.GetValue(_NombreLinea)
                    '12/06 CS
                    If Not dr.IsDBNull(_CodigoFamilia) Then Producto.CodigoFamilia = dr.GetValue(_CodigoFamilia)
                    '--
                    '12/06 CS
                    If Not dr.IsDBNull(_LugarEntrega) Then Producto.LugarEntrega = dr.GetValue(_LugarEntrega)
                    '--
                    lResult.Add(Producto)
                End While
            End Using
            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        Finally
            If conexion.State = ConnectionState.Open Then conexion.Close()
        End Try

    End Sub

    Public Function BuscarIdCotizacion(ByVal cnnSql As SqlConnection, ByVal obeProducto As beProducto, ByRef ListabeProducto As List(Of beProducto)) As Boolean
        Try
            blnValido = False
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = objBBDD.StoreProcedure.uspProductoBuscarIdCotizacion

            cmdSql.Parameters.Add(uData.CreaParametro("@IdCotizacion", obeProducto.IdCotizacion, SqlDbType.Int))

            Using dr As SqlDataReader = cmdSql.ExecuteReader

                Dim _idProducto As String = dr.GetOrdinal("idProducto")
                Dim _IdCotizacion As String = dr.GetOrdinal("IdCotizacion")
                Dim _IdPosicionSAP As String = dr.GetOrdinal("IdPosicionSAP")
                Dim _IdProductoSAP As String = dr.GetOrdinal("IdProductoSAP")
                Dim _Descripcion As String = dr.GetOrdinal("Descripcion")
                Dim _ValorUnitario As String = dr.GetOrdinal("ValorUnitario")
                Dim _Cantidad As String = dr.GetOrdinal("Cantidad")
                Dim _Unidad As String = dr.GetOrdinal("Unidad")
                Dim _ValorNeto As String = dr.GetOrdinal("ValorNeto")
                Dim _IdMonedaValorNeto As String = dr.GetOrdinal("IdMonedaValorNeto")
                Dim _NombreEstado As String = dr.GetOrdinal("NombreEstado")
                Dim _TipoProducto As String = dr.GetOrdinal("TipoProducto")
                Dim ebeProducto As beProducto = Nothing

                While dr.Read()
                    ebeProducto = New beProducto

                    With ebeProducto
                        If Not dr.IsDBNull(_idProducto) Then .IdProducto = dr.GetValue(_idProducto).ToString()
                        If Not dr.IsDBNull(_IdCotizacion) Then .IdCotizacion = dr.GetValue(_IdCotizacion).ToString()
                        If Not dr.IsDBNull(_IdPosicionSAP) Then .IdPosicion = dr.GetValue(_IdPosicionSAP).ToString()
                        If Not dr.IsDBNull(_IdProductoSAP) Then .IdProductoSap = dr.GetValue(_IdProductoSAP).ToString()
                        If Not dr.IsDBNull(_Descripcion) Then .Descripcion = dr.GetValue(_Descripcion).ToString()
                        If Not dr.IsDBNull(_ValorUnitario) Then .ValorUnitario = dr.GetValue(_ValorUnitario).ToString()
                        If Not dr.IsDBNull(_Cantidad) Then .Cantidad = dr.GetValue(_Cantidad).ToString()
                        If Not dr.IsDBNull(_Unidad) Then .Unidad = dr.GetValue(_Unidad).ToString()
                        If Not dr.IsDBNull(_ValorNeto) Then .ValorNeto = dr.GetValue(_ValorNeto).ToString()
                        If Not dr.IsDBNull(_IdMonedaValorNeto) Then .IdMonedaValorNeto = dr.GetValue(_IdMonedaValorNeto).ToString()
                        If Not dr.IsDBNull(_NombreEstado) Then .NombreEstado = dr.GetValue(_NombreEstado).ToString()
                        If Not dr.IsDBNull(_TipoProducto) Then .TipoProducto = dr.GetValue(_TipoProducto).ToString()

                    End With
                    ListabeProducto.Add(ebeProducto)
                End While
                dr.Close()
            End Using
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function BuscarNumeroCotizacion(ByVal cnnSql As SqlConnection, ByVal obeProducto As beProducto, ByRef ListabeProducto As List(Of beProducto)) As Boolean
        Try
            blnValido = False
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = objBBDD.StoreProcedure.uspProductoBuscarNumeroCotizacion

            cmdSql.Parameters.Add(uData.CreaParametro("@IdCotizacionSap", obeProducto.IdCotizacion, SqlDbType.VarChar, 20))

            Using dr As SqlDataReader = cmdSql.ExecuteReader

                Dim _idProducto As String = dr.GetOrdinal("idProducto")
                Dim _IdCotizacion As String = dr.GetOrdinal("IdCotizacion")
                Dim _IdCotizacionSap As String = dr.GetOrdinal("IdCotizacionSap")
                Dim _IdPosicionSAP As String = dr.GetOrdinal("IdPosicionSAP")
                Dim _IdProductoSAP As String = dr.GetOrdinal("IdProductoSAP")
                Dim _Descripcion As String = dr.GetOrdinal("Descripcion")
                Dim ebeProducto As beProducto = Nothing

                While dr.Read()
                    ebeProducto = New beProducto

                    With ebeProducto
                        If Not dr.IsDBNull(_idProducto) Then .IdProducto = dr.GetValue(_idProducto).ToString()
                        If Not dr.IsDBNull(_IdCotizacion) Then .IdCotizacion = dr.GetValue(_IdCotizacion).ToString()
                        If Not dr.IsDBNull(_IdCotizacionSap) Then .IdCotizacionSap = dr.GetValue(_IdCotizacionSap).ToString()
                        If Not dr.IsDBNull(_IdPosicionSAP) Then .IdPosicion = dr.GetValue(_IdPosicionSAP).ToString()
                        If Not dr.IsDBNull(_IdProductoSAP) Then .IdProductoSap = dr.GetValue(_IdProductoSAP).ToString()
                        If Not dr.IsDBNull(_Descripcion) Then .Descripcion = dr.GetValue(_Descripcion).ToString()

                    End With
                    ListabeProducto.Add(ebeProducto)
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
