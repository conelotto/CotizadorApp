Imports System.Data.SqlClient

Public Class frmAdmLLaves
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function listarLLaves(ByVal sortColumn As String, ByVal sortOrder As String, ByVal pageSize As String, ByVal currentPage As String)
        'sidx, sord, page, pagesize
        Dim conn = New SqlConnection(Modulo.strConexionSql)
        conn.Open()
        Dim strSelect As String
        strSelect = "select Codigo, DescripcionLarga from dbo.LLaves where estado='1'"
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
    Public Shared Function GuardarLLaves(ByVal LLave As String, ByVal usuario As String) As String
        Dim strValorReturn As String = String.Empty

        Try
            Dim existe As Integer = validarExisteLLave(LLave)
            If existe = 0 Then
                usuario = AdminSeguridad.DesEncriptar(usuario)

                Using conn = New SqlConnection(Modulo.strConexionSql)
                    Dim strInsert As String = "INSERT INTO FSACotizador.dbo.LLaves(" &
                                              "DescripcionCorta,DescripcionLarga,Estado,FechaCreacion,UsuarioCreacion)" &
                                              "VALUES('" & LLave & "','" & LLave & "','1', GETDATE(),'" & usuario.ToString & "')"
                    conn.Open()
                    Dim cmd As New SqlCommand(strInsert, conn)
                    cmd.ExecuteNonQuery()
                    conn.Close()
                End Using
                strValorReturn = "1"
            ElseIf existe > 0 Then
                strValorReturn = "Esta LLave ya existe, ingrese otra."
            Else
                strValorReturn = "Error al validar LLave."
            End If

        Catch ex As Exception
            strValorReturn = "Error al insertar LLave."
        End Try

        Return strValorReturn
    End Function

    Private Shared Function validarExisteLLave(ByVal LLave As String) As Integer
        Dim retorno As Integer
        Try
            Using cn As New SqlConnection(Modulo.strConexionSql)
                Dim cmd As New SqlCommand
                cmd.CommandText = "select count(*) from dbo.LLaves where upper(DescripcionLarga)='" & LLave.ToUpper & "' and estado='1'"
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
    Public Shared Function eliminarLLaves(ByVal IdLLave As String, ByVal usuario As String)

        Dim strValorReturn As String = String.Empty
        Dim usuarioDeco As String = AdminSeguridad.DesEncriptar(usuario)
        Try
            Using conn = New SqlConnection(Modulo.strConexionSql)
                Dim strDelete As String = "UPDATE FSACotizador.dbo.LLaves " &
                                          "SET Estado= '0'," &
                                          "UsuarioModificacion= '" & usuarioDeco & "'," &
                                          "FechaModificacion = GETDATE() " &
                                          "WHERE Codigo = " & IdLLave
                conn.Open()
                Dim cmd As New SqlCommand(strDelete, conn)
                cmd.ExecuteNonQuery()
                conn.Close()
                frmAdmConfiguracionCampoLLave.eliminarCampos(IdLLave, usuario, "LLAVES")
            End Using
            strValorReturn = "1"
        Catch ex As Exception
            strValorReturn = "0"
        End Try

        Return strValorReturn

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

End Class