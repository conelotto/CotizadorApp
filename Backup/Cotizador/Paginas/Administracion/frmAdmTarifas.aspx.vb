Imports Ferreyros.BECotizador
Imports Ferreyros.BCCotizador
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros
Imports System.Web.Script.Serialization
Imports System
Imports System.IO
Imports Excel
Imports System.Configuration
Imports log4net

Public Class frmAdmTarifas
    Inherits System.Web.UI.Page

#Region "--- Declaraciones ---"

    Private miContexto As Base.Interfase.MiUsuario
    Private Shared eTarifa As beTarifa = Nothing
    Private Shared oTarifa As bcTarifa = Nothing
    Private Shared l_Tarifa As List(Of beTarifa) = Nothing
    Private Shared l_Response As JQGridJsonResponse = Nothing
    Private Shared configPlanes As String = ConfigurationManager.AppSettings("Planes").ToString
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(frmAdmTarifas))
    Private Shared eValidacion As beValidacion = Nothing
    Private Structure oFluidos
        Event z()
        Public Const Con As String = "Con Fluidos"
        Public Const Sin As String = "Sin Fluidos"
    End Structure

#End Region

#Region "--- Eventos ---"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If sender Is Nothing OrElse e Is Nothing Then
            Return
        End If

        Dim AccesoValido As Boolean
        If Session("MiUsuario") IsNot Nothing Then
            miContexto = CType(Cache(Session("MiUsuario")), Base.Interfase.MiUsuario)
            AccesoValido = miContexto IsNot Nothing
        End If

        If Not AccesoValido Then
            Response.Redirect("~/Default.aspx", True)
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If sender Is Nothing OrElse e Is Nothing Then
            Return
        End If

        If Not IsPostBack Then
            Session("idUsuario") = CType(Cache(Session("MiUsuario")), Base.Interfase.MiUsuario).login
        End If

    End Sub

#End Region

#Region "--- Metodos ---"

    Private Shared Sub ExtraerPlan(ByRef Cadena As String, ByRef Lista As List(Of String))

        Dim Posicion = InStr(Cadena, ";")
        If Posicion <> 0 AndAlso Cadena.Length > 0 Then
            Dim Valor = Mid(Cadena, 1, Posicion - 1)
            Cadena = Mid(Cadena, Posicion + 1)
            Lista.Add(Valor)
        ElseIf Cadena.Length > 0 Then
            Lista.Add(Cadena)
            Cadena = String.Empty
        End If

    End Sub

#End Region

#Region "--- Funciones ---"

    Private Shared Function fc_ExtraerValor(ByVal Tipo As Int16, ByVal Cadena As String) As String

        Dim lResult As String = String.Empty
        If String.IsNullOrEmpty(Cadena) Then
            Return lResult
        End If

        Dim Posicion = InStr(Cadena, ":")

        If Tipo = 1 Then
            lResult = Mid(Cadena, 1, Posicion - 1)
        Else
            lResult = Mid(Cadena, Posicion + 1)
        End If

        Return lResult

    End Function

#End Region

#Region "--- WebMethod ---"

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function Importar(ByVal ruta As String) As Boolean

        'Dim fileName As String = HttpContext.Current.Server.HtmlEncode(ruta)
        'Dim extension As String = System.IO.Path.GetExtension(fileName)

        ''Contenido = FileUpload1.FileContent



        'Dim strdocPath As String
        'strdocPath = "C:\Users\outsourcingavanceste\Desktop\Cronograma semanal.xlsx"

        'Dim objfilestream As New FileStream(strdocPath, FileMode.Open, FileAccess.Read)
        'Dim len As Integer = objfilestream.Length
        'Dim arr(len) As Byte

        'Dim x As Integer = objfilestream.Read(arr, 0, len)
        'objfilestream.Close()



        Return True

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)> _
    Public Shared Function ConsultarPlanes() As List(Of beTarifa)

        Dim c_Planes As New List(Of String)
        Dim Rpt As beTarifa = Nothing

        l_Tarifa = New List(Of beTarifa)

        Do
            ExtraerPlan(configPlanes, c_Planes)
        Loop While configPlanes.Length > 0

        Rpt = New beTarifa
        l_Tarifa.Add(Rpt)

        For Each cad In c_Planes
            Rpt = New beTarifa
            Rpt.id = fc_ExtraerValor(1, cad)
            Rpt.plan = fc_ExtraerValor(2, cad)
            l_Tarifa.Add(Rpt)
        Next

        Return l_Tarifa

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)> _
    Public Shared Function ConsultarEventos() As List(Of beTarifa)

        Dim Rpt As beTarifa = Nothing
        l_Tarifa = New List(Of beTarifa)

        Rpt = New beTarifa
        l_Tarifa.Add(Rpt)

        Rpt = New beTarifa
        Rpt.id = "PM 1"
        Rpt.evento = "PM 1"
        l_Tarifa.Add(Rpt)

        Rpt = New beTarifa
        Rpt.id = "PM 2"
        Rpt.evento = "PM 2"
        l_Tarifa.Add(Rpt)

        Rpt = New beTarifa
        Rpt.id = "PM 3"
        Rpt.evento = "PM 3"
        l_Tarifa.Add(Rpt)

        Rpt = New beTarifa
        Rpt.id = "PM 4"
        Rpt.evento = "PM 4"
        l_Tarifa.Add(Rpt)

        Rpt = New beTarifa
        Rpt.id = "PM 1F"
        Rpt.evento = "PM 1F"
        l_Tarifa.Add(Rpt)

        Rpt = New beTarifa
        Rpt.id = "PM 2F"
        Rpt.evento = "PM 2F"
        l_Tarifa.Add(Rpt)

        Return l_Tarifa

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)> _
    Public Shared Function Consultar(ByVal prefijo As String, ByVal modelo As String, _
                                     ByVal modeloBase As String, ByVal familia As String, _
                                     ByVal plan As String, ByVal evento As String, _
                                     ByVal kitRepuestos As String, ByVal fluidos As String, _
                                     ByVal servicioContratado As String, ByVal SOS As String, _
                                     ByVal total As String, ByVal eventosNueva As String, _
                                     ByVal eventosUsada As String, ByVal kitRepuestosT As String, _
                                     ByVal fluidosT As String, ByVal servicioContratadoT As String, _
                                     ByVal totalT As String, ByVal tarifaUSDxH As String, _
                                     ByVal sortColumn As String, ByVal sortOrder As String) As JQGridJsonResponse

        oTarifa = New bcTarifa
        eTarifa = New beTarifa
        eValidacion = New beValidacion

        l_Tarifa = New List(Of beTarifa)

        eTarifa.prefijo = Trim(prefijo)
        eTarifa.modelo = Trim(modelo)
        eTarifa.modeloBase = Trim(modeloBase)
        eTarifa.familia = Trim(familia)
        eTarifa.plan = Trim(plan)
        eTarifa.evento = Trim(evento)
        eTarifa.kitRepuestos = IIf(String.IsNullOrEmpty(kitRepuestos), 0, kitRepuestos)
        eTarifa.fluidos = IIf(String.IsNullOrEmpty(fluidos), 0, fluidos)
        eTarifa.servicioContratado = IIf(String.IsNullOrEmpty(servicioContratado), 0, servicioContratado)
        eTarifa.SOS = IIf(String.IsNullOrEmpty(SOS), 0, SOS)
        eTarifa.total = IIf(String.IsNullOrEmpty(total), 0, total)
        eTarifa.eventosNueva = IIf(String.IsNullOrEmpty(eventosNueva), 0, eventosNueva)
        eTarifa.eventosUsada = IIf(String.IsNullOrEmpty(eventosUsada), 0, eventosUsada)
        eTarifa.kitRepuestosT = IIf(String.IsNullOrEmpty(kitRepuestosT), 0, kitRepuestosT)
        eTarifa.fluidosT = IIf(String.IsNullOrEmpty(fluidosT), 0, fluidosT)
        eTarifa.servicioContratadoT = IIf(String.IsNullOrEmpty(servicioContratadoT), 0, servicioContratadoT)
        eTarifa.totalT = IIf(String.IsNullOrEmpty(totalT), 0, totalT)
        eTarifa.tarifaUSDxH = IIf(String.IsNullOrEmpty(tarifaUSDxH), 0, tarifaUSDxH)
        eValidacion.sortColumn = sortColumn
        eValidacion.sortOrder = sortOrder

        eValidacion.cadenaAleatoria = strCadenaAleatoria
        eValidacion.cadenaLog4Net = String.Concat("prefijo=", eTarifa.prefijo, _
                                                  ",modelo=", eTarifa.modelo, _
                                                  ",modeloBase=", eTarifa.modeloBase, _
                                                  ",familia=", eTarifa.familia, _
                                                  ",plan=", eTarifa.plan, _
                                                  ",evento=", eTarifa.evento, _
                                                  ",kitRepuestos=", eTarifa.kitRepuestos, _
                                                  ",fluidos=", eTarifa.fluidos, _
                                                  ",servicioContratado=", eTarifa.servicioContratado, _
                                                  ",SOS=", eTarifa.SOS, _
                                                  ",eventosNueva=", eTarifa.eventosNueva, _
                                                  ",eventosUsada=", eTarifa.eventosUsada, _
                                                  ",kitRepuestosT=", eTarifa.kitRepuestosT, _
                                                  ",fluidosT=", eTarifa.fluidosT, _
                                                  ",servicioContratadoT=", eTarifa.servicioContratadoT, _
                                                  ",totalT=", eTarifa.totalT, _
                                                  ",tarifaUSDxH=", eTarifa.tarifaUSDxH)

        log.Info(strCadenaAleatoria + ": " + eValidacion.cadenaLog4Net)
        oTarifa.ListarTarifas(strConexionSql, eTarifa, eValidacion, l_Tarifa)

        l_Response = New JQGridJsonResponse(1, 1, 1, "T", l_Tarifa)

        Return l_Response

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)> _
    Public Shared Function Grabar(ByVal tipo As String, ByVal id As String, _
                                  ByVal prefijo As String, ByVal familia As String, _
                                  ByVal modelo As String, ByVal modeloBase As String, _
                                  ByVal codigoPlan As String, ByVal plan As String, _
                                  ByVal evento As String, ByVal kitRepuestos As String, _
                                  ByVal conFluidos As String, ByVal fluidos As String, _
                                  ByVal servicioContratado As String, ByVal SOS As String, _
                                  ByVal eventosNueva As String, ByVal eventosUsada As String, _
                                  ByVal tarifaUSDxH As String, ByVal usuario As String) As beValidacion

        oTarifa = New bcTarifa
        eTarifa = New beTarifa
        eValidacion = New beValidacion

        eValidacion.flag = If(tipo = "N", 1, 2)
        eValidacion.usuario = usuario

        eTarifa.id = id
        eTarifa.prefijo = UCase(prefijo)
        eTarifa.modelo = UCase(modelo)
        eTarifa.modeloBase = UCase(modeloBase)
        eTarifa.familia = UCase(familia)
        eTarifa.codigoPlan = codigoPlan
        eTarifa.plan = UCase(plan)
        eTarifa.evento = UCase(evento)
        eTarifa.kitRepuestos = kitRepuestos
        If conFluidos.Equals("SI") Then
            eTarifa.conFluidos = oFluidos.Con
            eTarifa.aceites = oFluidos.Con
            eTarifa.fluidos = fluidos
        Else
            eTarifa.conFluidos = oFluidos.Sin
            eTarifa.aceites = oFluidos.Sin
            eTarifa.fluidos = "0"
            eTarifa.fluidosT = "0"
        End If
        eTarifa.servicioContratado = servicioContratado
        eTarifa.SOS = SOS
        eTarifa.eventosNueva = eventosNueva
        eTarifa.eventosUsada = eventosUsada
        eTarifa.tarifaUSDxH = tarifaUSDxH

        eValidacion.cadenaAleatoria = strCadenaAleatoria
        eValidacion.cadenaLog4Net = String.Concat("flag=", eValidacion.flag, _
                                                  ",usuario=", eValidacion.usuario, _
                                                  ",id=", eTarifa.id, _
                                                  ",prefijo=", eTarifa.prefijo, _
                                                  ",modelo=", eTarifa.modelo, _
                                                  ",modeloBase=", eTarifa.modeloBase, _
                                                  ",familia=", eTarifa.familia, _
                                                  ",codigoPlan=", eTarifa.codigoPlan, _
                                                  ",plan=", eTarifa.plan, _
                                                  ",evento=", eTarifa.evento, _
                                                  ",kitRepuestos=", eTarifa.kitRepuestos, _
                                                  ",conFluidos=", eTarifa.conFluidos, _
                                                  ",aceites=", eTarifa.aceites, _
                                                  ",fluidos=", eTarifa.fluidos, _
                                                  ",fluidosT=", eTarifa.fluidosT, _
                                                  ",servicioContratado=", eTarifa.servicioContratado, _
                                                  ",SOS=", eTarifa.SOS, _
                                                  ",eventosNueva=", eTarifa.eventosNueva, _
                                                  ",eventosUsada=", eTarifa.eventosUsada, _
                                                  ",tarifaUSDxH=", eTarifa.tarifaUSDxH)

        log.Info(strCadenaAleatoria + ": " + eValidacion.cadenaLog4Net)
        oTarifa.MantenimientoTarifas(strConexionSql, eTarifa, eValidacion)

        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)> _
    Public Shared Function Eliminar(ByVal id As String, _
                                    ByVal usuario As String) As beValidacion

        oTarifa = New bcTarifa
        eTarifa = New beTarifa
        eValidacion = New beValidacion

        eValidacion.flag = 3
        eValidacion.usuario = usuario
        eTarifa.id = id

        eValidacion.cadenaAleatoria = strCadenaAleatoria
        eValidacion.cadenaLog4Net = String.Concat("flag=", eValidacion.flag, _
                                                  ",usuario=", eValidacion.usuario, _
                                                  ",id=", eTarifa.id)

        log.Info(strCadenaAleatoria + ": " + eValidacion.cadenaLog4Net)
        oTarifa.MantenimientoTarifas(strConexionSql, eTarifa, eValidacion)

        Return eValidacion

    End Function

#End Region


End Class