Imports Ferreyros.BECotizador
Imports Ferreyros.BCCotizador
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros
Imports System.Web.Script.Serialization
Imports System.Globalization
Imports System.IO
Public Class frmCotizacionRegistro
    Inherits System.Web.UI.Page

#Region "---------- Declaracion ----------"

    Private miContexto As Base.Interfase.MiUsuario
    
    Private Shared ItemFila As Integer
     
    Private Structure oOtros
        Public Const codigo As String = "0"
        Public Const descripcion As String = "<< OTROS >>"
    End Structure


    Private Shared ReadOnly Property Nomb_Usuario() As String
        Get
            Return "Usuario_"
        End Get
    End Property
    Private Shared ReadOnly Property Nomb_Cotizacion() As Object
        Get
            Return "Cotizacion_"
        End Get
    End Property

    Private Shared ReadOnly Property Nomb_Respaldo() As String
        Get
            Return "Respaldo_"
        End Get
    End Property

    Private Shared ReadOnly Property Nomb_VerLista() As String
        Get
            Return "VerLista_"
        End Get
    End Property

    Private Shared ReadOnly Property Nomb_Producto() As String
        Get
            Return "ProductoCSA_"
        End Get
    End Property

    Private Shared ReadOnly Property Nomb_TarifaRS_Respaldo() As String
        Get
            Return "TarifaRSRespaldo_"
        End Get
    End Property

    Private Shared ReadOnly Property Nomb_TarifaRS() As String
        Get
            Return "TarifaRS_"
        End Get
    End Property

#End Region

#Region "----------  Funciones  ----------"

    Private Function fc_ConsultarDatosCotizacion(ByVal IdCotizacionSap As String, ByVal IdRequestCliente As String) As beValidacion

        Dim eValidacion As New beValidacion
        eValidacion.validacion = True

        ' ----------------- Consultar CotizacionContacto ----------------- '
        Dim l_CotizacionContacto As New List(Of beCotizacionContacto)
        l_CotizacionContacto = fc_ConsultarContacto(IdCotizacionSap)
        If Not eValidacion.validacion Then GoTo Terminar

        ' --------------- Consultar Lista de Productos     --------------- '
        Dim l_Producto As New List(Of beProducto)
        l_Producto = fc_ConsultarProducto(IdCotizacionSap)
        If Not eValidacion.validacion Then GoTo Terminar

        ' --------------- Consultar Lista de SolucionCombinada     --------------- '
        Dim l_SolucionCombinada As New List(Of beSolucionCombinada)
        l_SolucionCombinada = fc_ConsultarSolucionCombinada(IdCotizacionSap, l_Producto)
        If Not eValidacion.validacion Then GoTo Terminar

        If Not IsNothing(l_Producto) Then
            Dim obEstructura As New Utiles.Estructuras.TipoProducto
            For Each Rpt As beProducto In l_Producto
                Select Case Rpt.TipoProducto
                    Case TipoProducto.PRIME  ' Prime
                        fc_ConsultarProductoPrime(Rpt)
                        Exit Select
                    Case TipoProducto.SOLUCION_COMBINADA
                        fc_ConsultarProductoSolucionCombinada(Rpt)
                        Exit Select
                    Case TipoProducto.CSA ' CSA
                        Rpt.ProductoCSA = fc_ConsultarDatosProductoCSA(IdCotizacionSap, Rpt.IdPosicion, Rpt.IdProductoSap)
                        Exit Select
                    Case TipoProducto.ALQUILER   ' ALQUILER
                        fc_ConsultarProductoAlquiler(Rpt)
                        Exit Select
                End Select
            Next
        End If

        ' --------------------- Consultar Cotizacion --------------------- '
        Dim eCotizacion As New beCotizacion
        eCotizacion = fc_ConsultarCotizacion(IdCotizacionSap)
        If Not eValidacion.validacion Then
            GoTo Terminar
        End If

        eCotizacion.ListabeCotizacionContacto = l_CotizacionContacto
        eCotizacion.ListaProducto = l_Producto
        eCotizacion.ListaSolucionCombinada = l_SolucionCombinada

        'Dim IdRequestCliente As String = Modulo.IdRequestCliente(HttpContext.Current.Request.QueryString.ToString)
        HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, IdRequestCliente)) = eCotizacion
        'Session_Cotizacion = eCotizacion

        HttpContext.Current.Session(String.Concat(Nomb_Respaldo, IdRequestCliente)) = Modulo.CopiarClase(eCotizacion)
        'Session_Respaldo = Modulo.CopiarClase(eCotizacion)

Terminar:
        Return eValidacion

    End Function

    Private Function fc_ConsultarDatosProductoCSA(ByVal IdCotizacionSap As String, ByVal IdPosicion As String, ByVal IdProductoSap As String) As beProductoCSA

        Dim eProductoCSA As New beProductoCSA
        Dim eValidacion As New beValidacion
        eValidacion.validacion = True
        ' --------------- Consultar Lista de Maquinarias    --------------- '
        Dim l_Maquinaria As List(Of beMaquinaria) = fc_ConsultarListaMaquinaria(IdCotizacionSap, IdPosicion, IdProductoSap)
        If Not eValidacion.validacion Then GoTo Terminar

        ' --------------------- Consultar ProductoCSA --------------------- '
        eProductoCSA = fc_ConsultarProductoCSA(IdCotizacionSap, IdPosicion, IdProductoSap)
        If Not eValidacion.validacion Then GoTo Terminar

        eProductoCSA.ListaMaquinaria = l_Maquinaria

Terminar:
        Return eProductoCSA

    End Function

    Private Function fc_ConsultarProductoPrime(ByRef beProducto As beProducto) As Boolean

        Dim blnExito As Boolean = False
        Dim obcProductoPrime As New bcProductoPrime
        Dim ebeProductoPrime As New beProductoPrime
        Dim ebeProductoAdicional As New beProductoAdicional
        Dim bcProductoAdicional As New bcProductoAdicional
        Dim listaAdicional As New List(Of beProductoAdicional)

        Dim obcProductoAccesorio As New bcProductoAccesorio
        Dim obeProductoAccesorio As New beProductoAccesorio
        Dim listaAccesorio As New List(Of beProductoAccesorio)

        ebeProductoAdicional.IdProducto = beProducto.IdProducto
        obeProductoAccesorio.IdProducto = beProducto.IdProducto

        If obcProductoPrime.BuscarId(Modulo.strConexionSql, beProducto) Then
            If bcProductoAdicional.BuscarIdProducto(Modulo.strConexionSql, ebeProductoAdicional, listaAdicional) Then
                beProducto.beProductoPrime.ListabeProductoAdicional = listaAdicional
                'blnExito = True
            End If
            If obcProductoAccesorio.BuscarIdProducto(Modulo.strConexionSql, obeProductoAccesorio, listaAccesorio) Then
                beProducto.beProductoPrime.ListabeProductoAccesorio = listaAccesorio
                'blnExito = True
            End If
            blnExito = True
        End If
        Return blnExito
    End Function

    Private Function fc_ConsultarProductoSolucionCombinada(ByRef beProducto As beProducto) As Boolean
        Dim blnExito As Boolean = False
        Dim obcProductoSolucionCombinada As New bcProductoSolucionCombinada 

        If obcProductoSolucionCombinada.BuscarId(Modulo.strConexionSql, beProducto) Then
            blnExito = True
        End If
        Return blnExito
    End Function
    Private Function fc_ConsultarProductoAlquiler(ByRef beProducto As beProducto) As Boolean

        Dim blnExito As Boolean = False
        Dim obcProductoAlquiler As New bcProductoAlquiler 

        If obcProductoAlquiler.BuscarId(Modulo.strConexionSql, beProducto) Then
            blnExito = True
        End If
        Return blnExito
    End Function
    Private Function fc_ConsultarCotizacion(ByVal IdCotizacionSap As String) As beCotizacion

        Dim eCotizacion as New beCotizacion
        Dim cCotizacion As New bcCotizacion

        eCotizacion.IdCotizacionSap = IdCotizacionSap
        Dim eValidacion As New beValidacion
        cCotizacion.CotizacionListar(strConexionSql, eCotizacion, eValidacion)

        Return eCotizacion

    End Function

    Private Function fc_ConsultarContacto(ByVal IdCotizacionSap As String) As List(Of beCotizacionContacto)

        Dim eValidacion As New beValidacion
        Dim eCotizacion As New beCotizacion
        Dim cCotizacionContacto As New bcCotizacionContacto
        Dim l_CotizacionContacto As New List(Of beCotizacionContacto)

        eCotizacion.IdCotizacionSap = IdCotizacionSap
        cCotizacionContacto.CotizacionContactoListar(strConexionSql, eCotizacion, eValidacion, l_CotizacionContacto)

        Return l_CotizacionContacto

    End Function

    Private Function fc_ConsultarProducto(ByVal IdCotizacionSap As String) As List(Of beProducto)

        Dim eCotizacion As New beCotizacion
        Dim cProducto as New bcProducto
        Dim l_Producto As New List(Of beProducto)

        eCotizacion.IdCotizacionSap = IdCotizacionSap
        Dim eValidacion As New beValidacion
        cProducto.ProductoListar(strConexionSql, eCotizacion, eValidacion, l_Producto)

        Return l_Producto

    End Function

    Private Function fc_ConsultarSolucionCombinada(ByVal IdCotizacionSap As String, ByRef eProducto As List(Of beProducto)) As List(Of beSolucionCombinada)

        Dim eCotizacion As New beCotizacion
        Dim cSolucionCombinada As New bcSolucionCombinada
        Dim l_SolucionCombinada As New List(Of beSolucionCombinada)

        eCotizacion.IdCotizacionSap = IdCotizacionSap
        Dim eValidacion As New beValidacion
        cSolucionCombinada.SolucionCombinadaListar(strConexionSql, eCotizacion, eValidacion, l_SolucionCombinada)

        For Each prod As beProducto In eProducto
            For Each solcomb As beSolucionCombinada In l_SolucionCombinada
                If prod.IdProductoSap = solcomb.IdProducto Then
                    prod.IdTarifaRS = solcomb.IdTarifa
                    Exit For
                End If
            Next
        Next

        Return l_SolucionCombinada

    End Function

    Private Function fc_ConsultarProductoCSA(ByVal IdCotizacionSap As String, ByVal IdPosicion As String, ByVal IdProductoSap As String) As beProductoCSA

        Dim eCotizacion As New beCotizacion
        Dim eProducto As New beProducto
        Dim eProductoCSA As New beProductoCSA
        Dim cProductoCSA As New bcProductoCSA
        Dim eValidacion As New beValidacion

        eCotizacion.IdCotizacionSap = IdCotizacionSap
        eProducto.IdPosicion = IdPosicion
        eProducto.IdProductoSap = IdProductoSap

        cProductoCSA.ProductoCSAListar(strConexionSql, eCotizacion, eProducto, eProductoCSA, eValidacion)

        Return eProductoCSA

    End Function

    Private Function fc_ConsultarListaMaquinaria(ByVal IdCotizacionSap As String, ByVal IdPosicion As String, ByVal IdProductoSap As String) As List(Of beMaquinaria)

        Dim eCotizacion As New beCotizacion
        Dim eProducto As New beProducto
        Dim cMaquinaria As New bcMaquinaria
        Dim eValidacion As New beValidacion
        Dim l_Maquinaria As New List(Of beMaquinaria)

        eCotizacion.IdCotizacionSap = IdCotizacionSap
        eProducto.IdPosicion = IdPosicion
        eProducto.IdProductoSap = IdProductoSap

        cMaquinaria.MaquinariaListar(strConexionSql, eCotizacion, eProducto, eValidacion, l_Maquinaria)

        Return l_Maquinaria

    End Function

    Private Shared Function fc_PresentarCotizacion(ByVal Cotizacion As beCotizacion) As List(Of String)

        Dim lResult As New List(Of String)
        Dim strIdCotizacionSap As String = String.Empty
        Dim strNombreEstado As String = String.Empty
        Dim strDescripSolicitante As String = String.Empty
        Dim strDescripResponsable As String = String.Empty
        Dim strFechaInicioValidez As String = String.Empty
        Dim strFechaFinalValidez As String = String.Empty
        Dim strValorNeto As String = String.Empty
        Dim strFechaPrecio As String = String.Empty
        Dim strValorImpuesto As String = String.Empty
        Dim strValorTipoCambio As String = String.Empty
        Dim strValorBruto As String = String.Empty
        Dim strFechaEstimadaFacturacion As String = String.Empty

        If Not IsNothing(Cotizacion) Then
            strIdCotizacionSap = Cotizacion.IdCotizacionSap
            strNombreEstado = Cotizacion.NombreEstado
            strDescripSolicitante = Cotizacion.DescripSolicitante
            strDescripResponsable = Cotizacion.DescripResponsable
            strFechaInicioValidez = Cotizacion.FechaInicioValidez
            strFechaFinalValidez = Cotizacion.FechaFinalValidez
            strValorNeto = Cotizacion.ValorNeto
            strFechaPrecio = Cotizacion.FechaPrecio
            strValorImpuesto = Cotizacion.ValorImpuesto
            strValorTipoCambio = Cotizacion.ValorTipoCambio
            strValorBruto = Cotizacion.ValorBruto
            strFechaEstimadaFacturacion = Cotizacion.FechaEstimadaFacturacion
            If IsNumeric(strValorNeto) Then
                strValorNeto = String.Format("{0:#,##0.00}", CDbl(strValorNeto))
                strValorNeto &= " " + Cotizacion.MonedaValorNeto
            End If
            If IsNumeric(strValorImpuesto) Then
                strValorImpuesto = String.Format("{0:#,##0.00}", CDbl(strValorImpuesto))
                strValorImpuesto &= " " + Cotizacion.MonedaValorImpuesto
            End If
            If IsNumeric(strValorBruto) Then
                strValorBruto = String.Format("{0:#,##0.00}", CDbl(strValorBruto))
                strValorBruto &= " " + Cotizacion.MonedaValorBruto
            End If

            If IsNumeric(strValorTipoCambio) Then 
                If CDbl(strValorTipoCambio) > 0 Then
                    strValorTipoCambio = String.Format("{0:#,##0.00}", CDbl(strValorTipoCambio))
                Else
                    strValorTipoCambio = "/" & String.Format("{0:#,##0.00}", (-1 * CDbl(strValorTipoCambio)))
                End If

            End If
        End If

        lResult.Add(strIdCotizacionSap)
        lResult.Add(strNombreEstado)
        lResult.Add(strDescripSolicitante)
        lResult.Add(strDescripResponsable)
        lResult.Add(strFechaInicioValidez)
        lResult.Add(strFechaFinalValidez)
        lResult.Add(strValorNeto)
        lResult.Add(strFechaPrecio)
        lResult.Add(strValorImpuesto)
        lResult.Add(strValorTipoCambio)
        lResult.Add(strValorBruto)
        lResult.Add(strFechaEstimadaFacturacion)

        'Validacion para Roles
        '----------------------------------------------------------
        Dim valEdicion As String = "NO"
        Dim valBloqueoVendedor As String = "NO"

        Dim listabeHomologacion As New List(Of beHomologacion)
        Dim obeHomologacion As New beHomologacion
        Dim obcHomologacion As New bcHomologacion
        Dim obeHomologacionBuscar As New beHomologacion

        '--Edicion--
        obeHomologacion.Tabla = TablaHomologacion.ROL_EDICION
        obcHomologacion.BuscarTabla(Modulo.strConexionSql, obeHomologacion, listabeHomologacion)
        If Not listabeHomologacion Is Nothing Then
            If listabeHomologacion.Count > 0 Then
                obeHomologacionBuscar = listabeHomologacion.Where(Function(c) c.ValorSap = Cotizacion.RolUsuario).FirstOrDefault
                If Not obeHomologacionBuscar Is Nothing Then
                    valEdicion = "USUARIO_EDICION"
                End If
            End If 
        End If

        '--Bloqueo--
        For Each objproducto In Cotizacion.ListaProducto
            If objproducto.TipoProducto = TipoProducto.CSA Then
                obeHomologacion.Tabla = TablaHomologacion.ROL_BLOQUEO_VENDEDOR
                listabeHomologacion.Clear()
                obcHomologacion.BuscarTabla(Modulo.strConexionSql, obeHomologacion, listabeHomologacion)
                If Not listabeHomologacion Is Nothing Then
                    If listabeHomologacion.Count > 0 Then
                        obeHomologacionBuscar = Nothing
                        obeHomologacionBuscar = listabeHomologacion.Where(Function(c) c.ValorSap = Cotizacion.RolUsuario).FirstOrDefault
                        If Not obeHomologacionBuscar Is Nothing Then
                            valBloqueoVendedor = "VENDEDOR"
                        End If

                    End If
                End If
                Exit For
            End If
        Next
        
        '---------------------------------------------------------------- 

        '-- validacion para cotizacion Previa ---------------------------------------------
        Dim valMostarCotizacionPrevia As String = "0"
        For Each objproducto In Cotizacion.ListaProducto
            If objproducto.TipoProducto = TipoProducto.ALQUILER Then
                valMostarCotizacionPrevia = "SI"
                Exit For
            End If
        Next
        '----------------------------------------------------------------------------------
        lResult.Add(Cotizacion.RolUsuario)
        lResult.Add(valEdicion)
        lResult.Add(valBloqueoVendedor)
        lResult.Add(valMostarCotizacionPrevia)
        Return lResult

    End Function

    Private Shared Function fc_CalcularMontoPrime(ByVal ProductoCSA As beProductoCSA, ByVal Maquina As beMaquinaria) As Double

        Dim lResult As Double = 0
        Dim l_docDetPlan As List(Of beTarifa) = Nothing
        Dim SubTotal As Double = 0
        'Dim TotalFinal As Double = 0
        Dim ValorEvento As Double = 0

        Dim obeHomologacion As New beHomologacion
        Dim listaHomologacion As New List(Of beHomologacion)
        Dim obcHomologacion As New bcHomologacion


        'Dim Local_Llave As String = String.Concat(IIf(String.IsNullOrEmpty(Maquina.modelo), Maquina.modeloBase, Maquina.modelo), "@", ProductoCSA.IdPlan, "@", "PM 1")

        '---------------------------------------------
        Dim valIncluyeFluido As String = String.Empty
        If ProductoCSA.IncluyeFluidos Then
            'valIncluyeFluido = "1"
            valIncluyeFluido = "Con Fluidos"
        Else
            'valIncluyeFluido = "0"
            valIncluyeFluido = "Sin Fluidos"
        End If
 
        '---------------------------------------------
        Dim Local_Llave As String = String.Concat(IIf(String.IsNullOrEmpty(Maquina.modelo), Maquina.modeloBase, Maquina.modelo), "@", ProductoCSA.IdPlan, "@", "PM 1")

        l_docDetPlan = New List(Of beTarifa)
        l_docDetPlan = fc_ModeloCodPlanEvento(Local_Llave)

        l_docDetPlan = l_docDetPlan.Where(Function(a) a.conFluidos = valIncluyeFluido).ToList()
        If Not l_docDetPlan.Any Then
            lResult = 0
            Throw New Exception(String.Concat("No se encontró tarifa Prime para la máquina ", Local_Llave))
            GoTo Terminar
        Else

            Dim duracionPrime As Decimal = 0

            obeHomologacion.Tabla = TablaHomologacion.DURACION_PLAN_GENERADOR_PRIME  ' tabla
            listaHomologacion.Clear()
            listaHomologacion = f_ListaTablaHomologacion(obeHomologacion)
            obeHomologacion = listaHomologacion.ToList().FirstOrDefault
            If Not obeHomologacion Is Nothing Then
                duracionPrime = obeHomologacion.ValorSap
            End If

            'SubTotal = CDbl(l_docDetPlan.First.tarifaUSDxH) * ProductoCSA.Duracion
            SubTotal = CDbl(l_docDetPlan.First.tarifaUSDxH) * duracionPrime 
        End If

        lResult = SubTotal

Terminar:
        Return lResult

    End Function

    Private Shared Function f_ListaTablaHomologacion(ByVal obeHomologacion As beHomologacion) As List(Of beHomologacion)
        Dim listHomologacion As New List(Of beHomologacion)
        Dim obcHomologacion As New bcHomologacion
        obcHomologacion.BuscarTabla(Modulo.strConexionSql, obeHomologacion, listHomologacion)
        Return listHomologacion
    End Function

    Private Shared Function fc_CalcularMontoStandBy(ByVal ProductoCSA As beProductoCSA, ByVal Maquina As beMaquinaria) As Double

        Dim lResult As Double = 0
        Dim l_docDetPlan As List(Of beTarifa) = Nothing
        Dim SubTotal As Double = 0
        Dim TotalFinal As Double = 0
        Dim ValorEvento As Double = 0
        Dim lEvento As New List(Of String)

        lEvento.Add("INSPECCIÓN")
        lEvento.Add("PRUEBA")
        lEvento.Add("MANTENIMIENTO")

        For i As Short = 0 To 2
            SubTotal = 0
            Dim Local_Llave As String = String.Concat(IIf(String.IsNullOrEmpty(Maquina.modelo), Maquina.modeloBase, Maquina.modelo), "@", ProductoCSA.IdPlan, "@", lEvento(i))
            l_docDetPlan = New List(Of beTarifa)
            l_docDetPlan = fc_ModeloCodPlanEvento(Local_Llave)
            If Not l_docDetPlan.Any Then
                lResult = 0
                Throw New Exception(String.Concat("No se encontró tarifa Stand By para la máquina ", Local_Llave))
                GoTo Terminar
            Else
                SubTotal = SubTotal + CDbl(l_docDetPlan.First.servicioContratado) * CDbl(l_docDetPlan.First.eventosNueva)
                TotalFinal = TotalFinal + SubTotal
            End If
        Next

        lResult = TotalFinal

Terminar:
        Return lResult

    End Function

    Private Shared Function fc_ModeloCodPlanEvento(ByRef Llave As String) As List(Of beTarifa)

        Dim cTarifa as New bcTarifa
        Dim eTarifa As New beTarifa
        Dim l_Tarifa As New List(Of beTarifa)
        Dim eValidacion as New beValidacion

        eTarifa.llave = Llave
        cTarifa.ListarModeloCodPlanEvento(strConexionSql, eTarifa, eValidacion, l_Tarifa)

        Return l_Tarifa

    End Function

    Private Shared Function fc_ListarModelo(ByVal ModeloBase As String, ByVal Prefijo As String) As String

        Dim cParametros As New bcParametros
        Dim eMaquinaria1 As New beMaquinaria
        Dim eValidacion As New beValidacion
        Dim l_Maquinaria As New List(Of beMaquinaria)

        eMaquinaria1.flag = "5"
        eMaquinaria1.modeloBase = ModeloBase
        eMaquinaria1.prefijo = Prefijo

        cParametros.ListarDetalleCsa(strConexionSql, eMaquinaria1, eValidacion, l_Maquinaria)

        If l_Maquinaria.Any Then
            eMaquinaria1 = l_Maquinaria.First
        End If

        Return eMaquinaria1.codigo

    End Function

    Private Shared Function fc_BuscarAnteriorxNroSerie(ByVal Maquinaria As beMaquinaria, ByVal ProductoCSA As beProductoCSA) As beMaquinaria
        Dim lResult As beMaquinaria = Nothing

        For i As Integer = CInt(Maquinaria.item) - 2 To 0 Step -1
            If Maquinaria.numeroSerie = ProductoCSA.ListaMaquinaria(i).numeroSerie Then
                lResult = ProductoCSA.ListaMaquinaria(i)
                Exit For
            End If
        Next

Terminar:
        Return lResult
    End Function

    Private Shared Function fc_ModBaseCodPlanPrefPM(ByRef Llave As String) As List(Of beTarifa)

        Dim cTarifa As New bcTarifa
        Dim eTarifa As New beTarifa
        Dim l_Tarifa As New List(Of beTarifa)
        Dim eValidacion As New beValidacion

        eValidacion.flag = 0
        eTarifa.llave = Llave
        cTarifa.ListarModBaseCodPlanPrefPM(strConexionSql, eTarifa, eValidacion, l_Tarifa)

        Return l_Tarifa

    End Function

    Private Shared Function fc_CalcularMontoMT(ByVal ProductoCSA As beProductoCSA, ByVal Maquina As beMaquinaria) As Double

        Dim lResult As Double = 0
        Dim l_docDetPlan As List(Of beTarifa) = Nothing
        Dim SubTotal As Double = 0
        Dim TotalFinal As Double = 0
        Dim ValorEvento As Double = 0

        For i As Integer = 1 To 4

            Dim prefijo As String = ""
            If Maquina.prefijo = "0" Then
                prefijo = Maquina.prefijoOt
            Else
                prefijo = Maquina.prefijo
            End If

            Dim Local_Llave As String

            'Controlar si es Monitoreo
            If ProductoCSA.TipoCotizacion <> TipoCotizacionCSA.MONITOREO Then
                Local_Llave = String.Concat(Maquina.modeloBase, "@", prefijo, "@", ProductoCSA.IdPlan, "@", "PM ", CStr(i))
            Else
                ''Linea para q todos los planes de monitoreos se calculen sin fluidos
                'ProductoCSA.IncluyeFluidos = False
                If i <> 3 Then
                    Local_Llave = String.Concat(Maquina.modeloBase, "@", prefijo, "@", ProductoCSA.IdPlan, "@", "Inspección ", CStr(i))
                Else
                    Local_Llave = String.Concat(Maquina.modeloBase, "@", prefijo, "@", ProductoCSA.IdPlan, "@", "Monitoreo")
                End If

            End If

            l_docDetPlan = New List(Of beTarifa)
            l_docDetPlan = fc_ModBaseCodPlanPrefPM(Local_Llave)
            If Not l_docDetPlan.Any Then
                lResult = 0
                Throw New Exception(String.Concat("No se encontró tarifa para la máquina ", Local_Llave))
                GoTo Terminar
            Else
                SubTotal = l_docDetPlan.First.kitRepuestos

                '-----------------------------------------------
                'verificar si producto incluye fluidos exita tarifas con fluidos
                If CBool(ProductoCSA.IncluyeFluidos) Then
                    l_docDetPlan = l_docDetPlan.Where(Function(c) c.conFluidos.ToString().ToUpper() = "CON FLUIDOS").ToList()
                Else
                    l_docDetPlan = l_docDetPlan.Where(Function(c) c.conFluidos.ToString().ToUpper() = "SIN FLUIDOS").ToList()
                End If
                If l_docDetPlan.Count < 1 Then
                    lResult = 0
                    Throw New Exception(String.Concat("No se encontró tarifa para la máquina: ", Local_Llave))
                    GoTo Terminar
                End If
                '------------------------------------------------

                If CBool(ProductoCSA.IncluyeFluidos) Then
                    SubTotal = SubTotal + l_docDetPlan.First.fluidos
                End If
                If CBool(Maquina.maquinaNueva) Then
                    If IsNumeric(l_docDetPlan.First.eventosNueva) Then
                        ValorEvento = l_docDetPlan.First.eventosNueva
                    End If
                Else
                    If IsNumeric(l_docDetPlan.First.eventosUsada) Then
                        ValorEvento = l_docDetPlan.First.eventosUsada
                    End If
                End If
                SubTotal = (SubTotal + l_docDetPlan.First.servicioContratado) * ValorEvento
                TotalFinal = TotalFinal + SubTotal
                If CBool(Maquina.maquinaNueva) And i <= 2 Then
                    Local_Llave = String.Concat(Maquina.modeloBase, "@", Maquina.prefijo, "@", ProductoCSA.IdPlan, "@", "PM ", CStr(i), "F")
                    l_docDetPlan = New List(Of beTarifa)
                    l_docDetPlan = fc_ModBaseCodPlanPrefPM(Local_Llave)
                    If l_docDetPlan.Any Then
                        SubTotal = l_docDetPlan.First.kitRepuestos
                        If CBool(ProductoCSA.IncluyeFluidos) Then
                            SubTotal = SubTotal + l_docDetPlan.First.fluidos
                        End If
                        SubTotal = (SubTotal + l_docDetPlan.First.servicioContratado) * l_docDetPlan.First.eventosNueva
                        TotalFinal = TotalFinal + SubTotal
                    End If

                End If
            End If
            'para plan underground Plus
            '=======================================================
            If i = 1 Then
                Local_Llave = String.Concat(Maquina.modeloBase, "@", prefijo, "@", ProductoCSA.IdPlan, "@", "PM  125")
                l_docDetPlan.Clear()
                l_docDetPlan = fc_ModBaseCodPlanPrefPM(Local_Llave)
                If Not l_docDetPlan.Any Then
                    'lResult = 0
                    'Throw New Exception(String.Concat("No se encontró tarifa PM para la máquina ", Local_Llave))
                    'GoTo Terminar
                Else
                    SubTotal = l_docDetPlan.First.kitRepuestos
                    If CBool(ProductoCSA.IncluyeFluidos) Then
                        SubTotal = SubTotal + l_docDetPlan.First.fluidos
                    End If
                    If CBool(Maquina.maquinaNueva) Then
                        If IsNumeric(l_docDetPlan.First.eventosNueva) Then
                            ValorEvento = l_docDetPlan.First.eventosNueva
                        End If
                    Else
                        If IsNumeric(l_docDetPlan.First.eventosUsada) Then
                            ValorEvento = l_docDetPlan.First.eventosUsada
                        End If
                    End If
                    SubTotal = (SubTotal + l_docDetPlan.First.servicioContratado) * ValorEvento
                    TotalFinal = TotalFinal + SubTotal
                End If
            End If

            '=======================================================
            'Controlar si es Monitoreo
            If ProductoCSA.TipoCotizacion = TipoCotizacionCSA.MONITOREO Then
                If i = 3 Then
                    Exit For
                End If
            End If
        Next
        lResult = TotalFinal

Terminar:
        Return lResult
    End Function
    Private Shared Function fc_ValidarCalculoMontoProducto(ByVal Producto As beProducto) As beValidacion
        Dim eValidacion As New beValidacion
        Try
            For Each Rpt As beMaquinaria In Producto.ProductoCSA.ListaMaquinaria
                'If Producto.ProductoCSA.ClaseCsa <> "P" Then
                If Producto.ProductoCSA.ClaseCsa <> Homologacion.ClaseCSA.Planes Then
                    Rpt.montoItem = "0"
                Else
                    Select Case Producto.ProductoCSA.TipoCotizacion
                        Case "P"
                            Rpt.montoItem = fc_CalcularMontoPrime(Producto.ProductoCSA, Rpt)
                        Case "S"
                            Rpt.montoItem = fc_CalcularMontoStandBy(Producto.ProductoCSA, Rpt)
                        Case Else
                            Rpt.montoItem = fc_CalcularMontoMT(Producto.ProductoCSA, Rpt)
                    End Select
                End If
            Next
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try
        Return eValidacion
    End Function

    Private Shared Function fc_CalculoProductoCsa(ByRef Producto As beProducto) As beValidacion

        Dim eValidacion As New beValidacion

        Try
            For Each Rpt As beMaquinaria In Producto.ProductoCSA.ListaMaquinaria
                If Producto.ProductoCSA.ClaseCsa <> "P" Then
                    Rpt.montoItem = "0"
                Else
                    Select Case Producto.ProductoCSA.TipoCotizacion
                        Case "P"
                            Rpt.montoItem = fc_CalcularMontoPrime(Producto.ProductoCSA, Rpt)
                        Case "S"
                            Rpt.montoItem = fc_CalcularMontoStandBy(Producto.ProductoCSA, Rpt)
                        Case Else
                            Rpt.montoItem = fc_CalcularMontoMT(Producto.ProductoCSA, Rpt)
                    End Select
                End If
            Next

            If Producto.ProductoCSA.ListaMaquinaria.Count > 0 Then
                Producto.ValorUnitario = Producto.ProductoCSA.ListaMaquinaria.Sum(Function(Rpt) CDbl(Rpt.montoItem))
            Else
                Producto.ValorUnitario = 0
            End If


            If Not IsNumeric(Producto.Cantidad) Then Producto.Cantidad = 1

            Producto.ValorNeto = CDbl(Producto.ValorUnitario) * CInt(Producto.Cantidad)

            Producto.ValorUnitario = Math.Round(CDbl(Producto.ValorUnitario), 2)

            Producto.ValorNeto = Math.Round(CDbl(Producto.ValorNeto), 2)

            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

        Return eValidacion

    End Function

    Private Shared Function fc_GetStreamAsByteArray(ByVal stream As System.IO.Stream) As Byte()
        Dim streamLength As Integer = Convert.ToInt32(stream.Length)
        Dim fileData As Byte() = New Byte(streamLength) {}
        ' Read the file into a byte array
        stream.Read(fileData, 0, streamLength)
        stream.Close()
        Return fileData
    End Function

    Private Shared Function fc_ConsultarSeccionesCotizacion() As List(Of beTablaMaestra)

        Dim eValidacion As New beValidacion
        Dim cTablaMaestra As New bcTablaMaestra
        Dim l_TablaMaestra As New List(Of beTablaMaestra)

        Dim IdRequestCliente As String = Modulo.IdRequestCliente(HttpContext.Current.Request.UrlReferrer.Query.Trim)
        eValidacion.usuario = HttpContext.Current.Session(String.Concat(Nomb_Usuario, IdRequestCliente)) 'Session_Usuario
        cTablaMaestra.ListarSeccionesxUsuario(strConexionSql, eValidacion, l_TablaMaestra)

        Return l_TablaMaestra
    End Function

    Private Shared Function fc_CalculoProductoSolCombinada(ByRef eProducto As beProducto, ByVal total As String) As beValidacion

        Dim Producto As New beProducto
        Producto = Modulo.CopiarClase(eProducto)

        Dim eValidacion As New beValidacion

        Try

            If Not IsNumeric(Producto.Cantidad) Then Producto.Cantidad = 1
            If Not IsNumeric(Producto.ValorLista) Then Producto.ValorLista = 0

            Producto.ValorUnitario = Math.Round(CDbl(total), 2)
            Producto.ValorNeto = Math.Round(CDbl(Producto.ValorUnitario) * CInt(Producto.Cantidad), 2)
            Producto.ValorReal = Math.Round(CDbl(Producto.ValorUnitario) * CDbl(Producto.ValorLista), 2)
            Producto.ValorVenta = Math.Round(CDbl(Producto.ValorNeto), 2)
            Producto.ValorImpuesto = Math.Round(CDbl(Producto.ValorVenta) * (0.18), 2)
            'Producto.PorcDescuento = Producto.ValorImpuesto
            Producto.PrecioVentaFinal = Math.Round(CDbl(Producto.ValorImpuesto) + CDbl(Producto.ValorVenta), 2)

            eProducto = Producto

            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

        Return eValidacion

    End Function

#End Region

#Region "----------   Metodos   ----------"

    Private Shared Sub CargarDatosxMaquinaria(ByRef Maquinaria As beMaquinaria, _
                                        ByVal item As String, ByVal maquinariaNueva As String, _
                                        ByVal familia As String, ByVal familiaOt As String, _
                                        ByVal modeloBase As String, ByVal modeloBaseOt As String, _
                                        ByVal prefijo As String, ByVal prefijoOt As String, _
                                        ByVal numeroMaquinas As String, ByVal numeroSerie As String, _
                                        ByVal numeroSerieOt As String, ByVal horometroInicial As String, _
                                        ByVal fechaHorometro As String, ByVal horasPromedioMensual As String, _
                                        ByVal codDepartamento As String, ByVal departamento As String, _
                                        ByVal ClaseCsa As String)
        Maquinaria.item = item
        Maquinaria.maquinaNueva = maquinariaNueva
        Maquinaria.familia = familia
        Maquinaria.familiaOt = familiaOt
        Maquinaria.modeloBase = modeloBase
        Maquinaria.modeloBaseOt = modeloBaseOt
        Maquinaria.prefijo = prefijo
        Maquinaria.prefijoOt = prefijoOt
        Maquinaria.numeroMaquinas = numeroMaquinas
        Maquinaria.numeroSerie = numeroSerie
        Maquinaria.numeroSerieOt = numeroSerieOt
        Maquinaria.horometroInicial = horometroInicial
        Maquinaria.fechaHorometro = fechaHorometro
        Maquinaria.horasPromedioMensual = horasPromedioMensual
        Maquinaria.codDepartamento = codDepartamento
        Maquinaria.departamento = departamento
        Maquinaria.modelo = fc_ListarModelo(modeloBase, prefijo)
        If String.IsNullOrEmpty(Maquinaria.modelo) Then
            If ClaseCsa = Homologacion.ClaseCSA.Acuerdos Then
                If modeloBase <> "0" Then
                    Maquinaria.modelo = modeloBase
                Else
                    Maquinaria.modelo = modeloBaseOt
                End If
            Else
                Maquinaria.modelo = modeloBase
            End If
        End If


    End Sub

    Private Shared Sub RecalcularMaquinaria(ByVal Cotizacion As beCotizacion, ByVal ProductoCSA As beProductoCSA, ByRef Rpt As beMaquinaria)


        If ProductoCSA.ClaseCsa <> "P" Then
            'Si el tipo de CSA del presente item no es un plan
            Rpt.horometroFinal = String.Empty

            'si no se ha ingresado numero de serie de la maquina
            If String.IsNullOrEmpty(Rpt.numeroSerie) Then
                Rpt.renovacion = False
                Rpt.renovacionValida = False
            Else
                Dim docAnterior As beMaquinaria = Nothing
                'If CInt(Rpt.item) > 1 Then fc_BuscarAnteriorxNroSerie(Rpt, ProductoCSA)
                If docAnterior IsNot Nothing Then
                    Rpt.renovacion = True
                Else
                    Rpt.renovacion = False
                End If
                Rpt.renovacionValida = False
            End If
            If Not IsNumeric(Rpt.horometroInicial) Then
                Rpt.horometroInicial = 0

            End If

            Rpt.horometroFinal = CDbl(Rpt.horometroInicial) + IIf(IsNumeric(ProductoCSA.Duracion), CDbl(ProductoCSA.Duracion), 0)
            Rpt.montoItem = "0"
        Else
            'Si el tipo de CSA del presente item es un plan
            If Not IsNumeric(Rpt.horometroInicial) Then
                Rpt.horometroInicial = "0"
                Rpt.horometroFinal = "0"
            Else
                Rpt.horometroInicial = Rpt.horometroInicial
                Rpt.horometroFinal = CDbl(Rpt.horometroInicial) + IIf(IsNumeric(ProductoCSA.Duracion), CDbl(ProductoCSA.Duracion), 0)
            End If
            If String.IsNullOrEmpty(Rpt.numeroSerie) Then
                'si no se ha ingresado nro serie de la maquina
                Rpt.renovacion = False
                Rpt.renovacionValida = False
            Else
                'si se ha ingresado nro serie de la maquina se busca su registro anterior
                Dim docAnterior As beMaquinaria = Nothing
                'If CInt(Rpt.item) > 1 Then fc_BuscarAnteriorxNroSerie(Rpt, ProductoCSA)
                If docAnterior IsNot Nothing Then
                    'si la maquina ha sido ingresada anteriormente y es un plan
                    Rpt.renovacion = True
                    'se verifica si la renovacion es valida
                    If IsNumeric(docAnterior.horometroFinal) Then
                        Rpt.horometroFinal = docAnterior.horometroFinal
                    Else
                        Rpt.horometroFinal = "0"
                    End If
                    Rpt.renovacionValida = True
                Else
                    'si no hay maquina ingresada anteriormente
                    Rpt.renovacion = False
                    Rpt.renovacionValida = False
                End If
            End If
            'Se lee el precio de la maquina a agregar al plan
            'If Cotizacion.TipoCotizacion = "P" Then 'si es un plan de energia prime
            'Rpt.montoItem = fc_CalcularMontoPrime(ProductoCSA, Rpt)
            'ElseIf Cotizacion.TipoCotizacion = "S" Then 'si es un plan de stand by
            '    Rpt.montoItem = fc_CalcularMontoStandBy(ProductoCSA, Rpt)
            'Else  'si es cualquier otro plan
            '    Rpt.montoItem = fc_CalcularMT(ProductoCSA, Rpt)
            'End If
        End If

    End Sub

    Private Shared Sub RecorrerMaquinaria(ByVal Rpt As beMaquinaria)

        If String.IsNullOrEmpty(Rpt.maquinaNueva) Then
            Rpt.condicionMaquinaria = String.Empty
        ElseIf CBool(Rpt.maquinaNueva) Then
            Rpt.condicionMaquinaria = "N"
        Else
            Rpt.condicionMaquinaria = "U"
        End If

        If String.IsNullOrEmpty(Rpt.renovacion) Then
            Rpt.descripRenovacion = String.Empty
        ElseIf CBool(Rpt.renovacion) Then
            Rpt.descripRenovacion = "SI"
        Else
            Rpt.descripRenovacion = "NO"
        End If

        If String.IsNullOrEmpty(Rpt.renovacionValida) Then
            Rpt.descripRenovacionValida = String.Empty
        ElseIf CBool(Rpt.renovacionValida) Then
            Rpt.descripRenovacionValida = "SI"
        Else
            Rpt.descripRenovacionValida = "NO"
        End If

    End Sub

    Private Sub Inicializar()

        Dim IdCotizacionSap As String = Request.QueryString("IdCotizacionSap")

        If String.IsNullOrEmpty(IdCotizacionSap) Then Return

        Dim IdRequestCliente As String = Modulo.IdRequestCliente(HttpContext.Current.Request.QueryString.ToString)
        HttpContext.Current.Session(String.Concat(Nomb_Usuario, IdRequestCliente)) = Request.QueryString("Usuario")
        Me.lblIdSession.Text = IdRequestCliente.Trim
        'Session_Usuario = Request.QueryString("Usuario")

        Dim eValidacion As New beValidacion
        eValidacion = fc_ConsultarDatosCotizacion(IdCotizacionSap, IdRequestCliente)

        If Not eValidacion.validacion Then
            ScriptManager.RegisterStartupScript(Page, Page.GetType, "alert", "alert('" + eValidacion.mensaje + "')", True)
        End If

    End Sub

    Private Shared Sub AgregarVacio(ByRef oListado As List(Of beMaquinaria))
        Dim Rpt As New beMaquinaria
        Rpt.descripcion = String.Empty
        Rpt.codigo = String.Empty
        oListado.Add(Rpt)
    End Sub

    Private Shared Sub AgregarOtros(ByRef oListado As List(Of beMaquinaria))
        Dim Rpt As New beMaquinaria
        Rpt.descripcion = oOtros.descripcion
        Rpt.codigo = oOtros.codigo
        oListado.Add(Rpt)
    End Sub

    Private Shared Sub ForEach_Maquinaria(ByVal Rpt As beMaquinaria)

        ItemFila += 1
        Rpt.item = ItemFila

    End Sub

    Private Shared Sub ActualizarCalculosCotizacion(ByRef Cotizacion As beCotizacion)

        Dim l_Producto As New List(Of beProducto)
        l_Producto = Cotizacion.ListaProducto

        Dim dValorNeto, dValorImpuesto, dValorBruto As Double

        dValorNeto = l_Producto.Sum(Function(Rpt) CDbl(Rpt.ValorNeto))
        dValorNeto = Math.Round(dValorNeto, 2)

        'dValorImpuesto = dValorNeto * 0.18

        'Calcular el Valor impuesto para todo los productos CSA
        For Each eProducto As beProducto In l_Producto
            If eProducto.TipoProducto = TipoProducto.CSA Then
                Try
                    'eProducto.ValorImpuesto = eProducto.PorcDescuento  * eProducto.ValorNeto
                    eProducto.ValorImpuesto = (0.18) * eProducto.ValorNeto
                Catch ex As Exception
                End Try

            End If
        Next

        dValorImpuesto = l_Producto.Sum(Function(Rpt) CDbl(Rpt.ValorImpuesto))
        dValorImpuesto = Math.Round(dValorImpuesto, 2)

        dValorBruto = dValorNeto + dValorImpuesto

        Cotizacion.ValorNeto = dValorNeto
        Cotizacion.ValorImpuesto = dValorImpuesto
        Cotizacion.ValorBruto = dValorBruto

        Cotizacion.ListaProducto = l_Producto

    End Sub

#End Region

#Region "----------   Eventos   ----------"
 
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If sender Is Nothing OrElse e Is Nothing Then
            Return
        End If

        If Not IsPostBack Then
            Call Me.Inicializar()
        End If
         
        'Dim auth = If(Request.Cookies(FormsAuthentication.FormsCookieName) Is Nothing, String.Empty, Request.Cookies(FormsAuthentication.FormsCookieName).Value) 'Request.Cookies(FormsAuthentication.FormsCookieName) is Nothing ? string.Empty : Request.Cookies(FormsAuthentication.FormsCookieName).Value  
        'Dim a As String = WindowsIdentity.GetCurrent.Name

    End Sub

#End Region

#Region "----------  WebMethod  ----------"

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarCotizacion(ByVal session As String) As List(Of String)

        Dim eCotizacion As New beCotizacion
        Dim eValidacion As New beValidacion
         
        eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion) 'CType(Session_Cotizacion, beCotizacion)

        Return fc_PresentarCotizacion(eCotizacion)

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarContacto(ByVal sortColumn As String, ByVal sortOrder As String, ByVal session As String) As JQGridJsonResponse

        Dim eCotizacion As New beCotizacion
        Dim eValidacion As New beValidacion

        Dim l_CotizacionContacto As New List(Of beCotizacionContacto)

        eValidacion.sortColumn = sortColumn
        eValidacion.sortOrder = sortOrder

        'Dim IdRequestCliente As String = Modulo.IdRequestCliente(HttpContext.Current.Request.UrlReferrer.Query.Trim)
        eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion) 'CType(Session_Cotizacion, beCotizacion)
        l_CotizacionContacto = eCotizacion.ListabeCotizacionContacto

        Call ArmarPaginacion(eValidacion, l_CotizacionContacto)

        Dim lResponse As New JQGridJsonResponse(1, 1, 10, l_CotizacionContacto)

        Return lResponse

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarProducto(ByVal sortColumn As String, ByVal sortOrder As String, _
                                             ByVal pageSize As String, ByVal currentPage As String, ByVal session As String) As JQGridJsonResponse

        Dim eCotizacion As New beCotizacion
        Dim l_Producto As New List(Of beProducto)
        Dim eValidacion as New beValidacion

        eValidacion.sortColumn = sortColumn
        eValidacion.sortOrder = sortOrder
        eValidacion.pageSize = pageSize
        eValidacion.currentPage = currentPage

        'Dim IdRequestCliente As String = Modulo.IdRequestCliente(HttpContext.Current.Request.UrlReferrer.Query.Trim)
        eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion) 'CType(Session_Cotizacion, beCotizacion)
        l_Producto = eCotizacion.ListaProducto

        Call ArmarPaginacion(eValidacion, l_Producto)

        Dim lResponse As New JQGridJsonResponse(eValidacion.pageCount, eValidacion.currentPage, eValidacion.recordCount, l_Producto)

        Return lResponse

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarProductoSeleccionado(ByVal IdPosicion As String, ByVal IdProductoSap As String, ByVal session As String) As beProducto

        Dim eProducto As New beProducto
        Dim eCotizacion As New beCotizacion
        Dim l_Producto as New List(Of beProducto)

        'Dim IdRequestCliente As String = Modulo.IdRequestCliente(HttpContext.Current.Request.UrlReferrer.Query.Trim)
        eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion) 'CType(Session_Cotizacion, beCotizacion)
        l_Producto = eCotizacion.ListaProducto

        For Each Producto As beProducto In l_Producto
            If Producto.IdPosicion = IdPosicion AndAlso Producto.IdProductoSap = IdProductoSap Then
                eProducto = Producto : Exit For
            End If
        Next

        eProducto.ValorLista = Modulo.FormatoMoneda(CDbl(eProducto.ValorLista))
        eProducto.ValorReal = Modulo.FormatoMoneda(CDbl(eProducto.ValorReal))
        eProducto.PorcDescuento = Modulo.FormatoMoneda(CDbl(eProducto.PorcDescuento))
        eProducto.DescuentoImp = Modulo.FormatoMoneda(CDbl(eProducto.DescuentoImp))
        eProducto.Flete = Modulo.FormatoMoneda(CDbl(eProducto.Flete))
        eProducto.ValorVenta = Modulo.FormatoMoneda(CDbl(eProducto.ValorVenta))
        eProducto.PorcImpuesto = Modulo.FormatoMoneda(CDbl(eProducto.PorcImpuesto))
        eProducto.PrecioVentaFinal = Modulo.FormatoMoneda(CDbl(eProducto.PrecioVentaFinal))


        HttpContext.Current.Session(String.Concat(Nomb_Producto, session)) = eProducto
        'Session_Producto = eProducto

        Return eProducto

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarMaquinaria(ByVal sortColumn As String, ByVal sortOrder As String, _
                                               ByVal pageSize As String, ByVal currentPage As String, _
                                               ByVal session As String) As JQGridJsonResponse

        Dim eProducto As New beProducto
        Dim l_Maquinaria As New List(Of beMaquinaria)
        Dim eValidacion As New beValidacion

        eValidacion.sortColumn = sortColumn
        eValidacion.sortOrder = sortOrder
        eValidacion.pageSize = pageSize
        eValidacion.currentPage = currentPage

        'Dim IdRequestCliente As String = Modulo.IdRequestCliente(HttpContext.Current.Request.UrlReferrer.Query.Trim)
        eProducto = CType(HttpContext.Current.Session(String.Concat(Nomb_Producto, session)), beProducto) 'CType(Session_Producto, beProducto)

        If IsNothing(eProducto) Then GoTo Terminar

        l_Maquinaria = eProducto.ProductoCSA.ListaMaquinaria
        l_Maquinaria.ForEach(AddressOf RecorrerMaquinaria)

        '--- actualizar Producto de sesion de respaldo -------
        'Para poder saber si hubo algun cambio en las maquinarias
        Dim eCotizacionRespaldo As beCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Respaldo, session)), beCotizacion)
        If Not IsNothing(eCotizacionRespaldo) Then
            Dim posicionProducto As Integer = -1
            Dim contador As Integer = 0
            For Each oProducto In eCotizacionRespaldo.ListaProducto
                If oProducto.IdProducto = eProducto.IdProducto Then
                    posicionProducto = contador
                    Exit For
                End If
                contador = contador + 1
            Next

            If posicionProducto <> -1 Then
                If eCotizacionRespaldo.ListaProducto(posicionProducto).FlatModificadoMemoria = False Then
                    eProducto.FlatModificadoMemoria = True
                    eCotizacionRespaldo.ListaProducto(posicionProducto) = eProducto
                    HttpContext.Current.Session(String.Concat(Nomb_Respaldo, session)) = eCotizacionRespaldo
                End If

            End If
        End If
        '-----------------------------------------

Terminar:
        Call ArmarPaginacion(eValidacion, l_Maquinaria)
        Dim lResponse As New JQGridJsonResponse(eValidacion.pageCount, eValidacion.currentPage, eValidacion.recordCount, l_Maquinaria)
        Return lResponse

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarFamilia(ByVal session As String) As List(Of beMaquinaria)

        Dim cParametros As New bcParametros
        Dim eMaquinaria As New beMaquinaria
        Dim eValidacion as New beValidacion
        Dim l_Maquinaria As New List(Of beMaquinaria)
        Dim eProducto As New beProducto

         eProducto = CType(HttpContext.Current.Session(String.Concat(Nomb_Producto, session)), beProducto) 'CType(Session_Producto, beProducto)

        eMaquinaria.flag = "1"
        If eProducto.ProductoCSA.ClaseCsa = Homologacion.ClaseCSA.Acuerdos Then
            eMaquinaria.plan = "ACUERDO"

        Else
            eMaquinaria.plan = eProducto.ProductoCSA.IdPlan
        End If


        AgregarVacio(l_Maquinaria)

        Dim eProductoCSA As New beProductoCSA
        eProductoCSA = eProducto.ProductoCSA

        If Not (eProductoCSA.IdPlan.Equals("")) Then
            cParametros.ListarDetalleCsa(strConexionSql, eMaquinaria, eValidacion, l_Maquinaria)
        End If
        AgregarOtros(l_Maquinaria)

        Return l_Maquinaria

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarModeloBase(ByVal familia As String, ByVal session As String) As List(Of beMaquinaria)

        Dim cParametros As New bcParametros
        Dim eMaquinaria As New beMaquinaria
        Dim eValidacion As New beValidacion
        Dim l_Maquinaria = New List(Of beMaquinaria)
        Dim eProducto As New beProducto

        'Dim IdRequestCliente As String = Modulo.IdRequestCliente(HttpContext.Current.Request.UrlReferrer.Query.Trim)

        eProducto = CType(HttpContext.Current.Session(String.Concat(Nomb_Producto, session)), beProducto) 'CType(Session_Producto, beProducto)

        eMaquinaria.flag = "2"

        If eProducto.ProductoCSA.ClaseCsa = Homologacion.ClaseCSA.Acuerdos Then
            eMaquinaria.plan = "ACUERDO"

        Else
            eMaquinaria.plan = eProducto.ProductoCSA.IdPlan
        End If

        'eMaquinaria.plan = eProducto.ProductoCSA.IdPlan
        eMaquinaria.familia = familia
        Dim eProductoCSA As New beProductoCSA
        eProductoCSA = eProducto.ProductoCSA

        AgregarVacio(l_Maquinaria)
        If Not (eProductoCSA.IdPlan.Equals("") OrElse familia.Equals("0")) Then
            cParametros.ListarDetalleCsa(strConexionSql, eMaquinaria, eValidacion, l_Maquinaria)
        End If
        AgregarOtros(l_Maquinaria)

        Return l_Maquinaria

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarDepartamento() As List(Of beMaquinaria)

        Dim cParametros As New bcParametros
        Dim eMaquinaria As New beMaquinaria
        Dim eValidacion As New beValidacion
        Dim l_Maquinaria As New List(Of beMaquinaria)

        eMaquinaria.flag = "4"
        cParametros.ListarDetalleCsa(strConexionSql, eMaquinaria, eValidacion, l_Maquinaria)

        Return l_Maquinaria

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarPrefijo(ByVal familia As String, ByVal modeloBase As String) As List(Of beMaquinaria)

        Dim cParametros as New bcParametros
        Dim eMaquinaria As New beMaquinaria
        Dim eValidacion As New beValidacion
        Dim l_Maquinaria = New List(Of beMaquinaria)

        eMaquinaria.flag = "3"
        eMaquinaria.familia = familia
        eMaquinaria.modeloBase = modeloBase

        AgregarVacio(l_Maquinaria)
        If Not (modeloBase.Equals("") OrElse modeloBase.Equals("0")) Then
            cParametros.ListarDetalleCsa(strConexionSql, eMaquinaria, eValidacion, l_Maquinaria)
        End If

        AgregarOtros(l_Maquinaria)

        Return l_Maquinaria

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarNroSerie(ByVal prefijo As String) As List(Of beMaquinaria)

        Dim cParametros As New bcParametros
        Dim eMaquinaria As New beMaquinaria
        Dim eValidacion As New beValidacion
        Dim l_Maquinaria As New List(Of beMaquinaria)

        eMaquinaria.prefijo = prefijo

        AgregarVacio(l_Maquinaria)
        If Not (prefijo.Equals("") OrElse prefijo.Equals("0")) Then
            cParametros.ListarNroSerie(strConexion, strLibreria, eMaquinaria, eValidacion, l_Maquinaria)
        End If
        AgregarOtros(l_Maquinaria)

        Return (l_Maquinaria)

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function AceptarMaquinaria(ByVal tipoOperacion As String, _
                                                ByVal item As String, _
                                                ByVal maquinaNueva As String, _
                                                ByVal familia As String, ByVal familiaOt As String, _
                                                ByVal modeloBase As String, ByVal modeloBaseOt As String, _
                                                ByVal prefijo As String, ByVal prefijoOt As String, _
                                                ByVal numeroMaquinas As String, _
                                                ByVal numeroSerie As String, ByVal numeroSerieOt As String, _
                                                ByVal horometroInicial As String, _
                                                ByVal fechaHorometro As String, _
                                                ByVal horasPromedioMensual As String, _
                                                ByVal codDepartamento As String, _
                                                ByVal departamento As String, _
                                                ByVal session As String) As beValidacion
        Dim eValidacion As New beValidacion
        'Dim eMaquinaria As New beMaquinaria
        'Dim eProducto As New beProducto
        'Dim eProductoSession As New beProducto
        'Dim eCotizacion As New beCotizacion
        'Dim indMaquinaEncontrado As Integer = 0

        'eProductoSession = CType(HttpContext.Current.Session(String.Concat(Nomb_Producto, session)), beProducto) 'CType(Session_Producto, beProducto)

        ''Copiar para q solo al final actualizarlo en la session si es que este pasa las validaciones
        'eProducto = Modulo.CopiarClase(eProductoSession)

        'Dim ItemFila As Integer = 0

        'Dim claseCSA As String = eProducto.ProductoCSA.ClaseCsa

        'Try
        '    If tipoOperacion = "1" Then
        '        Call CargarDatosxMaquinaria(eMaquinaria, item, maquinaNueva, familia, _
        '                               familiaOt, modeloBase, modeloBaseOt, prefijo, prefijoOt, _
        '                               numeroMaquinas, numeroSerie, numeroSerieOt, horometroInicial, fechaHorometro, _
        '                               horasPromedioMensual, codDepartamento, departamento, claseCSA)
        '        eProducto.ProductoCSA.ListaMaquinaria.Add(eMaquinaria)
        '        eProducto.ProductoCSA.ListaMaquinaria.ForEach(AddressOf ForEach_Maquinaria)
        '        Call RecalcularMaquinaria(eCotizacion, eProducto.ProductoCSA, eMaquinaria)
        '        'eProducto.ProductoCSA.ListaMaquinaria.ForEach(AddressOf ForEach_Maquinaria)
        '        'eProducto.ProductoCSA.ListaMaquinaria.Add(eMaquinaria)
        '    ElseIf tipoOperacion = "2" Then
        '        For Each eMaquina As beMaquinaria In eProducto.ProductoCSA.ListaMaquinaria

        '            If eMaquina.item.Equals(item) Then

        '                'Para poder tener una copia cuando se edita
        '                eMaquinaria = Modulo.CopiarClase(eMaquina)

        '                Call CargarDatosxMaquinaria(eMaquina, item, maquinaNueva, familia, _
        '                                       familiaOt, modeloBase, modeloBaseOt, prefijo, prefijoOt, _
        '                                       numeroMaquinas, numeroSerie, numeroSerieOt, horometroInicial, fechaHorometro, _
        '                                       horasPromedioMensual, codDepartamento, departamento, claseCSA)
        '                eProducto.ProductoCSA.ListaMaquinaria.ForEach(AddressOf ForEach_Maquinaria)
        '                Call RecalcularMaquinaria(eCotizacion, eProducto.ProductoCSA, eMaquina)
        '                'eProducto.ProductoCSA.ListaMaquinaria(ItemFila) = eMaquina
        '                Exit For
        '            End If
        '            indMaquinaEncontrado = indMaquinaEncontrado + 1
        '            ItemFila += 1

        '        Next
        '    End If

        '    'Validacion Montos de Tarifas
        '    If eProducto.ProductoCSA.ClaseCsa = Homologacion.ClaseCSA.Planes Then
        '        eValidacion = fc_ValidarCalculoMontoProducto(eProducto)
        '        If eValidacion.mensaje <> String.Empty Then

        '            eValidacion.mensaje = eValidacion.mensaje & ". Comuníquese con la línea del producto"
        '            eValidacion.validacion = False

        '            If tipoOperacion = "1" Then
        '                eProducto.ProductoCSA.ListaMaquinaria.Remove(eMaquinaria)
        '            ElseIf tipoOperacion = "2" Then
        '                eProducto.ProductoCSA.ListaMaquinaria(indMaquinaEncontrado) = eMaquinaria
        '            End If

        '            Return eValidacion
        '        End If
        '    End If

        '    'Actulizar el producto de la Session de Cotizacion
        '    eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion) 'CType(Session_Cotizacion, beCotizacion)
        '    For i As Integer = 0 To eCotizacion.ListaProducto.Count - 1
        '        If eCotizacion.ListaProducto(i).IdProducto = eProducto.IdProducto Then
        '            eCotizacion.ListaProducto(i) = eProducto : Exit For
        '        End If
        '    Next


        '    'Actualizar las Sessiones
        '    HttpContext.Current.Session(String.Concat(Nomb_Producto, session)) = eProducto

        '    HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)) = eCotizacion


        '    eValidacion.validacion = True
        'Catch ex As Exception
        '    eValidacion.validacion = False
        '    eValidacion.mensaje = ex.Message.ToString
        'End Try

        'Return eValidacion

        '--------------------------------------------------
        eValidacion = f_AceptarMaquinaria(tipoOperacion, item, maquinaNueva, _
                                                 familia, familiaOt, _
                                                 modeloBase, modeloBaseOt, _
                                                 prefijo, prefijoOt, _
                                                 numeroMaquinas, _
                                                 numeroSerie, numeroSerieOt, _
                                                 horometroInicial, _
                                                 fechaHorometro, _
                                                 horasPromedioMensual, _
                                                 codDepartamento, _
                                                 departamento, _
                                                 session)
        Return eValidacion
        '-------------------------------------------------------
    End Function

    Public Shared Function f_AceptarMaquinaria(ByVal tipoOperacion As String, ByVal item As String, ByVal maquinaNueva As String, _
                                                ByVal familia As String, ByVal familiaOt As String, _
                                                ByVal modeloBase As String, ByVal modeloBaseOt As String, _
                                                ByVal prefijo As String, ByVal prefijoOt As String, _
                                                ByVal numeroMaquinas As String, _
                                                ByVal numeroSerie As String, ByVal numeroSerieOt As String, _
                                                ByVal horometroInicial As String, _
                                                ByVal fechaHorometro As String, _
                                                ByVal horasPromedioMensual As String, _
                                                ByVal codDepartamento As String, _
                                                ByVal departamento As String, _
                                                ByVal session As String) As beValidacion

        Dim eValidacion As New beValidacion
        Dim eMaquinaria As New beMaquinaria
        Dim eProducto As New beProducto
        Dim eProductoSession As New beProducto
        Dim eCotizacion As New beCotizacion
        Dim indMaquinaEncontrado As Integer = 0

        eProductoSession = CType(HttpContext.Current.Session(String.Concat(Nomb_Producto, session)), beProducto) 'CType(Session_Producto, beProducto)

        'Copiar para q solo al final actualizarlo en la session si es que este pasa las validaciones
        eProducto = Modulo.CopiarClase(eProductoSession)

        Dim ItemFila As Integer = 0

        Dim claseCSA As String = eProducto.ProductoCSA.ClaseCsa

        Try
            If tipoOperacion = "1" Then
                Call CargarDatosxMaquinaria(eMaquinaria, item, maquinaNueva, familia, _
                                       familiaOt, modeloBase, modeloBaseOt, prefijo, prefijoOt, _
                                       numeroMaquinas, numeroSerie, numeroSerieOt, horometroInicial, fechaHorometro, _
                                       horasPromedioMensual, codDepartamento, departamento, claseCSA)
                eProducto.ProductoCSA.ListaMaquinaria.Add(eMaquinaria)
                eProducto.ProductoCSA.ListaMaquinaria.ForEach(AddressOf ForEach_Maquinaria)
                Call RecalcularMaquinaria(eCotizacion, eProducto.ProductoCSA, eMaquinaria)
                'eProducto.ProductoCSA.ListaMaquinaria.ForEach(AddressOf ForEach_Maquinaria)
                'eProducto.ProductoCSA.ListaMaquinaria.Add(eMaquinaria)
            ElseIf tipoOperacion = "2" Then
                For Each eMaquina As beMaquinaria In eProducto.ProductoCSA.ListaMaquinaria

                    If eMaquina.item.Equals(item) Then

                        'Para poder tener una copia cuando se edita
                        eMaquinaria = Modulo.CopiarClase(eMaquina)

                        Call CargarDatosxMaquinaria(eMaquina, item, maquinaNueva, familia, _
                                               familiaOt, modeloBase, modeloBaseOt, prefijo, prefijoOt, _
                                               numeroMaquinas, numeroSerie, numeroSerieOt, horometroInicial, fechaHorometro, _
                                               horasPromedioMensual, codDepartamento, departamento, claseCSA)
                        eProducto.ProductoCSA.ListaMaquinaria.ForEach(AddressOf ForEach_Maquinaria)
                        Call RecalcularMaquinaria(eCotizacion, eProducto.ProductoCSA, eMaquina)
                        'eProducto.ProductoCSA.ListaMaquinaria(ItemFila) = eMaquina
                        Exit For
                    End If
                    indMaquinaEncontrado = indMaquinaEncontrado + 1
                    ItemFila += 1

                Next
            End If

            'Validacion Montos de Tarifas
            If eProducto.ProductoCSA.ClaseCsa = Homologacion.ClaseCSA.Planes Then
                eValidacion = fc_ValidarCalculoMontoProducto(eProducto)
                If eValidacion.mensaje <> String.Empty Then

                    eValidacion.mensaje = eValidacion.mensaje & ". Comuníquese con la línea del producto"
                    eValidacion.validacion = False

                    If tipoOperacion = "1" Then
                        eProducto.ProductoCSA.ListaMaquinaria.Remove(eMaquinaria)
                    ElseIf tipoOperacion = "2" Then
                        eProducto.ProductoCSA.ListaMaquinaria(indMaquinaEncontrado) = eMaquinaria
                    End If

                    Return eValidacion
                End If
            End If

            'Actulizar el producto de la Session de Cotizacion
            eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion) 'CType(Session_Cotizacion, beCotizacion)
            For i As Integer = 0 To eCotizacion.ListaProducto.Count - 1
                If eCotizacion.ListaProducto(i).IdProducto = eProducto.IdProducto Then
                    eCotizacion.ListaProducto(i) = eProducto : Exit For
                End If
            Next


            'Actualizar las Sessiones
            HttpContext.Current.Session(String.Concat(Nomb_Producto, session)) = eProducto

            HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)) = eCotizacion


            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.validacion = False
            eValidacion.mensaje = ex.Message.ToString
        End Try

        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function EliminarMaquinaria(ByVal item As String, ByVal session As String) As beValidacion

        Dim eProducto As New beProducto
        'Dim IdRequestCliente As String = Modulo.IdRequestCliente(HttpContext.Current.Request.UrlReferrer.Query.Trim)

        eProducto = CType(HttpContext.Current.Session(String.Concat(Nomb_Producto, session)), beProducto) 'CType(Session_Producto, beProducto)

        For Each Rpt As beMaquinaria In eProducto.ProductoCSA.ListaMaquinaria
            If Rpt.item = item Then
                eProducto.ProductoCSA.ListaMaquinaria.Remove(Rpt) : Exit For
            End If
        Next

        HttpContext.Current.Session(String.Concat(Nomb_Producto, session)) = eProducto
        'Session_Producto = eProducto
        Dim eValidacion As New beValidacion
        eValidacion.validacion = True

        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarProductoCsaActualizado(ByVal IncluyeFluidos As String, _
                                                            ByVal IncluyeDetallePartes As String, _
                                                            ByVal FechaInicioContrato As String, _
                                                            ByVal FechaEstimadaCierre As String, _
                                                            ByVal ParticipacionVendedor1 As String, _
                                                            ByVal ParticipacionVendedor2 As String, _
                                                            ByVal session As String) As beProducto

        Dim eProducto As New beProducto
        Dim eValidacion As New beValidacion

        'Dim IdRequestCliente As String = Modulo.IdRequestCliente(HttpContext.Current.Request.UrlReferrer.Query.Trim)

        eProducto = CType(HttpContext.Current.Session(String.Concat(Nomb_Producto, session)), beProducto) 'CType(Session_Producto, beProducto)

        eProducto.ProductoCSA.IncluyeFluidos = CBool(IncluyeFluidos)
        eProducto.ProductoCSA.IncluyeDetallePartes = CBool(IncluyeDetallePartes)
        eProducto.ProductoCSA.FechaInicioContrato = FechaInicioContrato
        eProducto.ProductoCSA.FechaEstimadaCierre = FechaEstimadaCierre
        eProducto.ProductoCSA.ParticipacionVendedor1 = ParticipacionVendedor1
        eProducto.ProductoCSA.ParticipacionVendedor2 = ParticipacionVendedor2

        If eProducto.ProductoCSA.ClaseCsa = Homologacion.ClaseCSA.Planes Then
            eValidacion = fc_CalculoProductoCsa(eProducto)
        End If

        HttpContext.Current.Session(String.Concat(Nomb_Producto, session)) = eProducto
        'Session_Producto = eProducto

        Return eProducto

    End Function

    Public Shared Function FormatearProducto(ByRef obeProducto) As Boolean
        Dim bolReturn As Boolean = False
        Try


            obeProducto.ValorLista = Modulo.FormatoMoneda(CDbl(obeProducto.ValorLista))
            obeProducto.ValorReal = Modulo.FormatoMoneda(CDbl(obeProducto.ValorReal))
            obeProducto.PorcDescuento = Modulo.FormatoMoneda(CDbl(obeProducto.PorcDescuento))
            obeProducto.DescuentoImp = Modulo.FormatoMoneda(CDbl(obeProducto.DescuentoImp))
            obeProducto.Flete = Modulo.FormatoMoneda(CDbl(obeProducto.Flete))
            obeProducto.ValorVenta = Modulo.FormatoMoneda(CDbl(obeProducto.ValorVenta))
            obeProducto.PorcImpuesto = Modulo.FormatoMoneda(CDbl(obeProducto.PorcImpuesto))
            obeProducto.PrecioVentaFinal = Modulo.FormatoMoneda(CDbl(obeProducto.PrecioVentaFinal))  'String.Format("{0:#,##0.00}", CDbl(obeProducto.PrecioVentaFinal))

            bolReturn = True
        Catch ex As Exception

        End Try
        Return bolReturn
    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function AceptarProductoCsa(ByVal IncluyeFluidos As String, _
                                              ByVal IncluyeDetallePartes As String, _
                                              ByVal FechaInicioContrato As String, _
                                              ByVal FechaEstimadaCierre As String, _
                                              ByVal ParticipacionVendedor1 As String, _
                                              ByVal ParticipacionVendedor2 As String, _
                                              ByVal session As String) As beValidacion
        Dim eCotizacion As New beCotizacion
        Dim eProducto As New beProducto
        Dim eValidacion As New beValidacion

        Try
            'Dim IdRequestCliente As String = Modulo.IdRequestCliente(HttpContext.Current.Request.UrlReferrer.Query.Trim)
            eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion) ' CType(Session_Cotizacion, beCotizacion)
            eProducto = CType(HttpContext.Current.Session(String.Concat(Nomb_Producto, session)), beProducto) 'CType(Session_Producto, beProducto)

            eProducto.ProductoCSA.IncluyeFluidos = CBool(IncluyeFluidos)
            eProducto.ProductoCSA.IncluyeDetallePartes = CBool(IncluyeDetallePartes)
            eProducto.ProductoCSA.FechaInicioContrato = FechaInicioContrato
            eProducto.ProductoCSA.FechaEstimadaCierre = FechaEstimadaCierre
            eProducto.ProductoCSA.ParticipacionVendedor1 = ParticipacionVendedor1
            eProducto.ProductoCSA.ParticipacionVendedor2 = ParticipacionVendedor2

            For Each Rpt As beProducto In eCotizacion.ListaProducto
                If Rpt.IdProductoSap = eProducto.IdProductoSap AndAlso Rpt.IdPosicion = eProducto.IdPosicion Then
                    Rpt = eProducto : Exit For
                End If
            Next
            HttpContext.Current.Session(String.Concat(Nomb_Producto, session)) = Nothing
            'Session_Producto = Nothing
            Call ActualizarCalculosCotizacion(eCotizacion)
            HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)) = eCotizacion
            'Session_Cotizacion = eCotizacion

            eValidacion.validacion = True

        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function GrabarCotizacion(ByVal session As String) As beValidacion

        Dim strRecorrido As String = String.Empty
        strRecorrido = "1"

        Dim eValidacion As New beValidacion
        Dim eCotizacion As New beCotizacion
        Dim cCotizacion As New bcCotizacion

        Try
            strRecorrido = "2"
            eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion)

            strRecorrido = "3"
            eValidacion.usuario = HttpContext.Current.Session(String.Concat(Nomb_Usuario, session))

            strRecorrido = "4"
            cCotizacion.ActualizarCotizacionEnvioSAP(strConexionSql, eCotizacion, eValidacion)

            'Guarda el contenido de los combos Solucion Combinada
            If Not IsNothing(eCotizacion.ListaSolucionCombinada) AndAlso eCotizacion.ListaSolucionCombinada.Count > 0 Then
                strRecorrido = "4.1 - SC "

                For Each auxProducto In eCotizacion.ListaProducto
                    'Dim eProducto As beProducto = CType(HttpContext.Current.Session(String.Concat(Nomb_Producto, session)), beProducto)
                    Dim l_SolComb As IEnumerable(Of beSolucionCombinada) = eCotizacion.ListaSolucionCombinada.Where(Function(c) c.IdProducto = auxProducto.IdProductoSap)

                    cCotizacion.ActualizarSolComb(strConexionSql, CType(l_SolComb.ToList, List(Of beSolucionCombinada)), eCotizacion.IdCotizacionSap, auxProducto.IdProductoSap, auxProducto.IdPosicion, auxProducto.IdTarifaRS, eValidacion)
                Next
                
            End If

            '--------------------------------------------------
            'Reiniciar a false para mostrar el mensaje en pantalla debidamente
            eValidacion.validacion = False
            '--------------------------------------------------
            strRecorrido = "5"
            Dim ServicioSAP As New WSAccesoLocal
            Dim UbicServidor As String = String.Empty
            UbicServidor = Modulo.strUbicServidor

            Select Case UbicServidor.ToString
                Case UbicacionServidor.Desarrollo
                    strRecorrido = "6"
                    ServicioSAP.GuardarCambiosSapDEV(eCotizacion, eValidacion)
                    Exit Select
                Case UbicacionServidor.Calidad
                    strRecorrido = "7"
                    ServicioSAP.GuardarCambiosSapQA(eCotizacion, eValidacion)
                    Exit Select
                Case UbicacionServidor.Produccion
                    strRecorrido = "8"
                    ServicioSAP.GuardarCambiosSapPRD(eCotizacion, eValidacion)
                    Exit Select
            End Select
            strRecorrido = "9"
        Catch ex As Exception
            Dim mensaje As String = "Error en comunicación con Sap : "
            mensaje = String.Concat(mensaje, strRecorrido, " - ", ex.Message, " - ", ex.StackTrace)

            eValidacion.mensaje = mensaje
            eValidacion.validacion = False
        End Try
        'ServicioSAP.GuardarCambiosSAP(eCotizacion, eValidacion)
        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ActualizarCotizacionVersion(ByVal IdCotizacionVersion As String, ByVal NombreArchivo As String) As beValidacion

        Dim eValidacion As New beValidacion
        Dim obeCotizacionVersion As New beCotizacionVersion
        Dim obcCotizacionVersion As New bcCotizacionVersion

        obeCotizacionVersion.IdCotizacionVersion = IdCotizacionVersion
        obeCotizacionVersion.NombreArchivo = NombreArchivo
        Try
            obcCotizacionVersion.ActualizarArchivo(strConexionSql, obeCotizacionVersion, eValidacion)
        Catch ex As Exception

            eValidacion.mensaje = ex.Message
            eValidacion.validacion = False
        End Try
        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function KeepActiveSession(ByVal session As String) As String
        Dim bolRetorno As String = "1"

        'Dim IdRequestCliente As String = Modulo.IdRequestCliente(HttpContext.Current.Request.UrlReferrer.Query.Trim)
        If HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)) Is Nothing Then
            bolRetorno = "0"
        End If
        'If Session_Producto Is Nothing Then
        '    bolRetorno = "0"
        'End If

        'If Session_VerLista Is Nothing Then
        '    bolRetorno = "0"
        'End If

        Return bolRetorno
    End Function

    'Public Function Compare(ByVal x As Ferreyros.BECotizador.beCotizacion, ByVal y As Ferreyros.BECotizador.beCotizacion) As Integer Implements System.Collections.Generic.IComparer(Of Ferreyros.BECotizador.beCotizacion).Compare
    '    Return New CaseInsensitiveComparer().Compare(y, x)
    'End Function

    'Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
    '    Return New CaseInsensitiveComparer().Compare(y, x)
    'End Function

    Public Function CompararEntidad(ByVal objEntidad1 As beCotizacion, ByVal objEntidad2 As beCotizacion) As Boolean

        'Dim l As New List(Of beCotizacion)
        'Dim x1 = From Tabla1 In l
        '         Select Tabla1

        'If objEntidad1.AnexoTelefonoResponsable = objEntidad2.AnexoTelefonoResponsable _
        '    AndAlso objEntidad1.CargoResponsable = objEntidad2.CargoResponsable _
        '    AndAlso objEntidad1.DescripResponsable = objEntidad2.DescripResponsable _
        '    AndAlso objEntidad1.DescripSolicitante = objEntidad2.DescripSolicitante _
        '    AndAlso objEntidad1.DNISolicitante = objEntidad2.DNISolicitante _
        '    AndAlso objEntidad1.EmailResponsable = objEntidad2.EmailResponsable _
        '    AndAlso objEntidad1.FechaCreacion = objEntidad2.FechaCreacion _
        '    AndAlso objEntidad1.FechaEstimadaFacturacion = objEntidad2.FechaEstimadaFacturacion _
        '    AndAlso objEntidad1.FechaFinalValidez = objEntidad2.FechaFinalValidez _
        '    AndAlso objEntidad1.FechaInicioValidez = objEntidad2.FechaInicioValidez _
        '    AndAlso objEntidad1.FechaModificacion = objEntidad2.FechaModificacion _
        '    AndAlso objEntidad1.FechaPrecio = objEntidad2.FechaPrecio _
        '    AndAlso objEntidad1.FlatEliminado = objEntidad2.FlatEliminado _
        '    AndAlso objEntidad1.IdCompania = objEntidad2.IdCompania _
        '    AndAlso objEntidad1.IdCorporacion = objEntidad2.IdCorporacion _
        '    AndAlso objEntidad1.IdCotizacion = objEntidad2.IdCotizacion _
        '    AndAlso objEntidad1.IdCotizacionSap = objEntidad2.IdCotizacionSap _
        '    AndAlso objEntidad1.IdPersonaResponsable = objEntidad2.IdPersonaResponsable _
        '    AndAlso objEntidad1.IdSolicitante = objEntidad2.IdSolicitante _
        '    AndAlso objEntidad1.ItemOportunidad = objEntidad2.ItemOportunidad _
        '    AndAlso objEntidad1.ListabeCotizacionContacto Is objEntidad2.ListabeCotizacionContacto _
        '    AndAlso objEntidad1.ListabeProductoPrime Is objEntidad2.ListabeProductoPrime _
        '    AndAlso objEntidad1.ListaProducto Is objEntidad2.ListaProducto _
        '    AndAlso objEntidad1.MonedaValorBruto = objEntidad2.MonedaValorBruto _
        '    AndAlso objEntidad1.MonedaValorImpuesto = objEntidad2.MonedaValorImpuesto Then
        '    Return True
        'Else
        '    Return False
        'End If
        Return True
    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarOfertaValor(ByVal session As String) As List(Of beCotizacionVersion)

        Dim lista As New List(Of beCotizacionVersion)
        Dim obcCotizacionVersion As New bcCotizacionVersion
        Dim obeCotizacionVersion As New beCotizacionVersion

        Dim eCotizacion As New beCotizacion

        'Dim IdRequestCliente As String = Modulo.IdRequestCliente(HttpContext.Current.Request.UrlReferrer.Query.Trim)
        eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion) 'CType(Session_Cotizacion, beCotizacion)

        obeCotizacionVersion.IdCotizacionSap = eCotizacion.IdCotizacionSap
        obcCotizacionVersion.BuscarIdCotizacionSap(Modulo.strConexionSql, obeCotizacionVersion, lista)

        Return lista

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarVerLista(ByVal session As String) As List(Of beTablaMaestra)

        Dim l_TablaMaestra As New List(Of beTablaMaestra)
        Dim eCotizacion As New beCotizacion
        Dim esCompania As Boolean = False
        Dim esTipoProducto As Boolean = False
        Dim esAccesorio As Boolean = False

        '----------------------------------

        Dim listabeHomologacion As New List(Of beHomologacion)
        Dim obeHomologacion As New beHomologacion
        Dim obcHomologacion As New ImpresionExtender

        Dim codFamilia As String = ""
        Dim valores() As String

        eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion)

        l_TablaMaestra = fc_ConsultarSeccionesCotizacion()

        Try
            obeHomologacion.Tabla = TablaHomologacion.REG_COTIZACION_LISTA_COMPLETA_EMPRESA
            obeHomologacion.ValorSap = eCotizacion.IdCompania
            listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
            If Not listabeHomologacion Is Nothing AndAlso listabeHomologacion.Count > 0 Then
                obeHomologacion = listabeHomologacion.First
                If (obeHomologacion.ValorCotizador = "TRUE") Then
                    esCompania = True
                End If
            End If
        Catch ex As Exception
            esCompania = False
        End Try


        If eCotizacion.ListaProducto.Count > 0 Then

            listabeHomologacion.Clear()

            Try
                obeHomologacion.Tabla = TablaHomologacion.REG_COTIZACION_LISTA_COMPLETA_TIPO_PRODUCTO
                obeHomologacion.ValorSap = eCotizacion.ListaProducto.Item(0).TipoProducto
                listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
                If Not listabeHomologacion Is Nothing AndAlso listabeHomologacion.Count > 0 Then
                    obeHomologacion = listabeHomologacion.First
                    If (obeHomologacion.ValorCotizador = "TRUE") Then
                        esTipoProducto = True
                    End If
                End If
            Catch ex As Exception
                esTipoProducto = False
            End Try


            listabeHomologacion.Clear()

            Try
                obeHomologacion.Tabla = TablaHomologacion.REG_COTIZACION_LISTA_COMPLETA_ACCESORIO
                obeHomologacion.ValorSap = eCotizacion.ListaProducto.Item(0).TipoProducto
                listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
                If Not listabeHomologacion Is Nothing AndAlso listabeHomologacion.Count > 0 Then
                    obeHomologacion = listabeHomologacion.First
                    valores = Split(obeHomologacion.ValorCotizador, ",")
                    codFamilia = eCotizacion.ListaProducto.Item(0).CodigoFamilia.Substring(0, Val(valores(0)))
                    If (valores.Length > 1 AndAlso codFamilia = valores(1)) Then
                        esAccesorio = True
                    End If
                End If
            Catch ex As Exception
                esAccesorio = False
            End Try

        End If

        If Not (esCompania AndAlso esTipoProducto) OrElse esAccesorio Then
            Dim lista = From tabla In l_TablaMaestra Take 6
            l_TablaMaestra = CType(lista.ToList, List(Of beTablaMaestra))
        End If

        '---------------Fin Codigo Nuevo


        'Dim IdRequestCliente As String = Modulo.IdRequestCliente(HttpContext.Current.Request.UrlReferrer.Query.Trim)
        HttpContext.Current.Session(String.Concat(Nomb_VerLista, session)) = l_TablaMaestra

        'Session_VerLista = l_TablaMaestra

        Return l_TablaMaestra

    End Function

    <System.Web.Services.WebMethod(enableSession:=True, CacheDuration:=0)>
    Public Shared Function DescargarCotizacion(ByVal IdSeccion As String, ByVal Imprimir As String, ByVal Guardar As String, ByVal session As String) As beValidacion

        Dim eValidacion As New beValidacion
        Dim l_TablaMaestra As New List(Of beTablaMaestra)
        Dim cTablaMaestra As New bcTablaMaestra

        Try
            Dim l_IdSeccion As String() = IdSeccion.Split(",")
            Dim l_Imprimir As String() = Imprimir.Split(",")
            Dim l_Result As New List(Of beTablaMaestra)
            Dim PosicionInicial As Integer = 0

            'Dim IdRequestCliente As String = Modulo.IdRequestCliente(HttpContext.Current.Request.UrlReferrer.Query.Trim)

            l_TablaMaestra = CType(HttpContext.Current.Session(String.Concat(Nomb_VerLista, session)), List(Of beTablaMaestra)) 'CType(Session_VerLista, List(Of beTablaMaestra))

            eValidacion.usuario = HttpContext.Current.Session(String.Concat(Nomb_Usuario, session)) 'Session_Usuario

            For i As Integer = 0 To l_IdSeccion.Length - 1
                PosicionInicial += 10
                Dim eTablaMaestra As New beTablaMaestra
                eTablaMaestra = l_TablaMaestra.Where(Function(Rpt) Rpt.IdSeccion = l_IdSeccion(i)).First
                eTablaMaestra.Imprimir = l_Imprimir(i)
                eTablaMaestra.PosicionInicial = PosicionInicial
                l_Result.Add(eTablaMaestra)
            Next

            If CBool(Guardar) Then
                cTablaMaestra.MantenimientoListaSeccion(strConexionSql, l_Result, eValidacion)

            End If

            HttpContext.Current.Session(String.Concat(Nomb_VerLista, session)) = l_Result 

            Dim eCotizacionDescarga As New beCotizacion
            Dim eCotizacionRespaldo As New beCotizacion

            'obtener Cotizacion modificada en session
            eCotizacionDescarga = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion) 'CType(Session_Cotizacion, beCotizacion)

            'obtener objeto cotizacion de Respaldo en session
            eCotizacionRespaldo = CType(HttpContext.Current.Session(String.Concat(Nomb_Respaldo, session)), beCotizacion)

            'verificar si hubo cambios en la cotizacion
            'SOLO VERIFICA CAMBIOS EN MAQUINARIA
            Dim boolIguales As Boolean = False
            boolIguales = Modulo.CotizacionesDiferentes(eCotizacionDescarga, eCotizacionRespaldo)
            If boolIguales = True Then
                'Codigo para Grabar
                GrabarCotizacion(session)
            End If

            '=====Grabar soluciones combinadas
            For Each producto As beProducto In eCotizacionDescarga.ListaProducto
                If producto.TipoProducto = "Z005" Then
                    GrabarCotizacion(session)
                    Exit For
                End If
            Next
            '=====

            '=====Obtener los Nombre de los archivos para cada seccion =========
            Dim dtbSeccionArchivo As New DataTable
            Dim obeTablaMaestra As New beTablaMaestra
            Dim obcArchivoConfiguracion As New bcArchivoConfiguracion

            obeTablaMaestra.IdSeccion = IdSeccion.ToString
            obeTablaMaestra.IdTablaMaestra = eCotizacionDescarga.IdCotizacion

            obcArchivoConfiguracion.BuscarArchivoSeccion(Modulo.strConexionSql, obeTablaMaestra, dtbSeccionArchivo)


            Dim objImpresion As New Impresion
            eValidacion = objImpresion.GenerarDocumentoCotizacion(l_Result, dtbSeccionArchivo, eCotizacionDescarga, session)

        Catch ex As Exception
            eValidacion.validacion = False
            eValidacion.mensaje = ex.Message.ToString
        End Try

        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True, CacheDuration:=0)>
    Public Shared Function DescargarCotizacionPrevia(ByVal session As String) As beValidacion

        Dim eValidacion As New beValidacion
        Dim l_TablaMaestra As New List(Of beTablaMaestra)
        Dim cTablaMaestra As New bcTablaMaestra

        Try
             
            eValidacion.usuario = HttpContext.Current.Session(String.Concat(Nomb_Usuario, session))  
            Dim eCotizacionDescarga As New beCotizacion 

            'obtener Cotizacion modificada en session
            eCotizacionDescarga = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion) 'CType(Session_Cotizacion, beCotizacion)
            Dim objImpresion As New Impresion
            eValidacion = objImpresion.GenerarDocumentoCotizacionPrevia(eCotizacionDescarga)

        Catch ex As Exception
            eValidacion.validacion = False
            eValidacion.mensaje = ex.Message.ToString
        End Try

        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ListarProductoVersion(ByVal IdCotizacionVersion As String) As List(Of beCotizacionVersionProducto)

        Dim lista As New List(Of beCotizacionVersionProducto)
        Dim obcCotizacionVersionProducto As New bcCotizacionVersionProducto
        Dim obeCotizacionVersionProducto As New beCotizacionVersionProducto
        obeCotizacionVersionProducto.IdCotizacionVersion = IdCotizacionVersion

        'buscar por IdProductoSap
        obcCotizacionVersionProducto.BuscarIdCotizacionVersion(Modulo.strConexionSql, obeCotizacionVersionProducto, lista)

        Return lista

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function BuscarCotizacionCargar(ByVal numeroCotizacion As String) As List(Of beProducto)
        Dim obcProducto As New bcProducto
        Dim lista As New List(Of beProducto)
        Dim obeProducto As New beProducto
        obeProducto.IdCotizacion = numeroCotizacion ' Solo se utiliza el campo

        obcProducto.BuscarNumeroCotizacion(Modulo.strConexionSql, obeProducto, lista)
        Return lista
    End Function

    <System.Web.Services.WebMethod(enablesession:=True)>
    Public Shared Function BuscarMaquinariaCargar(ByVal IdCotizacionSap As String, ByVal IdPosicion As String, ByVal IdProductoSap As String) As List(Of beMaquinaria)

        Dim l_Maquinaria As New List(Of beMaquinaria)
        l_Maquinaria = f_BuscarMaquinariaCargar(IdCotizacionSap, IdPosicion, IdProductoSap)

        Return l_Maquinaria

    End Function
    Public Shared Function f_BuscarMaquinariaCargar(ByVal IdCotizacionSap As String, ByVal IdPosicion As String, ByVal IdProductoSap As String) As List(Of beMaquinaria)
        Dim eCotizacion As New beCotizacion
        Dim eProducto As New beProducto
        Dim cMaquinaria As New bcMaquinaria
        Dim eValidacion As New beValidacion
        Dim l_Maquinaria As New List(Of beMaquinaria)

        eCotizacion.IdCotizacionSap = IdCotizacionSap
        eProducto.IdPosicion = IdPosicion
        eProducto.IdProductoSap = IdProductoSap

        cMaquinaria.MaquinariaListar(strConexionSql, eCotizacion, eProducto, eValidacion, l_Maquinaria)

        Return l_Maquinaria

    End Function
    <System.Web.Services.WebMethod(enablesession:=True)>
    Public Shared Function AceptarCargarMaquinaria(ByVal IdCotizacionSap As String, ByVal IdPosicion As String, ByVal IdProductoSap As String, ByVal idMaquinaria As String, ByVal session As String) As beValidacion
        Dim eValidacion As New beValidacion
        Dim l_Maquinaria As New List(Of beMaquinaria)
        Dim listaIdMaquinaria As String()
        listaIdMaquinaria = idMaquinaria.Split(",")

        l_Maquinaria = f_BuscarMaquinariaCargar(IdCotizacionSap, IdPosicion, IdProductoSap)

        For Each omaquina In l_Maquinaria

            If Not listaIdMaquinaria.Contains(omaquina.codigo) Then
                Continue For
            End If
            '1:Nuevo 
            eValidacion = f_AceptarMaquinaria("1", omaquina.item, omaquina.maquinaNueva, _
                                                            omaquina.familia, omaquina.familiaOt, _
                                                            omaquina.modeloBase, omaquina.modeloBaseOt, _
                                                            omaquina.prefijo, omaquina.prefijoOt, _
                                                            omaquina.numeroMaquinas, _
                                                            omaquina.numeroSerie, omaquina.numeroSerieOt, _
                                                            omaquina.horometroInicial, _
                                                            omaquina.fechaHorometro, _
                                                            omaquina.horasPromedioMensual, _
                                                            omaquina.codDepartamento, _
                                                            omaquina.departamento, _
                                                            session)
            If eValidacion.validacion = False Then
                Exit For
            End If
        Next
        'eValidacion.validacion = True
        Return eValidacion
    End Function
#End Region


#Region "----- Detalle Producto Prime ----"
    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function BuscarProductoPrimeId(ByVal IdProducto As String, ByVal session As String) As beProducto

        Dim eProducto As New beProducto
        Dim eCotizacion As New beCotizacion
        Dim l_Producto As New List(Of beProducto)

        'Dim IdRequestCliente As String = Modulo.IdRequestCliente(HttpContext.Current.Request.UrlReferrer.Query.Trim)
        eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion) 'CType(Session_Cotizacion, beCotizacion)
        l_Producto = eCotizacion.ListaProducto
        
        For Each Producto As beProducto In l_Producto
            If Producto.IdProducto = IdProducto Then 
                eProducto = Producto
                Exit For
            End If 
        Next

        eProducto.ValorUnitario = String.Format("{0:#,##0.00}", CDbl(eProducto.ValorUnitario))
        'eProducto.ValorNeto = String.Format("{0:#,##0.00}", CDbl(eProducto.ValorNeto))
        eProducto.ValorLista = String.Format("{0:#,##0.00}", CDbl(eProducto.ValorLista))
        eProducto.ValorReal = String.Format("{0:#,##0.00}", CDbl(eProducto.ValorReal))
        eProducto.PorcDescuento = String.Format("{0:#,##0.00}", CDbl(eProducto.PorcDescuento))
        eProducto.DescuentoImp = String.Format("{0:#,##0.00}", CDbl(eProducto.DescuentoImp))
        eProducto.Flete = String.Format("{0:#,##0.00}", CDbl(eProducto.Flete))
        eProducto.ValorVenta = String.Format("{0:#,##0.00}", CDbl(eProducto.ValorVenta))
        eProducto.PorcImpuesto = String.Format("{0:#,##0.00}", CDbl(eProducto.PorcImpuesto))
        eProducto.PrecioVentaFinal = String.Format("{0:#,##0.00}", CDbl(eProducto.PrecioVentaFinal))

        HttpContext.Current.Session(String.Concat(Nomb_Producto, session)) = eProducto
 

        Return eProducto

    End Function
    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function EliminarAdicional(ByVal IdProductoAdicional As String, ByVal session As String) As beValidacion
        Dim blnExito As Boolean = False

        Dim listabeProductoAdicionalEliminado As New List(Of beProductoAdicional)

        Dim arrayIdProductoAdicional() As String = IdProductoAdicional.Split(",")


        Dim eProducto As New beProducto
        eProducto = CType(HttpContext.Current.Session(String.Concat(Nomb_Producto, session)), beProducto) 'CType(Session_Producto, beProducto)
        'For Each valorId As String In arrayIdProductoAdicional.ToArray()
        For Each obeProductoAdicional As beProductoAdicional In eProducto.beProductoPrime.ListabeProductoAdicional
            If arrayIdProductoAdicional.Contains(obeProductoAdicional.IdProductoAdicional) = False Then
                listabeProductoAdicionalEliminado.Add(obeProductoAdicional)
            End If
        Next
        'Next

        For Each ebeProductoAdicional As beProductoAdicional In listabeProductoAdicionalEliminado
            eProducto.beProductoPrime.ListabeProductoAdicional.Remove(ebeProductoAdicional)
        Next

        HttpContext.Current.Session(String.Concat(Nomb_Producto, session)) = eProducto
        'Session_Producto = eProducto
        Dim eValidacion As New beValidacion
        eValidacion.validacion = True

        Return eValidacion

    End Function
    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function AceptarProductoPrime(ByVal IdProducto As String, _
                                              ByVal Cantidad As String, _
                                              ByVal IncluyeCLC As String, _
                                              ByVal PlazoEntrega As String, _
                                              ByVal session As String) As beValidacion
        'eCotizacion = New beCotizacion
        'eProducto = New beProducto
        'eValidacion = New beValidacion

        'Try
        '    eCotizacion = CType(Session_Cotizacion, beCotizacion)
        '    eProducto = CType(Session_Producto, beProducto)

        '    eProducto.Cantidad = Cantidad
        '    eProducto.beProductoPrime.FlatIncluyeCLC = CBool(IncluyeCLC)
        '    eProducto.beProductoPrime.PlazoEntregaEstim = PlazoEntrega
        '    eProducto.ValorNeto = (Cantidad * eProducto.ValorUnitario)


        '    For Each Rpt As beProducto In eCotizacion.ListaProducto
        '        If Rpt.IdProducto = IdProducto Then
        '            Rpt = eProducto : Exit For
        '        End If
        '    Next

        '    'Session_Producto = Nothing
        '    Call ActualizarCalculosCotizacion(eCotizacion)
        '    Session_Cotizacion = eCotizacion
        Dim eValidacion As New beValidacion
        eValidacion.validacion = True

        'Catch ex As Exception
        '    eValidacion.mensaje = ex.Message.ToString
        'End Try

        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarListaLLave(ByVal session As String, ByVal codigoLinea As String) As List(Of beLlave)

        Dim lista As New List(Of beLlave)
        Dim listaReturn As New List(Of beLlave)
        Dim obcLlave As New bcLlave
        Dim obeLinea As New beLinea
        Dim l_eTarifaRS As New List(Of beTarifaRS)
        Dim cTarifaRS As New bcTarifaRS
        Dim eDatoGeneral As New beDatoGeneral
        Dim eCotizacion As New beCotizacion
        Dim eProducto As New beProducto
        Dim l_SolucionCombinada As IEnumerable(Of beSolucionCombinada) 'New List(Of beSolucionCombinada)

        eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion)
        eProducto = CType(HttpContext.Current.Session(String.Concat(Nomb_Producto, session)), beProducto)

        eDatoGeneral.Campo1 = ""
        eDatoGeneral.Campo2 = codigoLinea
        eDatoGeneral.Campo3 = eProducto.IdProductoSap

        cTarifaRS.BuscarCombinacionLlave(Modulo.strConexionSql, eDatoGeneral, l_eTarifaRS)
        HttpContext.Current.Session(String.Concat(Nomb_TarifaRS, session)) = l_eTarifaRS
        HttpContext.Current.Session(String.Concat(Nomb_TarifaRS_Respaldo, session)) = l_eTarifaRS
        

        obeLinea.DescripcionCodigo = codigoLinea
        obcLlave.BuscarCodigoLinea(Modulo.strConexionSql, eProducto.IdProductoSap, obeLinea, lista)

        If Not IsNothing(eCotizacion.ListaSolucionCombinada) Then
            l_SolucionCombinada = eCotizacion.ListaSolucionCombinada.Where(Function(c) c.IdProducto = eProducto.IdProductoSap And c.IdPosicion = eProducto.IdPosicion)

            For Each llave As beLlave In lista
                For Each SolucionCombinada As beSolucionCombinada In l_SolucionCombinada
                    If llave.Campo.ToUpper = SolucionCombinada.LLave.ToUpper Then
                        llave.valorDefault = SolucionCombinada.Valor
                    End If
                Next
                listaReturn.Add(llave)
            Next
        End If
        Return listaReturn
    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function dependenciaCombos(ByVal valor As String, ByVal campo As String, ByVal session As String)

        Dim l_eTarifaRS As New List(Of beTarifaRS)

        l_eTarifaRS = CType(HttpContext.Current.Session(String.Concat(Nomb_TarifaRS, session)), List(Of beTarifaRS))

        HttpContext.Current.Session(String.Concat(Nomb_TarifaRS_Respaldo, session)) = CType(l_eTarifaRS.ToList, List(Of beTarifaRS))

        Dim l_TarifaRS As IEnumerable(Of beTarifaRS) = l_eTarifaRS.Where(Function(c) DataBinder.Eval(c, campo) = valor)

        Return CType(l_TarifaRS.ToList, List(Of beTarifaRS))

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function filtrarCombos(ByVal valor As String, ByVal campo As String, ByVal session As String)

        Dim l_eTarifaRS As New List(Of beTarifaRS)

        l_eTarifaRS = CType(HttpContext.Current.Session(String.Concat(Nomb_TarifaRS_Respaldo, session)), List(Of beTarifaRS))

        Dim l_TarifaRS As IEnumerable(Of beTarifaRS) = l_eTarifaRS.Where(Function(c) DataBinder.Eval(c, campo) = valor)

        HttpContext.Current.Session(String.Concat(Nomb_TarifaRS_Respaldo, session)) = CType(l_TarifaRS.ToList, List(Of beTarifaRS))

        Return CType(l_TarifaRS.ToList, List(Of beTarifaRS))

    End Function
    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function restaurarDatosSesion(ByVal session As String)

        Dim l_eTarifaRS As New List(Of beTarifaRS)

        l_eTarifaRS = CType(HttpContext.Current.Session(String.Concat(Nomb_TarifaRS, session)), List(Of beTarifaRS))

        HttpContext.Current.Session(String.Concat(Nomb_TarifaRS_Respaldo, session)) = CType(l_eTarifaRS.ToList, List(Of beTarifaRS))

        Return "0"
    End Function


    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function BuscarCombinacionLlave(ByVal session As String, ByVal codigoLinea As String, ByVal campos As String, ByVal valores As String, ByVal idProducto As String) As List(Of beTarifaRS)

        Dim eCotizacion As New beCotizacion
        Dim eProductoAux As New beProducto
        Dim l_SolCombinada As New List(Of beSolucionCombinada)
        Dim l_SolCombinadaAux As IEnumerable(Of beSolucionCombinada)

        Dim lista As New List(Of beTarifaRS)
        Dim obcTarifaRS As New bcTarifaRS
        Dim obeDatoGeneral As New beDatoGeneral
        Dim condicion As String = String.Empty
        Dim ArrayCampos As String() = campos.Split("|")
        Dim ArrayValores As String() = valores.Split("|")

        eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion)
        eProductoAux = CType(HttpContext.Current.Session(String.Concat(Nomb_Producto, session)), beProducto)

        Dim idCotizacion As String = eCotizacion.IdCotizacionSap
        Dim idPosicion As String = eProductoAux.IdPosicion

        For i = 0 To ArrayCampos.Count - 1

            'actualizo la lista en sesion
            Dim SolCombinada As New beSolucionCombinada
            SolCombinada.IdCotizacion = idCotizacion
            SolCombinada.IdProducto = idProducto
            SolCombinada.IdPosicion = idPosicion
            SolCombinada.Valor = ArrayValores(i)
            SolCombinada.LLave = ArrayCampos(i)
            l_SolCombinada.Add(SolCombinada)

            If condicion = String.Empty Then
                condicion = ArrayCampos(i) + " = " + "'" + ArrayValores(i) + "'"
            Else
                condicion = condicion + " AND " + ArrayCampos(i) + " = " + "'" + ArrayValores(i) + "'"
            End If
        Next

        obeDatoGeneral.Campo1 = condicion
        obeDatoGeneral.Campo2 = codigoLinea
        obeDatoGeneral.Campo3 = idProducto
        obcTarifaRS.BuscarCombinacionLlave(Modulo.strConexionSql, obeDatoGeneral, lista)


        'guardo la lista solucion combinada en sesion
        'l_SolCombinadaAux = eCotizacion.ListaSolucionCombinada.Where(Function(c) c.IdProducto <> idProducto And c.IdPosicion <> idPosicion)
        l_SolCombinadaAux = eCotizacion.ListaSolucionCombinada.Where(Function(c) c.IdPosicion <> idPosicion)
        l_SolCombinadaAux = l_SolCombinadaAux.Union(l_SolCombinada)
        eCotizacion.ListaSolucionCombinada = CType(l_SolCombinadaAux.ToList, List(Of beSolucionCombinada))
        'HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)) = eCotizacion

        'guardo la tarifa en sesion
        Dim eProducto As beProducto = CType(HttpContext.Current.Session(String.Concat(Nomb_Producto, session)), beProducto)
        If lista.Count = 1 Then
            eProducto.IdTarifaRS = lista.Item(0).IdTarifas

            'calculo los montos
            Call fc_CalculoProductoSolCombinada(eProducto, lista.Item(0).Total)
        Else
            Call fc_CalculoProductoSolCombinada(eProducto, 0)
            eProducto.IdTarifaRS = 0
        End If
        
        'Actualizo la sesion Producto
        HttpContext.Current.Session(String.Concat(Nomb_Producto, session)) = eProducto

        'Actualizo la sesion Cotizacion
        For i As Integer = 0 To eCotizacion.ListaProducto.Count - 1
            If eCotizacion.ListaProducto(i).IdProducto = eProducto.IdProducto Then
                eCotizacion.ListaProducto(i) = eProducto : Exit For
            End If
        Next

        Call ActualizarCalculosCotizacion(eCotizacion)
        HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)) = eCotizacion

        '------------
        'Dim interprete As New Interpreter()

        'interprete.SetVariable("producto", New beProducto)

        'Dim expression As String = "producto.x"

        'Dim  parsedExpression as new Lambda = interprete.Parse(expression, new Parameter("x", String))

        'parsedExpression.Invoke("CodigoLinea")
        '---------

        Return lista

    End Function


#End Region

#Region " -- Detalle Producto Alquiler ---"
    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function BuscarProductoAlquilerId(ByVal IdProducto As String, ByVal session As String) As beProducto

        Dim eProducto As New beProducto
        Dim eCotizacion As New beCotizacion
        Dim l_Producto As New List(Of beProducto)

        eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion) 'CType(Session_Cotizacion, beCotizacion)
        l_Producto = eCotizacion.ListaProducto

        For Each Producto As beProducto In l_Producto
            If Producto.IdProducto = IdProducto Then
                eProducto = Producto : Exit For
            End If
        Next

        HttpContext.Current.Session(String.Concat(Nomb_Producto, session)) = eProducto
        'Session_Producto = eProducto
        Return eProducto

    End Function

#End Region

#Region " -- Detalle Producto Accesorio"
    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function BuscarProductoAccesorioId(ByVal IdProducto As String, ByVal session As String) As beProducto

        Dim eProducto As New beProducto
        Dim eCotizacion As New beCotizacion
        Dim l_Producto As New List(Of beProducto)
        'Dim IdRequestCliente As String = Modulo.IdRequestCliente(HttpContext.Current.Request.UrlReferrer.Query.Trim)
        eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion) 'CType(Session_Cotizacion, beCotizacion)
        l_Producto = eCotizacion.ListaProducto

        For Each Producto As beProducto In l_Producto
            If Producto.IdProducto = IdProducto Then
                eProducto = Producto : Exit For
            End If
        Next
        HttpContext.Current.Session(String.Concat(Nomb_Producto, session)) = eProducto
        'Session_Producto = eProducto
        Return eProducto

    End Function
#End Region

#Region "Producto Solucion Combinada"
    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function BuscarProductoSolucionCombinadaId(ByVal IdProducto As String, ByVal session As String) As beProducto

         Dim eProducto As New beProducto
        Dim eCotizacion As New beCotizacion
        Dim l_Producto As New List(Of beProducto) 
        eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion) 'CType(Session_Cotizacion, beCotizacion)
        l_Producto = eCotizacion.ListaProducto

        For Each Producto As beProducto In l_Producto
            If Producto.IdProducto = IdProducto Then
                eProducto = Producto : Exit For
            End If
        Next
        HttpContext.Current.Session(String.Concat(Nomb_Producto, session)) = eProducto 
        Return eProducto

    End Function
#End Region

End Class