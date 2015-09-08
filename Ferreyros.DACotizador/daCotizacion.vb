Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.Utiles.Estructuras

Public Class daCotizacion
    Private Shared uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function BuscarId(ByVal cnnSql As SqlConnection, ByRef obeCotizacion As beCotizacion) As Boolean
        Try
            blnValido = False
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = objBBDD.StoreProcedure.uspCotizacionBuscarId
            With obeCotizacion
                cmdSql.Parameters.Add(uData.CreaParametro("@IdCotizacion", obeCotizacion.IdCotizacion, SqlDbType.VarChar, 20))
                Dim dr As SqlDataReader = cmdSql.ExecuteReader()
                If dr.HasRows Then
                    dr.Read()
                    With obeCotizacion

                        Dim _IdCotizacion As String = dr.GetOrdinal("IdCotizacion")
                        Dim _IdCotizacionSAP As String = dr.GetOrdinal("IdCotizacionSAP")
                        Dim _IdCorporacion As String = dr.GetOrdinal("IdCorporacion")
                        Dim _IdCompania As String = dr.GetOrdinal("IdCompania")
                        Dim _IdSolicitante As String = dr.GetOrdinal("IdSolicitante")
                        Dim _DescripSolicitante As String = dr.GetOrdinal("DescripSolicitante")
                        Dim _RUCSolicitante As String = dr.GetOrdinal("RUCSolicitante")
                        Dim _DNISolicitante As String = dr.GetOrdinal("DNISolicitante")
                        Dim _IdPersonaResponsable As String = dr.GetOrdinal("IdPersonaResponsable")
                        Dim _DescripResponsable As String = dr.GetOrdinal("DescripResponsable")
                        Dim _OficinaResponsable As String = dr.GetOrdinal("OficinaResponsable")
                        Dim _CargoResponsable As String = dr.GetOrdinal("CargoResponsable")
                        Dim _EmailResponsable As String = dr.GetOrdinal("EmailResponsable")
                        Dim _TelefonoResponsable As String = dr.GetOrdinal("TelefonoResponsable")
                        Dim _AnexoTelefonoResponsable As String = dr.GetOrdinal("AnexoTelefonoResponsable")
                        Dim _FechaInicioValidez As String = dr.GetOrdinal("FechaInicioValidez")
                        Dim _FechaFinalValidez As String = dr.GetOrdinal("FechaFinalValidez")
                        Dim _FechaPrecio As String = dr.GetOrdinal("FechaPrecio")
                        Dim _FechaEstimadaFacturacion As String = dr.GetOrdinal("FechaEstimadaFacturacion")
                        Dim _NumeroOportunidad As String = dr.GetOrdinal("NumeroOportunidad")
                        Dim _ItemOportunidad As String = dr.GetOrdinal("ItemOportunidad")
                        Dim _Version As String = dr.GetOrdinal("Version")
                        Dim _ValorTipoCambio As String = dr.GetOrdinal("ValorTipoCambio")
                        Dim _MonedaValorNeto As String = dr.GetOrdinal("MonedaValorNeto")
                        Dim _MonedaValorImpuesto As String = dr.GetOrdinal("MonedaValorImpuesto")
                        Dim _MonedaValorBruto As String = dr.GetOrdinal("MonedaValorBruto")
                        Dim _ValorNeto As String = dr.GetOrdinal("ValorNeto")
                        Dim _ValorImpuesto As String = dr.GetOrdinal("ValorImpuesto")
                        Dim _ValorBruto As String = dr.GetOrdinal("ValorBruto")
                        Dim _NombreEstado As String = dr.GetOrdinal("NombreEstado") 

                        If Not dr.IsDBNull(_IdCotizacion) Then .IdCotizacion = dr.GetValue(_IdCotizacion).ToString()
                        If Not dr.IsDBNull(_IdCotizacionSAP) Then .IdCotizacionSap = dr.GetValue(_IdCotizacionSAP).ToString()
                        If Not dr.IsDBNull(_IdCorporacion) Then .IdCorporacion = dr.GetValue(_IdCorporacion).ToString()
                        If Not dr.IsDBNull(_IdCompania) Then .IdCompania = dr.GetValue(_IdCompania).ToString()
                        If Not dr.IsDBNull(_IdSolicitante) Then .IdSolicitante = dr.GetValue(_IdSolicitante).ToString()
                        If Not dr.IsDBNull(_DescripSolicitante) Then .DescripSolicitante = dr.GetValue(_DescripSolicitante).ToString()
                        If Not dr.IsDBNull(_RUCSolicitante) Then .RUCSolicitante = dr.GetValue(_RUCSolicitante).ToString()
                        If Not dr.IsDBNull(_DNISolicitante) Then .DNISolicitante = dr.GetValue(_DNISolicitante).ToString()
                        If Not dr.IsDBNull(_IdPersonaResponsable) Then .IdPersonaResponsable = dr.GetValue(_IdPersonaResponsable).ToString()
                        If Not dr.IsDBNull(_DescripResponsable) Then .DescripResponsable = dr.GetValue(_DescripResponsable).ToString()
                        If Not dr.IsDBNull(_OficinaResponsable) Then .OficinaResponsable = dr.GetValue(_OficinaResponsable).ToString()
                        If Not dr.IsDBNull(_CargoResponsable) Then .CargoResponsable = dr.GetValue(_CargoResponsable).ToString()
                        If Not dr.IsDBNull(_EmailResponsable) Then .EmailResponsable = dr.GetValue(_EmailResponsable).ToString()
                        If Not dr.IsDBNull(_TelefonoResponsable) Then .TelefonoResponsable = dr.GetValue(_TelefonoResponsable).ToString()
                        If Not dr.IsDBNull(_AnexoTelefonoResponsable) Then .AnexoTelefonoResponsable = dr.GetValue(_AnexoTelefonoResponsable).ToString()
                        If Not dr.IsDBNull(_FechaInicioValidez) Then .FechaInicioValidez = dr.GetValue(_FechaInicioValidez).ToString()
                        If Not dr.IsDBNull(_FechaFinalValidez) Then .FechaFinalValidez = dr.GetValue(_FechaFinalValidez).ToString()
                        If Not dr.IsDBNull(_FechaPrecio) Then .FechaPrecio = dr.GetValue(_FechaPrecio).ToString()
                        If Not dr.IsDBNull(_FechaEstimadaFacturacion) Then .FechaEstimadaFacturacion = dr.GetValue(_FechaEstimadaFacturacion).ToString()
                        If Not dr.IsDBNull(_NumeroOportunidad) Then .NumeroOportunidad = dr.GetValue(_NumeroOportunidad).ToString()
                        If Not dr.IsDBNull(_ItemOportunidad) Then .ItemOportunidad = dr.GetValue(_ItemOportunidad).ToString()
                        If Not dr.IsDBNull(_Version) Then .Version = dr.GetValue(_Version).ToString()
                        If Not dr.IsDBNull(_ValorTipoCambio) Then .ValorTipoCambio = dr.GetValue(_ValorTipoCambio).ToString()
                        If Not dr.IsDBNull(_MonedaValorNeto) Then .MonedaValorNeto = dr.GetValue(_MonedaValorNeto).ToString()
                        If Not dr.IsDBNull(_MonedaValorImpuesto) Then .MonedaValorImpuesto = dr.GetValue(_MonedaValorImpuesto).ToString()
                        If Not dr.IsDBNull(_MonedaValorBruto) Then .MonedaValorBruto = dr.GetValue(_MonedaValorBruto).ToString()
                        If Not dr.IsDBNull(_ValorNeto) Then .ValorNeto = dr.GetValue(_ValorNeto).ToString()
                        If Not dr.IsDBNull(_ValorImpuesto) Then .ValorImpuesto = dr.GetValue(_ValorImpuesto).ToString()
                        If Not dr.IsDBNull(_ValorBruto) Then .ValorBruto = dr.GetValue(_ValorBruto).ToString()
                        If Not dr.IsDBNull(_NombreEstado) Then .NombreEstado = dr.GetValue(_NombreEstado).ToString()
                    End With
                    blnValido = True
                Else
                    blnValido = False
                End If
            End With

        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function Busqueda(ByVal cnnSql As SqlConnection, ByVal eCotizacion As beCotizacion, ByRef dtbConsulta As DataTable) As Boolean
        Dim vExito As Boolean = False
        Try
            Dim cmd As SqlCommand = cnnSql.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = objBBDD.StoreProcedure.uspCotizacionBusqueda
            With eCotizacion
                cmd.Parameters.Add(uData.CreaParametro("@IdCorporacion", .IdCorporacion, SqlDbType.VarChar, 15))
                cmd.Parameters.Add(uData.CreaParametro("@IdCompania", .IdCompania, SqlDbType.VarChar, 15))
                cmd.Parameters.Add(uData.CreaParametro("@IdCotizacionSAP", .IdCotizacionSap, SqlDbType.VarChar, 20))
                cmd.Parameters.Add(uData.CreaParametro("@DescripResponsable", .IdPersonaResponsable, SqlDbType.VarChar, 150))
                cmd.Parameters.Add(uData.CreaParametro("@IdProductoSAP", "", SqlDbType.VarChar, 20))
                cmd.Parameters.Add(uData.CreaParametro("@DescripSolicitante", .IdSolicitante, SqlDbType.VarChar, 150))
                cmd.Parameters.Add(uData.CreaParametro("@NombreEstado", .NombreEstado, SqlDbType.VarChar, 50))
            End With
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dtbConsulta)
            vExito = True
        Catch ex As Exception
            strError = ex.Message
            Return False
        End Try
        Return vExito
    End Function

    Public Function ReportePrimeIdCotizacion(ByVal cnnSql As SqlConnection, ByVal eCotizacion As beCotizacion, ByRef dtsConsulta As DataSet) As Boolean
        Dim vExito As Boolean = False
        Try
            Dim cmd As SqlCommand = cnnSql.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = objBBDD.StoreProcedure.uspCotizacionReportePrime
            With eCotizacion
                cmd.Parameters.Add(uData.CreaParametro("@IdCotizacion", .IdCotizacion, SqlDbType.Int))
            End With
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dtsConsulta)
            vExito = True
        Catch ex As Exception
            strError = ex.Message
            Return False
        End Try
        Return vExito
    End Function

    Public Function BusquedaEnAprobacion(ByVal cnnSql As SqlConnection, ByVal eCotizacion As beCotizacion, ByRef dtbConsulta As DataTable) As Boolean
        Dim vExito As Boolean = False
        Try
            Dim cmd As SqlCommand = cnnSql.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = objBBDD.StoreProcedure.uspCotizacionBuscarEnAprobacion
            With eCotizacion
                cmd.Parameters.Add(uData.CreaParametro("@IdCorporacion", .IdCorporacion, SqlDbType.VarChar, 15))
                cmd.Parameters.Add(uData.CreaParametro("@IdCompania", .IdCompania, SqlDbType.VarChar, 15))
                cmd.Parameters.Add(uData.CreaParametro("@IdCotizacionSAP", .IdCotizacionSap, SqlDbType.VarChar, 20))
                cmd.Parameters.Add(uData.CreaParametro("@DescripSolicitante", .DescripSolicitante, SqlDbType.VarChar, 150))
            End With
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dtbConsulta)
            vExito = True
        Catch ex As Exception
            strError = ex.Message
            Return False
        End Try
        Return vExito
    End Function

    Public Sub ActualizarCotizacionEnvioSAP(ByVal eConexion As String, ByVal eCotizacion As beCotizacion, ByRef eValidacion As beValidacion)

        Dim transaction As SqlTransaction = Nothing
        Dim conexion As SqlConnection = Nothing
        Dim command As SqlCommand = Nothing

        Try
            conexion = New SqlConnection(eConexion)
            conexion.Open()
            transaction = conexion.BeginTransaction
            command = conexion.CreateCommand
            command.CommandText = objBBDD.StoreProcedure.CotizacionActualizar
            command.CommandType = CommandType.StoredProcedure
            command.Parameters.Add(uData.CreaParametro("@IdCotizacionSAP", eCotizacion.IdCotizacionSap, SqlDbType.VarChar, 20))
            command.Parameters.Add(uData.CreaParametro("@ValorNeto", eCotizacion.ValorNeto, SqlDbType.Float, 20))
            command.Parameters.Add(uData.CreaParametro("@ValorImpuesto", eCotizacion.ValorImpuesto, SqlDbType.Float, 20))
            command.Parameters.Add(uData.CreaParametro("@ValorBruto", eCotizacion.ValorBruto, SqlDbType.Float, 20))
            command.Parameters.Add(uData.CreaParametro("@Usuario", eValidacion.usuario, SqlDbType.VarChar, 15))
            command.Transaction = transaction
            command.ExecuteNonQuery()
            For Each Producto As beProducto In eCotizacion.ListaProducto
                Dim cmdProducto As New SqlCommand
                cmdProducto = conexion.CreateCommand
                cmdProducto.CommandText = objBBDD.StoreProcedure.ProductoActualizar
                cmdProducto.CommandType = CommandType.StoredProcedure
                cmdProducto.Parameters.Add(uData.CreaParametro("@IdProducto", Producto.IdProducto, SqlDbType.Int, 15))
                cmdProducto.Parameters.Add(uData.CreaParametro("@IdCotizacion", eCotizacion.IdCotizacion, SqlDbType.Int, 15))
                cmdProducto.Parameters.Add(uData.CreaParametro("@IdPosicionSAP", Producto.IdPosicion, SqlDbType.VarChar, 20))
                cmdProducto.Parameters.Add(uData.CreaParametro("@IdProductoSAP", Producto.IdProductoSap, SqlDbType.VarChar, 20))
                cmdProducto.Parameters.Add(uData.CreaParametro("@ValorUnitario", Producto.ValorUnitario, SqlDbType.Float, 20))
                cmdProducto.Parameters.Add(uData.CreaParametro("@Cantidad", Producto.Cantidad, SqlDbType.Int, 15))
                cmdProducto.Parameters.Add(uData.CreaParametro("@ValorNeto", Producto.ValorNeto, SqlDbType.Float, 20))
                cmdProducto.Parameters.Add(uData.CreaParametro("@Usuario", eValidacion.usuario, SqlDbType.VarChar, 15))
                cmdProducto.Transaction = transaction
                cmdProducto.ExecuteNonQuery()
                If Producto.TipoProducto = TipoProducto.PRIME Then 'PRIME" Then

                End If
                If Producto.TipoProducto = TipoProducto.CSA Then '"Z002" Then 'CSA
                    'Al Actualizar Producto CSA se elimina sus maquinarias
                    Dim cmdProductoCSA As New SqlCommand
                    cmdProductoCSA = conexion.CreateCommand
                    cmdProductoCSA.CommandText = objBBDD.StoreProcedure.ProductoCsaActualizar
                    cmdProductoCSA.CommandType = CommandType.StoredProcedure
                    cmdProductoCSA.Parameters.Add(uData.CreaParametro("@IdProducto", Producto.IdProducto, SqlDbType.Int, 15))
                    cmdProductoCSA.Parameters.Add(uData.CreaParametro("@IncluyeFluidos", Producto.ProductoCSA.IncluyeFluidos, SqlDbType.Bit, 5))
                    cmdProductoCSA.Parameters.Add(uData.CreaParametro("@IncluyeDetallePartes", Producto.ProductoCSA.IncluyeDetallePartes, SqlDbType.Bit, 5))
                    cmdProductoCSA.Parameters.Add(uData.CreaParametro("@FechaInicioContrato", Producto.ProductoCSA.FechaInicioContrato, SqlDbType.Date, 10))
                    cmdProductoCSA.Parameters.Add(uData.CreaParametro("@FechaEstimadaCierre", Producto.ProductoCSA.FechaEstimadaCierre, SqlDbType.Date, 10))
                    cmdProductoCSA.Parameters.Add(uData.CreaParametro("@ParticipacionVendedor1", Producto.ProductoCSA.ParticipacionVendedor1, SqlDbType.Int, 15))
                    cmdProductoCSA.Parameters.Add(uData.CreaParametro("@ParticipacionVendedor2", Producto.ProductoCSA.ParticipacionVendedor2, SqlDbType.Int, 15))
                    cmdProductoCSA.Parameters.Add(uData.CreaParametro("@Usuario", eValidacion.usuario, SqlDbType.VarChar, 15))
                    cmdProductoCSA.Transaction = transaction
                    cmdProductoCSA.ExecuteNonQuery()

                    ' El SP uspMaquinariaActualizar es de insercion
                    For Each Maquinaria As beMaquinaria In Producto.ProductoCSA.ListaMaquinaria
                        'Como se inserta un nuevo el codigo se envia 0 y regresa con el valor de IdMaquinaria generado
                        Maquinaria.codigo = -1
                        Dim cmdMaquinaria As New SqlCommand
                        'Validaciones
                        If IsDBNull(Maquinaria.modelo) Or String.IsNullOrEmpty(Maquinaria.modelo) Then
                            Maquinaria.modelo = Maquinaria.modeloBase
                        End If
                        cmdMaquinaria = conexion.CreateCommand
                        cmdMaquinaria.CommandText = objBBDD.StoreProcedure.MaquinariaActualizar
                        cmdMaquinaria.CommandType = CommandType.StoredProcedure
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@IdProducto", Producto.IdProducto, SqlDbType.Int, 15))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@Item", Maquinaria.item, SqlDbType.Int, 15))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@MaquinaNueva", Maquinaria.maquinaNueva, SqlDbType.Bit, 10))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@Familia", Maquinaria.familia, SqlDbType.VarChar, 100))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@FamiliaOt", Maquinaria.familiaOt, SqlDbType.VarChar, 100))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@ModeloBase", Maquinaria.modeloBase, SqlDbType.VarChar, 100))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@ModeloBaseOt", Maquinaria.modeloBaseOt, SqlDbType.VarChar, 100))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@Modelo", Maquinaria.modelo, SqlDbType.VarChar, 100))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@Prefijo", Maquinaria.prefijo, SqlDbType.VarChar, 100))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@PrefijoOt", Maquinaria.prefijoOt, SqlDbType.VarChar, 100))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@NumeroMaquinas", Maquinaria.numeroMaquinas, SqlDbType.Int, 15))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@NumeroSerie", Maquinaria.numeroSerie, SqlDbType.VarChar, 100))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@NumeroSerieOt", Maquinaria.numeroSerieOt, SqlDbType.VarChar, 100))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@HorometroInicial", Maquinaria.horometroInicial, SqlDbType.Int, 10))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@HorometroFin", Maquinaria.horometroFinal, SqlDbType.Int, 10))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@FechaHorometro", Maquinaria.fechaHorometro, SqlDbType.Date, 10))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@HorasPromedioUso", Maquinaria.horasPromedioMensual, SqlDbType.Int, 10))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@CodDepartamento", Maquinaria.codDepartamento, SqlDbType.VarChar, 30))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@Departamento", Maquinaria.departamento, SqlDbType.VarChar, 100))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@Renovacion", Maquinaria.renovacion, SqlDbType.Bit, 10))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@RenovacionValida", Maquinaria.renovacionValida, SqlDbType.Bit, 10))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@PrecioNegociado", Maquinaria.montoItem, SqlDbType.Float, 15))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@Usuario", eValidacion.usuario, SqlDbType.VarChar, 15))
                        'cmdMaquinaria.Parameters.Add(uData.CreaParametro("@IdMaquinaria", Maquinaria.codigo, SqlDbType.Int, ParameterDirection.InputOutput))
                        cmdMaquinaria.Parameters.Add(uData.CreaParametro("@IdMaquinaria", Maquinaria.codigo, SqlDbType.Int, 15, ParameterDirection.InputOutput))
                        cmdMaquinaria.Transaction = transaction
                        cmdMaquinaria.ExecuteNonQuery()

                        Maquinaria.codigo = cmdMaquinaria.Parameters("@IdMaquinaria").Value

                        'Aqui todas las maquinarias
                        If Maquinaria.codigo <> -1 Then 
                            Dim oMaquinariaEnvioSAp As New beMaquinariaEnvioSap

                            oMaquinariaEnvioSAp.IdCotizacionSap = eCotizacion.IdCotizacionSap
                            oMaquinariaEnvioSAp.IdPosicionSap = Producto.IdPosicion
                            oMaquinariaEnvioSAp.IdProductoSap = Producto.IdProductoSap
                            oMaquinariaEnvioSAp.IdMaquinaria = Maquinaria.codigo

                            Dim cmdMaquinariaEnvioSap As New SqlCommand
                            cmdMaquinaria = conexion.CreateCommand
                            cmdMaquinaria.CommandText = objBBDD.StoreProcedure.uspMaquinariaEnvioSapInsertar
                            cmdMaquinaria.CommandType = CommandType.StoredProcedure
                            cmdMaquinaria.Parameters.Add(uData.CreaParametro("@IdCotizacionSap", oMaquinariaEnvioSAp.IdCotizacionSap, SqlDbType.VarChar, 20))
                            cmdMaquinaria.Parameters.Add(uData.CreaParametro("@IdPosicionSap", oMaquinariaEnvioSAp.IdPosicionSap, SqlDbType.VarChar, 20))
                            cmdMaquinaria.Parameters.Add(uData.CreaParametro("@IdProductoSap", oMaquinariaEnvioSAp.IdProductoSap, SqlDbType.VarChar, 20))
                            cmdMaquinaria.Parameters.Add(uData.CreaParametro("@IdMaquinaria", oMaquinariaEnvioSAp.IdMaquinaria, SqlDbType.Int, 15))
                             cmdMaquinaria.Transaction = transaction
                            cmdMaquinaria.ExecuteNonQuery()

                        End If

                    Next
                End If
            Next
            command = Nothing
            transaction.Commit()
            eValidacion.validacion = True
        Catch ex As Exception
            transaction.Rollback()
            eValidacion.mensaje = ex.Message.ToString
        Finally
            If conexion.State = ConnectionState.Open Then conexion.Close()
        End Try

    End Sub

    Public Sub ActualizarSolComb(ByVal eConexion As String, ByVal l_SolComb As List(Of beSolucionCombinada), ByVal idCotizacion As String, ByVal idProducto As String, ByVal idPosicion As String, ByVal idTarifa As String, ByRef eValidacion As beValidacion)

        Dim conexionSC As SqlConnection = Nothing
        Dim SolucionCombinada As New beSolucionCombinada
        conexionSC = New SqlConnection(eConexion)
        conexionSC.Open()
        Try
            Using cmd1 As SqlCommand = conexionSC.CreateCommand
                cmd1.CommandType = CommandType.StoredProcedure
                cmd1.CommandText = "uspSolucionCombinadaMantenimiento"
                With SolucionCombinada
                    cmd1.Parameters.Add(uData.CreaParametro("@Tipo", 1, SqlDbType.Int))
                    cmd1.Parameters.Add(uData.CreaParametro("@IdCotizacion", idCotizacion, SqlDbType.VarChar, 20))
                    cmd1.Parameters.Add(uData.CreaParametro("@IdProducto", idProducto, SqlDbType.VarChar, 20))
                    cmd1.Parameters.Add(uData.CreaParametro("@IdPosicion", idPosicion, SqlDbType.VarChar, 20))
                    cmd1.Parameters.Add(uData.CreaParametro("@IdTarifa", CInt(idTarifa), SqlDbType.Int))
                    cmd1.Parameters.Add(uData.CreaParametro("@LLave", .LLave, SqlDbType.VarChar, 50))
                    cmd1.Parameters.Add(uData.CreaParametro("@Valor", .Valor, SqlDbType.VarChar, -1))
                    cmd1.Parameters.Add(uData.CreaParametro("@Usuario", eValidacion.usuario, SqlDbType.VarChar, 50))
                    cmd1.ExecuteNonQuery()
                End With
            End Using
        Catch ex As Exception
            strError = ex.Message
        Finally
            If conexionSC.State = ConnectionState.Open Then conexionSC.Close()
        End Try

        conexionSC = New SqlConnection(eConexion)
        conexionSC.Open()
        Try
            For Each SolucionCombinada In l_SolComb
                Using cmd As SqlCommand = conexionSC.CreateCommand()
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandText = "uspSolucionCombinadaMantenimiento"
                    With SolucionCombinada
                        cmd.Parameters.Add(uData.CreaParametro("@TIPO", 2, SqlDbType.Int))
                        cmd.Parameters.Add(uData.CreaParametro("@IdCotizacion", .IdCotizacion, SqlDbType.VarChar, 20))
                        cmd.Parameters.Add(uData.CreaParametro("@IdProducto", idProducto, SqlDbType.VarChar, 20))
                        cmd.Parameters.Add(uData.CreaParametro("@IdPosicion", idPosicion, SqlDbType.VarChar, 20))
                        cmd.Parameters.Add(uData.CreaParametro("@IdTarifa", CInt(idTarifa), SqlDbType.Int))
                        cmd.Parameters.Add(uData.CreaParametro("@LLave", .LLave, SqlDbType.VarChar, 50))
                        cmd.Parameters.Add(uData.CreaParametro("@Valor", .Valor, SqlDbType.VarChar, -1))
                        cmd.Parameters.Add(uData.CreaParametro("@Usuario", eValidacion.usuario, SqlDbType.VarChar, 50))
                        cmd.ExecuteNonQuery()
                    End With
                End Using
            Next
            eValidacion.validacion = True
        Catch ex As Exception
            strError = ex.Message
        Finally
            If conexionSC.State = ConnectionState.Open Then conexionSC.Close()
        End Try
    End Sub

    Public Sub CotizacionConsultar(ByVal oConexion As String, ByVal Cotizacion As beCotizacion, ByRef oValidacion As beValidacion, ByRef oDetalle As List(Of beCotizacion))
        Try
            Using cnnSql As New SqlConnection(oConexion)
                cnnSql.Open()
                Dim cmdSql As SqlCommand = cnnSql.CreateCommand
                cmdSql.CommandText = objBBDD.StoreProcedure.CotizacionConsulta
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.Parameters.Add(uData.CreaParametro("@IdCotizacion", Cotizacion.IdCotizacionSap, SqlDbType.VarChar, 20))
                cmdSql.Parameters.Add(uData.CreaParametro("@DescripSolicitante", Cotizacion.DescripSolicitante, SqlDbType.VarChar, 150))
                cmdSql.Parameters.Add(uData.CreaParametro("@DescripResponsable", Cotizacion.DescripResponsable, SqlDbType.VarChar, 150))
                Using dr As IDataReader = cmdSql.ExecuteReader
                    Dim _IdCotizacion As Integer = dr.GetOrdinal("IdCotizacion")
                    Dim _IdCotizacionSAP As Integer = dr.GetOrdinal("IdCotizacionSAP")
                    Dim _DescripSolicitante As Integer = dr.GetOrdinal("DescripSolicitante")
                    Dim _DescripResponsable As Integer = dr.GetOrdinal("DescripResponsable")
                    Dim _FechaInicioValidez As Integer = dr.GetOrdinal("FechaInicioValidez")
                    Dim _FechaFinalValidez As Integer = dr.GetOrdinal("FechaFinalValidez")
                    Dim _NumeroOportunidad As Integer = dr.GetOrdinal("NumeroOportunidad")
                    Dim _ItemOportunidad As Integer = dr.GetOrdinal("ItemOportunidad")
                    Dim _NombreEstado As Integer = dr.GetOrdinal("NombreEstado")
                    Dim Rpt As beCotizacion = Nothing
                    While dr.Read
                        Rpt = New beCotizacion
                        If Not dr.IsDBNull(_IdCotizacion) Then Rpt.IdCotizacion = dr.GetValue(_IdCotizacion)
                        If Not dr.IsDBNull(_IdCotizacionSAP) Then Rpt.IdCotizacionSap = dr.GetValue(_IdCotizacionSAP)
                        If Not dr.IsDBNull(_DescripSolicitante) Then Rpt.DescripSolicitante = dr.GetValue(_DescripSolicitante)
                        If Not dr.IsDBNull(_DescripResponsable) Then Rpt.DescripResponsable = dr.GetValue(_DescripResponsable)
                        If Not dr.IsDBNull(_FechaInicioValidez) Then Rpt.FechaInicioValidez = dr.GetValue(_FechaInicioValidez)
                        If Not dr.IsDBNull(_FechaFinalValidez) Then Rpt.FechaFinalValidez = dr.GetValue(_FechaFinalValidez)
                        If Not dr.IsDBNull(_NumeroOportunidad) Then Rpt.NumeroOportunidad = dr.GetValue(_NumeroOportunidad)
                        If Not dr.IsDBNull(_ItemOportunidad) Then Rpt.ItemOportunidad = dr.GetValue(_ItemOportunidad)
                        If Not dr.IsDBNull(_NombreEstado) Then Rpt.NombreEstado = dr.GetValue(_NombreEstado)
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


    Public Sub CotizacionListar(ByVal oConexion As String, ByRef Cotizacion As beCotizacion, ByRef oValidacion As beValidacion)
        Try
            Using cnnSql As New SqlConnection(oConexion)
                cnnSql.Open()
                Dim cmdSql As SqlCommand = cnnSql.CreateCommand
                cmdSql.CommandText = objBBDD.StoreProcedure.CotizacionListar
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.Parameters.Add(uData.CreaParametro("@IdCotizacionSap", Cotizacion.IdCotizacionSap, SqlDbType.VarChar, 20))
                Using dr As IDataReader = cmdSql.ExecuteReader
                    Dim _IdCotizacion As Integer = dr.GetOrdinal("IdCotizacion")
                    Dim _IdCotizacionSAP As Integer = dr.GetOrdinal("IdCotizacionSAP")
                    Dim _IdCorporacion As Integer = dr.GetOrdinal("IdCorporacion")
                    Dim _IdCompania As Integer = dr.GetOrdinal("IdCompania")
                    Dim _IdSolicitante As Integer = dr.GetOrdinal("IdSolicitante")
                    Dim _DescripSolicitante As Integer = dr.GetOrdinal("DescripSolicitante")
                    Dim _RUCSolicitante As Integer = dr.GetOrdinal("RUCSolicitante")
                    Dim _DNISolicitante As Integer = dr.GetOrdinal("DNISolicitante")
                    Dim _IdPersonaResponsable As Integer = dr.GetOrdinal("IdPersonaResponsable")
                    Dim _DescripResponsable As Integer = dr.GetOrdinal("DescripResponsable")
                    Dim _OficinaResponsable As Integer = dr.GetOrdinal("OficinaResponsable")
                    Dim _CargoResponsable As Integer = dr.GetOrdinal("CargoResponsable")
                    Dim _EmailResponsable As Integer = dr.GetOrdinal("EmailResponsable")
                    Dim _TelefonoResponsable As Integer = dr.GetOrdinal("TelefonoResponsable")
                    Dim _AnexoTelefonoResponsable As Integer = dr.GetOrdinal("AnexoTelefonoResponsable")
                    Dim _FechaInicioValidez As Integer = dr.GetOrdinal("FechaInicioValidez")
                    Dim _FechaFinalValidez As Integer = dr.GetOrdinal("FechaFinalValidez")
                    Dim _FechaPrecio As Integer = dr.GetOrdinal("FechaPrecio")
                    Dim _FechaEstimadaFacturacion As Integer = dr.GetOrdinal("FechaEstimadaFacturacion")
                    Dim _NumeroOportunidad As Integer = dr.GetOrdinal("NumeroOportunidad")
                    Dim _ItemOportunidad As Integer = dr.GetOrdinal("ItemOportunidad")
                    Dim _Version As Integer = dr.GetOrdinal("Version")
                    Dim _ValorTipoCambio As Integer = dr.GetOrdinal("ValorTipoCambio")
                    Dim _MonedaValorNeto As Integer = dr.GetOrdinal("MonedaValorNeto")
                    Dim _MonedaValorImpuesto As Integer = dr.GetOrdinal("MonedaValorImpuesto")
                    Dim _MonedaValorBruto As Integer = dr.GetOrdinal("MonedaValorBruto")
                    Dim _ValorNeto As Integer = dr.GetOrdinal("ValorNeto")
                    Dim _ValorImpuesto As Integer = dr.GetOrdinal("ValorImpuesto")
                    Dim _ValorBruto As Integer = dr.GetOrdinal("ValorBruto")
                    Dim _NombreEstado As Integer = dr.GetOrdinal("NombreEstado")

                    Dim _RolUsuario As String = dr.GetOrdinal("RolUsuario")

                    While dr.Read
                        If Not dr.IsDBNull(_IdCotizacion) Then Cotizacion.IdCotizacion = dr.GetValue(_IdCotizacion)
                        If Not dr.IsDBNull(_IdCotizacionSAP) Then Cotizacion.IdCotizacionSap = dr.GetValue(_IdCotizacionSAP)
                        If Not dr.IsDBNull(_IdCorporacion) Then Cotizacion.IdCorporacion = dr.GetValue(_IdCorporacion)
                        If Not dr.IsDBNull(_IdCompania) Then Cotizacion.IdCompania = dr.GetValue(_IdCompania)
                        If Not dr.IsDBNull(_IdSolicitante) Then Cotizacion.IdSolicitante = dr.GetValue(_IdSolicitante)
                        If Not dr.IsDBNull(_DescripSolicitante) Then Cotizacion.DescripSolicitante = dr.GetValue(_DescripSolicitante)
                        If Not dr.IsDBNull(_RUCSolicitante) Then Cotizacion.RUCSolicitante = dr.GetValue(_RUCSolicitante)
                        If Not dr.IsDBNull(_DNISolicitante) Then Cotizacion.DNISolicitante = dr.GetValue(_DNISolicitante)
                        If Not dr.IsDBNull(_IdPersonaResponsable) Then Cotizacion.IdPersonaResponsable = dr.GetValue(_IdPersonaResponsable)
                        If Not dr.IsDBNull(_DescripResponsable) Then Cotizacion.DescripResponsable = dr.GetValue(_DescripResponsable)
                        If Not dr.IsDBNull(_OficinaResponsable) Then Cotizacion.OficinaResponsable = dr.GetValue(_OficinaResponsable)
                        If Not dr.IsDBNull(_CargoResponsable) Then Cotizacion.CargoResponsable = dr.GetValue(_CargoResponsable)
                        If Not dr.IsDBNull(_EmailResponsable) Then Cotizacion.EmailResponsable = dr.GetValue(_EmailResponsable)
                        If Not dr.IsDBNull(_TelefonoResponsable) Then Cotizacion.TelefonoResponsable = dr.GetValue(_TelefonoResponsable)
                        If Not dr.IsDBNull(_AnexoTelefonoResponsable) Then Cotizacion.AnexoTelefonoResponsable = dr.GetValue(_AnexoTelefonoResponsable)
                        If Not dr.IsDBNull(_FechaInicioValidez) Then Cotizacion.FechaInicioValidez = dr.GetValue(_FechaInicioValidez)
                        If Not dr.IsDBNull(_FechaFinalValidez) Then Cotizacion.FechaFinalValidez = dr.GetValue(_FechaFinalValidez)
                        If Not dr.IsDBNull(_FechaPrecio) Then Cotizacion.FechaPrecio = dr.GetValue(_FechaPrecio)
                        If Not dr.IsDBNull(_FechaEstimadaFacturacion) Then Cotizacion.FechaEstimadaFacturacion = dr.GetValue(_FechaEstimadaFacturacion)
                        If Not dr.IsDBNull(_NumeroOportunidad) Then Cotizacion.NumeroOportunidad = dr.GetValue(_NumeroOportunidad)
                        If Not dr.IsDBNull(_ItemOportunidad) Then Cotizacion.ItemOportunidad = dr.GetValue(_ItemOportunidad)
                        If Not dr.IsDBNull(_Version) Then Cotizacion.Version = dr.GetValue(_Version)
                        If Not dr.IsDBNull(_ValorTipoCambio) Then Cotizacion.ValorTipoCambio = dr.GetValue(_ValorTipoCambio)
                        If Not dr.IsDBNull(_MonedaValorNeto) Then Cotizacion.MonedaValorNeto = dr.GetValue(_MonedaValorNeto)
                        If Not dr.IsDBNull(_MonedaValorImpuesto) Then Cotizacion.MonedaValorImpuesto = dr.GetValue(_MonedaValorImpuesto)
                        If Not dr.IsDBNull(_MonedaValorBruto) Then Cotizacion.MonedaValorBruto = dr.GetValue(_MonedaValorBruto)
                        If Not dr.IsDBNull(_ValorNeto) Then Cotizacion.ValorNeto = dr.GetValue(_ValorNeto)
                        If Not dr.IsDBNull(_ValorImpuesto) Then Cotizacion.ValorImpuesto = dr.GetValue(_ValorImpuesto)
                        If Not dr.IsDBNull(_ValorBruto) Then Cotizacion.ValorBruto = dr.GetValue(_ValorBruto)
                        If Not dr.IsDBNull(_NombreEstado) Then Cotizacion.NombreEstado = dr.GetValue(_NombreEstado)
                        If Not dr.IsDBNull(_RolUsuario) Then Cotizacion.RolUsuario = dr.GetValue(_RolUsuario)

                    End While

                    dr.Close()
                End Using
            End Using
            oValidacion.validacion = True
        Catch ex As Exception
            oValidacion.mensaje = ex.Message
        End Try
    End Sub

    Public Function DatosDocumento(ByVal cnnSql As SqlConnection, ByVal obeCotizacion As beCotizacion, ByRef dtsCotizacion As DataSet) As Boolean
        Try
            blnValido = False
            Using cmdSql As SqlCommand = cnnSql.CreateCommand()
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.CommandText = objBBDD.StoreProcedure.uspCotizacionDatosDocumento
                With obeCotizacion
                    cmdSql.Parameters.Add(uData.CreaParametro("@IdCotizacion", obeCotizacion.IdCotizacion, SqlDbType.VarChar, 20))
                    Dim da As New SqlDataAdapter(cmdSql)
                    da.Fill(dtsCotizacion)
                    If dtsCotizacion.Tables.Count Then
                        dtsCotizacion.Tables(0).TableName = Entidad.CartaPresentacion
                        dtsCotizacion.Tables(1).TableName = Entidad.ResumenPropuesta
                        dtsCotizacion.Tables(2).TableName = Entidad.AdicionalProducto
                        dtsCotizacion.Tables(3).TableName = Entidad.AccesorioProducto
                        dtsCotizacion.Tables(4).TableName = Entidad.CondicionesGeneralesPrime '  "CondicionesGenerales"
                        dtsCotizacion.Tables(5).TableName = Entidad.CondicionesGeneralesCSA '
                        dtsCotizacion.Tables(6).TableName = Entidad.TerminosCondiciones '  "DetalleProducto"
                        dtsCotizacion.Tables(7).TableName = Entidad.DetalleProducto '  "TerminosCondiciones" 
                        dtsCotizacion.Tables(8).TableName = Entidad.ProductoAlquiler  '  "ProductoAlquiler" 
                        dtsCotizacion.Tables(9).TableName = Entidad.CondicionesGeneralesAlquiler '  "CondicionesGenerales"
                    End If
                    blnValido = True
                End With
            End Using
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

End Class
