Imports System.Data.SqlClient
Imports Ferreyros.BECotizador

Public Class daDetallePartes
    Private uData As New Utiles.Datos
    Public Function BuscarLlave(ByVal oConexion As String, _
                                ByVal oDetallePartes As beDetallePartes, _
                                ByRef eValidacion As beValidacion, _
                                ByRef oListaDetallePartes As List(Of beDetallePartes)) As Boolean

        Dim boolExito As Boolean = False 
         
            Using conexion As SqlConnection = New SqlConnection(oConexion)
                conexion.Open()
                Using cmd As SqlCommand = conexion.CreateCommand()
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandText = objBBDD.StoreProcedure.uspDetallePartesBuscarLlave
                    cmd.Parameters.Add(uData.CreaParametro("@Familia", oDetallePartes.Familia, SqlDbType.VarChar, 100))
                    cmd.Parameters.Add(uData.CreaParametro("@Modelo", oDetallePartes.ModeloBase, SqlDbType.VarChar, 100))
                    cmd.Parameters.Add(uData.CreaParametro("@Prefijo", oDetallePartes.Prefijo, SqlDbType.VarChar, 100))
                    cmd.Parameters.Add(uData.CreaParametro("@IncluyeFluidos", oDetallePartes.IncluyeFluidos, SqlDbType.TinyInt))
                    cmd.Parameters.Add(uData.CreaParametro("@CodPlan", oDetallePartes.CodPlan, SqlDbType.VarChar, 100))


                    Using reader As IDataReader = cmd.ExecuteReader

                        Dim _Ide As Integer = reader.GetOrdinal("Id")
                        Dim _Prefijo As String = reader.GetOrdinal("Prefijo")
                        Dim _Modelo As String = reader.GetOrdinal("Modelo")
                        Dim _ModeloBase As String = reader.GetOrdinal("ModeloBase")
                        Dim _Familia As String = reader.GetOrdinal("Familia")
                        Dim _ServiceCategory As String = reader.GetOrdinal("ServiceCategory")
                        Dim _Rodetail As String = reader.GetOrdinal("Rodetail")
                        Dim _CompQty As Decimal = reader.GetOrdinal("CompQty")
                        Dim _FirstInterval As Decimal = reader.GetOrdinal("FirstInterval")
                        Dim _NextInterval As Decimal = reader.GetOrdinal("NextInterval")
                        Dim _JODETAIL As String = reader.GetOrdinal("JODETAIL")
                        Dim _SOSPartNumber As String = reader.GetOrdinal("SOSPartNumber")
                        Dim _SOSDescription As String = reader.GetOrdinal("SOSDescription")
                        Dim _Quantity As Decimal = reader.GetOrdinal("Quantity")
                        Dim _Replacement As Decimal = reader.GetOrdinal("Replacement")
                        Dim _UnitPrice As Decimal = reader.GetOrdinal("UnitPrice")
                        Dim _ExtendedPrice As Decimal = reader.GetOrdinal("ExtendedPrice")
                        Dim _SellEvent As Decimal = reader.GetOrdinal("SellEvent")
                        Dim _Eventos As Decimal = reader.GetOrdinal("Eventos")
                        Dim _Sell As Decimal = reader.GetOrdinal("Sell")
                        Dim _Estado As Integer = reader.GetOrdinal("Estado")
                        Dim _FleetOrigin As String = reader.GetOrdinal("FleetOrigin")
                        Dim _CodPlan As String = reader.GetOrdinal("CodPlan")

                        Dim beDetallePartes As beDetallePartes
                        While reader.Read
                            beDetallePartes = New beDetallePartes
                            If Not reader.IsDBNull(_Ide) Then beDetallePartes.Ide = reader.GetValue(_Ide)
                            If Not reader.IsDBNull(_Prefijo) Then beDetallePartes.Prefijo = reader.GetValue(_Prefijo)
                            If Not reader.IsDBNull(_Modelo) Then beDetallePartes.Modelo = reader.GetValue(_Modelo)
                            If Not reader.IsDBNull(_ModeloBase) Then beDetallePartes.ModeloBase = reader.GetValue(_ModeloBase)
                            If Not reader.IsDBNull(_Familia) Then beDetallePartes.Familia = reader.GetValue(_Familia)
                            If Not reader.IsDBNull(_ServiceCategory) Then beDetallePartes.ServiceCategory = reader.GetValue(_ServiceCategory)
                            If Not reader.IsDBNull(_Rodetail) Then beDetallePartes.Rodetail = reader.GetValue(_Rodetail)
                            If Not reader.IsDBNull(_CompQty) Then beDetallePartes.CompQty = reader.GetValue(_CompQty)
                            If Not reader.IsDBNull(_FirstInterval) Then beDetallePartes.FirstInterval = reader.GetValue(_FirstInterval)
                            If Not reader.IsDBNull(_NextInterval) Then beDetallePartes.NextInterval = reader.GetValue(_NextInterval)
                            If Not reader.IsDBNull(_JODETAIL) Then beDetallePartes.JODETAIL = reader.GetValue(_JODETAIL)
                            If Not reader.IsDBNull(_SOSPartNumber) Then beDetallePartes.SOSPartNumber = reader.GetValue(_SOSPartNumber)
                            If Not reader.IsDBNull(_SOSDescription) Then beDetallePartes.SOSDescription = reader.GetValue(_SOSDescription)
                            If Not reader.IsDBNull(_Quantity) Then beDetallePartes.Quantity = reader.GetValue(_Quantity)
                            If Not reader.IsDBNull(_Replacement) Then beDetallePartes.Replacement = reader.GetValue(_Replacement)
                            If Not reader.IsDBNull(_UnitPrice) Then beDetallePartes.UnitPrice = reader.GetValue(_UnitPrice)
                            If Not reader.IsDBNull(_ExtendedPrice) Then beDetallePartes.ExtendedPrice = reader.GetValue(_ExtendedPrice)
                            If Not reader.IsDBNull(_SellEvent) Then beDetallePartes.SellEvent = reader.GetValue(_SellEvent)
                            If Not reader.IsDBNull(_Eventos) Then beDetallePartes.Eventos = reader.GetValue(_Eventos)
                            If Not reader.IsDBNull(_Sell) Then beDetallePartes.Sell = reader.GetValue(_Sell)
                            If Not reader.IsDBNull(_Estado) Then beDetallePartes.Estado = reader.GetValue(_Estado)
                            If Not reader.IsDBNull(_FleetOrigin) Then beDetallePartes.FleetOrigin = reader.GetValue(_FleetOrigin)
                            If Not reader.IsDBNull(_CodPlan) Then beDetallePartes.CodPlan = reader.GetValue(_CodPlan)


                            oListaDetallePartes.Add(beDetallePartes)

                        End While
                    End Using
                    boolExito = True
                End Using
            End Using

        Return boolExito
    End Function

End Class
