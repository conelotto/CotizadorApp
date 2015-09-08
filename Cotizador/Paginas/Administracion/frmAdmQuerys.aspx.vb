Imports System.Data.SqlClient
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

Public Class frmAdmQuerys
    Inherits System.Web.UI.Page
    Private miContexto As Base.Interfase.MiUsuario
    Private Shared eQuery As beQuery = Nothing
    Private Shared oQuery As bcQuery = Nothing
    Private Shared l_Response As JQGridJsonResponse = Nothing
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(frmAdmDetallePartes))
    Private Shared configPlanes As String = ConfigurationManager.AppSettings("Planes").ToString
    Private Shared eValidacion As beValidacion = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function listarQuerys(ByVal sortColumn As String, ByVal sortOrder As String, ByVal pageSize As String, ByVal currentPage As String)
        Dim conn = New SqlConnection(Modulo.strConexionSql)
        conn.Open()
        Dim strSelect As String
        strSelect = "select distinct q.Codigo, q.Descripcion, q.Query , (dbo.funcion_concatenar_QCResultado(q.Codigo))," &
                    " (dbo.funcion_concatenar_QCondicionales(q.Codigo)) from dbo.Querys q, dbo.Querys_CamposResultado qcr," &
                    "dbo.Querys_Condicionales qc where q.Codigo = qcr.CodQuery and q.Codigo = qc.CodQuery and q.Estado = '1' order by 1 desc"

        Dim cmd = New SqlCommand(strSelect, conn)
        Dim da = New SqlDataAdapter(cmd)
        Dim dt = New DataTable()

        Dim startIndex As Integer = (Val(currentPage) - 1) * Val(pageSize)
        Dim endIndex As Integer = Val(currentPage) * Val(pageSize)

        Dim rows = da.Fill(dt)
        Dim result = New s_GridResult
        Dim rowsadded = New List(Of s_RowData)
        Dim fila As Integer = 1
        For Each drFila As DataRow In dt.Rows
            Dim newrow = New s_RowData
            newrow.id = fila
            ReDim newrow.cell(5)
            newrow.cell(0) = drFila.Item(0)
            newrow.cell(1) = drFila.Item(1)
            newrow.cell(2) = drFila.Item(2)
            newrow.cell(3) = drFila.Item(3)
            newrow.cell(4) = drFila.Item(4)
            fila = fila + 1
            rowsadded.Add(newrow)
        Next
        result.rows = rowsadded.ToArray
        result.page = Val(currentPage)
        result.total = dt.Rows.Count
        result.records = rowsadded.Count

        conn.Close()

        Return result
    End Function

    <System.Web.Services.WebMethod(enableSession:=True)> _
    Public Shared Function GuardarQuerys(ByVal accion As String, ByVal usuario As String, ByVal descripcion As String, _
                                         ByVal query As String, ByVal where As String, ByVal result As String, _
                                         ByVal codigo As String) As beValidacion

        oQuery = New bcQuery
        eQuery = New beQuery
        eValidacion = New beValidacion

        usuario = AdminSeguridad.DesEncriptar(usuario)

        If accion = "MOD" Then
            eValidacion.flag = 2
            eQuery.codigo = codigo
        Else
            eValidacion.flag = 1
        End If

        eValidacion.usuario = usuario

        eQuery.descripcion = descripcion
        eQuery.query = query
        eQuery.cadenaCampos = result
        eQuery.cadenaCondiciones = where

        eValidacion.cadenaAleatoria = strCadenaAleatoria
        eValidacion.cadenaLog4Net = String.Concat("flag=", eValidacion.flag, _
                                                 ",usuario=", eValidacion.usuario)

        log.Info(strCadenaAleatoria + ": " + eValidacion.cadenaLog4Net)
        oQuery.MantenimientoQuerys(strConexionSql, eQuery, eValidacion)

        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)> _
    Public Shared Function EliminarQuerys(ByVal idQuery As String, ByVal usuario As String) As beValidacion

        oQuery = New bcQuery
        eQuery = New beQuery
        eValidacion = New beValidacion

        usuario = AdminSeguridad.DesEncriptar(usuario)

        eValidacion.flag = 3
        eValidacion.usuario = usuario

        eQuery.codigo = idQuery

        eValidacion.cadenaAleatoria = strCadenaAleatoria
        eValidacion.cadenaLog4Net = String.Concat("flag=", eValidacion.flag, _
                                                  ",usuario=", eValidacion.usuario)

        log.Info(strCadenaAleatoria + ": " + eValidacion.cadenaLog4Net)
        oQuery.MantenimientoQuerys(strConexionSql, eQuery, eValidacion)

        Return eValidacion

    End Function

    Public Structure s_GridResult

        Dim page As Integer           ' Current page of data that is being viewed/requested
        Dim total As Integer          ' Total number of pages avaialble to view in the entire list
        Dim records As Integer         ' Number of records available in the rows[] array
        Dim rows() As s_RowData     ' Rows of data
    End Structure


    Public Structure s_RowData

        Dim id As Integer               ' ItemID value for the row
        Dim cell() As String        ' Array of strings that hold the field values for the given row
    End Structure

    Public Structure combo
        Dim id As Integer               ' ItemID value for the row
        Dim Descripcion As String        ' Array of strings that hold the field values for the given row
    End Structure

End Class