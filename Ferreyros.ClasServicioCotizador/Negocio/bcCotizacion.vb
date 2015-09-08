Imports System.Data.SqlClient
Imports log4net
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros.Utiles.uConfiguracion
Imports System.IO
Imports System.Web

Public Class bcCotizacion

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(bcCotizacion))
    Private Connection As SqlConnection = Nothing
    Private Transaction As SqlTransaction = Nothing
    Private dalCotizacion As daCotizacion = Nothing
    Private dalCotizacionContacto As daCotizacionContacto = Nothing
    Private dalProducto As daProducto = Nothing
    Private dalProductoCSA As daProductoCSA = Nothing
    Private dalMaquinaria As daMaquinaria = Nothing
    Private daTelefonoResponsable As daTelefonoResponsable = Nothing

    Private odaProductoPrime As daProductoPrime = Nothing
    Private odaDetalleRecompra As daDetalleRecompra = Nothing
    Private odaProductoAdicional As daProductoAdicional = Nothing
    Private odaProductoAccesorio As daProductoAccesorio = Nothing
    Private odaProductoAlquiler As daProductoAlquiler = Nothing
    Private odaProductoAlquilerTarifa As daProductoAlquilerTarifa = Nothing
    Private odaProductoCaracteristica As daProductoCaracteristica = Nothing
    Private odaProductoDetallePrecios As daProductoDetallePrecios = Nothing
    Private odaProductoSolucionCombinada As daProductoSolucionCombinada = Nothing
    Private odaConsultaAS400 As daConsultaAS400 = Nothing
    Private _ErrorDescripcion As String = String.Empty
    Private uConfig As New Utiles.uConfiguracion
    Private odaCotizacionVersion As daCotizacionVersion = Nothing

    Dim _UrlServicio As String = String.Empty
    Public Property UrlServicio As String
        Get
            Return _UrlServicio
        End Get
        Set(value As String)
            _UrlServicio = value
        End Set
    End Property
    Public Property ErrorDescripcion() As String
        Get
            Return _ErrorDescripcion
        End Get
        Set(ByVal value As String)
            _ErrorDescripcion = value
        End Set
    End Property

    Public Function InsertarCotizacion(ByVal eConexion As String, ByVal eConexionAS400 As String, ByVal Cotizacion As beCotizacion, ByVal Validacion As beValidacion) As Boolean
        log.Error("InsertarCotizacion-------------------------------------------------------------------------------")
        Dim lResult As Boolean = False

        Try
            log.Error("VALIDANDO...")
            Call ValidarCotizacion(Cotizacion)
            log.Error("HOMOLOGANDO...")
            Call HomologarCamposCotizacion(eConexionAS400, Cotizacion, Validacion)
            Connection = New SqlConnection(eConexion)
            log.Error("conexion: " + eConexion)
            Connection.Open()
            Transaction = Connection.BeginTransaction
            REM 1.- Insertar la Cotizacion ------------------------------------------
            dalCotizacion = New daCotizacion
            'las monedas siempre seran del mismo tipo que la moneda del primer producto
            
            Dim obProducto As beProducto = Cotizacion.ListaProducto.First
            log.Error("PRODUCTO...")
            If Not obProducto Is Nothing Then
                log.Error("producto: " + Cotizacion.ListaProducto.First.IdProducto)
                log.Error("posicion: " + Cotizacion.ListaProducto.First.IdPosicion)
                log.Error("tipo producto: " + Cotizacion.ListaProducto.First.TipoProducto)
                Cotizacion.MonedaValorBruto = obProducto.MonedaCotizacion
                Cotizacion.MonedaValorImpuesto = obProducto.MonedaCotizacion
                Cotizacion.MonedaValorNeto = obProducto.MonedaCotizacion
            End If

            Call LogCotizacion(Cotizacion, Validacion)
            Call dalCotizacion.InsertarCotizacion(Connection, Transaction, Cotizacion)
            REM 2.- Insertar CotizacionContacto -------------------------------------
            If Not (Cotizacion.ListaCotizacionContacto Is Nothing OrElse Cotizacion.ListaCotizacionContacto.Count = 0) Then
                For Each CotizacionContacto As beCotizacionContacto In Cotizacion.ListaCotizacionContacto
                    dalCotizacionContacto = New daCotizacionContacto
                    Call LogCotizacionContacto(CotizacionContacto, Validacion)
                    Call dalCotizacionContacto.InsertarCotizacionContacto(Connection, Transaction, dalCotizacion.IdCotizacion, dalCotizacion.Usuario, CotizacionContacto)
                Next
            End If

            'Inserta Tabla TelefonoResponsable
            If Not (Cotizacion.ListaTelefonoResponsable Is Nothing OrElse Cotizacion.ListaTelefonoResponsable.Count = 0) Then
                For Each TelefonoResponsable As beTelefonoResponsable In Cotizacion.ListaTelefonoResponsable
                    daTelefonoResponsable = New daTelefonoResponsable
                    Call LogTelefonoResponsable(TelefonoResponsable, Validacion)
                    Call daTelefonoResponsable.InsertarTelefonoResponsable(Connection, Transaction, dalCotizacion.IdCotizacion, TelefonoResponsable)
                Next
            End If


            REM 3.- Insertar Producto -----------------------------------------------
            For Each Producto As beProducto In Cotizacion.ListaProducto

                log.Error("TIPO PRODUCTO" + ": " + Producto.TipoProducto.ToString)

                dalProducto = New daProducto

                'Insertar los accesorios como producto Prime
                If Producto.TipoProducto = TipoProducto.ACCESORIO Then
                    Producto.TipoProducto = TipoProducto.PRIME
                End If

                Dim obeHomologacion As New beHomologacion
                obeHomologacion.Tabla = TablaHomologacion.COD_LINEA_COD_FAMILIA_SOLUCION_COMBINADA  ' tabla
                obeHomologacion.ValorSap = Producto.CodigoFamilia

                Dim odaHomologacion As New bcHomologacion
                Dim ListaHomologacion As New List(Of beHomologacion)

                odaHomologacion.BuscarTabla(eConexion, obeHomologacion, ListaHomologacion)

                If ListaHomologacion.Count > 0 Then
                    obeHomologacion = ListaHomologacion.ToList().FirstOrDefault
                    Producto.CodigoLinea = obeHomologacion.ValorCotizador
                End If

                Call LogProducto(dalCotizacion.IdCotizacion, dalCotizacion.Usuario, Producto, Validacion)
                Call dalProducto.InsertarProducto(Connection, Transaction, dalCotizacion.IdCotizacion, dalCotizacion.Usuario, Producto)
                REM 4.- Insertar por tipo de producto -----------------------------------------------
                If Producto.TipoProducto = TipoProducto.PRIME Then
                    REM 4.1.- PRIME -----------------------------------------------
                    'Asignar IdProducto Generado
                    Producto.ProductoPrime.IdProducto = dalProducto.IdProducto
                    'Insertar producto Prime
                    odaProductoPrime = New daProductoPrime
                    Call LogProductoPrime(dalProducto.IdProducto, dalCotizacion.Usuario, Producto.ProductoPrime, Validacion)
                    If odaProductoPrime.Insertar(Connection, Producto.ProductoPrime, Transaction) Then

                        ' Insertar lista de DetalleRecompra
                        odaDetalleRecompra = New daDetalleRecompra
                        For Each obeDetalleRecompra As beDetalleRecompra In Producto.ProductoPrime.ListabeDetalleRecompra
                            ''Asignar IdProducto Generado
                            obeDetalleRecompra.IdProducto = dalProducto.IdProducto
                            Call LogDetalleRecompra(dalProducto.IdProducto, dalCotizacion.Usuario, obeDetalleRecompra, Validacion)
                            Call odaDetalleRecompra.Insertar(Connection, obeDetalleRecompra, Transaction)
                        Next

                        'Insertar lista de ProductoAdicional
                        odaProductoAdicional = New daProductoAdicional
                        For Each obeProductoAdicional As beProductoAdicional In Producto.ProductoPrime.ListabeProductoAdicional
                            'Asignar IdProducto Generado
                            obeProductoAdicional.IdProducto = dalProducto.IdProducto
                            Call LogProductoAdicional(dalProducto.IdProducto, dalCotizacion.Usuario, obeProductoAdicional, Validacion)
                            Call odaProductoAdicional.Insertar(Connection, obeProductoAdicional, Transaction)
                        Next

                        'Insertar Lista de ProductoAccesorio
                        odaProductoAccesorio = New daProductoAccesorio
                        For Each obeProductoAccesorio As beProductoAccesorio In Producto.ProductoPrime.ListabeProductoAccesorio
                            'Asignar IdProducto Generado
                            obeProductoAccesorio.IdProducto = dalProducto.IdProducto
                            obeProductoAccesorio.UsuarioCreacion = dalCotizacion.Usuario
                            Call LogProductoAccesorio(dalCotizacion.Usuario, obeProductoAccesorio, Validacion)
                            Call odaProductoAccesorio.Insertar(Connection, obeProductoAccesorio, Transaction)
                        Next
                    Else
                        log.Error("Error en odaProductoPrime.Insertar " + ": " + odaProductoPrime.ErrorDes.ToString())
                    End If

                ElseIf Producto.TipoProducto = TipoProducto.CSA Then
                    REM 4.2.- CSA -----------------------------------------------
                    dalProductoCSA = New daProductoCSA
                    Call ParametrizarCamposProductoCsa(Producto.ProductoCSA)

                    'Siempre incluir detalle partes, y ocultarlo en la pantalla
                    Producto.ProductoCSA.IncluyeDetallePartes = "SI" ' o tambien "SI", "YES", "TRUE", "1", "01"

                    Call LogProductoCSA(dalProducto.IdProducto, dalCotizacion.Usuario, Producto.ProductoCSA, Validacion)
                    Call dalProductoCSA.InsertarProductoCSA(Connection, Transaction, dalProducto.IdProducto, dalCotizacion.Usuario, Producto.ProductoCSA)

                    ''1.- La Primera vez la cotizacion desde sap viene siempre sin maquinarias.
                    ''2.- Se insertan las maquinarias de los productos csa en el SP de la base de datos
                    ''3.- Con la tabla MaquinariaEnvioSap se toma las ultimas maquinarias que se a enviado a sap desde la tabla Maquinarias
                    ''4.- Se insertan las ultimas maquinarias de producto csa en la tabla Maquinarias 

                    'If Not (Producto.ProductoCSA.ListaMaquinaria Is Nothing OrElse Producto.ProductoCSA.ListaMaquinaria.Count = 0) Then
                    '    Dim Item As Integer = 0
                    '    For Each Maquinaria As beMaquinaria In Producto.ProductoCSA.ListaMaquinaria
                    '        Item += 1
                    '        dalMaquinaria = New daMaquinaria
                    '        Call LogMaquinaria(dalProducto.IdProducto, Item, dalCotizacion.Usuario, Maquinaria, Validacion)
                    '        Call dalMaquinaria.InsertarMaquinaria(Connection, Transaction, dalProducto.IdProducto, Item, dalCotizacion.Usuario, Maquinaria)
                    '    Next
                    'End If

                ElseIf Producto.TipoProducto = TipoProducto.ALQUILER Then
                    odaProductoAlquiler = New daProductoAlquiler()

                    'Llamar LogProductoAlquiler
                    'Asignar IdProducto Generado
                    Producto.beProductoAlquiler.IdProducto = dalProducto.IdProducto
                    Call LogProductoAlquiler(dalProducto.IdProducto, dalCotizacion.Usuario, Producto.beProductoAlquiler, Validacion)
                    Call odaProductoAlquiler.Insertar(Connection, Producto.beProductoAlquiler, Transaction)

                    'Insertar ProductoAlquilerTarifa
                    odaProductoAlquilerTarifa = New daProductoAlquilerTarifa()
                    For Each obeProductoAlquilerTarifa As beProductoAlquilerTarifa In Producto.beProductoAlquiler.ListaProductoAlquilerTarifa
                        obeProductoAlquilerTarifa.IdProducto = dalProducto.IdProducto
                        obeProductoAlquilerTarifa.UsuarioCreacion = dalCotizacion.Usuario
                        obeProductoAlquilerTarifa.ValorEscala = uConfig.fc_ConvertirEntero(obeProductoAlquilerTarifa.ValorEscala)
                        If obeProductoAlquilerTarifa.ValorEscala <> 0 Then
                            Call LogProductoAlquilerTarifa(dalProducto.IdProducto, dalCotizacion.Usuario, obeProductoAlquilerTarifa, Validacion)
                            Call odaProductoAlquilerTarifa.Insertar(Connection, obeProductoAlquilerTarifa, Transaction)
                        End If
                    Next

                    'Insertar lista de ProductoAdicional
                    odaProductoAdicional = New daProductoAdicional
                    For Each obeProductoAdicional As beProductoAdicional In Producto.beProductoAlquiler.ListabeProductoAdicional
                        'Asignar IdProducto Generado
                        obeProductoAdicional.IdProducto = dalProducto.IdProducto
                        Call LogProductoAdicional(dalProducto.IdProducto, dalCotizacion.Usuario, obeProductoAdicional, Validacion)
                        Call odaProductoAdicional.Insertar(Connection, obeProductoAdicional, Transaction)
                    Next

                    'Insertar Lista de ProductoAccesorio
                    odaProductoAccesorio = New daProductoAccesorio
                    For Each obeProductoAccesorio As beProductoAccesorio In Producto.beProductoAlquiler.ListabeProductoAccesorio
                        'Asignar IdProducto Generado
                        obeProductoAccesorio.IdProducto = dalProducto.IdProducto
                        obeProductoAccesorio.UsuarioCreacion = dalCotizacion.Usuario
                        Call LogProductoAccesorio(dalCotizacion.Usuario, obeProductoAccesorio, Validacion)
                        Call odaProductoAccesorio.Insertar(Connection, obeProductoAccesorio, Transaction)
                    Next

                ElseIf Producto.TipoProducto = TipoProducto.SOLUCION_COMBINADA Then
                    odaProductoSolucionCombinada = New daProductoSolucionCombinada

                    'hasta q se cambie el ws_cotizador
                    'Producto.beProductoSolucionCombinada.IdProducto = dalProducto.IdProducto
                    Producto.ProductoPrime.IdProducto = dalProducto.IdProducto

                    Call LogProductoSolucionCombinada(dalProducto.IdProducto, dalCotizacion.Usuario, Producto.ProductoPrime, Validacion)

                    'Call odaProductoSolucionCombinada.Insertar(Connection, Producto.beProductoSolucionCombinada, Transaction)
                    If Not odaProductoSolucionCombinada.Insertar(Connection, Producto.ProductoPrime, Transaction) Then
                        log.Error("Error en odaProductoSolucionCombinada.Insertar " + ": " + odaProductoSolucionCombinada.ErrorDes.ToString())
                    End If

                    'Insertar lista de ProductoAdicional
                    odaProductoAdicional = New daProductoAdicional
                    For Each obeProductoAdicional As beProductoAdicional In Producto.beProductoSolucionCombinada.ListabeProductoAdicional
                        'Asignar IdProducto Generado
                        obeProductoAdicional.IdProducto = dalProducto.IdProducto
                        Call LogProductoAdicional(dalProducto.IdProducto, dalCotizacion.Usuario, obeProductoAdicional, Validacion)
                        Call odaProductoAdicional.Insertar(Connection, obeProductoAdicional, Transaction)
                    Next

                    'Insertar Lista de ProductoAccesorio
                    odaProductoAccesorio = New daProductoAccesorio
                    For Each obeProductoAccesorio As beProductoAccesorio In Producto.beProductoSolucionCombinada.ListabeProductoAccesorio
                        'Asignar IdProducto Generado
                        obeProductoAccesorio.IdProducto = dalProducto.IdProducto
                        obeProductoAccesorio.UsuarioCreacion = dalCotizacion.Usuario
                        Call LogProductoAccesorio(dalCotizacion.Usuario, obeProductoAccesorio, Validacion)
                        Call odaProductoAccesorio.Insertar(Connection, obeProductoAccesorio, Transaction)
                    Next

                End If

                'Insertar las caracteristicas de los productos
                odaProductoCaracteristica = New daProductoCaracteristica
                For Each obeProductoCaracteristica As beProductoCaracteristica In Producto.ListaProductoCaracteristica
                    obeProductoCaracteristica.IdProducto = dalProducto.IdProducto
                    obeProductoCaracteristica.UsuarioCreacion = dalCotizacion.Usuario
                    Call LogProductoCaracteristica(dalProducto.IdProducto, dalCotizacion.Usuario, obeProductoCaracteristica, Validacion)
                    Call odaProductoCaracteristica.Insertar(Connection, obeProductoCaracteristica, Transaction)
                Next

                'Insertar detalle de precio por producto
                '20/10 CS
                odaProductoDetallePrecios = New daProductoDetallePrecios
                For Each obeDetallePrecio As beProductoDetallePrecios In Producto.ListaProductoDetallePrecios
                    obeDetallePrecio.IdProducto = dalProducto.IdProducto
                    Call LogProductoDetallePrecios(obeDetallePrecio, Validacion)
                    Call odaProductoDetallePrecios.Insertar(Connection, obeDetallePrecio, Transaction)
                Next

            Next
            lResult = True
            Transaction.Commit()
        Catch ex As Exception

            If Transaction IsNot Nothing Then
                Transaction.Rollback()
            End If

            ErrorDescripcion = ex.Message.ToString
            log.Error(Validacion.cadenaAleatoria + ": " + ErrorDescripcion)
        Finally

            If Transaction IsNot Nothing Then Transaction.Dispose()

            If Connection IsNot Nothing Then
                If Connection.State = ConnectionState.Open Then Connection.Close()
            End If

        End Try

        Return lResult

    End Function

    Public Function BuscarIdCotizacionSap(ByVal eConexion As String, ByVal obeCotizacionVersion As beCotizacionVersion, ByRef lista As List(Of beCotizacionVersion)) As Boolean

        Dim bol_Retorno As Boolean = False
        odaCotizacionVersion = New daCotizacionVersion

        Try
            Connection = New SqlConnection(eConexion)
            Connection.Open()
            odaCotizacionVersion.BuscarIdCotizacionSap(Connection, obeCotizacionVersion, lista)
            bol_Retorno = True
        Catch ex As Exception

        End Try

        Return bol_Retorno
    End Function

#Region "---Imprimir Log---"

    Private Sub LogCotizacion(ByVal Cotizacion As beCotizacion, ByVal Validacion As beValidacion)
        With Cotizacion
            log.Info(String.Concat(Validacion.cadenaAleatoria, ": ", "IdCotizacion = ", .IdCotizacion, _
                                                            ",IdCorporacion = " + .IdCorporacion, _
                                                            ",IdCompanhia = " + .IdCompanhia, _
                                                            ",IdSolicitante = " + .IdSolicitante, _
                                                            ",DescripSolicitante = " + .DescripSolicitante, _
                                                            ",RUCSolicitante = " + .RUCSolicitante, _
                                                            ",DNISolicitante = " + .DNISolicitante, _
                                                            ",IdPersonaResponsable = " + .IdPersonaResponsable, _
                                                            ",DescripResponsable = " + .DescripResponsable, _
                                                            ",OficinaResponsable = " + .OficinaResponsable, _
                                                            ",CargoResponsable = " + .CargoResponsable, _
                                                            ",EmailResponsable = " + .EmailResponsable, _
                                                            ",TelefonoResponsable = " + .TelefonoResponsable, _
                                                            ",AnexoTelefonoResponsable = " + .AnexoTelefonoResponsable, _
                                                            ",FechaInicioValidez = " + .FechaInicioValidez, _
                                                            ",FechaFinalValidez = " + .FechaFinalValidez, _
                                                            ",FechaPrecio = " + .FechaPrecio, _
                                                            ",FechaEstimadaFacturacion = " + .FechaEstimadaFacturacion, _
                                                            ",NumeroOportunidad = " + .NumeroOportunidad, _
                                                            ",ItemOportunidad = " + .ItemOportunidad, _
                                                            ",Version = " + .Version, _
                                                            ",ValorTipoCambio = " + .ValorTipoCambio, _
                                                            ",MonedaValorNeto = " + .MonedaValorNeto, _
                                                            ",MonedaValorImpuesto = " + .MonedaValorImpuesto, _
                                                            ",MonedaValorBruto = " + .MonedaValorBruto, _
                                                            ",ValorNeto = " + .ValorNeto, _
                                                            ",ValorImpuesto = " + .ValorImpuesto, _
                                                            ",ValorBruto = " + .ValorBruto, _
                                                            ",NombreEstado = " + .NombreEstado, _
                                                            ",TotalValorLista = " + .TotalValorLista, _
                                                            ",TotalValorReal = " + .TotalValorReal, _
                                                            ",TotalDescuentoPorc = " + .TotalDescuentoPorc, _
                                                            ",TotalDescuentoImp = " + .TotalDescuentoImp, _
                                                            ",TotalFlete = " + .TotalFlete, _
                                                            ",TotalValorVenta = " + .TotalValorVenta, _
                                                            ",TotalValorImpuesto = " + .TotalValorImpuesto, _
                                                            ",TotalPrecioVentaFinal = " + .TotalPrecioVentaFinal, _
                                                            ",IdResponsableServicio = " + .IdResponsableServicio, _
                                                            ",NombreResponsableServicio = " + .NombreResponsableServicio, _
                                                            ",IdSupervisorServicio = " + .IdSupervisorServicio, _
                                                            ",NombreSupervisorServicio = " + .NombreSupervisorServicio, _
                                                            ",Otros = " + .Otros, _
                                                            ",Usuario = " + .Usuario))


            ''Cambiado para corregir envio desde sap
            '   ",ValorNeto = " + .ValorNeto, _
            '    ",ValorBruto = " + .ValorBruto,_


        End With
    End Sub

    Private Sub LogCotizacionContacto(ByVal CotizacionContacto As beCotizacionContacto, ByVal Validacion As beValidacion)
        With CotizacionContacto
            log.Info(String.Concat(Validacion.cadenaAleatoria, ": ", "Nombres = ", .Nombres, _
                                                         ",Direccion = ", .Direccion, _
                                                         ",Email = ", .Email, _
                                                         ",Cargo = ", .Cargo, _
                                                         ",Telefono = ", .Telefono))
        End With
    End Sub

    Private Sub LogTelefonoResponsable(ByVal TelefonoResponsable As beTelefonoResponsable, ByVal Validacion As beValidacion)
        With TelefonoResponsable
            log.Info(String.Concat(Validacion.cadenaAleatoria, ": ", "codTelefono = ", .CodTipoTelefono, _
                                                         ",NroTelefono = ", .NroTelefono, _
                                                         ",Anexo = ", .Anexo))
        End With
    End Sub

    Private Sub LogProducto(ByVal IdCotizacion As Integer, ByVal Usuario As String, ByVal Producto As beProducto, ByVal Validacion As beValidacion)
        With Producto

            log.Info(String.Concat(Validacion.cadenaAleatoria, ": ", ",IdCotizacion = ", IdCotizacion, _
          ",IdPosicionSAP = ", .IdPosicion, _
          ",IdProductoSAP = ", .IdProducto, _
          ",TipoProducto = ", .TipoProducto, _
          ",Descripcion = ", .Descripcion, _
          ",ValorUnitario = ", .ValorUnitario, _
          ",IdMonedaValorUnitario = ", .IdMonedaValorUnitario, _
          ",Cantidad = ", .Cantidad, _
          ",Unidad = ", .Unidad, _
          ",ValorNeto = ", .ValorNeto, _
          ",IdMonedaValorNeto = ", .IdMonedaValorNeto, _
          ",CostoProducto = ", .CostoProducto, _
          ",MonedaCostoProducto = ", .MonedaCostoProducto, _
          ",MonedaCotizacion = ", .MonedaCotizacion, _
          ",ValorLista = ", .ValorLista, _
          ",ValorReal = ", .ValorReal, _
          ",PorcDescuento = ", .PorcDescuento, _
          ",DescuentoImp = ", .DescuentoImp, _
          ",Flete = ", .Flete, _
          ",ValorVenta = ", .ValorVenta, _
          ",PorcImpuesto = ", .PorcImpuesto, _
          ",ValorImpuesto = ", .ValorImpuesto, _
          ",PrecioVentaFinal = ", .PrecioVentaFinal, _
          ",IdEstado = ", .IdEstado, _
          ",NombreEstado = ", .NombreEstado, _
          ",DescripcionModelo = ", .DescripcionModelo, _
          ",IdMarca = ", .IdMarca, _
          ",NombreMarca = ", .NombreMarca, _
          ",Otros = ", .Otros, _
          ",DescripGarantia =", .DescripGarantia, _
          ",CodigoFamilia =", .CodigoFamilia, _
          ",DescripcionFamilia =", .DescripcionFamilia, _
          ",CodigoFormaPago =", .CodigoFormaPago, _
          ",DescripFormaPago =", .DescripFormaPago, _
          ",CodigoLinea =", .CodigoLinea, _
          ",NombreLinea =", .NombreLinea, _
          ",Usuario = ", Usuario))

        End With
    End Sub

    Private Sub LogProductoCSA(ByVal IdProducto As Integer, ByVal Usuario As String, ByVal ProductoCSA As beProductoCSA, ByVal Validacion As beValidacion)
        With ProductoCSA
            log.Info(String.Concat(Validacion.cadenaAleatoria, ": ", "IdProducto = ", IdProducto, _
                                                                    ",ClaseCsa = ", .ClaseCsa, _
                                                                    ",TipoCotizacion = ", .TipoCotizacion, _
                                                                    ",IdTipoCSA = ", .IdTipoCSA, _
                                                                    ",IdPlan = ", .IdPlan, _
                                                                    ",DescripcionPlan = ", .DescripcionPlan, _
                                                                    ",IdUnidadDuracion = ", .IdUnidadDuracion, _
                                                                    ",Duracion = ", .Duracion, _
                                                                    ",Tiempo = ", .Tiempo, _
                                                                    ",IdUnidadPlazoRenovacion = ", .IdUnidadPlazoRenovacion, _
                                                                    ",PlazoRenovacion = ", .PlazoRenovacion, _
                                                                    ",IncluyeFluidos = ", .IncluyeFluidos, _
                                                                    ",IncluyeDetallePartes = ", .IncluyeDetallePartes, _
                                                                    ",FechaInicioContrato = ", .FechaInicioContrato, _
                                                                    ",FechaEstimadaCierre = ", .FechaEstimadaCierre, _
                                                                    ",ParticipacionVendedor1 = ", .ParticipacionVendedor1, _
                                                                    ",ParticipacionVendedor2 = ", .ParticipacionVendedor2, _
                                                                    ",Usuario = ", Usuario))
        End With
    End Sub

    Private Sub LogMaquinaria(ByVal IdProducto As Integer, ByVal Item As Integer, ByVal Usuario As String, ByVal Maquinaria As beMaquinaria, ByVal Validacion As beValidacion)

        With Maquinaria
            log.Info(String.Concat(Validacion.cadenaAleatoria, ": ", "IdProducto = ", IdProducto, _
                                                                    ",Item = ", Item, _
                                                                    ",Familia = ", .Familia, _
                                                                    ",ModeloBase = ", .ModeloBase, _
                                                                    ",Modelo = ", .Modelo, _
                                                                    ",Prefijo = ", .Prefijo, _
                                                                    ",MaquinaNueva = ", .MaquinaNueva, _
                                                                    ",NumeroSerie = ", .NumeroSerie, _
                                                                    ",HorometroInicial = ", .HorometroInicial, _
                                                                    ",FechaHorometro = ", .FechaHorometro, _
                                                                    ",HorasPromedioMensual = ", .HorasPromedioMensual, _
                                                                    ",HorometroFinal = ", .HorometroFinal, _
                                                                    ",Renovacion = ", .Renovacion, _
                                                                    ",RenovacionValida = ", .RenovacionValida, _
                                                                    ",CodDepartamento = ", .CodDepartamento, _
                                                                    ",Departamento = ", .Departamento, _
                                                                    ",Usuario = ", Usuario))
        End With

    End Sub

    Private Sub LogProductoPrime(ByVal IdProducto As Integer, ByVal Usuario As String, ByVal obeProductoPrime As beProductoPrime, ByVal Validacion As beValidacion)
        With obeProductoPrime
            log.Info(String.Concat(Validacion.cadenaAleatoria, ": ", ",IdProducto = ", .IdProducto, _
                                                                    ",FechaEstimCierre = ", .FechaEstimCierre, _
                                                                    ",PlazoEntregaEstim  = ", .PlazoEntregaEstim, _
                                                                    ",CodigoFormaPago = ", .CodigoFormaPago, _
                                                                    ",FormaPago = ", .FormaPago, _
                                                                    ",FlatIncluyeRecompra  = ", .FlatIncluyeRecompra, _
                                                                    ",FlatIncluyeCLC  = ", .FlatIncluyeCLC, _
                                                                    ",PromHorasMensualUso  = ", .PromHorasMensualUso, _
                                                                    ",AnioFabricacion  = ", .AnioFabricacion, _
                                                                    ",AnioModelo  = ", .AnioModelo, _
                                                                    ",Garantia  = ", .Garantia, _
                                                                    ",Condicion  = ", .Condicion, _
                                                                    ",PlazoEntrega  = ", .PlazoEntrega, _
                                                                    ",Orden  = ", .Orden, _
                                                                    ",Serie  = ", .Serie, _
                                                                    ",Horas  = ", .Horas, _
                                                                    ",CodClasificacion  = ", .CodClasificacion, _
                                                                    ",Interlote  = ", .Interlote, _
                                                                    ",UsuarioCreacion  = ", .UsuarioCreacion))


        End With
    End Sub

    Private Sub LogProductoAlquiler(ByVal IdProducto As Integer, ByVal Usuario As String, ByVal obeProductoAlquiler As beProductoAlquiler, ByVal Validacion As beValidacion)
        With obeProductoAlquiler
            log.Info(String.Concat(Validacion.cadenaAleatoria, ": ", ",IdProducto = ", .IdProducto, _
                                                                    ",CodTipoAlquiler = ", .CodTipoAlquiler, _
                                                                    ",DesTipoAlquiler  = ", .DesTipoAlquiler, _
                                                                    ",CodTipoPago = ", .CodTipoPago, _
                                                                    ",DesTipoPago = ", .DesTipoPago, _
                                                                    ",CodTipoFacturacion  = ", .CodTipoFacturacion, _
                                                                    ",DesTipoFacturacion  = ", .DesTipoFacturacion, _
                                                                    ",CodMesAlquilar  = ", .CodMesAlquilar, _
                                                                    ",DesMesAlquilar  = ", .DesMesAlquilar,
                                                                    ",CodigoTipoArrendamiento  = ", .CodigoTipoArrendamiento,
                                                                    ",DescripcionTipoArrendamiento  = ", .DescripcionTipoArrendamiento,
                                                                    ",CodigoTipoAlquiler  = ", .CodigoTipoAlquiler,
                                                                    ",DescripcionTipoAlquiler  = ", .DescripcionTipoAlquiler,
                                                                    ",FechaInicioArrendamiento  = ", .FechaInicioArrendamiento,
                                                                    ",NroMaquinasMeses  = ", .NroMaquinasMeses,
                                                                    ",ValorAdicionalHora  = ", .ValorAdicionalHora,
                                                                    ",HorasUsoMensual  = ", .HorasUsoMensual,
                                                                    ",UsuarioCreacion  = ", .UsuarioCreacion))

        End With
    End Sub

    Private Sub LogProductoAlquilerTarifa(ByVal IdProducto As Integer, ByVal Usuario As String, ByVal obeProductoAlquilerTarifa As beProductoAlquilerTarifa, ByVal Validacion As beValidacion)
        With obeProductoAlquilerTarifa
            log.Info(String.Concat(Validacion.cadenaAleatoria, ": ", ",IdProducto= ", .IdProducto, _
                                                                     ",ValorEscala= ", .ValorEscala, _
                                                                     ",Importe= ", .Importe, _
                                                                     ",Moneda= ", .Moneda, _
                                                                     ",UnidadMedidaPrecio= ", .UnidadMedidaPrecio, _
                                                                     ",CodigoUnidadMedida= ", .CodigoUnidadMedida, _
                                                                     ",FlatEliminado= ", .FlatEliminado, _
                                                                     ",UsuarioCreacion= ", .UsuarioCreacion))




        End With
    End Sub

    Private Sub LogProductoCaracteristica(ByVal IdProducto As Integer, ByVal Usuario As String, ByVal obeProductoCaracteristica As beProductoCaracteristica, ByVal Validacion As beValidacion)
        With obeProductoCaracteristica
            log.Info(String.Concat(Validacion.cadenaAleatoria, ": ", ",IdProducto= ", .IdProducto, _
                                                                    ",Posicion= ", .Posicion, _
                                                                    ",CodigoAtributo= ", .CodigoAtributo, _
                                                                    ",DescripcionAtributo= ", .DescripcionAtributo, _
                                                                    ",ValorAtributo= ", .ValorAtributo, _
                                                                    ",CodigoUnidadMedida= ", .CodigoUnidadMedida, _
                                                                    ",NombreUnidadMedida= ", .NombreUnidadMedida, _
                                                                    ",FlatEliminado= ", .FlatEliminado, _
                                                                    ",UsuarioCreacion= ", .UsuarioCreacion))




        End With
    End Sub

    Private Sub LogProductoDetallePrecios(ByVal obeProductoDetallePrecios As beProductoDetallePrecios, ByVal Validacion As beValidacion)
        With obeProductoDetallePrecios
            log.Info(String.Concat(Validacion.cadenaAleatoria, ": ", ",IdProducto= ", .IdProducto, _
                                                                    ",IdProducto= ", .IdProducto, _
                                                                    ",Codigo= ", .Codigo, _
                                                                    ",Estado= ", .Estado, _
                                                                    ",ElementoPrecio= ", .ElementoPrecio, _
                                                                    ",Precio= ", .Precio, _
                                                                    ",Unidad= ", .Unidad, _
                                                                    ",Cantidad= ", .Cantidad, _
                                                                    ",ValorFinal= ", .ValorFinal))




        End With
    End Sub

    Private Sub LogDetalleRecompra(ByVal IdProducto As Integer, ByVal Usuario As String, ByVal obeDetalleRecompra As beDetalleRecompra, ByVal Validacion As beValidacion)
        With obeDetalleRecompra
            log.Info(String.Concat(Validacion.cadenaAleatoria, ": ", ",IdDetalleRecompra = ", .IdDetalleRecompra, _
                                                                    ",IdProducto = ", .IdProducto, _
                                                                    ",Numero = ", .Numero, _
                                                                    ",NumeroHoras = ", .NumeroHoras, _
                                                                    ",NumeroMeses = ", .NumeroMeses, _
                                                                    ",MontoRecompra = ", .MontoRecompra, _
                                                                    ",Moneda = ", .Moneda, _
                                                                    ",FlatEliminado = ", .FlatEliminado, _
                                                                    ",UsuarioCreacion = ", .UsuarioCreacion))

        End With
    End Sub
    Private Sub LogProductoAdicional(ByVal IdProducto As Integer, ByVal Usuario As String, ByVal obeProductoAdicional As beProductoAdicional, ByVal Validacion As beValidacion)
        With obeProductoAdicional
            log.Info(String.Concat(Validacion.cadenaAleatoria, ": ", ",IdProductoAdicional = ", .IdProductoAdicional, _
                                                                    ",IdProducto = ", .IdProducto, _
                                                                    ",IdAdicional = ", .IdAdicional, _
                                                                    ",CodigoProductoAdicional = ", .CodigoProductoAdicional, _
                                                                    ",NombreProdutoAdicional = ", .NombreProdutoAdicional, _
                                                                    ",Cantidad = ", .Cantidad, _
                                                                    ",UnidadMedida = ", .UnidadMedida, _
                                                                    ",ValorCosto = ", .ValorCosto, _
                                                                    ",ValorLista = ", .ValorLista, _
                                                                    ",MonedaValorCosto = ", .MonedaValorCosto, _
                                                                    ",MonedaValorLista = ", .MonedaValorLista, _
                                                                    ",TipoAdicional = ", .TipoAdicional, _
                                                                    ",FlatMostrarEspTecnica = ", .FlatMostrarEspTecnica, _
                                                                    ",UsuarioCreacion = ", .UsuarioCreacion))

        End With
    End Sub
    Private Sub LogProductoAccesorio(ByVal Usuario As String, ByVal obeProductoAccesorio As beProductoAccesorio, ByVal Validacion As beValidacion)
        With obeProductoAccesorio
            log.Info(String.Concat(Validacion.cadenaAleatoria, ": ", ",IdProductoAccesorio = ", .IdProductoAccesorio, _
                                                                    ",IdProducto = ", .IdProducto, _
                                                                    ",IdAccesorio = ", .IdAccesorio, _
                                                                    ",CodigoProductoAccesorio = ", .CodigoProductoAccesorio, _
                                                                    ",NombreProductoAccesorio = ", .NombreProductoAccesorio, _
                                                                    ",Cantidad = ", .Cantidad, _
                                                                    ",UnidadMedida = ", .UnidadMedida, _
                                                                    ",ValorLista = ", .ValorLista, _
                                                                    ",MonedaValorLista = ", .MonedaValorLista, _
                                                                    ",FlatMostrarEspTecnica = ", .FlatMostrarEspTecnica, _
                                                                    ",FlatEliminado = ", .FlatEliminado, _
                                                                    ",UsuarioCreacion = ", .UsuarioCreacion))



        End With
    End Sub
    Private Sub LogProductoSolucionCombinada(ByVal IdProducto As Integer, ByVal Usuario As String, ByVal obeProductoSolucionCombinada As beProductoPrime, ByVal Validacion As beValidacion)
        With obeProductoSolucionCombinada
            log.Info(String.Concat(Validacion.cadenaAleatoria, ": ", "IdProducto = ", .IdProducto, _
                                                                    ",Año Fabicación = ", .AnioFabricacion, _
                                                                    ",Cod Fabricacion = ", .CodClasificacion, _
                                                                    ",Cod Formato Pago = ", .CodigoFormaPago, _
                                                                    ",Condicion = ", .Condicion, _
                                                                    ",Fecha Estim Cierre = ", .FechaEstimCierre, _
                                                                    ",Flat Incluye CLC = ", .FlatIncluyeCLC, _
                                                                    ",Flat Incluye Recompra = ", .FlatIncluyeRecompra, _
                                                                    ",Forma Pago = ", .FormaPago, _
                                                                    ",Garantia = ", .Garantia, _
                                                                    ",Horas = ", .Horas, _
                                                                    ",IdProducto = ", .IdProducto, _
                                                                    ",Interlote = ", .Interlote, _
                                                                    ",Serie = ", .Serie, _
                                                                    ",Orden = ", .Orden, _
                                                                    ",UsuarioCreacion = ", .UsuarioCreacion))

        End With
    End Sub
#End Region

    Private Sub ValidarCotizacion(ByVal Cotizacion As beCotizacion)

        With Cotizacion

            If IsNothing(.IdCotizacion) OrElse String.IsNullOrEmpty(.IdCotizacion) Then
                Throw New Exception("Debe ingresar el IdCotizacion")
            End If

            If Not IsNothing(.FechaInicioValidez) AndAlso _
               Not String.IsNullOrEmpty(.FechaInicioValidez) AndAlso _
               .FechaInicioValidez.Length <> 8 Then
                Throw New Exception("Debe ingresar FechaInicioValidez valida")
            End If

            If Not IsNothing(.FechaFinalValidez) AndAlso _
               Not String.IsNullOrEmpty(.FechaFinalValidez) AndAlso _
               .FechaFinalValidez.Length <> 8 Then
                Throw New Exception("Debe ingresar FechaInicioValidez valida")
            End If

            If Not IsNothing(.FechaPrecio) AndAlso _
               Not String.IsNullOrEmpty(.FechaPrecio) AndAlso _
               .FechaPrecio.Length <> 8 Then
                Throw New Exception("Debe ingresar FechaPrecio valida")
            End If

            If Not IsNothing(.FechaEstimadaFacturacion) AndAlso _
               Not String.IsNullOrEmpty(.FechaEstimadaFacturacion) AndAlso _
               .FechaEstimadaFacturacion.Length <> 8 Then
                Throw New Exception("Debe ingresar FechaEstimadaFacturacion valida")
            End If

            If Not IsNothing(.Version) AndAlso _
               Not String.IsNullOrEmpty(.Version) AndAlso _
               Not IsNumeric(.Version) Then
                Throw New Exception("Debe ingresar Version valida")
            End If

            If Not IsNothing(.ValorTipoCambio) AndAlso _
               Not String.IsNullOrEmpty(.ValorTipoCambio) AndAlso _
               Not IsNumeric(.ValorTipoCambio) Then
                Throw New Exception("Debe ingresar ValorTipoCambio valida")
            End If

            If Not IsNothing(.ValorNeto) AndAlso _
                Not String.IsNullOrEmpty(.ValorNeto) AndAlso _
                Not IsNumeric(.ValorNeto) Then
                Throw New Exception("Debe ingresar ValorNeto valida")
            End If

            If Not IsNothing(.ValorImpuesto) AndAlso _
                Not String.IsNullOrEmpty(.ValorImpuesto) AndAlso _
                Not IsNumeric(.ValorImpuesto) Then
                Throw New Exception("Debe ingresar ValorImpuesto valida")
            End If

            If Not IsNothing(.ValorBruto) AndAlso _
                Not String.IsNullOrEmpty(.ValorBruto) AndAlso _
                Not IsNumeric(.ValorBruto) Then
                Throw New Exception("Debe ingresar ValorBruto valida")
            End If

            If IsNothing(.Usuario) OrElse String.IsNullOrEmpty(.Usuario) Then
                Throw New Exception("Debe ingresar el Usuario")
            End If

            If IsNothing(.ListaProducto) OrElse .ListaProducto.Count = 0 Then
                Throw New Exception("La Cotizacion no tiene productos para insertar")
            Else
                For Each Producto As beProducto In .ListaProducto
                    If IsNothing(Producto.IdPosicion) OrElse String.IsNullOrEmpty(Producto.IdPosicion) Then
                        Throw New Exception("Producto, Debe ingresar el IdPosicion valida")
                    End If
                    If IsNothing(Producto.IdProducto) OrElse String.IsNullOrEmpty(Producto.IdProducto) Then
                        Throw New Exception("Producto, Debe ingresar el IdProducto valida")
                    End If
                    If Not IsNothing(Producto.ValorUnitario) AndAlso _
                        Not String.IsNullOrEmpty(Producto.ValorUnitario) AndAlso _
                        Not IsNumeric(Producto.ValorUnitario) Then
                        Throw New Exception("Producto, Debe ingresar ValorUnitario valida")
                    End If
                    If Not IsNothing(Producto.Cantidad) AndAlso _
                       Not String.IsNullOrEmpty(Producto.Cantidad) AndAlso _
                       Not IsNumeric(Producto.Cantidad) Then
                        Throw New Exception("Producto, Debe ingresar Cantidad valida")
                    End If
                    If Not IsNothing(Producto.ValorNeto) AndAlso _
                       Not String.IsNullOrEmpty(Producto.ValorNeto) AndAlso _
                       Not IsNumeric(Producto.ValorNeto) Then
                        Throw New Exception("Producto, Debe ingresar ValorNeto valida")
                    End If
                Next
            End If

        End With

    End Sub
    Private Sub ParametrizarCamposProductoCsa(ByRef ProductoCSA As beProductoCSA)

        Dim TipoCotizacion As String = ProductoCSA.TipoCotizacion
        Dim IdTipoClaseCsa As String = ProductoCSA.IdTipoCSA
        Dim ClaseCsa As String = String.Empty


        'Homologar Tipo de Cotizacion
        If TipoCotizacion = "01" Then
            TipoCotizacion = ParameterTipoCotizacion.HeavyConstruction
        ElseIf TipoCotizacion = "02" Then
            TipoCotizacion = ParameterTipoCotizacion.GeneradorPrime
        ElseIf TipoCotizacion = "03" Then
            TipoCotizacion = ParameterTipoCotizacion.GeneradorStandBy
        ElseIf TipoCotizacion = "04" Then
            TipoCotizacion = ParameterTipoCotizacion.Monitoreo
        End If

        ProductoCSA.TipoCotizacion = TipoCotizacion


        'Homologar Clase CSA
        Dim lengclase As Integer = 0
        lengclase = clsGeneral.TipoClaseCSA.Plan.ToString.Length
        If IdTipoClaseCsa.Length >= lengclase Then
            IdTipoClaseCsa = IdTipoClaseCsa.Substring(0, lengclase)
        End If

        Select Case IdTipoClaseCsa
            Case clsGeneral.TipoClaseCSA.Plan
                ClaseCsa = clsGeneral.ClaseCSA.Planes
                Exit Select
            Case clsGeneral.TipoClaseCSA.Acuerdo
                ClaseCsa = clsGeneral.ClaseCSA.Acuerdos
                Exit Select
        End Select
        ProductoCSA.ClaseCsa = ClaseCsa

    End Sub

    Private Structure ParameterTipoCotizacion
        Public Shared HeavyConstruction As String = "H"
        Public Shared GeneradorPrime As String = "P"
        Public Shared GeneradorStandBy As String = "S"
        Public Shared Monitoreo As String = "M"
    End Structure

    Public Sub HomologarCamposCotizacion(ByVal eConexion As String, ByRef Cotizacion As beCotizacion, ByVal Validacion As beValidacion)
        Dim IdCorporacion As String = Cotizacion.IdCorporacion
        Dim IdCompanhia As String = Cotizacion.IdCompanhia
        Dim OficinaResponsable As String = Cotizacion.OficinaResponsable

        Dim CodCompaniaAS400 As String = String.Empty
        Dim CodCorporacionAS400 As String = String.Empty
        Dim NombreOficinaResponsableAS400 As String = String.Empty

        ' Contenedor de Objetos Principales
        Dim tablaDatos As New Dictionary(Of String, Object)

        Call fnCargarDatosXml(UrlServicio, tablaDatos, eConexion)

        'Homologando codigo Coorporacion
        Try
            If Not String.IsNullOrEmpty(IdCorporacion) Then
                If tablaDatos.ContainsKey("dsCorporacion") Then
                    For Each dr As DataRow In tablaDatos("dsCorporacion").Tables("Corporacion").Select("TELEX='" & IdCorporacion.Trim & "'")
                        CodCorporacionAS400 = dr("CODIGO").ToString.Trim
                    Next

                    If Not String.IsNullOrEmpty(CodCorporacionAS400) Then
                        Cotizacion.IdCorporacion = CodCorporacionAS400
                    Else
                        Cotizacion.IdCorporacion = "001"
                    End If

                End If
            End If
        Catch ex As Exception
            log.Error(Validacion.cadenaAleatoria + ": " + ex.Message.ToString)
        End Try

        'homologando codigo de compania
        Try
            If IdCompanhia.Trim <> "" Then
                IdCompanhia = IdCompanhia.Replace("O", "").Trim
                If tablaDatos.ContainsKey("dsCompania") Then
                    For Each dr As DataRow In tablaDatos("dsCompania").Tables("Compania").Select("TELEX='" & IdCompanhia.Trim & "'")
                        CodCompaniaAS400 = dr("CODIGO").ToString.Trim
                    Next

                    If Not String.IsNullOrEmpty(CodCompaniaAS400) Then
                        Cotizacion.IdCompanhia = CodCompaniaAS400
                    Else
                        Cotizacion.IdCompanhia = "000000000002"
                    End If
                End If

            End If
        Catch ex As Exception
            log.Error(Validacion.cadenaAleatoria + ": " + ex.Message.ToString)
        End Try


        'Homologando Codigo de Oficina
        Try

            If OficinaResponsable.Trim <> "" Then
                If tablaDatos.ContainsKey("dsSucursal") Then
                    For Each dr As DataRow In tablaDatos("dsSucursal").Tables("Sucursal").Select("TELEX='" & OficinaResponsable.Trim & "'")
                        NombreOficinaResponsableAS400 = dr("DESCRIPN").ToString.Trim
                    Next
                    If Not String.IsNullOrEmpty(NombreOficinaResponsableAS400) Then
                        Cotizacion.OficinaResponsable = NombreOficinaResponsableAS400
                    Else
                        'Cotizacion.OficinaResponsable = "01"
                        Cotizacion.OficinaResponsable = "LIMA"
                    End If
                End If

            End If
        Catch ex As Exception
            log.Error(Validacion.cadenaAleatoria + ": " + ex.Message.ToString)
        End Try

    End Sub
#Region "Datos XML"
    Public Function fnIniciarDia(ByVal ruta As String) As Boolean

        fnIniciarDia = True

        Dim file As FileInfo

        Dim dt As DataTable
        Dim dr As DataRow

        Try
            file = New FileInfo(ruta & "\" & "StartDay.xml")

            If file.Exists Then

                dt = New DataTable
                dt.ReadXmlSchema(ruta & "\" & "StartDay.xml")
                dt.ReadXml(ruta & "\" & "StartDay.xml")

                Dim pst_fecha As Date = Today.ToShortDateString
                Dim Directorio As New DirectoryInfo(ruta)

                ' ARCHIVOS
                Dim archivos() As FileInfo = Directorio.GetFiles

                If dt.Rows.Count > 0 Then

                    If Convert.ToDateTime(dt.Rows(0)("fecha")) < pst_fecha Then

                        ' ACTUALIZAR FECHA DEL DIA
                        dt.Rows(0)("fecha") = pst_fecha

                        log.Error("Inicio de  ELIMINAR ARCHIVOS " + ": " & DateTime.Now & " =============")
                        ' ELIMINAR ARCHIVOS
                        For i As Integer = 0 To archivos.Length - 1

                            If archivos(i).Name.ToString.Contains(".xml") Then

                                archivos(i).Delete()

                            End If

                        Next
                        log.Error("Inicio de  CREA EL ARCHIVO " + ": " & DateTime.Now & " =============")
                        ' CREA EL ARCHIVO
                        dt.TableName = "StartDay"
                        dt.WriteXml(ruta & "\" & "StartDay.xml", XmlWriteMode.WriteSchema)

                    End If

                Else

                    ' ACTUALIZAR NUEVAMENTE EL ARCHIVO CON FECHA
                    dr = dt.NewRow
                    dr("fecha") = Today.ToShortDateString()
                    dt.Rows.Add(dr)

                    ' CREANDO ARCHIVO FISICO
                    dt.TableName = "StartDay"
                    dt.WriteXml(ruta & "\" & "StartDay.xml", XmlWriteMode.WriteSchema)

                    ' ELIMINAR ARCHIVOS
                    For i As Integer = 0 To archivos.Length - 1

                        If archivos(i).Name.ToString.Contains(".xml") Then

                            archivos(i).Delete()

                        End If

                    Next

                End If

            Else

                ' CONTENEDOR ORIGEN
                dt = New DataTable
                dt.Columns.Add("fecha")

                dr = dt.NewRow
                dr("fecha") = Today.ToShortDateString()
                dt.Rows.Add(dr)

                ' CREANDO ARCHIVO FISICO
                dt.TableName = "StartDay"
                dt.WriteXml(ruta & "\" & "StartDay.xml", XmlWriteMode.WriteSchema)

            End If

        Catch ex As Exception

            fnIniciarDia = False

        End Try

        Return fnIniciarDia

    End Function
    Public Function fnCargarDatosXml(ByVal ruta As String, ByRef tablas As Dictionary(Of String, Object), ByVal eConexion As String) As Boolean
        Dim boolRetorno As Boolean = False
        Dim odaConsultaAS400 As New daConsultaAS400
        'Iniciar: Borrar todo los dias los archivos Xml 
        Call fnIniciarDia(UrlServicio)

        Try
            ' Contenedor de Variables
            Dim ds As New DataSet
            Dim file As FileInfo

            'Data de Coorporacion
            If Not tablas.ContainsKey("dsCorporacion") Then

                file = New FileInfo(ruta & "\" & "dsCorporacion.xml")

                If file.Exists Then

                    ds = New DataSet
                    ds.ReadXmlSchema(ruta & "\" & "dsCorporacion.xml")
                    ds.ReadXml(ruta & "\" & "dsCorporacion.xml")

                    ' CONTENEDOR DE MEMORIA
                    tablas.Add("dsCorporacion", ds)

                Else

                    tablas.Add("dsCorporacion", odaConsultaAS400.ListarCorporacion(eConexion))

                    ' CREANDO ARCHIVO FISICO
                    'pst_oTablas("dsTVia").TableName = "dsTVia"
                    tablas("dsCorporacion").WriteXml(ruta & "\" & "dsCorporacion.xml", XmlWriteMode.WriteSchema)

                End If

            End If

            'Data de Compañia
            If Not tablas.ContainsKey("dsCompania") Then

                file = New FileInfo(ruta & "\" & "dsCompania.xml")

                If file.Exists Then

                    ds = New DataSet
                    ds.ReadXmlSchema(ruta & "\" & "dsCompania.xml")
                    ds.ReadXml(ruta & "\" & "dsCompania.xml")

                    ' CONTENEDOR DE MEMORIA
                    tablas.Add("dsCompania", ds)

                Else

                    tablas.Add("dsCompania", odaConsultaAS400.ListarCompania(eConexion))

                    ' CREANDO ARCHIVO FISICO
                    tablas("dsCompania").WriteXml(ruta & "\" & "dsCompania.xml", XmlWriteMode.WriteSchema)

                End If

            End If


            'Data de Oficina
            If Not tablas.ContainsKey("dsSucursal") Then

                file = New FileInfo(ruta & "\" & "dsSucursal.xml")

                If file.Exists Then

                    ds = New DataSet
                    ds.ReadXmlSchema(ruta & "\" & "dsSucursal.xml")
                    ds.ReadXml(ruta & "\" & "dsSucursal.xml")

                    ' CONTENEDOR DE MEMORIA
                    tablas.Add("dsSucursal", ds)

                Else

                    tablas.Add("dsSucursal", odaConsultaAS400.ListarSucursal(eConexion))

                    ' CREANDO ARCHIVO FISICO
                    'pst_oTablas("dsTVia").TableName = "dsTVia"
                    tablas("dsSucursal").WriteXml(ruta & "\" & "dsSucursal.xml", XmlWriteMode.WriteSchema)

                End If

            End If
            boolRetorno = True
        Catch ex As Exception

        End Try

        Return boolRetorno

    End Function
#End Region

End Class
