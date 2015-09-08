Imports Ferreyros.BCCotizador
Imports Ferreyros.BECotizador
Imports System.Xml
Imports System.IO
Imports System.Web.Script.Serialization
Imports System.Runtime.Serialization.Json
Imports log4net
Imports System.Data.SqlClient

Public Class frmAdmConfiguracionSolucionCombinada
    Inherits System.Web.UI.Page
    Private Shared eConfiguracionSolucionCombinada As beConfiguracionSolucionCombinada = Nothing
    Private Shared cConfiguracionSolucionCombinada As bcConfiguracionSolucionCombinada = Nothing
    Private Shared eValidacion As beValidacion = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function listarConfiguracionCombinada(ByVal sortColumn As String, ByVal sortOrder As String, ByVal pageSize As String, ByVal currentPage As String)
        'sidx, sord, page, pagesize
        Dim conn = New SqlConnection(Modulo.strConexionSql)
        conn.Open()
        Dim strSelect As String
        strSelect = "select co.codlinea as IdLineas, li.DescripcionLarga as Lineas, " &
                    "dbo.funcion_concatenar_codigo_llaves(co.codlinea) as IdLLaves, " &
                    "dbo.funcion_concatenar_llaves(co.codlinea) as LLaves, " &
                    "dbo.funcion_concatenar_orden(co.codlinea) as Orden " &
                    "from dbo.ConfiguracionSolComb co, dbo.llaves ll, dbo.lineas li " &
                    "where ll.codigo=co.codllave and li.codigo=co.codlinea and co.estado='1' " &
                    "group by co.codlinea,li.DescripcionLarga, co.estado"
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
            ReDim newrow.cell(4)
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


    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function eliminarCombinacion(ByVal IdLinea As String, ByVal usuario As String)
        ', ByVal mantenimiento As String

        Dim strValorReturn As String = String.Empty
        usuario = AdminSeguridad.DesEncriptar(usuario)
        Dim strDelete As String
        Try
            Using conn = New SqlConnection(Modulo.strConexionSql)
                strDelete = "UPDATE FSACotizador.dbo.ConfiguracionSolComb " &
                                    "SET Estado= '0'," &
                                    "UsuarioModificacion= '" & usuario & "'," &
                                    "FechaModificacion = GETDATE() " &
                                    "WHERE CodLinea = " & IdLinea
                conn.Open()
                Dim cmd As New SqlCommand(strDelete, conn)
                cmd.ExecuteNonQuery()
                conn.Close()
            End Using
            strValorReturn = "1"
        Catch ex As Exception
            strValorReturn = "0"
        End Try

        Return strValorReturn

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function llenarCombo(ByVal nombreCombo As String)

        Dim lista As New List(Of comboLinea)
        Using conn = New SqlConnection(Modulo.strConexionSql)
            conn.Open()
            Dim strSelect As String
            If nombreCombo = "MOD" Then
                strSelect = "SELECT Codigo,DescripcionLarga FROM dbo.Lineas where estado='1' "
            Else
                strSelect = "select codigo, descripcionLarga from dbo.Lineas where estado='1' " &
                            "and Codigo not in (select codlinea from dbo.ConfiguracionSolComb where estado='1' group by CodLinea)"
            End If
            Dim cmd = New SqlCommand(strSelect, conn)
            Dim da = New SqlDataAdapter(cmd)
            Dim dt = New DataTable()

            Dim rows = da.Fill(dt)
            For Each drFila As DataRow In dt.Rows
                Dim newrow = New comboLinea
                newrow.id = drFila.Item(0)
                newrow.Linea = drFila.Item(1)
                lista.Add(newrow)
            Next
            conn.Close()
        End Using
        Return lista
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function listarLLaves(ByVal sortColumn As String, ByVal sortOrder As String, ByVal pageSize As String, _
                                        ByVal currentPage As String, ByVal accion As String, ByVal idLLaves As String, _
                                        ByVal Orden As String)
        Dim conn = New SqlConnection(Modulo.strConexionSql)
        conn.Open()
        Dim strSelect As String = " select codigo, descripcionLarga from dbo.llaves where estado='1' "

        Dim cmd = New SqlCommand(strSelect, conn)
        Dim da = New SqlDataAdapter(cmd)
        Dim dt = New DataTable()

        Dim rows = da.Fill(dt)
        Dim result = New s_GridResult
        Dim rowsadded = New List(Of s_RowData)
        Dim fila As Integer = 1
        For Each drFila As DataRow In dt.Rows
            Dim newrow = New s_RowData
            newrow.id = fila
            ReDim newrow.cell(3)
            newrow.cell(0) = drFila.Item(0)
            newrow.cell(1) = drFila.Item(1)
            If accion = "MOD" Then
                Dim llaves() As String = Split(idLLaves, ",")
                Dim arrOrden() As String = Split(Orden, ",")
                newrow.cell(2) = "False"
                newrow.cell(3) = ""
                For i As Integer = 0 To llaves.Length - 1
                    If llaves(i) = drFila.Item(0) Then
                        newrow.cell(2) = "True"
                        newrow.cell(3) = arrOrden(i)
                    End If
                Next
            Else
                newrow.cell(2) = "False"
                newrow.cell(3) = ""
            End If
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

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function GuardarCombinacionLLaves(ByVal IdLinea As String, ByVal IdLLaves As String, ByVal Dependencias As String, ByVal usuario As String) As beValidacion
        Dim strValorReturn As String = String.Empty
        cConfiguracionSolucionCombinada = New bcConfiguracionSolucionCombinada
        eConfiguracionSolucionCombinada = New beConfiguracionSolucionCombinada
        eValidacion = New beValidacion

            IdLLaves = IdLLaves.Substring(1, IdLLaves.Length - 1)
            Dependencias = Dependencias.Substring(1, Dependencias.Length - 1)

            eValidacion.flag = 1
            eValidacion.usuario = AdminSeguridad.DesEncriptar(usuario)

            Dim LLaves As String() = Split(IdLLaves, ",")
            Dim Dep As String() = Split(Dependencias, ",")
            eliminarCombinacion(IdLinea, usuario)

            For i As Integer = 0 To LLaves.Length - 1
                eConfiguracionSolucionCombinada.CodLinea = IdLinea
                eConfiguracionSolucionCombinada.CodLLave = Val(LLaves(i))
                eConfiguracionSolucionCombinada.LLaveDependencia = Val(Dep(i))

                cConfiguracionSolucionCombinada.MantenimientoConfigSolComb(strConexionSql, eConfiguracionSolucionCombinada, eValidacion)
            Next

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

    Public Structure comboLinea
        Dim id As Integer               ' ItemID value for the row
        Dim Linea As String        ' Array of strings that hold the field values for the given row
    End Structure
End Class