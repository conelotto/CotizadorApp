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

Public Class frmAdmDetallePartes
    Inherits System.Web.UI.Page

#Region "--- Declaraciones ---"

    Private miContexto As Base.Interfase.MiUsuario
    Private Shared eDetallePartes As beTarifa = Nothing
    Private Shared oTarifa As bcTarifa = Nothing
    Private Shared l_DetallePartes As List(Of beTarifa) = Nothing
    Private Shared l_Response As JQGridJsonResponse = Nothing
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(frmAdmDetallePartes))
    Private Shared configPlanes As String = ConfigurationManager.AppSettings("Planes").ToString
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

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function Consultar(ByVal prefijo As String, ByVal modelo As String, _
                                     ByVal modeloBase As String, ByVal familia As String, _
                                     ByVal serviceCategory As String, ByVal Rodetail As String, _
                                     ByVal compQty As String, ByVal firstInterval As String, _
                                     ByVal nextInterval As String, ByVal jodetail As String, _
                                     ByVal SOSPartNumber As String, ByVal SOSDescription As String, _
                                     ByVal quantity As String, ByVal replacement As String, _
                                     ByVal unitPrice As String, ByVal extendedPrice As String, _
                                     ByVal sellEvent As String, ByVal eventos As String, _
                                     ByVal sell As String, ByVal sortColumn As String, _
                                     ByVal sortOrder As String) As JQGridJsonResponse

        oTarifa = New bcTarifa
        eDetallePartes = New beTarifa
        eValidacion = New beValidacion

        l_DetallePartes = New List(Of beTarifa)

        eDetallePartes.prefijo = Trim(prefijo)
        eDetallePartes.modelo = Trim(modelo)
        eDetallePartes.modeloBase = Trim(modeloBase)
        eDetallePartes.familia = Trim(familia)
        eDetallePartes.serviceCategory = Trim(serviceCategory)
        eDetallePartes.rodetail = Trim(Rodetail)
        eDetallePartes.compQty = IIf(String.IsNullOrEmpty(compQty), 0, compQty)
        eDetallePartes.firstInterval = IIf(String.IsNullOrEmpty(firstInterval), 0, firstInterval)
        eDetallePartes.nextInterval = IIf(String.IsNullOrEmpty(nextInterval), 0, nextInterval)
        eDetallePartes.jodetail = Trim(jodetail)
        eDetallePartes.SOSPartNumber = Trim(SOSPartNumber)
        eDetallePartes.SOSDescription = Trim(SOSDescription)
        eDetallePartes.quantity = IIf(String.IsNullOrEmpty(quantity), 0, quantity)
        eDetallePartes.replacement = IIf(String.IsNullOrEmpty(replacement), 0, replacement)
        eDetallePartes.unitPrice = IIf(String.IsNullOrEmpty(unitPrice), 0, unitPrice)
        eDetallePartes.extendedPrice = IIf(String.IsNullOrEmpty(extendedPrice), 0, extendedPrice)
        eDetallePartes.sellEvent = IIf(String.IsNullOrEmpty(sellEvent), 0, sellEvent)
        eDetallePartes.evento = IIf(String.IsNullOrEmpty(eventos), 0, eventos)
        eDetallePartes.sell = IIf(String.IsNullOrEmpty(sell), 0, sell)
        eValidacion.sortColumn = sortColumn
        eValidacion.sortOrder = sortOrder

        eValidacion.cadenaAleatoria = strCadenaAleatoria
        eValidacion.cadenaLog4Net = String.Concat("prefijo=", eDetallePartes.prefijo, _
                                                  ",modelo=", eDetallePartes.modelo, _
                                                  ",modeloBase=", eDetallePartes.modeloBase, _
                                                  ",familia=", eDetallePartes.familia, _
                                                  ",serviceCategory=", eDetallePartes.serviceCategory, _
                                                  ",rodetail=", eDetallePartes.rodetail, _
                                                  ",compQty=", eDetallePartes.compQty, _
                                                  ",firstInterval=", eDetallePartes.firstInterval, _
                                                  ",nextInterval=", eDetallePartes.nextInterval, _
                                                  ",jodetail=", eDetallePartes.jodetail, _
                                                  ",SOSPartNumber=", eDetallePartes.SOSPartNumber, _
                                                  ",SOSDescription=", eDetallePartes.SOSDescription, _
                                                  ",quantity=", eDetallePartes.quantity, _
                                                  ",replacement=", eDetallePartes.replacement, _
                                                  ",unitPrice=", eDetallePartes.unitPrice, _
                                                  ",extendedPrice=", eDetallePartes.extendedPrice, _
                                                  ",sellEvent=", eDetallePartes.sellEvent, _
                                                  ",evento=", eDetallePartes.evento, _
                                                  ",sell=", eDetallePartes.sell)

        log.Info(strCadenaAleatoria + ": " + eValidacion.cadenaLog4Net)

        oTarifa.ListarDetallePartes(strConexionSql, eDetallePartes, eValidacion, l_DetallePartes)

        l_Response = New JQGridJsonResponse(1, 1, 1, "D", l_DetallePartes)

        Return l_Response

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function Grabar(ByVal tipo As String, ByVal id As String, _
                                  ByVal prefijo As String, ByVal familia As String, _
                                  ByVal modelo As String, ByVal modeloBase As String, _
                                  ByVal serviceCategory As String, ByVal Rodetail As String, _
                                  ByVal compQty As String, ByVal firstInterval As String, _
                                  ByVal nextInterval As String, ByVal jodetail As String, _
                                  ByVal SOSPartNumber As String, ByVal SOSDescription As String, _
                                  ByVal quantity As String, ByVal replacement As String, _
                                  ByVal unitPrice As String, ByVal extendedPrice As String, _
                                  ByVal sellEvent As String, ByVal eventos As String, _
                                  ByVal sell As String, ByVal usuario As String) As beValidacion

        oTarifa = New bcTarifa
        eDetallePartes = New beTarifa
        eValidacion = New beValidacion

        eValidacion.flag = If(tipo = "N", 1, 2)
        eValidacion.usuario = usuario

        eDetallePartes.id = id
        eDetallePartes.prefijo = UCase(prefijo)
        eDetallePartes.modelo = UCase(modelo)
        eDetallePartes.modeloBase = UCase(modeloBase)
        eDetallePartes.familia = UCase(familia)
        eDetallePartes.serviceCategory = Trim(serviceCategory)
        eDetallePartes.rodetail = Trim(Rodetail)
        eDetallePartes.compQty = Trim(compQty)
        eDetallePartes.firstInterval = Trim(firstInterval)
        eDetallePartes.nextInterval = Trim(nextInterval)
        eDetallePartes.jodetail = Trim(jodetail)
        eDetallePartes.SOSPartNumber = Trim(SOSPartNumber)
        eDetallePartes.SOSDescription = Trim(SOSDescription)
        eDetallePartes.quantity = Trim(quantity)
        eDetallePartes.replacement = Trim(replacement)
        eDetallePartes.unitPrice = Trim(unitPrice)
        eDetallePartes.extendedPrice = Trim(extendedPrice)
        eDetallePartes.sellEvent = Trim(sellEvent)
        eDetallePartes.evento = Trim(eventos)
        eDetallePartes.sell = Trim(sell)

        eValidacion.cadenaAleatoria = strCadenaAleatoria
        eValidacion.cadenaLog4Net = String.Concat("flag=", eValidacion.flag, _
                                                  ",usuario=", eValidacion.usuario, _
                                                  ",id=", eDetallePartes.id, _
                                                  ",prefijo=", eDetallePartes.prefijo, _
                                                  ",modelo=", eDetallePartes.modelo, _
                                                  ",modeloBase=", eDetallePartes.modeloBase, _
                                                  ",familia=", eDetallePartes.familia, _
                                                  ",serviceCategory=", eDetallePartes.serviceCategory, _
                                                  ",rodetail=", eDetallePartes.rodetail, _
                                                  ",compQty=", eDetallePartes.compQty, _
                                                  ",firstInterval=", eDetallePartes.firstInterval, _
                                                  ",nextInterval=", eDetallePartes.nextInterval, _
                                                  ",jodetail=", eDetallePartes.jodetail, _
                                                  ",SOSPartNumber=", eDetallePartes.SOSPartNumber, _
                                                  ",SOSDescription=", eDetallePartes.SOSDescription, _
                                                  ",quantity=", eDetallePartes.quantity, _
                                                  ",replacement=", eDetallePartes.replacement, _
                                                  ",unitPrice=", eDetallePartes.unitPrice, _
                                                  ",extendedPrice=", eDetallePartes.extendedPrice, _
                                                  ",sellEvent=", eDetallePartes.sellEvent, _
                                                  ",evento=", eDetallePartes.evento, _
                                                  ",sell=", eDetallePartes.sell)

        log.Info(strCadenaAleatoria + ": " + eValidacion.cadenaLog4Net)
        oTarifa.MantenimientoDetallePartes(strConexionSql, eDetallePartes, eValidacion)

        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function Eliminar(ByVal id As String, ByVal usuario As String) As beValidacion

        oTarifa = New bcTarifa
        eDetallePartes = New beTarifa
        eValidacion = New beValidacion

        eValidacion.flag = 3
        eValidacion.usuario = usuario
        eDetallePartes.id = id

        eValidacion.cadenaAleatoria = strCadenaAleatoria
        eValidacion.cadenaLog4Net = String.Concat("flag=", eValidacion.flag, _
                                                  ",usuario=", eValidacion.usuario, _
                                                  ",id=", eDetallePartes.id)

        log.Info(strCadenaAleatoria + ": " + eValidacion.cadenaLog4Net)

        oTarifa.MantenimientoTarifas(strConexionSql, eDetallePartes, eValidacion)

        Return eValidacion

    End Function

#End Region


End Class