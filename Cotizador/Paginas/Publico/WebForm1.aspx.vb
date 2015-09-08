Imports System.Data.SqlClient

Public Class WebForm1
    Inherits System.Web.UI.Page

    'Private Shared Property Session_Id() As Object
    '    Get
    '        Return HttpContext.Current.Session("id")
    '    End Get
    '    Set(ByVal value As Object)
    '        HttpContext.Current.Session("id") = value
    '    End Set
    'End Property

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    '    Dim NegWS As New WSAccesoLocal
    '    'Dim vCotizacion As New WcfLocal.beCotizacion
    '    'Dim UrlResult As String = String.Empty
    '    'Dim ErrorDescripcion As String = String.Empty

    '    'vCotizacion._IdCotizacion = "102402-2012"
    '    'vCotizacion._IdCorporacion = "000000000001"
    '    'vCotizacion._IdCompanhia = "000000000002"
    '    'vCotizacion._IdSolicitante = "1254"
    '    'vCotizacion._DescripSolicitante = "MINERA YANACOCHA S.A"
    '    'vCotizacion._RUCSolicitante = "11054545401"
    '    'vCotizacion._DNISolicitante = ""
    '    'vCotizacion._IdPersonaResponsable = "7897"
    '    'vCotizacion._DescripResponsable = "Alexander Chosec Algoner"
    '    'vCotizacion._OficinaResponsable = "LIMA"
    '    'vCotizacion._CargoResponsable = "Vendedor"
    '    'vCotizacion._EmailResponsable = "alex@gmail.com"
    '    'vCotizacion._TelefonoResponsable = "2541025"
    '    'vCotizacion._AnexoTelefonoResponsable = "1052"
    '    'vCotizacion._FechaInicioValidez = "10122012"
    '    'vCotizacion._FechaFinalValidez = "31122012"
    '    'vCotizacion._FechaPrecio = "15122012"
    '    'vCotizacion._NumeroOportunidad = ""
    '    'vCotizacion._ItemOportunidad = ""
    '    'vCotizacion._Version = ""
    '    'vCotizacion._ValorNeto = "45265300"
    '    'vCotizacion._MonedaValorNeto = "USD"
    '    'vCotizacion._ValorImpuesto = "1500"
    '    'vCotizacion._MonedaValorImpuesto = "USD"
    '    'vCotizacion._ValorBruto = "45266800"
    '    'vCotizacion._MonedaValorBruto = "USD"
    '    'vCotizacion._ValorTipoCambio = "3"
    '    'vCotizacion._FechaEstimadaFacturacion = "15122012"
    '    'vCotizacion._NombreEstado = "Abierta"
    '    'vCotizacion._Usuario = "achosec"

    '    'Dim l_contacto(1) As WcfLocal.beCotizacionContacto
    '    'Dim econtacto As WcfLocal.beCotizacionContacto

    '    'econtacto = New WcfLocal.beCotizacionContacto
    '    'econtacto._Nombres = "Maria Chavez"
    '    'econtacto._Cargo = "jefa de logistica"
    '    'econtacto._Email = "MariaChavez@gmail.com"
    '    'econtacto._Direccion = "lima"
    '    'econtacto._Telefono = "995620012"

    '    'l_contacto(0) = econtacto

    '    'econtacto = New WcfLocal.beCotizacionContacto
    '    'econtacto._Nombres = "Camila Gonzalez"
    '    'econtacto._Cargo = "secretaria de logistica"
    '    'econtacto._Email = "camilagonzalez@gmail.com"
    '    'econtacto._Direccion = "lima"
    '    'econtacto._Telefono = "996581258"

    '    'l_contacto(1) = econtacto

    '    'vCotizacion._ListaCotizacionContacto = l_contacto

    '    'REM *************************************************************************** 

    '    'Dim l_Producto(1) As WcfLocal.beProducto
    '    'Dim eProducto As WcfLocal.beProducto
    '    'Dim eProductoCsa As WcfLocal.beProductoCSA


    '    'eProducto = New WcfLocal.beProducto
    '    'eProducto._IdPosicion = "10"
    '    'eProducto._IdProducto = "100000006"
    '    'eProducto._Descripcion = "TRAKKER 470"
    '    'eProducto._ValorUnitario = "50000"
    '    'eProducto._IdMonedaValorUnitario = "USD"
    '    'eProducto._Cantidad = "2"
    '    'eProducto._Unidad = "U"
    '    'eProducto._ValorNeto = "100000"
    '    'eProducto._IdMonedaValorNeto = "USD"
    '    'eProducto._NombreEstado = "Sin Cotización"
    '    'eProducto._TipoProducto = "PRIME"
    '    ''eProducto._ProductoPrime 

    '    'l_Producto(0) = eProducto

    '    'eProducto = New WcfLocal.beProducto
    '    'eProducto._IdPosicion = "20"
    '    'eProducto._IdProducto = "100000013"
    '    'eProducto._Descripcion = "PLAN PREMIUN"
    '    'eProducto._ValorUnitario = "8560"
    '    'eProducto._IdMonedaValorUnitario = "USD"
    '    'eProducto._Cantidad = "1"
    '    'eProducto._Unidad = "H"
    '    'eProducto._ValorNeto = "8560"
    '    'eProducto._IdMonedaValorNeto = "USD"
    '    'eProducto._NombreEstado = "Sin Cotización"
    '    'eProducto._TipoProducto = "CSA"

    '    'eProductoCsa = New WcfLocal.beProductoCSA
    '    'eProductoCsa._ClaseCsa = "P"
    '    'eProductoCsa._FechaInicioContrato = "15102012"
    '    'eProductoCsa._IdTipoCSA = "T1"
    '    'eProductoCsa._IdPlan = "P01"
    '    'eProductoCsa._DescripcionPlan = "PLAN BÁSICO"
    '    'eProductoCsa._IncluyeFluidos = "1"
    '    'eProductoCsa._IncluyeDetallePartes = "1"
    '    'eProductoCsa._IdUnidadPlazoRenovacion = "1"
    '    'eProductoCsa._PlazoRenovacion = "250"
    '    'eProductoCsa._Duracion = "2000"
    '    'eProductoCsa._Tiempo = "HR"


    '    'Dim l_Maquinaria(0) As WcfLocal.beMaquinaria
    '    'Dim eMaquinaria As New WcfLocal.beMaquinaria

    '    'eMaquinaria._CodDepartamento = "X00"
    '    'eMaquinaria._Departamento = "TUMBES"
    '    'eMaquinaria._Familia = "EXCAVADORAS HIDRÁULICAS"
    '    'eMaquinaria._FechaHorometro = "15092012"
    '    'eMaquinaria._HorasPromedioMensual = "500"
    '    'eMaquinaria._HorometroFinal = "2200"
    '    'eMaquinaria._HorometroInicial = "200"
    '    'eMaquinaria._MaquinaNueva = "1"
    '    'eMaquinaria._Modelo = "329D L"
    '    'eMaquinaria._ModeloBase = "329"
    '    'eMaquinaria._NumeroSerie = "0JHJ00352"
    '    'eMaquinaria._Prefijo = "JHJ"
    '    'eMaquinaria._Renovacion = "0"
    '    'eMaquinaria._RenovacionValida = "0"

    '    'l_Maquinaria(0) = eMaquinaria
    '    'eProductoCsa._ListaMaquinaria = l_Maquinaria

    '    'eProducto._ProductoCSA = eProductoCsa

    '    'l_Producto(1) = eProducto


    '    'vCotizacion._ListaProducto = l_Producto

    '    'NegWS.WS_InsertarCotizacion(vCotizacion, UrlResult, ErrorDescripcion)



    '    '==================================================================================
    '    'PRUEBA COTIZACION PRIME
    '    '==================================================================================

    '     Dim obeCotizacion As New WcfLocal.beCotizacion
    '    Dim UrlResult As String = String.Empty
    '    Dim ErrorDescripcion As String = String.Empty

    '    obeCotizacion._IdCotizacion = "2000293"
    '    obeCotizacion._IdCorporacion = "000000000001"
    '    obeCotizacion._IdCompanhia = "000000000002"
    '    obeCotizacion._IdSolicitante = "1254"
    '    obeCotizacion._DescripSolicitante = "MINERA YANACOCHA S.A"
    '    obeCotizacion._RUCSolicitante = "11054545401"
    '    obeCotizacion._DNISolicitante = ""
    '    obeCotizacion._IdPersonaResponsable = "7898"
    '    obeCotizacion._DescripResponsable = "Elmer Rojas"
    '    obeCotizacion._OficinaResponsable = "LIMA"
    '    obeCotizacion._CargoResponsable = "Vendedor"
    '    obeCotizacion._EmailResponsable = "elmer.rojas@gmail.com"
    '    obeCotizacion._TelefonoResponsable = "1234567"
    '    obeCotizacion._AnexoTelefonoResponsable = "1052"
    '    obeCotizacion._FechaInicioValidez = "18122012"
    '    obeCotizacion._FechaFinalValidez = "31122012"
    '    obeCotizacion._FechaPrecio = "20122012"
    '    obeCotizacion._NumeroOportunidad = ""
    '    obeCotizacion._ItemOportunidad = "0000000010"
    '    obeCotizacion._Version = ""
    '    obeCotizacion._ValorNeto = "55000000"
    '    obeCotizacion._MonedaValorNeto = "USD"
    '    obeCotizacion._ValorImpuesto = "1500"
    '    obeCotizacion._MonedaValorImpuesto = "USD"
    '    obeCotizacion._ValorBruto = "55001500"
    '    obeCotizacion._MonedaValorBruto = "USD"
    '    obeCotizacion._ValorTipoCambio = "3"
    '    obeCotizacion._FechaEstimadaFacturacion = "15122012"
    '    obeCotizacion._NombreEstado = "Abierta"
    '    obeCotizacion._Usuario = "erojas"

    '    Dim listaContacto As New List(Of WcfLocal.beCotizacionContacto)
    '    Dim econtacto As New WcfLocal.beCotizacionContacto
    '    econtacto._Nombres = "Camila Gonzalez"
    '    econtacto._Cargo = "secretaria de logistica"
    '    econtacto._Email = "camilagonzalez@gmail.com"
    '    econtacto._Direccion = "lima"
    '    econtacto._Telefono = "996688125"
    '    listaContacto.Add(econtacto)

    '    Dim obeProducto As New WcfLocal.beProducto

    '    obeProducto._IdPosicion = "10"
    '    obeProducto._IdProducto = "100000006"

    '    obeProducto._Descripcion = "TRAKKER 470"
    '    obeProducto._ValorUnitario = "150000"
    '    obeProducto._IdMonedaValorUnitario = "USD"
    '    obeProducto._Cantidad = "2"
    '    obeProducto._Unidad = "U"
    '    obeProducto._ValorNeto = "300000"
    '    obeProducto._IdMonedaValorNeto = "USD"
    '    obeProducto._NombreEstado = "Sin Cotización"
    '    obeProducto._TipoProducto = "PRIME"
    '    'eProducto._ProductoPrime 

    '    Dim obeproductoPrime As New WcfLocal.beProductoPrime

    '    obeproductoPrime._IdProducto = ""
    '    obeproductoPrime._FechaEstimCierre = "31012013"
    '    obeproductoPrime._PlazoEntregaEstim = "Quincena de Febrero 2013"
    '    obeproductoPrime._CodigoFormaPago = "02"
    '    obeproductoPrime._FormaPago = ""
    '    obeproductoPrime._FlatIncluyeRecompra = "01"
    '    obeproductoPrime._FlatIncluyeCLC = "01"
    '    obeproductoPrime._PromHorasMensualUso = ""
    '    obeproductoPrime._UsuarioCreacion = ""

    '    Dim listaDetalleRecompra As New List(Of WcfLocal.beDetalleRecompra)
    '    Dim obeDetalleRecompra As New WcfLocal.beDetalleRecompra

    '    obeDetalleRecompra._FlatEliminado = ""
    '    obeDetalleRecompra._IdDetalleRecompra = ""
    '    obeDetalleRecompra._IdProducto = ""
    '    obeDetalleRecompra._Moneda = "USD"
    '    obeDetalleRecompra._MontoRecompra = "3000.00"
    '    obeDetalleRecompra._Numero = "003"
    '    obeDetalleRecompra._NumeroHoras = "000000030"
    '    obeDetalleRecompra._NumeroMeses = "000000300"
    '    obeDetalleRecompra._UsuarioCreacion = ""

    '    Dim listaProductoAdicional As New List(Of WcfLocal.beProductoAdicional)
    '    Dim obeProductoAdicional As New WcfLocal.beProductoAdicional

    '    obeProductoAdicional._IdProductoAdicional = ""
    '    obeProductoAdicional._IdProducto = ""
    '    obeProductoAdicional._IdAdicional = "10000067"
    '    obeProductoAdicional._CodigoProductoAdicional = "10000067"
    '    obeProductoAdicional._NombreProdutoAdicional = "Llanta"
    '    obeProductoAdicional._Cantidad = "1"
    '    obeProductoAdicional._UnidadMedida = "U"
    '    obeProductoAdicional._ValorCosto = ""
    '    obeProductoAdicional._ValorLista = ""
    '    obeProductoAdicional._MonedaValorCosto = ""
    '    obeProductoAdicional._MonedaValorLista = ""
    '    obeProductoAdicional._TipoAdicional = "ACCESORIO"

    '    Dim obeProductoAdicional1 As New WcfLocal.beProductoAdicional
    '    obeProductoAdicional1._IdProductoAdicional = ""
    '    obeProductoAdicional1._IdProducto = ""
    '    obeProductoAdicional1._IdAdicional = "10000076"
    '    obeProductoAdicional1._CodigoProductoAdicional = "10000067"
    '    obeProductoAdicional1._NombreProdutoAdicional = "Plan Básico"
    '    obeProductoAdicional1._Cantidad = "1"
    '    obeProductoAdicional1._UnidadMedida = "U"
    '    obeProductoAdicional1._ValorCosto = ""
    '    obeProductoAdicional1._ValorLista = ""
    '    obeProductoAdicional1._MonedaValorCosto = ""
    '    obeProductoAdicional1._MonedaValorLista = ""
    '    obeProductoAdicional1._TipoAdicional = "PLAN MATENIMIENTO"

    '    listaProductoAdicional.Add(obeProductoAdicional)
    '    listaProductoAdicional.Add(obeProductoAdicional1)

    '    listaDetalleRecompra.Add(obeDetalleRecompra)
    '    obeproductoPrime._ListabeDetalleRecompra = listaDetalleRecompra.ToArray()
    '    obeproductoPrime._ListabeProductoAdicional = listaProductoAdicional.ToArray()

    '    obeProducto._ProductoPrime = obeproductoPrime
    '    Dim listaProducto As New List(Of WcfLocal.beProducto)

    '    listaProducto.Add(obeProducto)
    '    obeCotizacion._ListaProducto = listaProducto.ToArray()
    '    obeCotizacion._ListaCotizacionContacto = listaContacto.ToArray()

    '    NegWS.WS_InsertarCotizacion(obeCotizacion, UrlResult, ErrorDescripcion)

    'End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        'Dim beProductoPrime As New Ferreyros.ClasServicioCotizador.beProductoPrime
        'Dim daProductoPrime As New Ferreyros.ClasServicioCotizador.daProductoPrime
        'Dim Connection = New SqlConnection(Modulo.strConexionSql)
        'Connection.Open()
        'beProductoPrime.IdProducto = "239"
        'beProductoPrime.FechaEstimCierre = ""
        'beProductoPrime.PlazoEntregaEstim = "Quincena de Febrero 2013"
        'beProductoPrime.CodigoFormaPago = "02"
        'beProductoPrime.FormaPago = " Forma de pago"
        'beProductoPrime.FlatIncluyeRecompra = ""
        'beProductoPrime.FlatIncluyeCLC = "0"
        'beProductoPrime.PromHorasMensualUso = "18.125"

        'beProductoPrime.AnioFabricacion = "1"
        'beProductoPrime.Garantia = "2"
        'beProductoPrime.Condicion = "3"
        'beProductoPrime.PlazoEntrega = "4"
        'beProductoPrime.Orden = "5"
        'beProductoPrime.Serie = "6"
        'beProductoPrime.Horas = "7"
        'beProductoPrime.CodClasificacion = "8"
        'daProductoPrime.Insertar(Connection, beProductoPrime)

    End Sub

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim valor As String = String.Empty
        valor = Hosting.HostingEnvironment.ApplicationHost.ToString
        valor = Hosting.HostingEnvironment.ApplicationPhysicalPath.ToString
        valor = Hosting.HostingEnvironment.ApplicationVirtualPath.ToString
        valor = Hosting.HostingEnvironment.ApplicationID.ToString
        valor = Hosting.HostingEnvironment.VirtualPathProvider.ToString
        valor = Hosting.HostingEnvironment.SiteName.ToString
        valor = Hosting.HostingEnvironment.MapPath(Hosting.HostingEnvironment.ApplicationVirtualPath.ToString)
        valor = Environment.MachineName
        valor = Environment.CurrentDirectory
         
    End Sub
End Class