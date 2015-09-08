Imports System.Data.OleDb
Imports Ferreyros.Utiles.Estructuras
Imports System.Text

Public Class daConsultaAS400

    Private Connection As OleDbConnection = Nothing
    Private Command As OleDbCommand = Nothing
    Private lResult As String = String.Empty

    Public Function ConsultarCompanhia(ByVal eConexion As String, ByVal Valor As String) As String

        Dim Consulta As String = "SELECT CODIGO, DESCRIPN FROM LPDBS.UFPSC160 A WHERE TABLA = 'CIA' AND TELEX='" + Valor + "'"

        Connection = New OleDbConnection(eConexion)
        Connection.Open()
        Command = Connection.CreateCommand

        Command.CommandText = Consulta
        Command.CommandType = CommandType.Text
        Command.Prepare()

        lResult = Command.ExecuteScalar()

        Return lResult

    End Function

    Public Function ConsultarCorporacion(ByVal eConexion As String, ByVal Valor As String) As String

        Dim Consulta As String = "SELECT CODIGO, DESCRIPN FROM LPDBS.UFPSC160 A WHERE TABLA = 'COR' AND TELEX= '" + Valor + "'"

        Connection = New OleDbConnection(eConexion)
        Connection.Open()
        Command = Connection.CreateCommand

        Command.CommandText = Consulta
        Command.CommandType = CommandType.Text
        Command.Prepare()

        lResult = Command.ExecuteScalar()

        Return lResult

    End Function

    Public Function ConsultarOficina(ByVal eConexion As String, ByVal Valor As String) As String

        Dim Consulta As String = "SELECT CODIGO, DESCRIPN FROM LPDBS.UFPSC160 A WHERE TABLA = 'SUC' AND TELEX='" + Valor + "'"

        Connection = New OleDbConnection(eConexion)
        Connection.Open()
        Command = Connection.CreateCommand

        Command.CommandText = Consulta
        Command.CommandType = CommandType.Text
        Command.Prepare()

        lResult = Command.ExecuteScalar()

        Return lResult

    End Function

    ''' <summary>
    ''' TABLA DE CORPORACION
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ListarCorporacion(ByVal eConexion As String) As DataSet

        Dim Dt As New DataTable
        Connection = New OleDbConnection(eConexion)
        Connection.Open()
        Dim dtsRetorno = New DataSet
        Dim Consulta As String = ""
        Dim command = Connection.CreateCommand()
        Consulta = " SELECT CODIGO, DESCRIPN,TELEX FROM LPDBS.UFPSC160 where tabla = 'COR' AND TIPREG <> '*'"
        command.CommandType = CommandType.Text
        command.CommandText = Consulta

        Try

            Using dr As OleDbDataReader = command.ExecuteReader()

                Dt = Estructura(dr.GetSchemaTable)

                If dr.HasRows Then

                    While dr.Read

                        Dim drItem As DataRow = Dt.NewRow

                        For i As Integer = 0 To dr.FieldCount - 1

                            If dr.GetValue(i).ToString.Trim <> "" Then

                                drItem(i) = dr.GetValue(i).ToString.Trim

                            End If

                        Next

                        Dt.Rows.Add(drItem)

                    End While

                End If

            End Using

            dtsRetorno.Tables.Add(Dt)
            dtsRetorno.Tables(0).TableName = "Corporacion"
        Catch ex As Exception

            Throw

        Finally

            command.Dispose()

        End Try

        Return dtsRetorno

    End Function

    Public Function ListarCompania(ByVal eConexion As String) As DataSet

        Dim Dt As New DataTable
        Connection = New OleDbConnection(eConexion)
        Connection.Open()
        Dim dtsRetorno = New DataSet

        Dim Consulta As String = ""
        Dim command = Connection.CreateCommand()
        Consulta = " SELECT CODIGO, DESCRIPN,TELEX FROM LPDBS.UFPSC160 A where tabla = 'CIA' AND TIPREG <> '*'"
        command.CommandType = CommandType.Text

        command.CommandText = Consulta


        Try
            Using dr As OleDbDataReader = command.ExecuteReader()

                Dt = Estructura(dr.GetSchemaTable)

                If dr.HasRows Then

                    While dr.Read

                        Dim drItem As DataRow = Dt.NewRow

                        For i As Integer = 0 To dr.FieldCount - 1

                            If dr.GetValue(i).ToString.Trim <> "" Then

                                drItem(i) = dr.GetValue(i).ToString.Trim

                            End If

                        Next

                        Dt.Rows.Add(drItem)

                    End While

                End If

            End Using

            dtsRetorno.Tables.Add(Dt)
            dtsRetorno.Tables(0).TableName = "Compania"


        Catch ex As Exception

            Throw

        Finally

            command.Dispose()

        End Try

        Return dtsRetorno

    End Function

    ''' <summary>
    ''' TABLA DE SUCURSAL
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ListarSucursal(ByVal eConexion As String) As DataSet

        Dim Dt As New DataTable
        Connection = New OleDbConnection(eConexion)
        Connection.Open()
        Dim dtsRetorno = New DataSet

        Dim Consulta As String = ""
        Dim command = Connection.CreateCommand()
        Consulta = " SELECT CODIGO, DESCRIPN,TELEX FROM LPDBS.UFPSC160 where tabla = 'SUC' AND TIPREG <> '*'"
        command.CommandType = CommandType.Text

        command.CommandText = Consulta

        Try

            Using dr As OleDbDataReader = command.ExecuteReader()

                Dt = Estructura(dr.GetSchemaTable)

                If dr.HasRows Then

                    While dr.Read

                        Dim drItem As DataRow = Dt.NewRow

                        For i As Integer = 0 To dr.FieldCount - 1

                            If dr.GetValue(i).ToString.Trim <> "" Then

                                drItem(i) = dr.GetValue(i).ToString.Trim

                            End If

                        Next

                        Dt.Rows.Add(drItem)

                    End While

                End If

            End Using

            dtsRetorno.Tables.Add(Dt)
            dtsRetorno.Tables(0).TableName = "Sucursal"

        Catch ex As Exception

            Throw

        Finally

            command.Dispose()

        End Try

        Return dtsRetorno

    End Function
    Private Function Estructura(ByVal esquema As DataTable) As DataTable

        Estructura = New DataTable

        For i As Integer = 0 To esquema.Rows.Count - 1

            Dim dr As DataRow = esquema.Rows(i)
            Dim name As String = CType(dr("ColumnName"), String)
            Dim col As DataColumn = New DataColumn(name, CType(dr("DataType"), Type))
            Estructura.Columns.Add(col)

        Next

        Return Estructura

    End Function
End Class
