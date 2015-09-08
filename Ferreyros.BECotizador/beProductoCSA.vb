<Serializable()>
Public Class beProductoCSA

    Public Property ClaseCsa() As String
    Public Property IdTipoCSA() As String
    Public Property IdPlan() As String
    Public Property DescripcionPlan() As String
    Public Property IdUnidadDuracion() As String
    Public Property Duracion() As String
    Public Property Tiempo() As String
    Public Property IdUnidadPlazoRenovacion() As String
    Public Property PlazoRenovacion() As String
    Public Property IncluyeFluidos() As String
    Public Property IncluyeDetallePartes() As String
    Public Property FechaInicioContrato() As String
    Public Property FechaEstimadaCierre() As String
    Public Property ParticipacionVendedor1() As String
    Public Property ParticipacionVendedor2() As String
    Public Property TipoCotizacion() As String
    Public Property ListaMaquinaria() As List(Of beMaquinaria)

    Public Sub New()

        TipoCotizacion = String.Empty
        ParticipacionVendedor2 = String.Empty
        ParticipacionVendedor1 = String.Empty
        IdUnidadDuracion = String.Empty
        IncluyeFluidos = String.Empty
        FechaInicioContrato = String.Empty
        IdUnidadPlazoRenovacion = String.Empty
        FechaEstimadaCierre = String.Empty
        ClaseCsa = String.Empty
        IdPlan = String.Empty
        IncluyeDetallePartes = String.Empty
        PlazoRenovacion = String.Empty
        Tiempo = String.Empty
        Duracion = String.Empty
        IdTipoCSA = String.Empty
        DescripcionPlan = String.Empty
        ListaMaquinaria = New List(Of beMaquinaria)

    End Sub

End Class
