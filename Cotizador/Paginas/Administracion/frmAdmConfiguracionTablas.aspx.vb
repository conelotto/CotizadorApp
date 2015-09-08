Imports System.Data.SqlClient

Public Class frmAdmConfiguracionTablas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function listarCeldas(ByVal sortColumn As String, ByVal sortOrder As String, ByVal pageSize As String, _
                                        ByVal currentPage As String, ByVal IdTabla As String)
        Dim conn = New SqlConnection(Modulo.strConexionSql)
        conn.Open()
        Dim strSelect As String

        strSelect = "select ct.codigo as IdCelda,ct.codtabla as IdTabla, ct.Fila as Fila, ct.Columna as Columna, ct.Tipo,ct.valortipo as IdValor," &
                    "isnull((select distinct q.Descripcion+'.'+cr.Campos from Querys q , Querys_CamposResultado cr where cr.Codigo=ct.ValorTipo and " &
                    "cr.CodQuery = q.Codigo and q.estado='1'),'') as Valor from celdasTablas ct where ct.estado='1' and ct.codTabla=" & IdTabla

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
            ReDim newrow.cell(7)
            newrow.cell(0) = drFila.Item(0)
            newrow.cell(1) = drFila.Item(1)
            newrow.cell(2) = drFila.Item(2)
            newrow.cell(3) = drFila.Item(3)
            newrow.cell(4) = drFila.Item(4)
            newrow.cell(5) = drFila.Item(5)
            newrow.cell(6) = drFila.Item(6)
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
    Public Shared Function listarTablas(ByVal sortColumn As String, ByVal sortOrder As String, ByVal pageSize As String, ByVal currentPage As String)
        Dim conn = New SqlConnection(Modulo.strConexionSql)
        conn.Open()
        Dim strSelect As String
        strSelect = "select b.Codigo as IdTabla, b.Descripcion as Descripcion, " &
                    "CASE WHEN b.Repetir = 1 THEN 'SI' ELSE 'NO'	END AS Repartir " &
                    "from dbo.ConfiguracionTablas b where estado ='1'"

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
            ReDim newrow.cell(3)
            newrow.cell(0) = drFila.Item(0)
            newrow.cell(1) = drFila.Item(1)
            newrow.cell(2) = drFila.Item(2)
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
    Public Shared Function GuardarTablas(ByVal accion As String, ByVal usuario As String, ByVal descripcion As String, _
                                         ByVal repetir As String, ByVal idTabla As String) As String
        Dim strValorReturn As String = String.Empty
        Dim retorno As Integer
        Dim strCadena As String = ""
        Try
            usuario = AdminSeguridad.DesEncriptar(usuario)
            Using conn = New SqlConnection(Modulo.strConexionSql)
                If accion = "PIV" Then
                    strCadena = "INSERT INTO FSACotizador.dbo.ConfiguracionTablas(Descripcion,Repetir,Estado,UsuarioCreacion,FechaCreacion)" &
                                " VALUES('PIV' , 0 , '0','" & usuario & "', GETDATE() ) select SCOPE_IDENTITY()"
                Else
                    strCadena = "update dbo.ConfiguracionTablas set " &
                                "Descripcion='" & descripcion & "',Repetir=" & repetir & ",Estado='1' where Codigo=" & idTabla
                End If
                conn.Open()

                Dim cmd As New SqlCommand
                cmd.CommandText = strCadena
                cmd.CommandType = CommandType.Text
                cmd.Connection = conn
                retorno = CType(cmd.ExecuteScalar(), Integer)
                conn.Close()
            End Using
            strValorReturn = retorno
        Catch ex As Exception
            strValorReturn = "0"
        End Try

        Return strValorReturn
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function GuardarCeldas(ByVal IdTabla As String, ByVal Fila As String, ByVal Columna As String, _
                                         ByVal Tipo As String, ByVal ValorTipo As String, ByVal usuario As String) As String
        Dim strValorReturn As String = String.Empty
        Try
            usuario = AdminSeguridad.DesEncriptar(usuario)
            Using conn = New SqlConnection(Modulo.strConexionSql)
                Dim strInsert As String = "INSERT INTO FSACotizador.dbo.CeldasTablas" &
                                          "(CodTabla,Fila,Columna,Tipo,ValorTipo,UsuarioCreacion,FechaCreacion,Estado)" &
                                          "VALUES(" & IdTabla & " , " & Fila & " , " & Columna & ",'" & Tipo &
                                          "'," & ValorTipo & ",'" & usuario & "', GETDATE(), '1' )"
                conn.Open()

                Dim cmd As New SqlCommand(strInsert, conn)
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
    Public Shared Function EliminarTablas(ByVal accion As String, ByVal Id As String, ByVal usuario As String)

        Dim strValorReturn As String = String.Empty

        Try
            Dim strDelete As String = ""
            Using conn = New SqlConnection(Modulo.strConexionSql)
                If accion = "PIV" Then
                    strDelete = "DELETE FSACotizador.dbo.ConfiguracionTablas WHERE Codigo=" & Id
                Else
                    EliminarCeldas(accion, Id, "TODAS", usuario)
                    usuario = AdminSeguridad.DesEncriptar(usuario)
                    strDelete = "update dbo.ConfiguracionTablas set Estado='0', UsuarioModificacion='" & usuario &
                                "', FechaModificacion=GETDATE() where Codigo=" & Id
                End If
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
    Public Shared Function EliminarCeldas(ByVal accion As String, ByVal IdTabla As String, ByVal IdCelda As String, ByVal usuario As String)

        Dim strValorReturn As String = String.Empty

        Try
            Dim strDelete As String = ""
            Using conn = New SqlConnection(Modulo.strConexionSql)
                If accion = "PIV" Then
                    strDelete = "delete from dbo.CeldasTablas where codtabla=" & IdTabla
                Else
                    usuario = AdminSeguridad.DesEncriptar(usuario)
                    If IdCelda = "TODAS" Then
                        strDelete = "update dbo.CeldasTablas set Estado='0', UsuarioModificacion='" & usuario &
                                    "', FechaModificacion=GETDATE() where CodTabla =" & IdTabla
                    Else
                        strDelete = "update dbo.CeldasTablas set Estado='0', UsuarioModificacion='" & usuario &
                                    "', FechaModificacion=GETDATE() where Codigo =" & IdCelda & " And CodTabla =" & IdTabla
                    End If
                End If
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
    Public Shared Function llenarComboQuery(ByVal accion As String)

        Dim lista As New List(Of combo)
        Using conn = New SqlConnection(Modulo.strConexionSql)
            conn.Open()
            Dim strSelect As String
            If accion = "MOD" Then
                strSelect = "select cr.Codigo as id, Descripcion+'.'+ cr.Campos as Descripcion from dbo.Querys q, " &
                            "dbo.Querys_CamposResultado cr where q.Codigo=cr.CodQuery and q.Estado='1' order by 1"
            Else
                strSelect = "select cr.Codigo as id, Descripcion+'.'+ cr.Campos as Descripcion from dbo.Querys q, " &
                            "dbo.Querys_CamposResultado cr where q.Codigo=cr.CodQuery and q.Estado='1' order by 1"
            End If

            Dim cmd = New SqlCommand(strSelect, conn)
            Dim da = New SqlDataAdapter(cmd)
            Dim dt = New DataTable()

            Dim rows = da.Fill(dt)
            For Each drFila As DataRow In dt.Rows
                Dim newrow = New combo
                newrow.id = drFila.Item(0)
                newrow.Descripcion = drFila.Item(1)
                lista.Add(newrow)
            Next
            conn.Close()
        End Using
        Return lista
    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function llenarComboCampo(ByVal accion As String)

        Dim lista As New List(Of combo)
        Using conn = New SqlConnection(Modulo.strConexionSql)
            conn.Open()
            Dim strSelect As String
            If accion = "MOD" Then
                strSelect = "select Codigo, Tabla+'.'+Campo as Campo from dbo.Campos where Estado ='1'"
            Else
                strSelect = "select Codigo, Tabla+'.'+Campo as Campo from dbo.Campos where Estado ='1' " 'and Codigo not IN (select CodCampo from ConfiguracionCampoLLave where estado='1')"
            End If

            Dim cmd = New SqlCommand(strSelect, conn)
            Dim da = New SqlDataAdapter(cmd)
            Dim dt = New DataTable()

            Dim rows = da.Fill(dt)
            For Each drFila As DataRow In dt.Rows
                Dim newrow = New combo
                newrow.id = drFila.Item(0)
                newrow.Descripcion = drFila.Item(1)
                lista.Add(newrow)
            Next
            conn.Close()
        End Using
        Return lista
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