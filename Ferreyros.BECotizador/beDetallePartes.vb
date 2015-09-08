<Serializable()>
Public Class beDetallePartes
    Private _Ide As Integer
    Private _Prefijo As String
    Private _Modelo As String
    Private _ModeloBase As String
    Private _Familia As String
    Private _ServiceCategory As String
    Private _Rodetail As String
    Private _CompQty As Decimal
    Private _FirstInterval As Decimal
    Private _NextInterval As Decimal
    Private _JODETAIL As String
    Private _SOSPartNumber As String
    Private _SOSDescription As String
    Private _Quantity As Decimal
    Private _Replacement As Decimal
    Private _UnitPrice As Decimal
    Private _ExtendedPrice As Decimal
    Private _SellEvent As Decimal
    Private _Eventos As Decimal
    Private _Sell As Decimal
    Private _Estado As Integer
    Private _FleetOrigin As String
    Private _IncluyeFluidos As String
    Private _CodPlan As String

    Public Property Ide As Integer
        Get
            Return _Ide
        End Get
        Set(value As Integer)
            _Ide = value
        End Set
    End Property
    Public Property Prefijo As String
        Get
            Return _Prefijo
        End Get
        Set(value As String)
            _Prefijo = value
        End Set
    End Property
    Public Property Modelo As String
        Get
            Return _Modelo
        End Get
        Set(value As String)
            _Modelo = value
        End Set
    End Property
    Public Property ModeloBase As String
        Get
            Return _ModeloBase
        End Get
        Set(value As String)
            _ModeloBase = value
        End Set
    End Property
    Public Property Familia As String
        Get
            Return _Familia
        End Get
        Set(value As String)
            _Familia = value
        End Set
    End Property
    Public Property ServiceCategory As String
        Get
            Return _ServiceCategory
        End Get
        Set(value As String)
            _ServiceCategory = value
        End Set
    End Property
    Public Property Rodetail As String
        Get
            Return _Rodetail
        End Get
        Set(value As String)
            _Rodetail = value
        End Set
    End Property
    Public Property CompQty As Decimal
        Get
            Return _CompQty
        End Get
        Set(value As Decimal)
            _CompQty = value
        End Set
    End Property
    Public Property FirstInterval As Decimal
        Get
            Return _FirstInterval
        End Get
        Set(value As Decimal)
            _FirstInterval = value
        End Set
    End Property
    Public Property NextInterval As Decimal
        Get
            Return _NextInterval
        End Get
        Set(value As Decimal)
            _NextInterval = value
        End Set
    End Property
    Public Property JODETAIL As String
        Get
            Return _JODETAIL
        End Get
        Set(value As String)
            _JODETAIL = value
        End Set
    End Property
    Public Property SOSPartNumber As String
        Get
            Return _SOSPartNumber
        End Get
        Set(value As String)
            _SOSPartNumber = value
        End Set
    End Property
    Public Property SOSDescription As String
        Get
            Return _SOSDescription
        End Get
        Set(value As String)
            _SOSDescription = value
        End Set
    End Property
    Public Property Quantity As Decimal
        Get
            Return _Quantity
        End Get
        Set(value As Decimal)
            _Quantity = value
        End Set
    End Property
    Public Property Replacement As Decimal
        Get
            Return _Replacement
        End Get
        Set(value As Decimal)
            _Replacement = value
        End Set
    End Property
    Public Property UnitPrice As Decimal
        Get
            Return _UnitPrice
        End Get
        Set(value As Decimal)
            _UnitPrice = value
        End Set
    End Property
    Public Property ExtendedPrice As Decimal
        Get
            Return _ExtendedPrice
        End Get
        Set(value As Decimal)
            _ExtendedPrice = value
        End Set
    End Property
    Public Property SellEvent As Decimal
        Get
            Return _SellEvent
        End Get
        Set(value As Decimal)
            _SellEvent = value
        End Set
    End Property
    Public Property Eventos As Decimal
        Get
            Return _Eventos
        End Get
        Set(value As Decimal)
            _Eventos = value
        End Set
    End Property
    Public Property Sell As Decimal
        Get
            Return _Sell
        End Get
        Set(value As Decimal)
            _Sell = value
        End Set
    End Property
    Public Property Estado As Integer
        Get
            Return _Estado
        End Get
        Set(value As Integer)
            _Estado = value
        End Set
    End Property
    Public Property FleetOrigin As String
        Get
            Return _FleetOrigin
        End Get
        Set(value As String)
            _FleetOrigin = value
        End Set
    End Property

    Public Property IncluyeFluidos As String
        Get
            Return _IncluyeFluidos
        End Get
        Set(value As String)
            _IncluyeFluidos = value
        End Set
    End Property
    Public Property CodPlan As String
        Get
            Return _CodPlan
        End Get
        Set(value As String)
            _CodPlan = value
        End Set
    End Property


End Class
