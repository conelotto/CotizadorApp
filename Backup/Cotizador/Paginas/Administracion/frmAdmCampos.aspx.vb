Imports System.Data.SqlClient

Public Class frmAdmCampos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function listarCampos(ByVal sortColumn As String, ByVal sortOrder As String, ByVal pageSize As String, ByVal currentPage As String)
        'sidx, sord, page, pagesize
        Dim conn = New SqlConnection(Modulo.strConexionSql)
        conn.Open()
        Dim strSelect As String
        strSelect = "select Codigo,Descripcion, Tabla, Campo  from dbo.Campos where Estado='1'"
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
            newrow.cell(3) = drFila.Item(3)
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
    Public Shared Function GuardarCampos(ByVal Tabla As String, ByVal Campo As String, ByVal Descripcion As String _
                                         , ByVal usuario As String, ByVal accion As String, ByVal codigo As String) As String
        Dim strValorReturn As String = String.Empty

        Try
            usuario = AdminSeguridad.DesEncriptar(usuario)
            Dim strCadena As String
            Using conn = New SqlConnection(Modulo.strConexionSql)
                If accion = "MOD" Then
                    strCadena = "UPDATE FSACotizador.dbo.Campos SET " &
                                "Tabla='" & Tabla & "',Campo='" & Campo & "',Descripcion='" & Descripcion & "',FechaModificacion=GETDATE(),UsuarioModificacion='" & usuario.ToString & "'" &
                                "where Codigo=" & codigo & " and estado='1'"
                Else
                    strCadena = "INSERT INTO FSACotizador.dbo.Campos(" &
                                "Tabla,Campo,Descripcion,Estado,FechaCreacion,UsuarioCreacion)" &
                                "VALUES('" & Tabla & "','" & Campo & "','" & Descripcion & "','1', GETDATE(),'" & usuario.ToString & "')"
                End If
                conn.Open()
                Dim cmd As New SqlCommand(strCadena, conn)
                cmd.ExecuteNonQuery()
                conn.Close()
            End Using
            strValorReturn = "1"
        Catch ex As Exception
            strValorReturn = "Error al insertar Campo."
        End Try

        Return strValorReturn
    End Function

    Private Shared Function validarExisteCampo(ByVal Campo As String) As Integer
        Dim retorno As Integer
        Try
            Using cn As New SqlConnection(Modulo.strConexionSql)
                Dim cmd As New SqlCommand
                cmd.CommandText = "select count(*) from dbo.Campos where upper(DescripcionLarga)='" & Campo.ToUpper & "' and estado='1'"
                cmd.CommandType = CommandType.Text
                cmd.Connection = cn

                cn.Open()
                retorno = CType(cmd.ExecuteScalar(), Integer)
                cn.Close()
            End Using
        Catch ex As Exception
            retorno = -1
        End Try
        Return retorno
    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function eliminarCampos(ByVal IdCampo As String, ByVal usuario As String)

        Dim strValorReturn As String = String.Empty
        Dim usuarioDeco As String = AdminSeguridad.DesEncriptar(usuario)
        Try
            Using conn = New SqlConnection(Modulo.strConexionSql)
                Dim strDelete As String = "UPDATE FSACotizador.dbo.Campos " &
                                          "SET Estado= '0'," &
                                          "UsuarioModificacion= '" & usuarioDeco & "'," &
                                          "FechaModificacion = GETDATE() " &
                                          "WHERE Codigo = " & IdCampo
                conn.Open()
                Dim cmd As New SqlCommand(strDelete, conn)
                cmd.ExecuteNonQuery()
                conn.Close()
                frmAdmConfiguracionCampoLLave.eliminarCampos(IdCampo, usuario, "CAMPOS")
            End Using
            strValorReturn = "1"
        Catch ex As Exception
            strValorReturn = "0"
        End Try

        Return strValorReturn

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function llenarComboTabla(ByVal nombreCombo As String)

        Dim lista As New List(Of combo)
        Using conn = New SqlConnection(Modulo.strConexionSql)
            conn.Open()
            Dim strSelect As String
         
            strSelect = "SELECT TABLE_NAME FROM FSACotizador.INFORMATION_SCHEMA.TABLES " &
                        "WHERE TABLE_SCHEMA = 'dbo' and TABLE_NAME not in ('sysdiagrams') group by TABLE_NAME order by 1"

            Dim cmd = New SqlCommand(strSelect, conn)
            Dim da = New SqlDataAdapter(cmd)
            Dim dt = New DataTable()

            Dim rows = da.Fill(dt)
            For Each drFila As DataRow In dt.Rows
                Dim newrow = New combo
                newrow.id = drFila.Item(0)
                newrow.Descripcion = drFila.Item(0)
                lista.Add(newrow)
            Next
            conn.Close()
        End Using
        Return lista
    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function llenarComboCampo(ByVal nombreTabla As String, ByVal accion As String)

        Dim lista As New List(Of combo)
        Using conn = New SqlConnection(Modulo.strConexionSql)
            conn.Open()
            Dim strSelect As String
            If accion = "MOD" Then
                strSelect = "SELECT COLUMN_NAME FROM FSACotizador.INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" & nombreTabla & "' order by 1"
            Else
                strSelect = "SELECT COLUMN_NAME FROM FSACotizador.INFORMATION_SCHEMA.COLUMNS where" &
                        " COLUMN_NAME not in (select Campo from Campos where estado='1' and Tabla='" & nombreTabla & "')" &
                        " and TABLE_NAME = '" & nombreTabla & "' order by 1"
            End If

            
            Dim cmd = New SqlCommand(strSelect, conn)
            Dim da = New SqlDataAdapter(cmd)
            Dim dt = New DataTable()

            Dim rows = da.Fill(dt)
            For Each drFila As DataRow In dt.Rows
                Dim newrow = New combo
                newrow.id = drFila.Item(0)
                newrow.Descripcion = drFila.Item(0)
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
        Dim id As String               ' ItemID value for the row
        Dim Descripcion As String        ' Array of strings that hold the field values for the given row
    End Structure

End Class