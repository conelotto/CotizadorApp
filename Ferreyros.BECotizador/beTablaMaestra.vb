<Serializable()>
Public Class beTablaMaestra

    Private pr_intIdTablaMaestra As String
    Private pr_strGrupo As String
    Private pr_strIdSeccion As String
    Private pr_strNombre As String
    Private pr_strDescripcion As String
    Private pr_strEstado As String
    Private pr_strUsuarioCreacion As String
    Private pr_dtmFechaCreacion As String
    Private pr_strUsuarioModificacion As String
    Private pr_dtmFechaModificacion As String
    Private pr_strPosicionInicial As String
    Private pr_strOpcional As String
    Private pr_strCambioPosicion As String
    Private pr_strIdSubSeccion As String
    Private pr_strPrioridad As String
    Private pr_strCodigo As String
    Private pr_strTipo As String
    Private pr_strIdSeccionCriterio As String
    Private pr_strIdCriterio As String
    Private pr_strIdSubSeccionCriterio As String
    Private pr_strImprimir As String
    Private pr_codigoSeccion As String
    Public Property IdTablaMaestra() As String
        Get
            Return (pr_intIdTablaMaestra)
        End Get
        Set(ByVal value As String)
            pr_intIdTablaMaestra = value
        End Set
    End Property

    Public Property Grupo() As String
        Get
            Return (pr_strGrupo)
        End Get
        Set(ByVal value As String)
            pr_strGrupo = value
        End Set
    End Property

    Public Property IdSeccion() As String
        Get
            Return (pr_strIdSeccion)
        End Get
        Set(ByVal value As String)
            pr_strIdSeccion = value
        End Set
    End Property

    Public Property IdSubSeccion() As String
        Get
            Return (pr_strIdSubSeccion)
        End Get
        Set(ByVal value As String)
            pr_strIdSubSeccion = value
        End Set
    End Property

    Public Property Nombre() As String
        Get
            Return (pr_strNombre)
        End Get
        Set(ByVal value As String)
            pr_strNombre = value
        End Set
    End Property

    Public Property Descripcion() As String
        Get
            Return (pr_strDescripcion)
        End Get
        Set(ByVal value As String)
            pr_strDescripcion = value
        End Set
    End Property

    Public Property Estado() As String
        Get
            Return (pr_strEstado)
        End Get
        Set(ByVal value As String)
            pr_strEstado = value
        End Set
    End Property

    Public Property UsuarioCreacion() As String
        Get
            Return (pr_strUsuarioCreacion)
        End Get
        Set(ByVal value As String)
            pr_strUsuarioCreacion = value
        End Set
    End Property

    Public Property FechaCreacion() As String
        Get
            Return (pr_dtmFechaCreacion)
        End Get
        Set(ByVal value As String)
            pr_dtmFechaCreacion = value
        End Set
    End Property

    Public Property UsuarioModificacion() As String
        Get
            Return (pr_strUsuarioModificacion)
        End Get
        Set(ByVal value As String)
            pr_strUsuarioModificacion = value
        End Set
    End Property

    Public Property FechaModificacion() As String
        Get
            Return (pr_dtmFechaModificacion)
        End Get
        Set(ByVal value As String)
            pr_dtmFechaModificacion = value
        End Set
    End Property

    Public Property PosicionInicial() As String
        Get
            Return pr_strPosicionInicial
        End Get
        Set(ByVal value As String)
            pr_strPosicionInicial = value
        End Set
    End Property

    Public Property Opcional() As String
        Get
            Return pr_strOpcional
        End Get
        Set(ByVal value As String)
            pr_strOpcional = value
        End Set
    End Property

    Public Property CambioPosicion() As String
        Get
            Return pr_strCambioPosicion
        End Get
        Set(ByVal value As String)
            pr_strCambioPosicion = value
        End Set
    End Property

    Public Property Prioridad() As String
        Get
            Return pr_strPrioridad
        End Get
        Set(ByVal value As String)
            pr_strPrioridad = value
        End Set
    End Property

    Public Property Codigo() As String
        Get
            Return pr_strCodigo
        End Get
        Set(ByVal value As String)
            pr_strCodigo = value
        End Set
    End Property

    Public Property Tipo() As String
        Get
            Return pr_strTipo
        End Get
        Set(ByVal value As String)
            pr_strTipo = value
        End Set
    End Property
    Public Property IdSeccionCriterio() As String
        Get
            Return pr_strIdSeccionCriterio
        End Get
        Set(ByVal value As String)
            pr_strIdSeccionCriterio = value
        End Set
    End Property

    Public Property IdCriterio() As String
        Get
            Return pr_strIdCriterio
        End Get
        Set(ByVal value As String)
            pr_strIdCriterio = value
        End Set
    End Property

    Public Property IdSubSeccionCriterio()
        Get
            Return pr_strIdSubSeccionCriterio
        End Get
        Set(ByVal value)
            pr_strIdSubSeccionCriterio = value
        End Set
    End Property


    Public Property Imprimir() As String
        Get
            Return pr_strImprimir
        End Get
        Set(ByVal value As String)
            pr_strImprimir = value
        End Set
    End Property

    Public Property CodigoSeccion() As String
        Get
            Return pr_codigoSeccion
        End Get
        Set(value As String)
            pr_codigoSeccion = value
        End Set
    End Property

    Public Sub New()

        pr_strImprimir = String.Empty
        pr_strTipo = String.Empty
        pr_strCodigo = String.Empty
        pr_strPrioridad = String.Empty
        pr_strCambioPosicion = String.Empty
        pr_strOpcional = String.Empty
        pr_intIdTablaMaestra = -1
        pr_strGrupo = String.Empty
        pr_strIdSeccion = String.Empty
        pr_strNombre = String.Empty
        pr_strDescripcion = String.Empty
        pr_strEstado = String.Empty
        pr_strUsuarioCreacion = String.Empty
        pr_dtmFechaCreacion = Nothing
        pr_strUsuarioModificacion = String.Empty
        pr_dtmFechaModificacion = Nothing
        pr_strPosicionInicial = String.Empty
        pr_strIdSeccionCriterio = String.Empty
        pr_strIdCriterio = String.Empty
        pr_strIdSubSeccionCriterio = String.Empty
        pr_codigoSeccion = String.Empty
    End Sub

    Public Sub New(ByVal prioridad As String, ByVal codigoSeccion As String, ByVal Nombre As String)
        pr_strPrioridad = prioridad
        pr_strCodigo = codigoSeccion
        pr_strNombre = Nombre
    End Sub
End Class
