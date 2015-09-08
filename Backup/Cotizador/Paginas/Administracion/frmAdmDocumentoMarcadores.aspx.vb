Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.BCCotizador

Public Class frmAdmDocumentoMarcadores
    Inherits System.Web.UI.Page
    Private Shared eDocumentoMarcadores As beDocumentoMarcadores = Nothing
    Private Shared cDocumentoMarcadores As bcDocumentoMarcadores = Nothing
    Private Shared eValidacion As beValidacion = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function listarDocumentoMarcadores(ByVal sortColumn As String, ByVal sortOrder As String, ByVal pageSize As String, ByVal currentPage As String)

        cDocumentoMarcadores = New bcDocumentoMarcadores
        eDocumentoMarcadores = New beDocumentoMarcadores
        eValidacion = New beValidacion

        Dim l_documentoMarcadores As New List(Of beDocumentoMarcadores)
        Dim result = New EstructuraDatos.s_GridResult
        Dim rowsadded = New List(Of EstructuraDatos.s_RowData)
        Try
            cDocumentoMarcadores.DocumentoMarcadoresListar(Modulo.strConexionSql, eValidacion, l_documentoMarcadores)
            
            Dim fila As Integer = 1
            For Each drFila As beDocumentoMarcadores In l_documentoMarcadores
                Dim newrow = New EstructuraDatos.s_RowData
                newrow.id = fila
                ReDim newrow.cell(5)
                newrow.cell(0) = drFila.Codigo
                newrow.cell(1) = drFila.Descripcion
                newrow.cell(2) = drFila.IdCampo
                newrow.cell(3) = drFila.DescripcionCampo
                newrow.cell(4) = drFila.IdTipo
                newrow.cell(5) = drFila.DescripcionTipo
                fila = fila + 1
                rowsadded.Add(newrow)
            Next
            result.rows = rowsadded.ToArray
            result.page = Val(currentPage)
            result.total = l_documentoMarcadores.Count
            result.records = rowsadded.Count
        Catch ex As Exception

        End Try
        Return result
    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function llenarComboQuery(ByVal accion As String)

        Dim cQuery As New bcQuery
        Dim eQuery As New beQuery
        eValidacion = New beValidacion

        Dim l_Query As New List(Of beQuery)
        Dim lista As New List(Of EstructuraDatos.combo)

        Try
            cQuery.listarComboQuery(Modulo.strConexionSql, eValidacion, l_Query)
            For Each drFila As beQuery In l_Query
                Dim newrow = New EstructuraDatos.combo
                newrow.id = drFila.codigo
                newrow.Descripcion = drFila.descripcion
                lista.Add(newrow)
            Next
        Catch ex As Exception

        End Try
        
        Return lista
    End Function

    <System.Web.Services.WebMethod(enableSession:=True)> _
    Public Shared Function GuardarDocumentoMarcadores(ByVal accion As String, ByVal usuario As String, ByVal descripcion As String, _
                                         ByVal campo As String, ByVal idMarcador As String, ByVal idTipo As String) As beValidacion

        cDocumentoMarcadores = New bcDocumentoMarcadores
        eDocumentoMarcadores = New beDocumentoMarcadores
        eValidacion = New beValidacion

        usuario = AdminSeguridad.DesEncriptar(usuario)

        If accion = "MOD" Then
            eValidacion.flag = 2
            eDocumentoMarcadores.Codigo = idMarcador
        Else
            eValidacion.flag = 1
        End If
        eValidacion.usuario = usuario
        eDocumentoMarcadores.Descripcion = descripcion
        eDocumentoMarcadores.IdCampo = campo
        If idTipo = "Marcador" Then
            eDocumentoMarcadores.IdTipo = 1
        Else
            eDocumentoMarcadores.IdTipo = 0
        End If


        cDocumentoMarcadores.MantenimientoDocumentoMarcadores(strConexionSql, eDocumentoMarcadores, eValidacion)

        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)> _
    Public Shared Function EliminarMarcador(ByVal accion As String, ByVal id As String, ByVal usuario As String) As beValidacion

        cDocumentoMarcadores = New bcDocumentoMarcadores
        eDocumentoMarcadores = New beDocumentoMarcadores
        eValidacion = New beValidacion

        usuario = AdminSeguridad.DesEncriptar(usuario)
        eValidacion.flag = 3
        eValidacion.usuario = usuario
        eDocumentoMarcadores.Codigo = id

        cDocumentoMarcadores.MantenimientoDocumentoMarcadores(strConexionSql, eDocumentoMarcadores, eValidacion)

        Return eValidacion

    End Function
End Class