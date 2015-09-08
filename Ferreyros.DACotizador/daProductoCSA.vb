Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.Utiles.Estructuras

Public Class daProductoCSA

    Private uData As New Utiles.Datos

    Public Sub ProductoCSAListar(ByVal eConexion As String, ByVal Cotizacion As beCotizacion, ByRef Producto As beProducto, ByRef ProductoCSA As beProductoCSA, ByRef eValidacion As beValidacion)

        Dim conexion As SqlConnection = Nothing
        Dim cmdProductoCsa As SqlCommand = Nothing

        Try
            conexion = New SqlConnection(eConexion)
            conexion.Open()
            cmdProductoCsa = conexion.CreateCommand
            cmdProductoCsa.CommandText = objBBDD.StoreProcedure.ProductoCSAListar
            cmdProductoCsa.CommandType = CommandType.StoredProcedure
            cmdProductoCsa.Parameters.Add(uData.CreaParametro("@IdCotizacionSap", Cotizacion.IdCotizacionSap, SqlDbType.VarChar, 20))
            cmdProductoCsa.Parameters.Add(uData.CreaParametro("@IdPosicionSap", Producto.IdPosicion, SqlDbType.VarChar, 20))
            cmdProductoCsa.Parameters.Add(uData.CreaParametro("@IdProductoSap", Producto.IdProductoSap, SqlDbType.VarChar, 20))
            Using dr As IDataReader = cmdProductoCsa.ExecuteReader
                Dim IdProducto As Integer = dr.GetOrdinal("IdProducto")
                Dim TipoProducto As Integer = dr.GetOrdinal("TipoProducto")
                Dim Descripcion As Integer = dr.GetOrdinal("Descripcion")
                Dim ValorUnitario As Integer = dr.GetOrdinal("ValorUnitario")
                Dim IdMonedaValorUnitario As Integer = dr.GetOrdinal("IdMonedaValorUnitario")
                Dim Cantidad As Integer = dr.GetOrdinal("Cantidad")
                Dim Unidad As Integer = dr.GetOrdinal("Unidad")
                Dim ValorNeto As Integer = dr.GetOrdinal("ValorNeto")
                Dim IdMonedaValorNeto As Integer = dr.GetOrdinal("IdMonedaValorNeto")
                Dim NombreEstado As Integer = dr.GetOrdinal("NombreEstado")
                Dim ClaseCsa As Integer = dr.GetOrdinal("ClaseCsa")
                Dim TipoCotizacion As Integer = dr.GetOrdinal("TipoCotizacion")
                Dim IdTipoCSA As Integer = dr.GetOrdinal("IdTipoCSA")
                Dim IdPlan As Integer = dr.GetOrdinal("IdPlan")
                Dim DescripcionPlan As Integer = dr.GetOrdinal("DescripcionPlan")
                Dim IdUnidadDuracion As Integer = dr.GetOrdinal("IdUnidadDuracion")
                Dim Duracion As Integer = dr.GetOrdinal("Duracion")
                Dim Tiempo As Integer = dr.GetOrdinal("Tiempo")
                Dim IdUnidadPlazoRenovacion As Integer = dr.GetOrdinal("IdUnidadPlazoRenovacion")
                Dim PlazoRenovacion As Integer = dr.GetOrdinal("PlazoRenovacion")
                Dim IncluyeFluidos As Integer = dr.GetOrdinal("IncluyeFluidos")
                Dim IncluyeDetallePartes As Integer = dr.GetOrdinal("IncluyeDetallePartes")
                Dim FechaInicioContrato As Integer = dr.GetOrdinal("FechaInicioContrato")
                Dim FechaEstimadaCierre As Integer = dr.GetOrdinal("FechaEstimadaCierre")
                Dim ParticipacionVendedor1 As Integer = dr.GetOrdinal("ParticipacionVendedor1")
                Dim ParticipacionVendedor2 As Integer = dr.GetOrdinal("ParticipacionVendedor2")
                While dr.Read
                    If Not dr.IsDBNull(IdProducto) Then Producto.IdProducto = dr.GetValue(IdProducto)
                    If Not dr.IsDBNull(TipoProducto) Then Producto.TipoProducto = dr.GetValue(TipoProducto)
                    If Not dr.IsDBNull(Descripcion) Then Producto.Descripcion = dr.GetValue(Descripcion)
                    If Not dr.IsDBNull(ValorUnitario) Then Producto.ValorUnitario = dr.GetValue(ValorUnitario)
                    If Not dr.IsDBNull(IdMonedaValorUnitario) Then Producto.IdMonedaValorUnitario = dr.GetValue(IdMonedaValorUnitario)
                    If Not dr.IsDBNull(Cantidad) Then Producto.Cantidad = dr.GetValue(Cantidad)
                    If Not dr.IsDBNull(Unidad) Then Producto.Unidad = dr.GetValue(Unidad)
                    If Not dr.IsDBNull(ValorNeto) Then Producto.ValorNeto = dr.GetValue(ValorNeto)
                    If Not dr.IsDBNull(IdMonedaValorNeto) Then Producto.IdMonedaValorNeto = dr.GetValue(IdMonedaValorNeto)
                    If Not dr.IsDBNull(NombreEstado) Then Producto.NombreEstado = dr.GetValue(NombreEstado)
                    If Not dr.IsDBNull(ClaseCsa) Then ProductoCSA.ClaseCsa = dr.GetValue(ClaseCsa)
                    If Not dr.IsDBNull(TipoCotizacion) Then ProductoCSA.TipoCotizacion = dr.GetValue(TipoCotizacion)
                    If Not dr.IsDBNull(IdTipoCSA) Then ProductoCSA.IdTipoCSA = dr.GetValue(IdTipoCSA)
                    If Not dr.IsDBNull(IdPlan) Then ProductoCSA.IdPlan = dr.GetValue(IdPlan)
                    If Not dr.IsDBNull(DescripcionPlan) Then ProductoCSA.DescripcionPlan = dr.GetValue(DescripcionPlan)
                    If Not dr.IsDBNull(IdUnidadDuracion) Then ProductoCSA.IdUnidadDuracion = dr.GetValue(IdUnidadDuracion)
                    If Not dr.IsDBNull(Duracion) Then ProductoCSA.Duracion = dr.GetValue(Duracion)
                    If Not dr.IsDBNull(Tiempo) Then ProductoCSA.Tiempo = dr.GetValue(Tiempo)
                    If Not dr.IsDBNull(IdUnidadPlazoRenovacion) Then ProductoCSA.IdUnidadPlazoRenovacion = dr.GetValue(IdUnidadPlazoRenovacion)
                    If Not dr.IsDBNull(PlazoRenovacion) Then ProductoCSA.PlazoRenovacion = dr.GetValue(PlazoRenovacion)
                    If Not dr.IsDBNull(IncluyeFluidos) Then ProductoCSA.IncluyeFluidos = dr.GetValue(IncluyeFluidos)
                    If Not dr.IsDBNull(IncluyeDetallePartes) Then ProductoCSA.IncluyeDetallePartes = dr.GetValue(IncluyeDetallePartes)
                    If Not dr.IsDBNull(FechaInicioContrato) Then ProductoCSA.FechaInicioContrato = dr.GetValue(FechaInicioContrato)
                    If Not dr.IsDBNull(FechaEstimadaCierre) Then ProductoCSA.FechaEstimadaCierre = dr.GetValue(FechaEstimadaCierre)
                    If Not dr.IsDBNull(ParticipacionVendedor1) Then ProductoCSA.ParticipacionVendedor1 = dr.GetValue(ParticipacionVendedor1)
                    If Not dr.IsDBNull(ParticipacionVendedor2) Then ProductoCSA.ParticipacionVendedor2 = dr.GetValue(ParticipacionVendedor2)
                End While
            End Using

            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        Finally
            If conexion.State = ConnectionState.Open Then conexion.Close()
        End Try

    End Sub

End Class
