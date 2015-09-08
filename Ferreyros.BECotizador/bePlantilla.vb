<Serializable()>
Public Class bePlantilla
    Private pr_intIdPlantilla As String
    Private pr_strCodigoModelo As String
    Private pr_strVersionModelo As String
    Private pr_strNombre As String
    Private pr_bytArchivo As Byte()
    Private pr_strEstado As String
    Private pr_strUsuarioCreacion As String
    Private pr_dtmFechaCreacion As String
    Private pr_strUsuarioModificacion As String
    Private pr_dtmFechaModificacion As String
    Private loPlantillaCampos As List(Of bePlantillaCampo)

    Public Property IdPlantilla() As String
        Get
            Return (pr_intIdPlantilla)
        End Get
        Set(ByVal value As String)
            pr_intIdPlantilla = value
        End Set
    End Property

    Public Property CodigoModelo() As String
        Get
            Return (pr_strCodigoModelo)
        End Get
        Set(ByVal value As String)
            pr_strCodigoModelo = value
        End Set
    End Property

    Public Property VersionModelo() As String
        Get
            Return (pr_strVersionModelo)
        End Get
        Set(ByVal value As String)
            pr_strVersionModelo = value
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

    Public Property Archivo() As Byte()
        Get
            Return (pr_bytArchivo)
        End Get
        Set(ByVal value As Byte())
            pr_bytArchivo = value
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

    Public Property PlantillaCampos() As List(Of bePlantillaCampo)
        Get
            Return (loPlantillaCampos)
        End Get
        Set(ByVal value As List(Of bePlantillaCampo))
            loPlantillaCampos = value
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

    Private _Items As String
    Public Property Items() As String
        Get
            Return _Items
        End Get
        Set(ByVal value As String)
            _Items = value
        End Set
    End Property

    Private _ItemsEmpleados As String
    Public Property ItemsEmpleados() As String
        Get
            Return _ItemsEmpleados
        End Get
        Set(ByVal value As String)
            _ItemsEmpleados = value
        End Set
    End Property


    Public Sub New()

        _Items = String.Empty
        _ItemsEmpleados = String.Empty
        pr_intIdPlantilla = -1
        pr_strCodigoModelo = String.Empty
        pr_strVersionModelo = String.Empty
        pr_strNombre = String.Empty
        pr_bytArchivo = Nothing
        pr_strEstado = String.Empty
        pr_strUsuarioCreacion = String.Empty
        pr_dtmFechaCreacion = Nothing
        pr_strUsuarioModificacion = String.Empty
        pr_dtmFechaModificacion = Nothing
        loPlantillaCampos = New List(Of bePlantillaCampo)
    End Sub
End Class
