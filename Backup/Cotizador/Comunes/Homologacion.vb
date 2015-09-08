Public Class Homologacion

    Public Shared Function ObtenerUnidadDuracion(ByVal IdUnidad As String) As String
        Dim strRetur As String = String.Empty
        Select Case IdUnidad
            Case "H"
                strRetur = "Hora(s)"
                Exit Select
            Case "M"
                strRetur = "Mes(es)"
                Exit Select
            Case Else
                strRetur = ""
        End Select
        Return strRetur
    End Function
    Public Structure ClaseCSA
        Public Shared Planes = "P" 'WebConfigurationManager.AppSettings("CodClasPlanes") '"P"
        Public Shared Acuerdos = "A" 'WebConfigurationManager.AppSettings("CodClasAcuerdo") '"A"
    End Structure
End Class
