Imports System
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.SessionState.HttpSessionState
Imports System.Web.HttpResponse

Public Class main

    Inherits System.Web.UI.MasterPage

    Sub New()
    End Sub

    Public Sub Msg_Exception(ByVal mensaje As String)
        Session("MsgSource") = "Error en Carga"
        Session("MsgError") = mensaje
        Response.Redirect("~/Error.aspx", False)
    End Sub

    Public Sub Msg_Exception(ByRef Exp As Exception)
        Session("MsgSource") = Exp.Source
        Session("MsgError") = Exp.Message
        Response.Redirect("~/Error.aspx", False)
    End Sub

End Class
