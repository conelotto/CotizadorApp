﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Este código fue generado por una herramienta.
'     Versión de runtime:4.0.30319.18408
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace wcfEmailCotizador
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute(ConfigurationName:="wcfEmailCotizador.IwcfEmailCotizador")>  _
    Public Interface IwcfEmailCotizador
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IwcfEmailCotizador/EnviarEmail", ReplyAction:="http://tempuri.org/IwcfEmailCotizador/EnviarEmailResponse")>  _
        Function EnviarEmail(ByVal beEmail As Ferreyros.ClasServicioCotizador.beEmail, ByRef ErrorDescripcion As String) As Boolean
    End Interface
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface IwcfEmailCotizadorChannel
        Inherits wcfEmailCotizador.IwcfEmailCotizador, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class IwcfEmailCotizadorClient
        Inherits System.ServiceModel.ClientBase(Of wcfEmailCotizador.IwcfEmailCotizador)
        Implements wcfEmailCotizador.IwcfEmailCotizador
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String)
            MyBase.New(endpointConfigurationName)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As String)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal binding As System.ServiceModel.Channels.Binding, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(binding, remoteAddress)
        End Sub
        
        Public Function EnviarEmail(ByVal beEmail As Ferreyros.ClasServicioCotizador.beEmail, ByRef ErrorDescripcion As String) As Boolean Implements wcfEmailCotizador.IwcfEmailCotizador.EnviarEmail
            Return MyBase.Channel.EnviarEmail(beEmail, ErrorDescripcion)
        End Function
    End Class
End Namespace
