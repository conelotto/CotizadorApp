Imports Ferreyros.BECotizador
Imports Ferreyros.BCCotizador

Public Class frmReporte
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If sender Is Nothing OrElse e Is Nothing Then
            Return
        End If

        Dim nombre As String = Session("Reporte_Nombre")
        Dim data As String = Session("Reporte_Data")

        ExportarExcel(nombre, data)

    End Sub


    Private Sub ExportarExcel(ByVal nombre As String, ByVal data As String)

        Response.ClearHeaders()
        Response.Buffer = True
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("iso-8859-1")
        Response.Clear()
        Response.AppendHeader("content-disposition", String.Concat("attachment;filename=", nombre, ".", "xls"))
        Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache)
        Response.Write(data)
        Response.End()

    End Sub

End Class