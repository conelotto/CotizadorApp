Imports System.Web.SessionState
Imports System.Security.Principal

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
        log4net.Config.XmlConfigurator.Configure()
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request

        ''==Validar Autentificacion de subir archivos ==================================================
        Dim TIPOACCION = "TIPOACCION"
        If Not HttpContext.Current.Request.QueryString(TIPOACCION) Is Nothing Then

            If HttpContext.Current.Request.QueryString(TIPOACCION) = "SUBIR_ARCHIVO" Then
                Try
                    Dim session_param_name = "ASPSESSID"
                    Dim session_cookie_name = "ASP.NET_SessionId"

                    If Not HttpContext.Current.Request.Form(session_param_name) Is Nothing Then
                        UpdateCookie(session_cookie_name, HttpContext.Current.Request.Form(session_param_name))
                    ElseIf Not HttpContext.Current.Request.QueryString(session_param_name) Is Nothing Then
                        UpdateCookie(session_cookie_name, HttpContext.Current.Request.QueryString(session_param_name))
                    End If
                Catch ex As Exception
                End Try


                Try
                    Dim auth_param_name = "AUTHID"
                    Dim auth_cookie_name = FormsAuthentication.FormsCookieName

                    If Not HttpContext.Current.Request.Form(auth_param_name) Is Nothing Then
                        UpdateCookie(auth_cookie_name, HttpContext.Current.Request.Form(auth_param_name))
                    ElseIf Not HttpContext.Current.Request.QueryString(auth_param_name) Is Nothing Then
                        Dim UsuarioAutenticado As String = ""
                        If Not String.IsNullOrEmpty(HttpContext.Current.Request.QueryString(auth_param_name)) Then
                            UsuarioAutenticado = HttpContext.Current.Request.QueryString(auth_param_name)
                            UpdateCookie(auth_cookie_name, UsuarioAutenticado)
                        Else
                            'UsuarioAutenticado = WindowsIdentity.GetCurrent.Name ' obtiene: DOMFERREYROS\backoutsourcing
                            'UsuarioAutenticado = Environment.UserName ' obtiene: backoutsourcing
                            If Not WindowsIdentity.GetCurrent Is Nothing Then
                                UsuarioAutenticado = WindowsIdentity.GetCurrent.Name
                                UpdateCookie(auth_cookie_name, UsuarioAutenticado)
                            End If
                        End If
                    End If
                Catch ex As Exception
                End Try

            End If

        End If

        '======================================================================
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub
    Private Sub UpdateCookie(ByVal cookie_name As String, ByVal cookie_value As String)
        'Dim cookie = HttpContext.Current.Request.Cookies.Get(cookie_name)
        'If cookie Is Nothing Then

        '    cookie = New HttpCookie(cookie_name)
        'End If
        'cookie.Value = cookie_value
        'HttpContext.Current.Request.Cookies.Set(cookie)

        '---------------------------------
        Dim cookie = HttpContext.Current.Request.Cookies.Get(cookie_name)
        Try

            If cookie Is Nothing Then

                Dim UsuarioAutenticado = WindowsIdentity.GetCurrent.Name
                Dim ticket As New FormsAuthenticationTicket(1, UsuarioAutenticado.ToString(), DateTime.Now, DateTime.Now.AddMinutes(60), False, UsuarioAutenticado.ToString(), FormsAuthentication.FormsCookiePath)

                Dim hashCookies As String = FormsAuthentication.Encrypt(ticket)
                cookie = New HttpCookie(cookie_name, hashCookies)
                cookie.HttpOnly = True
                cookie.Value = cookie_value
                'HttpContext.Current.Response.Cookies.Add(cookie)
                HttpContext.Current.Request.Cookies.Add(cookie)
            End If
        Catch ex As Exception
        End Try

        'cookie.Value = cookie_value
        'HttpContext.Current.Request.Cookies.Set(cookie)
             
        

        '--------------------------------------
    End Sub
End Class