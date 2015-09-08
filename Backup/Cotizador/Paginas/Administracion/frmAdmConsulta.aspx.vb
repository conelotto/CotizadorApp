Imports Ferreyros
Imports System.Data
Imports System.Data.SqlClient
Public Class frmAdmConsulta
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnConsulta_Click(sender As Object, e As EventArgs) Handles btnConsulta.Click
        Dim eConexion As String = Modulo.strConexionSql
        Dim Connection = New SqlConnection(eConexion)


        Dim Command As SqlCommand = Nothing
        Dim _Resultado As String = String.Empty
        Try
            Connection.Open()
            Command = Connection.CreateCommand
            Command.CommandType = CommandType.Text
            Command.CommandText = Me.txtConsulta.Text.Trim()
            Command.ExecuteNonQuery() 
            ScriptManager.RegisterStartupScript(Page, Page.GetType, "alert", "alert('" + "Comprobación correcta..." + "')", True)
        Catch ex As Exception 
            ScriptManager.RegisterStartupScript(Page, Page.GetType, "alert", "alert('" + "No se pudo Comprobar..." + "')", True)
        End Try
         
    End Sub
End Class