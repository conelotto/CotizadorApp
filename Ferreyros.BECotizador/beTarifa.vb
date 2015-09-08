<Serializable()>
Public Class beTarifa
    Private _id As String
    Public Property id() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _prefijo As String
    Public Property prefijo() As String
        Get
            Return _prefijo
        End Get
        Set(ByVal value As String)
            _prefijo = value
        End Set
    End Property

    Private _modelo As String
    Public Property modelo() As String
        Get
            Return _modelo
        End Get
        Set(ByVal value As String)
            _modelo = value
        End Set
    End Property

    Private _modeloBase As String
    Public Property modeloBase() As String
        Get
            Return _modeloBase
        End Get
        Set(ByVal value As String)
            _modeloBase = value
        End Set
    End Property

    Private _familia As String
    Public Property familia() As String
        Get
            Return _familia
        End Get
        Set(ByVal value As String)
            _familia = value
        End Set
    End Property

    Private _codigoPlan As String
    Public Property codigoPlan() As String
        Get
            Return _codigoPlan
        End Get
        Set(ByVal value As String)
            _codigoPlan = value
        End Set
    End Property

    Private _plan As String
    Public Property plan() As String
        Get
            Return _plan
        End Get
        Set(ByVal value As String)
            _plan = value
        End Set
    End Property

    Private _evento As String
    Public Property evento() As String
        Get
            Return _evento
        End Get
        Set(ByVal value As String)
            _evento = value
        End Set
    End Property

    Private _kitRepuestos As String
    Public Property kitRepuestos() As String
        Get
            Return _kitRepuestos
        End Get
        Set(ByVal value As String)
            _kitRepuestos = value
        End Set
    End Property

    Private _fluidos As String
    Public Property fluidos() As String
        Get
            Return _fluidos
        End Get
        Set(ByVal value As String)
            _fluidos = value
        End Set
    End Property

    Private _servicioContratado As String
    Public Property servicioContratado() As String
        Get
            Return _servicioContratado
        End Get
        Set(ByVal value As String)
            _servicioContratado = value
        End Set
    End Property

    Private _SOS As String
    Public Property SOS() As String
        Get
            Return _SOS
        End Get
        Set(ByVal value As String)
            _SOS = value
        End Set
    End Property

    Private _total As String
    Public Property total() As String
        Get
            Return _total
        End Get
        Set(ByVal value As String)
            _total = value
        End Set
    End Property

    Private _eventosNueva As String
    Public Property eventosNueva() As String
        Get
            Return _eventosNueva
        End Get
        Set(ByVal value As String)
            _eventosNueva = value
        End Set
    End Property

    Private _eventosUsada As String
    Public Property eventosUsada() As String
        Get
            Return _eventosUsada
        End Get
        Set(ByVal value As String)
            _eventosUsada = value
        End Set
    End Property

    Private _kitRepuestosT As String
    Public Property kitRepuestosT() As String
        Get
            Return _kitRepuestosT
        End Get
        Set(ByVal value As String)
            _kitRepuestosT = value
        End Set
    End Property

    Private _fluidosT As String
    Public Property fluidosT() As String
        Get
            Return _fluidosT
        End Get
        Set(ByVal value As String)
            _fluidosT = value
        End Set
    End Property

    Private _servicioContratadoT As String
    Public Property servicioContratadoT() As String
        Get
            Return _servicioContratadoT
        End Get
        Set(ByVal value As String)
            _servicioContratadoT = value
        End Set
    End Property

    Private _totalT As String
    Public Property totalT() As String
        Get
            Return _totalT
        End Get
        Set(ByVal value As String)
            _totalT = value
        End Set
    End Property

    Private _tarifaUSDxH As String
    Public Property tarifaUSDxH() As String
        Get
            Return _tarifaUSDxH
        End Get
        Set(ByVal value As String)
            _tarifaUSDxH = value
        End Set
    End Property

    Private _conFluidos As String
    Public Property conFluidos() As String
        Get
            Return _conFluidos
        End Get
        Set(ByVal value As String)
            _conFluidos = value
        End Set
    End Property

    Private _Aceites As String
    Public Property aceites() As String
        Get
            Return _Aceites
        End Get
        Set(ByVal value As String)
            _Aceites = value
        End Set
    End Property

    Private _servicioCategoria As String
    Public Property serviceCategory() As String
        Get
            Return _servicioCategoria
        End Get
        Set(ByVal value As String)
            _servicioCategoria = value
        End Set
    End Property

    Private _rodetail As String
    Public Property rodetail() As String
        Get
            Return _rodetail
        End Get
        Set(ByVal value As String)
            _rodetail = value
        End Set
    End Property

    Private _compQty As String
    Public Property compQty() As String
        Get
            Return _compQty
        End Get
        Set(ByVal value As String)
            _compQty = value
        End Set
    End Property

    Private _firstInterval As String
    Public Property firstInterval() As String
        Get
            Return _firstInterval
        End Get
        Set(ByVal value As String)
            _firstInterval = value
        End Set
    End Property

    Private _nextInterval As String
    Public Property nextInterval() As String
        Get
            Return _nextInterval
        End Get
        Set(ByVal value As String)
            _nextInterval = value
        End Set
    End Property

    Private _jodetail As String
    Public Property jodetail() As String
        Get
            Return _jodetail
        End Get
        Set(ByVal value As String)
            _jodetail = value
        End Set
    End Property

    Private _SOSPartNumber As String
    Public Property SOSPartNumber() As String
        Get
            Return _SOSPartNumber
        End Get
        Set(ByVal value As String)
            _SOSPartNumber = value
        End Set
    End Property

    Private _SOSDescription As String
    Public Property SOSDescription() As String
        Get
            Return _SOSDescription
        End Get
        Set(ByVal value As String)
            _SOSDescription = value
        End Set
    End Property

    Private _quantity As String
    Public Property quantity() As String
        Get
            Return _quantity
        End Get
        Set(ByVal value As String)
            _quantity = value
        End Set
    End Property

    Private _replacement As String
    Public Property replacement() As String
        Get
            Return _replacement
        End Get
        Set(ByVal value As String)
            _replacement = value
        End Set
    End Property

    Private _unitPrice As String
    Public Property unitPrice() As String
        Get
            Return _unitPrice
        End Get
        Set(ByVal value As String)
            _unitPrice = value
        End Set
    End Property

    Private _extendedPrice As String
    Public Property extendedPrice() As String
        Get
            Return _extendedPrice
        End Get
        Set(ByVal value As String)
            _extendedPrice = value
        End Set
    End Property

    Private _sellEvent As String
    Public Property sellEvent() As String
        Get
            Return _sellEvent
        End Get
        Set(ByVal value As String)
            _sellEvent = value
        End Set
    End Property

    Private _sell As String
    Public Property sell() As String
        Get
            Return _sell
        End Get
        Set(ByVal value As String)
            _sell = value
        End Set
    End Property

    Private _llave As String
    Public Property llave() As String
        Get
            Return _llave
        End Get
        Set(ByVal value As String)
            _llave = value
        End Set
    End Property
 

    Public Sub New()

        _llave = String.Empty
        _servicioCategoria = String.Empty
        _rodetail = String.Empty
        _compQty = String.Empty
        _firstInterval = String.Empty
        _nextInterval = String.Empty
        _jodetail = String.Empty
        _SOSPartNumber = String.Empty
        _SOSDescription = String.Empty
        _quantity = String.Empty
        _replacement = String.Empty
        _unitPrice = String.Empty
        _extendedPrice = String.Empty
        _sellEvent = String.Empty
        _sell = String.Empty
        _conFluidos = String.Empty
        _id = String.Empty
        _prefijo = String.Empty
        _modelo = String.Empty
        _modeloBase = String.Empty
        _familia = String.Empty
        _plan = String.Empty
        _evento = String.Empty
        _kitRepuestos = String.Empty
        _fluidos = String.Empty
        _servicioContratado = String.Empty
        _SOS = String.Empty
        _total = String.Empty
        _eventosNueva = String.Empty
        _eventosUsada = String.Empty
        _kitRepuestosT = String.Empty
        _fluidosT = String.Empty
        servicioContratadoT = String.Empty
        _totalT = String.Empty
        _tarifaUSDxH = String.Empty
        _codigoPlan = String.Empty
        _ConFluidos = String.Empty
    End Sub
End Class
