Imports System.Web.Configuration
Public Class clsGeneral
    Public Structure TipoProducto
        'Siempre son los mismo valores en desarrollo, calidad y produccion
        Public Shared PRIME As String = WebConfigurationManager.AppSettings("CodProdPrime")
        Public Shared CSA As String = WebConfigurationManager.AppSettings("CodProdCSA")
        Public Shared ACCESORIO As String = WebConfigurationManager.AppSettings("CodAccesorio")
        Public Shared ALQUILER As String = WebConfigurationManager.AppSettings("CodAlquiler")
    End Structure
    Public Structure TipoClaseCSA
        Public Shared Plan = WebConfigurationManager.AppSettings("CodPlan") ' Desarrollo:CSA0101 , Calidad: CSA_PLN , Produccion: SE_CSA_PLN
        Public Shared Acuerdo = WebConfigurationManager.AppSettings("CodAcuerdo") ' Desarrollo:CSA0102 , Calidad: CSA_ACD , Produccion: SE_CSA_ACD
    End Structure
    Public Structure ClaseCSA
        Public Shared Planes = WebConfigurationManager.AppSettings("CodClasPlanes") '"P"
        Public Shared Acuerdos = WebConfigurationManager.AppSettings("CodClasAcuerdo") '"A"
    End Structure
End Class
