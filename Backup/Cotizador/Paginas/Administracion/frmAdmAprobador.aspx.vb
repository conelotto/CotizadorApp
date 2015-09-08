Imports Ferreyros.BECotizador
Imports Ferreyros.BCCotizador
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros
Imports System.Web.Script.Serialization
Imports log4net
Public Class frmAdmAprobador
    Inherits System.Web.UI.Page

#Region "----------  Declaracion  ----------"

    Private miContexto As Base.Interfase.MiUsuario
    Private Shared eAprobador As beAprobador = Nothing
    Private Shared oAprobador As bcAprobador = Nothing
    Private Shared eValidacion As beValidacion = Nothing
    Private Shared l_Aprobador As List(Of beAprobador) = Nothing
    Private Shared eAprobadorUsuario As beAprobadorUsuario = Nothing
    Private Shared oAprobadorUsuario As bcAprobadorUsuario = Nothing
    Private Shared l_AprobadorUsuario As List(Of beAprobadorUsuario) = Nothing
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(frmAdmAprobador))
    Private Shared Property s_Aprobador() As Object
        Get
            Return HttpContext.Current.Session("AdmAprobador_Consulta")
        End Get
        Set(ByVal value As Object)
            HttpContext.Current.Session("AdmAprobador_Consulta") = value
        End Set
    End Property
    Private Shared Property s_Reporte_Nombre() As String
        Get
            Return HttpContext.Current.Session("Reporte_Nombre")
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session("Reporte_Nombre") = value
        End Set
    End Property
    Private Shared Property s_Reporte_Data() As Object
        Get
            Return HttpContext.Current.Session("Reporte_Data")
        End Get
        Set(ByVal value As Object)
            HttpContext.Current.Session("Reporte_Data") = value
        End Set
    End Property
    Private Shared Property s_Corporacion() As Object
        Get
            Return HttpContext.Current.Session("AdmAprobador_Corporacion")
        End Get
        Set(ByVal value As Object)
            HttpContext.Current.Session("AdmAprobador_Corporacion") = value
        End Set
    End Property
    Private Shared Property s_Companhia() As Object
        Get
            Return HttpContext.Current.Session("AdmAprobador_Companhia")
        End Get
        Set(ByVal value As Object)
            HttpContext.Current.Session("AdmAprobador_Companhia") = value
        End Set
    End Property

#End Region

#Region "----------    Eventos    ----------"

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
            ConsultarCombos()
        End If

    End Sub

#End Region

#Region "----------   WebMethod   ----------"

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ListarCombos() As ArrayList

        Dim l_Corporacion As New List(Of beTablaMaestra)
        Dim l_Companhia As New List(Of beTablaMaestra)
        Dim lResult As New ArrayList

        If s_Corporacion IsNot Nothing Then
            l_Corporacion = CType(s_Corporacion, List(Of beTablaMaestra))
        End If
        If s_Companhia IsNot Nothing Then
            l_Companhia = CType(s_Companhia, List(Of beTablaMaestra))
        End If

        lResult.Add(l_Corporacion)
        lResult.Add(l_Companhia)

        Return lResult

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function Consultar(ByVal corporacion As String, ByVal companhia As String, ByVal aprobador As String) As List(Of beAprobador)

        eValidacion = New beValidacion
        eAprobador = New beAprobador
        oAprobador = New bcAprobador
        l_Aprobador = New List(Of beAprobador)

        eAprobador.IdCorporacion = corporacion
        eAprobador.IdCompañia = companhia
        eAprobador.Aprobador = Trim(aprobador)
        eAprobador.Estado = Estado.Habilitado
        eValidacion.cadenaAleatoria = strCadenaAleatoria
        eValidacion.cadenaLog4Net = String.Concat("IdCorporacion=", eAprobador.IdCorporacion, _
                                                  ",IdCompañia=", eAprobador.IdCompañia, _
                                                  ",Aprobador=", eAprobador.Aprobador, _
                                                  ",Estado=", eAprobador.Estado)
        log.Info(eValidacion.cadenaAleatoria + ": " + eValidacion.cadenaLog4Net)
        oAprobador.ListarAprobador(strConexionSql, eAprobador, eValidacion, l_Aprobador)

        s_Aprobador = l_Aprobador

        Return l_Aprobador

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function ConsultarAprobadorUsuario(ByVal idAprobador As String) As List(Of beAprobadorUsuario)

        Dim lResult As New List(Of beAprobadorUsuario)
        If String.IsNullOrEmpty(idAprobador) Then
            Return lResult
        End If

        eValidacion = New beValidacion
        eAprobadorUsuario = New beAprobadorUsuario
        oAprobadorUsuario = New bcAprobadorUsuario
        Dim dstAprobadorUsuario As New DataSet

        eAprobadorUsuario.IdAprobador = idAprobador
        eAprobadorUsuario.Estado = Estado.Habilitado
        eValidacion.cadenaAleatoria = strCadenaAleatoria
        eValidacion.cadenaLog4Net = String.Concat("IdAprobador=", eAprobadorUsuario.IdAprobador, _
                                                  ",Estado=", eAprobadorUsuario.Estado)
        log.Info(eValidacion.cadenaAleatoria + ": " + eValidacion.cadenaLog4Net)
        If oAprobadorUsuario.Buscar(strConexionSql, eAprobadorUsuario, dstAprobadorUsuario) Then
            Dim dt As DataTable = dstAprobadorUsuario.Tables(0)
            For Each Row In dt.Rows
                eAprobadorUsuario = New beAprobadorUsuario
                eAprobadorUsuario.MatriculaUsuario = IIf(IsDBNull(Row("MatriculaUsuario")), String.Empty, Row("MatriculaUsuario"))
                eAprobadorUsuario.NombreUsuario = IIf(IsDBNull(Row("NombreUsuario")), String.Empty, Row("NombreUsuario"))
                eAprobadorUsuario.CorreoUsuario = IIf(IsDBNull(Row("CorreoUsuario")), String.Empty, Row("CorreoUsuario"))
                lResult.Add(eAprobadorUsuario)
            Next
        Else
            log.Error(eValidacion.cadenaAleatoria + ": " + oAprobadorUsuario.ErrorDes)
        End If

        Return lResult

    End Function

    <System.Web.Services.WebMethodAttribute(enableSession:=True), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function EliminarAprobador(ByVal idAprobador As String, ByVal usuario As String) As beValidacion

        eValidacion = New beValidacion
        eAprobador = New beAprobador
        oAprobador = New bcAprobador

        eAprobador.IdAprobador = idAprobador
        eAprobador.UsuarioModificacion = usuario
        eValidacion.cadenaAleatoria = strCadenaAleatoria
        eValidacion.cadenaLog4Net = String.Concat("IdAprobador=", eAprobador.IdAprobador, _
                                                  ",UsuarioModificacion=", eAprobador.UsuarioModificacion)

        If Not oAprobador.Anular(strConexionSql, eAprobador) Then
            eValidacion.mensaje = oAprobador.ErrorDes
            log.Error(eValidacion.cadenaAleatoria + ": " + oAprobadorUsuario.ErrorDes)
        Else
            eValidacion.validacion = True
        End If

        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function MostrarUsuario(ByVal companhia As String, ByVal prefijoUsuario As String) As List(Of beAprobadorUsuario)

        Dim lResult As New List(Of beAprobadorUsuario)
        Dim Rpt As beAprobadorUsuario = Nothing
        Dim oUsuario As New bcUsuario
        Dim dstUsuario As New DataSet

        If oUsuario.Buscar(strConexionAdryan, IIf(companhia.Length > 2, Right(companhia, 2), companhia), prefijoUsuario.ToUpper.Trim, dstUsuario) Then
            For Each dtrFila As DataRow In dstUsuario.Tables(0).Rows
                Rpt = New beAprobadorUsuario
                Rpt.MatriculaUsuario = dtrFila("MatriculaUsuario").ToString
                Rpt.NombreUsuario = dtrFila("NombreUsuario").ToString
                lResult.Add(Rpt)
            Next
        Else
            log.Error(strCadenaAleatoria + ": " + oUsuario.ErrorDes)
        End If

        Return lResult

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function AgregarUsuario(ByVal matriculaUsuario As String, ByVal nombreUsuario As String) As beAprobadorUsuario

        'eValidacion = New beValidacion
        eAprobadorUsuario = New beAprobadorUsuario
        oAprobadorUsuario = New bcAprobadorUsuario
        Dim oUsuario As bcUsuario = New bcUsuario
        Dim dtrUsuario As DataRow = Nothing
      
        eAprobadorUsuario.MatriculaUsuario = matriculaUsuario
        eAprobadorUsuario.Estado = Estado.Habilitado
        eAprobadorUsuario.NombreUsuario = nombreUsuario

        'Verifica si el usuario ha sido asignado a otro aprobador
        If oAprobadorUsuario.ExisteUsuarioEnOtroAprobador(strConexionSql, oAprobadorUsuario, eAprobadorUsuario) Then
            log.Error(strCadenaAleatoria + ": " + oAprobadorUsuario.ErrorDes)
            Throw New Exception(oAprobadorUsuario.ErrorDes)
        Else
            'Recupera todos los datos del usuario a ingresar
            If Not oUsuario.Buscar(strConexionAdryan, eAprobadorUsuario, dtrUsuario) Then
                log.Error(strCadenaAleatoria + ": " + oUsuario.ErrorDes)
                Throw New Exception(oUsuario.ErrorDes)
            Else
                eAprobadorUsuario.MatriculaUsuario = dtrUsuario.Item("MatriculaUsuario")
                eAprobadorUsuario.NombreUsuario = dtrUsuario.Item("NombreUsuario")
                eAprobadorUsuario.CorreoUsuario = dtrUsuario.Item("CorreoUsuario")
            End If
        End If

        Return eAprobadorUsuario

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function Grabar(ByVal tipo As String, ByVal idAprobador As String, _
                                  ByVal corporacion As String, ByVal companhia As String, _
                                  ByVal aprobador As String, ByVal usuario As String, _
                                  ByVal lsUsuario As String) As beValidacion

        Dim ls_Usuario As New List(Of beAprobadorUsuario)
        convertirCadenaSerializable(lsUsuario, ls_Usuario)

        eAprobador = New beAprobador
        oAprobador = New bcAprobador
        eValidacion = New beValidacion

        eValidacion.flag = If(tipo = "N", 1, 2)
        eAprobador.IdAprobador = If(IsNumeric(idAprobador), idAprobador, 0)
        eAprobador.Aprobador = Trim(aprobador)
        eAprobador.IdCorporacion = corporacion
        eAprobador.IdCompañia = companhia
        eValidacion.usuario = usuario

        eValidacion.cadenaAleatoria = strCadenaAleatoria
        eValidacion.cadenaLog4Net = String.Concat("flag=", eValidacion.flag, _
                                                  ",IdAprobador=", eAprobador.IdAprobador, _
                                                  ",Aprobador=", eAprobador.Aprobador, _
                                                  ",IdCorporacion=", eAprobador.IdCorporacion, _
                                                  ",IdCompañia=", eAprobador.IdCompañia, _
                                                  ",usuario=", eValidacion.usuario)

        log.Info(eValidacion.cadenaAleatoria + ": " + eValidacion.cadenaLog4Net)
        oAprobador.MantenimientoAprobador(strConexionSql, eAprobador, ls_Usuario, eValidacion)

        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function Exportar() As Boolean

        Dim lResult As String = String.Empty

        lResult = fc_cadenaExportar()

        If String.IsNullOrEmpty(lResult) Then
            Return False
        End If

        s_Reporte_Nombre = "Aprobador"
        s_Reporte_Data = lResult

        Return True

    End Function

#End Region

#Region "----------    Metodos    ----------"

    Private Shared Sub convertirCadenaSerializable(ByVal cadena As String, ByRef lResult As Object)

        Dim ls_Usuario As New List(Of beAprobadorUsuario)
        Dim lineas As String()
        lineas = cadena.Split("|")
        For Each Rpt In lineas
            Dim subLineas As String()
            subLineas = Rpt.Split(";")
            lResult = New beAprobadorUsuario
            lResult.MatriculaUsuario = subLineas(0)
            lResult.NombreUsuario = subLineas(1)
            lResult.CorreoUsuario = subLineas(2)
            ls_Usuario.Add(lResult)
        Next

        lResult = ls_Usuario

    End Sub

#End Region

#Region "----------   Funciones   ----------"

    Private Shared Function fc_cadenaExportar() As String

        eValidacion = New beValidacion
        eAprobador = New beAprobador
        oAprobador = New bcAprobador
        l_Aprobador = New List(Of beAprobador)

        Dim lResult As New StringBuilder

        l_Aprobador = CType(s_Aprobador, List(Of beAprobador))

        If Not (l_Aprobador IsNot Nothing AndAlso l_Aprobador.Count > 0) Then
            GoTo Salir
        End If

        lResult.Append("<TABLE>")
        lResult.Append("<TR>")
        lResult.Append("<TD colspan='4' style='font-family: Verdana,Arial, Helvetica, sans-serif;font-size: 22px;font-weight: normal;color:#00086E;vertical-align: middle;text-align: left;text-decoration:none' >")
        lResult.Append("LISTADO DE APROBADORES")
        lResult.Append("</TD>")
        lResult.Append("</TR>")
        lResult.Append("<TR>")
        lResult.Append("</TD>")
        lResult.Append("</TD>")
        lResult.Append("</TR>")
        lResult.Append("</TABLE>")
        REM ----------------------
        lResult.Append("<TABLE border='1px'>")
        lResult.Append("<TR>")
        lResult.Append("<TD style='width: 150px;font-family: Verdana,Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #E8F8FF;vertical-align: middle;text-align: center;background-color: #00086E;text-decoration:none'>Aprobador</TD>")
        lResult.Append("<TD style='width: 1000px;font-family: Verdana,Arial, Helvetica, sans-serif;font-size: 10px;font-weight: normal;color: #E8F8FF;vertical-align: middle;text-align: center;background-color: #00086E;text-decoration:none'>Usuario</TD>")
        lResult.Append("</TR>")

        For Each Rpt In l_Aprobador
            lResult.Append("<tr>")
            lResult.Append("<td style='vertical-align: top; mso-number-format: \@' align='left'>" & Rpt.Aprobador & "</td>")
            lResult.Append("<td style='vertical-align: top; mso-number-format: \@' align='left'>")
            l_AprobadorUsuario = New List(Of beAprobadorUsuario)
            l_AprobadorUsuario = ConsultarAprobadorUsuario(Rpt.IdAprobador)
            If l_AprobadorUsuario.Count > 0 Then
                lResult.Append("<TABLE border='1px'>")
                For Each det In l_AprobadorUsuario
                    lResult.Append("<TR>")
                    lResult.Append("<td style='width: 200px;mso-number-format: \@' align='left'>")
                    lResult.Append(det.MatriculaUsuario)
                    lResult.Append("</td>")
                    lResult.Append("<td style='width: 400px;mso-number-format: \@' align='left'>")
                    lResult.Append(det.NombreUsuario)
                    lResult.Append("</td>")
                    lResult.Append("<td style='width: 400px;mso-number-format: \@' align='left'>")
                    lResult.Append(det.CorreoUsuario)
                    lResult.Append("</td>")
                    lResult.Append("</tr>")
                Next
                lResult.Append("</TABLE>")
            End If
            lResult.Append("</td>")
            lResult.Append("</tr>")
        Next
        lResult.Append("</TABLE>")

Salir:
        Return lResult.ToString

    End Function

    Private Sub ConsultarCombos()

        Dim eTablaMaestra As beTablaMaestra = Nothing
        Dim l_Corporacion As New List(Of beTablaMaestra)
        Dim l_Companhia As New List(Of beTablaMaestra)
        Dim oDbs As New bcDbs
        Dim EntidadesMaestrasDbs As New List(Of String)
        EntidadesMaestrasDbs.Add(EntidadMaestraDbs.Corporacion)
        EntidadesMaestrasDbs.Add(EntidadMaestraDbs.Compañia)
        Dim dstDbs As New DataSet

        If oDbs.BuscarEntidadMaestraDbs(strConexion, strLibreria, EntidadesMaestrasDbs, dstDbs) Then
            For Indice As Int32 = 0 To dstDbs.Tables.Count - 1
                If Not dstCache.Tables.Contains(dstDbs.Tables(Indice).TableName) Then
                    dstCache.Tables.Add(dstDbs.Tables(Indice).Copy)
                End If
            Next
        End If

        eTablaMaestra = New beTablaMaestra
        l_Corporacion.Add(eTablaMaestra)
        For Each Row In dstDbs.Tables(0).Rows
            eTablaMaestra = New beTablaMaestra
            eTablaMaestra.IdSeccion = Row("CODIGO")
            eTablaMaestra.Descripcion = Row("DESCRIPE")
            l_Corporacion.Add(eTablaMaestra)
        Next

        s_Corporacion = l_Corporacion

        eTablaMaestra = New beTablaMaestra
        l_Companhia.Add(eTablaMaestra)
        For Each Row In dstDbs.Tables(1).Rows
            eTablaMaestra = New beTablaMaestra
            eTablaMaestra.IdSeccion = Row("CODIGO")
            eTablaMaestra.Descripcion = Row("DESCRIPE")
            l_Companhia.Add(eTablaMaestra)
        Next

        s_Companhia = l_Companhia

    End Sub

#End Region

End Class