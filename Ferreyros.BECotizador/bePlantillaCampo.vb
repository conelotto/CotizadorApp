<Serializable()>
Public Class bePlantillaCampo
    Private pr_intIdPlantillaCampo As String
    Private pr_intIdPlantilla As String
    Private pr_strTabla As String
    Private pr_strCampoTabla As String
    Private pr_strCampoPlantilla As String
    Private pr_strEstado As String
    Private pr_blnEliminado As String
    Private pr_strUsuarioCreacion As String
    Private pr_dtmFechaCreacion As String
    Private pr_strUsuarioModificacion As String
    Private pr_dtmFechaModificacion As String

    Public Property IdPlantillaCampo() As String
        Get
            Return (pr_intIdPlantillaCampo)
        End Get
        Set(ByVal value As String)
            pr_intIdPlantillaCampo = value
        End Set
    End Property

    Public Property IdPlantilla() As String
        Get
            Return (pr_intIdPlantilla)
        End Get
        Set(ByVal value As String)
            pr_intIdPlantilla = value
        End Set
    End Property

    Public Property Tabla() As String
        Get
            Return (pr_strTabla)
        End Get
        Set(ByVal value As String)
            pr_strTabla = value
        End Set
    End Property

    Public Property CampoTabla() As String
        Get
            Return (pr_strCampoTabla)
        End Get
        Set(ByVal value As String)
            pr_strCampoTabla = value
        End Set
    End Property

    Public Property CampoPlantilla() As String
        Get
            Return (pr_strCampoPlantilla)
        End Get
        Set(ByVal value As String)
            pr_strCampoPlantilla = value
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

    Public Property Eliminado() As String
        Get
            Return (pr_blnEliminado)
        End Get
        Set(ByVal value As String)
            pr_blnEliminado = value
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

    Public Sub New()
        pr_intIdPlantillaCampo = -1
        pr_intIdPlantilla = -1
        pr_strTabla = String.Empty
        pr_strCampoTabla = String.Empty
        pr_strCampoPlantilla = String.Empty
        pr_strEstado = String.Empty
        pr_strUsuarioCreacion = String.Empty
        pr_dtmFechaCreacion = Nothing
        pr_strUsuarioModificacion = String.Empty
        pr_dtmFechaModificacion = Nothing
    End Sub
End Class
