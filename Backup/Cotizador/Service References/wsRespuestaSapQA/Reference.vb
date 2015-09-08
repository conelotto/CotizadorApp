﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Este código fue generado por una herramienta.
'     Versión de runtime:4.0.30319.1008
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace wsRespuestaSapQA
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute([Namespace]:="urn:sap-com:document:sap:rfc:functions", ConfigurationName:="wsRespuestaSapQA.ZWS_IN_COT_CSA_B")>  _
    Public Interface ZWS_IN_COT_CSA_B
        
        'CODEGEN: Se está generando un contrato de mensaje, ya que la operación ZWS_RECIBE_COTIZACION_CSA no es RPC ni está encapsulada en un documento.
        <System.ServiceModel.OperationContractAttribute(Action:="", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function ZWS_RECIBE_COTIZACION_CSA(ByVal request As wsRespuestaSapQA.ZWS_RECIBE_COTIZACION_CSARequest) As wsRespuestaSapQA.ZWS_RECIBE_COTIZACION_CSAResponse1
    End Interface
    
    '''<comentarios/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1009"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="urn:sap-com:document:sap:rfc:functions")>  _
    Partial Public Class ZWS_RECIBE_COTIZACION_CSA
        Inherits Object
        Implements System.ComponentModel.INotifyPropertyChanged
        
        Private iD_COTIZACIONField As String
        
        Private pRODUCTOField() As ZPRODWSCSA
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=0)>  _
        Public Property ID_COTIZACION() As String
            Get
                Return Me.iD_COTIZACIONField
            End Get
            Set
                Me.iD_COTIZACIONField = value
                Me.RaisePropertyChanged("ID_COTIZACION")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlArrayAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=1),  _
         System.Xml.Serialization.XmlArrayItemAttribute("item", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=false)>  _
        Public Property PRODUCTO() As ZPRODWSCSA()
            Get
                Return Me.pRODUCTOField
            End Get
            Set
                Me.pRODUCTOField = value
                Me.RaisePropertyChanged("PRODUCTO")
            End Set
        End Property
        
        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        
        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class
    
    '''<comentarios/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1009"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:sap-com:document:sap:rfc:functions")>  _
    Partial Public Class ZPRODWSCSA
        Inherits Object
        Implements System.ComponentModel.INotifyPropertyChanged
        
        Private iTEMField As String
        
        Private fLUIDOSField As String
        
        Private dETALLE_PARTESField As String
        
        Private fECHA_IField As String
        
        Private mONTOField As String
        
        Private mAQUINAField() As ZMAQWSCSA
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=0)>  _
        Public Property ITEM() As String
            Get
                Return Me.iTEMField
            End Get
            Set
                Me.iTEMField = value
                Me.RaisePropertyChanged("ITEM")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=1)>  _
        Public Property FLUIDOS() As String
            Get
                Return Me.fLUIDOSField
            End Get
            Set
                Me.fLUIDOSField = value
                Me.RaisePropertyChanged("FLUIDOS")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=2)>  _
        Public Property DETALLE_PARTES() As String
            Get
                Return Me.dETALLE_PARTESField
            End Get
            Set
                Me.dETALLE_PARTESField = value
                Me.RaisePropertyChanged("DETALLE_PARTES")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=3)>  _
        Public Property FECHA_I() As String
            Get
                Return Me.fECHA_IField
            End Get
            Set
                Me.fECHA_IField = value
                Me.RaisePropertyChanged("FECHA_I")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=4)>  _
        Public Property MONTO() As String
            Get
                Return Me.mONTOField
            End Get
            Set
                Me.mONTOField = value
                Me.RaisePropertyChanged("MONTO")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlArrayAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=5),  _
         System.Xml.Serialization.XmlArrayItemAttribute("item", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=false)>  _
        Public Property MAQUINA() As ZMAQWSCSA()
            Get
                Return Me.mAQUINAField
            End Get
            Set
                Me.mAQUINAField = value
                Me.RaisePropertyChanged("MAQUINA")
            End Set
        End Property
        
        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        
        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class
    
    '''<comentarios/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1009"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:sap-com:document:sap:rfc:functions")>  _
    Partial Public Class ZMAQWSCSA
        Inherits Object
        Implements System.ComponentModel.INotifyPropertyChanged
        
        Private fAMILIAField As String
        
        Private mODELO_BASEField As String
        
        Private mODELOField As String
        
        Private pREFIJOField As String
        
        Private mAQUINA_NUEVAField As String
        
        Private nUMERO_SERIEField As String
        
        Private hOROMETRO_IField As String
        
        Private fECHA_HOROMETROField As String
        
        Private hORAS_PROMEDIOField As String
        
        Private hOROMETRO_FField As String
        
        Private rENOVACIONField As String
        
        Private rENOVACION_VALField As String
        
        Private cOD_DPTOField As String
        
        Private dEPARTAMENTOField As String
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=0)>  _
        Public Property FAMILIA() As String
            Get
                Return Me.fAMILIAField
            End Get
            Set
                Me.fAMILIAField = value
                Me.RaisePropertyChanged("FAMILIA")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=1)>  _
        Public Property MODELO_BASE() As String
            Get
                Return Me.mODELO_BASEField
            End Get
            Set
                Me.mODELO_BASEField = value
                Me.RaisePropertyChanged("MODELO_BASE")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=2)>  _
        Public Property MODELO() As String
            Get
                Return Me.mODELOField
            End Get
            Set
                Me.mODELOField = value
                Me.RaisePropertyChanged("MODELO")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=3)>  _
        Public Property PREFIJO() As String
            Get
                Return Me.pREFIJOField
            End Get
            Set
                Me.pREFIJOField = value
                Me.RaisePropertyChanged("PREFIJO")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=4)>  _
        Public Property MAQUINA_NUEVA() As String
            Get
                Return Me.mAQUINA_NUEVAField
            End Get
            Set
                Me.mAQUINA_NUEVAField = value
                Me.RaisePropertyChanged("MAQUINA_NUEVA")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=5)>  _
        Public Property NUMERO_SERIE() As String
            Get
                Return Me.nUMERO_SERIEField
            End Get
            Set
                Me.nUMERO_SERIEField = value
                Me.RaisePropertyChanged("NUMERO_SERIE")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=6)>  _
        Public Property HOROMETRO_I() As String
            Get
                Return Me.hOROMETRO_IField
            End Get
            Set
                Me.hOROMETRO_IField = value
                Me.RaisePropertyChanged("HOROMETRO_I")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=7)>  _
        Public Property FECHA_HOROMETRO() As String
            Get
                Return Me.fECHA_HOROMETROField
            End Get
            Set
                Me.fECHA_HOROMETROField = value
                Me.RaisePropertyChanged("FECHA_HOROMETRO")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=8)>  _
        Public Property HORAS_PROMEDIO() As String
            Get
                Return Me.hORAS_PROMEDIOField
            End Get
            Set
                Me.hORAS_PROMEDIOField = value
                Me.RaisePropertyChanged("HORAS_PROMEDIO")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=9)>  _
        Public Property HOROMETRO_F() As String
            Get
                Return Me.hOROMETRO_FField
            End Get
            Set
                Me.hOROMETRO_FField = value
                Me.RaisePropertyChanged("HOROMETRO_F")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=10)>  _
        Public Property RENOVACION() As String
            Get
                Return Me.rENOVACIONField
            End Get
            Set
                Me.rENOVACIONField = value
                Me.RaisePropertyChanged("RENOVACION")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=11)>  _
        Public Property RENOVACION_VAL() As String
            Get
                Return Me.rENOVACION_VALField
            End Get
            Set
                Me.rENOVACION_VALField = value
                Me.RaisePropertyChanged("RENOVACION_VAL")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=12)>  _
        Public Property COD_DPTO() As String
            Get
                Return Me.cOD_DPTOField
            End Get
            Set
                Me.cOD_DPTOField = value
                Me.RaisePropertyChanged("COD_DPTO")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=13)>  _
        Public Property DEPARTAMENTO() As String
            Get
                Return Me.dEPARTAMENTOField
            End Get
            Set
                Me.dEPARTAMENTOField = value
                Me.RaisePropertyChanged("DEPARTAMENTO")
            End Set
        End Property
        
        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        
        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class
    
    '''<comentarios/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1009"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="urn:sap-com:document:sap:rfc:functions")>  _
    Partial Public Class ZWS_RECIBE_COTIZACION_CSAResponse
        Inherits Object
        Implements System.ComponentModel.INotifyPropertyChanged
        
        Private e_RESULTADOField As String
        
        Private mENSAJE_RESULTADOField As String
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=0)>  _
        Public Property E_RESULTADO() As String
            Get
                Return Me.e_RESULTADOField
            End Get
            Set
                Me.e_RESULTADOField = value
                Me.RaisePropertyChanged("E_RESULTADO")
            End Set
        End Property
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=1)>  _
        Public Property MENSAJE_RESULTADO() As String
            Get
                Return Me.mENSAJE_RESULTADOField
            End Get
            Set
                Me.mENSAJE_RESULTADOField = value
                Me.RaisePropertyChanged("MENSAJE_RESULTADO")
            End Set
        End Property
        
        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        
        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced),  _
     System.ServiceModel.MessageContractAttribute(IsWrapped:=false)>  _
    Partial Public Class ZWS_RECIBE_COTIZACION_CSARequest
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:sap-com:document:sap:rfc:functions", Order:=0)>  _
        Public ZWS_RECIBE_COTIZACION_CSA As wsRespuestaSapQA.ZWS_RECIBE_COTIZACION_CSA
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal ZWS_RECIBE_COTIZACION_CSA As wsRespuestaSapQA.ZWS_RECIBE_COTIZACION_CSA)
            MyBase.New
            Me.ZWS_RECIBE_COTIZACION_CSA = ZWS_RECIBE_COTIZACION_CSA
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced),  _
     System.ServiceModel.MessageContractAttribute(IsWrapped:=false)>  _
    Partial Public Class ZWS_RECIBE_COTIZACION_CSAResponse1
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:sap-com:document:sap:rfc:functions", Order:=0)>  _
        Public ZWS_RECIBE_COTIZACION_CSAResponse As wsRespuestaSapQA.ZWS_RECIBE_COTIZACION_CSAResponse
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal ZWS_RECIBE_COTIZACION_CSAResponse As wsRespuestaSapQA.ZWS_RECIBE_COTIZACION_CSAResponse)
            MyBase.New
            Me.ZWS_RECIBE_COTIZACION_CSAResponse = ZWS_RECIBE_COTIZACION_CSAResponse
        End Sub
    End Class
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface ZWS_IN_COT_CSA_BChannel
        Inherits wsRespuestaSapQA.ZWS_IN_COT_CSA_B, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class ZWS_IN_COT_CSA_BClient
        Inherits System.ServiceModel.ClientBase(Of wsRespuestaSapQA.ZWS_IN_COT_CSA_B)
        Implements wsRespuestaSapQA.ZWS_IN_COT_CSA_B
        
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
        
        <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Function wsRespuestaSapQA_ZWS_IN_COT_CSA_B_ZWS_RECIBE_COTIZACION_CSA(ByVal request As wsRespuestaSapQA.ZWS_RECIBE_COTIZACION_CSARequest) As wsRespuestaSapQA.ZWS_RECIBE_COTIZACION_CSAResponse1 Implements wsRespuestaSapQA.ZWS_IN_COT_CSA_B.ZWS_RECIBE_COTIZACION_CSA
            Return MyBase.Channel.ZWS_RECIBE_COTIZACION_CSA(request)
        End Function
        
        Public Function ZWS_RECIBE_COTIZACION_CSA(ByVal ZWS_RECIBE_COTIZACION_CSA1 As wsRespuestaSapQA.ZWS_RECIBE_COTIZACION_CSA) As wsRespuestaSapQA.ZWS_RECIBE_COTIZACION_CSAResponse
            Dim inValue As wsRespuestaSapQA.ZWS_RECIBE_COTIZACION_CSARequest = New wsRespuestaSapQA.ZWS_RECIBE_COTIZACION_CSARequest()
            inValue.ZWS_RECIBE_COTIZACION_CSA = ZWS_RECIBE_COTIZACION_CSA1
            Dim retVal As wsRespuestaSapQA.ZWS_RECIBE_COTIZACION_CSAResponse1 = CType(Me,wsRespuestaSapQA.ZWS_IN_COT_CSA_B).ZWS_RECIBE_COTIZACION_CSA(inValue)
            Return retVal.ZWS_RECIBE_COTIZACION_CSAResponse
        End Function
    End Class
End Namespace
