Imports System.Data.OleDb
Imports System
Imports System.Linq
Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet
Imports System.IO
Imports System.Net
Imports System.Net.FtpWebRequest

Public Class CargaRS
    'Public Shared Function GetDataExcel(ByVal rutaArchivo As String, ByVal nombreHoja As String) As DataTable
    '    'Comprobamos que hayan datos
    '    If ((String.IsNullOrEmpty(rutaArchivo)) OrElse (String.IsNullOrEmpty(nombreHoja))) Then Throw New ArgumentNullException()
    '    Try
    '        Dim extension As String = IO.Path.GetExtension(rutaArchivo)
    '        Dim nombre As String = IO.Path.GetFileName(rutaArchivo)
    '        Dim connString As String = "Data Source=" & "C:\Users\bizoutsourcing05\Desktop\mat\" + nombre 'Prueba rutaArchivo
    '        Dim dt1 As New DataTable
    '        'dt1 = ExcelToDataTable("C:\Users\bizoutsourcing05\Desktop\mat\" + nombre)
    '        'If (extension = ".xls") Then
    '        If (extension = ".xls") Or (extension = ".xlsx") Then
    '            connString &= ";Provider=Microsoft.Jet.OLEDB.4.0;" & "Extended Properties='Excel 8.0;HDR=YES;IMEX=1'"
    '            'ElseIf (extension = ".xlsx") Then
    '            '    connString &= ";Provider=Microsoft.ACE.OLEDB.12.0;" & "Persist Security Info=False;Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'"
    '        Else
    '            Throw New ArgumentException("La extensión " & extension & " del archivo no está permitida.")
    '        End If
    '        Using conexion As New OleDbConnection(connString)
    '            Dim sql As String = "SELECT * FROM [" & nombreHoja & "$]"
    '            Dim adaptador As New OleDbDataAdapter(sql, conexion)
    '            Dim dt As New DataTable()
    '            adaptador.Fill(dt)
    '            Return dt
    '        End Using
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function

    Public Shared Function ExcelToDataTable(ByVal mem As Stream) As DataTable
        Try
            Dim dt As New DataTable()
            Using doc As SpreadsheetDocument = SpreadsheetDocument.Open(mem, False)
                Dim workbookPart As WorkbookPart = doc.WorkbookPart
                Dim sheets As IEnumerable(Of Sheet) = doc.WorkbookPart.Workbook.GetFirstChild(Of Sheets)().Elements(Of Sheet)()
                Dim relationshipId As String = sheets.First().Id.Value
                Dim worksheetPart As WorksheetPart = DirectCast(doc.WorkbookPart.GetPartById(relationshipId), WorksheetPart)
                Dim workSheet As Worksheet = worksheetPart.Worksheet
                Dim sheetData As SheetData = workSheet.GetFirstChild(Of SheetData)()
                Dim rows As IEnumerable(Of Row) = sheetData.Descendants(Of Row)()

                For Each cell As Cell In rows.ElementAt(0)
                    'dt.Columns.Add(GetCellValue(doc, cell))
                    dt.Columns.Add(cell.CellReference.Value)
                Next

                For Each row As Row In rows
                    'this will also include your header row...
                    Dim tempRow As DataRow = dt.NewRow()

                    For i As Integer = 0 To row.Descendants(Of Cell)().Count() - 1
                        tempRow(i) = GetCellValue(doc, row.Descendants(Of Cell)().ElementAt(i))
                    Next

                    dt.Rows.Add(tempRow)
                Next
            End Using

            'dt.Rows.RemoveAt(0)

            Return dt

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Public Shared Function ExcelToDataTable(ByVal filename As String) As DataTable
    '    Try
    '        Dim dt As New DataTable()
    '        Using doc As SpreadsheetDocument = SpreadsheetDocument.Open(filename, False)
    '            Dim workbookPart As WorkbookPart = doc.WorkbookPart
    '            Dim sheets As IEnumerable(Of Sheet) = doc.WorkbookPart.Workbook.GetFirstChild(Of Sheets)().Elements(Of Sheet)()
    '            Dim relationshipId As String = sheets.First().Id.Value
    '            Dim worksheetPart As WorksheetPart = DirectCast(doc.WorkbookPart.GetPartById(relationshipId), WorksheetPart)
    '            Dim workSheet As Worksheet = worksheetPart.Worksheet
    '            Dim sheetData As SheetData = workSheet.GetFirstChild(Of SheetData)()
    '            Dim rows As IEnumerable(Of Row) = sheetData.Descendants(Of Row)()

    '            For Each cell As Cell In rows.ElementAt(0)
    '                dt.Columns.Add(GetCellValue(doc, cell))
    '            Next

    '            For Each row As Row In rows
    '                'this will also include your header row...
    '                Dim tempRow As DataRow = dt.NewRow()

    '                For i As Integer = 0 To row.Descendants(Of Cell)().Count() - 1
    '                    tempRow(i) = GetCellValue(doc, row.Descendants(Of Cell)().ElementAt(i))
    '                Next

    '                dt.Rows.Add(tempRow)
    '            Next
    '        End Using

    '        dt.Rows.RemoveAt(0)

    '        Return dt

    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function


    Public Shared Function GetCellValue(ByVal document As SpreadsheetDocument, ByVal cell As Cell) As String
        Try

            If IsNothing(cell.CellValue) Then
                Return ""
            End If

            Dim value As String = cell.CellValue.InnerXml

            If cell.DataType IsNot Nothing AndAlso cell.DataType.Value = CellValues.SharedString Then
                Dim stringTablePart As SharedStringTablePart = document.WorkbookPart.SharedStringTablePart
                Return stringTablePart.SharedStringTable.ChildElements(Int32.Parse(value)).InnerText
            Else
                Return value
            End If

        Catch ex As Exception
            Return ""
        End Try
    End Function

End Class
