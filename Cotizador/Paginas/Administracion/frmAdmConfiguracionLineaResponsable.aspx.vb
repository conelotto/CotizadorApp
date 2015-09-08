Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.BCCotizador

Public Class frmAdmConfiguracionLineaResponsable
    Inherits System.Web.UI.Page
    Private Shared eLineaResponsable As beLineaResponsable = Nothing
    Private Shared cLineaResponsable As bcLineaResponsable = Nothing
    Private Shared eValidacion As beValidacion = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function listarResponsableLinea(ByVal sortColumn As String, ByVal sortOrder As String, ByVal pageSize As String, ByVal currentPage As String)

        cLineaResponsable = New bcLineaResponsable
        eLineaResponsable = New beLineaResponsable
        eValidacion = New beValidacion
        Dim l_lineaResponsable As New List(Of beLineaResponsable)
        Dim result = New EstructuraDatos.s_GridResult
        Dim rowsadded = New List(Of EstructuraDatos.s_RowData)
        Try
            cLineaResponsable.LineaResponsableListar(Modulo.strConexionSql, eValidacion, l_lineaResponsable)

            Dim fila As Integer = 1
            For Each drFila As beLineaResponsable In l_lineaResponsable
                Dim newrow = New EstructuraDatos.s_RowData
                newrow.id = fila
                ReDim newrow.cell(5)
                newrow.cell(0) = drFila.Codigo
                newrow.cell(1) = drFila.Usuario
                newrow.cell(2) = drFila.Linea
                newrow.cell(3) = drFila.CodigoLinea
                newrow.cell(4) = drFila.DescripcionLinea
                fila = fila + 1
                rowsadded.Add(newrow)
            Next
            result.rows = rowsadded.ToArray
            result.page = Val(currentPage)
            result.total = l_lineaResponsable.Count
            result.records = rowsadded.Count
        Catch ex As Exception

        End Try
        Return result
    End Function
    
    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function llenarComboLineas(ByVal accion As String, ByVal usuario As String)

        Dim lista As New List(Of EstructuraDatos.combo)
        Using conn = New SqlConnection(Modulo.strConexionSql)
            conn.Open()
            Dim strSelect As String
            If accion = "MOD" Then
                strSelect = "SELECT Codigo,DescripcionLarga FROM dbo.Lineas where estado='1' "
            Else
                strSelect = "SELECT Codigo,DescripcionLarga FROM dbo.Lineas where estado='1' "
            End If

            Dim cmd = New SqlCommand(strSelect, conn)
            Dim da = New SqlDataAdapter(cmd)
            Dim dt = New DataTable()

            Dim rows = da.Fill(dt)
            For Each drFila As DataRow In dt.Rows
                Dim newrow = New EstructuraDatos.combo
                newrow.id = drFila.Item(0)
                newrow.Descripcion = drFila.Item(1)
                lista.Add(newrow)
            Next
            conn.Close()
        End Using
        Return lista
    End Function

    <System.Web.Services.WebMethod(enableSession:=True)> _
    Public Shared Function GuardarResponsableLinea(ByVal accion As String, ByVal usuario As String, ByVal idLinea As String, _
                                                   ByVal idResponsableLinea As String) As beValidacion

        cLineaResponsable = New bcLineaResponsable
        eLineaResponsable = New beLineaResponsable
        eValidacion = New beValidacion

        If accion = "MOD" Then
            eValidacion.flag = 6
            eLineaResponsable.Codigo = idResponsableLinea
        Else
            eValidacion.flag = 5
        End If
        eLineaResponsable.Usuario = usuario.ToUpper
        eLineaResponsable.Linea = idLinea
        cLineaResponsable.LineaResponsableMant(strConexionSql, eLineaResponsable, eValidacion)

        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)> _
    Public Shared Function EliminarResponsableLinea(ByVal idResponsableLinea As String) As beValidacion

        cLineaResponsable = New bcLineaResponsable
        eLineaResponsable = New beLineaResponsable
        eValidacion = New beValidacion

        eValidacion.flag = 7
        eLineaResponsable.Codigo = idResponsableLinea

        cLineaResponsable.LineaResponsableMant(strConexionSql, eLineaResponsable, eValidacion)

        Return eValidacion

    End Function
End Class