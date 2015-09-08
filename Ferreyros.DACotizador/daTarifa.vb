Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports log4net

Public Class daTarifa

    Private uData As New Utiles.Datos
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(daTarifa))

    Public Sub ListarTarifas(ByVal oConexion As String, _
                               ByVal oTarifa As beTarifa, _
                               ByRef oValidacion As beValidacion, _
                               ByRef oDetalle As List(Of beTarifa))

        Try
            Using cnnSql As New SqlConnection(oConexion)
                cnnSql.Open()
                Dim cmdSql As SqlCommand = cnnSql.CreateCommand
                cmdSql.CommandText = objBBDD.StoreProcedure.TarifaBuscar
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.Parameters.Add(uData.CreaParametro("@Prefijo", oTarifa.prefijo, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@Modelo", oTarifa.modelo, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@ModeloBase", oTarifa.modeloBase, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@Familia", oTarifa.familia, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@Plan", oTarifa.plan, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@Evento", oTarifa.evento, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@KitRepuestos", oTarifa.kitRepuestos, SqlDbType.Float, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@Fluidos", oTarifa.fluidos, SqlDbType.Float, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@ServicioContratado", oTarifa.servicioContratado, SqlDbType.Float, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@SOS", oTarifa.SOS, SqlDbType.Float, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@Total", oTarifa.total, SqlDbType.Float, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@EventosNueva", oTarifa.eventosNueva, SqlDbType.Float, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@EventosUsada", oTarifa.eventosUsada, SqlDbType.Float, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@KitRepuestosT", oTarifa.kitRepuestosT, SqlDbType.Float, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@FluidosT", oTarifa.fluidosT, SqlDbType.Float, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@ServicioContratadoT", oTarifa.servicioContratadoT, SqlDbType.Float, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@TotalT", oTarifa.totalT, SqlDbType.Float, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@TarifaUSDxH", oTarifa.tarifaUSDxH, SqlDbType.Float, 10))
                cmdSql.Parameters.Add(uData.CreaParametro("@SortColumn", oValidacion.sortColumn, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@SortOrder", oValidacion.sortOrder, SqlDbType.VarChar, 20))
                Using dr As IDataReader = cmdSql.ExecuteReader

                    Dim _Id As Integer = dr.GetOrdinal("Id")
                    Dim _Prefijo As Integer = dr.GetOrdinal("Prefijo")
                    Dim _Modelo As Integer = dr.GetOrdinal("Modelo")
                    Dim _ModeloBase As Integer = dr.GetOrdinal("ModeloBase")
                    Dim _Familia As Integer = dr.GetOrdinal("Familia")
                    Dim _CodPlan As Integer = dr.GetOrdinal("CodPlan")
                    Dim _Plan As Integer = dr.GetOrdinal("Plan")
                    Dim _Evento As Integer = dr.GetOrdinal("Evento")
                    Dim _ConFluidos As Integer = dr.GetOrdinal("ConFluidos")
                    Dim _Aceites As Integer = dr.GetOrdinal("Aceites")
                    Dim _KitRepuestos As Integer = dr.GetOrdinal("KitRepuestos")
                    Dim _Fluidos As Integer = dr.GetOrdinal("Fluidos")
                    Dim _ServicioContratado As Integer = dr.GetOrdinal("ServicioContratado")
                    Dim _SOS As Integer = dr.GetOrdinal("SOS")
                    Dim _Total As Integer = dr.GetOrdinal("Total")
                    Dim _EventosNueva As Integer = dr.GetOrdinal("EventosNueva")
                    Dim _EventosUsada As Integer = dr.GetOrdinal("EventosUsada")
                    Dim _KitRepuestosT As Integer = dr.GetOrdinal("KitRepuestosT")
                    Dim _FluidosT As Integer = dr.GetOrdinal("FluidosT")
                    Dim _ServicioContratadoT As Integer = dr.GetOrdinal("ServicioContratadoT")
                    Dim _TotalT As Integer = dr.GetOrdinal("TotalT")
                    Dim _TarifaUSDxH As Integer = dr.GetOrdinal("TarifaUSDxH")

                    Dim Rpt As beTarifa = Nothing
                    While dr.Read

                        Rpt = New beTarifa

                        If Not dr.IsDBNull(_Id) Then Rpt.id = dr.GetValue(_Id)
                        If Not dr.IsDBNull(_Prefijo) Then Rpt.prefijo = dr.GetValue(_Prefijo)
                        If Not dr.IsDBNull(_Modelo) Then Rpt.modelo = dr.GetValue(_Modelo)
                        If Not dr.IsDBNull(_ModeloBase) Then Rpt.modeloBase = dr.GetValue(_ModeloBase)
                        If Not dr.IsDBNull(_Familia) Then Rpt.familia = dr.GetValue(_Familia)
                        If Not dr.IsDBNull(_CodPlan) Then Rpt.codigoPlan = dr.GetValue(_CodPlan)
                        If Not dr.IsDBNull(_Plan) Then Rpt.plan = dr.GetValue(_Plan)
                        If Not dr.IsDBNull(_Evento) Then Rpt.evento = dr.GetValue(_Evento)
                        If Not dr.IsDBNull(_ConFluidos) Then Rpt.conFluidos = dr.GetValue(_ConFluidos)
                        If Not dr.IsDBNull(_Aceites) Then Rpt.aceites = dr.GetValue(_Aceites)
                        If Not dr.IsDBNull(_KitRepuestos) Then Rpt.kitRepuestos = dr.GetValue(_KitRepuestos)
                        If Not dr.IsDBNull(_Fluidos) Then Rpt.fluidos = dr.GetValue(_Fluidos)
                        If Not dr.IsDBNull(_ServicioContratado) Then Rpt.servicioContratado = dr.GetValue(_ServicioContratado)
                        If Not dr.IsDBNull(_SOS) Then Rpt.SOS = dr.GetValue(_SOS)
                        If Not dr.IsDBNull(_Total) Then Rpt.total = dr.GetValue(_Total)
                        If Not dr.IsDBNull(_EventosNueva) Then Rpt.eventosNueva = dr.GetValue(_EventosNueva)
                        If Not dr.IsDBNull(_EventosUsada) Then Rpt.eventosUsada = dr.GetValue(_EventosUsada)
                        If Not dr.IsDBNull(_KitRepuestosT) Then Rpt.kitRepuestosT = dr.GetValue(_KitRepuestosT)
                        If Not dr.IsDBNull(_FluidosT) Then Rpt.fluidosT = dr.GetValue(_FluidosT)
                        If Not dr.IsDBNull(_ServicioContratadoT) Then Rpt.servicioContratadoT = dr.GetValue(_ServicioContratadoT)
                        If Not dr.IsDBNull(_TotalT) Then Rpt.totalT = dr.GetValue(_TotalT)
                        If Not dr.IsDBNull(_TarifaUSDxH) Then Rpt.tarifaUSDxH = dr.GetValue(_TarifaUSDxH)

                        oDetalle.Add(Rpt)
                    End While
                    dr.Close()
                End Using

            End Using
            oValidacion.validacion = True
        Catch ex As Exception
            oValidacion.mensaje = ex.Message
            log.Error(oValidacion.cadenaAleatoria + ": " + oValidacion.mensaje)
        End Try

    End Sub

    Public Sub MantenimientoTarifas(ByVal oConexion As String, _
                            ByVal oTarifa As beTarifa, _
                            ByRef oValidacion As beValidacion)

        If String.IsNullOrEmpty(oConexion) OrElse oTarifa Is Nothing Then
            Return
        End If

        Try
            Using cnnSql As New SqlConnection(oConexion)
                cnnSql.Open()
                Dim cmdSql As SqlCommand = cnnSql.CreateCommand
                cmdSql.CommandText = objBBDD.StoreProcedure.TarifaMantenimiento
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.Parameters.Add(uData.CreaParametro("@Tipo", oValidacion.flag, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@Id", oTarifa.id, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@Prefijo", oTarifa.prefijo, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@Modelo", oTarifa.modelo, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@ModeloBase", oTarifa.modeloBase, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@Familia", oTarifa.familia, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@CodPlan", oTarifa.codigoPlan, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@Plan", oTarifa.plan, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@Evento", oTarifa.evento, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@ConFluidos", oTarifa.conFluidos, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@Aceites", oTarifa.aceites, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@KitRepuestos", oTarifa.kitRepuestos, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@Fluidos", oTarifa.fluidos, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@ServicioContratado", oTarifa.servicioContratado, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@SOS", oTarifa.SOS, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@Total", oTarifa.total, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@EventosNueva", oTarifa.eventosNueva, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@EventosUsada", oTarifa.eventosUsada, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@KitRepuestosT", oTarifa.kitRepuestosT, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@FluidosT", oTarifa.fluidosT, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@ServicioContratadoT", oTarifa.servicioContratadoT, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@TotalT", oTarifa.totalT, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@TarifaUSDxH", oTarifa.tarifaUSDxH, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@Usuario", oValidacion.usuario, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@Return", String.Empty, SqlDbType.Int, , ParameterDirection.ReturnValue))

                cmdSql.ExecuteNonQuery()

                oValidacion.respuesta = CType(cmdSql.Parameters("@Return").Value, Integer)
                If oValidacion.respuesta = 0 Then oValidacion.validacion = True

            End Using

        Catch ex As Exception
            oValidacion.mensaje = ex.Message
            log.Error(oValidacion.cadenaAleatoria + ": " + oValidacion.mensaje)
        End Try

    End Sub

    Public Sub ListarDetallePartes(ByVal oConexion As String, _
                            ByVal oTarifa As beTarifa, _
                            ByRef oValidacion As beValidacion, _
                            ByRef oDetalle As List(Of beTarifa))

        Try
            Using cnnSql As New SqlConnection(oConexion)
                cnnSql.Open()
                Dim cmdSql As SqlCommand = cnnSql.CreateCommand
                cmdSql.CommandText = objBBDD.StoreProcedure.DetallePartesBuscar
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.Parameters.Add(uData.CreaParametro("@Prefijo", oTarifa.prefijo, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@Modelo", oTarifa.modelo, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@ModeloBase", oTarifa.modeloBase, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@Familia", oTarifa.familia, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@ServiceCategory", oTarifa.serviceCategory, SqlDbType.VarChar, 500))
                cmdSql.Parameters.Add(uData.CreaParametro("@Rodetail", oTarifa.rodetail, SqlDbType.VarChar, 500))
                cmdSql.Parameters.Add(uData.CreaParametro("@CompQty", oTarifa.compQty, SqlDbType.Float, 15))
                cmdSql.Parameters.Add(uData.CreaParametro("@FirstInterval", oTarifa.firstInterval, SqlDbType.Float, 15))
                cmdSql.Parameters.Add(uData.CreaParametro("@NextInterval", oTarifa.nextInterval, SqlDbType.Float, 15))
                cmdSql.Parameters.Add(uData.CreaParametro("@JODETAIL", oTarifa.jodetail, SqlDbType.VarChar, 500))
                cmdSql.Parameters.Add(uData.CreaParametro("@SOSPartNumber", oTarifa.SOSPartNumber, SqlDbType.VarChar, 500))
                cmdSql.Parameters.Add(uData.CreaParametro("@SOSDescription", oTarifa.SOSDescription, SqlDbType.VarChar, 500))
                cmdSql.Parameters.Add(uData.CreaParametro("@Quantity", oTarifa.quantity, SqlDbType.Float, 15))
                cmdSql.Parameters.Add(uData.CreaParametro("@Replacement", oTarifa.replacement, SqlDbType.Float, 15))
                cmdSql.Parameters.Add(uData.CreaParametro("@UnitPrice", oTarifa.unitPrice, SqlDbType.Float, 15))
                cmdSql.Parameters.Add(uData.CreaParametro("@ExtendedPrice", oTarifa.extendedPrice, SqlDbType.Float, 15))
                cmdSql.Parameters.Add(uData.CreaParametro("@SellEvent", oTarifa.sellEvent, SqlDbType.Float, 15))
                cmdSql.Parameters.Add(uData.CreaParametro("@Eventos", oTarifa.evento, SqlDbType.Float, 15))
                cmdSql.Parameters.Add(uData.CreaParametro("@Sell", oTarifa.sell, SqlDbType.Float, 15))
                cmdSql.Parameters.Add(uData.CreaParametro("@SortColumn", oValidacion.sortColumn, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@SortOrder", oValidacion.sortOrder, SqlDbType.VarChar, 20))


                Using dr As IDataReader = cmdSql.ExecuteReader

                    Dim _Id As Integer = dr.GetOrdinal("Id")
                    Dim _Prefijo As Integer = dr.GetOrdinal("Prefijo")
                    Dim _Modelo As Integer = dr.GetOrdinal("Modelo")
                    Dim _ModeloBase As Integer = dr.GetOrdinal("ModeloBase")
                    Dim _Familia As Integer = dr.GetOrdinal("Familia")
                    Dim _ServiceCategory As Integer = dr.GetOrdinal("ServiceCategory")
                    Dim _Rodetail As Integer = dr.GetOrdinal("Rodetail")
                    Dim _CompQty As Integer = dr.GetOrdinal("CompQty")
                    Dim _FirstInterval As Integer = dr.GetOrdinal("FirstInterval")
                    Dim _NextInterval As Integer = dr.GetOrdinal("NextInterval")
                    Dim _JODETAIL As Integer = dr.GetOrdinal("JODETAIL")
                    Dim _SOSPartNumber As Integer = dr.GetOrdinal("SOSPartNumber")
                    Dim _SOSDescription As Integer = dr.GetOrdinal("SOSDescription")
                    Dim _Quantity As Integer = dr.GetOrdinal("Quantity")
                    Dim _Replacement As Integer = dr.GetOrdinal("Replacement")
                    Dim _UnitPrice As Integer = dr.GetOrdinal("UnitPrice")
                    Dim _ExtendedPrice As Integer = dr.GetOrdinal("ExtendedPrice")
                    Dim _SellEvent As Integer = dr.GetOrdinal("SellEvent")
                    Dim _Eventos As Integer = dr.GetOrdinal("Eventos")
                    Dim _Sell As Integer = dr.GetOrdinal("Sell")

                    Dim Rpt As beTarifa = Nothing
                    While dr.Read

                        Rpt = New beTarifa

                        If Not dr.IsDBNull(_Id) Then Rpt.id = dr.GetValue(_Id)
                        If Not dr.IsDBNull(_Prefijo) Then Rpt.prefijo = dr.GetValue(_Prefijo)
                        If Not dr.IsDBNull(_Modelo) Then Rpt.modelo = dr.GetValue(_Modelo)
                        If Not dr.IsDBNull(_ModeloBase) Then Rpt.modeloBase = dr.GetValue(_ModeloBase)
                        If Not dr.IsDBNull(_Familia) Then Rpt.familia = dr.GetValue(_Familia)
                        If Not dr.IsDBNull(_ServiceCategory) Then Rpt.serviceCategory = dr.GetValue(_ServiceCategory)
                        If Not dr.IsDBNull(_Rodetail) Then Rpt.rodetail = dr.GetValue(_Rodetail)
                        If Not dr.IsDBNull(_CompQty) Then Rpt.compQty = dr.GetValue(_CompQty)
                        If Not dr.IsDBNull(_FirstInterval) Then Rpt.firstInterval = dr.GetValue(_FirstInterval)
                        If Not dr.IsDBNull(_NextInterval) Then Rpt.nextInterval = dr.GetValue(_NextInterval)
                        If Not dr.IsDBNull(_JODETAIL) Then Rpt.jodetail = dr.GetValue(_JODETAIL)
                        If Not dr.IsDBNull(_SOSPartNumber) Then Rpt.SOSPartNumber = dr.GetValue(_SOSPartNumber)
                        If Not dr.IsDBNull(_SOSDescription) Then Rpt.SOSDescription = dr.GetValue(_SOSDescription)
                        If Not dr.IsDBNull(_Quantity) Then Rpt.quantity = dr.GetValue(_Quantity)
                        If Not dr.IsDBNull(_Replacement) Then Rpt.replacement = dr.GetValue(_Replacement)
                        If Not dr.IsDBNull(_UnitPrice) Then Rpt.unitPrice = dr.GetValue(_UnitPrice)
                        If Not dr.IsDBNull(_ExtendedPrice) Then Rpt.extendedPrice = dr.GetValue(_ExtendedPrice)
                        If Not dr.IsDBNull(_SellEvent) Then Rpt.sellEvent = dr.GetValue(_SellEvent)
                        If Not dr.IsDBNull(_Eventos) Then Rpt.evento = dr.GetValue(_Eventos)
                        If Not dr.IsDBNull(_Sell) Then Rpt.sell = dr.GetValue(_Sell)

                        oDetalle.Add(Rpt)
                    End While
                    dr.Close()
                End Using

            End Using
            oValidacion.validacion = True
        Catch ex As Exception
            oValidacion.mensaje = ex.Message
            log.Error(oValidacion.cadenaAleatoria + ": " + oValidacion.mensaje)
        End Try

    End Sub

    Public Sub MantenimientoDetallePartes(ByVal oConexion As String, ByVal oPartes As beTarifa, ByRef oValidacion As beValidacion)

        If String.IsNullOrEmpty(oConexion) OrElse oPartes Is Nothing Then
            Return
        End If

        Try
            Using cnnSql As New SqlConnection(oConexion)
                cnnSql.Open()
                Dim cmdSql As SqlCommand = cnnSql.CreateCommand
                cmdSql.CommandText = objBBDD.StoreProcedure.DetallePartesMantenimiento
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.Parameters.Add(uData.CreaParametro("@Tipo", oValidacion.flag, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@Id", oPartes.id, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@Prefijo", oPartes.prefijo, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@Modelo", oPartes.modelo, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@ModeloBase", oPartes.modeloBase, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@Familia", oPartes.familia, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@ServiceCategory", oPartes.serviceCategory, SqlDbType.VarChar, 500))
                cmdSql.Parameters.Add(uData.CreaParametro("@Rodetail", oPartes.rodetail, SqlDbType.VarChar, 500))
                cmdSql.Parameters.Add(uData.CreaParametro("@CompQty", oPartes.compQty, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@FirstInterval", oPartes.firstInterval, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@NextInterval", oPartes.nextInterval, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@JODETAIL", oPartes.jodetail, SqlDbType.VarChar, 500))
                cmdSql.Parameters.Add(uData.CreaParametro("@SOSPartNumber", oPartes.SOSPartNumber, SqlDbType.VarChar, 500))
                cmdSql.Parameters.Add(uData.CreaParametro("@SOSDescription", oPartes.SOSDescription, SqlDbType.VarChar, 500))
                cmdSql.Parameters.Add(uData.CreaParametro("@Quantity", oPartes.quantity, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@Replacement", oPartes.replacement, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@UnitPrice", oPartes.unitPrice, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@ExtendedPrice", oPartes.extendedPrice, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@SellEvent", oPartes.sellEvent, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@Eventos", oPartes.evento, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@Sell", oPartes.sell, SqlDbType.Float))
                cmdSql.Parameters.Add(uData.CreaParametro("@Usuario", oValidacion.usuario, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@Return", String.Empty, SqlDbType.Int, , ParameterDirection.ReturnValue))

                cmdSql.ExecuteNonQuery()

                oValidacion.respuesta = CType(cmdSql.Parameters("@Return").Value, Integer)
                If oValidacion.respuesta = 0 Then oValidacion.validacion = True

            End Using

        Catch ex As Exception
            oValidacion.mensaje = ex.Message
            log.Error(oValidacion.cadenaAleatoria + ": " + oValidacion.mensaje)
        End Try

    End Sub

    Public Sub ListarModBaseCodPlanPrefPM(ByVal oConexion As String, _
                                           ByVal oTarifa As beTarifa, _
                               ByRef oValidacion As beValidacion, _
                               ByRef oDetalle As List(Of beTarifa))

        Try
            Using cnnSql As New SqlConnection(oConexion)
                cnnSql.Open()
                Using cmdSql As SqlCommand = cnnSql.CreateCommand
                    cmdSql.CommandText = objBBDD.StoreProcedure.TarifaCsaModBPlanPrefPM
                    cmdSql.CommandType = CommandType.StoredProcedure
                    cmdSql.Parameters.Add(uData.CreaParametro("@Tipo", oValidacion.flag, SqlDbType.Int, 100))
                    cmdSql.Parameters.Add(uData.CreaParametro("@Clave", oTarifa.llave, SqlDbType.VarChar, 100))

                    Using dr As IDataReader = cmdSql.ExecuteReader

                        Dim _Prefijo As Integer = dr.GetOrdinal("Prefijo")
                        Dim _Modelo As Integer = dr.GetOrdinal("Modelo")
                        Dim _ModeloBase As Integer = dr.GetOrdinal("ModeloBase")
                        Dim _Familia As Integer = dr.GetOrdinal("Familia")
                        Dim _CodPlan As Integer = dr.GetOrdinal("CodPlan")
                        Dim _Plan As Integer = dr.GetOrdinal("Plan")
                        Dim _Evento As Integer = dr.GetOrdinal("Evento")
                        Dim _ConFluidos As Integer = dr.GetOrdinal("ConFluidos")
                        Dim _Aceites As Integer = dr.GetOrdinal("Aceites")
                        Dim _KitRepuestos As Integer = dr.GetOrdinal("KitRepuestos")
                        Dim _Fluidos As Integer = dr.GetOrdinal("Fluidos")
                        Dim _ServicioContratado As Integer = dr.GetOrdinal("ServicioContratado")
                        Dim _SOS As Integer = dr.GetOrdinal("SOS")
                        Dim _Total As Integer = dr.GetOrdinal("Total")
                        Dim _EventosNueva As Integer = dr.GetOrdinal("EventosNueva")
                        Dim _EventosUsada As Integer = dr.GetOrdinal("EventosUsada")
                        Dim _KitRepuestosT As Integer = dr.GetOrdinal("KitRepuestosT")
                        Dim _FluidosT As Integer = dr.GetOrdinal("FluidosT")
                        Dim _ServicioContratadoT As Integer = dr.GetOrdinal("ServicioContratadoT")
                        Dim _TotalT As Integer = dr.GetOrdinal("TotalT")
                        Dim _TarifaUSDxH As Integer = dr.GetOrdinal("TarifaUSDxH")

                        Dim Rpt As beTarifa = Nothing
                        While dr.Read

                            Rpt = New beTarifa

                            If Not dr.IsDBNull(_Prefijo) Then Rpt.prefijo = dr.GetValue(_Prefijo)
                            If Not dr.IsDBNull(_Modelo) Then Rpt.modelo = dr.GetValue(_Modelo)
                            If Not dr.IsDBNull(_ModeloBase) Then Rpt.modeloBase = dr.GetValue(_ModeloBase)
                            If Not dr.IsDBNull(_Familia) Then Rpt.familia = dr.GetValue(_Familia)
                            If Not dr.IsDBNull(_CodPlan) Then Rpt.codigoPlan = dr.GetValue(_CodPlan)
                            If Not dr.IsDBNull(_Plan) Then Rpt.plan = dr.GetValue(_Plan)
                            If Not dr.IsDBNull(_Evento) Then Rpt.evento = dr.GetValue(_Evento)
                            If Not dr.IsDBNull(_ConFluidos) Then Rpt.conFluidos = dr.GetValue(_ConFluidos)
                            If Not dr.IsDBNull(_Aceites) Then Rpt.aceites = dr.GetValue(_Aceites)
                            If Not dr.IsDBNull(_KitRepuestos) Then Rpt.kitRepuestos = dr.GetValue(_KitRepuestos)
                            If Not dr.IsDBNull(_Fluidos) Then Rpt.fluidos = dr.GetValue(_Fluidos)
                            If Not dr.IsDBNull(_ServicioContratado) Then Rpt.servicioContratado = dr.GetValue(_ServicioContratado)
                            If Not dr.IsDBNull(_SOS) Then Rpt.SOS = dr.GetValue(_SOS)
                            If Not dr.IsDBNull(_Total) Then Rpt.total = dr.GetValue(_Total)
                            If Not dr.IsDBNull(_EventosNueva) Then Rpt.eventosNueva = dr.GetValue(_EventosNueva)
                            If Not dr.IsDBNull(_EventosUsada) Then Rpt.eventosUsada = dr.GetValue(_EventosUsada)
                            If Not dr.IsDBNull(_KitRepuestosT) Then Rpt.kitRepuestosT = dr.GetValue(_KitRepuestosT)
                            If Not dr.IsDBNull(_FluidosT) Then Rpt.fluidosT = dr.GetValue(_FluidosT)
                            If Not dr.IsDBNull(_ServicioContratadoT) Then Rpt.servicioContratadoT = dr.GetValue(_ServicioContratadoT)
                            If Not dr.IsDBNull(_TotalT) Then Rpt.totalT = dr.GetValue(_TotalT)
                            If Not dr.IsDBNull(_TarifaUSDxH) Then Rpt.tarifaUSDxH = dr.GetValue(_TarifaUSDxH)

                            oDetalle.Add(Rpt)
                        End While
                        dr.Close()
                    End Using
                End Using
            End Using
            oValidacion.validacion = True
        Catch ex As Exception
            oValidacion.mensaje = ex.Message
        End Try
    End Sub

    Public Sub ListarModeloCodPlanEvento(ByVal oConexion As String, ByVal oTarifa As beTarifa, ByRef oValidacion As beValidacion, ByRef oDetalle As List(Of beTarifa))

        Try
            Using cnnSql As New SqlConnection(oConexion)
                cnnSql.Open()
                Dim cmdSql As SqlCommand = cnnSql.CreateCommand
                cmdSql.CommandText = objBBDD.StoreProcedure.TarifaModeloPlanEvento
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.Parameters.Add(uData.CreaParametro("@Clave", oTarifa.llave, SqlDbType.VarChar, 100))

                Using dr As IDataReader = cmdSql.ExecuteReader

                    Dim _Familia As Integer = dr.GetOrdinal("Familia")
                    Dim _ModeloBase As Integer = dr.GetOrdinal("ModeloBase")
                    Dim _Modelo As Integer = dr.GetOrdinal("Modelo")
                    Dim _Prefijo As Integer = dr.GetOrdinal("Prefijo")
                    Dim _PlanMantenimiento As Integer = dr.GetOrdinal("Plan")
                    Dim _CodPlan As Integer = dr.GetOrdinal("CodPlan")
                    Dim _Evento As Integer = dr.GetOrdinal("Evento")
                    Dim _KitRepuestos As Integer = dr.GetOrdinal("KitRepuestos")
                    Dim _Fluidos As Integer = dr.GetOrdinal("Fluidos")
                    Dim _ServicioContratado As Integer = dr.GetOrdinal("ServicioContratado")
                    Dim _SOS As Integer = dr.GetOrdinal("SOS")
                    Dim _Total As Integer = dr.GetOrdinal("Total")
                    Dim _EventosNuevos As Integer = dr.GetOrdinal("EventosNueva")
                    Dim _EventosUsados As Integer = dr.GetOrdinal("EventosUsada")
                    Dim _TarifaUSDxH As Integer = dr.GetOrdinal("TarifaUSDxH")
                    Dim _KitRepuestosT As Integer = dr.GetOrdinal("KitRepuestosT")
                    Dim _FluidosT As Integer = dr.GetOrdinal("FluidosT")
                    Dim _ServicioContratadoT As Integer = dr.GetOrdinal("ServicioContratadoT")
                    Dim _TotalT As Integer = dr.GetOrdinal("TotalT")
                    Dim _ConFluidos As Integer = dr.GetOrdinal("ConFluidos")

                    Dim Rpt As beTarifa = Nothing
                    While dr.Read

                        Rpt = New beTarifa

                        If Not dr.IsDBNull(_Familia) Then Rpt.familia = dr.GetValue(_Familia)
                        If Not dr.IsDBNull(_ModeloBase) Then Rpt.modeloBase = dr.GetValue(_ModeloBase)
                        If Not dr.IsDBNull(_Modelo) Then Rpt.modelo = dr.GetValue(_Modelo)
                        If Not dr.IsDBNull(_Prefijo) Then Rpt.prefijo = dr.GetValue(_Prefijo)
                        If Not dr.IsDBNull(_PlanMantenimiento) Then Rpt.plan = dr.GetValue(_PlanMantenimiento)
                        If Not dr.IsDBNull(_CodPlan) Then Rpt.codigoPlan = dr.GetValue(_CodPlan)
                        If Not dr.IsDBNull(_Evento) Then Rpt.evento = dr.GetValue(_Evento)
                        If Not dr.IsDBNull(_KitRepuestos) Then Rpt.kitRepuestos = dr.GetValue(_KitRepuestos)
                        If Not dr.IsDBNull(_Fluidos) Then Rpt.fluidos = dr.GetValue(_Fluidos)
                        If Not dr.IsDBNull(_ServicioContratado) Then Rpt.servicioContratado = dr.GetValue(_ServicioContratado)
                        If Not dr.IsDBNull(_SOS) Then Rpt.SOS = dr.GetValue(_SOS)
                        If Not dr.IsDBNull(_Total) Then Rpt.total = dr.GetValue(_Total)
                        If Not dr.IsDBNull(_EventosNuevos) Then Rpt.eventosNueva = dr.GetValue(_EventosNuevos)
                        If Not dr.IsDBNull(_EventosUsados) Then Rpt.eventosUsada = dr.GetValue(_EventosUsados)
                        If Not dr.IsDBNull(_TarifaUSDxH) Then Rpt.tarifaUSDxH = dr.GetValue(_TarifaUSDxH)
                        If Not dr.IsDBNull(_KitRepuestosT) Then Rpt.kitRepuestosT = dr.GetValue(_KitRepuestosT)
                        If Not dr.IsDBNull(_FluidosT) Then Rpt.fluidosT = dr.GetValue(_FluidosT)
                        If Not dr.IsDBNull(_ServicioContratadoT) Then Rpt.servicioContratadoT = dr.GetValue(_ServicioContratadoT)
                        If Not dr.IsDBNull(_TotalT) Then Rpt.totalT = dr.GetValue(_TotalT)
                        If Not dr.IsDBNull(_ConFluidos) Then Rpt.ConFluidos = dr.GetValue(_ConFluidos)

                        oDetalle.Add(Rpt)
                    End While
                    dr.Close()
                End Using

            End Using
            oValidacion.validacion = True
        Catch ex As Exception
            oValidacion.mensaje = ex.Message
        End Try
    End Sub

End Class
